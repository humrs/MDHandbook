using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            _nav.NavigateAsync("/MenuPage/NavPage/MainPage", animated: false);
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
            
        }
    }
}
