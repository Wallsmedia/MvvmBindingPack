using Xunit;
using System.Windows;
using MvvmBindingPack;


namespace UnitTestMvvmBindingPack
{
    class AutoWireDataContextView : Window { }
    class AutoWireDataContext : Window { }
    class AutoWireAliasDataContext : Window { }
    class AliasDataContext : Window { }

    public class UnitTestsAutoWireVmDataContext
    {
        [Fact]
        public void AutoWireDataContextNoXName()
        {
            ExecuteInStaMode.Invoke(() =>
            {
                // case UnitTest.AutoWireDataContextView ==> UnitTest.ViewModels.AutoWireDataContextViewModel

                AutoWireDataContextView dependencyObject = new AutoWireDataContextView();
                object dataContext = dependencyObject.DataContext;
                Assert.Null(dataContext);

                AutoWireVmDataContext wireProvider = new AutoWireVmDataContext();
                wireProvider.Execute(dependencyObject);
                dataContext = dependencyObject.DataContext;
                Assert.NotNull(dataContext);

                object awVM = BindXAML.GetAutoWiredViewModel(dependencyObject);
                Assert.True(object.ReferenceEquals(awVM, dataContext));
            });
        }

        [Fact]
        public void AutoWireDataContextNoXNameTag()
        {
            ExecuteInStaMode.Invoke(() =>
            {

                // case UnitTest.AutoWireDataContextView ==> UnitTest.ViewModels.AutoWireDataContextViewModel

                AutoWireDataContextView dependencyObject = new AutoWireDataContextView();
                object viewModel = dependencyObject.Tag;
                Assert.Null(viewModel);

                AutoWireVmDataContext wireProvider = new AutoWireVmDataContext() { TargetPropertyName = "Tag" };
                wireProvider.Execute(dependencyObject);
                viewModel = dependencyObject.Tag;
                Assert.NotNull(viewModel);

                object awVM = BindXAML.GetAutoWiredViewModel(dependencyObject);
                Assert.True(object.ReferenceEquals(awVM, viewModel));
            });
        }

        [Fact]
        public void AutoWireDataContextNoXNameNeg()
        {
            ExecuteInStaMode.Invoke(() =>
            {

                // case UnitTest.AutoWireDataContext !=> UnitTest.ViewModels.AutoWireDataContext
                //                                   !=> UnitTest.ViewModels.AutoWireDataContextViewModel

                AutoWireDataContext dependencyObject = new AutoWireDataContext();
                object dataContext = dependencyObject.DataContext;
                Assert.Null(dataContext);

                AutoWireVmDataContext wireProvider = new AutoWireVmDataContext();
                wireProvider.Execute(dependencyObject);
                dataContext = dependencyObject.DataContext;
                Assert.Null(dataContext);
            });
        }

        [Fact]
        public void AutoWireDataContextNoXNameSubMatch()
        {

            ExecuteInStaMode.Invoke(() =>
            {
                // case UnitTest.AutoWireDataContext ==> UnitTest.ViewModels.AutoWireDataContext because (sub match)
                //                                   !=> UnitTest.ViewModels.AutoWireDataContextViewModel

                AutoWireDataContext dependencyObject = new AutoWireDataContext();
                object dataContext = dependencyObject.DataContext;
                Assert.Null(dataContext);

                AutoWireVmDataContext wireProvider = new AutoWireVmDataContext() { UseMaxNameSubMatch = true };
                wireProvider.Execute(dependencyObject);
                dataContext = dependencyObject.DataContext;
                Assert.NotNull(dataContext);

                object awVM = BindXAML.GetAutoWiredViewModel(dependencyObject);
                Assert.True(object.ReferenceEquals(awVM, dataContext));
            });
        }

        [Fact]
        public void AutoWireDataContextXNamePriority1()
        {
            ExecuteInStaMode.Invoke(() =>
            {

                // case UnitTest.AutoWireDataContext   !=> UnitTest.ViewModels.AutoWireDataContext
                // x:Name=AutoWireDataContextViewModel ==> UnitTest.ViewModels.AutoWireDataContextViewModel

                AutoWireDataContext dependencyObject = new AutoWireDataContext();
                dependencyObject.Name = "AutoWireDataContextViewModel";
                object dataContext = dependencyObject.DataContext;
                Assert.Null(dataContext);

                AutoWireVmDataContext wireProvider = new AutoWireVmDataContext();
                wireProvider.Execute(dependencyObject);
                dataContext = dependencyObject.DataContext;
                Assert.NotNull(dataContext);

                object awVM = BindXAML.GetAutoWiredViewModel(dependencyObject);
                Assert.True(object.ReferenceEquals(awVM, dataContext));
            });
        }

