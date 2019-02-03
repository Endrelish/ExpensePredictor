using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ExpensePrediction.BusinessLogicLayer.Interfaces.Services;
using ExpensePrediction.DataAccessLayer.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ExpensePrediction.WebAPI
{
    public class HostedRegressionService : IHostedService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public HostedRegressionService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var categoryService = scope.ServiceProvider.GetRequiredService<ICategoryService<ExpenseCategory>>();
                var predictionService = scope.ServiceProvider.GetRequiredService<IPredictionService>();

                var categories = (await categoryService.GetCategoriesAsync()).Select(c => c.Id);

                var tasks = new List<Task>();
                foreach (var category in categories)
                {
                    tasks.Add(predictionService.SetModels(category));
                }

                await Task.WhenAll(tasks);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}