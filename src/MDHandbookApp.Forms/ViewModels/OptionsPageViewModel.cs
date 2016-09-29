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
        private IObservable<bool> islicencekeyset;
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
            IReduxService reduxService,
            IServerActionCreators serverActionCreators) : base(logService, navigationService, reduxService, serverActionCreators)
        {
            NavigateToMainPage = DelegateCommand.FromAsyncHandler(navigateToMainPage);

            Logout = DelegateCommand.FromAsyncHandler(logout);
            ResetLicenceKey = DelegateCommand.FromAsyncHandler(resetLicenceKey);
            RefreshContents = DelegateCommand.FromAsyncHandler(refreshContents);
        }

        private async Task refreshContents()
        {
            await _reduxService.Store.Dispatch(_serverActionCreators.RefreshContentsAction());
        }

        private async Task resetLicenceKey()
        {
            await _reduxService.Store.Dispatch(_serverActionCreators.ResetLicenceKeyAction());
        }

        private async Task logout()
        {
            await _reduxService.Store.Dispatch(_serverActionCreators.LogoutAction());
        }

        protected override void setupObservables()
        {
            isloggedin = _reduxService.Store
                .DistinctUntilChanged(state => new { state.CurrentState.IsLoggedIn })
                .Select(d => d.CurrentState.IsLoggedIn);

            islicencekeyset = _reduxService.Store
                .DistinctUntilChanged(state => new { state.CurrentState.IsLicenceKeySet })
                .Select(d => d.CurrentState.IsLicenceKeySet);

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

            islicencekeyset
                .DistinctUntilChanged()
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(
                    x => {
                        ShowResetLicenceKey = x;
                    });

            islicenced
                .DistinctUntilChanged()
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(
                    x => {
                        ShowRefreshContents = x;
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
