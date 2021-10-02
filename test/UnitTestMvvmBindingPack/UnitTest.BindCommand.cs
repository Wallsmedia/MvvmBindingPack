// 
//  MVVM-WPF-NetCore Markup Binding Extensions
//  Copyright © 2013-2021 Alexander Paskhin /paskhin@hotmail.co.uk/ All rights reserved.
// 
//
// Licensed under the Apache License, Version 2.0 (the "License"); you
// may not use this file except in compliance with the License. You may
// obtain a copy of the License at  http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or
// implied. See the License for the specific language governing permissions
// and limitations under the License.

using Xunit;

using System.Windows.Markup;
using MvvmBindingPack;
using System.Windows.Input;
using System.Windows;

namespace UnitTestMvvmBindingPack
{
    public class MockServiceProvider : IServiceProvider, IProvideValueTarget
    {

        public static MockServiceProvider Instance => new MockServiceProvider();
        public object TargetObject => new object();

        public event RoutedEventHandler fakeTargetHandler;
        public object TargetProperty => GetType().GetProperty("fakeTargetHandler");

        public object GetService(Type serviceType)
        {
            if (serviceType == typeof(IProvideValueTarget))
            {
                return this;
            }
            return null;
        }
    }

    /// <summary>
    /// Summary description for BindCommand
    /// </summary>
    public class UnitTestBindCommand
    {


        [Fact]
        public void BindEventHandlerConstructor()
        {
            const string source = "Source";
            var bindCommand = new BindCommand(source);
            Assert.Equal(source, bindCommand.Source);
        }

        int _count;
        void EventHandlerCanExecuteChanged(object sender, EventArgs e)
        {
            _count++;
        }

        [Fact]
        public void BindCommandMethodExCanPropertyAction()
        {

            var stubServiceProvider = MockServiceProvider.Instance;

            var bindCommand = new BindCommand();
            var viewModel = new _TestBindCommand();
            bindCommand.Source = viewModel;
            bindCommand.ExecuteMethodName = "ExecuteMethod";
            bindCommand.CanExecuteMethodName = "CanExecuteMethod";
            bindCommand.PropertyActionCanExecuteChanged = "PropertyActionCanExecuteChanged";


            var bindResolve = bindCommand.ProvideValue(stubServiceProvider);
            var fakeTargetICommand = (ICommand)bindResolve;

            if (fakeTargetICommand != null) fakeTargetICommand.Execute(null);
            Assert.Equal(viewModel.ExecuteCalled, true);

            if (fakeTargetICommand != null) fakeTargetICommand.CanExecute(null);
            Assert.Equal(viewModel.CanExecuteCalled, true);

            if (fakeTargetICommand != null) fakeTargetICommand.CanExecuteChanged += EventHandlerCanExecuteChanged;

            _count = 0;

            // Execute notification
            viewModel.PropertyActionCanExecuteChanged();
            Assert.Equal(_count, 1);

        }


        [Fact]
        public void BindCommandPropertyExCanPropertyAction()
        {

            var stubServiceProvider = MockServiceProvider.Instance;

            var bindCommand = new BindCommand();
            var viewModel = new _TestBindCommand();
            bindCommand.Source = viewModel;
            bindCommand.ExecutePropertyName = "ExecutePropertyName";
            bindCommand.CanExecutePropertyName = "CanExecutePropertyName";
            bindCommand.PropertyActionCanExecuteChanged = "PropertyActionCanExecuteChanged";


            var bindResolve = bindCommand.ProvideValue(stubServiceProvider);
            var fakeTargetICommand = (ICommand)bindResolve;

            if (fakeTargetICommand != null) fakeTargetICommand.Execute(null);
            Assert.Equal(viewModel.ExecuteCalled, true);

            if (fakeTargetICommand != null) fakeTargetICommand.CanExecute(null);
            Assert.Equal(viewModel.CanExecuteCalled, true);

            if (fakeTargetICommand != null) fakeTargetICommand.CanExecuteChanged += EventHandlerCanExecuteChanged;

            _count = 0;

            // Execute notification
            viewModel.PropertyActionCanExecuteChanged();
            Assert.Equal(_count, 1);

        }

