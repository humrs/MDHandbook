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
using Redux;
using MDHandbookApp.Forms.Actions;
using MDHandbookApp.Forms.Services;
using MDHandbookApp.Forms.States;


namespace MDHandbookApp.Forms.Reducers
{
    public class HandbookStateReducers : IHandbookStateReducers
    {
        private ILogService _logService;

        public HandbookStateReducers(ILogService logService)
        {
            _logService = logService;
        }

        public HandbookState HandbookStateReducer(HandbookState previousState, IAction action)
        { 
            if (action is LoginAction)
            {
                return loginReducer(previousState, (LoginAction) action);
            }

            if (action is LogoutAction)
            {
                return logoutReducer(previousState, (LogoutAction) action);
            }

            if (action is SetCheckingLoginAction)
            {
                return setCheckingLoginReducer(previousState, (SetCheckingLoginAction) action);
            }

            if (action is SetLicenceKeyAction)
            {
                return setLicenceKeyReducer(previousState, (SetLicenceKeyAction) action);
            }

            if (action is ClearLicenceKeyAction)
            {
                return clearLicenceKeyReducer(previousState, (ClearLicenceKeyAction) action);
            }

            if (action is VerifyingLicenceKeyAction)
            {
                return checkingLicenceKeyReducer(previousState, (VerifyingLicenceKeyAction) action);
            }

            if (action is ClearVerifyingLicenceKeyAction)
            {
                return cancelCheckingLicenceKeyReducer(previousState, (ClearVerifyingLicenceKeyAction) action);
            }

            if (action is SetLicensedAction)
            {
                return setLicensedReducer(previousState, (SetLicensedAction) action);
            }

            if (action is ClearLicensedAction)
            {
                return clearLicensedReducer(previousState, (ClearLicensedAction) action);
            }


            if (action is SetUpdatingDataAction)
            {
                return setUpdatingDataReducer(previousState, (SetUpdatingDataAction) action);
            }

            if (action is ClearUpdatingDataAction)
            {
                return clearUpdatingDataReducer(previousState, (ClearUpdatingDataAction) action);
            }

            if (action is SetLastUpdateTimeAction)
            {
                return setLastUpdateTimeReducer(previousState, (SetLastUpdateTimeAction) action);
            }


            if (action is SetPostingUpdateDataAction)
            {
                return setPostingUpdateDataReducer(previousState, (SetPostingUpdateDataAction) action);
            }

            if (action is ClearPostingUpdateDataAction)
            {
                return clearPostingUpdateDataReducer(previousState, (ClearPostingUpdateDataAction) action);
            }


            if (action is SetRefreshingTokenAction)
            {
                return setRefreshingTokenReducer(previousState, (SetRefreshingTokenAction) action);
            }

            if (action is ClearRefreshingTokenAction)
            {
                return clearRefreshingTokenReducer(previousState, (ClearRefreshingTokenAction) action);
            }

            if (action is SetRefreshTokenAction)
            {
                return setRefreshTokenReducer(previousState, (SetRefreshTokenAction) action);
            }


            if (action is SetLoadingAppLogAction)
            {
                return setLoadingAppLogReducer(previousState, (SetLoadingAppLogAction) action);
            }

            if (action is ClearLoadingAppLogAction)
            {
                return clearLoadingAppLogReducer(previousState, (ClearLoadingAppLogAction) action);
            }

            if (action is SetHasLicensedErrorAction)
            {
                return setHasLicensedErrorReducer(previousState, (SetHasLicensedErrorAction) action);
            }

            if (action is ClearHasLicensedErrorAction)
            {
                return clearHasLicensedErrorReducer(previousState, (ClearHasLicensedErrorAction) action);
            }

            if (action is SetHasUnauthorizedErrorAction)
            {
                return setHasUnauthorizedErrorReducer(previousState, (SetHasUnauthorizedErrorAction)action);
            }

            if (action is ClearHasUnauthorizedErrorAction)
            {
                return clearHasUnauthorizedErrorReducer(previousState, (ClearHasUnauthorizedErrorAction)action);
            }

            if (action is SetNeedsUpdateAction)
            {
                return setNeedsUpdateReducer(previousState, (SetNeedsUpdateAction) action);
            }

            if (action is SetResettingUpdatesAction)
            {
                return setResettingUpdatesReducer(previousState, (SetResettingUpdatesAction) action);
            }

            if (action is ClearResettingUpdatesAction)
            {
                return clearResettingUpdatesReducer(previousState, (ClearResettingUpdatesAction) action);
            }

            if (action is SetReloadedAction)
            {
                return setReloadedReducer(previousState, (SetReloadedAction) action);
            }

            if (action is ClearReloadedAction)
            {
                return clearReloadedReducer(previousState, (ClearReloadedAction) action);
            }

            return previousState;
        }

