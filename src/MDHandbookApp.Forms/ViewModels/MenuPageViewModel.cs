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
using Prism.Navigation;


namespace MDHandbookApp.Forms.ViewModels
{
    public class MenuPageViewModel : ViewModelBase
    {
        private INavigationService _navigationService;
        private bool _loggedIn = false;
        private bool _licenced = false;
        public DelegateCommand<string> NavigateCommand { get; set; }

        public DelegateCommand ToggleLoggedIn { get; set; }
        public DelegateCommand ToggleLicenced { get; set; }

        private bool _showLogin = true;
        public bool ShowLogin
        {
            get { return _showLogin; }
            set { SetProperty(ref _showLogin, value); }
        }
        
        private bool _showSetLicenceKey = false;
        public bool ShowSetLicenceKey
        {
            get { return _showSetLicenceKey; }
            set { SetProperty(ref _showSetLicenceKey, value); }
        }

        private bool _showOptions = false;
        public bool ShowOptions
        {
            get { return _showOptions; }
            set { SetProperty(ref _showOptions, value); }
        }
        
        public MenuPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            NavigateCommand = new DelegateCommand<string>(Navigate);
            ToggleLoggedIn = new DelegateCommand(toggleLoggedIn);
            ToggleLicenced = new DelegateCommand(toggleLicenced);

            _loggedIn = false;
            _licenced = false;
            updateShowFunctions();
        }

        private void toggleLicenced()
        {
            _licenced = !_licenced;
            updateShowFunctions();
        }

        private void Navigate(string name)
        {
            _navigationService.NavigateAsync(name);
        }

        private void toggleLoggedIn()
        {
            _loggedIn = !_loggedIn;
            updateShowFunctions();
        }

        private void updateShowFunctions()
        {
            ShowLogin = updateShowLogin(_loggedIn);
            ShowSetLicenceKey = updateShowSetLicenceKey(_loggedIn, _licenced);
            ShowOptions = updateShowOptions(_loggedIn, _licenced);
        }

        private bool updateShowOptions(bool loggedIn, bool licenced)
        {
            return loggedIn || licenced;
        }

        private bool updateShowSetLicenceKey(bool loggedIn, bool licenced)
        {
            return loggedIn && !licenced;
        }

        private bool updateShowLogin(bool loggedIn)
        {
            return !loggedIn;
        }
    }
}
