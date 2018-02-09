using System;
using System.Diagnostics.Contracts;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace JezekT.RopeClimbing.Api.Client
{
    public class RopeClimbingHttpClient
    {
        private const int InitAttemptMaxCount = 5;

        private HttpClient _httpClient;
        private readonly ILogger _logger;
        private readonly string _identityServerUrl;
        private readonly string _tokenClientId;
        private readonly string _tokenClientSecret;
        private readonly string _apiUrl;

        private int _initAttemptNumber;
        private bool _disposing;
        private DateTime _accessTokenExpireTime;


        public async Task<string> GetResponseAsync(string requestRoute)
        {
            if (!_disposing)
            {
                try
                {
                    if ((_httpClient == null || _accessTokenExpireTime <= DateTime.Now) &&
                        !await InitializeClientAsync())
                    {
                        return null;
                    }

                    var getResult = await _httpClient.GetAsync(_apiUrl + requestRoute).ConfigureAwait(false);
                    if (getResult.IsSuccessStatusCode)
                    {
                        var responseString = await getResult.Content.ReadAsStringAsync().ConfigureAwait(false);
                        return responseString;
                    }
                    _logger.LogError($"Failed to get response. Status code: {getResult.StatusCode}.");
                    return null;
                }
                catch (HttpRequestException ex)
                {
                    _logger.LogError($"Failed to get response. {ex.GetBaseException().Message}");
                }
                catch (JsonReaderException ex)
                {
                    _logger.LogError($"Failed to read json. {ex.GetBaseException().Message}");
                }
                catch (OperationCanceledException ex)
                {
                    _logger.LogError($"Failed to get response. Task canceled. {ex.GetBaseException().Message}");
                }
                catch (WebException ex)
                {
                    _logger.LogError($"Failed to get response. Web exception occured. {ex.GetBaseException().Message}");
                }
            }
            return null;
        }

        public async Task<string> PostAsync(string requestRoute, object obj)
        {
            if (!_disposing)
            {
                try
                {
                    if ((_httpClient == null || _accessTokenExpireTime <= DateTime.Now) && !await InitializeClientAsync())
                    {
                        return null;
                    }

                    var json = obj != null ? JsonConvert.SerializeObject(obj, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects }) : null;
                    HttpResponseMessage postResult;
                    if (json != null)
                    {
                        var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
                        postResult = await _httpClient.PostAsync(_apiUrl + requestRoute, stringContent).ConfigureAwait(false);
                    }
                    else
                    {
                        postResult = await _httpClient.PostAsync(_apiUrl + requestRoute, null).ConfigureAwait(false);
                    }

                    if (postResult.IsSuccessStatusCode)
                    {
                        return await postResult.Content.ReadAsStringAsync().ConfigureAwait(false);
                    }
                    _logger.LogError($"Failed to post request. Status code: {postResult.StatusCode}.");
                    return null;
                }
                catch (HttpRequestException ex)
                {
                    _logger.LogError($"Failed to get response. {ex.GetBaseException().Message}");
                }
                catch (JsonReaderException ex)
                {
                    _logger.LogError($"Failed to read json. {ex.GetBaseException().Message}");
                }
                catch (OperationCanceledException ex)
                {
                    _logger.LogError($"Failed to get response. Task canceled. {ex.GetBaseException().Message}");
                }
                catch (WebException ex)
                {
                    _logger.LogError($"Failed to get response. Web exception occured. {ex.GetBaseException().Message}");
                }
            }
            return null;
        }

        public void Dispose()
        {
            if (!_disposing)
            {
                _disposing = true;
                _httpClient?.Dispose();
            }
        }


        public RopeClimbingHttpClient(string identityServerUrl, string tokenClientId, string tokenClientSecret, string apiUrl, ILogger logger)
        {
            if (string.IsNullOrEmpty(identityServerUrl) || string.IsNullOrEmpty(tokenClientId) || string.IsNullOrEmpty(tokenClientSecret) ||
                string.IsNullOrEmpty(apiUrl) || logger == null) throw new ArgumentNullException();
            Contract.EndContractBlock();

            _identityServerUrl = identityServerUrl;
            _tokenClientId = tokenClientId;
            _tokenClientSecret = tokenClientSecret;
            _apiUrl = apiUrl;
            _logger = logger;
        }


        private async Task<bool> InitializeClientAsync()
        {
            _logger.LogInformation("Initializing Http Client...");
            _initAttemptNumber = 0;
            while (_initAttemptNumber < InitAttemptMaxCount && !_disposing)
            {
                _initAttemptNumber++;
                var disco = await DiscoveryClient.GetAsync(_identityServerUrl).ConfigureAwait(false);
                if (!disco.IsError)
                {
                    var tokenClient = new TokenClient(disco.TokenEndpoint, _tokenClientId, _tokenClientSecret);
                    var tokenResponse = await tokenClient.RequestClientCredentialsAsync().ConfigureAwait(false);
                    if (!tokenResponse.IsError)
                    {
                        _accessTokenExpireTime = DateTime.Now.AddSeconds(tokenResponse.ExpiresIn).AddMinutes(-1);
                        _httpClient = new HttpClient();
                        _httpClient.SetBearerToken(tokenResponse.AccessToken);
                        _logger.LogInformation("Http Client initialized");
                        return true;
                    }
                    _logger.LogError($"Failed to get access token. {tokenResponse.Error}");
                }
                else
                {
                    _logger.LogError($"Failed to get discovery response. {disco.Error}");
                }
                await Task.Delay(5000);
            }
            return false;
        }

    }
}
