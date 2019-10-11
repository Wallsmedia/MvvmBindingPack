// 
//  MVVM-WPF-NetCore Markup, Binding and other Extensions.
//  Copyright © 2013-2018 Alexander Paskhin /paskhin@hotmail.co.uk/ All rights reserved.
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
using System.Reflection;
using System.Diagnostics;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
#else
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
#if BEHAVIOR
using System.Windows.Interactivity;
#endif
using System.Windows.Threading;
using System.Reflection;
using System.Windows.Markup;
using System.Diagnostics;
using System.Windows.Data;
#endif

namespace MvvmBindingPack
{

    /// <summary>
    /// XAML WPF and Win Store Application -  MVVM Behavior Extension Enhancer.
    /// <para>The Behavior Extension Enhancer logic is based on using of the x:Name directive. x:Name directive uniquely identifies XAML-defined elements in a XAML namescope. </para>
    /// <para> AutoWireViewConrols wires and binds x:Named XAML-defined elements or View XAML UI elements to View Model properties, methods and fields. </para>
    /// <para>The View (XAML) element targets are dependency properties or routing events. They are subject of binding to properties,fields and methods in a View Model class.</para>
    /// <para>__</para>
    /// 
    /// View Control General Wiring - Binding rules:
    /// <para>__</para>
    /// 
    /// <para> - The x:Named View (XAML) element can be bind one to many distinguish properties, fields or methods, in a View Model.</para>
    /// <para> - The View Model properties has a priority to bind over the methods. </para>
    /// <para> - The fist found match will be bind fist. The order of the declaration may be applicable in ambiguous cases.</para>
    /// <para> - It is used always the full name match, a part sub match is used as an option, see the 'UseMaxNameSubMatch' property.</para>
    /// <para> - One to One: The View Model property or event can be bind only once for one x:Named View XAML-defined element.</para> 
    /// <para> - View element targets will be bind to View Model element targets.</para> 
    /// <para> - The desired target names should be defined in the View Model.</para>
    /// <para> - The View Model name without targets will be ignored.</para>
    /// <para>__</para>
    /// 
    /// <para> View Model ==> View name matching rules:</para>
    /// <para> The name is split into parts by capital letter or '_'. </para>
    /// <para>   The character '_' is not included into parts.</para>
    /// <para>  - The View x:Name="_Example_Name_"   -> The x:Name is ignored as it starts with "_".</para>
    /// <para>__</para>
    /// 
    /// <para>  - The View name "Example_Name_"  will considered as matching parts {"Example","Name"}</para>
    /// <para>  - The View name "ExampleName"  will considered as matching parts {"Example","Name"}</para>
    /// <para>  - The View name "exampleName_Ver1"  will considered as matching parts {"example","Name","Ver1"}</para>
    /// <para>  - The attached property or event name should be set in format "TypeOwner.Name" example "Grid.Row", "Mouse.MouseMove" ...</para>
    /// <para>__</para>
    ///  
    /// <para> Examples of wiring the View Model method to the View event:</para>
    /// <para> Wiring goal: the View element with x:Name="Example_Name_"  and event "Click" will be wired/set to a method handler in the View Model. </para>
    /// <para> The View Model wiring definition variants:</para>
    /// <para>__</para>
    /// 
    /// <para> *1*  Without any attributes</para>
    /// <para>     void  Example_Name_Click(...){}  or;</para>
    /// <para>     void  ExampleName_Click(...){} </para>   
    /// <para>__</para>
    ///       
    /// <para> *2*  With  attribute [ViewTarget(...)]</para>
    /// <para>     [ViewTarget("Click")]</para>
    /// <para>     void  Example_Name(...){}  or;   </para>
    /// <para>__</para>
    ///     
    /// <para>     [ViewTarget("Click")]</para>
    /// <para>     void  ExampleName_BadTag(...){}    </para>
    /// <para>__</para>
    /// 
    /// <para> *3*  With attribute [ViewXNameAlias(...)]</para>
    /// <para>     [ViewXNameAlias("ExampleName","Click")]</para>
    /// <para>     void  AbracadbraName(...){}  or;</para>
    /// <para>__</para>
    /// 
    /// <para>     [ViewXNameAlias("Example_Name","Click")</para>
    /// <para>     void  _AbracadbraName(...){} /* the name starting with "_" will be ignored, but the attribute don't */ or;  </para>
    /// <para>__</para>
    /// 
    /// <para>     [ViewXNameAlias("Example_Name_","Click")]</para>
    /// <para>     void  Abracadbra_Name(...){}    </para>
    /// <para>__</para>
    /// 
    /// <para> Examples of wiring the View Model method to the View property:</para>
    /// <para> Wiring goal: the View element with x:Name="Example_Name_"  and property "Content" will be {Binding ...}  to a method handler in the View Model. </para>
    /// <para> The View Model wiring definition variants:</para>
    /// <para>__</para>
    /// 
    /// <para> *1*  Without any attributes</para>
    /// <para>     string  Example_Name_Content {get;set;}  or;</para>
    /// <para>__</para>
    ///     
    /// <para>     string  ExampleName_Content {get;set;}</para>
    /// <para>__</para>
    /// 
    /// <para> *2*  With  attribute [ViewTarget(...)]</para>
    /// <para>     [ViewTarget("Content")]</para>
    /// <para>     string  Example_Name {get;set;}  or;   </para>
    /// <para>__</para>
    ///     
    ///  <para>    [ViewTarget("Content")]</para>
    ///  <para>    string  ExampleName_BadTag {get;set;}   </para>
    /// <para>__</para>
    /// 
    /// <para> *3*  With attribute [ViewXNameAlias(...)]</para>
    /// <para>     [ViewXNameAlias("ExampleName","Content")]</para>
    /// <para>     string  AbracadbraName{get;set;}    or;</para>
    /// <para>__</para>
    /// 
    /// <para>     [ViewXNameAlias("Example_Name","Content")]</para>
    /// <para>     string  _AbracadbraName{get;set;}   /* the name starting with "_" will be ignored, but the attribute don't */ or;  </para>
    /// <para>__</para>
    /// 
    /// <para>     [ViewXNameAlias("Example_Name_","Content")]</para>
    /// <para>     string  Abracadbra_Name{get;set;}    </para>
    /// <para>__</para>
    /// 
    /// <para> Examples of wiring the View Model method to the View "Command" property:</para>
    /// <para> Wiring goal: the View element with x:Name="Example_Name_"  and property "Command" will be wire/set to a property in the View Model. </para>
    /// <para> The View Model wiring definition variants:</para>
    /// <para>__</para>
    ///
    /// <para> *1*  Without any attributes</para>
    /// <para>     ICommand  Example_Name_Command {get;set;}  or;</para>
    /// <para>__</para>
    ///     
    /// <para>     string  ExampleName_Command {get;set;}</para>
    /// <para>__</para>
    /// 
    /// <para> *2*  With  attribute [ViewTarget(...)]</para>
    /// <para>     [ViewTarget("Command")]</para>
    /// <para>     ICommand  Example_Name {get;set;}  or;   </para>
    /// <para>__</para>
    ///     
    /// <para>     [ViewTarget("Command")]</para>
    /// <para>     ICommand  ExampleName_BadTag {get;set;}   </para>
    /// <para>__</para>
    /// 
    /// <para> *3*  With attribute [ViewXNameAlias(...)]</para>
    /// <para>     [ViewXNameAlias("ExampleName","Command")]</para>
    /// <para>     ICommand  AbracadbraName{get;set;}    or;</para>
    /// <para>__</para>
    /// 
    /// <para>     [ViewXNameAlias("Example_Name","Command")]</para>
    /// <para>     ICommand  _AbracadbraName{get;set;}   /* the name starting with "_" will be ignored, but the attribute don't */ or;  </para>
    /// <para>__</para>
    /// 
    ///  <para>    [ViewXNameAlias("Example_Name_","Command")]</para>
    ///  <para>    ICommand  Abracadbra_Name{get;set;}</para>    
    /// <para>__</para>
    /// 
    /// <para> *4* Separate "Execute" and "CanExecute" wiring</para>
    /// <para>     [ViewXNameAlias("Example_Name", "Command.Execute")]</para>
    /// <para>     void NameVM2MExecute(object obj){...}</para>
    /// <para>__</para>
    /// 
    /// <para>     [ViewXNameAlias("Example_Name", "Command.CanExecute")]</para>
    /// <para>     bool Method_NameVM2MCanExecute(object obj){....} or;</para>
    /// <para>__</para>
    /// 
    ///  <para>    [ViewXNameAlias("Example_Name", "Command.CanExecute")]</para>
    ///  <para>    bool Prop_NameVM2MCanExecute{get;set;}</para>
    /// <para>__</para>
    /// 
    /// <para> Examples of wiring(just copy) the View Model fields to the View property:</para>
    /// <para> Wiring goal: the View element with x:Name="Example_Name_"  and property "Width" will be wired/set/copied to a property in the View Model. </para>
    /// <para> The View Model wiring definition variants:</para>
    /// <para>__</para>
    /// 
    /// <para> *1*  With attribute [ViewXNameAlias(...)]</para>
    /// <para>     [ViewXNameAlias("ExampleName","Width")]</para>
    /// <para>      string _textAndMsgLabelTxtC = "Content was copied from the field"; /*The field name will always be ignored.*/</para>
    /// <para>__</para>
    /// 
    /// <para> View ==> View Model name mapping/linking rules. Sometimes, very often there is a vital case to have a link from </para>
    /// <para> View Model to a View element or property or event.</para>
    /// <para>__</para>
    /// 
    /// <para> Examples of wiring/referencing  the View  fields to the View Model:</para>
    /// <para>__</para>
    /// 
    /// <para> *1*  Get a reference/link to the element type Label with x:Name="LabelXNameVM2M" </para>
    /// <para>     [ViewXNameSourceObjectMapping("LabelXNameVM2M")]</para>
    /// <para>     private object _LabelXNameVM2M; // can be used the 'Label' type  instead of the 'Object' type.</para>
    /// <para>__</para>
    /// 
    /// <para> *2*  Get a reference/link to the property "Content" of the element type Label with x:Name="LabelXNameVM2M"  </para>
    /// <para>     [ViewXNameSourceTargetMapping("LabelXNameVM2M", "Content")]</para>
    /// <para>     private ViewXNameSourceTarget _LabelXNameVM2MContent;</para>
    /// <para>__</para>
    ///     
    /// </summary>

#if BEHAVIOR
    public class AutoWireViewConrols : Behavior<FrameworkElement>
#else
    public class AutoWireViewConrols
#endif
    {
        const string ConstructorTarget = "#@this@#";
        /// <summary>
        /// Class container.
        /// Holds the resolved View XAML-defined element target metadata.
        /// It may be a property or event metadata of the x:Named View XAML-defined element target.
        /// </summary>
        [DebuggerDisplay("TargetName = {TargetName}, IsProperty = {IsProperty}, IsEvent = {IsEvent}, IsObject = {IsObject}")]
        internal class ResolvedViewTargetName
        {
            public ViewXNameCandidate ViewNameCandidate { get; set; }
            public string TargetName { get; set; }
            public PropertyInfo ResolvedPropertyInfo { get; set; }
            public DependencyProperty ResolvedDependencyProperty { get; set; }
#if !WINDOWS_UWP
            public DependencyPropertyKey ResolvedDependencyPropertyKey { get; set; }
#endif
            public EventInfo ResolvedEventInfo { get; set; }
            public RoutedEvent ResolvedRoutedEvent { get; set; }
            public bool IsProperty => ResolvedPropertyInfo != null || ResolvedDependencyProperty != null;
            public bool IsEvent => ResolvedEventInfo != null || ResolvedRoutedEvent != null;
            public bool IsObject => TargetName == ConstructorTarget;
            public bool IsResolved => IsProperty || IsEvent || IsObject;
        }

        /// <summary>
        /// Class container.
        /// Holds the resolved View XAML-defined element target metadata.
        /// It may be a property or event metadata of the x:Named View XAML-defined element target.
        /// </summary>
        internal class TargetLinkParameters
        {
            /// <summary>
            /// If it is true to register the handler such that it is invoked even when the routed event is marked handled in its event data;
            /// false to register the handler with the default condition that it will not be invoked if the routed event is already marked handled.
            /// </summary>
            public bool? HandledEventsToo;

#if !WINDOWS_UWP
            /// <summary>
            /// The DataErrorValidationRule is a built-in validation rule that checks for errors that are raised by the IDataErrorInfo
            /// implementation of the source object. If an error is raised, the binding engine creates a ValidationError with the error 
            /// and adds it to the Validation.Errors collection of the bound element. The lack of an error clears this validation feedback,
            /// unless another rule raises a validation issue.
            /// </summary>
            public bool? ValidatesOnDataErrors;

            /// <summary>
            /// When ValidatesOnNotifyDataErrors is true, the binding checks for and reports errors that are raised by a data source that implements INotifyDataErrorInfo.
            /// </summary>
            public bool? ValidatesOnNotifyDataErrors;

            /// <summary>
            /// The ExceptionValidationRule is a built-in validation rule that checks for exceptions that are thrown during the update
            /// of the source property. If an exception is thrown, the binding engine creates a ValidationError with the exception and
            /// adds it to the Validation.Errors collection of the bound element. The lack of an error clears this validation feedback, 
            /// unless another rule raises a validation issue.
            /// </summary>
            public bool? ValidatesOnExceptions;
#endif

            /// <summary>
            ///  Describes the direction of the data flow in a binding.
            /// </summary>
            public BindingMode? BindingMode;
        }

        /// <summary>
        /// Internal class container.
        /// Holds the 'x:Name' or View XAML-defined element with XAML-defined element target names. 
        /// </summary>
        [DebuggerDisplay("FullXName = {FullXName}, Rank = {Rank}")]
        internal class ViewXNameCandidate
        {
            public string FullXName { get; set; }
            public string BaseXName { get; set; }
            public int Rank { get { return SplitFullXName.Count; } }
            public Type XamlElementType { get; set; }
            public Object XamlElementObject { get; set; }
            public HashSet<string> BaseXNameSubNames { get; set; } = new HashSet<string>();
            public List<string> SplitFullXName { get; set; } = new List<string>();
        }

        /// <summary>
        /// Internal class container.
        /// Holds the View Model property or method name and includes XAML-defined element target names.
        /// </summary>
        [DebuggerDisplay("Name = {MemberName}, IsProperty = {IsProperty}, IsMethod = {IsMethod}, FldInfo ={FldInfo}")]
        class ViewModelMemberCandidate
        {
            public string MemberName { get; set; }
            public MethodInfo MethdInfo { get; set; }
            public PropertyInfo PropInfo { get; set; }
            public FieldInfo FldInfo { get; set; }
            public Type MemberType { get; set; }
            public Object MemberObject { get; set; }
            public TargetLinkParameters BindingParameters { get; set; }
            public bool CopyToView { get; set; }
            public bool IsProperty => PropInfo != null;
            public bool IsMethod => MethdInfo != null;
            public bool IsField => FldInfo != null;

            public List<string> SplitMemberName { get; set; } = new List<string>();
            public HashSet<string> TargetLinkNames { get; set; } = new HashSet<string>();

        }

        /// <summary>
        /// Internal class container.
        /// Holds the matching pair of binding candidates.
        /// </summary>
        [DebuggerDisplay("MatchingName = {MatchingName}, XNameRank = {XNameRank}, MatchingRank = {MatchingRank}, ExactMatch = {ExactMatch}, SubNameMatch = {SubNameMatch}")]
        class SelectedBindingPair
        {
            public ViewXNameCandidate ViewNameCandidate { get; set; }
            public ViewModelMemberCandidate ViewModelNameCandidate { get; set; }
            public int XNameRank { get; set; }
            public int MatchingRank { get; set; }
            public int NameRank { get; set; }
            public string MatchingName { get; set; }
            public bool ExactMatch => (XNameRank == MatchingRank) && (XNameRank == NameRank) && (MatchingRank > 0);
            public bool SubNameMatch => (XNameRank == MatchingRank) && (XNameRank != NameRank) && (MatchingRank > 0);
        }

        /// <summary>
        /// Constant definitions.
        /// </summary>
        private const string CommandConst = "Command";
        private const string CommandExecuteConst = "Command.Execute";
        private const string CommandCanExecuteConst = "Command.CanExecute";
        private const string DataContextConst = "DataContext";

#if !WINDOWS_UWP
        private readonly Dictionary<UIElement, RoutedEventArgs> _delayedListOfBindedLoadedEvents = new Dictionary<UIElement, RoutedEventArgs>();
#endif

        /// <summary>
        /// The default string collection contains the prefixes of the internal, auxiliary class methods that should be ignored when 
        /// they are reflected from the View Model class type.
        /// </summary>
        public static List<string> KnownExcludeMethodPrefixes { get; } = new List<string>
                                                                                {
                                                                                    "get_", "set_", "add_", "remove_",
                                                                                   "GetFieldInfo", "FieldGetter", "FieldSetter",
                                                                                   "MemberwiseClone", "Finalize", "GetType",
                                                                                   "GetHashCode", "ReferenceEquals", "Equals", "ToString"
                                                                                };

        private DependencyProperty SourceDependencyProperty { get; set; } = FrameworkElement.DataContextProperty;
        /// <summary>
        /// Source dependency property name. The property value will be used as a reference to the View Model object.
        /// Default dependency property name is "DataContext".
        /// </summary>
        public string SourcePropertyName { get; set; } = DataContextConst;

        /// <summary>
        ///  Gets or sets the object to use as the wiring source i.e. View Model instance. It has priority over 'SourcePropertyName'.
        /// It is a "back-door" feature which allows to setup the source object. If it is not set on, by default,
        /// the markup extension will use the defined DataContext property value or redefined by 'SourcePropertyName'.
        /// There may be used {IocBinding ...} or other "agnostic" mark up extension(not {Binding ...}) which provides by the independent way a source object.
        /// </summary>
        public object Source { get; set; }

        /// <summary>
        /// Defines the additional sub matching rule when a view name candidate (x:Name without targets) compared to a view model candidate name.
        /// If it is true, the view model candidate name is considered as a match to a view name if it starts with the 'view name'.
        /// Example: view name "WindowAutoBindViewModel" match to view modelName "WindowAutoBindViewModelSubMath".
        /// </summary>
        public bool UseMaxNameSubMatch { get; set; }

        /// <summary>
        /// Include visual tree x:Named elements onto wiring.
        /// Default value is false.
        /// </summary>
        public bool IncludeVisualTreeNames { get; set; }

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
            base.OnDetaching();
        }
#endif

