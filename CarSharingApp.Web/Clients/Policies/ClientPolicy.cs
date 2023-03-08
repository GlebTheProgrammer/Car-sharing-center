using Polly;
using Polly.Contrib.WaitAndRetry;
using Polly.Retry;

namespace CarSharingApp.Web.Clients.Policies
{
    public sealed class ClientPolicy
    {
        public AsyncRetryPolicy<HttpResponseMessage> ImmediateHttpRetry { get; }
        public AsyncRetryPolicy<HttpResponseMessage> LinearHttpRetry { get; }
        public AsyncRetryPolicy<HttpResponseMessage> ExponentialHttpRetry { get; }
        public AsyncRetryPolicy<HttpResponseMessage> BackoffHttpRetry { get; }

        public ClientPolicy(int retryCount)
        {
            ImmediateHttpRetry = Policy.HandleResult<HttpResponseMessage>(
                res => (int) res.StatusCode == StatusCodes.Status500InternalServerError)
                .RetryAsync(retryCount);

            LinearHttpRetry = Policy.HandleResult<HttpResponseMessage>(
                res => (int) res.StatusCode == StatusCodes.Status500InternalServerError)
                .WaitAndRetryAsync(retryCount, retryAttempt => TimeSpan.FromSeconds(2));

            ExponentialHttpRetry = Policy.HandleResult<HttpResponseMessage>(
                res => (int) res.StatusCode == StatusCodes.Status500InternalServerError)
                .WaitAndRetryAsync(retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

            BackoffHttpRetry = Policy.HandleResult<HttpResponseMessage>(
                res => (int) res.StatusCode == StatusCodes.Status500InternalServerError)
                .WaitAndRetryAsync(Backoff.DecorrelatedJitterBackoffV2(TimeSpan.FromSeconds(2), retryCount));
        }
    }
}
