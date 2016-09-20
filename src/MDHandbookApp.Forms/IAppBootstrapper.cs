using Microsoft.Practices.Unity;
using Prism.Navigation;

namespace MDHandbookApp.Forms
{
    public interface IAppBootstrapper
    {
        void InitializeMDHandbookServices(IUnityContainer _container);
        void OnInitializedNavigation(INavigationService _nav);
        void RegisterTypes(IUnityContainer _container);
    }
}