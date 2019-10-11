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
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace UnitTestMvvmBindingPack
{
    class DataContextButonVFM : Button { }

    /// <summary>
    /// Test cases where the View Model defines what to resolve in the View
    /// </summary>
    [TestClass]
    public class UnitTestsAutoWireViewConrolsTheViewModelFirst
    {

        [TestMethod]
        public void AutoWireControlsTestButtonEvents()
        {
            ExecuteInStaMode.Invoke(() =>
            {
                DataContextButonVFM dependencyObject = new DataContextButonVFM();
                var viewmodel = new UnitTestMvvmBindingPack.ViewModels.DataContextViewModelButtonT2();
                dependencyObject.DataContext = viewmodel;
                AutoWireViewConrols wireProvider = new AutoWireViewConrols();
                dependencyObject.Name = "TestButton";
                wireProvider.FrameworkElementLoaded(dependencyObject, new RoutedEventArgs(FrameworkElement.LoadedEvent));

                //
                // check wiring results of wiring - EVENTS
                //
                dependencyObject.RaiseEvent(new RoutedEventArgs(UIElement.GotFocusEvent));
                Assert.IsTrue(viewmodel.TestButtonGotFocusCalled, "TestButton_GotFocus in View  was not wired");

                dependencyObject.RaiseEvent(new RoutedEventArgs(UIElement.LostFocusEvent));
                Assert.IsTrue(viewmodel.TestButtonLostFocusCalled, "TestButton_GotFocus  in View was not wired");

                Assert.IsNotNull(viewmodel._ButtonXName_TestButton, "TestButton control was not wired to View Model from View");

                Assert.IsNotNull(viewmodel._Button_GotFocus, "TestButton GotFocus event was not wired to View Model from View");
                Assert.IsTrue(viewmodel._Button_GotFocus.IsEvent, "TestButton GotFocus event was not wired to View Model from View");

                Assert.IsNotNull(viewmodel._Button_LostFocus, "TestButton GotFocus event was not wired to View Model from View");
                Assert.IsTrue(viewmodel._Button_LostFocus.IsEvent, "TestButton GotFocus event was not wired to View Model from View");

            });
        }

        [TestMethod]
        public void AutoWireControlsTestButtonEventsTag()
        {
            ExecuteInStaMode.Invoke(() =>
            {
                DataContextButonVFM dependencyObject = new DataContextButonVFM();
                var viewmodel = new UnitTestMvvmBindingPack.ViewModels.DataContextViewModelButtonT2();
                dependencyObject.Tag = viewmodel;
                AutoWireViewConrols wireProvider = new AutoWireViewConrols() { SourcePropertyName = "Tag" };
                dependencyObject.Name = "TestButton";
                wireProvider.FrameworkElementLoaded(dependencyObject, new RoutedEventArgs(FrameworkElement.LoadedEvent));

                //
                // check wiring results of wiring - EVENTS
                //
                dependencyObject.RaiseEvent(new RoutedEventArgs(UIElement.GotFocusEvent));
                Assert.IsTrue(viewmodel.TestButtonGotFocusCalled, "TestButton_GotFocus in View  was not wired");

                dependencyObject.RaiseEvent(new RoutedEventArgs(UIElement.LostFocusEvent));
                Assert.IsTrue(viewmodel.TestButtonLostFocusCalled, "TestButton_GotFocus  in View was not wired");

                Assert.IsNotNull(viewmodel._ButtonXName_TestButton, "TestButton control was not wired to View Model from View");

                Assert.IsNotNull(viewmodel._Button_GotFocus, "TestButton GotFocus event was not wired to View Model from View");
                Assert.IsTrue(viewmodel._Button_GotFocus.IsEvent, "TestButton GotFocus event was not wired to View Model from View");

                Assert.IsNotNull(viewmodel._Button_LostFocus, "TestButton GotFocus event was not wired to View Model from View");
                Assert.IsTrue(viewmodel._Button_LostFocus.IsEvent, "TestButton GotFocus event was not wired to View Model from View");
            });

        }

        [TestMethod]
        public void AutoWireControlsTestButtonEventsSource()
        {
            ExecuteInStaMode.Invoke(() =>
            {
                DataContextButonVFM dependencyObject = new DataContextButonVFM();
                var viewmodel = new UnitTestMvvmBindingPack.ViewModels.DataContextViewModelButtonT2();
                AutoWireViewConrols wireProvider = new AutoWireViewConrols() { Source = viewmodel };
                dependencyObject.Name = "TestButton";
                wireProvider.FrameworkElementLoaded(dependencyObject, new RoutedEventArgs(FrameworkElement.LoadedEvent));

                //
                // check wiring results of wiring - EVENTS
                //
                dependencyObject.RaiseEvent(new RoutedEventArgs(UIElement.GotFocusEvent));
                Assert.IsTrue(viewmodel.TestButtonGotFocusCalled, "TestButton_GotFocus in View  was not wired");

                dependencyObject.RaiseEvent(new RoutedEventArgs(UIElement.LostFocusEvent));
                Assert.IsTrue(viewmodel.TestButtonLostFocusCalled, "TestButton_GotFocus  in View was not wired");

                Assert.IsNotNull(viewmodel._ButtonXName_TestButton, "TestButton control was not wired to View Model from View");

                Assert.IsNotNull(viewmodel._Button_GotFocus, "TestButton GotFocus event was not wired to View Model from View");
                Assert.IsTrue(viewmodel._Button_GotFocus.IsEvent, "TestButton GotFocus event was not wired to View Model from View");

                Assert.IsNotNull(viewmodel._Button_LostFocus, "TestButton GotFocus event was not wired to View Model from View");
                Assert.IsTrue(viewmodel._Button_LostFocus.IsEvent, "TestButton GotFocus event was not wired to View Model from View");
            });

        }

        [TestMethod]
        public void AutoWireControlsTestButtonProperties()
        {
            ExecuteInStaMode.Invoke(() =>
            {
                DataContextButonVFM dependencyObject = new DataContextButonVFM();
                var viewmodel = new UnitTestMvvmBindingPack.ViewModels.DataContextViewModelButtonT2();
                dependencyObject.DataContext = viewmodel;
                AutoWireViewConrols wireProvider = new AutoWireViewConrols();
                dependencyObject.Name = "TestButton";
                wireProvider.FrameworkElementLoaded(dependencyObject, new RoutedEventArgs(FrameworkElement.LoadedEvent));
                //
                // check wiring results of wiring - PROPERTIES
                //
                Assert.AreEqual("Content", dependencyObject.Content, "TestButton.Content was not wired to View Model from View");

                Assert.AreEqual("Tag", dependencyObject.Tag, "TestButton.Tag was not wired to View Model from View");

                dependencyObject.Tag = "TagTag";
                Assert.AreEqual("TagTag", viewmodel._propertyDymanicContent, "TestButton.Tag was not wired to View Model from View");

                viewmodel.PropertyDymanicContent = "TagTagTag";
                Assert.AreEqual("TagTagTag", dependencyObject.Tag, "TestButton.Tag was not wired to View Model from View");

                Assert.AreEqual(123, Grid.GetRow(dependencyObject), "TestButton (Grid.Row) was not wired to View Model from View");

                Assert.IsNotNull(viewmodel._Button_Content, "TestButton Content property was not wired to View Model from View");
                Assert.IsTrue(viewmodel._Button_Content.IsProperty, "TestButton Content property was not wired to View Model from View");
                Assert.AreEqual("Content", viewmodel._Button_Content.GetPropertyValue(), "TestButton Content property was not wired to View Model from View");

                Assert.IsNotNull(viewmodel._Button_Tag, "TestButton Tag property was not wired to View Model from View");
                Assert.IsTrue(viewmodel._Button_Tag.IsProperty, "TestButton Tag property was not wired to View Model from View");
                Assert.AreEqual("TagTagTag", viewmodel._Button_Tag.GetPropertyValue(), "TestButton Tag property was not wired to View Model from View");

                Assert.IsNotNull(viewmodel._Button_GridRow, "TestButton (Grid.Row) property was not wired to View Model from View");
                Assert.IsTrue(viewmodel._Button_GridRow.IsProperty, "TestButton (Grid.Row) property was not wired to View Model from View");
                Assert.AreEqual(123, viewmodel._Button_GridRow.GetPropertyValue(), "TestButton (Grid.Row) property was not wired to View Model from View");
            });
        }

        [TestMethod]
        public void AutoWireControlsTestButtonICommand1()
        {
            ExecuteInStaMode.Invoke(() =>
            {
                DataContextButonVFM dependencyObject = new DataContextButonVFM();
                var viewmodel = new UnitTestMvvmBindingPack.ViewModels.DataContextViewModelButtonT2();
                dependencyObject.DataContext = viewmodel;
                AutoWireViewConrols wireProvider = new AutoWireViewConrols();
                dependencyObject.Name = "TestButton";
                wireProvider.FrameworkElementLoaded(dependencyObject, new RoutedEventArgs(FrameworkElement.LoadedEvent));
                //
                // check wiring results of wiring - ICommand interface
                //
                Assert.IsFalse(dependencyObject.IsEnabled, "TestButton ICommand.CanExecute was not wired to View Model from View");

                viewmodel.ButtonCanExecute = true;
                Assert.IsTrue(dependencyObject.IsEnabled, "TestButton ICommand.CanExecute was not wired to View Model from View");

                var iCmd = dependencyObject.Command;
                iCmd.Execute(null);

                Assert.IsTrue(viewmodel.ButtonExecuteCalled, "TestButton ICommand.Execute was not wired to View Model from View");
            });
        }

        [TestMethod]
        public void AutoWireControlsTestButtonICommand2()
        {
            ExecuteInStaMode.Invoke(() =>
            {
                DataContextButonVFM dependencyObject = new DataContextButonVFM();
                var viewmodel = new UnitTestMvvmBindingPack.ViewModels.DataContextViewModelButtonT2();
                dependencyObject.DataContext = viewmodel;
                AutoWireViewConrols wireProvider = new AutoWireViewConrols();
                dependencyObject.Name = "TestButton2";
                wireProvider.FrameworkElementLoaded(dependencyObject, new RoutedEventArgs(FrameworkElement.LoadedEvent));
                //
                // check wiring results of wiring - ICommand interface
                //

                viewmodel.ButtonCanExecute = true;
                var iCmd = dependencyObject.Command;
                iCmd.Execute(null);
                Assert.IsTrue(viewmodel.ButtonExecuteCalled, "TestButton ICommand.Execute was not wired to View Model from View");
            });
        }

        [TestMethod]
        public void AutoWireControlsTestButtonICommand3()
        {
            ExecuteInStaMode.Invoke(() =>
            {
                DataContextButonVFM dependencyObject = new DataContextButonVFM();
                var viewmodel = new UnitTestMvvmBindingPack.ViewModels.DataContextViewModelButtonT2();
                dependencyObject.DataContext = viewmodel;
                AutoWireViewConrols wireProvider = new AutoWireViewConrols();
                dependencyObject.Name = "TestButton3";
                wireProvider.FrameworkElementLoaded(dependencyObject, new RoutedEventArgs(FrameworkElement.LoadedEvent));
                //
                // check wiring results of wiring - ICommand interface
                //
                Assert.IsFalse(dependencyObject.IsEnabled, "TestButton ICommand.CanExecute was not wired to View Model from View");

                viewmodel.ButtonCanExecute = true;
                Assert.IsTrue(dependencyObject.IsEnabled, "TestButton ICommand.CanExecute was not wired to View Model from View");

                var iCmd = dependencyObject.Command;
                iCmd.Execute(null);

                Assert.IsTrue(viewmodel.ButtonExecuteCalled, "TestButton ICommand.Execute was not wired to View Model from View");
            });
        }
    }
}

