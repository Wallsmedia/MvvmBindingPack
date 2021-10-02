using Xunit;
using System.Windows;
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
    public class UnitTestsAutoWireViewConrolsTheViewModelFirst
    {

        [Fact]
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
                Assert.True(viewmodel.TestButtonGotFocusCalled, "TestButton_GotFocus in View  was not wired");

                dependencyObject.RaiseEvent(new RoutedEventArgs(UIElement.LostFocusEvent));
                Assert.True(viewmodel.TestButtonLostFocusCalled, "TestButton_GotFocus  in View was not wired");

                Assert.NotNull(viewmodel._ButtonXName_TestButton);

                Assert.NotNull(viewmodel._Button_GotFocus);
                Assert.True(viewmodel._Button_GotFocus.IsEvent);

                Assert.NotNull(viewmodel._Button_LostFocus);
                Assert.True(viewmodel._Button_LostFocus.IsEvent, "TestButton GotFocus event was not wired to View Model from View");

            });
        }

        [Fact]
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
                Assert.True(viewmodel.TestButtonGotFocusCalled, "TestButton_GotFocus in View  was not wired");

                dependencyObject.RaiseEvent(new RoutedEventArgs(UIElement.LostFocusEvent));
                Assert.True(viewmodel.TestButtonLostFocusCalled, "TestButton_GotFocus  in View was not wired");

                Assert.NotNull(viewmodel._ButtonXName_TestButton);

                Assert.NotNull(viewmodel._Button_GotFocus);
                Assert.True(viewmodel._Button_GotFocus.IsEvent);

                Assert.NotNull(viewmodel._Button_LostFocus);
                Assert.True(viewmodel._Button_LostFocus.IsEvent, "TestButton GotFocus event was not wired to View Model from View");
            });

        }

        [Fact]
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
                Assert.True(viewmodel.TestButtonGotFocusCalled, "TestButton_GotFocus in View  was not wired");

                dependencyObject.RaiseEvent(new RoutedEventArgs(UIElement.LostFocusEvent));
                Assert.True(viewmodel.TestButtonLostFocusCalled, "TestButton_GotFocus  in View was not wired");

                Assert.NotNull(viewmodel._ButtonXName_TestButton);

                Assert.NotNull(viewmodel._Button_GotFocus);
                Assert.True(viewmodel._Button_GotFocus.IsEvent, "TestButton GotFocus event was not wired to View Model from View");

                Assert.NotNull(viewmodel._Button_LostFocus);
                Assert.True(viewmodel._Button_LostFocus.IsEvent, "TestButton GotFocus event was not wired to View Model from View");
            });

        }

        [Fact]
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
                Assert.Equal("Content", dependencyObject.Content);

                Assert.Equal("Tag", dependencyObject.Tag);

                dependencyObject.Tag = "TagTag";
                Assert.Equal("TagTag", viewmodel._propertyDymanicContent);

                viewmodel.PropertyDymanicContent = "TagTagTag";
                Assert.Equal("TagTagTag", dependencyObject.Tag);

                Assert.Equal(123, Grid.GetRow(dependencyObject));

                Assert.NotNull(viewmodel._Button_Content);
                Assert.True(viewmodel._Button_Content.IsProperty, "TestButton Content property was not wired to View Model from View");
                Assert.Equal("Content", viewmodel._Button_Content.GetPropertyValue());

                Assert.NotNull(viewmodel._Button_Tag);
                Assert.True(viewmodel._Button_Tag.IsProperty, "TestButton Tag property was not wired to View Model from View");
                Assert.Equal("TagTagTag", viewmodel._Button_Tag.GetPropertyValue());

                Assert.NotNull(viewmodel._Button_GridRow);
                Assert.True(viewmodel._Button_GridRow.IsProperty, "TestButton (Grid.Row) property was not wired to View Model from View");
                Assert.Equal(123, viewmodel._Button_GridRow.GetPropertyValue());
            });
        }

        [Fact]
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
                Assert.False(dependencyObject.IsEnabled, "TestButton ICommand.CanExecute was not wired to View Model from View");

                viewmodel.ButtonCanExecute = true;
                Assert.True(dependencyObject.IsEnabled, "TestButton ICommand.CanExecute was not wired to View Model from View");

                var iCmd = dependencyObject.Command;
                iCmd.Execute(null);

                Assert.True(viewmodel.ButtonExecuteCalled, "TestButton ICommand.Execute was not wired to View Model from View");
            });
        }

        [Fact]
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
                Assert.True(viewmodel.ButtonExecuteCalled, "TestButton ICommand.Execute was not wired to View Model from View");
            });
        }

        [Fact]
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
                Assert.False(dependencyObject.IsEnabled, "TestButton ICommand.CanExecute was not wired to View Model from View");

                viewmodel.ButtonCanExecute = true;
                Assert.True(dependencyObject.IsEnabled, "TestButton ICommand.CanExecute was not wired to View Model from View");

                var iCmd = dependencyObject.Command;
                iCmd.Execute(null);

                Assert.True(viewmodel.ButtonExecuteCalled, "TestButton ICommand.Execute was not wired to View Model from View");
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