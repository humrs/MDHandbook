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

using System.Linq;
using Newtonsoft.Json;
using Redux;
using MDHandbookApp.Forms.Actions;
using MDHandbookAppService.Common.Models.Utility;
using MDHandbookApp.Forms.Services;
using MDHandbookApp.Forms.States;

namespace MDHandbookApp.Forms.Reducers
{
    public class PostUpdateStateReducers : IPostUpdateStateReducers
    {
        private ILogService _logService;

        public PostUpdateStateReducers(ILogService logService)
        {
            _logService = logService;
        }

        public PostUpdateState PostUpdateStateReducer(PostUpdateState previousState, IAction action)
        {
            if (action is AddPostUpdateAddBookIdsRangeAction)
            {
                return addPostUpdateAddBookIdsRangeReducer(previousState, (AddPostUpdateAddBookIdsRangeAction) action);
            }

            if (action is DeletePostUpdateAddBookIdsRangeAction)
            {
                return deletePostUpdateAddBookIdsRangeReducer(previousState, (DeletePostUpdateAddBookIdsRangeAction) action);
            }

            if (action is AddPostUpdateDeleteBookIdsRangeAction)
            {
                return addPostUpdateDeleteBookIdsRangeReducer(previousState, (AddPostUpdateDeleteBookIdsRangeAction)action);
            }

            if (action is DeletePostUpdateDeleteBookIdsRangeAction)
            {
                return deletePostUpdateDeleteBookIdsRangeReducer(previousState, (DeletePostUpdateDeleteBookIdsRangeAction)action);
            }


            if (action is AddPostUpdateAddFullpageIdsRangeAction)
            {
                return addPostUpdateAddFullpageIdsRangeReducer(previousState, (AddPostUpdateAddFullpageIdsRangeAction)action);
            }

            if (action is DeletePostUpdateAddFullpageIdsRangeAction)
            {
                return deletePostUpdateAddFullpageIdsRangeReducer(previousState, (DeletePostUpdateAddFullpageIdsRangeAction)action);
            }

            if (action is AddPostUpdateDeleteFullpageIdsRangeAction)
            {
                return addPostUpdateDeleteFullpageIdsRangeReducer(previousState, (AddPostUpdateDeleteFullpageIdsRangeAction)action);
            }

            if (action is DeletePostUpdateDeleteFullpageIdsRangeAction)
            {
                return deletePostUpdateDeleteFullpageIdsRangeReducer(previousState, (DeletePostUpdateDeleteFullpageIdsRangeAction)action);
            }


            if (action is RemoveLocalPostUpdatesDataAction)
            {
                return RemoveLocalPostUpdatesDataReducer(previousState, (RemoveLocalPostUpdatesDataAction)action);
            }

            return previousState;
        }

        private PostUpdateState RemoveLocalPostUpdatesDataReducer(PostUpdateState previousState, RemoveLocalPostUpdatesDataAction action)
        {
            var ujm = new UpdateJsonMessage {
                AddBookItemIds = previousState.AddedBookIds.ToList(),
                DeleteBookItemIds = previousState.DeletedBooksIds.ToList(),
                AddFullpageItemIds = previousState.AddedFullpagesIds.ToList(),
                DeleteFullpageItemIds = previousState.DeletedFullpagesIds.ToList()
            };
            _logService.Info($"{JsonConvert.SerializeObject(ujm)}");

            PostUpdateState newState = previousState.Clone();
            newState.AddedBookIds = previousState.AddedBookIds.RemoveRange(action.Data.AddBookItemIds);
            newState.DeletedBooksIds = previousState.DeletedBooksIds.RemoveRange(action.Data.DeleteBookItemIds);
            newState.AddedFullpagesIds = previousState.AddedFullpagesIds.RemoveRange(action.Data.AddFullpageItemIds);
            newState.DeletedFullpagesIds = previousState.DeletedFullpagesIds.RemoveRange(action.Data.DeleteFullpageItemIds);
            return newState;
        }

        private PostUpdateState deletePostUpdateDeleteFullpageIdsRangeReducer(PostUpdateState previousState, DeletePostUpdateDeleteFullpageIdsRangeAction action)
        {
            _logService.Info($"{JsonConvert.SerializeObject(action.FullpageIds)}");
            PostUpdateState newState = previousState.Clone();
            newState.DeletedFullpagesIds = previousState.DeletedFullpagesIds.RemoveRange(action.FullpageIds);
            return newState;
        }

        private PostUpdateState addPostUpdateDeleteFullpageIdsRangeReducer(PostUpdateState previousState, AddPostUpdateDeleteFullpageIdsRangeAction action)
        {
            _logService.Info($"{JsonConvert.SerializeObject(action.FullpageIds)}");
            PostUpdateState newState = previousState.Clone();
            newState.DeletedFullpagesIds = previousState.DeletedFullpagesIds.AddRange(action.FullpageIds);
            return newState;
        }

        private PostUpdateState deletePostUpdateAddFullpageIdsRangeReducer(PostUpdateState previousState, DeletePostUpdateAddFullpageIdsRangeAction action)
        {
            _logService.Info($"{JsonConvert.SerializeObject(action.FullpageIds)}");
            PostUpdateState newState = previousState.Clone();
            newState.AddedFullpagesIds = previousState.AddedFullpagesIds.RemoveRange(action.FullpageIds);
            return newState;
        }

        private PostUpdateState addPostUpdateAddFullpageIdsRangeReducer(PostUpdateState previousState, AddPostUpdateAddFullpageIdsRangeAction action)
        {
            _logService.Info($"{JsonConvert.SerializeObject(action.FullpageIds)}");
            PostUpdateState newState = previousState.Clone();
            newState.AddedFullpagesIds = previousState.AddedFullpagesIds.AddRange(action.FullpageIds);
            return newState;
        }

        private PostUpdateState deletePostUpdateDeleteBookIdsRangeReducer(PostUpdateState previousState, DeletePostUpdateDeleteBookIdsRangeAction action)
        {
            _logService.Info($"{JsonConvert.SerializeObject(action.BookIds)}");
            PostUpdateState newState = previousState.Clone();
            newState.DeletedBooksIds = previousState.DeletedBooksIds.RemoveRange(action.BookIds);
            return newState;
        }

        private PostUpdateState addPostUpdateDeleteBookIdsRangeReducer(PostUpdateState previousState, AddPostUpdateDeleteBookIdsRangeAction action)
        {
            _logService.Info($"{JsonConvert.SerializeObject(action.BookIds)}");
            PostUpdateState newState = previousState.Clone();
            newState.DeletedBooksIds = previousState.DeletedBooksIds.AddRange(action.BookIds);
            return newState;
        }

        private PostUpdateState deletePostUpdateAddBookIdsRangeReducer(PostUpdateState previousState, DeletePostUpdateAddBookIdsRangeAction action)
        {
            _logService.Info($"{JsonConvert.SerializeObject(action.BookIds)}");
            PostUpdateState newState = previousState.Clone();
            newState.AddedBookIds = previousState.AddedBookIds.RemoveRange(action.BookIds);
            return newState;
        }

        private PostUpdateState addPostUpdateAddBookIdsRangeReducer(PostUpdateState previousState, AddPostUpdateAddBookIdsRangeAction action)
        {
            _logService.Info($"{JsonConvert.SerializeObject(action.BookIds)}");
            PostUpdateState newState = previousState.Clone();
            newState.AddedBookIds = previousState.AddedBookIds.AddRange(action.BookIds);
            return newState;
        }
    }
}