namespace UnitTestMvvmBindingPack.ViewModels
{
    class DataContextViewModelButtonT2 : NotifyPropertyChangedBase
    {

        //
        // Events test section view model entities
        //

        public void TestButton_GotFocus(object sender, RoutedEventArgs e)
        {
            TestButtonGotFocusCalled = true;
        }
        public bool TestButtonGotFocusCalled;

        [ViewXNameAlias("TestButton", "LostFocus")]
        public void TestBtnLstFocus(object sender, RoutedEventArgs e)
        {
            TestButtonLostFocusCalled = true;
        }
        public bool TestButtonLostFocusCalled;

        [ViewXNameSourceObjectMapping("TestButton")]
        public Button _ButtonXName_TestButton;

        [ViewXNameSourceTargetMapping("TestButton", "GotFocus")]
        public ViewXNameSourceTarget _Button_GotFocus;

        [ViewXNameSourceTargetMapping("TestButton", "LostFocus")]
        public ViewXNameSourceTarget _Button_LostFocus;

        //
        // Properties test section view model entities
        //

        [ViewXNameAlias("TestButton", "Content")]
        public string _propertyStaticContent = "Content";

        public string _propertyDymanicContent = "Tag";

        [ViewXNameAlias("TestButton", "Tag", BindingMode = BindingMode.TwoWay)]
        public string PropertyDymanicContent
        {
            get { return _propertyDymanicContent; }
            set { _propertyDymanicContent = value; NotifyPropertyChanged(); }
        }

