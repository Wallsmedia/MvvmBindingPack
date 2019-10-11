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


namespace UnitTestMvvmBindingPack
{
    [TestClass]
    public class UnitTestAddEvents
    {
        [TestMethod]
        public void AddEvents()
        {
            UIElement dependencyObject = new UIElement();
            var bindEvent = new BindEventHandler();
            var viewModel = new _TestBindEventHandler();
            bindEvent.Source = viewModel;
            bindEvent.MethodName = "RoutedEventHandler";
            bindEvent.TargetEventName = "GotFocus";

            // rise event to the property         
            BindXAML.ProcessAddEventItems(dependencyObject, bindEvent);
            RoutedEventArgs newEventArgs = new RoutedEventArgs(UIElement.GotFocusEvent);
            dependencyObject.RaiseEvent(newEventArgs);

            Assert.IsTrue(viewModel.EventCalled, "Dependency property lister  event handler was not set");
        }

        class _TestBindEventHandler
        {
            public bool EventCalled;

            RoutedEventHandler GotFocusEvent;

            public void RoutedEventHandler(object sender, RoutedEventArgs e)
            {
                EventCalled = true;
            }
        }

    }
}
