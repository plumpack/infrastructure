using Microsoft.Extensions.Logging;
using SharpDataAccess;

namespace PlumPack.Infrastructure.Data.Impl
{
    [Service(typeof(IDataAccessLogger))]
    public class DataAccessLogger : IDataAccessLogger
    {
        private readonly ILogger<DataAccessLogger> _logger;

        public DataAccessLogger(ILogger<DataAccessLogger> logger)
        {
            _logger = logger;
        }
        
        public void Warn(string message)
        {
            _logger.LogWarning(message);
        }

        public void Info(string message)
        {
            _logger.LogInformation(message);
        }
    }
}