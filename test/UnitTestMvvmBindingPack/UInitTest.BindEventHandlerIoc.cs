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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvvmBindingPack;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
namespace UnitTestMvvmBindingPack
{
    /// <summary>
    /// Summary description for BindEventHandlerIoc
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable"), TestClass]
    public class UnitTestBindEventHandlerIoc
    {
        _TestBindEventHandlerIoc _viewModel;
        public UnitTestBindEventHandlerIoc()
        {
            ServiceCollection services = new ServiceCollection();
            services.AddSingleton<_TestBindEventHandlerIoc>();
            AutoWireVmDataContext.ServiceProvider = services.BuildServiceProvider();
            _viewModel = AutoWireVmDataContext.ServiceProvider.GetService<_TestBindEventHandlerIoc>();
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

        public event RoutedEventHandler FakeTargetHandler;

        [TestMethod]
        public void BindEventHandlerConstructor()
        {
            Type serviceType = typeof(String);
            var bindEvent = new BindEventHandlerIoc(serviceType);
            Assert.AreEqual(serviceType, bindEvent.ServiceType, "BindEventHandlerIoc parameterized constructor fail");
        }

        [TestMethod]
        public void ProvideValueFromSourceByPropertyName()
        {

            var stubServiceProvider = MockServiceProvider.Instance;

            var bindEvent = new BindEventHandlerIoc
            {
                ServiceType = typeof(_TestBindEventHandlerIoc),
                PropertyName = "ButtonClickPropDelegate"
            };

            var bindResolve = bindEvent.ProvideValue(stubServiceProvider);
            FakeTargetHandler += (RoutedEventHandler)bindResolve;
            FakeTargetHandler(null, null);
            Assert.AreEqual(_viewModel.Count, int.MaxValue, "ProvideValueFromSourceByPropertyName - bind was not resolved");
            FakeTargetHandler -= (RoutedEventHandler)bindResolve;

        }

        [TestMethod]
        public void ProvideValueFromSourceByMethodName()
        {

            var stubServiceProvider = MockServiceProvider.Instance;

            var bindEvent = new BindEventHandlerIoc
            {
                ServiceType = typeof(_TestBindEventHandlerIoc),
                MethodName = "ButtonClick"
            };

            var bindResolve = bindEvent.ProvideValue(stubServiceProvider);
            FakeTargetHandler += (RoutedEventHandler)bindResolve;
            FakeTargetHandler(null, null);
            Assert.AreEqual(_viewModel.Count, int.MaxValue, "ProvideValueFromSourceByMethodName - bind was not resolved");
            FakeTargetHandler -= (RoutedEventHandler)bindResolve;
        }


        [TestMethod]
        public void ProvideValueExceptions()
        {
            var stubServiceProvider = MockServiceProvider.Instance;

            var bindEvent = new BindEventHandlerIoc { ServiceType = typeof(_TestBindEventHandlerIoc) };

            var provider = stubServiceProvider;
            // ReSharper disable ImplicitlyCapturedClosure
            UnitTestInternal.AssertThrows(typeof(ArgumentException), () => bindEvent.ProvideValue(provider),
            "ProvideValueExceptions - expected exception - ArgumentException");
            // ReSharper restore ImplicitlyCapturedClosure
            bindEvent.PropertyName = "ButtonClickPropDelegate";
            bindEvent.MethodName = "ButtonClick";

            // ReSharper disable ImplicitlyCapturedClosure
            UnitTestInternal.AssertThrows(typeof(ArgumentException), () => bindEvent.ProvideValue(provider),
                // ReSharper restore ImplicitlyCapturedClosure
                "ProvideValueExceptions - expected exception - ArgumentException");

            bindEvent.PropertyName = null;
            bindEvent.MethodName = null;
            stubServiceProvider = MockServiceProvider.Instance;
            // ReSharper disable ImplicitlyCapturedClosure
            UnitTestInternal.AssertThrows(typeof(ArgumentException), () => bindEvent.ProvideValue(stubServiceProvider),
                "ProvideValueExceptions - expected exception - ArgumentException");
            // ReSharper restore ImplicitlyCapturedClosure
        }


        class _TestBindEventHandlerIoc
        {
            public int Count;

            public CommandHandlerProxy CommandProxy { get; set; }

            /// <summary>
            /// Just Constructor
            /// </summary>

            public _TestBindEventHandlerIoc()
            {
                CommandProxy = new CommandHandlerProxy(ExecuteMethod);
            }

            public void ExecuteMethod(object sender)
            {
                Count = int.MinValue;
            }

            public void ButtonClick(object sender, RoutedEventArgs e)
            {
                Count = int.MaxValue;
            }
            public RoutedEventHandler ButtonClickPropDelegate
            {
                get { return ButtonClick; }
            }

        }
    }

}
