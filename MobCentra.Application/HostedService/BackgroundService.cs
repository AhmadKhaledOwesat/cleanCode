using MobCentra.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MobCentra.Application.HostedService
{
    public class BackgroundService(ILogger<BackgroundService> logger, IServiceProvider  serviceScope) : IHostedService, IDisposable
    {
        private Timer _timer;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            // Run every 5 minitues
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(2));
            return Task.CompletedTask;
        }

        private async void DoWork(object state)
        {

            using var scope = serviceScope.CreateScope();
            var deviceBll = scope.ServiceProvider.GetRequiredService<IDeviceBll>();
            var devices = await deviceBll.GetAllAsync();

            foreach (var device in devices)
            {
                device.IsOnline = 0;
            }
            if (devices is { Count: > 0 })
            {
                await deviceBll.UpdateRangeAsync(devices);
                await deviceBll.SendCommandAsync(new Dto.SendCommandDto { Command = "check_connectivity", Token = [.. devices.Select(a => a.Token)] });
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("MyBackgroundService is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
