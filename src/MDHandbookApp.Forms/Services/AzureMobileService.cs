//
//  Copyright 2016  R. Stanley Hum <r.stanley.hum@gmail.com>
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
//

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ModernHttpClient;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using MDHandbookAppService.Common.Models.RequestMessages;
using MDHandbookAppService.Common.Models.ResponseMessages;
using MDHandbookAppService.Common.Models.Utility;



namespace MDHandbookApp.Forms.Services
{
    public class AzureMobileService : IDisposable, IMobileService
    {
        private IMobileClient _mobileClient;
        private HttpClient _httpClient;

        private const string VerifyLicenceKeyAPI = "api/verifylicencekey";
        private const string ResetUpdatesAPI = "api/resetupdates";
        private const string GetUpdatesAPI = "api/updates";
        private const string PostUpdatesAPI = "api/postupdates";
        private const string RefreshTokenAPI = "api/refreshtoken";
        private const string LoadAppLogAPI = "api/loadapplog";
        private const string ZumoAuthName = "X-ZUMO-AUTH";
        private const string ZumoApiVersionName = "ZUMO-API-VERSION";
        private const string ZumoApiVersion = "2.0.0";
        private const string ContentJsonString = "application/json";
        
        private ILogService _logService;

        public AzureMobileService(
            ILogService logService,
            IMobileClient mobileClient)
        {
            _logService = logService;
            _mobileClient = mobileClient;
            initializeHttpClient();
        }


        public void SetAzureUserCredentials(string userId, string token)
        {
            _mobileClient.SetUserCredentials(userId, token);
            _httpClient.DefaultRequestHeaders.Remove(ZumoAuthName);
            _httpClient.DefaultRequestHeaders.Add(ZumoAuthName, token);
        }


        public async Task<bool> VerifyLicenceKey(VerifyLicenceKeyMessage vlkm)
        {
            bool result = false;

            HttpResponseMessage response = null;
            HttpRequestMessage req = null;

            try
            {
                req = setupJSONRequest(VerifyLicenceKeyAPI, JsonConvert.SerializeObject(vlkm));
                response = await _httpClient.SendAsync(req);
                response.EnsureSuccessStatusCode();

                result = true;
                _logService.Info("LicenceKey is Verified");
            }
            catch (Exception ex)
            {
                if (response == null)
                {
                    _logService.InfoException("VerifyLicenceKey response is null", ex);
                    throw new ServerExceptions.NetworkFailure();
                }
                else
                {
                    switch (response.StatusCode)
                    {
                        case HttpStatusCode.Unauthorized:
                            _logService.InfoException("VerifyLicenceKey response code: Unauthorized", ex);
                            throw new ServerExceptions.Unauthorized();
                        case HttpStatusCode.BadRequest:
                            _logService.InfoException(string.Format("VerifyLicenceKey response code: BadRequest: {0}", await response.Content.ReadAsStringAsync()), ex);
                            throw new ServerExceptions.ActionFailure();
                        default:
                            _logService.InfoException(string.Format("VerifyLicenceKey response code: UnknownFailure: {0}", await response.Content.ReadAsStringAsync()), ex);
                            throw new ServerExceptions.UnknownFailure(ex);
                    }
                }
            }
            finally
            {
                if (req != null)
                {
                    req.Dispose();
                }

                if (response != null)
                {
                    response.Dispose();
                }
            }

            return result;
        }

        public async Task<bool> ResetUpdates()
        {
            bool result = false;

            HttpRequestMessage req = null;
            HttpResponseMessage response = null;

            try
            {
                req = setupJSONRequest(ResetUpdatesAPI, "");
                response = await _httpClient.SendAsync(req);
                response.EnsureSuccessStatusCode();

                result = true;
                _logService.Info("ResetUpdates success");
            }
            catch (Exception ex)
            {
                if (response == null)
                {
                    _logService.InfoException("ResetUdpates response is null", ex);
                    throw new ServerExceptions.NetworkFailure();
                }
                else
                {
                    switch (response.StatusCode)
                    {
                        case HttpStatusCode.Unauthorized:
                            _logService.InfoException("ResetUpdates response code: Unauthorized", ex);
                            throw new ServerExceptions.Unauthorized();
                        case HttpStatusCode.BadRequest:
                            _logService.InfoException(string.Format("ResetUpdates response code: BadRequest: {0}", await response.Content.ReadAsStringAsync()), ex);
                            throw new ServerExceptions.ActionFailure();
                        default:
                            _logService.InfoException(string.Format("ResetUpdates response code: UnknownFailure: {0}", await response.Content.ReadAsStringAsync()), ex);
                            throw new ServerExceptions.UnknownFailure(ex);
                    }
                }
            }
            finally
            {
                if (req != null)
                {
                    req.Dispose();
                }
                if (response != null)
                {
                    response.Dispose();
                }
            }

            return result;
        }