        /// <summary>
        /// Executes wiring or binding of XAML-defined elements to a View Model.
        /// It wires or binds all x:Named elements of the XAML node tree to a View Model class.
        /// It starts with current XAML node, defined by a 'dependencyObject' parameter, and goes down through descenders (children). 
        /// It's called when this class has been processed by "BindXAML.ProcessMvvmExtensions" 'fake' attached property collection. 
        /// </summary>
        /// <param name="dependencyObject">Parent, root the Dependency object.</param>
        public void Execute(DependencyObject dependencyObject)
        {
            FrameworkElement frameworkElement = dependencyObject as FrameworkElement;

            if (frameworkElement == null)
            {
                return;
            }

#if !WINDOWS_UWP
            if (frameworkElement.IsLoaded)
            {
                FrameworkElementLoaded(dependencyObject, new RoutedEventArgs());
            }
#endif

            // Setup a delay in processing of the FrameworkElement object until it is loaded.
            frameworkElement.Loaded += FrameworkElementLoaded;
        }

        /// <summary>
        /// Executes wiring or binding logic after the FrameworkElement object has loaded.
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

            object viewModelObject;

            if (Source != null)
            {
                viewModelObject = Source;
            }
            else
            {
                if (SourcePropertyName != DataContextConst)
                {
                    var tmp = BindHelper.LocateDependencyProperty(SourcePropertyName, frameworkElement);
                    if (tmp != null)
                    {
                        SourceDependencyProperty = tmp;
                    }
                }

                viewModelObject = frameworkElement.GetValue(SourceDependencyProperty);
            }

            if (viewModelObject != null)
            {
                ProcessViewAutoBinding(frameworkElement, viewModelObject);
            }

#if !WINDOWS_UWP
            else
            {
                DependencyPropertyDescriptor dppDescr = DependencyPropertyDescriptor.FromProperty(SourceDependencyProperty, SourceDependencyProperty.OwnerType);
                dppDescr.AddValueChanged(frameworkElement, FrameworkElementSourcePropertyChanged);
            }
#endif

        }

