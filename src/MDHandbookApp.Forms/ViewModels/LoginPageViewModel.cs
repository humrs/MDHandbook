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
using MDHandbookApp.Forms.Services;
using Prism.Commands;
using Prism.Logging;
using Prism.Navigation;

namespace MDHandbookApp.Forms.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {
        public DelegateCommand LoginGoogle { get; set; }
        public DelegateCommand LoginFacebook { get; set; }
        public DelegateCommand LoginMicrosoft { get; set; }
        public DelegateCommand LoginTwitter { get; set; }
        
        public LoginPageViewModel(
            ILogService logService,
            INavigationService navigationService) : base(logService, navigationService)
        {
            LoginGoogle =    DelegateCommand.FromAsyncHandler(loginGoogle);
            LoginFacebook =  DelegateCommand.FromAsyncHandler(loginFacebook);
            LoginMicrosoft = DelegateCommand.FromAsyncHandler(loginMicrosoft);
            LoginTwitter =   DelegateCommand.FromAsyncHandler(loginTwitter);
        }

        private async Task loginTwitter()
        {
            _logService.Log("Login Twitter", Category.Debug, Priority.Low);
            await navigateToMainPage();
        }

        private async Task loginMicrosoft()
        {
            _logService.Log("Login Microsoft", Category.Debug, Priority.Low);
            await navigateToMainPage();
        }

        private async Task loginFacebook()
        {
            _logService.Log("Login Facebook", Category.Debug, Priority.Low);
            await navigateToMainPage();
        }

        private async Task loginGoogle()
        {
            _logService.Log("Login Google", Category.Debug, Priority.Low);
            await navigateToMainPage();
        }
    }
}