        [Fact]
        public void BindCommandPropertyExCanEventToInvoke()
        {

            var stubServiceProvider = MockServiceProvider.Instance;

            var bindCommand = new BindCommand();
            var viewModel = new _TestBindCommand();
            bindCommand.Source = viewModel;
            bindCommand.ExecutePropertyName = "ExecutePropertyName";
            bindCommand.CanExecutePropertyName = "CanExecutePropertyName";
            bindCommand.EventToInvokeCanExecuteChanged = "EventToInvokeCanExecuteChanged";


            var bindResolve = bindCommand.ProvideValue(stubServiceProvider);
            var fakeTargetICommand = (ICommand)bindResolve;

            if (fakeTargetICommand != null) fakeTargetICommand.Execute(null);
            Assert.Equal(viewModel.ExecuteCalled, true);

            if (fakeTargetICommand != null) fakeTargetICommand.CanExecute(null);
            Assert.Equal(viewModel.CanExecuteCalled, true);

            if (fakeTargetICommand != null) fakeTargetICommand.CanExecuteChanged += EventHandlerCanExecuteChanged;

            _count = 0;

            // Execute notification
            viewModel.ExecuteEventNotifyCanExecuteChanged();
            Assert.Equal(_count, 1);

        }

        [Fact]
        public void BindCommandMethodExCanEventToInvoke()
        {

            var stubServiceProvider = MockServiceProvider.Instance;

            var bindCommand = new BindCommand();
            var viewModel = new _TestBindCommand();
            bindCommand.Source = viewModel;
            bindCommand.ExecuteMethodName = "ExecuteMethod";
            bindCommand.CanExecuteMethodName = "CanExecuteMethod";
            bindCommand.EventToInvokeCanExecuteChanged = "EventToInvokeCanExecuteChanged";

            var bindResolve = bindCommand.ProvideValue(stubServiceProvider);
            var fakeTargetICommand = (ICommand)bindResolve;

            if (fakeTargetICommand != null) fakeTargetICommand.Execute(null);
            Assert.Equal(viewModel.ExecuteCalled, true);

            if (fakeTargetICommand != null) fakeTargetICommand.CanExecute(null);
            Assert.Equal(viewModel.CanExecuteCalled, true);

            if (fakeTargetICommand != null) fakeTargetICommand.CanExecuteChanged += EventHandlerCanExecuteChanged;

            _count = 0;

            // Execute notification
            viewModel.ExecuteEventNotifyCanExecuteChanged();
            Assert.Equal(_count, 1);

        }

        [Fact]
        public void BindCommandMethodExCanExecuteBooleanProperty()
        {

            var stubServiceProvider = MockServiceProvider.Instance;

            var bindCommand = new BindCommand();
            var viewModel = new _TestBindCommand();
            bindCommand.Source = viewModel;
            bindCommand.ExecuteMethodName = "ExecuteMethod";
            bindCommand.CanExecuteBooleanPropertyName = "CanExecuteBooleanPropertyName";

            var bindResolve = bindCommand.ProvideValue(stubServiceProvider);
            var fakeTargetICommand = (ICommand)bindResolve;

            if (fakeTargetICommand != null) fakeTargetICommand.Execute(null);
            Assert.Equal(viewModel.ExecuteCalled, true);

            if (fakeTargetICommand != null) fakeTargetICommand.CanExecute(null);
            Assert.Equal(viewModel.CanExecuteBooleanPropertyName, true);

            if (fakeTargetICommand != null) fakeTargetICommand.CanExecuteChanged += EventHandlerCanExecuteChanged;

            _count = 0;
            // Execute notification
            viewModel.CanExecuteBooleanPropertyName = false;
            Assert.Equal(_count, 1);

        }



