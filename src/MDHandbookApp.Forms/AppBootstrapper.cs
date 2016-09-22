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
using System.Reactive.Linq;
using System.Threading.Tasks;
using MDHandbookApp.Forms.Actions;
using MDHandbookApp.Forms.Reducers;
using MDHandbookApp.Forms.Services;
using MDHandbookApp.Forms.Views;
using MDHandbookApp.Forms.Utilities;
using Microsoft.Practices.Unity;
using Prism.Navigation;
using Prism.Unity;
using System.Collections.Generic;
using JWT;

namespace MDHandbookApp.Forms
{
    public class AppBootstrapper : IAppBootstrapper
    {

#if DEBUG
        private static TimeSpan updateInterval = TimeSpan.FromSeconds(10);
        private static TimeSpan refreshTokenInterval = TimeSpan.FromSeconds(60);
        private const int MinimumRefreshTokenPeriodInDays = 1;
#else
        private static TimeSpan updateInterval = TimeSpan.FromHours(6);
        private static TimeSpan refreshTokenInterval = TimeSpan.FromHours(24);
        private const int MinimumRefreshTokenPeriodInDays = 15;
#endif

        private static TimeSpan throttleTime = TimeSpan.FromMilliseconds(100);

        private const string JWTExpiryTimeKey = "exp";

        private IReduxService _reduxService = null;
        private IServerActionCreators _serverActionCreators = null;
        private ILogService _logService = null;

        private IObservable<bool> isloggedin;
        private IObservable<bool> islicenced;
        private IObservable<bool> islicencekeyset;
        private IObservable<bool> canchecklicencekey;

        public AppBootstrapper()
        {
        }

        public void OnInitializedNavigation(INavigationService _nav)
        {
            _nav.NavigateAsync(ViewModels.Constants.MainPageAbsUrl, animated: false);
        }

        public void RegisterTypes(IUnityContainer _container)
        {
            _container.RegisterTypeForNavigation<AboutPage>();
            _container.RegisterTypeForNavigation<BookpagePage>();
            _container.RegisterTypeForNavigation<LicenceErrorPage>();
            _container.RegisterTypeForNavigation<LoginPage>();
            _container.RegisterTypeForNavigation<MainPage>();
            _container.RegisterTypeForNavigation<MenuPage>();
            _container.RegisterTypeForNavigation<NavPage>();
            _container.RegisterTypeForNavigation<OptionsPage>();
            _container.RegisterTypeForNavigation<PrivacyPage>();
            _container.RegisterTypeForNavigation<SetLicenceKeyPage>();
            _container.RegisterTypeForNavigation<UnauthorizedErrorPage>();
        }

        public void InitializeMDHandbookServices(IUnityContainer _container)
        {
            // Order is very important
            _container.RegisterType<ILogService, FullDebugLogService>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IOfflineService, OfflineService>(new ContainerControlledLifetimeManager());

            _container.RegisterType<IBookReducers, BookReducers>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IFullpageReducers, FullpageReducers>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IHandbookStateReducers, HandbookStateReducers>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IPostUpdateStateReducers, PostUpdateStateReducers>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IApplicationReducers, ApplicationReducers>(new ContainerControlledLifetimeManager());

            _container.RegisterType<IReduxService, ReduxService>(new ContainerControlledLifetimeManager());

            _container.RegisterType<IMobileClient, MyMobileClient>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IMobileService, AzureMobileService>(new ContainerControlledLifetimeManager());

            _container.RegisterType<IServerActionCreators, ServerActionCreators>(new ContainerControlledLifetimeManager());
        }



        public void SetupObservablesAndSubscriptions(IUnityContainer _container)
        {
            _reduxService = _reduxService ?? _container.Resolve<IReduxService>();
            _serverActionCreators = _serverActionCreators ?? _container.Resolve<IServerActionCreators>();
            _logService = _logService ?? _container.Resolve<ILogService>();

            setupObservables(_container);

            setupSubscriptions(_container);

        }

        private void setupObservables(IUnityContainer _container)
        {
            isloggedin = _reduxService.Store
                .DistinctUntilChanged(state => new { state.CurrentState.IsLoggedIn })
                .Select(d => d.CurrentState.IsLoggedIn);

            islicenced = _reduxService.Store
                .DistinctUntilChanged(state => new { state.CurrentState.IsLicensed })
                .Select(d => d.CurrentState.IsLicensed);

            islicencekeyset = _reduxService.Store
                .DistinctUntilChanged(state => new { state.CurrentState.IsLicenceKeySet })
                .Select(d => d.CurrentState.IsLicenceKeySet);

            canchecklicencekey = isloggedin
                .CombineLatest(islicencekeyset, (x, y) => x && y)
                .CombineLatest(islicenced, (x, y) => x && !y);
        }

        private void setupSubscriptions(IUnityContainer _container)
        {
            
            canchecklicencekey
                .Throttle(throttleTime)
                .DistinctUntilChanged()
                .Subscribe(
                    x => {
                        if(x)
                            _reduxService.Store.Dispatch(_serverActionCreators.VerifyLicenceKeyAction());
                    });

            Observable
                .Interval(updateInterval)
                .Subscribe(
                    x => {
                        checkUpdates();
                    });
            
            Observable
                .Interval(refreshTokenInterval)
                .Subscribe(
                    async x => {
                        await refreshToken();
                    });
        }

        private void checkUpdates()
        {
            if (_reduxService.Store.GetState().CurrentState.IsLicensed)
            {
                _reduxService.Store.Dispatch(_serverActionCreators.FullUpdateAction());
            }
        }

        private async Task refreshToken()
        {
            if(!_reduxService.Store.GetState().CurrentState.IsLicensed)
                return;

            if(shouldGetRefreshToken(_reduxService.Store.GetState().CurrentState.AuthToken))
            {
                await _reduxService.Store.Dispatch(_serverActionCreators.RefreshTokenAction());
            }

            return;
        }


        private bool shouldGetRefreshToken(string authToken)
        {
            Dictionary<string, object> payload = JsonWebToken.DecodeToObject(authToken, string.Empty, false) as Dictionary<string, object>;
            object expiryTimeObject;
            payload.TryGetValue(JWTExpiryTimeKey, out expiryTimeObject);
            var expiryTime = (long)expiryTimeObject;
            DateTimeOffset dtDateTime = new DateTimeOffset(1970, 1, 1, 0, 0, 0, 0, TimeSpan.FromHours(0));
            dtDateTime = dtDateTime.AddSeconds(expiryTime).ToLocalTime();
            var duration = dtDateTime.Subtract(DateTimeOffset.UtcNow);
            return (duration.TotalDays < MinimumRefreshTokenPeriodInDays);
        }



    }
}
