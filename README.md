### Model - View - ViewModel binding package.

Full details of package using link: [MvvmBindingPack - Wiki ](https://github.com/Wallsmedia/MvvmBindingPack/wiki/MvvmBindingPack-v-3.0)

#### Nuget.Org
- NuGet.org package http://www.nuget.org/packages/MvvmBindingPack/ 
- NuGet.org package http://www.nuget.org/packages/MvvmBindingPack.Debug/ 

# Version: 8.0.0
**.Net Core App windows Desktop support**
- Supports: **net8.0**


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
 
 # Introduction

The decision to develop this binding package had been made in response of minimizing a cost of the quality WPF UI development. The package provides full "Dependency Inversion" between Views and ViewModels.
**MvvmBindingPack** is the robust **MVVM** framework development platform for high-quality **WPF** UX solutions, based on using of IoC/DI containers.
**MVVM** pattern is widely used in developing of XAML-based GUI applications.

It is impossible to provide the quality UX design implementations without using this pattern.
Quality of UX design directly depends on the techniques or features that used for implementing the **MVVM** pattern.

Clear separation of concerns between View (XAML code coupled with its code-behind) and View Model characterizes a profession level and quality of the product.
The package has the compatible functional features for **XAML**, **WPF**, **UWP 10**.
**MVVM** pattern provides a principal of " **Dependency Inversion**" between **View** and **View Model**.


# MvvmBindingPack Binding Elements

- [**AutoWireVmDataContext**] - XAML MVVM extension enhancer, it automatically locates a  **View Model** and sets a reference for the **DataContext** Dependency property.
- [**AutoWireViewConrols**] - XAML MVVM extension enhancer, it automatically locates and binds named **View** controls to **View Model** class members.
- [**ProcessMvvmExtensions**] - XAML attached property, a fake collection that used for processing extensions: AutoWireVmDataContext,AutoWireViewConrols.
- [**ViewModelClassAlias**] - The alias attribute, it maps a **View Model** class to a **View**. It sets an alias of a candidate type name.
- [**ViewTarget**] - The mapping target attribute, it maps a method or property name (or **x:Name** candidate) with set " **targets**" for a **View** XAML **x:Name** element.
- [**ViewXNameAlias**] - The mapping attribute that marks a filed, method or property name (or **x:Name** candidate) with set " **names**" + " **targets**" for **View** XAML control element.
- [**ViewXNameSourceTargetMapping**] - The mapping attribute, it maps/sets a field reference to **ViewXNameSourceTarget** type for a **View** XAML **x:Name** element. This class will be used to access to properties or events of the View XAML element.
- [**ViewXNameSourceObjectMapping**] - The mapping attribute, it maps/sets a field with a  reference to XAML **x:Named**.
- [**AppendViewModel**] - it supports name and target aggregations for the **View** Model**.
- [**BindEventHandler**] - XAML mark-up, AddEvents and AddPropertyChangeEvents extensions; it binds a control event to a method with a compatible signature of the object which is located in **DataContext** referenced object.
- [**BindEventHandlerIoc**] - XAML mark-up, AddEvents and AddPropertyChangeEvents extensions; it binds a control event to a method with a compatible signature of the object which is located in a type resolved via the **IoC** container.
- [**BindEventHandlerResource**] - XAML mark-up, AddEvents and AddPropertyChangeEvents extensions; it binds control events to a method with a compatible signature of the object which is located in **Resources**.
- [**BindCommand**] - XAML mark-up and BindToCommand extensions; it binds binds a control command property to methods with using **ICommand** interface compatible signature methods.
- [**BindCommandIoc**] - XAML mark-up and BindToCommand extensions; it binds binds a control command property to methods with using **ICommand** interface compatible signature methods.
- [**BindCommandResource**] - XAML mark-up and BindToCommand extensions; it binds binds a control command property to methods with using **ICommand** interface compatible signature methods.
- [**IocBinding**] - XAML mark-up and AssignProperties extensions; it binds to **IoC** container elements.
- [**LocateDataContext**] - XAML mark-up and AssignProperties extensions; it finds in the chain of **DataContext** objects, the first, which contains the exact method or property. It comes through the parent elements of logical and visual trees.
- [**BindXAML.AddEvents**] - XAML attached property, a fake collection that used for processing extensions: BindEventHandler, BindEventHandlerIoc, and BindEventHandlerResource.
- [**BindXAML.AssignProperties**] - XAML attached property, a fake collection that used for processing IocBinding and LocateDataContext extensions.
- [**BindXAML.BindToCommand**] - XAML attached property, a fake collection that used for processing extensions: BindCommand, BindEventHandlerIoc, and BindEventHandlerResource.
- [**BindXAML.AddPropertyChangeEvents**] - XAML attached property, fake collect **i** on, is used for processing extensions: BindEventHandler, BindEventHandlerIoc, It binds a **View** dependency property change event handler** to the event handler in the View Model. It is supported only for WPF.

# Setup DI Container 

**Add DotNet Core DI reference in the WPF csproj:**
``` xml

  <ItemGroup Condition="'$(DisableImplicitFrameworkReferences)' != 'true' And '$(TargetFrameworkIdentifier)' == '.NETCoreApp' And '$(_TargetFrameworkVersionWithoutV)' >= '3.0'">
    <FrameworkReference Include="Microsoft.AspNetCore.App" IsImplicitlyDefined="true" />
  </ItemGroup>

```

**Setup DotNet Core DI container example for App.xaml.cs:**
``` CSharp
private void Application_Startup(object sender, StartupEventArgs e)
{

    ServiceCollection services = new ServiceCollection();
    services.AddSingleton<AutoBindingViewModel>();
    services.AddSingleton<IocBindingViewModel>();
    AutoWireVmDataContext.ServiceProvider = services.BuildServiceProvider();
}
```

# **AutoWireVmDataContext**

XAML MVVM extension enhancer, it automatically locates and sets(binds) and sets a reference for the **DataContext** Dependency property.

**Example:**
``` xml

   <mvvm:BindXAML.ProcessMvvmExtensions>
        <mvvm:AutoWireVmDataContext />
        <mvvm:AutoWireViewConrols />
    </mvvm:BindXAML.ProcessMvvmExtensions>

```

**Properties:**
- **ViewModelNamespaceOverwrite** - Overwrites the **x:Classnamespace**; it will be used for exact defining of the **View Model** type namespace. Original, the **x:Class** namespace will be ignored.

- **ViewModelNameOverwrite** - Overwrites the **x:Classname**; it will be used for exact **View Model** type name. Original, the **x:Class** name will be ignored.

- **TargetPropertyName** - The target dependency property name. Default value is **"DataContext"**.
   It will be set to a resolved reference to a **View Model**.

- **UseTheFirstOne** - If it is set to **'true' (default)**, it limits the types of **x:Class** 
  and  **x:Name**  to the first found control in the logical tree.

- **ResolveIocContainer** - If it is set to **'true'**, the IoC container will be used to resolve a **View Model** type instance. It has the first priority. **Default value is true**.

- **ResolveResources** - If it is set to **'true'**, the static Resources will be used to resolve a **View** Model** instance. It has the second priority.  **Default value is true**.

- **ResolveCreateInstance** - If it is set to **'true'**, the static CLR Activator will be used to create a **View Model** instance. It has the third priority.  **Default value is true**.

- **UseMaxNameSubMatch** -   Defines the additional sub matching ("start with") rule when a  **View Model**
   expected name compared to a View Model candidate name. If it is set to **'true'**, the **View Model candidate** name is considered as a match to a name if starts with '**View Model expected name**'. 
  
  **Example**:

``` xml

  <mvvm:BindXAML.ProcessMvvmExtensions>
    <mvvm:AutoWireVmDataContext  UseMaxNameSubMatch="True"/>
    <mvvm:AutoWireViewConrols />
  </mvvm:BindXAML.ProcessMvvmExtensions>
 
  The View Model expected name is FrameCapturePrice.  With UseMaxNameSubMatch="True" it will match 
  to the name FrameCapturePrice_Var1.
```

- **ViewsNamespaceSuffixSection** -  Defines the Views namespace section suffix (default " **Views**"). 
  It will be replaced (if it is exist) on the **ViewModelsNamespaceSuffixSection** property value. 
  It is ignored when the **ViewModelNamespaceOverwrite** is set.

- **ViewModelsNamespaceSuffixSection** - Defines the View Models namespace section suffix  (default " **ViewModels**"). 
  It will be used as a replacement for **ViewsNamespaceSuffixSection**. 
  It is ignored when the **ViewModelNamespaceOverwrite** is set.
 
  **Example**: 
  ```
  the namespace: Trade.GUI.Application.Views ===>  Trade.GUI.Application.ViewModels
  the namespace: Trade.GUI.Application       ===>  Trade.GUI.Application.ViewModels
  ```

  **Example**: 
``` xml

  <mvvm:BindXAML.ProcessMvvmExtensions>
    <mvvm:AutoWireVmDataContext ViewsNamespaceSuffixSection="Pages"
                                ViewModelsNamespaceSuffixSection="PageModels" />
    <mvvm:AutoWireViewConrols />
  </mvvm:BindXAML.ProcessMvvmExtensions>

    the namespace: Trade.GUI.Application.Pages ===>  Trade.GUI.Application.PageModels  
    the namespace: Trade.GUI.Application       ===>  Trade.GUI.Application.PageModels
```

- **OldViewNamePart** -  Defines the part of the class type name (default " **View**"). If it is exist, it will be replaced on the value of the property "NewViewModelNamePart". It is ignored when the "ViewModelNameOverwrite" is set. 

- **NewViewModelNamePart** - Defines the part of the class type name (default " **ViewModel**").It is ignored when the "ViewModelNameOverwrite" is set.

  
  **Example**:
```  
  the name "MainPageView"        ===>  "MainPageViewModel;
  the name "MainPageViewFrame_1" ===>  "MainPageViewModelFrame_1";
  the name "MainPage"            ===>  "MainPage".
```
- **IncludeInterfaces** - If it is set to 'true', there will be included interfaces from the loaded assemblies 
  into the list of type candidates. Default value is **true**. 
  It allows to use the interfaces in **ViewModelNameOverwrite** and resolve them via **IoC** container.

- **IocXName** - Default value is **false.** f it is set to 'true', 
  the  IoC type will be attempted to be resolved with using type of **x:Name** value.


## View to View Model mapping rules.

AutoWireVmDataContext setups a **DataContext** dependency property with a reference to a **View Model** class instance.
The name of the target dependency property can be defined via property "**TargetPropertyName**".
The **AutoWireVmDataContext** binding logic a View  to a View Model is based on using information from the **x:Name** and **x:Class** of XAML directives:

- **x:Name** directive uniquely identifies XAML-defined elements in a XAML namescope.
- **x:Class** directive configures XAML markup compilation to join partial classes between markup and code-behind and it has the type namespace.
 The namespace will be used to construct expected types.

The **View** (XAML) logical tree will be scanned. The non-"System.", non-"Microsoft.",
or other non - WPF class types will be filtered in.
For each "DependencyObject" view subclass, the "x:Name" property value will be extracted.
In the result, the list of types (namespace + name) (**x:Class**)  and names (**x:Name** if it was set) will be created.
For each element in the list will be applied transformation rules in order to construct the **View Model** **expected** type list.
 
The **candidate** type list will be obtained from loaded assemblies. It will be used for mapping **View** (expected) to **View Model** (candidate) types.

## General rules for forming View Model expected type names:

- If the **View** type namespace suffix section contains a " **Views**"(default see prop. ViewsNameSpaceSuffixSection),
 this section will be replaced on " **ViewModels**"(default see prop.ViewModelsNameSpaceSuffixSection). It forms "**expected namespace**".

  Examples of namespaces transformation into "**expected namespace**":

```
   Trade.SuperUI.Views       ==> Trade.SuperUI.ViewModels
   Trade.SuperUI.Views.Views ==> Trade.SuperUI.Views.ViewModels
   Trade.SuperUI.RViews      ==> Trade.SuperUI.RViews
```

- If the View type namespace suffix section doesn't contains a **Views** suffix section and the namespace has only **one or two** sections ,
 in this case the suffix section **ViewModels** (default see prop.ViewModelsNameSpaceSuffixSection) will be added. It forms "**expected namespace**".
  
  Example namespace transformation into " **expected namespace**":

``` 
  Trade.TicketPanel ==> Trade.TicketPanel.ViewModel
  Trade             ==> Trade.ViewModel
```

- If a type name (i.e. x:Class name) or **x:Name** contains " **View**" substring (default see prop ."OldViewNamePart"), it will be replaced all occurrence on " **ViewModel**" substring (default see prop. "NewViewModelNamePart"). They form a pair of " **expected type names**".
 + Example name transformation into " **expected type name**":
   Ticket**View**Panel ::=[map]=> Ticket**ViewModel**Panel ,but (!)
   Trade**View**TicketViewPanel ::=[map]=>; Trade**ViewModel**Ticket**ViewModel**Panel

- The " **expected fully qualified type names**" will be formed from the parts " **expected namespace**" 
-and " **expected type names**" from **x:Class** name and **x:Name**
- Formed from **x:Name** the "expected fully qualified type name" will have a priority over the **x:Class** formed type name.
- The list of candidate types and interfaces (see IncludeInterfaces) will be obtained from all loaded assemblies by filtering
 with "**expected namespace**". Each candidate type name will be examined on best matching to "**expected name**".
- Each possible candidate name will be split into a cased parts and matched against "desired name candidate" parts.
- The first candidate type with the full parts match will be selected.
- If you set "**UseMaxNameSubMatch**" flag true, the first candidate with a sub-match type name will selected.

## Obtain an Instance of the View Model type.

Type will be resolved in the sequence: IoC container, Resources and Activator.CreateInstance().
For controlling see properties "**ResolveIocContainer**","**ResolveResources**" and "**ResolveCreateInstance**".  
In success the resolved type will set as value to " **DataContext**" dependency property (set by default "**TargetPropertyName**") 
and attached property **BindXAML.AutoWiredViewModel**.
The order of resolving via the **IoC,** hosted via adapter that implements ServiceLocation interface:

- GetInstance(locatedItem_WiringType);
**    !or type will be created by Activator.**

Order of resolving via the Resource Locator:
- LocateResource(XName); or
- LocateResource(locatedItem_WiringType.Name); or
- LocateResource(locatedItem_WiringTType.FullName); or
- LocateResource(locatedItem_WiringType).

## Examples of XAML and View Model

#### Example of x:Class mapping

**XAML "View" fragment example:**
``` XML
<Window x:Class="WpfDemoAutoWire.Views.WindowAutoBind"
       xmlns:mark="MvvmBindingPack"
…...
        x:Name="WindowView"
        Title="AutoWireVmDataContext AutoWireViewConrols" Height="350" Width="300">

    <mark:BindXAML.ProcessMvvmExtensions>
        <mark:AutoWireVmDataContext/>
        <mark:AutoWireViewConrols/>
    </mark:BindXAML.ProcessMvvmExtensions>
```

**C# "View Model" fragment example:**

``` C#
namespace WpfDemoAutoWire.ViewModels
{
   publicclassWindowAutoBind : NotifyChangesBase
   {

   }
}
```

**Order of resolving via the IoC:**

- GetInstance(typeof(WpfDemoAutoWire.ViewModels.WindowAutoBind)).

**Order of resolving via the Resource Locator:**

- LocateResource("WindowView"); or
- LocateResource("WindowAutoBind"); or
- LocateResource("WpfDemoAutoWire.ViewModels.WindowAutoBind"); or
- LocateResource(typeof(WpfDemoAutoWire.ViewModels.WindowAutoBind)).



#### Example of using overwrite properties.

**XAML "View" fragment example:**
``` xml
<Window x:Class="WpfDemoAutoWire.Views.WindowAutoBind"
        xmlns:mark="MvvmBindingPack"
…...
        x:Name="WindowTrade"
        Title="AutoWireVmDataContext AutoWireViewConrols" Height="350" Width="300">

    <mark:BindXAML.ProcessMvvmExtensions>
        <mark:AutoWireVmDataContext  ViewModelNamespaceOverwrite="WpfDemo.AAA.FFF" ViewModelNameOverwrite="ICustomTrade"/>
        <mark:AutoWireViewConrols/>
    </mark:BindXAML.ProcessMvvmExtensions>
```

**C# "View Model" fragment example:**
``` c#
namespace WpfDemo.AAA.FFF
{
    publicclassAsdfgBertbind : NotifyChangesBase, ICustomTrade
    {

    }
}
```

**Order of resolving via the IoC:**

- GetInstance(typeof(WpfDemo.AAA.FFF.ICustomTrade)).

**Order of resolving via the Resource Locator:**

- LocateResource("WindowTrade"); or
- LocateResource("ICustomTrade"); or
- LocateResource("WpfDemo.AAA.FFF.ICustomTrade"); or
- LocateResource(typeof(WpfDemo.AAA.FFF.ICustomTrade)).

#### Example

**XAML "View" fragment example:**
``` xml
<Window x:Class="WpfDemoAutoWire.Views.WindowBind"
        xmlns:mark="MvvmBindingPack"
…...
        x:Name="WindowAutoBindView"
        Title="AutoWireVmDataContext AutoWireViewConrols" Height="350" Width="300">
    <mark:BindXAML.ProcessMvvmExtensions>
        <mark:AutoWireVmDataContext/>
        <mark:AutoWireViewConrols/>
    </mark:BindXAML.ProcessMvvmExtensions>
```

**C# "View Model" fragment example:**

``` c#
namespace WpfDemoAutoWire.ViewModels
{
    publicclassWindowAutoBindViewModel: NotifyChangesBase
    {
    }
}
```

#### Example

**XAML "View" fragment example:**
``` xml
<Window x:Class="WpfDemoAutoWire.Views.WindowBind"
        xmlns:mark="MvvmBindingPack"
…...
        x:Name="WindowA"
        Title="AutoWireVmDataContext AutoWireViewConrols" Height="350" Width="300">
    <mark:BindXAML.ProcessMvvmExtensions>
        <mark:AutoWireVmDataContext/>
        <mark:AutoWireViewConrols/>
    </mark:BindXAML.ProcessMvvmExtensions>
```

**C# "View Model" fragment example:**

``` c#
namespace WpfDemoAutoWire.ViewModels
{
    [ViewModelClassAlias("WindowA")]
    publicclassWindowAbracadabra: NotifyChangesBase
    {

    }
}
```

#### Example

**XAML "View" fragment example:**
``` xml
<Window x:Class="WpfDemoAutoWire.WindowBind"
        xmlns:mark="MvvmBindingPack"
…...
        x:Name="WindowA"
        Title="AutoWireVmDataContext AutoWireViewConrols" Height="350" Width="300">
    <mark:BindXAML.ProcessMvvmExtensions>
        <mark:AutoWireVmDataContext/>
        <mark:AutoWireViewConrols/>
    </mark:BindXAML.ProcessMvvmExtensions>
```

**C# "View Model" fragment example:**

``` c#
namespace WpfDemoAutoWire.ViewModels
{
    [ViewModelClassAlias("WindowBind ")]
    publicclassWindowAbracadabra: NotifyChangesBase
    {

    }
}
```



# **AutoWireViewConrols**

XAML MVVM extension enhancer, it automatically locates and binds/wires the View controls  to View Model class members.

**Example:**
``` xml

   <mvvm:BindXAML.ProcessMvvmExtensions>
        <mvvm:AutoWireVmDataContext />
        <mvvm:AutoWireViewConrols />
    </mvvm:BindXAML.ProcessMvvmExtensions>

```

**Properties:**

- **KnownExcludeMethodPrefixes** -  The default static string collection contains the prefixes of the internal, auxiliary class methods that should be ignored when they are reflected from the **View Model** class type. Default set is {"get\_", "set\_", "add\_", "remove\_", "GetFieldInfo", "FieldGetter", "FieldSetter", "MemberwiseClone", "Finalize", "GetType", "GetHashCode", "ReferenceEquals", "Equals", "ToString"}.
- **Source** - Gets or sets the object to use as the wiring source i.e. **View Model** instance. It has priority over 'SourcePropertyName'. It is a "back-door" feature which allows to setup the source object. If it is not set on, by default, the markup extension will use the defined **DataContext** property value or other property redefined by 'SourcePropertyName'. There may be used {IocBinding ...} or other "agnostic" mark up extension(not {Binding ...}) which provides by the independent way to a source object.
- **SourcePropertyName** -  Source dependency property name. The property value will be used as a reference to the **View Model** object. Default dependency property name is " **DataContext**".
- **UseMaxNameSubMatch** - Defines the additional sub matching rule when a expected view name  (x:Name without targets) compared to a view model candidate name. If it is true, the view model candidate name is considered as a match to a view expected name which starts with the 'view expected name'. Example: view name "WindowAutoBindViewModel" match to view modelName "WindowAutoBindViewModelSubMath".
- **IncludeVisualTreeNames** - Include visual tree x:Named elements onto wiring. Default value is false.

## Name "parts" split and matching rules

For comparing to names is used case sensitive name part matching algorithm.
It allows to add more flexibility in forming and using View Model naming conventions.

## General rules to form name "parts"

1. The name is split into parts by capital letter or '\_'. The character '\_' is not included into parts.
2. The name parts are considered as a case sensitive.
3. The names are considered as matched if they have the same consequential set of parts.
4. The name is considered as sub-match if it has been started at least one or more the same consequential parts.

**Examples of splitting:**
+ The View name " **\_Example\_Name\_**" will split into parts {" **Example**"," **Name**"}.
+ The View name " **ExampleName**" will split into parts {" **Example**"," **Name**"}.
+ The View name " **exampleName\_Ver1**" split into parts {" **example**"," **Name**"," **Ver1**"}.

**Examples of matching:**
+ "**Example_Name\_**" and " **ExampleName**" and " **Example\_\_\_Name\_**" are match because they have the same set of parts.

**Examples of sub-matching:**
+ "**Example\_Name\_**" and " **ExampleName\_Ver**" has a sub-match with rank 2 of the same set of parts.

## View to View Model controls binding/wiring.

The AutoWireViewConrols logic is based on using of the **x:Name** directive. **x:Name** directive uniquely 
identifies XAML elements in a XAML namescope.  AutoWireViewConrols wires and binds x:Named XAML elements
or View XAML UI elements to View Model properties, methods and fields. The View (XAML) element targets 
are dependency properties or routing events. They are subject of binding to properties, fields and methods in a View Model class.

## View Control General Wiring and Binding rules:

1. One x:Named **View** (XAML) element can be bind one to many distinguish properties, fields or methods, in a **View** Model**.
2. The **View** Model** properties has a priority to bind over the methods with the same binding name.
3. The first found match will be bind fist. The order of the declaration is not applicable in ambiguous cases.
4. It is used always the full name match of "parts", a part-sub match can be used as an option, see the [**UseMaxNameSubMatch**] property.
5. **One to One** : The **View** Model** property or event can be bind only once for one x:Named View XAML element.
6. **View** element targets (dependency properties, routing events) will be bind to **View** Model** element targets.
7. The element target names should be defined in the **View** Model**.
8. The **View** Model** element names without targets will be ignored.
9. The **x:Name** is ignored if it starts with "\_".
10. The attached property or event name should be set in format " **TypeOwner.Name**". 
    - Example: "**Grid.Row**", " **Mouse.MouseMove**" with using attributes:[**ViewXNameAlias**], [**ViewTarget**].

## Examples of wiring the View Model method to the View event.

**Wiring goal is to wire the View element event "Click"  to a method handler in the View Model:**
``` xml
<Buttonx:Name="Example_Name_" …>   .
``` 
**The View Model wiring C# definition variants:**

### Solution Without any attributes
``` c#
void  Example_Name_Click(object sender, RoutedEventArgs e){} // or;
void  ExampleName_Click(object sender, RoutedEventArgs e){}
```

### Solution With attribute [ViewTarget (...)]

``` c#
[ViewTarget("Click")]
void  ExampleName_Clk(object sender, RoutedEventArgs e){}//  or;

[ViewTarget("Click")]
void  ExampleName_Other(object sender, RoutedEventArgs e){}
```

### Solution With attribute [ViewXNameAlias (...)]

``` c#
[ViewXNameAlias("ExampleName","Click")]
void  AbracadbraName(object sender, RoutedEventArgs e){}//  or;

[ViewXNameAlias("Example_Name","Click")]
void  _AbracadbraName(object sender, RoutedEventArgs e){}

/* the name starting with "_" will be ignored, but the attribute don't */ or;

[ViewXNameAlias("Example_Name_","Click")]
void  Abracadbra_Name(object sender, RoutedEventArgs e){}
```

## Examples of binding the View Model property to the View property

**Binding goal is to bind the View element property "Content" to a property in the View Model:**

``` xml
<Label x:Name="Example_Name" ..>  and 
```


### Solution Without any attributes
``` c#
string  Example_Name_Content {get;set;}  or;
string  ExampleName_Content {get;set;}
```

### Solution With  attribute [ViewTarget (...)]
``` c#
[ViewTarget("Content")]
string  Example_Name {get;set;}  or;

[ViewTarget("Content")]
string  ExampleName_BadTag {get;set;}
```

### Solution With attribute [ViewXNameAlias (...)]
``` c#
[ViewXNameAlias("ExampleName","Content")]
string  AbracadbraName{get;set;}    or;

[ViewXNameAlias("Example_Name","Content")]
string  _AbracadbraName{get;set;}

/* the name starting with "_" will be ignored, but the attribute don't*/ or;
[ViewXNameAlias("Example_Name_","Content")]
string  Abracadbra_Name{get;set;}
```

## Examples of wiring the View Model method to the View "Command" property

**Wiring goal is to wire the View element property  "Command"  to  methods in the View Model.**
``` xml
 <Buttonx:Name="Example_Name_" …> 
```

### Solution Without any attributes
``` c#
ICommand  Example_Name_Command {get;set;}  or;

ICommand  ExampleName_Command {get;set;}
```

### Solution With  attribute [ViewTarget (...)]
``` c#
[ViewTarget("Command")]
ICommand  Example_Name {get;set;}  or;

[ViewTarget("Command")]
ICommand  ExampleName_BadTag {get;set;}
```

### Solution With attribute [ViewXNameAlias (...)]
``` c#
[ViewXNameAlias("ExampleName","Command")]
ICommand  AbracadbraName{get;set;}    or;

[ViewXNameAlias("Example_Name","Command")]
ICommand  AbracadbraName{get;set;}

/* the name starting with "_" will be ignored, but the attribute don't */ or;
[ViewXNameAlias("Example_Name_","Command")]
ICommand  Abracadbra_Name{get;set;}
```

### Solution With attribute [ViewXNameAlias (...)] and separated "Execute" and "CanExecute" wiring.

``` c#
[ViewXNameAlias("Example_Name", "Command.Execute")]
void NameVM2MExecute(object obj){...}

[ViewXNameAlias("Example_Name", "Command.CanExecute")]
bool Method_NameVM2MCanExecute(object obj){....} or;

[ViewXNameAlias("Example_Name", "Command.CanExecute")]
bool Prop_NameVM2MCanExecute{get;set;}
```

## Examples of wiring(just copy) the View Model fields to the View property

**Wiring goal is to wire the View element property  "Label"  to  fields (just copy from) in the View Model.**

``` xml
<Foo x:Name="Example_Name_" …> 
```

### Solution With attribute [ViewXNameAlias (...)]
``` c#
[ViewXNameAlias("ExampleName","Label")]
string _textAndMsgLabelTxtC = "Content was copied from the field";
/*the field name will always be ignored.*/

```

## Examples of wiring/referencing the View fields into the View Model

Sometimes, very often there is a vital case to have a link from View Model to a View element or property or event.

### Get a reference/link to the element type like:
``` xml
<Label x:Name="LabelXNameVM2M" ..>
```

**The View Model wiring C# definition variants:**
``` c#
[ViewXNameSourceObjectMapping("LabelXNameVM2M")]
privateobject _LabelXNameVM2M; // can be used the 'Label' type  instead of the 'Object' type.
```

### Get a reference/link to the property "Content" of the element type like

``` xml
<Label  x:Name="LabelXNameVM2M"  ..>
```

**The View Model wiring C# definition variants:**
``` c#
[ViewXNameSourceTargetMapping("LabelXNameVM2M", "Content")]
privateViewXNameSourceTarget _LabelXNameVM2MContent;
```


# **ViewModelClassAlias**

The mapping attribute that adds to a class the extra alias "candidate type names".It is used to map a **View** onto a **View** Model**.

**C# "View Model" fragment example:**
``` c#

    [ViewModelClassAlias("WindowAutoBindView")]
    [ViewModelClassAlias("WindowAutoBindViewModel")]
    [ViewModelClassAlias("WindowAutoBindViewModelSubMath")]
    [ViewModelClassAlias("WindowListboxSubMath")]
    [ViewModelClassAlias("WindowBindView")]
    [ViewModelClassAlias("WindowMainView")]
    publicclassWindowAutoBind : NotifyChangesBase
    {

    }
```

#  **ViewTarget**

The mapping attribute that marks a method or property name (or **x:Name** candidate) with set " **targets**" for a **View** XAML **x:Name** element.

**C# "View Model" fragment example:**
``` C#
[ViewTarget("Click")]
void  ExampleName_Clk(object sender, RoutedEventArgs e){}  or;

[ViewTarget("Click")]
void  ExampleName_Other(object sender, RoutedEventArgs e){}

[ViewTarget("Content")]
string  Example_Name {get;set;}  or;

[ViewTarget("Content")]
string  ExampleName_BadTag {get;set;}
```


#  **ViewXNameAlias**

The mapping attribute that marks a filed, method or property name (or **x:Name** candidate) with set alias " **names**" + " **targets**" for **View** XAML **x:Name** element.

**Properties:**

- **BindingMode** - Gets or sets a value that indicates the direction of the data flow in the binding.
- **HandledEventsToo** - If it is true to register the handler such that it is invoked even when the routed event is marked handled in its event data.
- **ValidatesOnDataErrors** - The DataErrorValidationRule is a built-in validation rule that checks for errors that are raised by the IDataErrorInfo implementation of the source object.
- **ValidatesOnExceptions** - The ExceptionValidationRule is a built-in validation rule that checks for exceptions that are thrown during the update of the source property.
- **ValidatesOnNotifyDataErrors** - When ValidatesOnNotifyDataErrors is true, the binding checks for and reports errors that are raised by a data source that implements INotifyDataErrorInfo.

**C# "View Model" fragment example:**

``` c#
[ViewXNameAlias("ExampleName","Content")]
string  AbracadbraName{get;set;}    or;

[ViewXNameAlias("Example_Name","Content")]
string  _AbracadbraName{get;set;}

/* the name starting with "_" will be ignored, but the attribute don't */ or;

[ViewXNameAlias("Example_Name_","Content")]
string  Abracadbra_Name{get;set;}

[ViewXNameAlias("LabelXNameC", "Content")]
string _textAndMsgLabelTxtC = "Content was binded - C";

[ViewXNameAlias("ExampleName","Command")]
ICommand  AbracadbraName{get;set;}    or;

[ViewXNameAlias("Example_Name","Command")]
ICommand  _AbracadbraName{get;set;}

/* the name starting with "_" will be ignored, but the attribute don't */ or;
[ViewXNameAlias("Example_Name_","Command")]
ICommand  Abracadbra_Name{get;set;}
```

**With Separate "Execute" and "CanExecute" wiring.**

``` c#
[ViewXNameAlias("Example_Name", "Command.Execute")]
void NameVM2MExecute(object obj){...}

[ViewXNameAlias("Example_Name", "Command.CanExecute")]
bool Method_NameVM2MCanExecute(object obj){....} or;

[ViewXNameAlias("Example_Name", "Command.CanExecute")]
bool Prop_NameVM2MCanExecute{get;set;}

[ViewXNameAlias("LabelXName", "Content", ValidatesOnNotifyDataErrors = true)]
publicstring KadLabelXNameD1
{

    get { return _textAndMsgLabelTxtF; }
    set { _textAndMsgLabelTxtF = value; NotifyPropertyChanged(); }
}
```

#  **ViewXNameSourceTargetMapping**

The mapping attribute that marks a field reference to ViewXNameSourceTarget type for a View XAML **x:Name** element. This class is used to access to properties or events of the View XAML element.

**C# "View Model" fragment example:**

``` c#
[ViewXNameSourceTargetMapping("LabelXNameVM2M", "Content")]
privateViewXNameSourceTarget _LabelXNameVM2MContent;

[ViewXNameSourceTargetMapping("ButtonXNameVM2M", "Click")]
privateViewXNameSourceTarget _ButtonXNameVM2MClick;
```

# **ViewXNameSourceObjectMapping**

The mapping attribute that marks the field of the any type where the reference to XAML x:Named element will be set to.

**C# "View Model" fragment example:**
``` c#
[ViewXNameSourceObjectMapping("LabelXNameVM2M")]
privateLabel _LabelXNameVM2M;

[ViewXNameSourceObjectMapping("ButtonXNameVM2M")]
privateButton _ButtonXNameVM2M;
```

#  **AppendViewModel**

The mapping attribute that appends(extends) the bindings list of wiring candidates with another reference type object members. Value type, "boxed value type" and types started with " **System**" .. " **MicroSoft**" will be ignored. The members are appended to a list of wiring candidates. They have a low priority.Recursive view model appending is not supported.

- **AppendViewModel** the mapping attribute that appends(extends) the binding list now is supported with:
  - BindEventHandler
  - BindCommand
  

**C# "View Model" fragment Example:**

``` c#
namespaceWpfDemoAutoWire.ViewModels
{
    publicclassWindowAutoBind : NotifyChangesBase
    {
        [AppendViewModel]
        privateAppendedViewModel1 _appendedViewModel1;

        [AppendViewModel]
        publicAppendedViewModel2  AppendedViewModel2 {get; set;};
    }
}
```

# **ProcessMvvmExtensions**

# **BindXAML.ProcessMvvmExtensions**

XAML attached property, fake collection, that used for processing extensions: AutoWireVmDataContext, AutoWireViewConrols.

**XAML "View" fragment example:**

``` xml
<vm:BindXAML.ProcessMvvmExtensions>

    <vm:AutoWireVmDataContext/>

    <vm:AutoWireViewConrols/>

</vm:BindXAML.ProcessMvvmExtensions
```



# **BindEventHandler**

XAML mark-up, BindXAML.AddEvents and BindXAML.AddPropertyChangeEvents extensions; it binds a control event to a method with a compatible signature of the object which is located in DataContext referenced object.

**Properties**:

- Source (default key) – It is a "back-door" feature which allows to setup the source object. If it is not set on, by default, the markup extension will use the defined DataContext property value. It is referring to the source object which has the method or property used by the markup extension.There may be used {IocBinding ...} or other "agnostic" mark up extension(not {Binding ...}) which provides by the independent way to a source object.
- MethodName – The method name of the source object  that has …EventHandler delegate signature (can be static).It's mutually exclusive versus PropertyName.
- PropertyName – The property name of the source object that contains …EventHandler delegate (can be static). It's mutually exclusive versus MethodName.

- TargetEventName (external key, used by AddEvents) – The key is used to pass a target event name to the AddEvents attached property collection.
- TargetPropertyName (external key, used for AddPropertyChangeEvents) – The key is used to pass a target property name to the AddPropertyChangeEvent.

- DeepScanAllTrees- If it is set to "true", all DataContext properties in the logical tree will be scanned until the math to a property or method name (PropertyName, MethodName). Smart feature allows to ignore the current DataContext property value and traverse to other parent DataContext value.If set on true, it will cause to scan for the DataContext property objects over the trees and get the first one that contains the binding property or method. It used in case when there is need to ignore the binding ItemsSource DataContext for the ItemsControl item, just bind a Button to a View Model for the item of the ListView or so on.

[**AppendViewModel**] - the mapping attribute that appends(extends) the binding list now is supported.



**XAML(WPF) "View" fragment example of using with** MethodName **:**
``` xml
<Button Content="Button Click Method"
        Click="{vm:BindEventHandler MethodName=ButtonClickMethod}"/>
```

**XAML(WinRt+WPF) "View" fragment example of using with** MethodName **:**

``` xml
<Button Content="Button Click Method" >
      <vm:BindXAML.AddEvents>
          <vm:BindEventHandler MethodName="ButtonClickMethod" TargetEventName="Click"/>
      </vm:BindXAML.AddEvents>
</Button>
```

**XAML(WPF) "View" fragment example of using with** PropertyName **:**

``` xml
<Button Content="Button Click Method via PropertyName"
        Click="{vm:BindEventHandler PropertyName=ButtonClickProperty}"/>
```

**XAML(WinRt+WPF) "View" fragment example of using with** PropertyName **:**

``` xml
<Button Content="Button Click Method via PropertyName" >
      <vm:BindXAML.AddEvents>
          <vm:BindEventHandler PropertyName="ButtonClickProperty" TargetEventName="Click"/>
      </vm:BindXAML.AddEvents>
</Button>
```

**C# "View Model" fragment example:**
``` c#
public ViewModelNew()
{
    _buttonClickPropDelegate = newRoutedEventHandler(ButtonClickMethod);
}

privateRoutedEventHandler _buttonClickPropDelegate;

publicRoutedEventHandlerButtonClickProperty
{
    get { return _buttonClickPropDelegate; }
}

publicvoidButtonClickMethod(object sender, RoutedEventArgs e)
{

}
```

**XAML(WPF) "View" fragment example of using with** TargetPropertyName;

** it subscribes to the Dependency property change events:**
``` xml
<Label Content="Button Click Method via PropertyName" >
      <vm:BindXAML.AddPropertyChangeEvents>
          <vm:BindEventHandler MethodName="DataContextChanged"
                        TargetPropertyName="DataContext"/>
      </vm:BindXAML.AddPropertyChangeEvents>
</Label>
``` 

**C# "View Model" fragment example of using with** TargetPropertyName;
**it subscribes to the Dependency property change events:**

``` c#
publicvoidDataContextChanged(object sender, EventArgs e)
{

}
```


# **BindEventHandlerIoc**

XAML mark-up, BindXAML.AddEvents and BindXAML.AddPropertyChangeEvents extensions; it binds a control event to a method with a compatible signature of the object which is located in the IoC container.

**Properties**:

- **ServiceType** (default key) –The type (System.Type) or the type name (System.String) of the requested object.
- **MethodName** – The method name of the source object  that has …EventHandler delegate signature (it can be static).It's mutually exclusive versus PropertyName.
- **PropertyName** – The property name of the source object  that contains …EventHandler delegate (it can be static). It's mutually exclusive versus MethodName.
- **TargetEventbName** (external key, used by AddEvents) – The key is used to pass a target event name to the AddEvents.
- **TargetPropertyName** (external key, used for AddPropertyChangeEvents) – The key is used to pass a target property name to the AddPropertyChangeEvent.

**XAML(WPF) "View" fragment example of using with** MethodName **:**
``` xml
<Button Content="Button Click Method"
        Click="{vm:BindEventHandlerIoc ServiceType=ViewModels.ViewModelNew,
                MethodName=ButtonClickMethod}"/>
```

**XAML(WinRt+WPF) "View" fragment example of using with** MethodName **:**
``` xml
<Button Content="Button Click Method" >
      <vm:BindXAML.AddEvents>
          <vm:BindEventHandlerIoc ServiceType=ViewModels.ViewModelNew,
              MethodName="ButtonClickMethod" TargetEventName="Click"/>
      </vm:BindXAML.AddEvents>
</Button>
```

**XAML(WPF) "View" fragment example of using with** PropertyName **:**
``` xml
<Button Content="Button Click Method via PropertyName"
        Click="{vm:BindEventHandlerIoc ServiceType=ViewModels.ViewModelNew,
                PropertyName=ButtonClickProperty}"/>
```

**XAML(WinRt+WPF) "View" fragment example of using with** PropertyName **:**

``` xml
<Button Content="Button Click Method via PropertyName" >
      <vm:BindXAML.AddEvents>
          <vm:BindEventHandlerIoc ServiceType="ViewModels.ViewModelNew"
               PropertyName="ButtonClickProperty" TargetEventName="Click"/>
      </vm:BindXAML.AddEvents>
</Button>
```


**C# "View Model" fragment example:**

``` c#
public ViewModelNew()
{
    _buttonClickPropDelegate = newRoutedEventHandler(ButtonClickMethod);
}

privateRoutedEventHandler _buttonClickPropDelegate;

publicRoutedEventHandlerButtonClickProperty
{
    get { return _buttonClickPropDelegate; }
}

publicvoidButtonClickMethod(object sender, RoutedEventArgs e)
{

}
```

**XAML(WPF) "View" fragment example of using with** TargetPropertyName;

** it subscribes to the Dependency property change events:**
``` xml
<Label Content="Button Click Method via PropertyName" >
      <vm:BindXAML.AddPropertyChangeEvents>
          <vm:BindEventHandlerIoc ServiceType="ViewModels.ViewModelNew"
                MethodName="DataContextChanged" TargetPropertyName="DataContext"/>
      </vm:BindXAML.AddPropertyChangeEvents>
</Label>
```

**C# "View Model" fragment example of using with** TargetPropertyName;

** it subscribes to the Dependency property change events:**

``` c#
publicvoidDataContextChanged(object sender, EventArgs e)
{

}
```

# **BindEventHandlerResource**

XAML mark-up, BindXAML.AddEvents and BindXAML.AddPropertyChangeEvents extensions; it binds control events to a method with a compatible signature of object which is located in Resources.

**Properties**:

- **ResourceKey** (default key) – Sets the key value to a static resource. The key is used to return the object matching that key in the resource dictionaries.
- **MethodName** – The method name of the source object that has …EventHandler delegate signature (it can be static).It's mutually exclusive versus PropertyName.
- **PropertyName** – The property name of the source object that contains …EventHandler delegate (it can be static). It's mutually exclusive versus MethodName.
- **TargetEventName** (external key, used by AddEvents) – The key is used to pass a target event name to the AddEvents.
- **TargetPropertyName** (external key, used for AddPropertyChangeEvents) – The key is used to pass a target property name to the AddPropertyChangeEvent.

**XAML(WPF) "View" fragment example of using with** MethodName **:**
``` xml
<Button Content="Button Click Method"
        Click="{vm:BindEventHandlerResource ResourceKey=ViewModelNewKey,
                MethodName=ButtonClickMethod}"/>
```

**XAML(WinRt+WPF) "View" fragment example of using with** MethodName **:**

``` c#
<Button Content="Button Click Method" >
      <vm:BindXAML.AddEvents>
          <vm:BindEventHandlerResource ResourceKey=ViewModelNewKey,
              MethodName="ButtonClickMethod" TargetEventName="Click"/>
      </vm:BindXAML.AddEvents>
</Button>
```

**XAML(WPF) "View" fragment example of using with** PropertyName **:**
``` xml
<Button Content="Button Click Method via PropertyName"
        Click="{vm:BindEventHandlerResource ResourceKey=ViewModelNewKey,
                PropertyName=ButtonClickProperty}"/>
```

**XAML(WinRt+WPF) "View" fragment example of using with** PropertyName **:**
``` xml
<Button Content="Button Click Method via PropertyName" >
      <vm:BindXAML.AddEvents>

          <vm:BindEventHandlerResource ResourceKey="ViewModelNewKey"

               PropertyName="ButtonClickProperty" TargetEventName="Click"/>

      </vm:BindXAML.AddEvents>

</Button>
```


**C# "View Model" fragment example:**
``` c#
public ViewModelNew()
{
    _buttonClickPropDelegate = newRoutedEventHandler(ButtonClickMethod);
}

privateRoutedEventHandler _buttonClickPropDelegate;

publicRoutedEventHandlerButtonClickProperty
{
    get { return _buttonClickPropDelegate; }
}

publicvoidButtonClickMethod(object sender, RoutedEventArgs e)
{

}
```


**XAML(WPF) "View" fragment example of using with** TargetPropertyName;

** it subscribes to the Dependency property change events:**
``` xml
<Label Content="Button Click Method via PropertyName" >
      <vm:BindXAML.AddPropertyChangeEvents>
          <vm:BindEventHandlerResource ResourceKey="ViewModelNewKey"
                MethodName="DataContextChanged"TargetPropertyName="DataContext"/>
      </vm:BindXAML.AddPropertyChangeEvents>
</Label>
```

**C# "View Model" fragment example of using with** TargetPropertyName;
** it subscribes to the Dependency property change events:**

``` xml
publicvoidDataContextChanged(object sender, EventArgs e)
{

}
```

# **BindXAML.AddEvents**

XAML attached property, fake collection, that used for processing extensions: BindEventHandler, BindEventHandlerIoc, BindEventHandlerResource.

It is used for compatibility between WinRT, Win Store App and WPF.


**XAML "View" fragment example:**
``` xml
<Button Content="Button Click Method" HorizontalAlignment="Left" >
     <vm:BindEventHandler MethodName="LoadedMethod" TargetEventName="Loaded"/>
     <vm:BindXAML.AddEvents>
         <vm:BindEventHandler MethodName="UnloadedMethod" TargetEventName="Unloaded"/>
         <vm:BindEventHandler MethodName="ButtonClickMethod" TargetEventName="Click"/>
     </vm:BindXAML.AddEvents>
</Button>
```

### **BindXAML.AssignProperties**

XAML attached extension that used for processing IocBinding and LocateDataContext  extensions. It is used for compatibility between WinRT, Win Store App and WPF.

**XAML "View" fragment example:**
``` xml
<Label Content="{Binding ButtonClickMethodMsg}" HorizontalAlignment="Left">
    <vm:BindXAML.AssignProperties>
        <vm:IocBinding ServiceType="MainWindowVm"
             ServiceKey="MainWindowVm" TargetPropertyName="DataContext"/>
    </vm:BindXAML.AssignProperties>
</Label>
```


# **BindCommand**

XAML mark-up and BindXAML.BindToCommand extensions; it binds binds a control command type dependency property to methods with using the **CommandHadlerProxy** wrapper class. It binds to the source object members defined by a **DataContext** dependency property.

**Properties:**

- **Source** (default key) – It is a "back-door" feature which allows to setup the source object. If it is not set on, by default,the markup extension will use the defined DataContext dependency property value. It may be used with {IocBinding ...} or other "agnostic" mark up extension(not {Binding ...}) which provides by the independent way a source object reference.
- **ExecuteMethodName** – The method name of the source object that performs as "void ICommand:Execute(object parameter)".  It's mutually exclusive versus ExecutePropertyName.  It can be static.
- **CanExecuteBooleanPropertyName** - The property name of the source object that refers to Boolean property that would be return by method "bool ICommand:CanExecute(object parameter)". INotifyPropertyChanged interface will be subscribed to trigger event "event EventHandler ICommand:CanExecuteChanged". It's mutually exclusive versus CanExecuteMethodName, CanExecutePropertyName, EventToInvokeCanExecuteChanged and PropertyActionCanExecuteChanged.
- **CanExecuteMethodName** – The method name of the source object that performs as "bool ICommand:CanExecute(object parameter)".  It can be static and optional. It's mutually exclusive versus CanExecutePropertyName.
- **ExecutePropertyName** –  The property name of the source object that has a type of Action<object> delegate that performs as "void ICommand:Execute(object parameter)".  It's mutually exclusive versus ExecutePropertyName.  It can be static.
- **CanExecutePropertyName** –The property name of the source object that has a type of **Func** <object, bool> delegate  that performs as "bool ICommand:CanExecute(object parameter)".  It can be static and optional. It's mutually exclusive versus CanExecutePropertyName.
- **EventToInvokeCanExecuteChanged** -  Name of an event class member to which will be added a delegate for rising an event in the proxy class "event EventHandler ICommand:CanExecuteChanged". Notification delegate of types **Action** <> or **EventHandler** <> will be added or removed synchronously when the event handler will be add or removed in "event EventHandler ICommand:CanExecuteChanged". It's mutually exclusive versus PropertyActionCanExecuteChanged. It can be static.
- **PropertyActionCanExecuteChanged** - Name of a property that will accept a delegate of **Action** <> delegate that can be used for rising an event in the proxy class "event EventHandler ICommand:CanExecuteChanged".Notification delegate of types **Action** <> or **EventHandler** <> will be set or cleared synchronously when the event handler will be add or removed in "event EventHandler ICommand:CanExecuteChanged". It's mutually exclusive versus EventToInvokeCanExecuteChanged. It can be static.
- **DeepScanAllTrees** - If it is set to "true", all DataContext properties in the logical tree will be scanned until the math to a property or method name (ExecutePropertyName, ExecuteMethodName). Smart feature allows to ignore the current DataContext property value and traverse to other parent DataContext value.If set on true, it will cause to scan for the DataContext property objects over the trees and get the first one that contains the binding property or method. It used in case when there is need to ignore the binding ItemsSource DataContext for the ItemsControl item, just bind a Button to a View Model for the item of the ListView or so on.

[**AppendViewModel**] - the mapping attribute that appends(extends) the binding list now is supported.



**XAML(WPF) "View" fragment example:**
``` xml
<Button Content="Click here!"
   Command="{vm:BindCommand ExecuteMethodName=CommandToExcute,
                CanExecuteBooleanPropertyName=Flag}"/>

<Button Content="Property Cmd-ExCe"
   Command="{vm:BindCommand ExecutePropertyName=ButtonExecuteProperty,
                CanExecutePropertyName=CanExecuteProperty,
                DeepScanAllTrees=True}"/>

<Button Content="Button Cmd-ExCeEv"
   Command="{vm:BindCommand ExecuteMethodName=ExecuteMethod,
                CanExecuteMethodName=CanExecuteMethod,
                EventToInvokeCanExecuteChanged=ActionNotifyCanExecuteChanged,
                DeepScanAllTrees=True}"/>
<Button Content="Button Cmd-ExProp"
   Command="{vm:BindCommand ExecuteMethodName=ExecuteMethod,
               CanExecuteBooleanPropertyName=CanExecuteFlag,
               DeepScanAllTrees=True}"/>
```

**XAML(WinRt+WPF) "View" fragment example:**

``` xml
<Button Content="Click here!">
    <vm:BindXAML.BindToCommand>
        <vm:BindCommand ExecuteMethodName="CommandToExcute"
            CanExecuteBooleanPropertyName="Flag"/>
    </vm:BindXAML.BindToCommand>
</Button>
```


**C# "View Model" fragment example:**
``` c#
bool _canExecuteFlag = true;
public bool CanExecuteFlag
 {
     get { return _canExecuteFlag; }
     set
     {
         _canExecuteFlag = value; NotifyPropertyChanged();
     }
 }

 publicAction<object> ButtonExecuteProperty { get; set; }

 public void ExecuteMethod(object sender)
 {

 }

 publicFunc<object, bool> CanExecuteProperty { get; set; }

 public bool CanExecuteMethod(object sender)
 {
     return CanExecuteFlag;
 }

 publiceventAction ActionNotifyCanExecuteChanged;

 publiceventEventHandler EventHandlerNotifyCanExecuteChanged;

 privateAction _propertyDelegateNotifyCanExecuteChanged;

 publicAction PropertyDelegateNotifyCanExecuteChanged
 {
     get { return _propertyDelegateNotifyCanExecuteChanged; }
     set { _propertyDelegateNotifyCanExecuteChanged = value; }
 }
```

# **BindCommandIoc**

XAML mark-up and BindXAML.BindToCommand extensions; it binds binds a control command dependency property to methods with using the **CommandHadlerProxy** wrapper class. It binds to the source object members of the type resolved by the IoC container.

**Properties:**

- **ServiceType** (default key) – The type of the requested object. The string of a type name of the requested object.
- **ExecuteMethodName** – The method name of the source object that performs as "void ICommand:Execute(object parameter)".  It's mutually exclusive versus ExecutePropertyName.  It can be static.
- **CanExecuteBooleanPropertyName** - The property name of the source object that refers to Boolean property that would be return by method "bool ICommand:CanExecute(object parameter)". INotifyPropertyChanged interface will be subscribed to trigger event "event EventHandler ICommand:CanExecuteChanged". It's mutually exclusive versus CanExecuteMethodName, CanExecutePropertyName, EventToInvokeCanExecuteChanged and PropertyActionCanExecuteChanged.
- **CanExecuteMethodName** – The method  name of the source object that performs as "bool ICommand:CanExecute(object parameter)".  It can be static and optional. It's mutually exclusive versus CanExecutePropertyName.
- **ExecutePropertyName** –  The property name of the source object that has a type of Action<object> delegate that performs as "void ICommand:Execute(object parameter)".  It's mutually exclusive versus ExecutePropertyName.  It can be static.
- **CanExecutePropertyName** –The property name of the source object that has a type of **Func** <object, bool> delegate  that performs as "bool ICommand:CanExecute(object parameter)".  It can be static and optional. It's mutually exclusive versus CanExecutePropertyName.
- **EventToInvokeCanExecuteChanged** -  Name of an event class member to which will be added a delegate for rising an event in the proxy class "event EventHandler ICommand:CanExecuteChanged". Notification delegate of types **Action** <> or **EventHandler** <> will be added or removed synchronously when the event handler will be add or removed in "event EventHandler ICommand:CanExecuteChanged". It's mutually exclusive versus PropertyActionCanExecuteChanged. It can be static.
- **PropertyActionCanExecuteChanged** - Name of a property that will accept a delegate of **Action** <> delegate that can be used for rising an event in the proxy class "event EventHandler ICommand:CanExecuteChanged".Notification delegate of types **Action** <> or **EventHandler** <> will be set or cleared synchronously when the event handler will be add or removed in "event EventHandler ICommand:CanExecuteChanged". It's mutually exclusive versus EventToInvokeCanExecuteChanged. It can be static.
- **DeepScanAllTrees** - If it is set to "true", all DataContext properties in the logical tree will be scanned until first math to a property or method name (ExecutePropertyName, ExecuteMethodName). Smart feature which allows ignore the current DataContext property value and traverse to other parent DataContext value.If set on true, it will cause to scan for the DataContext property objects over the trees and get the first one that contains the binding property or method. It used in case when there is need to ignore the binding ItemsSource DataContext for the ItemsControl item, just bind a Button to a View Model for the item of the ListView or so on.



**XAML(WPF) "View" fragment example:**
``` xml

<Button Content="Click here!"
   Command="{vm:BindCommandIoc ServiceType=ViewModels.ViewModelNew,
                ExecuteMethodName=CommandToExcute,
                CanExecuteBooleanPropertyName=Flag}"/>

<Button Content="Property Cmd-ExCe"
   Command="{vm:BindCommandIoc ServiceType=ViewModels.ViewModelNew,
                ExecutePropertyName=ButtonExecuteProperty,
                CanExecutePropertyName=CanExecuteProperty,
                DeepScanAllTrees=True}"/>

<Button Content="Button Cmd-ExCeEv"
   Command="{vm:BindCommandIoc ServiceType=ViewModels.ViewModelNew,
                ExecuteMethodName=ExecuteMethod,
                CanExecuteMethodName=CanExecuteMethod,
                EventToInvokeCanExecuteChanged=ActionNotifyCanExecuteChanged,
                DeepScanAllTrees=True}"/>

<Button Content="Button Cmd-ExProp"
   Command="{vm:BindCommandIoc ServiceType=ViewModels.ViewModelNew,
               ExecuteMethodName=ExecuteMethod,
               CanExecuteBooleanPropertyName=CanExecuteFlag,
               DeepScanAllTrees=True}"/>
```

**XAML(WinRt+WPF) "View" fragment example:**
``` xml
<Button Content="Click here!">
    <vm:BindXAML.BindToCommand>
        <vm:BindCommandIoc ServiceType="ViewModels.ViewModelNew"
            ExecuteMethodName="CommandToExcute"
            CanExecuteBooleanPropertyName="Flag"/>
    </vm:BindXAML.BindToCommand>
</Button>
```


**C# "View Model" fragment example:**
``` c#
bool _canExecuteFlag = true;

publicbool CanExecuteFlag
 {
     get { return _canExecuteFlag; }
     set
     {
         _canExecuteFlag = value; NotifyPropertyChanged();
     }
 }

 publicAction<object> ButtonExecuteProperty { get; set; }

 publicvoid ExecuteMethod(object sender)
 {

 }

 publicFunc<object, bool> CanExecuteProperty { get; set; }

 publicbool CanExecuteMethod(object sender)
 {
     return CanExecuteFlag;
 }

 publiceventAction ActionNotifyCanExecuteChanged;

 publiceventEventHandler EventHandlerNotifyCanExecuteChanged;

 privateAction _propertyDelegateNotifyCanExecuteChanged;

 publicAction PropertyDelegateNotifyCanExecuteChanged
 {
     get { return _propertyDelegateNotifyCanExecuteChanged; }
     set { _propertyDelegateNotifyCanExecuteChanged = value; }
 }
```

### **BindCommandResource**

XAML mark-up and BindXAML.BindToCommand extensions; it binds binds a control command dependency property to methods with using the **CommandHadlerProxy** wrapper class. It binds to the source object members located in Resources.

**Properties:**

- **ResourceKey** (default key) – Gets or sets the key value passed by a static resource reference. The key is used to return the object matching that key in resource dictionaries.
- **ExecuteMethodName** – The method name of the source object that performs as "void ICommand:Execute(object parameter)".  It's mutually exclusive versus ExecutePropertyName.  It can be static.
- **CanExecuteBooleanPropertyName** - The property name of the source object that refers to Boolean property that would be return by method "bool ICommand:CanExecute(object parameter)". INotifyPropertyChanged interface will be subscribed to trigger event "event EventHandler ICommand:CanExecuteChanged". It's mutually exclusive versus CanExecuteMethodName, CanExecutePropertyName, EventToInvokeCanExecuteChanged and PropertyActionCanExecuteChanged.
- **CanExecuteMethodName** – The method name of the source object that performs as "bool ICommand:CanExecute(object parameter)".  It can be static and optional. It's mutually exclusive versus CanExecutePropertyName.
- **ExecutePropertyName** –  The property name of the source object that has a type of Action<object> delegate that performs as "void ICommand:Execute(object parameter)".  It's mutually exclusive versus ExecutePropertyName.  It can be static.
- **CanExecutePropertyName** –The property name of the source object that has a type of **Func** <object, bool> delegate  that performs as "bool ICommand:CanExecute(object parameter)".  It can be static and optional. It's mutually exclusive versus CanExecutePropertyName.
- **EventToInvokeCanExecuteChanged** -  Name of an event class member to which will be added a delegate for rising an event in the proxy class "event EventHandler ICommand:CanExecuteChanged". Notification delegate of types **Action** <> or **EventHandler** <> will be added or removed synchronously when the event handler will be add or removed in "event EventHandler ICommand:CanExecuteChanged". It's mutually exclusive versus PropertyActionCanExecuteChanged. It can be static.
- **PropertyActionCanExecuteChanged** - Name of a property that will accept a delegate of **Action** <> delegate that can be used for rising an event in the proxy class "event EventHandler ICommand:CanExecuteChanged".Notification delegate of types **Action** <> or **EventHandler** <> will be set or cleared synchronously when the event handler will be add or removed in "event EventHandler ICommand:CanExecuteChanged". It's mutually exclusive versus EventToInvokeCanExecuteChanged. It can be static.
- **DeepScanAllTrees** - If it is set to "true", all DataContext properties in the logical tree will be scanned until first math to a property or method name (ExecutePropertyName, ExecuteMethodName). Smart feature which allows ignore the current DataContext property value and traverse to other parent DataContext value.If set on true, it will cause to scan for the DataContext property objects over the trees and get the first one that contains the binding property or method. It used in case when there is need to ignore the binding ItemsSource DataContext for the ItemsControl item, just bind a Button to a View Model for the item of the ListView or so on.

**XAML(WPF) "View" fragment example:**

``` xml
<Button Content="Click here!"
   Command="{vm:BindCommandResource ResourceKey=ViewModelNewKey,
                ExecuteMethodName=CommandToExcute,
                CanExecuteBooleanPropertyName=Flag}"/>

<Button Content="Property Cmd-ExCe"
   Command="{vm:BindCommandResource ResourceKey=ViewModelNewKey,
                ExecutePropertyName=ButtonExecuteProperty,
                CanExecutePropertyName=CanExecuteProperty,
                DeepScanAllTrees=True}"/>

<Button Content="Button Cmd-ExCeEv"
   Command="{vm:BindCommandResource ResourceKey=ViewModelNewKey,
                ExecuteMethodName=ExecuteMethod,
                CanExecuteMethodName=CanExecuteMethod,
                EventToInvokeCanExecuteChanged=ActionNotifyCanExecuteChanged,
                DeepScanAllTrees=True}"/>

<Button Content="Button Cmd-ExProp"
   Command="{vm:BindCommandResource ResourceKey=ViewModelNewKey,
                ExecuteMethodName=ExecuteMethod,
                CanExecuteBooleanPropertyName=CanExecuteFlag,
                DeepScanAllTrees=True}"/>
```

**XAML(WinRt+WPF) "View" fragment example:**

``` xml

<Button Content="Click here!">
    <vm:BindXAML.BindToCommand>
        <vm:BindCommandResource ResourceKey="ViewModelNewKey"
            ExecuteMethodName="CommandToExcute" CanExecuteBooleanPropertyName="Flag"/>
    </vm:BindXAML.BindToCommand>
</Button>
```


**C# "View Model" fragment example:**

``` c#
bool _canExecuteFlag = true;

publicbool CanExecuteFlag
 {
    get { return _canExecuteFlag; }
    set
     {
         _canExecuteFlag = value; NotifyPropertyChanged();
     }
 }

 publicAction<object> ButtonExecuteProperty { get; set; }

 publicvoid ExecuteMethod(object sender)
 {

 }


 publicFunc<object, bool> CanExecuteProperty { get; set; }

 publicbool CanExecuteMethod(object sender)
 {
     return CanExecuteFlag;
 }

 publiceventAction ActionNotifyCanExecuteChanged;

 publiceventEventHandler EventHandlerNotifyCanExecuteChanged;

 privateAction _propertyDelegateNotifyCanExecuteChanged;

 publicAction PropertyDelegateNotifyCanExecuteChanged
 {

     get { return _propertyDelegateNotifyCanExecuteChanged; }

     set { _propertyDelegateNotifyCanExecuteChanged = value; }
 }
```

# **BindXAML.BindToCommand**

XAML attached property, fake collection, is used for processing extensions: BindCommand, BindEventHandlerIoc, BindEventHandlerResource.

It is used for compatibility between WinRT, Win Store App and WPF.

**XAML "View" fragment example:**

``` xml
 <Button Content="Button Cmd-Ex " HorizontalAlignment="Left"">
    <vm:BindXAML.BindToCommand>
        <vm:BindCommand  ExecuteMethodName="ExecuteMethod" DeepScanAllTrees="True"/>
    </vm:BindXAML.BindToCommand>
</Button>
```

  **C# "View Model" fragment example:**

``` xml
bool _canExecuteFlag = true;

publicbool CanExecuteFlag
 {

     get { return _canExecuteFlag; }
     set
     {
         _canExecuteFlag = value; NotifyPropertyChanged();
    }
 }

 publicAction<object> ButtonExecuteProperty { get; set; }

 publicvoid ExecuteMethod(object sender)
 {

 }
```

# **LocateDataContext**

XAML mark-up and BindXAML.AssignProperties extensions; it finds in the chain of **DataContext** objects, the first, which contains the exact method or property. It comes through parent elements of logical and visual trees.

**Properties:**

- ··DataContextType (default key,optional) – The type (System.Type) or the type name (System.String) of the required **DataContext** object. If it is not set,  a method or property name will be only used to locate.
- ··MethodName – The method name is used to search in DataContext object methods. It's priority versus PropertyName.
- ··PropertyName – The property name is used to search in DataContext object properties.
- ··TargetPropertyName (external key, used for AssignProperties) – The target dependency property name; it will be to set to the located **DataContext** object.

# **BindXAML.AddPropertyChangeEvents**

XAML attached property, fake collection, is used for processing extensions: BindEventHandler, BindEventHandlerIoc, BindEventHandlerResource.

It binds a View dependency property change event handler to a event handler in a View Model. It is only applicable to WPF.

**XAML "View" fragment example of binding property "** Content **" change event to View Model:**

``` xml
<Label Content="{Binding FluentLabel}">
  <vm:BindXAML.AddPropertyChangeEvents>
    <vm:BindEventHandler MethodName="ContentChanged" TargetPropertyName="Content"/>
  </vm:BindXAML.AddPropertyChangeEvents>
</Label>
```

**C# "View Model" fragment example:**

``` c#
publicvoidContentChanged(object sender, EventArgs e)
{

}

```


# **IocBinding**

XAML mark-up and BindXAML.AssignProperties extensions; it binds to IoC container elements.

**Properties:**

- **ServiceType** (default key) – The type (System.Type) or the type name (System.String) of the requested object.
- **TargetPropertyName** (external key, used for AssignProperties) – The target dependency property name; it will be set to the located object by the IoC container.

**XAML "View" fragment example:**

``` c#
<Label Content="{Binding ButtonClickMethodMsg}" HorizontalAlignment="Left">
    <vm:BindXAML.AssignProperties>
        <vm:IocBinding ServiceType="MainWindowVm"
             ServiceKey="MainWindowVm" TargetPropertyName="DataContext"/>
    </vm:BindXAML.AssignProperties>
</Label>
```


# MvvmBindingPack BindEventHandler vs EventTrigger

**RelayCommand** , **DelegateCommand**  and **ActionCommand** classes are well known as solutions for creating instant **ICommand** interface implementations. It's quite a bulky job to wrap the every method of the **View** Model **withusing** RelayCommand **or** DelegateCommand **classes. Refactoring of such View Models is a real nightmare. The description of** DelegateCommand** class you can find [here](https://msdn.microsoft.com/en-us/library/microsoft.practices.prism.commands.delegatecommand(v=pandp.50).aspx). It is the class that implements an [ICommand](http://msdn2.microsoft.com/en-us/library/ms616869) interface and its delegates provides [Execute()](https://msdn.microsoft.com/en-us/library/gg405762(v=pandp.50).aspx) and [CanExecute()](https://msdn.microsoft.com/en-us/library/gg405761(v=pandp.50).aspx) method functionality.

It is hard to describe the inefficiency of the way of implementing the event invocation shown in the classic **View** Model** example.

For binding the CommandToExcute method you have to write lots line of code(see marked in red) and make a "magic dance" around the " **event trigger**".

**XAML fragment:**
``` xml
<Button Content="Click here!"   Margin="5">
        <i:Interaction.Triggers>
            <i:EventTrigger EventName="Click" >
                <i:InvokeCommandAction Command="{BindingClickCommand}" />
             </i:EventTrigger>
          </i:Interaction.Triggers>
</Button>
```

**View Model fragment:**
``` c#
classViewModelOld
    {
        private readonly DelegateCommand<string> _command;
        private bool _flag;
        public ViewModelOld()
        {
            _command = new DelegateCommand<string>(

                (s) => { CommandToExcute(s); }, //Execute

                (s) => { return _flag; } //CanExecute

                );
        }
        public DelegateCommand<string> ClickCommand
        {
            get { return _command; }
        }

        publicvoidCommandToExcute(object parameter)
        {

        }
    }
```

There will be absolutely simple solution if you are using **MvvmBindingPack.**

**XAML fragment:**

``` xml
<Button Content="Click here!"
        Click="{vm:BindEventHandler MethodName=Button_Click}" Margin="5"/>
```

**or XAML fragment (WinRt+WPF):**

``` xml
<Button Content="Click here!">
   <vm:BindXAML.AddEvents>
       <vm:BindEventHandler MethodName="Button_Click" TargetEventName="Click"/>
   </vm:BindXAML.AddEvents>
</Button>
```

**View Model fragment:**

``` c#
namespace ViewModels
{

  classViewModelNew : NotifyChangesBase
` {
       publicvoidButton_Click(object sender, RoutedEventArgs e)
       {

       }
    }
}
```

There is another variant to bind to a **View** Model with using Resources.**

**XAML fragment (WPF):**

``` xml
<Button Content="Click here!"
        Click="{vm:BindEventHandlerResource
        ResourceKey=ViewModelNewKey, MethodName=Button\_Click}"
        Margin="5"/>
