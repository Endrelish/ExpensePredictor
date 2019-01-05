using ExpensePrediction.BusinessLogicLayer.Interfaces.Services;
using ExpensePrediction.BusinessLogicLayer.Regression;
using ExpensePrediction.BusinessLogicLayer.Regression.Model;
using ExpensePrediction.DataAccessLayer.Entities;
using ExpensePrediction.DataAccessLayer.Interfaces;
using ExpensePrediction.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

            var value = expenseDto.Value * 0.75d;
            await CalculateModels(models, value);

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

            return predictors;
        }

        private async Task CalculateModels(RegressionModel[] models, double value)
        {
            var expenses = (await _expenseRepository.FindByConditionAync(e => e.Value >= value && e.Date.AddMonths(4).CompareTo(DateTime.Now) <= 0)).ToList();
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

            var tasks = new Task[models.Length];

            for(int i = 0; i < models.Length; i++)
            {
                tasks[i] = new Task(async () => await CalculateModel(models[i], expenses, predictors, i + 1));
                tasks[i].Start();
            }

            await Task.WhenAll(tasks);
        }

        private async Task CalculateModel(RegressionModel model, List<Expense> expenses, List<double[]> predictors, int month)
        {
            var targets = new List<double?>();
            foreach(var expense in expenses)
            {
                targets.Add(await GetMonthlyExpensesOfCategory(expense.UserId, expense.Date.AddMonths(month), expense.CategoryId));
            }

            var dataSet = new DataSet(targets.ToArray(), predictors.ToArray());
            model.CalculateCoefficients(dataSet);
        }

        private async Task<double> GetMonthlyIncome(string userId, DateTime date)
        {
            var incomes = await _incomeRepository.FindByConditionAync(i => i.UserId == userId &&
                                                                            i.Date.Year == date.Year &&
                                                                            i.Date.Month == date.Month);
            return incomes.Sum(i => i.Value);
        }
        private async Task<double> GetMonthlyExpensesOfCategory(string userId, DateTime date, string categoryId)
        {
            var expenses = await _expenseRepository.FindByConditionAync(e => e.UserId == userId &&
                                                                                e.Date.Year == date.Year &&
                                                                                e.Date.Month == date.Month &&
                                                                                e.CategoryId == categoryId);
            return expenses.Sum(e => e.Value);
        }
    }
}
