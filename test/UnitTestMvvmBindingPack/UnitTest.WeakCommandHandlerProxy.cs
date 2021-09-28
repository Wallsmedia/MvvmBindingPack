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

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvvmBindingPack;
using System.Reflection;
using System.Windows;

namespace UnitTestMvvmBindingPack
{
    /// <summary>
    ///  Unit tests for WeakCommandHandlerProxy class 
    /// </summary>
    [TestClass]
    public class UnitTestWeakCommandHandlerProxy
    {
        /// <summary>
        /// Test's stub class
        /// </summary>
        readonly CommandEventStubs _testStubs;

        public UnitTestWeakCommandHandlerProxy()
        {
            _testStubs = new CommandEventStubs();
        }

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        /// <summary>
        /// TestInitialize to run code before running each test 
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            _testStubs.ResetTestFlags();
            StaticCommandEventStubs.ResetTestFlags();
            _canExecuteCnagedCalledStatic = false;
            _canExecuteCnagedCalled = false;
        }


        [TestMethod]
        public void CommandHandlerProxyConstructorExceptionTests()
        {
            EventInfo outerEvent = _testStubs.GetType().GetEvent("IssueNotifyExecuteButtonChanged");
            // ReSharper restore ObjectCreationAsStatement

            UnitTestInternal.AssertThrows(typeof(ArgumentException),
                // ReSharper disable ObjectCreationAsStatement
                () => { new WeakCommandHandlerProxy(null); },
                // ReSharper restore ObjectCreationAsStatement
                "WeakCommandHandlerProxy - Constructor should throw Exception");

            UnitTestInternal.AssertThrows(typeof(ArgumentException),
                // ReSharper disable ObjectCreationAsStatement
                () => { new WeakCommandHandlerProxy(_testStubs.ExecuteButton_Click_External, null, outerEvent); },
                // ReSharper restore ObjectCreationAsStatement
                "WeakCommandHandlerProxy - Constructor should throw Exception");

            UnitTestInternal.AssertThrows(typeof(ArgumentException),
                // ReSharper disable ObjectCreationAsStatement
                () => { new WeakCommandHandlerProxy(_testStubs.ExecuteButton_Click_External, null, outerEvent, _testStubs); },
                // ReSharper restore ObjectCreationAsStatement
                "WeakCommandHandlerProxy - Constructor should throw Exception");

            UnitTestInternal.AssertThrows(typeof(ArgumentException),
                // ReSharper disable ObjectCreationAsStatement
                () => { new WeakCommandHandlerProxy(_testStubs.ExecuteButton_Click_External, null, (EventInfo)null, _testStubs); },
                // ReSharper restore ObjectCreationAsStatement
                "WeakCommandHandlerProxy - Constructor should throw Exception");

            UnitTestInternal.AssertThrows(typeof(ArgumentException),
                // ReSharper disable ObjectCreationAsStatement
                () => { new WeakCommandHandlerProxy(null, _testStubs.CanExecuteButton_Click_External, a => _testStubs.IssueNotifyExecuteButtonChanged += a); },
                // ReSharper restore ObjectCreationAsStatement
                "WeakCommandHandlerProxy - Constructor should throw Exception");

            UnitTestInternal.AssertThrows(typeof(ArgumentException),
                // ReSharper disable ObjectCreationAsStatement
                  () => { new WeakCommandHandlerProxy(_testStubs.ExecuteButton_Click_External, null, a => _testStubs.IssueNotifyExecuteButtonChanged += a); },
                // ReSharper restore ObjectCreationAsStatement
                  "WeakCommandHandlerProxy - Constructor should throw Exception");
            // ReSharper restore ObjectCreationAsStatement

        }

