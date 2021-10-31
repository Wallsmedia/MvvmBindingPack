// 
//  MVVM-WPF-NetCore Markup, Binding and other Extensions.
//  Copyright © 2013-2021 Alexander Paskhin /paskhin@hotmail.co.uk/ All rights reserved.
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

using System;
using System.Windows;
using System.Windows.Input;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

#if WINDOWS_UWP

using Windows.UI.Xaml;
using System.Diagnostics;
using System.Reflection;

#else
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;

#if BEHAVIOR
using System.Windows.Interactivity;
#endif

using System.Windows.Threading;
using System.Diagnostics;
#endif

namespace MvvmBindingPack
{
    /// <summary>
    ///  XAML MVVM extension enhancer, it automatically locates and sets(binds) the View dependency 
    ///  property(default is "DataContext") to a View Model reference.
    /// 
    ///  Attached property BindXAML.AutoWiredViewModel will be set to the reference to the View Model.
    ///  View to View Model mapping rules.
    ///
    ///  AutoWireVmDataContext setups a View dependency property with a reference to a View Model class instance. 
    ///  By default it is "DataContext". The name of the target dependency property can be changed via property
    ///  "TargetPropertyName". The AutoWireVmDataContext logic of wiring to a View Model is based on using 
    ///  from the x:Name and x:Class XAML directives:
    ///  x:Name directive uniquely identifies XAML-defined elements in a XAML namescope.
    ///  x:Class directive configures XAML markup compilation to join partial classes between markup and code-behind 
    ///  and it has the type namespace.The namespace will be used to construct expected types.
    ///
    ///  The View(XAML) logical tree elements will be scanned, in root direction, in order to detect non-“System.”, 
    ///  non-“Microsoft.” ,other non - WPF class types. For each “DependencyObject” based class will be obtained 
    ///  the “Name” property value.In the result, it will be formed the list of types (namespace + name) (x:Class) 
    ///  and names(x:Name if it was set).  For each element in the list will be applied transformation rules in order
    ///  to construct the View Model expected types.There will be formed the new list of View Model expected types.
    ///  The candidate list of types for matching will be obtained from loaded assemblies.
    ///
    /// General rules for forming View Model expected type names:
    ///
    /// If the View type namespace suffix section contains a "Views"(default see prop.ViewsNameSpaceSuffixSection),
    ///  this section will be replaced on "ViewModels"(default see prop.ViewModelsNameSpaceSuffixSection). It forms
    ///  a “expected namespace”.
    /// Example:
    ///   Trade.SuperUI.Views => Trade.SuperUI.ViewModels ,but(!)
    ///   Trade.SuperUI.Views.Views => Trade.SuperUI.Views.ViewModels
    ///   Trade.SuperUI.RViews => Trade.SuperUI.RViews
    ///
    ///  If the View type namespace suffix section doesn't contains a "Views" suffix section and  the namespace 
    ///  has only one or two sections, in this case the suffix section "ViewModels"
    ///  (default see prop.ViewModelsNameSpaceSuffixSection) will be added. It forms a “expected namespace”.
    /// Example:
    ///   Trade.TicketPanel => Trade.TicketPanel.ViewModel ,or(!)
    ///   Trade => Trade.ViewModel
    ///
    ///  If a type name(i.e.x:Class name) or x:Name contains "View" substring(default see prop."OldViewNamePart"), 
    ///  it will be replaced all occurrence on "ViewModel" substring(default see prop. "NewViewModelNamePart").
    ///  They form a pair  of  "expected type names”.
    /// Example:
    ///   TicketViewPanel => TicketViewModelPanel ,but(!)
    ///   TradeViewTicketViewPanel => TradeViewModelTicketViewModelPanel
    ///
    ///  The “expected fully qualified type names”  will be formed from the parts "expected namespace” and “expected 
    ///  type names” from x:Class name and x:Name.
    ///  Formed from x:Name the "expected fully qualified type name" will have a priority over the x:Class formed 
    ///  type name.
    ///  The list of candidate types and interfaces (see IncludeInterfaces) will be obtained from all loaded 
    ///  assemblies by filtering with "expected  namespace". Each candidate type name will be examined 
    ///  on best matching to “expected name”.
    ///  Each possible candidate name will be split into a cased parts and matched against "desired name candidate“ 
    ///  parts.
    ///  The first candidate type with the full parts match will be selected.
    ///  If you set “UseMaxNameSubMatch” flag true, the first candidate with a sub-match type name will selected.
    ///
    ///  Obtain instance of the View Model type.
    ///
    ///  Type will be resolved in the sequence: IoC container, Resources and Activator.CreateInstance(). 
    ///  For controlling see properties “ResolveIocContainer”,” ResolveResources” and “ResolveCreateInstance”. 
    ///  In success the resolved type will set as value to "DataContext" dependency property(set by default 
    ///  "TargetPropertyName") and attached property BindXAML.AutoWiredViewModel.
    ///
    ///  Order of resolving via the IoC hosted via adapter that implements ServiceLocation or the generic 
    ///  Service Locator interface:
    ///  GetInstance(locatedItem_WiringType, XName); or
    ///  GetInstance(locatedItem_WiringType, locatedItem_WiringType.Name); or
    ///  GetInstance(locatedItem_WiringType).
    ///  
    ///  Order of resolving via the Resource Locator:
    ///  LocateResource(XName); or
    ///  LocateResource(locatedItem_WiringType.Name); or
    ///  LocateResource(locatedItem_WiringTType.FullName); or
    ///  LocateResource(locatedItem_WiringType).
    /// </summary>

#if BEHAVIOR
    public class AutoWireVmDataContext : Behavior<FrameworkElement>
#else

