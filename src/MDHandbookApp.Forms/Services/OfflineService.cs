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
using System.Collections.Immutable;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Akavache;
using MDHandbookAppService.Common.Models.RequestMessages;
using MDHandbookApp.Forms.States;
using Splat;

namespace MDHandbookApp.Forms.Services
{
    public class OfflineService : IOfflineService
    {
        public OfflineService()
        {
            BlobCache.ApplicationName = Constants.AkavacheApplicationName;
        }


        public ImmutableList<AppLogItemMessage> LoadOfflineLogStore()
        {
            return BlobCache.UserAccount.GetObject<ImmutableList<AppLogItemMessage>>("logstore").Catch(Observable.Return(ImmutableList<AppLogItemMessage>.Empty)).Wait();
        }

        public void SaveLogStore(ImmutableList<AppLogItemMessage> logstores)
        {
            BlobCache.UserAccount.InsertObject("logstore", logstores).Wait();
        }

        public AppState LoadOfflineAppState()
        {
            var initialCurrentState = BlobCache.UserAccount.GetObject<HandbookState>("currentstate").Catch(Observable.Return(AppState.CreateEmptyHandbookState())).Wait();
            var initialBooks = BlobCache.UserAccount.GetObject<ImmutableDictionary<string, Book>>("books").Catch(Observable.Return(AppState.CreateEmptyBooks())).Wait();
            var initialFullpages = BlobCache.UserAccount.GetObject<ImmutableDictionary<string, Fullpage>>("fullpages").Catch(Observable.Return(AppState.CreateEmptyFullpages())).Wait();
            var initialCurrentPostUpdateState = BlobCache.UserAccount.GetObject<PostUpdateState>("currentpostupdatestate").Catch(Observable.Return(AppState.CreateEmptyPostUpdateState())).Wait();

            initialCurrentState.HasLicensedError = false;
            initialCurrentState.HasUnauthorizedError = false;
            initialCurrentState.IsNetworkBusy = false;
            
            var initialState = new AppState {
                Books = initialBooks,
                Fullpages = initialFullpages,
                CurrentPostUpdateState = initialCurrentPostUpdateState,
                CurrentState = initialCurrentState
            };

            return initialState;
        }

        public async Task SaveAppState(AppState state)
        {
            var currentHandbookState = state.CurrentState.Clone();
            var currentPostUpdateState = state.CurrentPostUpdateState.Clone();
            var books = state.Books;
            var fullpages = state.Fullpages;

            await Task.Run(() => {
                BlobCache.UserAccount.InsertObject("currentpostupdatestate", currentPostUpdateState).Wait();
                BlobCache.UserAccount.InsertObject("currentstate", currentHandbookState).Wait();
                BlobCache.UserAccount.InsertObject("books", books).Wait();
                BlobCache.UserAccount.InsertObject("fullpages", fullpages).Wait();
                BlobCache.UserAccount.Flush().Wait();
            });
        }
    }
}
