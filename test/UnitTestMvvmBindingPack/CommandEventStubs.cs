// 
//  MVVM-WPF-NetCore Markup Binding Extensions
//  Copyright © 2013-2021 Alexander Paskhin /paskhin@hotmail.co.uk/ All rights reserved.
// 
// Licensed under the Apache License, Version 2.0 (the "License"); you
// may not use this file except in compliance with the License. You may
// obtain a copy of the License at  http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or
// implied. See the License for the specific language governing permissions
// and limitations under the License.

using System;
using System.Windows;
using MvvmBindingPack;


namespace UnitTestMvvmBindingPack
{
    public class CommandEventStubs:NotifyPropertyChangedBase
    {

        bool _buttonClickExternalCalled;
        public bool ButtonClickExternalCalled
        {
            get { return _buttonClickExternalCalled; }
            set { _buttonClickExternalCalled = value; }
        }

        bool _executeButtonClickExternalCalled;
        public bool ExecuteButtonClickExternalCalled
        {
            get { return _executeButtonClickExternalCalled; }
            set { _executeButtonClickExternalCalled = value; }
        }

        bool _canExecuteButtonClickExternal = true;
        public bool CanExecuteButtonClickExternalFlag
        {
            get { return _canExecuteButtonClickExternal; }
            set { _canExecuteButtonClickExternal = value; }
        }

        bool _canExecuteFlag = false;
        public bool CanExecuteFlag
        {
            get { return _canExecuteFlag; }
            set
            {
                _canExecuteFlag = value;
                NotifyPropertyChanged();
            }
        }

        public void ResetTestFlags()
        {
            _buttonClickExternalCalled = false;
            _executeButtonClickExternalCalled = false;
            _canExecuteButtonClickExternal = true;
        }

        public void Button_Click_External(object sender, RoutedEventArgs e)
        {
            _buttonClickExternalCalled = true;
        }

        public RoutedEventHandler ClickDelegate
        {
            get { return Button_Click_External; }
        }

        public void ExecuteButton_Click_External(object sender)
        {
            _executeButtonClickExternalCalled = true;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        public event Action IssueNotifyExecuteButtonChanged;

        public bool IsNullIssueNotifyExecuteButtonChanged
        {
            get { return IssueNotifyExecuteButtonChanged == null; }
        }

        public void InvokeNotifyChanged()
        {
            IssueNotifyExecuteButtonChanged();
        }

        public bool CanExecuteButton_Click_External(object sender)
        {
            return _canExecuteButtonClickExternal;
        }

    }
}