        public async Task<List<ServerUpdateMessage>> GetUpdates()
        {
            List<ServerUpdateMessage> results = null;

            HttpRequestMessage req = null;
            HttpResponseMessage response = null;

            try
            {
                req = setupJSONRequest(GetUpdatesAPI, "");
                response = await _httpClient.SendAsync(req);
                response.EnsureSuccessStatusCode();

                results = await doSuccessfulGetUpdates(response);
                _logService.Info(string.Format("GetUpdates results received: {0}", results.Count.ToString()));
            }
            catch (Exception ex)
            {
                if (response == null)
                {
                    _logService.InfoException("GetUpdates response is null", ex);
                    throw new ServerExceptions.NetworkFailure();
                }
                else
                {
                    switch (response.StatusCode)
                    {
                        case HttpStatusCode.Unauthorized:
                            _logService.InfoException("GetUpdates response code: Unauthorized", ex);
                            throw new ServerExceptions.Unauthorized();
                        case HttpStatusCode.BadRequest:
                            _logService.InfoException(string.Format("GetUpdates response code: BadRequest: {0}", await response.Content.ReadAsStringAsync()), ex);
                            throw new ServerExceptions.ActionFailure();
                        default:
                            _logService.InfoException(string.Format("GetUpdates response code: UnknownFailure: {0}", await response.Content.ReadAsStringAsync()), ex);
                            throw new ServerExceptions.UnknownFailure(ex);
                    }
                }
            }
            finally
            {
                if (req != null)
                {
                    req.Dispose();
                }

                if (response != null)
                {
                    response.Dispose();
                }
            }

            return results;
        }


        public async Task<bool> PostUpdates(UpdateJsonMessage ujm)
        {
            bool result = false;

            HttpRequestMessage req = null;
            HttpResponseMessage response = null;

            try
            {
                req = setupJSONRequest(PostUpdatesAPI, JsonConvert.SerializeObject(ujm));
                response = await _httpClient.SendAsync(req);
                response.EnsureSuccessStatusCode();

                result = true;
                _logService.Info("PostUpdates success");
            }
            catch (Exception ex)
            {
                if (response == null)
                {
                    _logService.InfoException("PostUpdates response is null", ex);
                    throw new ServerExceptions.NetworkFailure();
                }
                else
                {
                    switch (response.StatusCode)
                    {
                        case HttpStatusCode.Unauthorized:
                            _logService.InfoException("PostUpdates response code: Unauthorized", ex);
                            throw new ServerExceptions.Unauthorized();
                        case HttpStatusCode.BadRequest:
                        _logService.InfoException(string.Format("PostUpdates response code: BadRequest: {0}", await response.Content.ReadAsStringAsync()), ex);
                            throw new ServerExceptions.ActionFailure();
                        default:
                        _logService.InfoException(string.Format("PostUpdates response code: UnknownFailure: {0}", await response.Content.ReadAsStringAsync()), ex);
                            throw new ServerExceptions.UnknownFailure(ex);
                    }
                }
            }
            finally
            {
                if (req != null)
                {
                    req.Dispose();
                }

                if (response != null)
                {
                    response.Dispose();
                }
            }

            return result;
        }


        public async Task<string> RefreshToken()
        {
            string result = null;

            HttpRequestMessage req = null;
            HttpResponseMessage response = null;

            try
            {
                req = setupJSONRequest(RefreshTokenAPI, "");
                response = await _httpClient.SendAsync(req);
                response.EnsureSuccessStatusCode();

                result = await doSuccessfulRefreshToken(response);
                _logService.Info("RefreshToken success");
            }
            catch (Exception ex)
            {
                if (response == null)
                {
                    _logService.InfoException("RefreshToken response is null", ex);
                    throw new ServerExceptions.NetworkFailure();
                }
                else
                {
                    switch (response.StatusCode)
                    {
                        case HttpStatusCode.Unauthorized:
                            _logService.InfoException("PostUpdates response code: Unauthorized", ex);
                            throw new ServerExceptions.Unauthorized();
                        case HttpStatusCode.BadRequest:
                            _logService.InfoException(string.Format("RefreshToken response code: BadRequest: {0}", await response.Content.ReadAsStringAsync()), ex);
                            throw new ServerExceptions.ActionFailure();
                        default:
                            _logService.InfoException(string.Format("RefreshToken response code: UnknownFailure: {0}", await response.Content.ReadAsStringAsync()), ex);
                            throw new ServerExceptions.UnknownFailure(ex);
                    }
                }
            }
            finally
            {
                if (req != null)
                {
                    req.Dispose();
                }
                if (response != null)
                {
                    response.Dispose();
                }
            }
            return result;
        }


