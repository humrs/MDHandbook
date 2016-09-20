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


namespace UniHandbookApp.Forms.ViewModels
{
    public class OptionsPageViewModel : ViewModelBase
    {
        private INavigationService _navigationService;
        private bool _loggedIn = false;
        private bool _licenced = false;
        public DelegateCommand<string> NavigateCommand { get; set; }

        public DelegateCommand ToggleLoggedIn { get; set; }
        public DelegateCommand ToggleLicenced { get; set; }

        private bool _showLogout = false;
        public bool ShowLogout
        {
            get { return _showLogout; }
            set { SetProperty(ref _showLogout, value); }
        }
        
        private bool _showResetLicenceKey = false;
        public bool ShowResetLicenceKey
        {
            get { return _showResetLicenceKey; }
            set { SetProperty(ref _showResetLicenceKey, value); }
        }

        private bool _showRefreshContents = false;
        public bool ShowRefreshContents
        {
            get { return _showRefreshContents; }
            set { SetProperty(ref _showRefreshContents, value); }
        }
        
        public OptionsPageViewModel(INavigationService navigationService)
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
            ShowLogout = updateShowLogout(_loggedIn);
            ShowResetLicenceKey = updateShowResetLicenceKey(_loggedIn, _licenced);
            ShowRefreshContents = updateShowRefreshContents(_loggedIn, _licenced);
        }

        private bool updateShowRefreshContents(bool loggedIn, bool licenced)
        {
            return loggedIn && licenced;
        }

        private bool updateShowResetLicenceKey(bool loggedIn, bool licenced)
        {
            return loggedIn || licenced;
        }

        private bool updateShowLogout(bool loggedIn)
        {
            return loggedIn;
        }
    }
}
