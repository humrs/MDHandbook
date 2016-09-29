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
using System.Reactive.Linq;
using System.Threading.Tasks;
using JWT;
using MDHandbookApp.Forms.Actions;
using MDHandbookApp.Forms.Reducers;
using MDHandbookApp.Forms.Services;
using MDHandbookApp.Forms.Utilities;
using MDHandbookApp.Forms.Views;
using Microsoft.Practices.Unity;
using Microsoft.WindowsAzure.MobileServices;
using Prism.Unity;


namespace MDHandbookApp.Forms
{
    public interface IAuthenticate
    {
        Task<bool> Authenticate(MobileServiceAuthenticationProvider provider);
    }

    public partial class App : PrismApplication
    {
#if DEBUG
        private static TimeSpan updateInterval = TimeSpan.FromSeconds(600);
        private static TimeSpan refreshTokenInterval = TimeSpan.FromSeconds(1200);
        private static TimeSpan clearNetworkInterval = TimeSpan.FromSeconds(150);
        private const int MinimumRefreshTokenPeriodInDays = 1;
#else
        private static TimeSpan updateInterval = TimeSpan.FromHours(6);
        private static TimeSpan refreshTokenInterval = TimeSpan.FromHours(24);
        private static TimeSpan clearNetworkInterval = TimeSpan.FromMinutes(15);
        private const int MinimumRefreshTokenPeriodInDays = 15;
#endif

        private const int maxUnauthorizedRetries = 6;
        private static TimeSpan throttleTime = TimeSpan.FromMilliseconds(100);

        private const string JWTExpiryTimeKey = "exp";

        private IObservable<bool> isloggedin;
        private IObservable<bool> islicenced;
        private IObservable<bool> islicencekeyset;
        private IObservable<bool> canchecklicencekey;
        private IObservable<bool> isnetworkdown;
        private IObservable<int>  unauthorizedcount;
        private IObservable<bool> needsupdate;


        public static IAuthenticate Authenticator { get; private set; }

        public static void Init(IAuthenticate authenticator)
        {
            Authenticator = authenticator;
        }


        public static MobileServiceClient ServerClient { get; private set; }


        public App(IPlatformInitializer initializer = null) : base(initializer)
        {
        }

        protected override void RegisterTypes()
        {
        }

        protected override void OnInitialized()
        {
            setupMobileClient();

            initializeServices();
           
            registerViews();

            setupMobileUser();

            setupObservables();

            setupSubscriptions();
            
            navigateToFirstPage();
        }

        private void navigateToFirstPage()
        {
            NavigationService.NavigateAsync(ViewModels.Constants.MainPageAbsUrl, animated: false);
        }

        private void setupSubscriptions()
        {
            var _reduxService = Container.Resolve<IReduxService>();
            var _serverActionCreators = Container.Resolve<IServerActionCreators>();

            canchecklicencekey
                .Throttle(throttleTime)
                .DistinctUntilChanged()
                .Subscribe(
                    x => {
                        if (x)
                            _reduxService.Store.Dispatch(_serverActionCreators.VerifyLicenceKeyAction());
                    });

            unauthorizedcount
                .Throttle(throttleTime)
                .DistinctUntilChanged()
                .Subscribe(x => {
                    if (x > maxUnauthorizedRetries)
                    {
                        _reduxService.Store.Dispatch(new LogoutAction());
                        _reduxService.Store.Dispatch(new SetReturnToMainPageAction());
                        _reduxService.Store.Dispatch(new SetUnauthorizedErrorAction());
                    }
                });

            Observable
                .Interval(updateInterval)
                .Subscribe(
                    x => {
                        _reduxService.Store.Dispatch(new SetNeedsUpdateAction());
                    });

            Observable
                .Interval(refreshTokenInterval)
                .Subscribe(
                    async x => {
                        await refreshToken();
                    });

            Observable
                .Interval(clearNetworkInterval)
                .Subscribe(
                    x => {
                        if(_reduxService.Store.GetState().CurrentEventsState.IsNetworkDown)
                            _reduxService.Store.Dispatch(new ClearIsNetworkDownAction());

                    });

            needsupdate
                .Throttle(throttleTime)
                .DistinctUntilChanged()
                .Subscribe(x => {
                    if(x)
                    {
                        checkUpdates();
                    }
                });
        }