        [Fact]
        public void AutoWireDataContextXNamePriority2()
        {
            ExecuteInStaMode.Invoke(() =>
            {

                // case UnitTest.AutoWireDataContext   !=> UnitTest.ViewModels.AutoWireDataContext
                // x:Name=AutoWireDataContextView      ==> UnitTest.ViewModels.AutoWireDataContextViewModel

                AutoWireDataContext dependencyObject = new AutoWireDataContext();
                dependencyObject.Name = "AutoWireDataContextView";
                object dataContext = dependencyObject.DataContext;
                Assert.Null(dataContext);

                AutoWireVmDataContext wireProvider = new AutoWireVmDataContext();
                wireProvider.Execute(dependencyObject);
                dataContext = dependencyObject.DataContext;
                Assert.NotNull(dataContext);

                object awVM = BindXAML.GetAutoWiredViewModel(dependencyObject);
                Assert.True(object.ReferenceEquals(awVM, dataContext));
            });
        }

        [Fact]
        public void AutoWireDataContextNoNameAlias1()
        {
            ExecuteInStaMode.Invoke(() =>
            {

                // case UnitTest.AutoWireAliaasDataContext   !=> UnitTest.ViewModels.AutoWireAliasDataContext
                //                                           ==> UnitTest.ViewModels.AutoWireAbracadabraName123 because  [ViewModelClassAlias("AutoWireAliasDataContext")]

                AutoWireAliasDataContext dependencyObject = new AutoWireAliasDataContext();
                object dataContext = dependencyObject.DataContext;
                Assert.Null(dataContext);

                AutoWireVmDataContext wireProvider = new AutoWireVmDataContext();
                wireProvider.Execute(dependencyObject);
                dataContext = dependencyObject.DataContext;
                Assert.NotNull(dataContext);
            });
        }

        [Fact]
        public void AutoWireDataContextXNameAlias2()
        {
            ExecuteInStaMode.Invoke(() =>
            {

                // case UnitTest.AliasDataContext            !=> UnitTest.ViewModels.AliasDataContext
                // x:Name=AutoWireAliasDataContext           !=> UnitTest.ViewModels.AutoWireAliasDataContext
                //                                           ==> UnitTest.ViewModels.AutoWireAbracadabraName123 because  [ViewModelClassAlias("AutoWireAliasDataContext")]

                AliasDataContext dependencyObject = new AliasDataContext();
                dependencyObject.Name = "AutoWireAliasDataContext";
                object dataContext = dependencyObject.DataContext;
                Assert.Null(dataContext);

                AutoWireVmDataContext wireProvider = new AutoWireVmDataContext();
                wireProvider.Execute(dependencyObject);
                dataContext = dependencyObject.DataContext;
                Assert.NotNull(dataContext);
            });
        }

        [Fact]
        public void OwrAutoWireDataContextNoXName()
        {
            ExecuteInStaMode.Invoke(() =>
            {

                // case UnitTest.AutoWireDataContextView ==> UnitTest.ViewModels.AutoWireDataContextViewModel

                AutoWireDataContextView dependencyObject = new AutoWireDataContextView();
                object dataContext = dependencyObject.DataContext;
                Assert.Null(dataContext);

                AutoWireVmDataContext wireProvider = new AutoWireVmDataContext() { ViewModelNamespaceOverwrite = "Abracadabra.Where.It.Placed" };
                wireProvider.Execute(dependencyObject);
                dataContext = dependencyObject.DataContext;
                Assert.NotNull(dataContext);
            });
        }

        [Fact]
        public void OwrAutoWireDataContextNoXNameNeg()
        {
            ExecuteInStaMode.Invoke(() =>
            {

                // case UnitTest.AutoWireDataContext !=> UnitTest.ViewModels.AutoWireDataContext
                //                                   !=> UnitTest.ViewModels.AutoWireDataContextViewModel

                AutoWireDataContext dependencyObject = new AutoWireDataContext();
                object dataContext = dependencyObject.DataContext;
                Assert.Null(dataContext);

                AutoWireVmDataContext wireProvider = new AutoWireVmDataContext() { ViewModelNamespaceOverwrite = "Abracadabra.Where.It.Placed" };
                wireProvider.Execute(dependencyObject);
                dataContext = dependencyObject.DataContext;
                Assert.Null(dataContext);
            });
        }