        [Fact]
        public void ProvideValueFromSourceExceptions()
        {

            var stubServiceProvider = MockServiceProvider.Instance;

            var bindCommand = new BindCommand();
            var viewModel = new _TestBindCommand();
            bindCommand.Source = viewModel;

            bindCommand.ExecuteMethodName = "_ExecuteMethod";
            bindCommand.CanExecuteMethodName = "CanExecuteMethod";
            bindCommand.EventToInvokeCanExecuteChanged = "EventNotifyCanExecuteChanged";
            bindCommand.PropertyActionCanExecuteChanged = "DelegateNotifyCanExecuteChanged";
            UnitTestInternal.AssertThrows(typeof(ArgumentException), () => bindCommand.ProvideValue(stubServiceProvider),
                "ProvideValueExceptions - expected exception - ArgumentException");

            bindCommand.ExecuteMethodName = "ExecuteMethod";
            bindCommand.CanExecuteMethodName = "_CanExecuteMethod";
            bindCommand.EventToInvokeCanExecuteChanged = "EventNotifyCanExecuteChanged";
            bindCommand.PropertyActionCanExecuteChanged = "DelegateNotifyCanExecuteChanged";
            UnitTestInternal.AssertThrows(typeof(ArgumentException), () => bindCommand.ProvideValue(stubServiceProvider),
                "ProvideValueExceptions - expected exception - ArgumentException");

            bindCommand.ExecuteMethodName = "ExecuteMethod";
            bindCommand.CanExecuteMethodName = "CanExecuteMethod";
            bindCommand.EventToInvokeCanExecuteChanged = "_EventNotifyCanExecuteChanged";
            bindCommand.PropertyActionCanExecuteChanged = "DelegateNotifyCanExecuteChanged";
            UnitTestInternal.AssertThrows(typeof(ArgumentException), () => bindCommand.ProvideValue(stubServiceProvider),
                "ProvideValueExceptions - expected exception - ArgumentException");

            bindCommand.ExecuteMethodName = "ExecuteMethod";
            bindCommand.CanExecuteMethodName = "CanExecuteMethod";
            bindCommand.EventToInvokeCanExecuteChanged = "EventNotifyCanExecuteChanged";
            bindCommand.PropertyActionCanExecuteChanged = "_DelegateNotifyCanExecuteChanged";
            UnitTestInternal.AssertThrows(typeof(ArgumentException), () => bindCommand.ProvideValue(stubServiceProvider),
                "ProvideValueExceptions - expected exception - ArgumentException");
        }

        static List<_TestBindCommand> _testList = new List<_TestBindCommand>();
        class _TestBindCommand : NotifyPropertyChangedBase
        {
            public _TestBindCommand()
            {
                _testList.Add(this);
            }

            public void Reset()
            {
                ExecuteCalled = false;
                CanExecuteCalled = false;
            }

            public bool ExecuteCalled;
            public bool CanExecuteCalled;

            public Action<object> ExecutePropertyName
            {
                get { return ExecuteMethod; }
            }

            public void ExecuteMethod(object sender)
            {
                ExecuteCalled = true;
            }

            public bool CanExecuteMethod(object sender)
            {
                CanExecuteCalled = true;
                return false;
            }

            public Func<object, bool> CanExecutePropertyName
            {
                get { return CanExecuteMethod; }
            }

            private bool _canExecuteBooleanPropertyName = true;

            public bool CanExecuteBooleanPropertyName
            {
                get { return _canExecuteBooleanPropertyName; }
                set { _canExecuteBooleanPropertyName = value; NotifyPropertyChanged(); }
            }

            public event Action EventToInvokeCanExecuteChanged;

            public void ExecuteEventNotifyCanExecuteChanged()
            {
                if (EventToInvokeCanExecuteChanged != null)
                {
                    EventToInvokeCanExecuteChanged();
                }
            }

            public Action PropertyActionCanExecuteChanged { get; set; }
        }
    }

}
