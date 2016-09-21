﻿//
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

using MDHandbookApp.Forms.Actions;
using MDHandbookApp.Forms.Reducers;
using MDHandbookApp.Forms.Services;
using MDHandbookApp.Forms.Views;
using Microsoft.Practices.Unity;
using Prism.Navigation;
using Prism.Unity;


namespace MDHandbookApp.Forms
{
    public class AppBootstrapper : IAppBootstrapper
    {
        public void OnInitializedNavigation(INavigationService _nav)
        {
            _nav.NavigateAsync(ViewModels.Constants.MainPageAbsUrl, animated: false);
        }

        public void RegisterTypes(IUnityContainer _container)
        {
            _container.RegisterTypeForNavigation<AboutPage>();
            _container.RegisterTypeForNavigation<BookpagePage>();
            _container.RegisterTypeForNavigation<LicenceErrorPage>();
            _container.RegisterTypeForNavigation<LoginPage>();
            _container.RegisterTypeForNavigation<MainPage>();
            _container.RegisterTypeForNavigation<MenuPage>();
            _container.RegisterTypeForNavigation<NavPage>();
            _container.RegisterTypeForNavigation<OptionsPage>();
            _container.RegisterTypeForNavigation<PrivacyPage>();
            _container.RegisterTypeForNavigation<SetLicenceKeyPage>();
            _container.RegisterTypeForNavigation<UnauthorizedErrorPage>();
        }

        public void InitializeMDHandbookServices(IUnityContainer _container)
        {
            // Order is very important
            _container.RegisterType<ILogService, FullDebugLogService>(new ContainerControlledLifetimeManager());

            _container.RegisterType<IBookReducers, BookReducers>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IFullpageReducers, FullpageReducers>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IHandbookStateReducers, HandbookStateReducers>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IPostUpdateStateReducers, PostUpdateStateReducers>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IApplicationReducers, ApplicationReducers>(new ContainerControlledLifetimeManager());

            _container.RegisterType<IReduxService, ReduxService>(new ContainerControlledLifetimeManager());

            _container.RegisterType<IMobileService, AzureMobileService>(new ContainerControlledLifetimeManager());

            _container.RegisterType<IServerActionCreators, ServerActionCreators>(new ContainerControlledLifetimeManager());
        }
    }
}
