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
            _container.RegisterTypeForNavigation<MainPage>();
            _container.RegisterTypeForNavigation<NavPage>();
            _container.RegisterTypeForNavigation<MenuPage>();
        }

        public void InitializeMDHandbookServices(IUnityContainer _container)
        {
            
        }
    }
}
