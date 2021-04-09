using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Azure.Core;
using Azure.Identity;

namespace CloudClientSecurity02
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceScope = Environment.GetEnvironmentVariable("APP_SERVICE_SCOPE");
            var serviceUri = Environment.GetEnvironmentVariable("APP_SERVICE_URI");

            var credential = new ChainedTokenCredential(
                new ManagedIdentityCredential(),
                new EnvironmentCredential());

            var token = credential.GetToken(new TokenRequestContext(new string[] { serviceScope }));

            var functionClient = new HttpClient();
            functionClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer",
                token.Token);

            var response = functionClient.GetAsync(serviceUri).Result;
            response.EnsureSuccessStatusCode();
        }
    }
}
