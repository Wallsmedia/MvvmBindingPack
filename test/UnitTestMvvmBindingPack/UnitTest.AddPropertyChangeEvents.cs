using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Collections;
using System.Collections.Generic;
using System.Linq;

using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Markup;
using System.Windows;
using System.ComponentModel;
using MvvmBindingPack;
using System.Threading;

namespace UnitTestMvvmBindingPack
{
    [TestClass]
    public class UnitTestAddPropertyChangeEvents
    {
        [TestMethod]
        public void AddPropertyChangeEvents()
        {
            Thread t = new Thread(ThreadProc);
            t.SetApartmentState(ApartmentState.STA);
            t.Start();

            UIElement dependencyObject = new UIElement();
            var bindEvent = new BindEventHandler();
            var viewModel = new _TestBindEventHandlerChange();
            bindEvent.Source = viewModel;
            bindEvent.MethodName = "EventHandler";
            bindEvent.TargetPropertyName = "IsEnabled";

            BindXAML.ProcessAddPropertyChangeEventItems(dependencyObject, bindEvent);

            // change the property
            dependencyObject.IsEnabled = false;
            Assert.IsTrue(viewModel.EventCalled, "Dependency property change event was not set");
        }

        class _TestBindEventHandlerChange
        {
            public bool EventCalled;
            public void EventHandler(object sender, EventArgs e)
            {
                EventCalled = true;
            }
        }

    }
}
