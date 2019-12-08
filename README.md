### Model - View - ViewModel binding package.

Full details of package using link: [MvvmBindingPack - Wiki ](https://github.com/Wallsmedia/MvvmBindingPack/wiki/MvvmBindingPack-v-3.0)

### Development Supported by JetBrains Open Source Program:

<a href="https://www.jetbrains.com/?from=XmlResult"> <img src="https://github.com/Wallsmedia/XmlResult/blob/master/Logo/rider/logo.png?raw=true" Width="40p" /></a> **Fast & powerful,
cross platform .NET IDE**

<a href="https://www.jetbrains.com/?from=XmlResult"> <img src="https://github.com/Wallsmedia/XmlResult/blob/master/Logo/resharper/logo.png?raw=true" Width="40p" /></a> **The Visual Studio Extension for .NET Developers**

# Nuget.Org
- NuGet.org package http://www.nuget.org/packages/MvvmBindingPack/ 
- NuGet.org package http://www.nuget.org/packages/MvvmBindingPack.Debug/ 

# Version: 3.1.0

**.Net Core App  windows Desktop support**
- Supports: **netcoreapp3.1**
- Signed assembly;
**Dependences:**
 - None


# Version: 3.0.0

**.Net Core App  windows Desktop support**
- Supports: **netcoreapp3.0**
- Signed assembly;
**Dependences:**
 - None
  

# Version: 2.5.0

**.Net Core transition version**
- NuGet.org package http://www.nuget.org/packages/MvvmBindingPack/ 
- NuGet.org package http://www.nuget.org/packages/MvvmBindingPack.Debug/ 
- Supports: **net462, net472, net48, uap10.0**
- Signed assembly;

**Dependences:**
 - None

## **(!) Breaking changes of IoC/DI container initialization.**

##  Setup Unity DI container example for App.xaml.cs

``` c#
    publicpartialclassApp : Application
    {

        privateUnityContainer _unityContainer;
        privateUnityServiceLocator _servicelocator;
        public App()
        {
            _unityContainer = newUnityContainer();
            _servicelocator = newUnityServiceLocator(_unityContainer);
            AutoWireVmDataContext.ServiceProvider = _servicelocator;
            var vmMw = newViewModelNew();
            // instance that will be resolved when it's used ServiceType
            _unityContainer.RegisterInstance(typeof(ViewModelNew),
            vmMw,newContainerControlledLifetimeManager());
        }
    }
```

## Setup DotNet Core DI container example for App.xaml.cs
``` C#
private void Application_Startup(object sender, StartupEventArgs e)
{

    ServiceCollection services = new ServiceCollection();
    services.AddSingleton<AutoBindingViewModel>();
    services.AddSingleton<IocBindingViewModel>();
    AutoWireVmDataContext.ServiceProvider = services.BuildServiceProvider();
}
```

# Version: 2.0.1

**Microsoft.Practices.ServiceLocation Support**
- NuGet.org package http://www.nuget.org/packages/MvvmBindingPack/ 
- NuGet.org package http://www.nuget.org/packages/MvvmBindingPack.Debug/ 
- Supports: **net452, net462, net47, uap10.0**
- Signed assembly;

**Dependences:**
CommonServiceLocator  (== 1.3.0) {namespace **Microsoft.Practices.ServiceLocation**}


# Version: 2.0.1

**Unity Container Support**

- NuGet.org package http://www.nuget.org/packages/MvvmBindingPack.Unity/ 
- NuGet.org package http://www.nuget.org/packages/MvvmBindingPack.Unity.Debug/ 
- Supports: **net452, net462, net47**
- Signed assembly;

**Dependences:**
 CommonServiceLocator (>= 2.0.2) {namespace **CommonServiceLocator**}
 
**Recommended Unity Nuget Dependences:**
https://www.nuget.org/packages/Unity.Container    (>= 5.8.6)
https://www.nuget.org/packages/Unity.ServiceLocation (>= 2.1.1)


The decision to develop this binding package had been made in response of minimizing a the cost of  the quality UI development along with using Agile development style. It is a big advantage to have a solution where you are able to refactor a dozen of Views and View Models, in a couple hours, by a customer request.

 
#### Welcome to MvvmBindingPack

MvvmBindingPack is the robust MVVM framework platform for UX high-quality solutions with support of IoC/DI containers. MVVM pattern is widely used in developing of XAML-based GUI applications. It is impossible to provide the quality UX design implementations without using this pattern. Quality of UX design directly depends on the techniques or features that used for implementing the MVVM pattern. Clear separation of concerns between View (XAML code coupled with its code-behind) and View Model characterizes a profession level and quality and the of the product implementation. The package has the compatible features implementation for XAML WPF and Win Store Application XAML.
MVVM pattern in UI development process looks like a principal of "Dependency Inversion" between  View and View Model.
 

