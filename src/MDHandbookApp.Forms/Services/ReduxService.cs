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
using MDHandbookApp.Forms.Reducers;
using MDHandbookApp.Forms.States;


namespace MDHandbookApp.Forms.Services
{
    class ReduxService : IReduxService
    {
        public IStore<AppState> Store { get; private set; }

        public ReduxService(IApplicationReducers _applicationReducers)
        {
            var initialAppState = AppState.CreateEmpty();

            Store = new Redux.Store<AppState>(reducer: _applicationReducers.ReduceApplication, initialState: initialAppState);
        }
    }
}
