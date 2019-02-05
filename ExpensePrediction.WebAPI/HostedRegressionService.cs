using ExpensePrediction.BusinessLogicLayer.Interfaces.Services;
using ExpensePrediction.DataAccessLayer.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ExpensePrediction.WebAPI
{
    public class HostedRegressionService : IHostedService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly System.Timers.Timer _timer;

        public HostedRegressionService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
            _timer = new System.Timers.Timer(24 * 60 * 60 * 1000);
            _timer.Elapsed += async (s, a) => await ComputeAsync();
            _timer.AutoReset = true;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await ComputeAsync();
            _timer.Start();
        }

        public async Task ComputeAsync()
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
            _timer.Stop();
            return Task.CompletedTask;
        }
    }
}