    public class AutoWireVmDataContext
#endif
    {
        /// <summary>
        /// Sets the service provider that resolves the dependency injection via <see cref="IServiceProvider"/>.
        /// </summary>
        public static System.IServiceProvider ServiceProvider { get; set; }

        /// <summary>
        /// Class container is used for holding the candidate x:Class, x:Name and type.
        /// </summary>
        [DebuggerDisplay("WiringName = {WiringName}, DesiredXCLassName = {DesiredXCLassName}, DesiredXName = {DesiredXName}, WiringFullName = {WiringFullName}")]
        class WiringCandidatePair
        {
            /// <summary>
            /// The Source subject for the wiring.
            /// </summary>
            public XClassTypeNameElement XClass { get; set; }

            /// <summary>
            /// The Desired target wiring name; it is formed from the x:Class name;
            /// </summary>
            public string DesiredXCLassName { get; set; }   //Formed from the x:Class
            public int DesiredXClassNameRank { get; set; }

            /// <summary>
            /// The Desired target wiring name; it is formed from the x:Name name;
            /// </summary>
            public string DesiredXName { get; set; }        // Formed from the x:Name
            public int DesiredXNameRank { get; set; }

            /// <summary>
            /// The located wiring target
            /// </summary>
            public Type WiringType { get; set; }

            public string WiringFullName => WiringType.FullName;
            public string WiringNamespace => WiringType.Namespace;
            public string WiringTypeName => WiringType.Name;

            public string WiringName { get; set; }
            public int WiringNameRank { get; set; }

            /// <summary>
            /// Matching results between desired and wiring names.
            /// </summary>
            public int DesiredXClassNameMatchToWiringNameRank { get; set; }
            public int DesiredXNameMatchToWiringNameRank { get; set; }

            public bool IsXClassNameMatch => (DesiredXClassNameRank > 0) && (DesiredXClassNameRank == WiringNameRank) && (DesiredXClassNameRank == DesiredXClassNameMatchToWiringNameRank);
            public bool IsXClassNameSubMatch => (DesiredXClassNameRank > 0) && (DesiredXClassNameRank < WiringNameRank) && (DesiredXClassNameRank == DesiredXClassNameMatchToWiringNameRank);
            public bool IsXNameMatch => (DesiredXNameRank > 0) && (DesiredXNameRank == WiringNameRank) && (DesiredXNameRank == DesiredXNameMatchToWiringNameRank);
            public bool IsXNameSubMatch => (DesiredXNameRank > 0) && (DesiredXNameRank < WiringNameRank) && (DesiredXNameRank == DesiredXNameMatchToWiringNameRank);
        }

        /// <summary>
        /// Overwrites the  x:Class namespace; it will be used for exact defining of the view model expected type
        /// namespace. Original, the x:Class namespace will be ignored.
        /// </summary>
        public string ViewModelNamespaceOverwrite { get; set; }

        /// <summary>
        /// Overwrites the  x:Class name; it will be used for exact type name defining  of view model expected type
        /// name candidates. Original, the x:Class name will be ignored.
        /// </summary>
        public string ViewModelNameOverwrite { get; set; }

        /// <summary>
        /// The target dependency property name. It will be set to a resolved reference to a View Model.
        /// </summary>
        public string TargetPropertyName { get; set; } = "DataContext";

        /// <summary>
        /// If it is set to 'true' (default), it limits the types of x:Class and x:Name  
        /// to the first found control in the logical tree.
        /// </summary>
        public bool UseTheFirstOne { get; set; } = true;

        /// <summary>
        /// If it is set to 'true', the IoC container will be used to resolve 
        /// a View Model type or instance. It has the first priority. Default value is true.
        /// </summary>
        public bool ResolveIocContainer { get; set; } = true;

        /// <summary>
        /// If it is set to 'true', the static Resources will be used to resolve 
        /// a View Model instance. It has the second priority.  Default value is true.       
        /// </summary>
        public bool ResolveResources { get; set; } = true;

        /// <summary>
        /// If it is set to 'true', the static CLR Activator will be used to create 
        /// a View Model instance. It has the third priority.  Default value is true.   
        /// </summary>
        public bool ResolveCreateInstance { get; set; } = true;

        /// <summary>
        ///  Defines the additional sub matching ("start with") rule when a  View Model expected name 
        ///  compared to a View Model candidate name. If it is set to 'true', the View Model expected name
        ///  is considered as a match to a name if starts with 'View Model expected name'. 
        /// </summary>
        /// <example>
        /// The View Model expected name "FrameCapturePrice" will match to the View Model 
        /// candidate name "FrameCapturePrice_Var1".
        /// </example>
        public bool UseMaxNameSubMatch { get; set; }

        /// <summary>
        /// Defines the namespace section suffix (default "Views"). It will be replaced (if it is exist)
        ///  on the "ViewModelsNamespaceSuffixSection" property value. 
        /// </summary>
        /// <remarks>Ignored when the "ViewModelNamespaceOverwrite" is set.</remarks>
        /// <example>
        /// The namespace 'Trade.GUI.Application.Views' will be transfered into  'Trade.GUI.Application.ViewModels'
        /// The namespace 'Trade.GUI.Application' will be transfered into  'Trade.GUI.Application.ViewModels'.
        /// </example>
        public string ViewsNamespaceSuffixSection { get; set; } = "Views";

        /// <summary>
        ///  Defines the namespace section suffix  (default "ViewModels"). It will be used as a replacement.
        /// </summary>
        /// <example>
        /// The namespace 'Trade.GUI.Application.Views' will be transfered into  'Trade.GUI.Application.ViewModels'
        /// The namespace 'Trade.GUI.Application' will be transfered into  'Trade.GUI.Application.ViewModels'
        /// </example>
        /// <remarks>Ignored when the "ViewModelNamespaceOverwrite" is set.</remarks>
        public string ViewModelsNamespaceSuffixSection { get; set; } = "ViewModels";

        /// <summary>
        ///Defines the part of the class type name (default "View"). If it is exist, it will be replaced on the value 
        /// of the property "NewViewModelNamePart". It is ignored when the "ViewModelNameOverwrite" is set.
        /// </summary>
        /// <example>
        /// The name "MainPageView" will be transfered into  "MainPageViewModel".
        /// The name "MainPageViewFrame_1" will be transfered into  "MainPageViewModelFrame_1".
        /// The name "MainPage" will be the same "MainPage".
        /// </example>
        public string OldViewNamePart { get; set; } = "View";

        /// <summary>
        /// Defines the part of the class type name (default "ViewModel").It is ignored when the "ViewModelNameOverwrite" is set. 
        /// </summary>
        public string NewViewModelNamePart { get; set; } = "ViewModel";

        /// <summary>
        /// If it is set to 'true', there will be included interfaces from the loaded assemblies into the list of type candidates.
        /// Default value is true. It allows to use the interfaces in ViewModelNameOverwrite  and resolve them via IoC container.
        /// </summary>
        public bool IncludeInterfaces { get; set; } = true;

        /// <summary>
        ///  Ioc resolver flag.f it is set to 'true', the  IoC type will be attempted to be resolved with using type and x:Name (if it was set!!!).
        ///  Default value is false.
        /// </summary>
        public bool IocXName { get; set; }

#if BEHAVIOR
        /// <summary>
        /// Implementation of interface IAttachedObject.
        /// Called after the behavior is attached to an AssociatedObject.
        /// </summary>
        protected override void OnAttached()
        {
            Execute(AssociatedObject);
            base.OnAttached();
        }

        /// <summary>
        /// Implementation of interface IAttachedObject.
        /// Called when the behavior is being detached from its AssociatedObject, but
        /// before it has actually occurred.
        /// </summary>
        protected override void OnDetaching()
        {
            AssociatedObject.Loaded -= FrameworkElementLoaded;
            base.OnDetaching();
        }
#endif

        /// <summary>
        /// Setups the Dependency property (default is "DataContext", see "TargetPropertyName") to a View Model reference.
        /// It starts with current XAML node, defined by a 'dependencyObject' parameter, and goes up through ancestors (parents). 
        /// It's directly called  when this class has been processed by "BindXAML.ProcessMvvmExtensions" 'fake' attached property collection. 
        /// Attached property 'BindXAML.AutoWiredViewModel' will be with value of the wired model.
        /// </summary>
        /// <param name="dependencyObject">The Dependency object.</param>
        public void Execute(DependencyObject dependencyObject)
        {
            object resolvedObject;
            FrameworkElement frameworkElement = dependencyObject as FrameworkElement;
#if !WINDOWS_UWP

            if (frameworkElement == null)
            {
                return;
            }

            resolvedObject = ProcessAutoWiringToViewModel(frameworkElement);
            if (frameworkElement.IsLoaded)
            {
                return;
            }

            if ((resolvedObject == null) && ResolveResources)
            {
#endif
                resolvedObject = ProcessAutoWiringToViewModel(frameworkElement);

                if (resolvedObject == null)
                {
                    // auto defer until loaded
                    frameworkElement.Loaded += FrameworkElementLoaded;
                }

#if !WINDOWS_UWP
            }
#endif

        }

        /// <summary>
        /// Executes auto wiring or binding logic after the FrameworkElement object has loaded. 
        /// </summary>
        /// <param name="sender">The FrameworkElement element.</param>
        /// <param name="routedEventArgs">The state information and event data associated with a routed event.</param>
        public void FrameworkElementLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            FrameworkElement frameworkElement = sender as FrameworkElement;
            if (frameworkElement == null)
            {
                return;
            }

            frameworkElement.Loaded -= FrameworkElementLoaded;
            ProcessAutoWiringToViewModel(frameworkElement);
        }

