// 
//  MVVM-WPF Markup Dependency Injection Binding Extensions
//  Copyright © 2013-2014 Alexander Paskhin /paskhin@hotmail.co.uk/ All rights reserved.
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

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Rhino.Mocks;
using System.Windows.Markup;
using MvvmBindingPack;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace UnitTestMvvmBindingPack
{
    /// <summary>
    /// Summary description for BindCommandIoc
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable"), TestClass]
    public class UnitTestBindCommandIoc
    {
        readonly UnityServiceLocator _servicelocator;

        public UnitTestBindCommandIoc()
        {
            var unityContainer = new UnityContainer();
            _servicelocator = new UnityServiceLocator(unityContainer);
            ServiceLocator.SetLocatorProvider(() => _servicelocator);

            // instance that will be resolved when it's used ServiceType 
            unityContainer.RegisterInstance(typeof(UnityContainer), unityContainer, new ContainerControlledLifetimeManager());

            // instance that will be resolved when it's used ServiceType  !!! every time new instance as not used  new ContainerControlledLifetimeManager()
            unityContainer.RegisterType(typeof(_TestBindCommandIoc));

            // instance that will be resolved when it's used ServiceType & ServiceKey !!! every time new instance as not used  new ContainerControlledLifetimeManager()
            unityContainer.RegisterType(typeof(_TestBindCommandIoc), "ServiceKey");
        }

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion


        [TestMethod]
        public void BindEventHandlerConstructor()
        {
            Type serviceType = typeof(String);
            var bindCommand = new BindCommandIoc(serviceType);
            Assert.AreEqual(serviceType, bindCommand.ServiceType, "BindCommandIoc parameterized constructor fail");
        }

        int _count;
        void EventHandlerCanExecuteChanged(object sender, EventArgs e)
        {
            _count++;
        }

        [TestMethod]
        public void BindCommandIocByType()
        {

            var stubServiceProvider = MockRepository.GenerateMock<IServiceProvider>();
            var stubProvideValueTarget = MockRepository.GenerateMock<IProvideValueTarget>();
            stubProvideValueTarget.Stub(x => x.TargetObject).Return(new object());
            stubProvideValueTarget.Stub(x => x.TargetProperty).Return(null);
            stubServiceProvider.Stub(x => x.GetService(typeof(IProvideValueTarget))).Return(stubProvideValueTarget);
            
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
            Assert.AreEqual(_viewModel.ExecuteCalled, true, "Execute - bind was not resolved");

            if (fakeTargetICommand != null) fakeTargetICommand.CanExecute(null);
            Assert.AreEqual(_viewModel.CanExecuteCalled, true, "CanExecute - bind was not resolved");

            if (fakeTargetICommand != null) fakeTargetICommand.CanExecuteChanged += EventHandlerCanExecuteChanged;
            _count = 0;
            // Execute notification
            _viewModel.ExecuteEventNotifyCanExecuteChanged();
            Assert.AreEqual(_count, 1, "CanExecuteChanged - bind was not resolved");

            // Execute notification
            if (_viewModel.DelegateNotifyCanExecuteChanged != null)
            {
                _viewModel.DelegateNotifyCanExecuteChanged();
                Assert.AreEqual(_count, 2, "CanExecuteChanged - bind was not resolved");
            }
        }

        [TestMethod]
        public void BindCommandIocByTypeAndServiceKey()
        {

            var stubServiceProvider = MockRepository.GenerateMock<IServiceProvider>();
            var stubProvideValueTarget = MockRepository.GenerateMock<IProvideValueTarget>();
            stubProvideValueTarget.Stub(x => x.TargetObject).Return(new object());
            stubProvideValueTarget.Stub(x => x.TargetProperty).Return(null);
            stubServiceProvider.Stub(x => x.GetService(typeof(IProvideValueTarget))).Return(stubProvideValueTarget);

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
            Assert.AreEqual(_viewModel.ExecuteCalled, true, "Execute - bind was not resolved");

            if (fakeTargetICommand != null) fakeTargetICommand.CanExecute(null);
            Assert.AreEqual(_viewModel.CanExecuteCalled, true, "CanExecute - bind was not resolved");

            if (fakeTargetICommand != null) fakeTargetICommand.CanExecuteChanged += EventHandlerCanExecuteChanged;
            _count = 0;
            // Execute notification
            _viewModel.ExecuteEventNotifyCanExecuteChanged();
            Assert.AreEqual(_count, 1, "CanExecuteChanged - bind was not resolved");

            // Execute notification
            if (_viewModel.DelegateNotifyCanExecuteChanged != null)
            {
                _viewModel.DelegateNotifyCanExecuteChanged();
                Assert.AreEqual(_count, 2, "CanExecuteChanged - bind was not resolved");
            }
        }

        [TestMethod]
        public void BindCommandIocExceptions()
        {

            var stubServiceProvider = MockRepository.GenerateMock<IServiceProvider>();
            var stubProvideValueTarget = MockRepository.GenerateMock<IProvideValueTarget>();
            stubProvideValueTarget.Stub(x => x.TargetObject).Return(new object());
            stubProvideValueTarget.Stub(x => x.TargetProperty).Return(null);
            stubServiceProvider.Stub(x => x.GetService(typeof(IProvideValueTarget))).Return(stubProvideValueTarget);

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
}
