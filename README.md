### Model - View - ViewModel binding package.

Full details package using details in MvvmBindingPack.2.5.pdf
**.Net Core transition support**
Version: 2.5.0
- NuGet.org package [http://www.nuget.org/packages/MvvmBindingPack.NetCore/] 
- NuGet.org package [http://www.nuget.org/packages/MvvmBindingPack.NetCore.Debug/] 
- Supports: **net462, net472, uap10.0**
- Signed assembly;

**Dependences:**
 - None



**Microsoft.Practices.ServiceLocation Support**

Version: 2.0.1
- NuGet.org package [http://www.nuget.org/packages/MvvmBindingPack/] 
- NuGet.org package [http://www.nuget.org/packages/MvvmBindingPack.Debug/] 
- Supports: **net452, net462, net47, uap10.0**
- Signed assembly;

**Dependences:**

CommonServiceLocator  (== 1.3.0) {namespace **Microsoft.Practices.ServiceLocation**}

**Unity Container Support**

Version: 2.0.1
- NuGet.org package [http://www.nuget.org/packages/MvvmBindingPack.Unity/] 
- NuGet.org package [http://www.nuget.org/packages/MvvmBindingPack.Unity.Debug/] 
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

 