        public async Task<bool> LoadAppLog(List<AppLogItemMessage> items)
        {
            bool result = false;

            HttpRequestMessage req = null;
            HttpResponseMessage response = null;

            try
            {
                req = setupJSONRequest(LoadAppLogAPI, JsonConvert.SerializeObject(items));
                response = await _httpClient.SendAsync(req);
                response.EnsureSuccessStatusCode();

                result = true;
                _logService.Info("LoadAppLog success");
            }
            catch (Exception ex)
            {
                if (response == null)
                {
                    _logService.InfoException("LoadAppLog response is null", ex);
                    throw new ServerExceptions.NetworkFailure();
                }
                else
                {
                    switch (response.StatusCode)
                    {
                        case HttpStatusCode.Unauthorized:
                            _logService.InfoException("LoadAppLog response code: Unauthorized", ex);
                            throw new ServerExceptions.Unauthorized();
                        case HttpStatusCode.BadRequest:
                            _logService.InfoException(string.Format("LoadAppLog response code: BadRequest: {0}", await response.Content.ReadAsStringAsync()), ex);
                            throw new ServerExceptions.ActionFailure();
                        default:
                            _logService.InfoException(string.Format("LoadAppLog response code: UnknownFailure: {0}", await response.Content.ReadAsStringAsync()), ex);
                            throw new ServerExceptions.UnknownFailure(ex);
                    }
                }
            }
            finally
            {
                if (req != null)
                {
                    req.Dispose();
                }

                if (response != null)
                {
                    response.Dispose();
                }
            }
            return result;
        }


        public void Dispose()
        {
            _httpClient.Dispose();
        }


        private void initializeHttpClient()
        {
            _httpClient = new HttpClient(new NativeMessageHandler());
#if DEBUG
            _httpClient.BaseAddress = new Uri(Constants.TestMobileURL);
#else
            _httpClient.BaseAddress = new Uri(Constants.ProductionMobileURL);
#endif
            _httpClient.DefaultRequestHeaders.Add(ZumoApiVersionName, ZumoApiVersion);
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(ContentJsonString));
        }

        private HttpRequestMessage setupJSONRequest(string url, string content)
        {
            HttpRequestMessage result = new HttpRequestMessage(HttpMethod.Post, url);
            result.Content = new StringContent(content);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue(ContentJsonString);
            return result;
        }

        private async Task<List<ServerUpdateMessage>> doSuccessfulGetUpdates(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            List<ServerUpdateMessage> messages = JsonConvert.DeserializeObject<List<ServerUpdateMessage>>(content);
            return messages;
        }

        private async Task<string> doSuccessfulRefreshToken(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            TokenResponseMessage message = JsonConvert.DeserializeObject<TokenResponseMessage>(content);
            return message.Token;
        }


        public async Task<bool> Authenticate(LoginProviders provider)
        {
            MobileServiceAuthenticationProvider azureProvider = MobileServiceAuthenticationProvider.Google;

            switch (provider)
            {
                case LoginProviders.Microsoft:
                    azureProvider = MobileServiceAuthenticationProvider.MicrosoftAccount;
                    break;
                case LoginProviders.Facebook:
                    azureProvider = MobileServiceAuthenticationProvider.Facebook;
                    break;
                case LoginProviders.Twitter:
                    azureProvider = MobileServiceAuthenticationProvider.Twitter;
                    break;
                case LoginProviders.Google:
                    azureProvider = MobileServiceAuthenticationProvider.Google;
                    break;
                default:
                    throw new Exception("Should Never Get here");
            }

            _logService.Debug($"Authenticate with {azureProvider}");
            return await _mobileClient.Authenticate(azureProvider);
        }

        public string GetUserId()
        {
            return _mobileClient.CurrentUser.UserId;
        }

        public string GetToken()
        {
            return _mobileClient.CurrentUser.MobileServiceAuthenticationToken.ToString();
        }
    }
}