```


**or XAML fragment (WinRt+WPF):**

``` xml
<Button Content="Click here!">
    <vm:BindXAML.AddEvents>
        <vm:BindEventHandlerResource ResourceKey="ViewModelNewKey"
         MethodName="Button\_Click" TargetEventName="Click"/>
    </vm:BindXAML.AddEvents>
</Button>
```

There is another variant to bind to a **View** Model with IoC containers.**

**XAML fragment (WPF):**

```xml
<Button Content="Click here!"
        Click="{vm:BindEventHandlerIoc
        ServiceType=ViewModels.ViewModelNew, MethodName=Button\_Click}"
        Margin="5"/>
```
**or XAML fragment (WinRt+WPF):**

``` xml
<Button Content="Click here!">
    <vm:BindXAML.AddEvents>
       <vm:BindEventHandlerIoc ServiceType="ViewModels.ViewModelNew"
                 MethodName="Button\_Click" TargetEventName="Click"/>
    </vm:BindXAML.AddEvents>
</Button>
```


# MvvmBindingPack BindCommand vs DelegateCommand

**RelayCommand** , **DelegateCommand** and **ActionCommand** classes are well known as solutions for creating instant **ICommand** interface implementations. It's quite a bulky job to wrap the every method of the **View** Model **withusing** RelayCommand **or** DelegateCommand **classes. Refactoring of such View Models is a real nightmare. The description of** DelegateCommand** class you can find [here](https://msdn.microsoft.com/en-us/library/microsoft.practices.prism.commands.delegatecommand(v=pandp.50).aspx). It is the class that implements an [ICommand](http://msdn2.microsoft.com/en-us/library/ms616869) interface and its delegates provides [Execute()](https://msdn.microsoft.com/en-us/library/gg405762(v=pandp.50).aspx) and [CanExecute()](https://msdn.microsoft.com/en-us/library/gg405761(v=pandp.50).aspx) method functionality. Inefficiency of using these class wrappers cost time and a budget surplus.

In the **View** Model** example, for calling the CommandToExcute method you have to write lots lines of code (see marked in red).


**XAML fragment:**
``` xml
<Button Content="Click here!" Command="{BindingButtonClickCommand}"Margin="5"/>
```

**View Model fragment:**
``` c#
classViewModelOld
    {

        private readonly DelegateCommand<string> _command;
        private bool _flag;
        public ViewModelOld()
        {
            _command = new DelegateCommand<string>(
                   (s) => { CommandToExcute(s); }, //Execute
                   (s) => { return _flag;       }  //CanExecute
                );
        }

        public DelegateCommand<string> ButtonClickCommand
        {
            get { return _command; }
        }

        public void SetFlag(bool flag)
        {
                _flag=flag;
                _command.RaiseCanExecuteChanged();
        }

        publicvoidCommandToExcute(object parameter)
        {

        }
    }
