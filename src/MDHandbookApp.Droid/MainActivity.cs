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
using System.Threading.Tasks;
using Android.App;
using Android.Content.PM;
using Android.OS;
using MDHandbookApp.Forms.Services;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.Practices.Unity;
using Prism.Unity;
using MDHandbookApp.Droid;

[assembly: Xamarin.Forms.Dependency(typeof(MainActivity))]
namespace MDHandbookApp.Droid
{
    [Activity(Label = "MDHandbookApp", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, IMobileClient
    {
        private MobileServiceClient _client;
        public MobileServiceClient Client
        {
            get
            {
                return _client;
            }
        }

        public MobileServiceUser CurrentUser
        {
            get
            {
                return _client.CurrentUser;
            }
        }

        public async Task<bool> Authenticate(MobileServiceAuthenticationProvider provider)
        {
            var success = false;
            try
            {
                var user = await _client.LoginAsync(this, provider);
                success = true;
            }
            catch (Exception)
            {

            }
            return success;

        }

        public void DisposeClient()
        {
            _client.Dispose();
        }

        public void SetUserCredentials(string userId, string token)
        {
            _client.CurrentUser.UserId = userId;
            _client.CurrentUser.MobileServiceAuthenticationToken = token;
        }

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.tabs;
            ToolbarResource = Resource.Layout.toolbar;

            base.OnCreate(bundle);

            Microsoft.WindowsAzure.MobileServices.CurrentPlatform.Init();

#if DEBUG
            _client = new MobileServiceClient(Constants.TestMobileURL);
            _client.AlternateLoginHost = new Uri(Constants.ProductionMobileURL);
#else
            _client = new MobileServiceClient(Constants.ProductionMobileURL);
#endif

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new MDHandbookApp.Forms.App(new AndroidInitializer()));
        }
    }

    public class AndroidInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IUnityContainer container)
        {

        }
    }
}

