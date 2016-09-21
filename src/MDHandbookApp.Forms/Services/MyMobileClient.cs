using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
