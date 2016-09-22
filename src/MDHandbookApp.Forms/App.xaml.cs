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
using System.Threading;
using System.Threading.Tasks;
using MDHandbookApp.Forms.Services;
using Microsoft.WindowsAzure.MobileServices;
using Prism.Logging;
using Prism.Unity;


namespace MDHandbookApp.Forms
{
    public interface IAuthenticate
    {
        Task<bool> Authenticate(MobileServiceAuthenticationProvider provider);
    }

    public partial class App : PrismApplication
    {
        public static IAuthenticate Authenticator { get; private set; }

        public static void Init(IAuthenticate authenticator)
        {
            Authenticator = authenticator;
        }


        public static MobileServiceClient ServerClient { get; private set; }


        private static AppBootstrapper _appBootstrapper = null;

        public App(IPlatformInitializer initializer = null) : base(initializer)
        {
        }

        public override void Initialize()
        {
#if DEBUG
            ServerClient = new MobileServiceClient(Constants.TestMobileURL);
            ServerClient.AlternateLoginHost = new Uri(Constants.ProductionMobileURL);
#else
            ServerClient = new MobileServiceCLient(Constants.ProductionMobileURL);
#endif


            _appBootstrapper = _appBootstrapper ?? new AppBootstrapper();

            base.Initialize();

            _appBootstrapper.InitializeMDHandbookServices(Container);

            _appBootstrapper.SetupObservables(Container);

            _appBootstrapper.SetupSubscriptions(Container);
        }

        protected override void OnInitialized()
        {
            InitializeComponent();

            _appBootstrapper.OnInitializedNavigation(NavigationService);
        }

        protected override void RegisterTypes()
        {
            _appBootstrapper.RegisterTypes(Container);
        }

        protected override ILoggerFacade CreateLogger()
        {
            return new CustomDebugLogger();
        }
    }
}
