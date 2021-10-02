using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows;
using MvvmBindingPack;

namespace UnitTestMvvmBindingPack
{

    [TestClass]
    public class UnitTestAddPropertyChangeEvents
    {
        [TestMethod]
        public void AddPropertyChangeEvents()
        {
            ExecuteInStaMode.Invoke(() =>
            {
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
            });
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
