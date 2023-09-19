using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;

namespace Products.Persistence;

    public class RetryPolicyProvider
    {
        private readonly ILogger<RetryPolicyProvider> _logger;

        public RetryPolicyProvider(ILogger<RetryPolicyProvider> logger)
        {
            _logger = logger;
        }

        public AsyncRetryPolicy GetDefaultRetryPolicy()
        {
            return Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(3, retryAttempt => 
                        TimeSpan.FromSeconds(2), 
                    (exception, timeSpan, retryCount, context) =>
                    {
                        _logger.LogWarning($"Delaying for {timeSpan.TotalSeconds} seconds, then making retry {retryCount}. Due to: {exception}");
                    });
        }
    }
