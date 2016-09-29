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

        private IObservable<bool> isloggedin;
        private IObservable<bool> islicenced;
        private IObservable<bool> islicencekeyset;
        private IObservable<bool> isnetworkdown;
        private IObservable<bool?> licencekeysuccessful;
        private IObservable<bool> isnetworkbusy;

        private IObservable<bool> showlicenced;
        private IObservable<bool> shownotlicenced;
        private IObservable<bool> showlicencekeysuccessful;
        private IObservable<bool> showlicencekeynotsuccessful;
        private IObservable<bool> shownetworkdown;
        private IObservable<bool> enablesetlicencekeybutton;

        private bool _showActivityIndicator = false;
        public bool ShowActivityIndicator
        {
            get { return _showActivityIndicator; }
            set { SetProperty(ref _showActivityIndicator, value); }
        }

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

        private bool _showNotLicencedMessage;
        public bool ShowNotLicencedMessage
        {
            get { return _showNotLicencedMessage; }
            set { SetProperty(ref _showNotLicencedMessage, value); }
        }

        private bool _showLicenceKeySuccessfulMessage;
        public bool ShowLicenceKeySuccessfulMessage
        {
            get { return _showLicenceKeySuccessfulMessage; }
            set { SetProperty(ref _showLicenceKeySuccessfulMessage, value); }
        }

        private bool _showLicenceKeyNotSuccessfulMessage;
        public bool ShowLicenceKeyNotSuccessfulMessage
        {
            get { return _showLicenceKeyNotSuccessfulMessage; }
            set { SetProperty(ref _showLicenceKeyNotSuccessfulMessage, value); }
        }

        private bool _showNetworkDownMessage;
        public bool ShowNetworkDownMessage
        {
            get { return _showNetworkDownMessage; }
            set { SetProperty(ref _showNetworkDownMessage, value); }
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
            isloggedin = _reduxService.Store
                .DistinctUntilChanged(state => new { state.CurrentState.IsLoggedIn })
                .Select(d => d.CurrentState.IsLoggedIn);

            islicenced = _reduxService.Store
                .DistinctUntilChanged(state => new { state.CurrentState.IsLicensed })
                .Select(d => d.CurrentState.IsLicensed);

            islicencekeyset = _reduxService.Store
                .DistinctUntilChanged(state => new { state.CurrentState.IsLicenceKeySet })
                .Select(d => d.CurrentState.IsLicenceKeySet);

            licencekeysuccessful = _reduxService.Store
                .DistinctUntilChanged(state => new { state.CurrentEventsState.LicenceKeySuccessful })
                .Select(d => d.CurrentEventsState.LicenceKeySuccessful);

            isnetworkdown = _reduxService.Store
                .DistinctUntilChanged(state => new { state.CurrentEventsState.IsNetworkDown })
                .Select(d => d.CurrentEventsState.IsNetworkDown);

            isnetworkbusy = _reduxService.Store
                .DistinctUntilChanged(state => new { state.CurrentEventsState.IsNetworkBusy })
                .Select(d => d.CurrentEventsState.IsNetworkBusy);

            showlicenced = isloggedin
                .CombineLatest(islicenced, (x, y) => x && y)
                .CombineLatest(licencekeysuccessful, (x, y) => x && y == null)
                .CombineLatest(isnetworkdown, (x, y) => x && !y);

            shownotlicenced = isloggedin
                .CombineLatest(islicencekeyset, (x, y) => x && !y)
                .CombineLatest(islicenced, (x, y) => x && !y)
                .CombineLatest(licencekeysuccessful, (x, y) => x && y == null)
                .CombineLatest(isnetworkdown, (x, y) => x && !y);

            showlicencekeysuccessful = isloggedin
                .CombineLatest(islicenced, (x, y) => x && y)
                .CombineLatest(licencekeysuccessful, (x, y) => x && y != null && (bool) y)
                .CombineLatest(isnetworkdown, (x, y) => x && !y);

            showlicencekeynotsuccessful = isloggedin
                .CombineLatest(islicenced, (x, y) => x && !y)
                .CombineLatest(licencekeysuccessful, (x, y) => x && y != null && (bool) !y)
                .CombineLatest(isnetworkdown, (x, y) => x && !y);

            shownetworkdown = isnetworkdown;

            enablesetlicencekeybutton = islicenced
                .CombineLatest(isnetworkbusy, (x, y) => !x && !y)
                .CombineLatest(isnetworkdown, (x, y) => x && !y);
            
        }

        protected override void setupSubscriptions()
        {
            showlicenced
                .DistinctUntilChanged()
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(x => {
                    ShowLicencedMessage = x;
                })
                .DisposeWith(subscriptionDisposibles);


            shownotlicenced
                .DistinctUntilChanged()
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(x => {
                    ShowNotLicencedMessage = x;
                })
                .DisposeWith(subscriptionDisposibles);


            showlicencekeysuccessful
                .DistinctUntilChanged()
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(x => {
                    ShowLicenceKeySuccessfulMessage = x;
                })
                .DisposeWith(subscriptionDisposibles);


            showlicencekeynotsuccessful
                .DistinctUntilChanged()
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(x => {
                    ShowLicenceKeyNotSuccessfulMessage = x;
                })
                .DisposeWith(subscriptionDisposibles);


            shownetworkdown
                .DistinctUntilChanged()
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(x => {
                    ShowNetworkDownMessage = x;
                })
                .DisposeWith(subscriptionDisposibles);


            enablesetlicencekeybutton
                .DistinctUntilChanged()
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(x => {
                    EnableSetLicenceKeyButton = x;
                })
                .DisposeWith(subscriptionDisposibles);


            isnetworkbusy
                .DistinctUntilChanged()
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(x => {
                    ShowActivityIndicator = x;
                })
                .DisposeWith(subscriptionDisposibles);

        }

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            _reduxService.Store.Dispatch(new ClearLicenceKeySuccessfulAction());

            setupObservables();

            setupSubscriptions();
        }
    }
}
