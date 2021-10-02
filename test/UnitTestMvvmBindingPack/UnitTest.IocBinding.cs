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
using Microsoft.Extensions.DependencyInjection;
using MvvmBindingPack;

namespace UnitTestMvvmBindingPack
{
    /// <summary>
    /// Unit test for IocBinding class 
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable"), TestClass]
    public class UnitTestIocBinding
    {

        public UnitTestIocBinding()
        {
            ServiceCollection services = new ServiceCollection();
            services.AddTransient<TestTypeIocClass>();
            AutoWireVmDataContext.ServiceProvider = services.BuildServiceProvider();
        }

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }



        [TestMethod]
        public void TestMethodIocBindingConstructing1()
        {
            var iocBinding = new IocBinding();
            Assert.AreEqual(iocBinding.ServiceKey, null, "IocBinding - expected value null for ServiceKey");
            Assert.AreEqual(iocBinding.ServiceType, null, "IocBinding - expected value null for ServiceType");
        }

        [TestMethod]
        public void TestMethodIocBindingConstructing2()
        {
            var iocBinding = new IocBinding(typeof(TestTypeIocClass));
            Assert.AreEqual(iocBinding.ServiceKey, null, "IocBinding - expected value null for ServiceKey");
            Assert.AreEqual(iocBinding.ServiceType, typeof(TestTypeIocClass), "IocBinding - expected value TestTypeIocClass for ServiceType");
        }


        [TestMethod]
        public void TestMethodIocBindingException()
        {
            var iocBinding = new IocBinding();
            // ReSharper disable AssignNullToNotNullAttribute
            UnitTestInternal.AssertThrows(typeof(ArgumentNullException), () => iocBinding.ProvideValue(null), " IocBinding - expected ArgumentNullException");
            // ReSharper restore AssignNullToNotNullAttribute
        }

    }

    public class TestTypeIocClass
    {
    }
}