        object ProcessAutoWiringToViewModel(FrameworkElement frameworkElement)
        {

            List<XClassTypeNameElement> parentXClassList = BindHelper.FindNonSystemParentClassNames(frameworkElement, UseTheFirstOne);

            //collect in the here below all possible candidates 
            var viewModelCandidates = new List<WiringCandidatePair>();
            string desiredVmNameSpace = string.Empty;

            foreach (XClassTypeNameElement parentXClass in parentXClassList)
            {

                // Split a full type name in the sections
                var parentFullClassNameSections = new List<string>(parentXClass.FullxClassTypeName.Split('.'));

                if ((!string.IsNullOrEmpty(ViewsNamespaceSuffixSection)) && (!string.IsNullOrEmpty(ViewModelsNamespaceSuffixSection)))
                {
                    if (parentFullClassNameSections.Count >= 3)
                    {
                        {
                            // if a name space suffix section contains a "Views"(default see prop. ViewsNameSpaceSuffixSection),
                            // this section will be replaced on "ViewModels"(default see prop.ViewModelsNameSpaceSuffixSection)
                            if (parentFullClassNameSections[parentFullClassNameSections.Count - 2].Contains(ViewsNamespaceSuffixSection))
                            {
                                parentFullClassNameSections[parentFullClassNameSections.Count - 2] = ViewModelsNamespaceSuffixSection;
                            }
                        }
                    }
                    else
                    {
                        // If there is no a suffix section, i.e a name space  has only one or two, the suffix section "ViewModels"(default see prop.ViewModelsNameSpaceSuffixSection)
                        // will be added.
                        parentFullClassNameSections.Insert(parentFullClassNameSections.Count - 1, ViewModelsNamespaceSuffixSection);
                    }
                }

                // Form a desired View Model  namespace for a type.
                desiredVmNameSpace = string.Join(".", parentFullClassNameSections.ToArray(), 0, parentFullClassNameSections.Count - 1);
                if (!string.IsNullOrEmpty(ViewModelNamespaceOverwrite))
                {
                    desiredVmNameSpace = ViewModelNamespaceOverwrite;
                }

                string desiredVmXName = string.Empty;
                string desiredVmXClassName = string.Empty;

                if (!string.IsNullOrEmpty(ViewModelNameOverwrite))
                {
                    // Overwrite the names i.e. x:Class x:Name
                    desiredVmXClassName = ViewModelNameOverwrite;
                }
                else
                {
                    if ((!string.IsNullOrEmpty(OldViewNamePart)) && (!string.IsNullOrEmpty(NewViewModelNamePart)))
                    {
                        // Form a desired View Model name (i.e. class name) for a type.
                        // If a type name (i.e. class name) contains "View"(default see prop."OldNamePart"), it will be replaced on "ViewModel"(default see prop."NewNamePart").
                        desiredVmXClassName = parentFullClassNameSections[parentFullClassNameSections.Count - 1];
                        if (!desiredVmXClassName.Contains(NewViewModelNamePart))
                        {
                            desiredVmXClassName = desiredVmXClassName.Replace(OldViewNamePart, NewViewModelNamePart);
                        }
                        if (!string.IsNullOrEmpty(parentXClass.XNameForXClass))
                        {
                            desiredVmXName = parentXClass.XNameForXClass;
                            if (!desiredVmXName.Contains(NewViewModelNamePart))
                            {
                                desiredVmXName = desiredVmXName.Replace(OldViewNamePart, NewViewModelNamePart);
                            }
                        }
                    }
                }

#if DEBUG
                if (!string.IsNullOrEmpty(desiredVmXClassName))
                {
                    Debug.WriteLine("#INFO#-A-W-V-M# Desired x:Class, Type: {0}.{1}", desiredVmNameSpace, desiredVmXClassName);
                }

                if (!string.IsNullOrEmpty(desiredVmXName))
                {
                    Debug.WriteLine("#INFO#-A-W-V-M# Desired x:Name,  Type: {0}.{1} ", desiredVmNameSpace, desiredVmXName);
                }
#endif
                var candidateTypes = BindHelper.ResolveTypesByNameSpace(desiredVmNameSpace);

                foreach (Type item in candidateTypes)
                {
                    WiringCandidatePair candidateVmType = new WiringCandidatePair();
                    candidateVmType.XClass = parentXClass;
                    candidateVmType.WiringType = item;
                    candidateVmType.WiringName = item.Name;
                    candidateVmType.DesiredXCLassName = desiredVmXClassName;
                    candidateVmType.DesiredXName = desiredVmXName;
                    viewModelCandidates.Add(candidateVmType);
                    FillCandidateNameRanks(candidateVmType);
                    // Process attributes for a class
#if !WINDOWS_UWP
                    var attribV = Attribute.GetCustomAttributes(item, typeof(ViewModelClassAliasAttribute));
#else
                    var attribV = item.GetTypeInfo().GetCustomAttributes(typeof(ViewModelClassAliasAttribute));
#endif
                    if (attribV != null)
                    {
                        foreach (Attribute att in attribV)
                        {
                            var list = ((ViewModelClassAliasAttribute)att).Aliases;
                            foreach (var alias in list)
                            {
                                candidateVmType = new WiringCandidatePair();
                                candidateVmType.XClass = parentXClass;
                                candidateVmType.WiringType = item;
                                candidateVmType.WiringName = alias;
                                candidateVmType.DesiredXCLassName = desiredVmXClassName;
                                candidateVmType.DesiredXName = desiredVmXName;
                                viewModelCandidates.Add(candidateVmType);
                                FillCandidateNameRanks(candidateVmType);
                            }
                        }
                    }
                }
            }