        [TestMethod]
        public void StaticCommandHandlerProxyConstructorExceptionTests()
        {
            EventInfo outerEvent = typeof(StaticCommandEventStubs).GetEvent("IssueNotifyExecuteButtonChanged");

            UnitTestInternal.AssertThrows(typeof(ArgumentException),
                // ReSharper disable ObjectCreationAsStatement
                () => { new WeakCommandHandlerProxy(null); },
                // ReSharper restore ObjectCreationAsStatement
                "WeakCommandHandlerProxy - Constructor should throw Exception");

            UnitTestInternal.AssertThrows(typeof(ArgumentException),
                // ReSharper disable ObjectCreationAsStatement
                // ReSharper disable ImplicitlyCapturedClosure
                () => { new WeakCommandHandlerProxy(StaticCommandEventStubs.ExecuteButton_Click_External, null, outerEvent); },
                // ReSharper restore ImplicitlyCapturedClosure
                // ReSharper restore ObjectCreationAsStatement
                "WeakCommandHandlerProxy - Constructor should throw Exception");

            UnitTestInternal.AssertThrows(typeof(ArgumentException),
                // ReSharper disable ObjectCreationAsStatement
                () => { new WeakCommandHandlerProxy(StaticCommandEventStubs.ExecuteButton_Click_External, null, outerEvent, _testStubs); },
                // ReSharper restore ObjectCreationAsStatement
                "WeakCommandHandlerProxy - Constructor should throw Exception");

            UnitTestInternal.AssertThrows(typeof(ArgumentException),
                // ReSharper disable ObjectCreationAsStatement
                () => { new WeakCommandHandlerProxy(StaticCommandEventStubs.ExecuteButton_Click_External, null, (EventInfo)null, _testStubs); },
                // ReSharper restore ObjectCreationAsStatement
                "WeakCommandHandlerProxy - Constructor should throw Exception");

            UnitTestInternal.AssertThrows(typeof(ArgumentException),
                // ReSharper disable ObjectCreationAsStatement
                () => { new WeakCommandHandlerProxy(null, StaticCommandEventStubs.CanExecuteButton_Click_External, a => StaticCommandEventStubs.IssueNotifyExecuteButtonChanged += a); },
                // ReSharper restore ObjectCreationAsStatement
                "WeakCommandHandlerProxy - Constructor should throw Exception");

            UnitTestInternal.AssertThrows<ArgumentException>(
                // ReSharper disable ObjectCreationAsStatement
                  () => { new WeakCommandHandlerProxy(StaticCommandEventStubs.ExecuteButton_Click_External, null, a => StaticCommandEventStubs.IssueNotifyExecuteButtonChanged += a); },
                // ReSharper restore ObjectCreationAsStatement
                  "WeakCommandHandlerProxy - Constructor should throw Exception");
        }

        [TestMethod]
        public void CommandHandlerProxyExecuteTest()
        {
            var chp = new WeakCommandHandlerProxy(_testStubs.ExecuteButton_Click_External);
            chp.Execute(null);
            Assert.AreEqual(_testStubs.ExecuteButtonClickExternalCalled, true, "Target delegate for ICommand:Execute() was called");
        }

        [TestMethod]
        public void StaticCommandHandlerProxyExecuteTest()
        {
            var chp = new WeakCommandHandlerProxy(StaticCommandEventStubs.ExecuteButton_Click_External);
            chp.Execute(null);
            Assert.AreEqual(StaticCommandEventStubs.ExecuteButtonClickExternalCalled, true, "Target delegate for ICommand:Execute() was called for static");
        }

        [TestMethod]
        public void CommandHandlerProxyCanExecuteTest()
        {
            var chp = new WeakCommandHandlerProxy(_testStubs.ExecuteButton_Click_External, _testStubs.CanExecuteButton_Click_External);
            chp.Execute(null);
            bool result = chp.CanExecute(null);
            Assert.AreEqual(_testStubs.ExecuteButtonClickExternalCalled, true, "Target delegate for ICommand:CanExecute() was not called");
            Assert.AreEqual(_testStubs.CanExecuteButtonClickExternalFlag, true, "Target delegate for ICommand:CanExecute() was not called");
            Assert.AreEqual(result, true, "Target delegate for ICommand:CanExecute() retrun wrong flag");
        }

        [TestMethod]
        public void StaticCommandHandlerProxyCanExecuteTest()
        {
            var chp = new WeakCommandHandlerProxy(StaticCommandEventStubs.ExecuteButton_Click_External, StaticCommandEventStubs.CanExecuteButton_Click_External);
            chp.Execute(null);
            bool result = chp.CanExecute(null);
            Assert.AreEqual(StaticCommandEventStubs.ExecuteButtonClickExternalCalled, true, "Target delegate for ICommand:CanExecute() was not called for static");
            Assert.AreEqual(StaticCommandEventStubs.CanExecuteButtonClickExternalFlag, true, "Target delegate for ICommand:CanExecute() was not called for static");
            Assert.AreEqual(result, true, "Target delegate for ICommand:CanExecute() retrun wrong flag");
        }

        bool _canExecuteCnagedCalled;
        void CanExecuteChangedTest(object sender, EventArgs e)
        {
            _canExecuteCnagedCalled = true;
        }

        [TestMethod]
        public void CommandHandlerProxyNotifyCanExecuteChangedTest()
        {
            EventInfo outerEvent = _testStubs.GetType().GetEvent("IssueNotifyExecuteButtonChanged");
            var chp = new WeakCommandHandlerProxy(_testStubs.ExecuteButton_Click_External, _testStubs.CanExecuteButton_Click_External, outerEvent, _testStubs);
            chp.CanExecuteChanged += CanExecuteChangedTest;
            chp.Execute(null);
            bool result = chp.CanExecute(null);

            _testStubs.InvokeNotifyChanged();

            Assert.AreEqual(_testStubs.ExecuteButtonClickExternalCalled, true, "Target delegate for ICommand:Execute() was not called");
            Assert.AreEqual(_testStubs.CanExecuteButtonClickExternalFlag, true, "Target delegate for ICommand:Execute() was not called");
            Assert.AreEqual(result, true, "Target delegate for ICommand:CanExecute() return wrong flag");
            Assert.AreEqual(_canExecuteCnagedCalled, true, "ICommand:CanExecuteChanged event was not risen");

        }

