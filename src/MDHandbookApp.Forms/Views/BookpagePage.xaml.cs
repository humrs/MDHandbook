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
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using MDHandbookApp.Forms.ViewModels;
using ReactiveUI;
using Xamarin.Forms;


namespace MDHandbookApp.Forms.Views
{
    public partial class BookpagePage : ContentPage
    {
        public BookpagePage()
        {
            InitializeComponent();
        }

        public void webOnNavigating(object s, WebNavigatingEventArgs e)
        {
            var _vm = (BookpagePageViewModel) this.BindingContext;
            _vm.WebOnNavigating(s, e);
        }

        public void webOnEndNavigating(object s, WebNavigatedEventArgs e)
        {
            var _vm = (BookpagePageViewModel) this.BindingContext;
            _vm.WebOnEndNavigating(s, e);
        }
    }
}