            WiringCandidatePair locatedItem = null;
            if (viewModelCandidates.Count == 0)
            {
                Debug.WriteLine("#ERROR#-A-W-V-M# Cannot locate candidate Types in namespace: " + desiredVmNameSpace);
                return null;
            }

            List<WiringCandidatePair> wiringTemp;

            if (locatedItem == null)
            {
                wiringTemp = viewModelCandidates.Where(itm => itm.IsXNameMatch).ToList();
                if (wiringTemp.Count > 0)
                {
                    locatedItem = wiringTemp[0];
                }
            }

            if (locatedItem == null)
            {
                wiringTemp = viewModelCandidates.Where(itm => itm.IsXClassNameMatch).ToList();
                if (wiringTemp.Count > 0)
                {
                    locatedItem = wiringTemp[0];
                }
            }

            if ((locatedItem == null) && (UseMaxNameSubMatch))
            {
                //if (locatedItem == null)
                {
                    wiringTemp = viewModelCandidates.Where(itm => itm.IsXNameSubMatch).ToList();
                    if (wiringTemp.Count > 0)
                    {
                        locatedItem = wiringTemp[0];
                    }
                }

                if (locatedItem == null)
                {
                    wiringTemp = viewModelCandidates.Where(itm => itm.IsXClassNameSubMatch).ToList();
                    if (wiringTemp.Count > 0)
                    {
                        locatedItem = wiringTemp[0];
                    }
                }
            }