        private void setupObservables()
        {
            var _reduxService = Container.Resolve<IReduxService>();

            isloggedin = _reduxService.Store
                .DistinctUntilChanged(state => new { state.CurrentState.IsLoggedIn })
                .Select(d => d.CurrentState.IsLoggedIn);

            islicenced = _reduxService.Store
                .DistinctUntilChanged(state => new { state.CurrentState.IsLicensed })
                .Select(d => d.CurrentState.IsLicensed);

            islicencekeyset = _reduxService.Store
                .DistinctUntilChanged(state => new { state.CurrentState.IsLicenceKeySet })
                .Select(d => d.CurrentState.IsLicenceKeySet);

            isnetworkdown = _reduxService.Store
                .DistinctUntilChanged(state => new { state.CurrentEventsState.IsNetworkDown })
                .Select(d => d.CurrentEventsState.IsNetworkDown);

            needsupdate = _reduxService.Store
                .DistinctUntilChanged(state => new { state.CurrentEventsState.NeedsUpdate })
                .Select(d => d.CurrentEventsState.NeedsUpdate);

            canchecklicencekey = isnetworkdown
                .CombineLatest(isloggedin, (x, y) => !x && y)
                .CombineLatest(islicencekeyset, (x, y) => x && y)
                .CombineLatest(islicenced, (x, y) => x && !y);

            unauthorizedcount = _reduxService.Store
                .DistinctUntilChanged(state => new { state.CurrentEventsState.UnauthorizedCount })
                .Select(d => d.CurrentEventsState.UnauthorizedCount);
        }

        private void setupMobileUser()
        {
            var _reduxService = Container.Resolve<IReduxService>();
            var _mobileService = Container.Resolve<IMobileService>();
            if (_reduxService.Store.GetState().CurrentState.IsLoggedIn)
            {
                var userId = _reduxService.Store.GetState().CurrentState.UserId;
                var authtoken = _reduxService.Store.GetState().CurrentState.AuthToken;
                _mobileService.SetAzureUserCredentials(userId, authtoken);
            }
        }

        private void registerViews()
        {
            Container.RegisterTypeForNavigation<AboutPage>();
            Container.RegisterTypeForNavigation<BookpagePage>();
            Container.RegisterTypeForNavigation<LoginPage>();
            Container.RegisterTypeForNavigation<MainPage>();
            Container.RegisterTypeForNavigation<MenuPage>();
            Container.RegisterTypeForNavigation<NavPage>();
            Container.RegisterTypeForNavigation<OptionsPage>();
            Container.RegisterTypeForNavigation<PrivacyPage>();
            Container.RegisterTypeForNavigation<SetLicenceKeyPage>();
        }

        private void initializeServices()
        {
            // Order is very important. This is split up with the Configure Container in App.xaml.cs
            Container.RegisterType<IOfflineService, OfflineService>(new ContainerControlledLifetimeManager());
            Container.RegisterType<ILogStoreService, LogStoreService>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IMyLogger, FullLogger>(new ContainerControlledLifetimeManager());
            Container.RegisterType<ILogService, FullDebugLogService>(new ContainerControlledLifetimeManager());

            Container.RegisterType<IBookReducers, BookReducers>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IFullpageReducers, FullpageReducers>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IHandbookStateReducers, HandbookStateReducers>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IPostUpdateStateReducers, PostUpdateStateReducers>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IEventsStateReducers, EventsStateReducers>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IApplicationReducers, ApplicationReducers>(new ContainerControlledLifetimeManager());

            Container.RegisterType<IReduxService, ReduxService>(new ContainerControlledLifetimeManager());

            Container.RegisterType<IMobileClient, MyMobileClient>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IMobileService, AzureMobileService>(new ContainerControlledLifetimeManager());

            Container.RegisterType<IServerActionCreators, ServerActionCreators>(new ContainerControlledLifetimeManager());
        }

        protected override void OnStart()
        {
            base.OnStart();
            var _reduxService = Container.Resolve<IReduxService>();
            _reduxService.Store.Dispatch(new SetNeedsUpdateAction());
        }

        protected async override void OnSleep()
        {
            var _offlineService = Container.Resolve<IOfflineService>();
            var _reduxService = Container.Resolve<IReduxService>();
            var _logStoreService = Container.Resolve<ILogStoreService>();
            await _offlineService.SaveAppState(_reduxService.Store.GetState());
            await _offlineService.SaveLogStore(_logStoreService.LogStore);
        }

        private void setupMobileClient()
        {
#if DEBUG
            ServerClient = new MobileServiceClient(Constants.TestMobileURL);
            ServerClient.AlternateLoginHost = new Uri(Constants.ProductionMobileURL);
#else
            ServerClient = new MobileServiceClient(Constants.ProductionMobileURL);
#endif
        }

        private void checkUpdates()
        {
            var _reduxService = Container.Resolve<IReduxService>();
            var _serverActionCreators = Container.Resolve<IServerActionCreators>();

            if (_reduxService.Store.GetState().CurrentState.IsLicensed)
            {
                _reduxService.Store.Dispatch(_serverActionCreators.FullUpdateAction());
            }
            _reduxService.Store.Dispatch(new ClearNeedsUpdateAction());
        }

        private async Task refreshToken()
        {
            var _reduxService = Container.Resolve<IReduxService>();
            var _serverActionCreators = Container.Resolve<IServerActionCreators>();

            if (!_reduxService.Store.GetState().CurrentState.IsLicensed)
                return;

            if (shouldGetRefreshToken(_reduxService.Store.GetState().CurrentState.AuthToken))
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
