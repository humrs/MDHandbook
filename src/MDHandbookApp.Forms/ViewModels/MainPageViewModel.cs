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
    public class MainPageViewModel : ViewModelBase
    {
        private INavigationService _navigationService;
        private bool _loggedIn = false;
        private bool _licenced = false;
        private bool _busy = false;

        public DelegateCommand<string> NavigateCommand { get; set; }

        public DelegateCommand ToggleLoggedIn { get; set; }
        public DelegateCommand ToggleLicenced { get; set; }
        public DelegateCommand ToggleBusy { get; set; }

        private bool _showActivityIndicator = false;
        public bool ShowActivityIndicator
        {
            get { return _showActivityIndicator; }
            set { SetProperty(ref _showActivityIndicator, value); }
        }

        private bool _showNotLoggedInMessage = true;
        public bool ShowNotLoggedInMessage
        {
            get { return _showNotLoggedInMessage; }
            set { SetProperty(ref _showNotLoggedInMessage, value); }
        }
        
        private bool _showNotLicencedMessage = false;
        public bool ShowNotLicencedMessage
        {
            get { return _showNotLicencedMessage; }
            set { SetProperty(ref _showNotLicencedMessage, value); }
        }

        private bool _showNeedLoginAndLicencedMessage = false;
        public bool ShowNeedLoginAndLicencedMessage
        {
            get { return _showNeedLoginAndLicencedMessage; }
            set { SetProperty(ref _showNeedLoginAndLicencedMessage, value); }
        }

        private bool _showBookList = false;
        public bool ShowBookList
        {
            get { return _showBookList; }
            set { SetProperty(ref _showBookList, value); }
        }
        
        public MainPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            NavigateCommand = new DelegateCommand<string>(Navigate);

            ToggleLoggedIn = new DelegateCommand(toggleLoggedIn);
            ToggleLicenced = new DelegateCommand(toggleLicenced);
            ToggleBusy = new DelegateCommand(toggleBusy);

            _loggedIn = false;
            _licenced = false;
            updateShowFunctions();
        }

        private void toggleBusy()
        {
            _busy = !_busy;
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
            ShowNotLoggedInMessage = updateShowNotLoggedInMessage(_loggedIn);
            ShowNotLicencedMessage = updateShowNotLicencedMessage(_loggedIn,_licenced);
            ShowNeedLoginAndLicencedMessage = updateShowNeedLoginAndLicencedMessage(_loggedIn, _licenced);
            ShowActivityIndicator = updateShowActivityIndicator(_busy);
            ShowBookList = updateShowBookList(_loggedIn, _licenced);
        }

        private bool updateShowBookList(bool loggedIn, bool licenced)
        {
            return loggedIn && licenced;
        }

        private bool updateShowActivityIndicator(bool busy)
        {
            return busy;
        }

        private bool updateShowNeedLoginAndLicencedMessage(bool loggedIn, bool licenced)
        {
            return !(loggedIn && licenced);
        }

        private bool updateShowNotLicencedMessage(bool loggedIn, bool licenced)
        {
            return loggedIn && !licenced;
        }

        private bool updateShowNotLoggedInMessage(bool loggedIn)
        {
            return !loggedIn;
        }
    }
}
