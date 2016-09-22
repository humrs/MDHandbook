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