        private HandbookState clearResettingUpdatesReducer(HandbookState previousState, ClearResettingUpdatesAction action)
        {
            _logService.Info("ClearResettingUpdatesReducer");
            HandbookState newState = previousState.Clone();
            newState.ResettingUpdates = false;
            return newState;
        }

        private HandbookState setResettingUpdatesReducer(HandbookState previousState, SetResettingUpdatesAction action)
        {
            _logService.Info("SetResettingUpdatesReducer");
            HandbookState newState = previousState.Clone();
            newState.ResettingUpdates = true;
            return newState;
        }

        private HandbookState clearReloadedReducer(HandbookState previousState, ClearReloadedAction action)
        {
            _logService.Info("ClearReloadedReducer");
            HandbookState newState = previousState.Clone();
            newState.Reloaded = false;
            return newState;
        }

        private HandbookState setReloadedReducer(HandbookState previousState, SetReloadedAction action)
        {
            _logService.Info("SetReloadReducer");
            HandbookState newState = previousState.Clone();
            newState.Reloaded = true;
            return newState;
        }

        private HandbookState setNeedsUpdateReducer(HandbookState previousState, SetNeedsUpdateAction action)
        {
            _logService.Info("SetNeedsUpdateReducer");
            HandbookState newState = previousState.Clone();
            newState.IsDataUpdated = false;
            return newState;
        }

        private HandbookState clearHasUnauthorizedErrorReducer(HandbookState previousState, ClearHasUnauthorizedErrorAction action)
        {
            _logService.Info("ClearHasUnauthorizedErrorReducer");
            HandbookState newState = previousState.Clone();
            newState.HasUnauthorizedError = false;
            return newState;
        }

        private HandbookState setHasUnauthorizedErrorReducer(HandbookState previousState, SetHasUnauthorizedErrorAction action)
        {
            _logService.Info("SetHasUnauthorizedErrorReducer");
            HandbookState newState = previousState.Clone();
            newState.HasUnauthorizedError = true;
            return newState;
        }

        private HandbookState clearHasLicensedErrorReducer(HandbookState previousState, ClearHasLicensedErrorAction action)
        {
            _logService.Info("ClearHasLicensedErrorReducer");
            HandbookState newState = previousState.Clone();
            newState.HasLicensedError = false;
            return newState;
        }

        private HandbookState setHasLicensedErrorReducer(HandbookState previousState, SetHasLicensedErrorAction action)
        {
            _logService.Info("SetHasLicensedErrorReducer");
            HandbookState newState = previousState.Clone();
            newState.HasLicensedError = true;
            return newState;
        }


        private HandbookState clearLoadingAppLogReducer(HandbookState previousState, ClearLoadingAppLogAction action)
        {
            _logService.Info("ClearLoadingAppLogReducer");
            HandbookState newState = previousState.Clone();
            newState.LoadingAppLog = false;
            return newState;
        }

        private HandbookState setLoadingAppLogReducer(HandbookState previousState, SetLoadingAppLogAction action)
        {
            _logService.Info("SetLoadingAppLogReducer");
            HandbookState newState = previousState.Clone();
            newState.LoadingAppLog = true;
            return newState;
        }

        private HandbookState setRefreshTokenReducer(HandbookState previousState, SetRefreshTokenAction action)
        {
            _logService.Info("SetRefreshTokenReducer");
            HandbookState newState = previousState.Clone();
            newState.AuthToken = action.Token;
            return newState;
        }

        private HandbookState clearRefreshingTokenReducer(HandbookState previousState, ClearRefreshingTokenAction action)
        {
            _logService.Info("ClearRefreshingTokenReducer");
            HandbookState newState = previousState.Clone();
            newState.RefreshingToken = false;
            return newState;
        }

        private HandbookState setRefreshingTokenReducer(HandbookState previousState, SetRefreshingTokenAction action)
        {
            _logService.Info("SetRefreshingTokenReducer");
            HandbookState newState = previousState.Clone();
            newState.RefreshingToken = true;
            return newState;
        }

        private HandbookState clearPostingUpdateDataReducer(HandbookState previousState, ClearPostingUpdateDataAction action)
        {
            _logService.Info("ClearPostingUpdateDataReducer");
            HandbookState newState = previousState.Clone();
            newState.PostingUpdateData = false;
            return newState;
        }

        private HandbookState setPostingUpdateDataReducer(HandbookState previousState, SetPostingUpdateDataAction action)
        {
            _logService.Info("SetPostingUpdateDataReducer");
            HandbookState newState = previousState.Clone();
            newState.PostingUpdateData = true;
            return newState;
        }

        private HandbookState setLastUpdateTimeReducer(HandbookState previousState, SetLastUpdateTimeAction action)
        {
            _logService.Info(string.Format("SetLastUpdateTimeReducer: {0}", action.UpdateTime.ToString("O")));
            HandbookState newState = previousState.Clone();
            newState.UpdatingData = false;
            newState.IsDataUpdated = true;
            newState.LastUpdateTime = action.UpdateTime;
            return newState;
        }