            if (locatedItem == null)
            {
                Debug.WriteLine("#ERROR#-A-W-V-M# Cannot locate proper candidate Types in: " + desiredVmNameSpace);
                return null;
            }
#if DEBUG
            else
            {
                Debug.WriteLine("#INFO#-A-W-V-M# Located Type: " + locatedItem.WiringFullName);
            }
#endif
            object resolvedObject = null;
            // Resolve over the IOC container - the first.
            if (ResolveIocContainer && BindHelper.IsIocContainerActive)
            {
                if (ServiceProvider != null)
                {
                        try { resolvedObject = ServiceProvider.GetService(locatedItem.WiringType); } catch { }
#if DEBUG
                        if (resolvedObject != null)
                        {
                            Debug.WriteLine("#INFO#-A-W-V-M# Resolved with IoC,  GetInstance(typeof({0}))", new object[] { locatedItem.WiringFullName});
                        }
#endif
                }
                else
                {
                    try { resolvedObject = Activator.CreateInstance(locatedItem.WiringType); } catch { }
#if DEBUG
                    if (resolvedObject != null)
                    {
                        Debug.WriteLine("#INFO#-A-W-V-M# Resolved with Activator Type: " + locatedItem.WiringFullName);
                    }
#endif
                }
            }


            // Resolve over the resources container the second.
            if ((ResolveResources) && (resolvedObject == null))
            {
                if (!string.IsNullOrEmpty(locatedItem.DesiredXName))
                {
                    resolvedObject = BindHelper.LocateResource(frameworkElement, locatedItem.DesiredXName);
#if DEBUG
                    if (resolvedObject != null)
                    {
                        Debug.WriteLine("#INFO#-A-W-V-M# Resolved with WPF Resource Key: " + locatedItem.DesiredXName);
                    }
#endif
                }
                resolvedObject = BindHelper.LocateResource(frameworkElement, locatedItem.WiringTypeName);
#if DEBUG
                if (resolvedObject != null)
                {
                    Debug.WriteLine("#INFO#-A-W-V-M# Resolved with WPF Resource Key: " + locatedItem.WiringTypeName);
                }
#endif
                if (resolvedObject == null)
                {
                    resolvedObject = BindHelper.LocateResource(frameworkElement, locatedItem.WiringFullName);
#if DEBUG
                    if (resolvedObject != null)
                    {
                        Debug.WriteLine("#INFO#-A-W-V-M# Resolved with WPF Resource Key: " + locatedItem.WiringFullName);
                    }
#endif
                }
                if (resolvedObject == null)
                {
                    resolvedObject = BindHelper.LocateResource(frameworkElement, locatedItem.WiringType);
#if DEBUG
                    if (resolvedObject != null)
                    {
                        Debug.WriteLine("#INFO#-A-W-V-M# Resolved with WPF Resource Key type of : " + locatedItem.WiringFullName);
                    }
#endif
                }
            }


