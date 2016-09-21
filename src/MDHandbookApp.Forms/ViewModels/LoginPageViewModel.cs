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
using MDHandbookApp.Forms.Actions;
using MDHandbookApp.Forms.Services;
using MDHandbookApp.Forms.Utilities;
using Prism.Commands;
using Prism.Logging;
using Prism.Navigation;

namespace MDHandbookApp.Forms.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {
        private IReduxService _reduxService;
        private IServerActionCreators _serverActionCreators;
        
        public DelegateCommand LoginGoogle { get; set; }
        public DelegateCommand LoginFacebook { get; set; }
        public DelegateCommand LoginMicrosoft { get; set; }
        public DelegateCommand LoginTwitter { get; set; }
        public DelegateCommand NavigateToMainPage { get; set; }
        
        public LoginPageViewModel(
            ILogService logService,
            INavigationService navigationService,
            IReduxService reduxService,
            IServerActionCreators serverActionCreators) : base(logService, navigationService)
        {
            _reduxService = reduxService;
            _serverActionCreators = serverActionCreators;

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
            await navigateToMainPage();
        }

        private async Task loginFacebook()
        {
            _logService.Debug("Login Facebook");
            await navigateToMainPage();
        }

        private async Task loginGoogle()
        {
            _logService.Debug("Login Google");
            await navigateToMainPage();
        }
    }
}