        private HandbookState clearUpdatingDataReducer(HandbookState previousState, ClearUpdatingDataAction action)
        {
            _logService.Info("ClearUpdatingDataReducer");
            HandbookState newState = previousState.Clone();
            newState.UpdatingData = false;
            return newState;
        }

        private HandbookState setUpdatingDataReducer(HandbookState previousState, SetUpdatingDataAction action)
        {
            _logService.Info("SetUpdatingDataReducer");
            HandbookState newState = previousState.Clone();
            newState.UpdatingData = true;
            return newState;
        }


        private HandbookState clearLicensedReducer(HandbookState previousState, ClearLicensedAction action)
        {
            _logService.Info("ClearLicensedReducer");
            HandbookState newState = previousState.Clone();
            newState.IsLicensed = false;
            return newState;
        }

        private HandbookState setLicensedReducer(HandbookState previousState, SetLicensedAction action)
        {
            _logService.Info("SetLicensedReducer");
            HandbookState newState = previousState.Clone();
            newState.VerifyingLicenceKey = false;
            newState.IsLicensed = true;
            return newState;
        }


        private HandbookState cancelCheckingLicenceKeyReducer(HandbookState previousState, ClearVerifyingLicenceKeyAction action)
        {
            _logService.Info("CancelCheckingLicenceKeyReducer");
            HandbookState newState = previousState.Clone();
            newState.VerifyingLicenceKey = false;
            return newState;
        }

        private HandbookState checkingLicenceKeyReducer(HandbookState previousState, VerifyingLicenceKeyAction action)
        {
            _logService.Info("CheckingLicenceKeyReducer");
            HandbookState newState = previousState.Clone();
            newState.VerifyingLicenceKey = true;
            return newState;
        }

        private HandbookState clearLicenceKeyReducer(HandbookState previousState, ClearLicenceKeyAction action)
        {
            _logService.Info("ClearLicenceKeyReducer");
            HandbookState newState = previousState.Clone();
            newState.LicenceKey = "";
            newState.IsLicenceKeySet = false;
            newState.IsLicensed = false;
            newState.VerifyingLicenceKey = false;
            return newState;
        }

        private HandbookState setLicenceKeyReducer(HandbookState previousState, SetLicenceKeyAction action)
        {
            _logService.Info("SetLicenceKeyReducer");
            HandbookState newState = previousState.Clone();
            newState.LicenceKey = "";
            if (!String.IsNullOrEmpty(action.LicenceKey))
            {
                char[] buffer = new char[action.LicenceKey.Length];
                action.LicenceKey.CopyTo(0, buffer, 0, action.LicenceKey.Length);
                newState.LicenceKey = new string(buffer);
            }
            newState.IsLicenceKeySet = true;
            return newState;
        }


        private HandbookState setCheckingLoginReducer(HandbookState previousState, SetCheckingLoginAction action)
        {
            _logService.Info("SetCheckingLoginReducer");
            HandbookState newState = previousState.Clone();
            newState.CheckingLogin = true;
            return newState;
        }

        private HandbookState logoutReducer(HandbookState previousState, LogoutAction action)
        {
            _logService.Info("LogoutReducer");
            HandbookState newState = previousState.Clone();
            newState.UserId = "";
            newState.AuthToken = "";

            newState.IsUserSet = false;
            newState.IsLoggedIn = false;
            newState.CheckingLogin = false;

            newState.IsDataUpdated = false;
            newState.UpdatingData = false;

            newState.IsLicensed = false;
            newState.VerifyingLicenceKey = false;
            return newState;
        }

        private HandbookState loginReducer(HandbookState previousState, LoginAction action)
        {
            _logService.Info("LoginReducer");
            HandbookState newState = previousState.Clone();
            newState.UserId = "";
            if (!String.IsNullOrEmpty(action.UserId))
            {
                char[] buffer = new char[action.UserId.Length];
                action.UserId.CopyTo(0, buffer, 0, action.UserId.Length);
                newState.UserId = new string(buffer);
            }
            newState.AuthToken = "";
            if (!String.IsNullOrEmpty(action.AuthToken))
            {
                char[] buffer = new char[action.AuthToken.Length];
                action.AuthToken.CopyTo(0, buffer, 0, action.AuthToken.Length);
                newState.AuthToken = new string(buffer);
            }

            newState.CheckingLogin = false;
            newState.IsUserSet = true;
            newState.IsLoggedIn = true;

            newState.IsDataUpdated = false;
            newState.UpdatingData = false;

            newState.IsLicensed = false;
            newState.VerifyingLicenceKey = false;
            return newState;
        }
    }
}
