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
        
        private IObservable<bool> isloggedin;
        private IObservable<bool> isunauthorized;

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

        private bool _showUnauthorizedMessage;
        public bool ShowUnauthorizedMessage
        {
            get { return _showUnauthorizedMessage; }
            set { SetProperty(ref _showUnauthorizedMessage, value); }
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
            _logService.Debug("Login Twitter");
            await _reduxService.Store.Dispatch(_serverActionCreators.LoginAction(LoginProviders.Twitter));    
        }

        
        private async Task loginMicrosoft()
        {
            _logService.Debug("Login Microsoft");
            await _reduxService.Store.Dispatch(_serverActionCreators.LoginAction(LoginProviders.Microsoft));
        }

        private async Task loginFacebook()
        {
            _logService.Debug("Login Facebook");
            await login();
            //await _reduxService.Store.Dispatch(_serverActionCreators.LoginAction(LoginProviders.Facebook));
        }

        private async Task loginGoogle()
        {
            _logService.Debug("Login Google");
            await login();
            //await _reduxService.Store.Dispatch(_serverActionCreators.LoginAction(LoginProviders.Google));
        }

        private async Task login()
        {
            _reduxService.Store.Dispatch(new LoginAction { UserId = "humrs", AuthToken = "token" });
            //await navigateToMainPage();
        }

        protected override void setupObservables()
        {
            isloggedin = _reduxService.Store
                .DistinctUntilChanged(state => new { state.CurrentState.IsLoggedIn })
                .Select(d => d.CurrentState.IsLoggedIn);

            isunauthorized = _reduxService.Store
                .DistinctUntilChanged(state => new { state.CurrentState.HasUnauthorizedError })
                .Select(d => d.CurrentState.HasUnauthorizedError);
        }

        protected override void setupSubscriptions()
        {
            isloggedin
                .DistinctUntilChanged()
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(x =>
                    {
                        EnableLoginButton = !x;
                        ShowLoggedInMessage = x;
                    });

            isunauthorized
                .DistinctUntilChanged()
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(x => {
                        ShowUnauthorizedMessage = x;
                    });
        }

        private void doMove()
        {
            _logService.Debug("Hello");
        }

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            setupObservables();

            setupSubscriptions();
        }
    }

}
