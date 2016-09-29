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
                return loginReducer(previousState, (LoginAction) action);
            
            if (action is LogoutAction)
                return logoutReducer(previousState, (LogoutAction) action);
            
            if (action is SetLicenceKeyAction)
                return setLicenceKeyReducer(previousState, (SetLicenceKeyAction) action);
            
            if (action is ClearLicenceKeyAction)
                return clearLicenceKeyReducer(previousState, (ClearLicenceKeyAction) action);
            
            if (action is SetLicensedAction)
                return setLicensedReducer(previousState, (SetLicensedAction) action);
            
            if (action is ClearLicensedAction)
                return clearLicensedReducer(previousState, (ClearLicensedAction) action);
            
            if (action is SetIsDataUpdatedAction)
                return setIsDataUpdatedReducer(previousState, (SetIsDataUpdatedAction) action);

            if (action is ClearIsDataUpdatedAction)
                return clearIsDataUpdatedReducer(previousState, (ClearIsDataUpdatedAction) action);

            if (action is SetLastUpdateTimeAction)
                return setLastUpdateTimeReducer(previousState, (SetLastUpdateTimeAction) action);
            
            if (action is SetRefreshTokenAction)
                return setRefreshTokenReducer(previousState, (SetRefreshTokenAction) action);

            return previousState;
        }

        private HandbookState clearIsDataUpdatedReducer(HandbookState previousState, ClearIsDataUpdatedAction action)
        {
            _logService.Info("ClearIsDataUpdatedReducer");
            HandbookState newState = previousState.Clone();
            newState.IsDataUpdated = false;
            return newState;
        }

        private HandbookState setIsDataUpdatedReducer(HandbookState previousState, SetIsDataUpdatedAction action)
        {
            _logService.Info("SetIsDataUpdatedReducer");
            HandbookState newState = previousState.Clone();
            newState.IsDataUpdated = true;
            return newState;
        }


        
        private HandbookState setRefreshTokenReducer(HandbookState previousState, SetRefreshTokenAction action)
        {
            _logService.Info("SetRefreshTokenReducer");
            HandbookState newState = previousState.Clone();
            newState.AuthToken = action.Token;
            return newState;
        }

        

        
        private HandbookState setLastUpdateTimeReducer(HandbookState previousState, SetLastUpdateTimeAction action)
        {
            _logService.Info(string.Format("SetLastUpdateTimeReducer: {0}", action.UpdateTime.ToString("O")));
            HandbookState newState = previousState.Clone();
            newState.IsDataUpdated = true;
            newState.LastUpdateTime = action.UpdateTime;
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
            newState.IsLicensed = true;
            return newState;
        }


        

        private HandbookState clearLicenceKeyReducer(HandbookState previousState, ClearLicenceKeyAction action)
        {
            _logService.Info("ClearLicenceKeyReducer");
            HandbookState newState = previousState.Clone();
            newState.LicenceKey = "";
            newState.IsLicenceKeySet = false;
            newState.IsLicensed = false;
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


        private HandbookState logoutReducer(HandbookState previousState, LogoutAction action)
        {
            _logService.Info("LogoutReducer");
            HandbookState newState = previousState.Clone();
            newState.UserId = "";
            newState.AuthToken = "";

            newState.IsUserSet = false;
            newState.IsLoggedIn = false;
         
            newState.IsDataUpdated = false;
         
            newState.IsLicensed = false;
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

            newState.IsUserSet = true;
            newState.IsLoggedIn = true;

            newState.IsDataUpdated = false;
        
            newState.IsLicensed = false;
            return newState;
        }
    }
}
