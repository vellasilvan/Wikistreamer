using Polly;
using Polly.Contrib.WaitAndRetry;
using Polly.Extensions.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Extensions
{
    public static class RetryPolicies
    {

        //This is a retry policy with the jitter strategy to prevent retries to trigger at the same time.
        static public IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {

            const short BaseDelay = 1;
            const short MaxRetry = 5;

            var delay = Backoff.DecorrelatedJitterBackoffV2(medianFirstRetryDelay: TimeSpan.FromSeconds(BaseDelay), retryCount: MaxRetry);

            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(delay);
        }
    }
}
