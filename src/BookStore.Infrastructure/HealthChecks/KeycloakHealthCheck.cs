using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Infrastructure.HealthChecks
{
    internal class KeycloakHealthCheck : IHealthCheck
    {
        private readonly HttpClient _httpClient;
        private readonly string _keycloakUrl;

        public KeycloakHealthCheck(HttpClient httpClient, string keycloakUrl)
        {
            _httpClient = httpClient;
            _keycloakUrl = keycloakUrl;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _httpClient.GetAsync(_keycloakUrl, cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    return HealthCheckResult.Healthy("Keycloak is running.");
                }
                else
                {
                    return HealthCheckResult.Unhealthy($"Keycloak returned status code {response.StatusCode}");
                }
            }
            catch (TaskCanceledException)
            {
                return HealthCheckResult.Unhealthy("Keycloak health check timed out.");
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy($"An exception occurred: {ex.Message}");
            }
        }
    }
}
