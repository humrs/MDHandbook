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

using Prism.Commands;


namespace MDHandbookApp.Forms.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {
        public DelegateCommand LoginGoogle { get; set; }
        public DelegateCommand LoginFacebook { get; set; }
        public DelegateCommand LoginMicrosoft { get; set; }
        public DelegateCommand LoginTwitter { get; set; }
        
        public LoginPageViewModel()
        {
            LoginGoogle = new DelegateCommand(loginGoogle);
            LoginFacebook = new DelegateCommand(loginFacebook);
            LoginMicrosoft = new DelegateCommand(loginMicrosoft);
            LoginTwitter = new DelegateCommand(loginTwitter);
        }

        private void loginTwitter()
        {
            System.Diagnostics.Debug.WriteLine("Login Twitter");
        }

        private void loginMicrosoft()
        {
            System.Diagnostics.Debug.WriteLine("Login Microsoft");
        }

        private void loginFacebook()
        {
            System.Diagnostics.Debug.WriteLine("Login Facebook");
        }

        private void loginGoogle()
        {
            System.Diagnostics.Debug.WriteLine("Login Google");
        }
    }
}
