using ExpensePrediction.BusinessLogicLayer.Interfaces.Services;
using ExpensePrediction.BusinessLogicLayer.Regression;
using ExpensePrediction.BusinessLogicLayer.Regression.Model;
using ExpensePrediction.DataAccessLayer.Entities;
using ExpensePrediction.DataAccessLayer.Interfaces;
using ExpensePrediction.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpensePrediction.BusinessLogicLayer.Services
{
    public class PredictionService : IPredictionService
    {
        private readonly IApplicationRepository<Expense> _expenseRepository;
        private readonly IApplicationRepository<Income> _incomeRepository;

        public PredictionService(IApplicationRepository<Expense> expenseRepository, IApplicationRepository<Income> incomeRepository)
        {
            _expenseRepository = expenseRepository;
            _incomeRepository = incomeRepository;
        }
        public async Task<PredictionResultDto> Prediction(ExpenseDto expenseDto, string userId)
        {
            var models = new RegressionModel[3];
            for(int i = 0; i < 3; i++)
            {
                models[i] = new RegressionModel(3);
            }
            
            await CalculateModels(models);

            var result = new PredictionResultDto();
            var expensePredictors = await GetPredictorsForExpense(expenseDto, userId);
            result.FirstMonthValue = models[0].PredictTarget(expensePredictors);
            result.SecondMonthValue = models[1].PredictTarget(expensePredictors);
            result.ThirdMonthValue = models[2].PredictTarget(expensePredictors);

            return result;
        }

        private async Task<double[]> GetPredictorsForExpense(ExpenseDto expense, string userId)
        {
            var predictors = new double[3];
            var previousMonth = expense.Date.AddMonths(-1);
            predictors[0] = await GetMonthlyIncome(userId, previousMonth);
            predictors[1] = await GetMonthlyExpensesOfCategory(userId, previousMonth, expense.CategoryId);
            predictors[2] = expense.Value;

            return predictors;
        }

        private async Task CalculateModels(RegressionModel[] models)
        {
            var expenses = (await _expenseRepository.FindByConditionAsync(e => e.Main && e.Date.AddMonths(4).CompareTo(DateTime.Now) <= 0)).ToList();
            const int coefficients = 3;
            var predictors = new List<double[]>();

            foreach(var expense in expenses)
            {
                var predictorsValues = new double[coefficients];
                var previousMonth = expense.Date.AddMonths(-1);
                predictorsValues[0] = await GetMonthlyIncome(expense.UserId, previousMonth);
                predictorsValues[1] = await GetMonthlyExpensesOfCategory(expense.UserId, previousMonth, expense.CategoryId);
                predictorsValues[2] = expense.Value;
                predictors.Add(predictorsValues);
            }
            

            for(int i = 0; i < models.Length; i++)
            {
                var index = i;
                await CalculateModel(models[index], expenses, predictors, index + 1);
            }
        }

        private async Task CalculateModel(RegressionModel model, List<Expense> expenses, List<double[]> predictors, int month)
        {
            var targets = new List<double?>();
            foreach(var expense in expenses)
            {
                var target =
                    await GetMonthlyExpensesOfCategory(expense.UserId, expense.Date.AddMonths(month),
                        expense.CategoryId) -
                    await GetMonthlyExpensesOfCategory(expense.UserId, expense.Date.AddMonths(-1), expense.CategoryId);
                if (target < 0.0d) target = 0.0d;
                targets.Add(target);
            }

            var dataSet = new DataSet(targets.ToArray(), predictors.ToArray());
            model.CalculateCoefficients(dataSet);
        }

        private async Task<double> GetMonthlyIncome(string userId, DateTime date)
        {
            var incomes = await _incomeRepository.FindByConditionAsync(i => i.UserId == userId &&
                                                                            i.Date.Year == date.Year &&
                                                                            i.Date.Month == date.Month);
            return incomes.Sum(i => i.Value);
        }
        private async Task<double> GetMonthlyExpensesOfCategory(string userId, DateTime date, string categoryId)
        {
            var expenses = await _expenseRepository.FindByConditionAsync(e => e.UserId == userId &&
                                                                                e.Date.Year == date.Year &&
                                                                                e.Date.Month == date.Month &&
                                                                                e.CategoryId == categoryId &&
                                                                                !e.Main);
            return expenses.Sum(e => e.Value);
        }
    }
}
