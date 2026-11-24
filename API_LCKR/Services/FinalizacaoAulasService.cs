using API_LCKR.Repository;

namespace API_LCKR.Services
{
    public class FinalizacaoAulasService : BackgroundService
    {
        private static readonly TimeSpan Intervalo = TimeSpan.FromHours(1);

        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<FinalizacaoAulasService> _logger;

        public FinalizacaoAulasService(IServiceScopeFactory scopeFactory, ILogger<FinalizacaoAulasService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var timer = new PeriodicTimer(Intervalo);

            do
            {
                FinalizarAulas();
            }
            while (await timer.WaitForNextTickAsync(stoppingToken));
        }

        private void FinalizarAulas()
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var repository = scope.ServiceProvider.GetRequiredService<AulaRepository>();

                var finalizadas = repository.FinalizarAulasPassadas(DateOnly.FromDateTime(DateTime.Today));

                if (finalizadas > 0)
                {
                    _logger.LogInformation("{Quantidade} aula(s) marcada(s) como realizada(s) automaticamente.", finalizadas);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao finalizar as aulas automaticamente.");
            }
        }
    }
}