        [Fact]
        public void OwrAutoWireDataContextNoXNameSubMatch()
        {
            ExecuteInStaMode.Invoke(() =>
            {

                // case UnitTest.AutoWireDataContext ==> UnitTest.ViewModels.AutoWireDataContext because (sub match)
                //                                   !=> UnitTest.ViewModels.AutoWireDataContextViewModel

                AutoWireDataContext dependencyObject = new AutoWireDataContext();
                object dataContext = dependencyObject.DataContext;
                Assert.Null(dataContext);

                AutoWireVmDataContext wireProvider = new AutoWireVmDataContext() { UseMaxNameSubMatch = true, ViewModelNamespaceOverwrite = "Abracadabra.Where.It.Placed" };
                wireProvider.Execute(dependencyObject);
                dataContext = dependencyObject.DataContext;
                Assert.NotNull(dataContext);
            });
        }


        [Fact]
        public void OwrAutoWireDataContextXNamePriority1()
        {
            ExecuteInStaMode.Invoke(() =>
            {

                // case UnitTest.AutoWireDataContext   !=> UnitTest.ViewModels.AutoWireDataContext
                // x:Name=AutoWireDataContextViewModel ==> UnitTest.ViewModels.AutoWireDataContextViewModel

                AutoWireDataContext dependencyObject = new AutoWireDataContext();
                dependencyObject.Name = "AutoWireDataContextViewModel";
                object dataContext = dependencyObject.DataContext;
                Assert.Null(dataContext);

                AutoWireVmDataContext wireProvider = new AutoWireVmDataContext();
                wireProvider.Execute(dependencyObject);
                dataContext = dependencyObject.DataContext;
                Assert.NotNull(dataContext);
            });
        }

        [Fact]
        public void OwrAutoWireDataContextXNamePriority2()
        {
            ExecuteInStaMode.Invoke(() =>
                       {
                // case UnitTest.AutoWireDataContext   !=> UnitTest.ViewModels.AutoWireDataContext
                // x:Name=AutoWireDataContextView      ==> UnitTest.ViewModels.AutoWireDataContextViewModel

                AutoWireDataContext dependencyObject = new AutoWireDataContext();
                           dependencyObject.Name = "AutoWireDataContextView";
                           object dataContext = dependencyObject.DataContext;
                           Assert.Null(dataContext);

                           AutoWireVmDataContext wireProvider = new AutoWireVmDataContext() { ViewModelNamespaceOverwrite = "Abracadabra.Where.It.Placed" };
                           wireProvider.Execute(dependencyObject);
                           dataContext = dependencyObject.DataContext;
                           Assert.NotNull(dataContext);
                       });
        }

        [Fact]
        public void OwrAutoWireDataContextNoNameAlias1()
        {
            ExecuteInStaMode.Invoke(() =>
            {

                // case UnitTest.AutoWireAliaasDataContext   !=> UnitTest.ViewModels.AutoWireAliasDataContext
                //                                           ==> UnitTest.ViewModels.AutoWireAbracadabraName123 because  [ViewModelClassAlias("AutoWireAliasDataContext")]

                AutoWireAliasDataContext dependencyObject = new AutoWireAliasDataContext();
                object dataContext = dependencyObject.DataContext;
                Assert.Null(dataContext);

                AutoWireVmDataContext wireProvider = new AutoWireVmDataContext() { ViewModelNamespaceOverwrite = "Abracadabra.Where.It.Placed" };
                wireProvider.Execute(dependencyObject);
                dataContext = dependencyObject.DataContext;
                Assert.NotNull(dataContext);
            });
        }

        [Fact]
        public void OwrAutoWireDataContextXNameAlias2()
        {
            ExecuteInStaMode.Invoke(() =>
            {

                // case UnitTest.AliasDataContext            !=> UnitTest.ViewModels.AliasDataContext
                // x:Name=AutoWireAliasDataContext           !=> UnitTest.ViewModels.AutoWireAliasDataContext
                //                                           ==> UnitTest.ViewModels.AutoWireAbracadabraName123 because  [ViewModelClassAlias("AutoWireAliasDataContext")]

                AliasDataContext dependencyObject = new AliasDataContext();
                dependencyObject.Name = "AutoWireAliasDataContext";
                object dataContext = dependencyObject.DataContext;
                Assert.Null(dataContext);

                AutoWireVmDataContext wireProvider = new AutoWireVmDataContext() { ViewModelNamespaceOverwrite = "Abracadabra.Where.It.Placed" };
                wireProvider.Execute(dependencyObject);
                dataContext = dependencyObject.DataContext;
                Assert.NotNull(dataContext);
            });
        }

