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
    ///  Unit tests for CommandHandlerProxy class 
    /// </summary>
    [TestClass]
    public class UnitTestBindPackExt
    {
        /// <summary>
        /// Test's stub class
        /// </summary>
        readonly CommandEventStubs _testStubs;

        public UnitTestBindPackExt()
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
        }

        [TestMethod]
        public void GetMethodInfoTests()
        {
            var info = _testStubs.GetType().GetMethod("Button_Click_External");
            MethodInfo testInfo = _testStubs.GetMethodInfo("Button_Click_External");
            Assert.AreEqual(info, testInfo, "GetMethodInfo - Test failed to get a proper info.");
        }

        [TestMethod]
        public void GetPropertyInfoTests()
        {
            var info = _testStubs.GetType().GetProperty("CanExecuteFlag");
            PropertyInfo testInfo = _testStubs.GetPropertyInfo("CanExecuteFlag");
            Assert.AreEqual(info, testInfo, "GetPropertyInfo - Test failed to get a proper info.");
        }

        [TestMethod]
        public void GetEventInfoTests()
        {
            var info = _testStubs.GetType().GetEvent("IssueNotifyExecuteButtonChanged");
            EventInfo testInfo = _testStubs.GetEventInfo("IssueNotifyExecuteButtonChanged");
            Assert.AreEqual(info, testInfo, "GetEventInfo - Test failed to get a proper info.");
        }

    }
}
