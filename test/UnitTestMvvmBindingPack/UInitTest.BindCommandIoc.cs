// 
//  MVVM-WPF Markup Dependency Injection Binding Extensions
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
using MvvmBindingPack;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;

namespace UnitTestMvvmBindingPack;

/// <summary>
/// Summary description for BindCommandIoc
/// </summary>
public class UnitTestBindCommandIoc
{

    public UnitTestBindCommandIoc()
    {
        ServiceCollection services = new ServiceCollection();
        services.AddTransient<_TestBindCommandIoc>();
        AutoWireVmDataContext.ServiceProvider = services.BuildServiceProvider();
    }

    /// <summary>
    ///Gets or sets the test context which provides
    ///information about and functionality for the current test run.
    ///</summary>


    [Fact]
    public void BindEventHandlerConstructor()
    {
        Type serviceType = typeof(String);
        var bindCommand = new BindCommandIoc(serviceType);
        Assert.Equal(serviceType, bindCommand.ServiceType);
    }

    int _count;
    void EventHandlerCanExecuteChanged(object sender, EventArgs e)
    {
        _count++;
    }

    [Fact]
    public void BindCommandIocByType()
    {

        var stubServiceProvider = MockServiceProvider.Instance;

        // just use one of the test cases for BindCommandBase
        var bindCommand = new BindCommandIoc
        {
            ServiceType = typeof(_TestBindCommandIoc),
            ExecuteMethodName = "ExecuteMethod",
            CanExecuteMethodName = "CanExecuteMethod",
            EventToInvokeCanExecuteChanged = "EventHandlerNotifyCanExecuteChanged"
        };

        var bindResolve = bindCommand.ProvideValue(stubServiceProvider);
        var fakeTargetICommand = (ICommand)bindResolve;
        _TestBindCommandIoc _viewModel = _testList[_testList.Count - 1];

        if (fakeTargetICommand != null) fakeTargetICommand.Execute(null);
        Assert.Equal(_viewModel.ExecuteCalled, true);

        if (fakeTargetICommand != null) fakeTargetICommand.CanExecute(null);
        Assert.Equal(_viewModel.CanExecuteCalled, true);

        if (fakeTargetICommand != null) fakeTargetICommand.CanExecuteChanged += EventHandlerCanExecuteChanged;
        _count = 0;
        // Execute notification
        _viewModel.ExecuteEventNotifyCanExecuteChanged();
        Assert.Equal(_count, 1);

        // Execute notification
        if (_viewModel.DelegateNotifyCanExecuteChanged != null)
        {
            _viewModel.DelegateNotifyCanExecuteChanged();
            Assert.Equal(_count, 2);
        }
    }

    [Fact]
    public void BindCommandIocByTypeAndServiceKey()
    {

        var stubServiceProvider = MockServiceProvider.Instance;

        Type ss = typeof(_TestBindCommandIoc);
        // just use one of the test cases for BindCommandBase
        var bindCommand = new BindCommandIoc
        {
            ServiceType = "_TestBindCommandIoc",
            ServiceKey = "ServiceKey",
            ExecuteMethodName = "ExecuteMethod",
            CanExecuteMethodName = "CanExecuteMethod",
            EventToInvokeCanExecuteChanged = "EventHandlerNotifyCanExecuteChanged"
        };

        var bindResolve = bindCommand.ProvideValue(stubServiceProvider);
        var fakeTargetICommand = (ICommand)bindResolve;
        _TestBindCommandIoc _viewModel = _testList[_testList.Count - 1];

        if (fakeTargetICommand != null) fakeTargetICommand.Execute(null);
        Assert.Equal(_viewModel.ExecuteCalled, true);

        if (fakeTargetICommand != null) fakeTargetICommand.CanExecute(null);
        Assert.Equal(_viewModel.CanExecuteCalled, true);

        if (fakeTargetICommand != null) fakeTargetICommand.CanExecuteChanged += EventHandlerCanExecuteChanged;
        _count = 0;
        // Execute notification
        _viewModel.ExecuteEventNotifyCanExecuteChanged();
        Assert.Equal(_count, 1);

        // Execute notification
        if (_viewModel.DelegateNotifyCanExecuteChanged != null)
        {
            _viewModel.DelegateNotifyCanExecuteChanged();
            Assert.Equal(_count, 2);
        }
    }

    [Fact]
    public void BindCommandIocExceptions()
    {

        var stubServiceProvider = MockServiceProvider.Instance;

        var bindCommand = new BindCommandIoc
        {
            ExecuteMethodName = "_ExecuteMethod",
            CanExecuteMethodName = "CanExecuteMethod",
            EventToInvokeCanExecuteChanged = "EventHandlerNotifyCanExecuteChanged",
        };

        UnitTestInternal.AssertThrows(typeof(ArgumentNullException), () => bindCommand.ProvideValue(stubServiceProvider),
            "ProvideValueExceptions - expected exception - ArgumentNullException");

        bindCommand = new BindCommandIoc
        {
            ServiceKey = "ServiceKey",
            ExecuteMethodName = "_ExecuteMethod",
            CanExecuteMethodName = "CanExecuteMethod",
            EventToInvokeCanExecuteChanged = "EventHandlerNotifyCanExecuteChanged",
        };

        UnitTestInternal.AssertThrows(typeof(ArgumentNullException), () => bindCommand.ProvideValue(stubServiceProvider),
            "ProvideValueExceptions - expected exception - ArgumentNullException");

        bindCommand = new BindCommandIoc
        {
            ServiceType = "ABRACADABRA",
            ServiceKey = "ServiceKey",
            ExecuteMethodName = "_ExecuteMethod",
            CanExecuteMethodName = "CanExecuteMethod",
            EventToInvokeCanExecuteChanged = "EventHandlerNotifyCanExecuteChanged",
        };

        UnitTestInternal.AssertThrows(typeof(InvalidOperationException), () => bindCommand.ProvideValue(stubServiceProvider),
            "ProvideValueExceptions - expected exception - InvalidOperationException");

        bindCommand = new BindCommandIoc
        {
            ServiceType = typeof(_TestBindCommandIoc),
            ServiceKey = "ABRACADABRA",
            ExecuteMethodName = "_ExecuteMethod",
            CanExecuteMethodName = "CanExecuteMethod",
            EventToInvokeCanExecuteChanged = "EventHandlerNotifyCanExecuteChanged",
        };

        UnitTestInternal.AssertThrows(typeof(ArgumentException), () => bindCommand.ProvideValue(stubServiceProvider),
            "ProvideValueExceptions - expected exception - ArgumentException");
    }


    static List<_TestBindCommandIoc> _testList = new List<_TestBindCommandIoc>();
    class _TestBindCommandIoc
    {
        public _TestBindCommandIoc()
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

        public void ExecuteMethod(object sender)
        {
            ExecuteCalled = true;
        }
        public bool CanExecuteMethod(object sender)
        {
            CanExecuteCalled = true;
            return false;
        }

        public event Action EventHandlerNotifyCanExecuteChanged;

        public void ExecuteEventNotifyCanExecuteChanged()
        {
            if (EventHandlerNotifyCanExecuteChanged != null)
            {
                EventHandlerNotifyCanExecuteChanged();
            }
        }

        public Action DelegateNotifyCanExecuteChanged { get; set; }
    }

}
