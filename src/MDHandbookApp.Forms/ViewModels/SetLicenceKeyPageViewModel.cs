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
    public class SetLicenceKeyPageViewModel : ViewModelBase
    {
        public DelegateCommand SetLicenceKey { get; set; }
        public DelegateCommand NavigateToMainPage { get; set; }

        private IObservable<bool> islicenced;
        private IObservable<bool> isunauthorized;

        private bool _enableSetLicenceKeyButton;
        public bool EnableSetLicenceKeyButton
        {
            get { return _enableSetLicenceKeyButton; }
            set { SetProperty(ref _enableSetLicenceKeyButton, value); }
        }

        private bool _showLicencedMessage;
        public bool ShowLicencedMessage
        {
            get { return _showLicencedMessage; }
            set { SetProperty(ref _showLicencedMessage, value); }
        }

        private bool _showUnauthorizedMessage;
        public bool ShowUnauthorizedMessage
        {
            get { return _showUnauthorizedMessage; }
            set { SetProperty(ref _showUnauthorizedMessage, value); }
        }

        private string _licenceKeyString;
        public string LicenceKeyString
        {
            get { return _licenceKeyString; }
            set { SetProperty(ref _licenceKeyString, value); }
        }
        
        public SetLicenceKeyPageViewModel(
            ILogService logService,
            INavigationService navigationService,
            IReduxService reduxService,
            IServerActionCreators serverActionCreators) : base(logService, navigationService, reduxService, serverActionCreators)
        {
            SetLicenceKey = DelegateCommand.FromAsyncHandler(setLicenceKey);
            NavigateToMainPage = DelegateCommand.FromAsyncHandler(navigateToMainPage);
        }

        private async Task setLicenceKey()
        {
            await _reduxService.Store.Dispatch(_serverActionCreators.VerifyLicenceKeyAction(_licenceKeyString));
        }

        protected override void setupObservables()
        {
            islicenced = _reduxService.Store
                .DistinctUntilChanged(state => new { state.CurrentState.IsLicensed })
                .Select(d => d.CurrentState.IsLicensed);

            isunauthorized = _reduxService.Store
                .DistinctUntilChanged(state => new { state.CurrentState.HasUnauthorizedError })
                .Select(d => d.CurrentState.HasUnauthorizedError);
        }

        protected override void setupSubscriptions()
        {
            islicenced
                .DistinctUntilChanged()
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(x =>
                {
                    EnableSetLicenceKeyButton = !x;
                    ShowLicencedMessage = x;
                });

            isunauthorized
                .DistinctUntilChanged()
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(x => {
                    ShowUnauthorizedMessage = x;
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
