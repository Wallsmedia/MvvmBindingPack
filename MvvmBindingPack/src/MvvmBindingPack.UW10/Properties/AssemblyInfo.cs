// 
//  MVVM-WPF-NetCore Markup Binding Extensions
//  Copyright © 2013-2017 Alexander Paskhin /paskhin@hotmail.co.uk/ All rights reserved.
// 
//
// Licensed under the Apache License, Version 2.0 (the "License"); you
// may not use this file except in compliance with the License. You may
// obtain a copy of the License at  http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or
// implied. See the License for the specific language governing permissions
// and limitations under the License.

using System.Reflection;
using System.Runtime.InteropServices;


#if WINDOWS_UWP
using Windows.UI.Xaml;
//[assembly: XmlnsDefinitionAttribute("MvvmBindingPack", "NetCoreMvvmBindingPack")]
#else
using System.Windows.Markup;
using System.Runtime.CompilerServices;
[assembly: XmlnsDefinition("Mvvm", "MvvmBindingPack")]
[assembly: XmlnsDefinition("MvvmBinding", "MvvmBindingPack")]
[assembly: XmlnsDefinition("MvvmBindingPack", "MvvmBindingPack")]

#if DEBUG
[assembly: InternalsVisibleToAttribute("UnitTestMvvmBindingPack")]
#endif

#endif


// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("MvvmBindingPack")]
[assembly: AssemblyDescription("MVVM-WPF-NetCore Markup Binding Extensions Auto Wiring")]
[assembly: AssemblyConfiguration("UAP10.0 onwards")]
[assembly: AssemblyCompany("Alexander Paskhin /paskhin@hotmail.co.uk/")]
[assembly: AssemblyProduct("MvvmBindingPack")]
[assembly: AssemblyCopyright("Copyright © 2013-2017 Alexander Paskhin /paskhin@hotmail.co.uk/ All rights reserved.")]
[assembly: AssemblyTrademark("Alexander Paskhin /paskhin@hotmail.co.uk/")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("72AA527B-FCA6-4E0B-BDDB-EFF98A54ACFB")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
[assembly: AssemblyVersion("2.5.0.0")]
[assembly: AssemblyFileVersion("2.5.0.0")]
