namespace ReadBattleResultService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;

            ReadBattleResult.Logger = _logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                //_logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                try
                {
                    if (string.IsNullOrEmpty(AppSettings.KancolleEOAPIFolder)) _logger.LogError("API folder not defined");
                    else ReadBattleResult.ReadAndParseFile(BattleResultPath, ParsedFleetPath);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.ToString());
                }

                await Task.Delay(60000, stoppingToken);
            }
        }

        private string ParsedFleetPath => Path.Combine(AppSettings.ParsedFleetFile, "parsedFleets.json");

        private string BattleResultPath => Path.Combine(AppSettings.KancolleEOAPIFolder, "kcsapi", "api_req_sortie", "battleresult");
    }
}