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
using Prism.Commands;
using Prism.Navigation;
using ReactiveUI;


namespace MDHandbookApp.Forms.ViewModels
{
    public class MenuPageViewModel : ViewModelBase
    {
        private bool _networkbusy;

        public DelegateCommand NavigateToAboutPage { get; set; }
        public DelegateCommand NavigateToPrivacyPage { get; set; }
        public DelegateCommand NavigateToLoginPage { get; set; }
        public DelegateCommand NavigateToMainPage { get; set; }
        public DelegateCommand NavigateToSetLicenceKeyPage { get; set; }
        public DelegateCommand NavigateToOptionsPage { get; set; }

        private IObservable<bool> isloggedin;
        private IObservable<bool> islicenced;


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
        
        public MenuPageViewModel(
            ILogService logService,
            INavigationService navigationService,
            IReduxService reduxService,
            IServerActionCreators serverActionCreators) : base(logService, navigationService, reduxService, serverActionCreators)
        {
            NavigateToAboutPage = DelegateCommand.FromAsyncHandler(navigateToAboutPage);
            NavigateToLoginPage = DelegateCommand.FromAsyncHandler(navigateToLoginPage);
            NavigateToMainPage = DelegateCommand.FromAsyncHandler(navigateToMainPageRel);
            NavigateToOptionsPage = DelegateCommand.FromAsyncHandler(navigateToOptionsPage);
            NavigateToPrivacyPage = DelegateCommand.FromAsyncHandler(navigateToPrivacyPage);
            NavigateToSetLicenceKeyPage = DelegateCommand.FromAsyncHandler(navigateToSetLicenceKeyPage);
        }


        private async Task navigateToSetLicenceKeyPage()
        {
            await _navigationService.NavigateAsync(Constants.SetLicenceKeyPageRelUrl);
        }

        private async Task navigateToPrivacyPage()
        {
            await _navigationService.NavigateAsync(Constants.PrivacyPageRelUrl);
        }

        private async Task navigateToOptionsPage()
        {
            await _navigationService.NavigateAsync(Constants.OptionsPageRelUrl);
        }

        private async Task navigateToLoginPage()
        {
            await _navigationService.NavigateAsync(Constants.LoginPageRelUrl);
        }

        private async Task navigateToAboutPage()
        {
            await _navigationService.NavigateAsync(Constants.AboutPageRelUrl);
        }

        protected override void setupObservables()
        {
            isloggedin = _reduxService.Store
                .DistinctUntilChanged(state => new { state.CurrentState.IsLoggedIn })
                .Select(d => d.CurrentState.IsLoggedIn);

            islicenced = _reduxService.Store
                .DistinctUntilChanged(state => new { state.CurrentState.IsLicensed })
                .Select(d => d.CurrentState.IsLicensed);
        }

        protected override void setupSubscriptions()
        {
            isloggedin
                .DistinctUntilChanged()
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(
                    x => {
                        ShowLogin = !x;
                    });

            isloggedin
                .CombineLatest(islicenced, (x, y) => x || y)
                .DistinctUntilChanged()
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(
                    x => {
                        ShowOptions = x;
                    });

            isloggedin
                .CombineLatest(islicenced, (x, y) => x && !y)
                .DistinctUntilChanged()
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(
                    x => {
                        ShowSetLicenceKey = x;
                    });

        }

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            setupObservables();

            setupSubscriptions();
        }
    }
}
