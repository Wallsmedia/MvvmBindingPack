### Model - View - ViewModel binding package.

Full details of package using link: [MvvmBindingPack - Wiki ](https://github.com/Wallsmedia/MvvmBindingPack/wiki/MvvmBindingPack-v-3.0)

### Development Supported by JetBrains Open Source Program:

<a href="https://www.jetbrains.com/?from=XmlResult"> <img src="https://github.com/Wallsmedia/XmlResult/blob/master/Logo/rider/logo.png?raw=true" Width="40p" /></a> **Fast & powerful,
cross platform .NET IDE**

<a href="https://www.jetbrains.com/?from=XmlResult"> <img src="https://github.com/Wallsmedia/XmlResult/blob/master/Logo/resharper/logo.png?raw=true" Width="40p" /></a> **The Visual Studio Extension for .NET Developers**

#### Nuget.Org
- NuGet.org package http://www.nuget.org/packages/MvvmBindingPack/ 
- NuGet.org package http://www.nuget.org/packages/MvvmBindingPack.Debug/ 

# Version: 6.0.0
**.Net Core App windows Desktop support**
- Supports: **net6.0**

# Version: 5.0.0
**.Net Core App  windows Desktop support**
- Supports: **net5.0**

# Version: 3.1.0
**.Net Core App  windows Desktop support**
- Supports: **netcoreapp3.1**

**.Net Core transition version**
- NuGet.org package http://www.nuget.org/packages/MvvmBindingPack/ 
- NuGet.org package http://www.nuget.org/packages/MvvmBindingPack.Debug/ 
- Supports: **net462, net472, net48, uap10.0**
- Signed assembly;

**Dependences:**
 - None


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
 
#### Welcome to MvvmBindingPack

MvvmBindingPack is the robust MVVM framework platform for UX high-quality solutions with support of IoC/DI containers. MVVM pattern is widely used in developing of XAML-based GUI applications. It is impossible to provide the quality UX design implementations without using this pattern. Quality of UX design directly depends on the techniques or features that used for implementing the MVVM pattern. Clear separation of concerns between View (XAML code coupled with its code-behind) and View Model characterizes a profession level and quality and the of the product implementation. The package has the compatible features implementation for XAML WPF and Win Store Application XAML.
MVVM pattern in UI development process looks like a principal of "Dependency Inversion" between  View and View Model.
 

