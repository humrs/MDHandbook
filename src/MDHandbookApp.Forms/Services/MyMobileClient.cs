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

using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;


namespace MDHandbookApp.Forms.Services
{
    public class MyMobileClient : IMobileClient
    {

        public MobileServiceClient Client
        {
            get
            {
                return App.ServerClient;
            }
        }

        public MobileServiceUser CurrentUser
        {
            get
            {
                return App.ServerClient.CurrentUser;
            }
        }

        public async Task<bool> Authenticate(MobileServiceAuthenticationProvider provider)
        {
            return await App.Authenticator.Authenticate(provider);
        }

        public void DisposeClient()
        {
        }

        public void SetUserCredentials(string userId, string token)
        {
            App.ServerClient.CurrentUser.UserId = userId;
            App.ServerClient.CurrentUser.MobileServiceAuthenticationToken = token;
        }
    }
}