        bool _canExecuteCnagedCalledStatic;
        void CanExecuteChangedTestStatic(object sender, EventArgs e)
        {
            _canExecuteCnagedCalledStatic = true;
        }
        [TestMethod]
        public void StaticCommandHandlerProxyNotifyCanExecuteChangedTest()
        {
            EventInfo outerEvent = typeof(StaticCommandEventStubs).GetEvent("IssueNotifyExecuteButtonChanged");
            var chp = new WeakCommandHandlerProxy(StaticCommandEventStubs.ExecuteButton_Click_External, StaticCommandEventStubs.CanExecuteButton_Click_External, outerEvent);
            chp.CanExecuteChanged += CanExecuteChangedTestStatic;
            chp.Execute(null);
            bool result = chp.CanExecute(null);
            StaticCommandEventStubs.InvokeNotifyChanged();
            Assert.AreEqual(StaticCommandEventStubs.ExecuteButtonClickExternalCalled, true, "Target delegate for ICommand:Execute() was not called for static");
            Assert.AreEqual(StaticCommandEventStubs.CanExecuteButtonClickExternalFlag, true, "Target delegate for ICommand:Execute() was not called for static");
            Assert.AreEqual(result, true, "Target delegate for ICommand:CanExecute() return wrong flag");
            Assert.AreEqual(_canExecuteCnagedCalledStatic, true, "ICommand:CanExecuteChanged event was not risen");
        }

        [TestMethod]
        public void CommandHandlerProxyNotifyCanExecuteChangedTestDelegate()
        {
            Action invokeNotify = null;
            var chp = new WeakCommandHandlerProxy(_testStubs.ExecuteButton_Click_External, _testStubs.CanExecuteButton_Click_External, a => invokeNotify = a);
            chp.CanExecuteChanged += CanExecuteChangedTest;
            chp.Execute(null);
            bool result = chp.CanExecute(null);
            invokeNotify();
            Assert.AreEqual(_testStubs.ExecuteButtonClickExternalCalled, true, "Target delegate for ICommand:Execute() was not called");
            Assert.AreEqual(_testStubs.CanExecuteButtonClickExternalFlag, true, "Target delegate for ICommand:Execute() was not called");
            Assert.AreEqual(result, true, "Target delegate for ICommand:CanExecute() return wrong flag");
            Assert.AreEqual(_canExecuteCnagedCalled, true, "ICommand:CanExecuteChanged event was not risen");
        }

        [TestMethod]
        public void StaticCommandHandlerProxyNotifyCanExecuteChangedTestDelegate()
        {
            Action invokeNotify = null;
            var chp = new WeakCommandHandlerProxy(StaticCommandEventStubs.ExecuteButton_Click_External, StaticCommandEventStubs.CanExecuteButton_Click_External, a => invokeNotify = a);
            chp.CanExecuteChanged += CanExecuteChangedTestStatic;
            chp.Execute(null);
            bool result = chp.CanExecute(null);
            invokeNotify();
            Assert.AreEqual(StaticCommandEventStubs.ExecuteButtonClickExternalCalled, true, "Target delegate for ICommand:Execute() was not called for static");
            Assert.AreEqual(StaticCommandEventStubs.CanExecuteButtonClickExternalFlag, true, "Target delegate for ICommand:Execute() was not called for static");
            Assert.AreEqual(result, true, "Target delegate for ICommand:CanExecute() return wrong flag");
            Assert.AreEqual(_canExecuteCnagedCalledStatic, true, "ICommand:CanExecuteChanged event was not risen");
        }

        [TestMethod]
        public void CommandHandlerProxyCanExecuteChangedBoolProperty()
        {
            PropertyInfo boolPropertyInfo = _testStubs.GetPropertyInfo("CanExecuteFlag");
            var chp = new WeakCommandHandlerProxy(_testStubs.ExecuteButton_Click_External, boolPropertyInfo, _testStubs);
            chp.CanExecuteChanged += CanExecuteChangedTest;
            bool result = chp.CanExecute(null);

            Assert.AreEqual(result, false, "Target delegate for ICommand:CanExecute() return wrong flag");
            _testStubs.CanExecuteFlag = true;

            chp.Execute(null);
            result = chp.CanExecute(null);
            Assert.AreEqual(_testStubs.ExecuteButtonClickExternalCalled, true, "Target delegate for ICommand:Execute() was not called");
            Assert.AreEqual(_testStubs.CanExecuteButtonClickExternalFlag, true, "Target delegate for ICommand:Execute() was not called");
            Assert.AreEqual(result, true, "Target delegate for ICommand:CanExecute() return wrong flag");
            Assert.AreEqual(_canExecuteCnagedCalled, true, "ICommand:CanExecuteChanged event was not risen");
        }

    }
}
