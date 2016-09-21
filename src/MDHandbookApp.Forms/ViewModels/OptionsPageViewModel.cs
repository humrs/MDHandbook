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
using Prism.Logging;
using Prism.Navigation;
using ReactiveUI;

namespace MDHandbookApp.Forms.ViewModels
{
    public class OptionsPageViewModel : ViewModelBase
    {
        public DelegateCommand NavigateToMainPage { get; set; }
        public DelegateCommand Logout { get; set; }
        public DelegateCommand ResetLicenceKey { get; set; }
        public DelegateCommand RefreshContents { get; set; }
        
        private IObservable<bool> isloggedin;
        private IObservable<bool> islicenced;
        
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
            INavigationService navigationService,
            IReduxService reduxService) : base(logService, navigationService, reduxService)
        {
            _reduxService = reduxService;

            NavigateToMainPage = DelegateCommand.FromAsyncHandler(navigateToMainPage);

            Logout = DelegateCommand.FromAsyncHandler(logout);
            ResetLicenceKey = DelegateCommand.FromAsyncHandler(resetLicenceKey);
            RefreshContents = DelegateCommand.FromAsyncHandler(refreshContents);
        }

        private async Task refreshContents()
        {
            _logService.Debug("Refresh Contents");
            await navigateToMainPage();
        }

        private async Task resetLicenceKey()
        {
            _logService.Debug("Reset Licence Key");
            _reduxService.Store.Dispatch(new ClearLicenceKeyAction());
            await navigateToMainPage();
        }

        private async Task logout()
        {
            _reduxService.Store.Dispatch(new LogoutAction());
            _logService.Debug("Logout");
            await navigateToMainPage();            
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
                        ShowLogout = x;
                    });

            islicenced
                .DistinctUntilChanged()
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(
                    x => {
                        ShowRefreshContents = x;
                        ShowResetLicenceKey = x;
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
