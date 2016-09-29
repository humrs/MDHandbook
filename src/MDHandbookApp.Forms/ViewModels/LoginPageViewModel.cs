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
using MDHandbookApp.Forms.Services;
using MDHandbookApp.Forms.Utilities;
using Prism.Commands;
using Prism.Navigation;
using ReactiveUI;


namespace MDHandbookApp.Forms.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {
        public DelegateCommand LoginGoogle { get; set; }
        public DelegateCommand LoginFacebook { get; set; }
        public DelegateCommand LoginMicrosoft { get; set; }
        public DelegateCommand LoginTwitter { get; set; }
        public DelegateCommand NavigateToMainPage { get; set; }

        private IObservable<bool>  isloggedin;
        private IObservable<bool>  isnetworkdown;
        private IObservable<bool?> loginsuccessful;
        private IObservable<bool>  isnetworkbusy;

        private IObservable<bool> showloggedin;
        private IObservable<bool> shownotloggedin;
        private IObservable<bool> showloginsuccessful;
        private IObservable<bool> showloginnotsuccessful;
        private IObservable<bool> shownetworkdown;
        private IObservable<bool> enableloginbutton;

        private bool _showActivityIndicator = false;
        public bool ShowActivityIndicator
        {
            get { return _showActivityIndicator; }
            set { SetProperty(ref _showActivityIndicator, value); }
        }

        private bool _enableLoginButton;
        public bool EnableLoginButton
        {
            get { return _enableLoginButton; }
            set { SetProperty(ref _enableLoginButton, value); }
        }

        private bool _showLoggedInMessage;
        public bool ShowLoggedInMessage
        {
            get { return _showLoggedInMessage; }
            set { SetProperty(ref _showLoggedInMessage, value); }
        }

        private bool _showNotLoggedInMessage;
        public bool ShowNotLoggedInMessage
        {
            get { return _showNotLoggedInMessage; }
            set { SetProperty(ref _showNotLoggedInMessage, value); }
        }


        private bool _showLoginSuccessfulMessage;
        public bool ShowLoginSuccessfulMessage
        {
            get { return _showLoginSuccessfulMessage; }
            set { SetProperty(ref _showLoginSuccessfulMessage, value); }
        }

        private bool _showLoginNotSuccessfulMessage;
        public bool ShowLoginNotSuccessfulMessage
        {
            get { return _showLoginNotSuccessfulMessage; }
            set { SetProperty(ref _showLoginNotSuccessfulMessage, value); }
        }

        private bool _showNetworkDownMessage;
        public bool ShowNetworkDownMessage
        {
            get { return _showNetworkDownMessage; }
            set { SetProperty(ref _showNetworkDownMessage, value); }
        }


        public LoginPageViewModel(
            ILogService logService,
            INavigationService navigationService,
            IReduxService reduxService,
            IServerActionCreators serverActionCreators) : base(logService, navigationService, reduxService, serverActionCreators)
        {
            LoginGoogle =    DelegateCommand.FromAsyncHandler(loginGoogle);
            LoginFacebook =  DelegateCommand.FromAsyncHandler(loginFacebook);
            LoginMicrosoft = DelegateCommand.FromAsyncHandler(loginMicrosoft);
            LoginTwitter =   DelegateCommand.FromAsyncHandler(loginTwitter);
            NavigateToMainPage = DelegateCommand.FromAsyncHandler(navigateToMainPage);
        }

        private async Task loginTwitter()
        {
            await _reduxService.Store.Dispatch(_serverActionCreators.LoginAction(LoginProviders.Twitter));    
        }

        
        private async Task loginMicrosoft()
        {
            await _reduxService.Store.Dispatch(_serverActionCreators.LoginAction(LoginProviders.Microsoft));
        }

        private async Task loginFacebook()
        {
            await _reduxService.Store.Dispatch(_serverActionCreators.LoginAction(LoginProviders.Facebook));
        }

        private async Task loginGoogle()
        {
            await _reduxService.Store.Dispatch(_serverActionCreators.LoginAction(LoginProviders.Google));
        }

        protected override void setupObservables()
        {
            isloggedin = _reduxService.Store
                .DistinctUntilChanged(state => new { state.CurrentState.IsLoggedIn })
                .Select(d => d.CurrentState.IsLoggedIn);

            isnetworkdown = _reduxService.Store
                .DistinctUntilChanged(state => new { state.CurrentEventsState.IsNetworkDown })
                .Select(d => d.CurrentEventsState.IsNetworkDown);

            loginsuccessful = _reduxService.Store
                .DistinctUntilChanged(state => new { state.CurrentEventsState.LoginSuccessful })
                .Select(d => d.CurrentEventsState.LoginSuccessful);

            isnetworkbusy = _reduxService.Store
                .DistinctUntilChanged(state => new { state.CurrentEventsState.IsNetworkBusy })
                .Select(d => d.CurrentEventsState.IsNetworkBusy);

            showloggedin = isloggedin
                .CombineLatest(loginsuccessful, (x, y) => x && y == null)
                .CombineLatest(isnetworkdown, (x, y) => x && !y);

            shownotloggedin = isloggedin
                .CombineLatest(loginsuccessful, (x, y) => !x && y == null)
                .CombineLatest(isnetworkdown, (x, y) => x && !y);
            
            showloginsuccessful = isloggedin
                .CombineLatest(loginsuccessful, (x, y) => x && (y != null) && (bool) y)
                .CombineLatest(isnetworkdown, (x, y) => x && !y);

            showloginnotsuccessful = isloggedin
                .CombineLatest(loginsuccessful, (x, y) => !x && y != null && (bool)!y)
                .CombineLatest(isnetworkdown, (x, y) => x && !y);

            shownetworkdown = isnetworkdown;

            enableloginbutton = isloggedin
                .CombineLatest(isnetworkbusy, (x, y) => !x && !y)
                .CombineLatest(isnetworkdown, (x, y) => x && !y);
        }

        protected override void setupSubscriptions()
        {
            showloggedin
                .DistinctUntilChanged()
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(x => {
                    ShowLoggedInMessage = x;
                })
                .DisposeWith(subscriptionDisposibles);

            shownotloggedin
                .DistinctUntilChanged()
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(x => {
                    ShowNotLoggedInMessage = x;
                })
                .DisposeWith(subscriptionDisposibles);

            showloginsuccessful
                .DistinctUntilChanged()
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(x => {
                    ShowLoginSuccessfulMessage = x;
                })
                .DisposeWith(subscriptionDisposibles);

            showloginnotsuccessful
                .DistinctUntilChanged()
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(x => {
                    ShowLoginNotSuccessfulMessage = x;
                })
                .DisposeWith(subscriptionDisposibles);

            shownetworkdown
                .DistinctUntilChanged()
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(x => {
                    ShowNetworkDownMessage = x;
                })
                .DisposeWith(subscriptionDisposibles);

            enableloginbutton
                .DistinctUntilChanged()
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(x => {
                    EnableLoginButton = x;
                })
                .DisposeWith(subscriptionDisposibles);

            isnetworkbusy
                .DistinctUntilChanged()
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(x => {
                    ShowActivityIndicator = x;
                })
                .DisposeWith(subscriptionDisposibles);
        }

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            _reduxService.Store.Dispatch(new ClearLoginSuccessfullAction());

            setupObservables();

            setupSubscriptions();
        }
    }

}
