using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;

namespace MDHandbookApp.Forms.Services
{
    public interface IMobileClient
    {
        MobileServiceClient Client { get; }
        MobileServiceUser   CurrentUser { get; }

        void SetUserCredentials(string userId, string token);

        Task<bool> Authenticate(MobileServiceAuthenticationProvider provider);

        void Dispose();
    }
}
