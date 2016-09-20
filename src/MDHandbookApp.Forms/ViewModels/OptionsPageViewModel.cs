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
using MDHandbookApp.Forms.Services;
using Prism.Commands;
using Prism.Logging;
using Prism.Navigation;


namespace MDHandbookApp.Forms.ViewModels
{
    public class OptionsPageViewModel : ViewModelBase
    {

#if DEBUG
        private bool _loggedIn = false;
        private bool _licenced = false;
#endif
        public DelegateCommand NavigateToMainPage { get; set; }
        public DelegateCommand Logout { get; set; }
        public DelegateCommand ResetLicenceKey { get; set; }
        public DelegateCommand RefreshContents { get; set; }

#if DEBUG
        public DelegateCommand ToggleLoggedIn { get; set; }
        public DelegateCommand ToggleLicenced { get; set; }
#endif

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
        
        public OptionsPageViewModel(
            ILogService logService,
            INavigationService navigationService) : base(logService, navigationService)
        {
            NavigateToMainPage = DelegateCommand.FromAsyncHandler(navigateToMainPage);

            Logout = new DelegateCommand(logout);
            ResetLicenceKey = new DelegateCommand(resetLicenceKey);
            RefreshContents = new DelegateCommand(refreshContents);

#if DEBUG
            ToggleLoggedIn = new DelegateCommand(toggleLoggedIn);
            ToggleLicenced = new DelegateCommand(toggleLicenced);
#endif

            _loggedIn = false;
            _licenced = false;
            updateShowFunctions();
        }

        private void refreshContents()
        {
            _logService.Log("Refresh Contents", Category.Debug, Priority.Low);
        }

        private void resetLicenceKey()
        {
            _logService.Log("Reset Licence Key", Category.Debug, Priority.Low);
        }

        private void logout()
        {
            _logService.Log("Logout", Category.Debug, Priority.Low);
            
        }


#if DEBUG
        private void toggleLicenced()
        {
            _licenced = !_licenced;
            updateShowFunctions();
        }
        
        private void toggleLoggedIn()
        {
            _loggedIn = !_loggedIn;
            updateShowFunctions();
        }
#endif

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