```

So, it is a very bulky and looking weird. In order to call (bind) one method you have to implement the redundant code lines. It is introducing additional complexity that are not appropriate or useful. It makes harder to understand the code and you cannot immediately change the code of the **View** Model after Agile scrum meeting**. There are also the practical difficulties: code browsing; try to find out what is a code about; and  the test coverage.

There is an absolutely different picture if you are using **MvvmBindingPack.**

**XAML fragment:**

```  c#
<Button Content="Click here!"
        Command="{vm:BindCommand ExecuteMethodName=CommandToExcute,
        CanExecuteBooleanPropertyName=Flag}" Margin="5"/>
```

**or XAML fragment (WinRt+WPF):**

``` c#
<Button Content="Click here!"   Margin="5">
   <vm:BindXAML.BindToCommand>
     <vm:BindCommand  ExecuteMethodName="CommandToExcute"
                     CanExecuteBooleanPropertyName="Flag"/>
   </vm:BindXAML.BindToCommand>
</Button>
```

**View Model fragment:**

``` c#
namespaceViewModels
{
 classViewModelNew : NotifyChangesBase
    {
        privatebool \_flag;
        publicboolFlag
        {
            get { return \_flag; }
            set { \_flag = value; NotifyPropertyChanged(); }
        }
        publicvoidCommandToExcute(object parameter)
        {

        }
    }
}
```
Nothing redundant that you will not want to have in the code.

There is another variant to bind to a **View** Model with using Resources.**

**XAML fragment (WPF):**

``` xml
 <Button Content="Click here!"
         Command="{vm:BindCommandResource ResourceKey=ViewModelNewKey,
         ExecuteMethodName=CommandToExcute,CanExecuteBooleanPropertyName=Flag}"
         Margin="5"/>
