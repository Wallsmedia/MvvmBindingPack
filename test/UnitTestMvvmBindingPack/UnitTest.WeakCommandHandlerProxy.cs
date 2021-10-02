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
using MvvmBindingPack;
using System.Reflection;

namespace UnitTestMvvmBindingPack
{
    /// <summary>
    ///  Unit tests for WeakCommandHandlerProxy class 
    /// </summary>
    public class UnitTestWeakCommandHandlerProxy
    {
        /// <summary>
        /// Test's stub class
        /// </summary>
        readonly CommandEventStubs _testStubs;

        public UnitTestWeakCommandHandlerProxy()
        {
            _testStubs = new CommandEventStubs();
            _testStubs.ResetTestFlags();
            StaticCommandEventStubs.ResetTestFlags();
            _canExecuteCnagedCalledStatic = false;
            _canExecuteCnagedCalled = false;
        }


        [Fact]
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

        [Fact]
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

        [Fact]
        public void CommandHandlerProxyExecuteTest()
        {
            var chp = new WeakCommandHandlerProxy(_testStubs.ExecuteButton_Click_External);
            chp.Execute(null);
            Assert.Equal(_testStubs.ExecuteButtonClickExternalCalled, true);
        }

        [Fact]
        public void StaticCommandHandlerProxyExecuteTest()
        {
            var chp = new WeakCommandHandlerProxy(StaticCommandEventStubs.ExecuteButton_Click_External);
            chp.Execute(null);
            Assert.Equal(StaticCommandEventStubs.ExecuteButtonClickExternalCalled, true);
        }

        [Fact]
        public void CommandHandlerProxyCanExecuteTest()
        {
            var chp = new WeakCommandHandlerProxy(_testStubs.ExecuteButton_Click_External, _testStubs.CanExecuteButton_Click_External);
            chp.Execute(null);
            bool result = chp.CanExecute(null);
            Assert.Equal(_testStubs.ExecuteButtonClickExternalCalled, true);
            Assert.Equal(_testStubs.CanExecuteButtonClickExternalFlag, true);
            Assert.Equal(result, true);
        }

        [Fact]
        public void StaticCommandHandlerProxyCanExecuteTest()
        {
            var chp = new WeakCommandHandlerProxy(StaticCommandEventStubs.ExecuteButton_Click_External, StaticCommandEventStubs.CanExecuteButton_Click_External);
            chp.Execute(null);
            bool result = chp.CanExecute(null);
            Assert.Equal(StaticCommandEventStubs.ExecuteButtonClickExternalCalled, true);
            Assert.Equal(StaticCommandEventStubs.CanExecuteButtonClickExternalFlag, true);
            Assert.Equal(result, true);
        }

        bool _canExecuteCnagedCalled;
        void CanExecuteChangedTest(object sender, EventArgs e)
        {
            _canExecuteCnagedCalled = true;
        }

        [Fact]
        public void CommandHandlerProxyNotifyCanExecuteChangedTest()
        {
            EventInfo outerEvent = _testStubs.GetType().GetEvent("IssueNotifyExecuteButtonChanged");
            var chp = new WeakCommandHandlerProxy(_testStubs.ExecuteButton_Click_External, _testStubs.CanExecuteButton_Click_External, outerEvent, _testStubs);
            chp.CanExecuteChanged += CanExecuteChangedTest;
            chp.Execute(null);
            bool result = chp.CanExecute(null);

            _testStubs.InvokeNotifyChanged();

            Assert.Equal(_testStubs.ExecuteButtonClickExternalCalled, true);
            Assert.Equal(_testStubs.CanExecuteButtonClickExternalFlag, true);
            Assert.Equal(result, true);
            Assert.Equal(_canExecuteCnagedCalled, true);

        }

        bool _canExecuteCnagedCalledStatic;
        void CanExecuteChangedTestStatic(object sender, EventArgs e)
        {
            _canExecuteCnagedCalledStatic = true;
        }
        [Fact]
        public void StaticCommandHandlerProxyNotifyCanExecuteChangedTest()
        {
            EventInfo outerEvent = typeof(StaticCommandEventStubs).GetEvent("IssueNotifyExecuteButtonChanged");
            var chp = new WeakCommandHandlerProxy(StaticCommandEventStubs.ExecuteButton_Click_External, StaticCommandEventStubs.CanExecuteButton_Click_External, outerEvent);
            chp.CanExecuteChanged += CanExecuteChangedTestStatic;
            chp.Execute(null);
            bool result = chp.CanExecute(null);
            StaticCommandEventStubs.InvokeNotifyChanged();
            Assert.Equal(StaticCommandEventStubs.ExecuteButtonClickExternalCalled, true);
            Assert.Equal(StaticCommandEventStubs.CanExecuteButtonClickExternalFlag, true);
            Assert.Equal(result, true);
            Assert.Equal(_canExecuteCnagedCalledStatic, true);
        }

        [Fact]
        public void CommandHandlerProxyNotifyCanExecuteChangedTestDelegate()
        {
            Action invokeNotify = null;
            var chp = new WeakCommandHandlerProxy(_testStubs.ExecuteButton_Click_External, _testStubs.CanExecuteButton_Click_External, a => invokeNotify = a);
            chp.CanExecuteChanged += CanExecuteChangedTest;
            chp.Execute(null);
            bool result = chp.CanExecute(null);
            invokeNotify();
            Assert.Equal(_testStubs.ExecuteButtonClickExternalCalled, true);
            Assert.Equal(_testStubs.CanExecuteButtonClickExternalFlag, true);
            Assert.Equal(result, true);
            Assert.Equal(_canExecuteCnagedCalled, true);
        }

        [Fact]
        public void StaticCommandHandlerProxyNotifyCanExecuteChangedTestDelegate()
        {
            Action invokeNotify = null;
            var chp = new WeakCommandHandlerProxy(StaticCommandEventStubs.ExecuteButton_Click_External, StaticCommandEventStubs.CanExecuteButton_Click_External, a => invokeNotify = a);
            chp.CanExecuteChanged += CanExecuteChangedTestStatic;
            chp.Execute(null);
            bool result = chp.CanExecute(null);
            invokeNotify();
            Assert.Equal(StaticCommandEventStubs.ExecuteButtonClickExternalCalled, true);
            Assert.Equal(StaticCommandEventStubs.CanExecuteButtonClickExternalFlag, true);
            Assert.Equal(result, true);
            Assert.Equal(_canExecuteCnagedCalledStatic, true);
        }

        [Fact]
        public void CommandHandlerProxyCanExecuteChangedBoolProperty()
        {
            PropertyInfo boolPropertyInfo = _testStubs.GetPropertyInfo("CanExecuteFlag");
            var chp = new WeakCommandHandlerProxy(_testStubs.ExecuteButton_Click_External, boolPropertyInfo, _testStubs);
            chp.CanExecuteChanged += CanExecuteChangedTest;
            bool result = chp.CanExecute(null);

            Assert.Equal(result, false);
            _testStubs.CanExecuteFlag = true;

            chp.Execute(null);
            result = chp.CanExecute(null);
            Assert.Equal(_testStubs.ExecuteButtonClickExternalCalled, true);
            Assert.Equal(_testStubs.CanExecuteButtonClickExternalFlag, true);
            Assert.Equal(result, true);
            Assert.Equal(_canExecuteCnagedCalled, true);
        }

    }
}