        // adding attached dependency property 
        private int _gridrow = 123;

        [ViewXNameAlias("TestButton", "Grid.Row", BindingMode = BindingMode.OneWay)]
        public int Gridrow
        {
            get { return _gridrow; }
            set { _gridrow = value; }
        }

        [ViewXNameSourceTargetMapping("TestButton", "Content")]
        public ViewXNameSourceTarget _Button_Content;

        [ViewXNameSourceTargetMapping("TestButton", "Tag")]
        public ViewXNameSourceTarget _Button_Tag;

        [ViewXNameSourceTargetMapping("TestButton", "Grid.Row")]
        public ViewXNameSourceTarget _Button_GridRow;

        //
        // ICommand interface tests section
        //

        //*1*2*
        [ViewXNameAlias("TestButton", "Command.Execute")]
        [ViewXNameAlias("TestButton2", "Command.Execute")]
        public void ButtonExecute(object obj)
        {
            ButtonExecuteCalled = true;
        }
        public bool ButtonExecuteCalled;

        //*1*
        public bool _buttonCanExecute;
        [ViewXNameAlias("TestButton", "Command.CanExecute")]
        public bool ButtonCanExecute
        {
            get
            {
                return _buttonCanExecute;
            }
            set { _buttonCanExecute = value; NotifyPropertyChanged(); }
        }

        //*2*
        [ViewXNameAlias("TestButton2", "Command.CanExecute")]
        public bool _button2CanExecute()
        {
            return _buttonCanExecute;
        }

        //*3*
        [ViewXNameAlias("TestButton3", "Command")]
        public ICommand ProxyCommandClass
        {
            get { return new CommandHandlerProxy(ButtonExecute, this.GetPropertyInfo("ButtonCanExecute"), this); }
        }
    }

}