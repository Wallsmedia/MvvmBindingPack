// 
//  MVVM-WPF-NetCore Markup Binding Extensions
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

namespace UnitTestMvvmBindingPack
{
    static class UnitTestInternal
    {
        /// <summary>
        /// Asserts that the given testedAction throws an expected of the type specified in the generic parameter, or a subtype thereof.
        /// </summary>
        /// <typeparam name="TExpectedException">Type of the expected to check for.</typeparam>
        /// <param name="testedAction is null">Action to run.</param>
        /// <param name="testedAction"></param>
        /// <expected cref="ArgumentNullException"><paramref name="testedAction"/> is null.</expected>
        public static void AssertThrows<TExpectedException>(Action testedAction) where TExpectedException : Exception
        {
            if (testedAction == null)
                throw new ArgumentNullException("testedAction");

            var exceptionFaild = false;
            try
            {
                testedAction();
                exceptionFaild = true;
            }
            catch (TExpectedException)
            {
            }
            catch (Exception ex)
            {
                // ReSharper disable RedundantStringFormatCall
                Assert.Fail(string.Format("Expected {0} threw {1}.\r\n\r\nStack trace:\r\n{2}", typeof(TExpectedException).Name, ex.GetType().Name, ex.StackTrace));
                // ReSharper restore RedundantStringFormatCall
            }

            if (exceptionFaild)
                // ReSharper disable RedundantStringFormatCall
                Assert.Fail(string.Format("Expected {0}.", typeof(TExpectedException).Name));
            // ReSharper restore RedundantStringFormatCall
        }

        /// <summary>
        /// Asserts that the given testedAction throws an expected of the type specified in the generic parameter, or a subtype thereof.
        /// </summary>
        /// <typeparam name="TExpectedException">Type of the expected to check for.</typeparam>
        /// <param name="testedAction is null">Action to run.</param>
        /// <param name="testedAction"></param>
        /// <param name="message">Error message for assert failure.</param>
        /// <expected cref="ArgumentNullException"><paramref name="testedAction"/> is null.</expected>
        public static void AssertThrows<TExpectedException>(Action testedAction, string message) where TExpectedException : Exception
        {
            if (testedAction == null)
                throw new ArgumentNullException("testedAction");

            var exceptionFaild = false;
            try
            {
                testedAction();
                exceptionFaild = true;
            }
            catch (TExpectedException)
            {
            }
            catch
            {
                Assert.Fail(message);
            }

            if (exceptionFaild)
                Assert.Fail(message);
        }


        /// <summary>
        /// Asserts that the given testedAction throws the specified expected.
        /// </summary>
        /// <param name="expected">Exception to assert being thrown.</param>
        /// <param name="testedAction">Action to run.</param>
        /// <param name="message">Error message for assert failure.</param>
        /// <expected cref="ArgumentNullException"><paramref name="testedAction is null"/> is null.</expected>
        public static void AssertThrows(Type expected, Action testedAction, string message)
        {
            if (testedAction == null)
                throw new ArgumentNullException("testedAction");

            var exceptionFaild = false;
            try
            {
                testedAction();
                exceptionFaild = true;
            }
            catch (Exception ex)
            {
                Assert.AreEqual(expected, ex.GetType());
            }

            if (exceptionFaild)
                Assert.Fail(message);
        }

    }
}
