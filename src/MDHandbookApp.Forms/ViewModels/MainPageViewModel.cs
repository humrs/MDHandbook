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
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using MDHandbookApp.Forms.Services;
using Prism.Commands;
using Prism.Navigation;
using ReactiveUI;


namespace MDHandbookApp.Forms.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public DelegateCommand NavigateToLoginPage { get; set; }
        public DelegateCommand NavigateToSetLicenceKeyPage { get; set; }

        private IObservable<bool> isloggedin;
        private IObservable<bool> islicenced;
        private IObservable<bool> isnetworkbusy;

        
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

        private string _updateTime = "";
        public string UpdateTime
        {
            get { return _updateTime; }
            set { SetProperty(ref _updateTime, value); }
        }

        private List<BookTileViewModel> _handbooks;
        public List<BookTileViewModel> Handbooks
        {
            get { return _handbooks; }
            set { SetProperty(ref _handbooks, value); }
        }
        
        public MainPageViewModel(
            ILogService logService,
            INavigationService navigationService,
            IReduxService reduxService) : base(logService, navigationService, reduxService)
        {
            Handbooks = new List<BookTileViewModel>();
            NavigateToLoginPage = DelegateCommand.FromAsyncHandler(navigateToLoginPage);
            NavigateToSetLicenceKeyPage = DelegateCommand.FromAsyncHandler(navigateToSetLicenceKeyPage);
        }

        private async Task navigateToSetLicenceKeyPage()
        {
            await _navigationService.NavigateAsync(Constants.SetLicenceKeyPageAbsUrl);
        }

        private async Task navigateToLoginPage()
        {
            await _navigationService.NavigateAsync(Constants.LoginPageAbsUrl);
        }


        protected override void setupObservables()
        {
            isloggedin = _reduxService.Store
                .DistinctUntilChanged(state => new { state.CurrentState.IsLoggedIn })
                .Select(d => d.CurrentState.IsLoggedIn);

            islicenced = _reduxService.Store
                .DistinctUntilChanged(state => new { state.CurrentState.IsLicensed })
                .Select(d => d.CurrentState.IsLicensed);

            isnetworkbusy = _reduxService.Store
                .DistinctUntilChanged(state => new { state.CurrentState.IsNetworkBusy })
                .Select(d => d.CurrentState.IsNetworkBusy);
        }

        protected override void setupSubscriptions()
        {
            _reduxService.Store
                .DistinctUntilChanged(state => new { state.CurrentState.LastUpdateTime })
                .Select(u => $"Last Updated: {u.CurrentState.LastUpdateTime.ToLocalTime()}")
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(
                    x => {
                        UpdateTime = x;
                    });

            _reduxService.Store
                .DistinctUntilChanged(state => new { state.Books })
                .Select(d => d.Books.Values.OrderBy(y => y.OrderIndex).ToList())
                .DistinctUntilChanged()
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(
                    xs => {
                        var newlist = new List<BookTileViewModel>();
                        newlist.AddRange(xs.Select(x => new BookTileViewModel(x, _logService, _navigationService)));
                        Handbooks.Clear();
                        Handbooks = newlist;
                    });

            isloggedin
                .DistinctUntilChanged()
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(
                    x => {
                        ShowNotLoggedInMessage = !x;
                    });

            isloggedin
                .CombineLatest(islicenced, (x, y) => x && !y)
                .DistinctUntilChanged()
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(
                    x => {
                        ShowNotLicencedMessage = x;
                    });

            isloggedin
                .CombineLatest(islicenced, (x, y) => x && y)
                .DistinctUntilChanged()
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(
                    x => {
                        ShowBookList = x;
                        ShowNeedLoginAndLicencedMessage = !x;
                    });

            isnetworkbusy
                .DistinctUntilChanged()
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(
                    x => {
                        ShowActivityIndicator = x;
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