```


**or XAML fragment (WinRt+WPF):**

``` xml
 <Button Content="Click here!"   Margin="5">
    <vm:BindXAML.BindToCommand>
        <vm:BindCommandResource ResourceKey="ViewModelNewKey"
                                ExecuteMethodName="CommandToExcute"
                                CanExecuteBooleanPropertyName="Flag"/>
    </vm:BindXAML.BindToCommand>
 </Button>
```

There is another variant to bind to a **View** Model with IoC containers.**

**XAML fragment (WPF):**

``` xml
<Button Content="Click here!"
        Command="{vm:BindCommandIoc ServiceType=ViewModels.ViewModelNew,
        ExecuteMethodName=CommandToExcute,CanExecuteBooleanPropertyName=Flag}"
        Margin="5"/>
```

**or with container key:**

``` xml
<Button Content="Click here!" Command="{vm:BindCommandIoc
        ServiceKey=NewModel,ServiceType=ViewModels.ViewModelNew,
        ExecuteMethodName=CommandToExcute,CanExecuteBooleanPropertyName=Flag}"
        Margin="5"/>
```

**or XAML fragment (WinRt+WPF):**
``` xml
 <Button Content="Click here!"   Margin="5">
    <vm:BindXAML.BindToCommand>
       <vm:BindCommandIoc ServiceKey="NewModel"
                          ServiceType="ViewModels.ViewModelNew"
                          ExecuteMethodName="CommandToExcute"
                          CanExecuteBooleanPropertyName="Flag"/>
    </vm:BindXAML.BindToCommand>
 </Button>
 ```