#if !WINDOWS_UWP
        /// <summary>
        /// Processes the wiring or binding logic after the SourcePropertyName(default "DataContext") property has changed. 
        /// </summary>
        /// <param name="sender">The FrameworkElement element.</param>
        /// <param name="eventArgs">The state information and event data associated with a routed event.</param>
        public void FrameworkElementSourcePropertyChanged(object sender, EventArgs eventArgs)
        {
            FrameworkElement frameworkElement = sender as FrameworkElement;
            if (frameworkElement == null)
            {
                return;
            }

            DependencyPropertyDescriptor dppDescr = DependencyPropertyDescriptor.FromProperty(SourceDependencyProperty, SourceDependencyProperty.OwnerType);
            dppDescr.RemoveValueChanged(frameworkElement, FrameworkElementSourcePropertyChanged);

            //    try { viewModelObject = frameworkElement.GetValue(BindXAML.AutoWiredViewModelProperty); }
            //    catch { }

            var viewModelObject = frameworkElement.DataContext;

            if (viewModelObject != null)
            {
                ProcessViewAutoBinding(frameworkElement, viewModelObject);
            }
        }
#endif

        /// <summary>
        /// Wires or binds all x:Named XAML-defined elements, starting with a node defined by 
        /// a 'frameworkElement' parameter, to a View Model defined by a 'viewModelObject' parameter. 
        /// 
        /// </summary>
        /// <param name="frameworkElement">The FrameworkElement object.</param>
        /// <param name="viewModelObject">The View Model object.</param>
        public void ProcessViewAutoBinding(FrameworkElement frameworkElement, object viewModelObject)
        {

            List<Tuple<Type, object>> locatedViewModels = BindHelper.LocateAppendedViewModels(viewModelObject);

            // Obtain View Model method candidates.
            List<ViewModelMemberCandidate> methodCandidates = GetMethodCandidates(locatedViewModels, viewModelObject);

            // Obtain View Model property candidates.
            List<ViewModelMemberCandidate> propertyCandidates = GetPropertyCandidates(locatedViewModels, viewModelObject);

            // Obtain View Model property candidates.
            List<ViewModelMemberCandidate> fieldCandidates = GetFieldCandidates(locatedViewModels, viewModelObject);

            // Obtain View X-Name candidates.
            List<ViewXNameCandidate> xNamecandidates = GetXNamesCandidates(frameworkElement);

            // Temporary collections.
            List<SelectedBindingPair> bindingMethodCand = new List<SelectedBindingPair>();
            List<SelectedBindingPair> bindingPropertyCand = new List<SelectedBindingPair>();
            List<SelectedBindingPair> bindingFieldCand = new List<SelectedBindingPair>();

            foreach (var xNameEntity in xNamecandidates)
            {
                bindingMethodCand.Clear();
                bindingPropertyCand.Clear();
                bindingFieldCand.Clear();

                // Select property candidates for x:Name.
                foreach (var property in propertyCandidates)
                {
                    int matchingRank = BindHelper.CalculateMatchingRank(xNameEntity.SplitFullXName, property.SplitMemberName);
                    if (matchingRank > 0)
                    {
                        // Add only if there is a some kind of matching.
                        var pair = new SelectedBindingPair
                        {
                            MatchingName = property.MemberName,
                            MatchingRank = matchingRank,
                            NameRank = property.SplitMemberName.Count,
                            XNameRank = xNameEntity.Rank,
                            ViewModelNameCandidate = property,
                            ViewNameCandidate = xNameEntity
                        };
                        bindingPropertyCand.Add(pair);
                    }
                }

                // Select method candidates for x:Name.
                foreach (var method in methodCandidates)
                {
                    int matchingRank = BindHelper.CalculateMatchingRank(xNameEntity.SplitFullXName, method.SplitMemberName);
                    if (matchingRank > 0)
                    {
                        // Add only if there is a some kind of matching.
                        var pair = new SelectedBindingPair
                        {
                            MatchingName = method.MemberName,
                            MatchingRank = matchingRank,
                            NameRank = method.SplitMemberName.Count,
                            XNameRank = xNameEntity.Rank,
                            ViewModelNameCandidate = method,
                            ViewNameCandidate = xNameEntity
                        };
                        bindingMethodCand.Add(pair);
                    }
                }

                // Select field source candidates for x:Name.
                foreach (var filed in fieldCandidates)
                {
                    int matchingRank = BindHelper.CalculateMatchingRank(xNameEntity.SplitFullXName, filed.SplitMemberName);
                    if (matchingRank > 0)
                    {
                        // Add only if there is a some kind of matching.
                        var pair = new SelectedBindingPair
                        {
                            MatchingName = filed.MemberName,
                            MatchingRank = matchingRank,
                            NameRank = filed.SplitMemberName.Count,
                            XNameRank = xNameEntity.Rank,
                            ViewModelNameCandidate = filed,
                            ViewNameCandidate = xNameEntity
                        };
                        bindingFieldCand.Add(pair);
                    }
                }

                if (bindingMethodCand.Count == 0 && bindingPropertyCand.Count == 0 && bindingFieldCand.Count == 0)
                {
                    // Nothing to bind.
                    continue;
                }

                if (bindingFieldCand.Count != 0)
                {
                    // There is only one case for fields
                    ResolveSourceXNameViewReferences(bindingFieldCand, xNameEntity);

                }

                if (bindingMethodCand.Count != 0 || bindingPropertyCand.Count != 0)
                {


                    ResolveLinksBasedOnViewAndViewModelTargets(bindingMethodCand, bindingPropertyCand, xNameEntity);
                }

            }

#if !WINDOWS_UWP
            // Raise Loaded events that we have bound
            foreach (var loadedEvent in _delayedListOfBindedLoadedEvents)
            {
                try
                {

                    loadedEvent.Key.RaiseEvent(loadedEvent.Value);

                }
                catch
                {
                    // ignored
                }
            }
            _delayedListOfBindedLoadedEvents.Clear();
#endif
        }

        private void ResolveSourceXNameViewReferences(List<SelectedBindingPair> bindingFieldCand, ViewXNameCandidate xNameEntity)
        {
            while (bindingFieldCand.Count != 0)
            {
                SelectedBindingPair fLocatedCandidate = LocateCandidateByXNameWithAnyTargets(bindingFieldCand, UseMaxNameSubMatch);

                if (fLocatedCandidate == null)
                {
                    // Cannot find a candidate, exit the while loop
                    break;
                }

                ResolvedViewTargetName resolvedTarget;
                // filter by View Model target names
                resolvedTarget = ResolveViewTargetNameFromList(fLocatedCandidate.ViewModelNameCandidate.TargetLinkNames, xNameEntity);

                if (!resolvedTarget.IsResolved)
                {
                    bindingFieldCand.Remove(fLocatedCandidate);
                }
                else
                {
                    ExcludeCandidate(bindingFieldCand, fLocatedCandidate);
#if DEBUG
                    PrintLinkedPair(fLocatedCandidate, resolvedTarget.TargetName);
#endif
                    BindFields(resolvedTarget, fLocatedCandidate);
                }
            }
        }

        /// <summary>
        /// Resolves all possible links of x:Named XAML-defined View elements to a View Model.
        /// x:Name has defined targets(i.e. dependency properties on routing events).
        /// x:Name defines targets. It forms a list which is requested to bind.
        /// A View Model properties or methods should have defined targets (View XAML dependency properties on routing events).
        /// Example: x:Name="ActionButton_Content_IsEnable_Click" where Content, IsEnable and Click are targets.
        /// Binds or wires proper candidate pairs.
        /// </summary>
        /// <param name="bindingMethodCand">The list collection of method candidates.</param>
        /// <param name="bindingPropertyCand">The list collection of the property candidates.</param>
        /// <param name="xNameEntity">The xName candidate.</param>
        private void ResolveLinksBasedOnViewAndViewModelTargets(List<SelectedBindingPair> bindingMethodCand, List<SelectedBindingPair> bindingPropertyCand, ViewXNameCandidate xNameEntity)
        {
            while (bindingPropertyCand.Count != 0)
            {
                SelectedBindingPair propertyLocatedCandidate;
                SelectedBindingPair propertyLocatedCommandExecute;
                SelectedBindingPair propertyLocatedCommandCanExecute;

                propertyLocatedCandidate = LocateCandidateByXNameWithAnyTargets(bindingPropertyCand, UseMaxNameSubMatch);

                if (propertyLocatedCandidate == null)
                {
                    // Cannot find a candidate, exit the while loop
                    break;
                }

                ResolvedViewTargetName resolvedTarget;


                // filter by View Model target names
                // Case - II x:Named XAML-defined element without a list of target names(i.e. dependency properties on routing events).
                // Example x:Name="NameNameName".
                // View element targets will be bind to View Model element targets. 
                // The desired target names should be defined in the View Model.
                // The View Model name without targets will be ignored
                resolvedTarget = ResolveViewTargetNameFromList(propertyLocatedCandidate.ViewModelNameCandidate.TargetLinkNames, xNameEntity);

                if (!resolvedTarget.IsResolved)
                {
                    bindingPropertyCand.Remove(propertyLocatedCandidate);
                }
                else
                {
                    ExcludeCandidate(bindingPropertyCand, propertyLocatedCandidate);

                    if (resolvedTarget.TargetName == CommandExecuteConst)
                    {
                        propertyLocatedCommandExecute = propertyLocatedCandidate;

                        // try to locate CommandCanExecuteConst over the properties
                        propertyLocatedCommandCanExecute = LocateCandidateByXNameAndTarget(bindingPropertyCand, CommandCanExecuteConst, UseMaxNameSubMatch);
                        ExcludeCandidate(bindingPropertyCand, propertyLocatedCommandCanExecute);

                        if (propertyLocatedCommandCanExecute == null)
                        {
                            // try to locate CommandCanExecuteConst over the Methods
                            propertyLocatedCommandCanExecute = LocateCandidateByXNameAndTarget(bindingMethodCand, CommandCanExecuteConst, UseMaxNameSubMatch);
                            ExcludeCandidate(bindingMethodCand, propertyLocatedCommandCanExecute);
                        }
#if DEBUG
                        PrintLinkedCmdPair(propertyLocatedCommandExecute, CommandExecuteConst);
                        PrintLinkedCmdPair(propertyLocatedCommandCanExecute, CommandCanExecuteConst);
#endif
                        BindCommand(resolvedTarget, null, propertyLocatedCommandExecute, propertyLocatedCommandCanExecute);

                    }
                    else if (resolvedTarget.TargetName == CommandCanExecuteConst)
                    {
                        propertyLocatedCommandCanExecute = propertyLocatedCandidate;

                        // try to locate CommandExecuteConst
                        propertyLocatedCommandExecute = LocateCandidateByXNameAndTarget(bindingPropertyCand, CommandExecuteConst, UseMaxNameSubMatch);
                        ExcludeCandidate(bindingPropertyCand, propertyLocatedCommandExecute);

                        if (propertyLocatedCommandExecute == null)
                        {
                            propertyLocatedCommandExecute = LocateCandidateByXNameAndTarget(bindingMethodCand, CommandExecuteConst, UseMaxNameSubMatch);
                            ExcludeCandidate(bindingMethodCand, propertyLocatedCommandExecute);
                        }
#if DEBUG
                        PrintLinkedCmdPair(propertyLocatedCommandExecute, CommandExecuteConst);
                        PrintLinkedCmdPair(propertyLocatedCommandCanExecute, CommandCanExecuteConst);
#endif
                        BindCommand(resolvedTarget, null, propertyLocatedCommandExecute, propertyLocatedCommandCanExecute);
                    }
                    else
                    {
#if DEBUG
                        PrintLinkedPair(propertyLocatedCandidate, resolvedTarget.TargetName);
#endif
                        if (resolvedTarget.TargetName == CommandConst)
                        {
                            BindCommand(resolvedTarget, propertyLocatedCandidate, null, null);
                        }
                        else
                        {
                            if ((resolvedTarget.IsProperty && propertyLocatedCandidate.ViewModelNameCandidate.IsProperty) ||
                                (resolvedTarget.IsEvent && propertyLocatedCandidate.ViewModelNameCandidate.IsMethod))
                            {
                                BindOthers(resolvedTarget, propertyLocatedCandidate);
                            }
#if DEBUG
                            else
                            {
                                Debug.WriteLine("#ERROR#-A-W-XAML-V-M# - Cannot bind incompatible type  '{0}' declared in x:Name= '{1}' to {2}", resolvedTarget.TargetName, xNameEntity.FullXName, propertyLocatedCandidate.MatchingName);
                            }
#endif
                        }
                    }
                }

            }

            while (bindingMethodCand.Count != 0)
            {
                SelectedBindingPair methodLocatedCandidate;
                SelectedBindingPair methodLocatedCommandExecuted;
                SelectedBindingPair methodLocatedCommandCanExecuted;

                methodLocatedCandidate = LocateCandidateByXNameWithAnyTargets(bindingMethodCand, UseMaxNameSubMatch);
                if (methodLocatedCandidate == null)
                {
                    // exit while  cannot find something
                    break;
                }

                ResolvedViewTargetName resolvedTarget = ResolveViewTargetNameFromList(methodLocatedCandidate.ViewModelNameCandidate.TargetLinkNames, xNameEntity);
                if (!resolvedTarget.IsResolved)
                {
                    bindingMethodCand.Remove(methodLocatedCandidate);
                }
                else
                {
                    ExcludeCandidate(bindingMethodCand, methodLocatedCandidate);

                    if (resolvedTarget.TargetName == CommandExecuteConst)
                    {
                        methodLocatedCommandExecuted = methodLocatedCandidate;

                        // try to locate CommandCanExecuteConst
                        methodLocatedCommandCanExecuted = LocateCandidateByXNameAndTarget(bindingMethodCand, CommandCanExecuteConst, UseMaxNameSubMatch);
                        ExcludeCandidate(bindingMethodCand, methodLocatedCommandCanExecuted);
#if DEBUG
                        PrintLinkedCmdPair(methodLocatedCommandExecuted, CommandExecuteConst);
                        PrintLinkedCmdPair(methodLocatedCommandCanExecuted, CommandCanExecuteConst);
#endif
                        BindCommand(resolvedTarget, null, methodLocatedCommandExecuted, methodLocatedCommandCanExecuted);
                    }
                    else if (resolvedTarget.TargetName == CommandCanExecuteConst)
                    {
                        methodLocatedCommandCanExecuted = methodLocatedCandidate;

                        // try to locate CommandExecuteConst
                        methodLocatedCommandExecuted = LocateCandidateByXNameAndTarget(bindingMethodCand, CommandExecuteConst, UseMaxNameSubMatch);
                        ExcludeCandidate(bindingMethodCand, methodLocatedCommandExecuted);
#if DEBUG
                        PrintLinkedCmdPair(methodLocatedCommandExecuted, CommandExecuteConst);
                        PrintLinkedCmdPair(methodLocatedCommandCanExecuted, CommandCanExecuteConst);
#endif
                        BindCommand(resolvedTarget, null, methodLocatedCommandExecuted, methodLocatedCommandCanExecuted);
                    }
                    else
                    {

#if DEBUG
                        PrintLinkedPair(methodLocatedCandidate, resolvedTarget.TargetName);
#endif

                        if (resolvedTarget.TargetName == CommandConst)
                        {
                            BindCommand(resolvedTarget, methodLocatedCandidate, null, null);
                        }
                        else
                        {
                            if ((resolvedTarget.IsProperty && methodLocatedCandidate.ViewModelNameCandidate.IsProperty) ||
                                (resolvedTarget.IsEvent && methodLocatedCandidate.ViewModelNameCandidate.IsMethod))
                            {
                                BindOthers(resolvedTarget, methodLocatedCandidate);
                            }
#if DEBUG
                            else
                            {
                                Debug.WriteLine("#ERROR#-A-W-XAML-V-M# - Cannot bind incompatible type  '{0}' declared in x:Name= '{1}' to {2}", resolvedTarget.TargetName, xNameEntity.FullXName, methodLocatedCandidate.MatchingName);
                            }
#endif
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Wires or binds a resolved target (property or event of a XAML-defined UI element) to a View Model located field.
        /// </summary>
        /// <param name="resolvedTarget">The resolved property or event of the framework element.</param>
        /// <param name="locatedCandidate">The bind pair that contains a x:Name and a View Model property or event.</param>
        private void BindFields(ResolvedViewTargetName resolvedTarget, SelectedBindingPair locatedCandidate)
        {
            ViewModelMemberCandidate viewModel = locatedCandidate.ViewModelNameCandidate;
            if (!viewModel.IsField)
            {
                return;
            }
#if DEBUG
            PrintBindingPair(locatedCandidate, resolvedTarget.TargetName);
#endif

            if (resolvedTarget.IsProperty && locatedCandidate.ViewModelNameCandidate.CopyToView)
            {
                try
                {
                    object value;
#if NET40
                    value = viewModel.FldInfo.GetValue(viewModel.MemberObject);
#else
                    value = viewModel.FldInfo.GetValue(viewModel.MemberObject);
#endif
#if WINDOWS_UWP
                    if (resolvedTarget.ResolvedDependencyProperty != null)
#else
                    if (resolvedTarget.ResolvedDependencyProperty != null && !resolvedTarget.ResolvedDependencyProperty.ReadOnly)
#endif
                    {
                        var dependencyObject = resolvedTarget.ViewNameCandidate.XamlElementObject as DependencyObject;
                        if (dependencyObject != null)
                            dependencyObject.SetValue(resolvedTarget.ResolvedDependencyProperty, value);
                    }
#if !WINDOWS_UWP
                    else if (resolvedTarget.ResolvedDependencyPropertyKey != null)
                    {
                        var dependencyObject = resolvedTarget.ViewNameCandidate.XamlElementObject as DependencyObject;
                        if (dependencyObject != null)
                            dependencyObject.SetValue(resolvedTarget.ResolvedDependencyPropertyKey, value);
                    }
#endif
                    else
                    {
                        resolvedTarget.ResolvedPropertyInfo.SetValue(resolvedTarget.ViewNameCandidate.XamlElementObject, value, null);
                    }

                }
                catch
                {
                    // ignored
                }
            }
            else if (resolvedTarget.IsProperty || resolvedTarget.IsEvent)
            {
                ViewXNameSourceTarget xNameSource = new ViewXNameSourceTarget()
                {
                    XName = resolvedTarget.ViewNameCandidate.FullXName,
                    TargetName = resolvedTarget.TargetName,
                    TargetObject = resolvedTarget.ViewNameCandidate.XamlElementObject,
                    TargetType = resolvedTarget.ViewNameCandidate.XamlElementType,
                    PropertyInfo = resolvedTarget.ResolvedPropertyInfo,
                    DependencyProperty = resolvedTarget.ResolvedDependencyProperty,
#if !WINDOWS_UWP
                    DependencyPropertyKey = resolvedTarget.ResolvedDependencyPropertyKey,
#endif
                    EventInfo = resolvedTarget.ResolvedEventInfo,
                    RoutedEvent = resolvedTarget.ResolvedRoutedEvent
                };

                try
                {
#if NET40
                    viewModel.FldInfo.SetValue(viewModel.MemberObject, xNameSource);
#else
                    viewModel.FldInfo.SetValue(viewModel.MemberObject, xNameSource);
#endif
                }
                catch
                {
                    // ignored
                }
            }
            else if (resolvedTarget.IsObject)
            {
                try
                {
#if NET40
                    viewModel.FldInfo.SetValue(viewModel.MemberObject, resolvedTarget.ViewNameCandidate.XamlElementObject);
#else
                    viewModel.FldInfo.SetValue(viewModel.MemberObject, resolvedTarget.ViewNameCandidate.XamlElementObject);
#endif
                }
                catch
                {
                    // ignored
                }
            }
        }

        /// <summary>
        /// Wires or binds a resolved target (property or event of a XAML-defined UI element) to a View Model located candidate (property or method).
        /// </summary>
        /// <param name="resolvedTarget">The resolved property or event of the framework element.</param>
        /// <param name="locatedCandidate">The bind pair that contains a x:Name and a View Model property or event.</param>
        private void BindOthers(ResolvedViewTargetName resolvedTarget, SelectedBindingPair locatedCandidate)
        {
#if DEBUG
            PrintBindingPair(locatedCandidate, resolvedTarget.TargetName);
#endif
            if (resolvedTarget.IsProperty)
            {
#if !WINDOWS_UWP
                if ((resolvedTarget.ResolvedDependencyProperty != null && !resolvedTarget.ResolvedDependencyProperty.ReadOnly) && (locatedCandidate.ViewModelNameCandidate.PropInfo != null))
#else
                if ((resolvedTarget.ResolvedDependencyProperty != null) && (locatedCandidate.ViewModelNameCandidate.PropInfo != null))
#endif
                {
                    var bindingParameters = locatedCandidate.ViewModelNameCandidate.BindingParameters;
                    Binding binding = new Binding
                    {
                        Source = locatedCandidate.ViewModelNameCandidate.MemberObject,
                        Path = new PropertyPath(locatedCandidate.ViewModelNameCandidate.PropInfo.Name)
                    };
#if NET40
                    if (locatedCandidate.ViewModelNameCandidate.PropInfo.GetSetMethod() != null)
#else
                    if (locatedCandidate.ViewModelNameCandidate.PropInfo.SetMethod != null)
#endif
                    {
                        binding.Mode = BindingMode.TwoWay;
                    }
                    if (bindingParameters != null && bindingParameters.BindingMode.HasValue)
                    {
                        binding.Mode = bindingParameters.BindingMode.Value;
                    }
#if !WINDOWS_UWP

#if !NET40
                    if (locatedCandidate.ViewModelNameCandidate.MemberType.GetInterface("INotifyDataErrorInfo") != null)
                    {
                        if (bindingParameters != null && bindingParameters.ValidatesOnNotifyDataErrors.HasValue)
                        {
                            binding.ValidatesOnNotifyDataErrors = bindingParameters.ValidatesOnNotifyDataErrors.Value;
                        }
                    }
#endif
                    if (locatedCandidate.ViewModelNameCandidate.MemberType.GetInterface("IDataErrorInfo") != null)
                    {
                        if (bindingParameters != null && bindingParameters.ValidatesOnDataErrors.HasValue)
                        {
                            binding.ValidatesOnDataErrors = bindingParameters.ValidatesOnDataErrors.Value;
                        }
                    }

                    if (bindingParameters != null && bindingParameters.ValidatesOnExceptions.HasValue)
                    {
                        binding.ValidatesOnExceptions = bindingParameters.ValidatesOnExceptions.Value;
                    }

#endif
                    try
                    {
                        if (binding.Mode == BindingMode.OneTime)
                        {
                            // just copy value from a view model to a view
                            object value = locatedCandidate.ViewModelNameCandidate.PropInfo.GetValue(locatedCandidate.ViewModelNameCandidate.MemberObject, null);
                            var dependencyObject = resolvedTarget.ViewNameCandidate.XamlElementObject as DependencyObject;
                            if (dependencyObject != null)
                                dependencyObject.SetValue(resolvedTarget.ResolvedDependencyProperty, value);
                        }
                        else
                        {
                            // setup binding expression
                            if (resolvedTarget.ViewNameCandidate != null)
                            {
                                var target = resolvedTarget.ViewNameCandidate.XamlElementObject as DependencyObject;
                                if (target != null)
                                    BindingOperations.SetBinding(target, resolvedTarget.ResolvedDependencyProperty, binding);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("#EXCEPTION#-A-W-XAML-V-M# x:Name='{0}' Alias='{1}' Real='{2}'", locatedCandidate.ViewNameCandidate.BaseXName, locatedCandidate.MatchingName,
                            resolvedTarget.TargetName);
                        Debug.WriteLine("#EXCEPTION#-A-W-XAML-V-M# Exception '{0}'", ex.Message);
                    }
                }
#if !WINDOWS_UWP
                else if ((resolvedTarget.ResolvedDependencyPropertyKey != null) && (locatedCandidate.ViewModelNameCandidate.PropInfo != null))
                {
                    var sourceClrPropertyInfo = locatedCandidate.ViewModelNameCandidate.PropInfo;
                    var sourceClrPropertyObj = locatedCandidate.ViewModelNameCandidate.MemberObject;
                    if (sourceClrPropertyInfo != null && sourceClrPropertyObj != null)
                    {
                        try
                        {
                            object value = sourceClrPropertyInfo.GetValue(sourceClrPropertyObj, null);
                            var dependencyObject = resolvedTarget.ViewNameCandidate.XamlElementObject as DependencyObject;
                            if (dependencyObject != null)
                                dependencyObject.SetValue(resolvedTarget.ResolvedDependencyPropertyKey, value);
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine("#EXCEPTION#-A-W-XAML-V-M# - x:Name='{0}' Alias='{1}' Real='{2}'", locatedCandidate.ViewNameCandidate.BaseXName, locatedCandidate.MatchingName,
                                resolvedTarget.TargetName);
                            Debug.WriteLine("#EXCEPTION#-A-W-XAML-V-M# Exception '{0}'", ex.Message);
                        }
                    }
                }
#endif
                else if ((resolvedTarget.ResolvedPropertyInfo != null) && (locatedCandidate.ViewModelNameCandidate.PropInfo != null))
                {
                    // CLR property set as one time copy
                    var targetClrPropertyInfo = resolvedTarget.ResolvedPropertyInfo;
                    var targetClrPropertyObj = resolvedTarget.ViewNameCandidate.XamlElementObject;
                    var sourceClrPropertyInfo = locatedCandidate.ViewModelNameCandidate.PropInfo;
                    var sourceClrPropertyObj = locatedCandidate.ViewModelNameCandidate.MemberObject;
                    if (targetClrPropertyInfo != null && targetClrPropertyObj != null && sourceClrPropertyInfo != null && sourceClrPropertyObj != null)
                    {
                        try
                        {
                            object value = sourceClrPropertyInfo.GetValue(sourceClrPropertyObj, null);
                            targetClrPropertyInfo.SetValue(targetClrPropertyObj, value, null);
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine("#EXCEPTION#-A-W-XAML-V-M# - x:Name='{0}' Alias='{1}' Real='{2}'", locatedCandidate.ViewNameCandidate.BaseXName, locatedCandidate.MatchingName,
                                resolvedTarget.TargetName);
                            Debug.WriteLine("#EXCEPTION#-A-W-XAML-V-M# Exception '{0}'", ex.Message);
                        }
                    }
                }

            }
            else if (resolvedTarget.IsEvent)
            {
#if !WINDOWS_UWP
                if ((resolvedTarget.ResolvedRoutedEvent != null) && (locatedCandidate.ViewModelNameCandidate.MethdInfo != null))
                {
                    var bindingParameters = locatedCandidate.ViewModelNameCandidate.BindingParameters;
                    var sourceClrMethodInfo = locatedCandidate.ViewModelNameCandidate.MethdInfo;
                    var sourceClrMethodObj = locatedCandidate.ViewModelNameCandidate.MemberObject;
                    var targetObject = resolvedTarget.ViewNameCandidate.XamlElementObject as UIElement;
                    var targetEvent = resolvedTarget.ResolvedRoutedEvent;
                    try
                    {
                        object result = BindHelper.EventHandlerDelegateFromMethodInfo(sourceClrMethodObj, targetEvent.HandlerType, sourceClrMethodInfo);
                        if ((result != null) && (targetObject != null))
                        {
                            var dlg = (Delegate)result;
                            if (bindingParameters != null && bindingParameters.HandledEventsToo.HasValue)
                            {
                                targetObject.AddHandler(targetEvent, dlg, bindingParameters.HandledEventsToo.Value);
                            }
                            else
                            {
                                targetObject.AddHandler(targetEvent, dlg);
                            }
                            if (targetEvent == FrameworkElement.LoadedEvent)
                            {
                                /* add the this load event as it was delayed loading but call it later */
                                _delayedListOfBindedLoadedEvents[targetObject] = new RoutedEventArgs(FrameworkElement.LoadedEvent);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("#EXCEPTION#-A-W-XAML-V-M# - x:Name='{0}' Alias='{1}' Real='{2}'", locatedCandidate.ViewNameCandidate.BaseXName, locatedCandidate.MatchingName,
                            resolvedTarget.TargetName);
                        Debug.WriteLine("#EXCEPTION#-A-W-XAML-V-M# Exception '{0}'", ex.Message);
                    }
                }
                else
#endif
                    if ((resolvedTarget.ResolvedEventInfo != null) && (locatedCandidate.ViewModelNameCandidate.MethdInfo != null))
                {
                    var sourceClrMethodInfo = locatedCandidate.ViewModelNameCandidate.MethdInfo;
                    var sourceClrMethodObj = locatedCandidate.ViewModelNameCandidate.MemberObject;
                    var targetObject = resolvedTarget.ViewNameCandidate.XamlElementObject;
                    var targetEvent = resolvedTarget.ResolvedEventInfo;
                    try
                    {
                        object result = BindHelper.EventHandlerDelegateFromMethodInfo(sourceClrMethodObj, targetEvent.EventHandlerType, sourceClrMethodInfo);
                        if (result != null)
                        {
#if WINDOWS_UWP
                            targetEvent.AddMethod.Invoke(targetObject, new object[] { result });
#else
                            targetEvent.AddEventHandler(targetObject, (Delegate)result);
#endif
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("#EXCEPTION#-A-W-XAML-V-M# - x:Name='{0}' Alias='{1}' Real='{2}'", locatedCandidate.ViewNameCandidate.BaseXName, locatedCandidate.MatchingName,
                            resolvedTarget.TargetName);
                        Debug.WriteLine("#EXCEPTION#-A-W-XAML-V-M# Exception '{0}'", ex.Message);
                    }
                }

            }
        }

        /// <summary>
        /// Wires or binds the "Command" property of the x:Name XAML-defined element to the View Model.
        /// </summary>
        /// <param name="resolvedTarget">The resolved "Command" property of the framework element.</param>
        /// <param name="command">The bind pair of a View Model property defined of the ICommand interface.</param>
        /// <param name="commandExecuted">The bind pair of a View Model method defined of the ICommand.Execute interface method.</param>
        /// <param name="commandCanExecuted">The bind pair of a View Model method or property defined of the ICommand.CanExecute interface method.</param>
        private void BindCommand(ResolvedViewTargetName resolvedTarget, SelectedBindingPair command, SelectedBindingPair commandExecuted, SelectedBindingPair commandCanExecuted)
        {
#if DEBUG
            PrintBindedCmdPair(command, "Command");
            PrintBindedCmdPair(commandExecuted, "Command.Execute");
            PrintBindedCmdPair(commandCanExecuted, "Command.CanExecute");
#endif
            if (resolvedTarget.IsProperty)
            {
                ICommand commandProxy = null;

                if (command != null)
                {
                    // get the property value of the command interface
                    if (command.ViewModelNameCandidate.IsProperty)
                    {
                        var propInfo = command.ViewModelNameCandidate.PropInfo;
                        var obj = command.ViewModelNameCandidate.MemberObject;
                        try
                        {
                            commandProxy = (ICommand)propInfo.GetValue(obj, null);
                        }
                        catch
                        {
                            // ignored
                        }
                    }
                }
                else if (commandExecuted != null)
                {
                    Action<object> execute = null;
                    Func<object, bool> canExecute = null;
                    PropertyInfo outerBooleanProperty = null;
                    object propertyBooleanTarget = null;

                    if (commandExecuted.ViewModelNameCandidate.IsProperty)
                    {
                        var propInfo = commandExecuted.ViewModelNameCandidate.PropInfo;
                        var obj = commandExecuted.ViewModelNameCandidate.MemberObject;
                        // get delegate from a property
                        try
                        {
                            var srcDelegate = propInfo.GetValue(obj, null) as Delegate;
                            if (srcDelegate != null)
                            {
#if WINDOWS_UWP
                                execute = (Action<object>)srcDelegate.GetMethodInfo().CreateDelegate(typeof(Action<object>), obj);
#else
                                execute = (Action<object>)Delegate.CreateDelegate(typeof(Action<object>), obj, srcDelegate.Method);
#endif
                            }
                        }
                        catch
                        {
                            // ignored
                        }
                    }
                    else if (commandExecuted.ViewModelNameCandidate.IsMethod)
                    {
                        // get method and create the delegate
                        var methodInfo = commandExecuted.ViewModelNameCandidate.MethdInfo;
                        var obj = commandExecuted.ViewModelNameCandidate.MemberObject;
                        try
                        {
#if WINDOWS_UWP
                            execute = (Action<object>)methodInfo.CreateDelegate(typeof(Action<object>), obj);
#else
                            execute = (Action<object>)Delegate.CreateDelegate(typeof(Action<object>), obj, methodInfo);
#endif
                        }
                        catch
                        {
                            // ignored
                        }
                    }

                    if (commandCanExecuted != null)
                    {
                        if (commandCanExecuted.ViewModelNameCandidate.IsProperty)
                        {
                            // get delegate from a property or property replaced can execute
                            var propInfo = commandCanExecuted.ViewModelNameCandidate.PropInfo;
                            var obj = commandCanExecuted.ViewModelNameCandidate.MemberObject;
                            // get delegate from a property
                            try
                            {
                                var srcDelegate = propInfo.GetValue(obj, null) as Delegate;
                                if (srcDelegate != null)
                                {
#if WINDOWS_UWP
                                    canExecute = (Func<object, bool>)srcDelegate.GetMethodInfo().CreateDelegate(typeof(Func<object, bool>), obj);
#else
                                    canExecute = (Func<object, bool>)Delegate.CreateDelegate(typeof(Func<object, bool>), obj, srcDelegate.Method);
#endif
                                }
                                else
                                {
                                    // or just any property type of bool
                                    if (propInfo.PropertyType == typeof(Boolean))
                                    {
                                        outerBooleanProperty = propInfo;
                                        propertyBooleanTarget = obj;
                                    }
                                }
                            }
                            catch
                            {
                                // ignored
                            }
                        }
                        if (commandCanExecuted.ViewModelNameCandidate.IsMethod)
                        {
                            // get method and create delegate for a command proxy
                            var methodInfo = commandCanExecuted.ViewModelNameCandidate.MethdInfo;
                            var obj = commandCanExecuted.ViewModelNameCandidate.MemberObject;
                            try
                            {
#if WINDOWS_UWP
                                canExecute = (Func<object, bool>)methodInfo.CreateDelegate(typeof(Func<object, bool>), obj);
#else
                                canExecute = (Func<object, bool>)Delegate.CreateDelegate(typeof(Func<object, bool>), obj, methodInfo);
#endif
                            }
                            catch
                            {
                                // ignored
                            }
                        }
                    }
                    if (execute != null)
                    {
                        // Cook the command proxy for method & method or method & property
                        if (outerBooleanProperty != null)
                        {
                            commandProxy = new CommandHandlerProxy(execute, outerBooleanProperty, propertyBooleanTarget);
                        }
                        else
                        {
                            commandProxy = new CommandHandlerProxy(execute, canExecute);
                        }
                    }

                }

                if (commandProxy == null)
                {
                    return;
                }

                if (resolvedTarget.ResolvedDependencyProperty != null)
                {
                    try
                    {
                        var dependencyObject = resolvedTarget.ViewNameCandidate.XamlElementObject as DependencyObject;
                        if (dependencyObject != null)
                            dependencyObject.SetValue(resolvedTarget.ResolvedDependencyProperty, commandProxy);
                    }
                    catch
                    {
                        // ignored
                    }
                }
                else if (resolvedTarget.ResolvedPropertyInfo != null)
                {
                    var targetClrPropertyInfo = resolvedTarget.ResolvedPropertyInfo;
                    var targetClrPropertyObj = resolvedTarget.ViewNameCandidate.XamlElementObject;
                    try
                    {
                        targetClrPropertyInfo.SetValue(targetClrPropertyObj, commandProxy, null);
                    }
                    catch
                    {
                        // ignored
                    }
                }
            }
        }

#if DEBUG
        private void PrintBindingPair(SelectedBindingPair pair, string target)
        {
            if (pair == null) { return; }
            string realName = "";
            string realType = "";
            if (pair.ViewModelNameCandidate.IsProperty)
            {
                realName = pair.ViewModelNameCandidate.PropInfo.Name;
                realType = "Property";
            }
            else if (pair.ViewModelNameCandidate.IsMethod)
            {
                realName = pair.ViewModelNameCandidate.MethdInfo.Name;
                realType = "Method";
            }
            else if (pair.ViewModelNameCandidate.IsField)
            {
                realName = pair.ViewModelNameCandidate.FldInfo.Name;
                realType = "Field";
            }

            Debug.WriteLine("#INFO#-A-W-XAML-V-M# - Type [{0}]  x:Name: {1}.@.{2} <|=V=| BINDED |=VM=|> {3}.@.{4}) ", realType, pair.ViewNameCandidate.FullXName, target, pair.ViewModelNameCandidate.MemberType.FullName, realName);
        }

        private void PrintLinkedPair(SelectedBindingPair pair, string target)
        {
            if (pair == null) { return; }
            string realName = "";
            string realType = "";
            if (pair.ViewModelNameCandidate.IsProperty)
            {
                realName = pair.ViewModelNameCandidate.PropInfo.Name;
                realType = "Property";
            }
            else if (pair.ViewModelNameCandidate.IsMethod)
            {
                realName = pair.ViewModelNameCandidate.MethdInfo.Name;
                realType = "Method";
            }
            else if (pair.ViewModelNameCandidate.IsField)
            {
                realName = pair.ViewModelNameCandidate.FldInfo.Name;
                realType = "Field";
            }

            Debug.WriteLine("#INFO#-A-W-XAML-V-M# - Type [{0}]  x:Name: {1}.@.{2} <|=V=| LINKED |=VM=|> {3}.@.{4}) ", realType, pair.ViewNameCandidate.FullXName, target, pair.ViewModelNameCandidate.MemberType.FullName, realName);
        }

        private void PrintBindedCmdPair(SelectedBindingPair pair, string target)
        {
            if (pair == null) { return; }
            string realName = "";
            string realType = "";
            if (pair.ViewModelNameCandidate.IsProperty)
            {
                realName = pair.ViewModelNameCandidate.PropInfo.Name;
                realType = "Property";
            }
            else if (pair.ViewModelNameCandidate.IsMethod)
            {
                realName = pair.ViewModelNameCandidate.MethdInfo.Name;
                realType = "Method";
            }
            else if (pair.ViewModelNameCandidate.IsField)
            {
                realName = pair.ViewModelNameCandidate.FldInfo.Name;
                realType = "Field";
            }
            Debug.WriteLine("#INFO#-A-W-CMD -V-M# - Type [{0}]  x:Name: {1}.@.{2} <|=V=| BINDED |=VM=|> {3}.@.{4}) ", realType, pair.ViewNameCandidate.FullXName, target, pair.ViewModelNameCandidate.MemberType.FullName, realName);
        }

        private void PrintLinkedCmdPair(SelectedBindingPair pair, string target)
        {
            if (pair == null) { return; }
            string realName = "";
            string realType = "";
            if (pair.ViewModelNameCandidate.IsProperty)
            {
                realName = pair.ViewModelNameCandidate.PropInfo.Name;
                realType = "Property";
            }
            else if (pair.ViewModelNameCandidate.IsMethod)
            {
                realName = pair.ViewModelNameCandidate.MethdInfo.Name;
                realType = "Method";
            }
            else if (pair.ViewModelNameCandidate.IsField)
            {
                realName = pair.ViewModelNameCandidate.FldInfo.Name;
                realType = "Field";
            }
            Debug.WriteLine("#INFO#-A-W-CMD -V-M# - Type [{0}]  x:Name: {1}.@.{2} <|=V=| LINKED |=VM=|> {3}.@.{4}) ", realType, pair.ViewNameCandidate.FullXName, target, pair.ViewModelNameCandidate.MemberType.FullName, realName);
        }

#endif

        private ResolvedViewTargetName ResolveViewTargetNameFromList(IEnumerable<string> targets, ViewXNameCandidate xNameEntity)
        {
            ResolvedViewTargetName result = new ResolvedViewTargetName();
            foreach (var target in targets)
            {
                FillExactTargetInfo(target, xNameEntity, result);
                if (result.IsResolved)
                {
                    break;
                }
            }
            return result;
        }

        //private ResolvedViewTargetName ResolveViewTargetName(string target, ViewXNameCandidate xNameEntity)
        //{
        //    ResolvedViewTargetName result = new ResolvedViewTargetName();
        //    FillExactTargetInfo(target, xNameEntity, result);
        //    return result;
        //}

        /// <summary>
        /// Resolves a XAML-defined element with possible metadata for a target name.
        /// It includes CLR and XAML-defined (WPF) property system metadata.
        /// </summary>
        /// <param name="target">The target name of the property or event of the XAML-defined element.</param>
        /// <param name="xNameEntity">The view XAML-defined element.</param>
        /// <param name="result">The result class reference which will be filled with results.</param>
        private static void FillExactTargetInfo(string target, ViewXNameCandidate xNameEntity, ResolvedViewTargetName result)
        {
            string resolveName = target;
            if ((target == CommandCanExecuteConst) || (target == CommandExecuteConst))
            {
                resolveName = CommandConst;
            }
            result.TargetName = target;
            result.ViewNameCandidate = xNameEntity;
            result.ResolvedPropertyInfo = BindHelper.LocateClrProperty(resolveName, xNameEntity.XamlElementObject);
            result.ResolvedEventInfo = BindHelper.LocateClrEvent(resolveName, xNameEntity.XamlElementObject);
            result.ResolvedDependencyProperty = BindHelper.LocateDependencyProperty(resolveName, xNameEntity.XamlElementObject as DependencyObject);
#if !WINDOWS_UWP
            if (result.ResolvedDependencyProperty != null && result.ResolvedDependencyProperty.ReadOnly)
            {
                result.ResolvedDependencyPropertyKey = BindHelper.LocateDependencyPropertyKey(resolveName, xNameEntity.XamlElementObject as DependencyObject);
            }
#endif
            result.ResolvedRoutedEvent = BindHelper.LocateRoutedEvent(resolveName, xNameEntity.XamlElementObject as DependencyObject);
        }
        /// <summary>
        /// Filters the list of binding pairs by removing the candidate pair from it.
        /// </summary>
        /// <param name="listOfCand">The list of candidates.</param>
        /// <param name="candidate">The candidate to clean up.</param>
        private void ExcludeCandidate(List<SelectedBindingPair> listOfCand, SelectedBindingPair candidate)
        {
            if (candidate != null)
            {
                var filteredList = listOfCand.Where(p => p.ViewModelNameCandidate.MethdInfo != candidate.ViewModelNameCandidate.MethdInfo
                    || p.ViewModelNameCandidate.PropInfo != candidate.ViewModelNameCandidate.PropInfo
                    || p.ViewModelNameCandidate.FldInfo != candidate.ViewModelNameCandidate.FldInfo).ToList();
                listOfCand.Clear();
                listOfCand.AddRange(filteredList);
            }
        }


        private static SelectedBindingPair LocateCandidateByXNameWithAnyTargets(List<SelectedBindingPair> listOfCand, bool inclSubNameMatch)
        {
            // Get all with Exact match
            if (listOfCand != null)
            {
                var bindingTemp = listOfCand.Where(pair => pair.ViewModelNameCandidate.TargetLinkNames.Count > 0 && pair.ExactMatch).OrderByDescending(a => a.MatchingRank).ToList();
                if (bindingTemp.Count > 0)
                {
                    return bindingTemp[0];
                }

                if (inclSubNameMatch)
                {
                    // Get all with a sub name match 
                    bindingTemp = listOfCand.Where(pair => pair.ViewModelNameCandidate.TargetLinkNames.Count > 0 && pair.SubNameMatch).OrderByDescending(a => a.MatchingRank).ToList();
                    if (bindingTemp.Count > 0)
                    {
                        return bindingTemp[0];
                    }

                }
            }
            return null;
        }

        private static SelectedBindingPair LocateCandidateByXNameAndTarget(List<SelectedBindingPair> listOfCand, string target, bool inclSubNameMatch)
        {
            // Get all with Exact match
            if (listOfCand != null)
            {
                var bindingTemp = listOfCand.Where(pair => pair.ViewModelNameCandidate.TargetLinkNames.Contains(target) && pair.ExactMatch).OrderByDescending(a => a.MatchingRank).ToList();
                if (bindingTemp.Count > 0)
                {
                    return bindingTemp[0];
                }

                if (inclSubNameMatch)
                {
                    // Get all with a sub name match 
                    bindingTemp = listOfCand.Where(pair => pair.ViewModelNameCandidate.TargetLinkNames.Contains(target) && pair.SubNameMatch).OrderByDescending(a => a.MatchingRank).ToList();
                    if (bindingTemp.Count > 0)
                    {
                        return bindingTemp[0];
                    }

                }
            }
            return null;
        }

        /// <summary>
        /// Gets a collection of all descended x:Named XAML-defined elements.
        /// </summary>
        /// <param name="obj">The Dependency object node.</param>
        /// <returns>The list of detected x:Name of XAML-defined elements.</returns>
        private List<ViewXNameCandidate> GetXNamesCandidates(DependencyObject obj)
        {
            Dictionary<string, ViewXNameCandidate> infoList = new Dictionary<string, ViewXNameCandidate>();
            Stack<DependencyObject> stack = new Stack<DependencyObject>();
            stack.Push(obj);
            while (stack.Count > 0)
            {
                var current = stack.Pop();
                string typeName = current.GetType().Name;
                var xName = current.GetValue(FrameworkElement.NameProperty) as string;
                if ((!string.IsNullOrEmpty(xName)) && (!xName.StartsWith("_")))
                {
                    if (string.Compare(typeName, xName, StringComparison.CurrentCultureIgnoreCase) == 0)
                    {
                        if (infoList.Keys.Contains(xName))
                        {
                            Debug.WriteLine("#DUPLICATE#-A-W-XAML# - x:Name: {0}.@.{1}", typeName, xName);
                            continue;
                        }
                    }

                    var listSubnames = xName.Split('_');
                    if (listSubnames.Length == 0 || String.IsNullOrEmpty(listSubnames[0]))
                    {
                        continue;
                    }

                    var mainContainer = new ViewXNameCandidate
                    {
                        XamlElementType = current.GetType(),
                        XamlElementObject = current,
                        FullXName = xName,
                        BaseXName = listSubnames[0]
                    };
                    mainContainer.SplitFullXName = BindHelper.SplitNameByCase(mainContainer.FullXName);
                    for (int i = 1; i < listSubnames.Length; i++)
                    {
                        if (!String.IsNullOrEmpty(listSubnames[i]))
                        {
                            mainContainer.BaseXNameSubNames.Add(listSubnames[i]);
                        }
                    }
                    infoList[xName] = mainContainer;
                }
#if DEBUG
                else
                {
                    if (!string.IsNullOrEmpty(xName))
                    {
                        Debug.WriteLine("#IGNORED  #-A-W-XAML# - x:Name: {0}.@.{1}", typeName, xName);
                    }
                }
#endif
                // check a NameScope

#if !WINDOWS_UWP
                IDictionary<string, object> nameScope = NameScope.GetNameScope(current) as IDictionary<string, object>;
                if (nameScope != null)
                {
                    foreach (var item in nameScope)
                    {
                        xName = item.Key;
                        string tpName = item.Value.GetType().Name;
                        if (string.Compare(tpName, xName, StringComparison.CurrentCultureIgnoreCase) == 0)
                        {

                            continue;
                        }

                        var listSubnames = xName.Split('_');
                        if (listSubnames.Length == 0 || String.IsNullOrEmpty(listSubnames[0]))
                        {
                            continue;
                        }

                        var container = new ViewXNameCandidate
                        {
                            XamlElementType = item.Value.GetType(),
                            XamlElementObject = item.Value,
                            FullXName = xName,
                            BaseXName = listSubnames[0]
                        };
                        container.SplitFullXName = BindHelper.SplitNameByCase(container.FullXName);
                        for (int i = 1; i < listSubnames.Length; i++)
                        {
                            if (!String.IsNullOrEmpty(listSubnames[i]))
                            {
                                container.BaseXNameSubNames.Add(listSubnames[i]);
                            }
                        }
                        infoList[xName] = container;
                    }
                }
#endif
#if !WINDOWS_UWP

                if (IncludeVisualTreeNames)
                {
                    if (current.GetType().IsSubclassOf(typeof(Visual)))
#else
                if (current.GetType().GetTypeInfo().IsSubclassOf(typeof(UIElement)))
#endif
                    {
                        int count = VisualTreeHelper.GetChildrenCount(current);
                        for (int i = 0; i < count; i++)
                        {
                            DependencyObject dpo = VisualTreeHelper.GetChild(current, i);
                            if (dpo != null)
                            {
                                stack.Push(dpo);
                            }
                        }
                    }
#if !WINDOWS_UWP
                }

                foreach (var item in LogicalTreeHelper.GetChildren(current))
                {
                    DependencyObject dpo = item as DependencyObject;
                    if (dpo != null)
                    {
                        stack.Push(dpo);
                    }
                }
#endif
            }
#if DEBUG
            foreach (var item in infoList)
            {
                Debug.WriteLine("#SELECTED #-A-W-XAML# - x:Name: {0}.@.{1}", item.Value.XamlElementType.Name, item.Value.BaseXName);
            }
#endif
            return new List<ViewXNameCandidate>(infoList.Values);
        }

        /// <summary>
        /// Gets all possible method candidates from a View Model object.
        /// </summary>
        /// <param name="locatedViewModels">The located view model list.</param>
        /// <param name="obj">The Dependency object node.</param>
        /// <returns>The list of detected candidates.</returns>
        private List<ViewModelMemberCandidate> GetMethodCandidates(List<Tuple<Type, object>> locatedViewModels, object obj)
        {
            Type type = obj.GetType();
            List<ViewModelMemberCandidate> infoList = new List<ViewModelMemberCandidate>();
#if WINDOWS_UWP
            IEnumerable<MethodInfo> info;
#else
            MethodInfo[] info;
#endif
            Queue<Tuple<Type, object>> lookupTypes = new Queue<Tuple<Type, object>>(locatedViewModels);
            lookupTypes.Enqueue(new Tuple<Type, object>(type, obj));
            while (lookupTypes.Count != 0)
            {
                var tuple = lookupTypes.Dequeue();
                var lookUpType = tuple.Item1;
                var lookUpObject = tuple.Item2;

#if WINDOWS_UWP
                info = lookUpType.GetTypeInfo().DeclaredMethods;
#else
                info = lookUpType.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
#endif
                foreach (var item in info)
                {
                    bool skip = false;
                    foreach (var str in KnownExcludeMethodPrefixes)
                    {
                        if (item.Name.StartsWith(str))
                        {
                            skip = true;
                            break;
                        }
                    }

                    if (skip)
                    {
                        continue;
                    }


                    var listSubnames = item.Name.Split('_');
                    if (listSubnames.Length != 0 && !String.IsNullOrEmpty(listSubnames[0]))
                    {
                        // Get additional attributes for the method
#if !WINDOWS_UWP
                        var attribV = Attribute.GetCustomAttributes(item, typeof(ViewTargetAttribute));
#else
                        var attribV = item.GetCustomAttributes(typeof(ViewTargetAttribute));
#endif
                        // Create the MAIN container for a method

                        if (listSubnames.Length > 1)
                        {
                            // Member name has the last as target link to view
                            var mainContainer = new ViewModelMemberCandidate
                            {
                                MethdInfo = item,
                                MemberObject = lookUpObject,
                                MemberType = type,
                                MemberName =
                                    item.Name.Substring(0,
                                        item.Name.Length - listSubnames[listSubnames.Length - 1].Length)
                            };
                            mainContainer.SplitMemberName = BindHelper.SplitNameByCase(mainContainer.MemberName);
                            mainContainer.TargetLinkNames.Add(listSubnames[listSubnames.Length - 1]);
                            infoList.Add(mainContainer);
                            // for the next stage just the naked name
                            listSubnames = new[] { item.Name };
                        }

                        if (listSubnames.Length == 1)
                        {
                            // there is target link try to add them from the attribute [ViewTargetAttribute("...")]
                            var mainContainer = new ViewModelMemberCandidate
                            {
                                MethdInfo = item,
                                MemberObject = lookUpObject,
                                MemberType = type,
                                MemberName = item.Name
                            };
                            mainContainer.SplitMemberName = BindHelper.SplitNameByCase(mainContainer.MemberName);
                            bool addedTargets = false;
                            foreach (Attribute att in attribV)
                            {
                                var lst = ((ViewTargetAttribute)att).Targets;
                                foreach (var sblst in lst)
                                {
                                    mainContainer.TargetLinkNames.Add(sblst);
                                    addedTargets = true;
                                }
                            }
                            if (addedTargets)
                            {
                                infoList.Add(mainContainer);
                            }
                        }
                    }
                    // Process all possible aliases
#if !WINDOWS_UWP
                    var attribVex = Attribute.GetCustomAttributes(item, typeof(ViewXNameAliasAttribute));
#else
                    var attribVex = item.GetCustomAttributes(typeof(ViewXNameAliasAttribute));
#endif
                    foreach (Attribute att in attribVex)
                    {
                        string alias = ((ViewXNameAliasAttribute)att).AliasName;
                        var targetNames = ((ViewXNameAliasAttribute)att).Targets;

                        var spcontainer = new ViewModelMemberCandidate
                        {
                            BindingParameters = ((ViewXNameAliasAttribute)att).ViewXNameCandidateParams,
                            MemberObject = lookUpObject,
                            MemberType = type,
                            MemberName = alias
                        };
                        spcontainer.SplitMemberName = BindHelper.SplitNameByCase(spcontainer.MemberName);
                        spcontainer.MethdInfo = item;
                        spcontainer.TargetLinkNames = new HashSet<string>(targetNames);
                        infoList.Add(spcontainer);

                    }

                }
            }

            return infoList;
        }

        /// <summary>
        /// Gets all possible property candidates from a View Model object.
        /// </summary>
        /// <param name="locatedViewModels">The located view model list.</param>
        /// <param name="obj">The Dependency object node.</param>
        /// <returns>The list of detected candidates.</returns>
        private List<ViewModelMemberCandidate> GetPropertyCandidates(List<Tuple<Type, object>> locatedViewModels, object obj)
        {
            Type type = obj.GetType();
            List<ViewModelMemberCandidate> infoList = new List<ViewModelMemberCandidate>();
#if WINDOWS_UWP
            IEnumerable<PropertyInfo> info;
#else
            PropertyInfo[] info;
#endif
            Queue<Tuple<Type, object>> lookupTypes = new Queue<Tuple<Type, object>>(locatedViewModels);
            lookupTypes.Enqueue(new Tuple<Type, object>(type, obj));
            while (lookupTypes.Count != 0)
            {
                var tuple = lookupTypes.Dequeue();
                var lookUpType = tuple.Item1;
                var lookUpObject = tuple.Item2;

#if WINDOWS_UWP
                info = lookUpType.GetTypeInfo().DeclaredProperties;
#else
                info = lookUpType.GetProperties(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
#endif
                foreach (var item in info)
                {
                    var listSubnames = item.Name.Split('_');
                    if (listSubnames.Length != 0 && !String.IsNullOrEmpty(listSubnames[0]))
                    {
                        // Get additional attributes for the method
#if !WINDOWS_UWP
                        var attribV = Attribute.GetCustomAttributes(item, typeof(ViewTargetAttribute));
#else
                        var attribV = item.GetCustomAttributes(typeof(ViewTargetAttribute));
#endif
                        // Create the MAIN container for a method

                        if (listSubnames.Length > 1)
                        {
                            // Member name has the last as target link to view
                            var mainContainer = new ViewModelMemberCandidate
                            {
                                PropInfo = item,
                                MemberObject = lookUpObject,
                                MemberType = type,
                                MemberName =
                                    item.Name.Substring(0,
                                        item.Name.Length - listSubnames[listSubnames.Length - 1].Length)
                            };
                            mainContainer.SplitMemberName = BindHelper.SplitNameByCase(mainContainer.MemberName);
                            mainContainer.TargetLinkNames.Add(listSubnames[listSubnames.Length - 1]);
                            infoList.Add(mainContainer);
                            // for the next stage just the naked name
                            listSubnames = new[] { item.Name };
                        }

                        if (listSubnames.Length == 1)
                        {
                            // there is target link try to add them from the attribute [ViewTargetAttribute("...")]
                            var mainContainer = new ViewModelMemberCandidate
                            {
                                PropInfo = item,
                                MemberObject = lookUpObject,
                                MemberType = type,
                                MemberName = item.Name
                            };
                            mainContainer.SplitMemberName = BindHelper.SplitNameByCase(mainContainer.MemberName);
                            bool addedTargets = false;
                            foreach (Attribute att in attribV)
                            {
                                var lst = ((ViewTargetAttribute)att).Targets;
                                foreach (var sblst in lst)
                                {
                                    mainContainer.TargetLinkNames.Add(sblst);
                                    addedTargets = true;
                                }
                            }
                            if (addedTargets)
                            {
                                infoList.Add(mainContainer);
                            }
                        }
                    }

                    // Process all possible aliases
#if !WINDOWS_UWP
                    var attribVex = Attribute.GetCustomAttributes(item, typeof(ViewXNameAliasAttribute));
#else
                    var attribVex = item.GetCustomAttributes(typeof(ViewXNameAliasAttribute));
#endif
                    foreach (Attribute att in attribVex)
                    {
                        string alias = ((ViewXNameAliasAttribute)att).AliasName;
                        var targetNames = ((ViewXNameAliasAttribute)att).Targets;
                        var spcontainer = new ViewModelMemberCandidate
                        {
                            BindingParameters = ((ViewXNameAliasAttribute)att).ViewXNameCandidateParams,
                            MemberObject = lookUpObject,
                            MemberType = type,
                            MemberName = alias
                        };
                        spcontainer.SplitMemberName = BindHelper.SplitNameByCase(spcontainer.MemberName);
                        spcontainer.PropInfo = item;
                        spcontainer.TargetLinkNames = new HashSet<string>(targetNames);
                        infoList.Add(spcontainer);
                    }
                }
            }

            return infoList;
        }

        /// <summary>
        /// Gets all the fields attributed candidates from a View Model object.
        /// </summary>
        /// <param name="locatedViewModels">The located view model list.</param>
        /// <param name="obj">The Dependency object node.</param>
        /// <returns>The list of detected candidates.</returns>
        private List<ViewModelMemberCandidate> GetFieldCandidates(List<Tuple<Type, object>> locatedViewModels, object obj)
        {
            Type type = obj.GetType();
            List<ViewModelMemberCandidate> infoList = new List<ViewModelMemberCandidate>();
#if WINDOWS_UWP
            IEnumerable<FieldInfo> info;
#else
            FieldInfo[] info;
#endif
            Queue<Tuple<Type, object>> lookupTypes = new Queue<Tuple<Type, object>>(locatedViewModels);
            lookupTypes.Enqueue(new Tuple<Type, object>(type, obj));
            while (lookupTypes.Count != 0)
            {
                var tuple = lookupTypes.Dequeue();
                var lookUpType = tuple.Item1;
                var lookUpObject = tuple.Item2;

#if WINDOWS_UWP
                info = lookUpType.GetTypeInfo().DeclaredFields;
#else
                info = lookUpType.GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
#endif
                foreach (var item in info)
                {
                    // Process aliases
#if !WINDOWS_UWP
                    var attribV = Attribute.GetCustomAttributes(item, typeof(ViewXNameSourceObjectMappingAttribute));
#else
                    var attribV = item.GetCustomAttributes(typeof(ViewXNameSourceObjectMappingAttribute));
#endif
                    foreach (Attribute att in attribV)
                    {
                        var sources = ((ViewXNameSourceObjectMappingAttribute)att).Sources;
                        foreach (var source in sources)
                        {
                            var spcontainer = new ViewModelMemberCandidate
                            {
                                FldInfo = item,
                                MemberObject = lookUpObject,
                                MemberType = type,
                                MemberName = source
                            };
                            spcontainer.SplitMemberName = BindHelper.SplitNameByCase(spcontainer.MemberName);
                            spcontainer.TargetLinkNames.Add(ConstructorTarget);
                            infoList.Add(spcontainer);
                        }
                    }

#if !WINDOWS_UWP
                    attribV = Attribute.GetCustomAttributes(item, typeof(ViewXNameSourceTargetMappingAttribute));
#else
                    attribV = item.GetCustomAttributes(typeof(ViewXNameSourceTargetMappingAttribute));
#endif
                    foreach (Attribute att in attribV)
                    {
                        string alias = ((ViewXNameSourceTargetMappingAttribute)att).AliasName;
                        var targetNames = ((ViewXNameSourceTargetMappingAttribute)att).Targets;
                        var spcontainer = new ViewModelMemberCandidate
                        {
                            MemberObject = lookUpObject,
                            MemberType = type
                        };
                        infoList.Add(spcontainer);
                        spcontainer.MemberName = alias;
                        spcontainer.SplitMemberName = BindHelper.SplitNameByCase(spcontainer.MemberName);
                        spcontainer.FldInfo = item;
                        spcontainer.TargetLinkNames = new HashSet<string>(targetNames);
                    }

                    // Process all possible aliases
#if !WINDOWS_UWP
                    attribV = Attribute.GetCustomAttributes(item, typeof(ViewXNameAliasAttribute));
#else
                    attribV = item.GetCustomAttributes(typeof(ViewXNameAliasAttribute));
#endif
                    foreach (Attribute att in attribV)
                    {
                        string alias = ((ViewXNameAliasAttribute)att).AliasName;
                        var targetNames = ((ViewXNameAliasAttribute)att).Targets;
                        var spcontainer = new ViewModelMemberCandidate
                        {
                            MemberObject = lookUpObject,
                            MemberType = type
                        };
                        infoList.Add(spcontainer);
                        spcontainer.MemberName = alias;
                        spcontainer.SplitMemberName = BindHelper.SplitNameByCase(spcontainer.MemberName);
                        spcontainer.FldInfo = item;
                        spcontainer.CopyToView = true;
                        spcontainer.TargetLinkNames = new HashSet<string>(targetNames);
                    }


                }
            }

            return infoList;
        }


    }
}
