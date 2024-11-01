﻿using AiPocWebsiteTemplateWithBackend.API;
using AiPocWebsiteTemplateWithBackend.API.Config;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AiPocWebsiteTemplateWithBackend.Client.IntelligenceHub
{
    public class AIAuthClient
    {
        private readonly HttpClient _client;
        private readonly AuthRequest _request;
        private readonly AuthRequest _elevatedRequest;

        private readonly string _authEndpoint;

        public AIAuthClient(AuthSettings settings, IHttpClientFactory factory)
        {
            _client = factory.CreateClient();
            _authEndpoint = settings.Endpoint;

            _request = new AuthRequest()
            {
                GrantType = settings.GrantType,
                ClientId = settings.DefaultClientId,
                ClientSecret = settings.DefaultClientSecret,
                Audience = settings.Audience,
            };

            _elevatedRequest = new AuthRequest()
            {
                GrantType = settings.GrantType,
                ClientId = settings.AdminClientId,
                ClientSecret = settings.AdminClientSecret,
                Audience = settings.Audience,
            };
        }

        // Returns a basic token that is safe to share with front end clients
        public async Task<AuthTokenResponse?> RequestAuthToken()
        {
            var json = JsonSerializer.Serialize(_request, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                var response = await _client.PostAsync(_authEndpoint, content);

                var jsonString = await response.Content.ReadAsStringAsync();

                var authToken = JsonSerializer.Deserialize<AuthTokenResponse>(jsonString, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                });

                return authToken ?? null;
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }

        // The token returned from the below method should not be shared with the front end
        public async Task<AuthTokenResponse?> RequestElevatedAuthToken()
        {
            var json = JsonSerializer.Serialize(_elevatedRequest, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                var response = await _client.PostAsync(_authEndpoint, content);

                var jsonString = await response.Content.ReadAsStringAsync();

                var authToken = JsonSerializer.Deserialize<AuthTokenResponse>(jsonString, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                });

                return authToken ?? null;
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }
    }
}