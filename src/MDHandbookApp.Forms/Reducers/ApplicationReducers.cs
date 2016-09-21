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

using Redux;
using MDHandbookApp.Forms.States;


namespace MDHandbookApp.Forms.Reducers
{
    public class ApplicationReducers : IApplicationReducers
    {
        private IBookReducers            _bookReducers;
        private IFullpageReducers        _fullpageReducers;
        private IHandbookStateReducers   _handbookStateReducers;
        private IPostUpdateStateReducers _postUpdateStateReducers;

        public ApplicationReducers(
            IBookReducers bookReducers,
            IFullpageReducers fullpageReducers,
            IHandbookStateReducers handbookStateReducers,
            IPostUpdateStateReducers postUpdateStateReducers)
        {
            _bookReducers = bookReducers;
            _fullpageReducers = fullpageReducers;
            _handbookStateReducers = handbookStateReducers;
            _postUpdateStateReducers = postUpdateStateReducers;
        }


        public AppState ReduceApplication(AppState previousState, IAction action)
        {
            return new AppState {
                Books = _bookReducers.BookReducer(previousState.Books, action),
                Fullpages = _fullpageReducers.FullpageReducer(previousState.Fullpages, action),
                CurrentPostUpdateState = _postUpdateStateReducers.PostUpdateStateReducer(previousState.CurrentPostUpdateState, action),
                CurrentState = _handbookStateReducers.HandbookStateReducer(previousState.CurrentState, action)
            };
        }
    }
}