        [Fact]
        public void OwrAutoWireDataContextXNameAlias3()
        {
            ExecuteInStaMode.Invoke(() =>
            {

                // The name "_AutoWire_Alias__DataContext" is split into parts {"Auto","Wire","Alias","Data","Context"} , and
                // The name "AutoWireAliasDataContext"     is split into parts {"Auto","Wire","Alias","Data","Context"};
                // so they are considered as the match.

                // case UnitTest.AliasDataContext            !=> UnitTest.ViewModels.AliasDataContext
                // x:Name=_AutoWire_Alias__DataContext       !=> UnitTest.ViewModels.AutoWireAliasDataContext
                //                                           ==> UnitTest.ViewModels.AutoWireAbracadabraName123 because  [ViewModelClassAlias("AutoWireAliasDataContext")]

                AliasDataContext dependencyObject = new AliasDataContext();
                dependencyObject.Name = "_AutoWire_Alias__DataContext";
                object dataContext = dependencyObject.DataContext;
                Assert.Null(dataContext);

                AutoWireVmDataContext wireProvider = new AutoWireVmDataContext() { ViewModelNamespaceOverwrite = "Abracadabra.Where.It.Placed" };
                wireProvider.Execute(dependencyObject);
                dataContext = dependencyObject.DataContext;
                Assert.NotNull(dataContext);
            });
        }

        [Fact]
        public void OwrAutoWireDataContextXNameAlias4()
        {
            ExecuteInStaMode.Invoke(() =>
            {

                // The name "AutoWireAliasDataContextVer123"        is split into parts {"Auto","Wire","Alias","Data","Context","Ver123"} , and
                // The name "AutoWire_Alias_DataContext_Ver123"     is split into parts {"Auto","Wire","Alias","Data","Context","Ver123"};
                // so they are considered as the match.

                // case UnitTest.AliasDataContext            !=> UnitTest.ViewModels.AliasDataContext
                // x:Name=AutoWireAliasDataContextVer123     !=> UnitTest.ViewModels.AutoWireAliasDataContext
                //                                           ==> UnitTest.ViewModels.AutoWireAbracadabraName123 because  [ViewModelClassAlias("AutoWire_Alias_DataContext_Ver123")]

                AliasDataContext dependencyObject = new AliasDataContext();
                dependencyObject.Name = "AutoWireAliasDataContextVer123";
                object dataContext = dependencyObject.DataContext;
                Assert.Null(dataContext);

                AutoWireVmDataContext wireProvider = new AutoWireVmDataContext() { ViewModelNamespaceOverwrite = "Abracadabra.Where.It.Placed" };
                wireProvider.Execute(dependencyObject);
                dataContext = dependencyObject.DataContext;
                Assert.NotNull(dataContext);
            });
        }
    }
}




namespace UnitTestMvvmBindingPack.ViewModels
{
    class AutoWireDataContextViewModel
    {
        public bool EventCalled;

        public void RoutedEventHandler(object sender, RoutedEventArgs e)
        {
            EventCalled = true;
        }
    }

    [ViewModelClassAlias("AutoWireAliasDataContext")]
    class AutoWireAbracadabraName123
    {
        public bool EventCalled;

        public void RoutedEventHandler(object sender, RoutedEventArgs e)
        {
            EventCalled = true;
        }
    }
}


namespace Abracadabra.Where.It.Placed
{
    class AutoWireDataContextViewModel
    {
        public bool EventCalled;

        public void RoutedEventHandler(object sender, RoutedEventArgs e)
        {
            EventCalled = true;
        }
    }

    [ViewModelClassAlias("AutoWireAliasDataContext")]
    [ViewModelClassAlias("AutoWire_Alias_DataContext_Ver123")]
    class AutoWireAbracadabraName123
    {
        public bool EventCalled;

        public void RoutedEventHandler(object sender, RoutedEventArgs e)
        {
            EventCalled = true;
        }
    }
}