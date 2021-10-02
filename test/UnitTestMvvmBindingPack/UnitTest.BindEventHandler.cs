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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvvmBindingPack;
using System.Windows;

namespace UnitTestMvvmBindingPack
{
    /// <summary>
    /// Summary description for BindEventHandler
    /// </summary>
    [TestClass]
    public class UnitTestBindEventHandler
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        public event RoutedEventHandler FakeTargetHandler;

        [TestMethod]
        public void BindEventHandlerConstructor()
        {
            const string source = "Source";
            var bindEvent = new BindEventHandler(source);
            Assert.AreEqual(source, bindEvent.Source, "BindEventHandler parameterized constructor fail");
        }

        [TestMethod]
        public void BindEventHandlerByPropertyName()
        {

            var stubServiceProvider = MockServiceProvider.Instance;

            var bindEvent = new BindEventHandler();
            var viewModel = new _TestBindEventHandler();
            bindEvent.Source = viewModel;
            bindEvent.PropertyName = "ButtonClickPropDelegate";

            var bindResolve = bindEvent.ProvideValue(stubServiceProvider);
            FakeTargetHandler += (RoutedEventHandler)bindResolve;
            FakeTargetHandler(null, null);
            Assert.AreEqual(viewModel.Count, int.MaxValue, "BindEventHandlerByPropertyName - bind was not resolved");
            FakeTargetHandler -= (RoutedEventHandler)bindResolve;

        }


        [TestMethod]
        public void BindEventHandlerByMethodName()
        {

            var stubServiceProvider = MockServiceProvider.Instance;

            var bindEvent = new BindEventHandler();
            var viewModel = new _TestBindEventHandler();
            bindEvent.Source = viewModel;
            bindEvent.MethodName = "ButtonClick";

            var bindResolve = bindEvent.ProvideValue(stubServiceProvider);
            FakeTargetHandler += (RoutedEventHandler)bindResolve;
            FakeTargetHandler(null, null);
            Assert.AreEqual(viewModel.Count, int.MaxValue, "BindEventHandlerByMethodName - bind was not resolved");
            FakeTargetHandler -= (RoutedEventHandler)bindResolve;
        }


        [TestMethod]
        public void ProvideValueExceptions()
        {
            var stubServiceProvider = MockServiceProvider.Instance;

            var bindEvent = new BindEventHandler();
            var viewModel = new _TestBindEventHandler();
            bindEvent.Source = viewModel;

            // ReSharper disable AccessToModifiedClosure
            UnitTestInternal.AssertThrows(typeof(ArgumentException), () => bindEvent.ProvideValue(stubServiceProvider),
            // ReSharper restore AccessToModifiedClosure
            "ProvideValueExceptions - expected exception - ArgumentException");
            bindEvent.PropertyName = "ButtonClickPropDelegate";
            bindEvent.MethodName = "ButtonClick";

            // ReSharper disable AccessToModifiedClosure
            UnitTestInternal.AssertThrows(typeof(ArgumentException), () => bindEvent.ProvideValue(stubServiceProvider),
                // ReSharper restore AccessToModifiedClosure
                "ProvideValueExceptions - expected exception - ArgumentException");

            bindEvent.PropertyName = null;
            bindEvent.MethodName = null;

            stubServiceProvider = MockServiceProvider.Instance;

            UnitTestInternal.AssertThrows(typeof(ArgumentException), () => bindEvent.ProvideValue(stubServiceProvider),
                "ProvideValueExceptions - expected exception - ArgumentException");
        }


        class _TestBindEventHandler
        {
            public int Count;

            /// <summary>
            /// Just Constructor
            /// </summary>

            public _TestBindEventHandler()
            {
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