            // Create instance over the real type the third.
            if ((ResolveCreateInstance) && (resolvedObject == null))
            {
                try { resolvedObject = Activator.CreateInstance(locatedItem.WiringType); }
                catch
                {
                    // ignored
                }
#if DEBUG
                if (resolvedObject != null)
                {
                    Debug.WriteLine("#INFO#-A-W-V-M# Resolved with Activator Type: " + locatedItem.WiringFullName);
                }
#endif
            }

            if (resolvedObject != null)
            {
                // setup resolved object to property
                DependencyProperty target = BindHelper.LocateDependencyProperty(TargetPropertyName, frameworkElement);
                if (target != null)
                {
                    frameworkElement.SetValue(target, resolvedObject);
                    // store auto resolved ViewModel into
                    BindXAML.SetAutoWiredViewModel(frameworkElement, resolvedObject);
                }
            }

#if DEBUG
            if (resolvedObject == null)
            {
                Debug.WriteLine("#ERROR#-A-W-V-M# Cannot Resolve Type: " + locatedItem.WiringFullName);
            }
#endif
            return resolvedObject;
        }

        void FillCandidateNameRanks(WiringCandidatePair candidateVmType)
        {
            var efffectiveAliasSplit = BindHelper.SplitNameByCase(candidateVmType.WiringName);
            var desiredXClassNameSplit = BindHelper.SplitNameByCase(candidateVmType.DesiredXCLassName);
            var desiredXNameSplit = BindHelper.SplitNameByCase(candidateVmType.DesiredXName);
            candidateVmType.WiringNameRank = efffectiveAliasSplit.Count;
            candidateVmType.DesiredXClassNameRank = desiredXClassNameSplit.Count;
            candidateVmType.DesiredXNameRank = desiredXNameSplit.Count;
            candidateVmType.DesiredXClassNameMatchToWiringNameRank = BindHelper.CalculateMatchingRank(desiredXClassNameSplit, efffectiveAliasSplit);
            candidateVmType.DesiredXNameMatchToWiringNameRank = BindHelper.CalculateMatchingRank(desiredXNameSplit, efffectiveAliasSplit);

        }

    }
}
