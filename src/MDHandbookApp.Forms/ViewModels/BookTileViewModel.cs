using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MDHandbookApp.Forms.Services;
using MDHandbookApp.Forms.States;
using Prism.Commands;
using Prism.Navigation;

namespace MDHandbookApp.Forms.ViewModels
{
    public class BookTileViewModel : ViewModelBase
    {
        private Book _model;
        public Book Model
        {
            get { return _model; }
            set { SetProperty(ref _model, value); }
        }

        public DelegateCommand OpenThisBook { get; set; }

        public BookTileViewModel(Book model,
            ILogService logService,
            INavigationService navigationService) : base(logService, navigationService)
        {
            Model = model;
            OpenThisBook = DelegateCommand.FromAsyncHandler(openThisBook);
        }

        private async Task openThisBook()
        {
            var startingpage = Model.StartingBookpage;
            await _navigationService.NavigateAsync($"{Constants.BookpagePageAbsUrl}?{Constants.BookpagePageUrlParamId}={startingpage}");
        }
    }
}
