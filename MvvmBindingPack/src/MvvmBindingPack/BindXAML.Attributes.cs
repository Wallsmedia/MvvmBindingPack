// 
//  MVVM-WPF-NetCore Markup, Binding and other Extensions.
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

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

#if !WINDOWS_UWP
using System.Windows.Data;
#else
using Windows.UI.Xaml.Data;
#endif

namespace MvvmBindingPack
{

    /// <summary>
    /// The mapping attribute that adds to a class the extra alias "candidate type names".
    /// It is used to map a View onto a View Model.
    /// </summary>
    [AttributeUsageAttribute(AttributeTargets.Class, AllowMultiple = true)]
    public class ViewModelClassAliasAttribute : Attribute
    {
        List<string> _aliases = new List<string>();
        /// <summary>
        /// Contains a collection of the View model  aliases.
        /// </summary>
        public List<string> Aliases
        {
            get { return _aliases; }
        }

        /// <summary>
        /// Constructs the attribute for View model mapping.
        /// </summary>
        /// <param name="commaSeparatedAliases">The comma separated aliases of the candidate type name.</param>
        public ViewModelClassAliasAttribute(string commaSeparatedAliases)
        {
            var targets = commaSeparatedAliases.Split(',');
            foreach (var item in targets)
            {
                var name = item.TrimStart().TrimEnd();
                if (!string.IsNullOrEmpty(name))
                {
                    if (BindHelper.ValidateElementName(name))
                    {
                        _aliases.Add(name);
                    }
                }
            }
#if DEBUG
            if (_aliases.Count == 0)
            {
                throw new InvalidOperationException("Names was not defined.");
            }
#endif
        }
    }

    /// <summary>
    /// The mapping attribute that marks a filed, method or property name (or x:Name candidate)
    /// with set alias "names" + "targets" for View XAML x:Name element.
    /// </summary>
    [AttributeUsageAttribute(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
    public class ViewXNameAliasAttribute : Attribute
    {
        AutoWireViewConrols.TargetLinkParameters _viewXNameCandidateParams;
        internal AutoWireViewConrols.TargetLinkParameters ViewXNameCandidateParams
        {
            get { return _viewXNameCandidateParams; }
        }

        /// <summary>
        /// If it is true to register the handler such that it is invoked even when the routed event is marked handled in its event data.
        /// </summary>
        public bool HandledEventsToo
        {
            set
            {
                if (_viewXNameCandidateParams == null)
                {
                    _viewXNameCandidateParams = new AutoWireViewConrols.TargetLinkParameters();
                }
                _viewXNameCandidateParams.HandledEventsToo = value;
            }
            get
            {
                if (_viewXNameCandidateParams != null && _viewXNameCandidateParams.HandledEventsToo.HasValue)
                {
                    return _viewXNameCandidateParams.HandledEventsToo.Value;
                }
                return false;
            }
        }

#if !WINDOWS_UWP
        /// <summary>
        /// The DataErrorValidationRule is a built-in validation rule that checks for errors that are raised by the IDataErrorInfo implementation of the source object.
        /// </summary>
        public bool ValidatesOnDataErrors
        {
            set
            {
                if (_viewXNameCandidateParams == null)
                {
                    _viewXNameCandidateParams = new AutoWireViewConrols.TargetLinkParameters();
                }
                _viewXNameCandidateParams.ValidatesOnDataErrors = value;

            }
            get
            {
                if (_viewXNameCandidateParams != null && _viewXNameCandidateParams.ValidatesOnDataErrors.HasValue)
                {
                    return _viewXNameCandidateParams.ValidatesOnDataErrors.Value;
                }
                return false;
            }
        }

        /// <summary>
        /// When ValidatesOnNotifyDataErrors is true, the binding checks for and reports errors 
        /// that are raised by a data source that implements INotifyDataErrorInfo.
        /// </summary>
        public bool ValidatesOnNotifyDataErrors
        {
            set
            {
                if (_viewXNameCandidateParams == null)
                {
                    _viewXNameCandidateParams = new AutoWireViewConrols.TargetLinkParameters();
                }
                _viewXNameCandidateParams.ValidatesOnNotifyDataErrors = value;

            }
            get
            {
                if (_viewXNameCandidateParams != null && _viewXNameCandidateParams.ValidatesOnNotifyDataErrors.HasValue)
                {
                    return _viewXNameCandidateParams.ValidatesOnNotifyDataErrors.Value;
                }
                return false;
            }
        }

        /// <summary>
        /// The ExceptionValidationRule is a built-in validation rule that checks for exceptions 
        /// that are thrown during the update of the source property.
        /// </summary>
        public bool ValidatesOnExceptions
        {
            set
            {
                if (_viewXNameCandidateParams == null)
                {
                    _viewXNameCandidateParams = new AutoWireViewConrols.TargetLinkParameters();
                }
                _viewXNameCandidateParams.ValidatesOnExceptions = value;
            }
            get
            {
                if (_viewXNameCandidateParams != null && _viewXNameCandidateParams.ValidatesOnExceptions.HasValue)
                {
                    return _viewXNameCandidateParams.ValidatesOnExceptions.Value;
                }
                return false;
            }
        }
#endif

        /// <summary>
        /// Gets or sets a value that indicates the direction of the data flow in the binding.
        /// </summary>
        public BindingMode BindingMode
        {
            set
            {
                if (_viewXNameCandidateParams == null)
                {
                    _viewXNameCandidateParams = new AutoWireViewConrols.TargetLinkParameters();
                }
                _viewXNameCandidateParams.BindingMode = value;
            }

            get
            {
                if (_viewXNameCandidateParams != null && _viewXNameCandidateParams.BindingMode.HasValue)
                {
                    return _viewXNameCandidateParams.BindingMode.Value;
                }
                return BindingMode.TwoWay;
            }

        }


        string _alias = string.Empty;
        /// <summary>
        /// Contains a collection of the XAML x:Name aliases.
        /// </summary>
        public string AliasName
        {
            get { return _alias; }
        }

        List<string> _targets = new List<string>();
        /// <summary>
        /// Contains the collection of the XAML x:Name element targets.
        /// </summary>
        public List<string> Targets
        {
            get { return _targets; }
        }

        /// <summary>
        /// Constructs the attribute for x:Name element and  target mapping.
        /// </summary>
        /// <param name="aliasName">Alias x:Name string. If it contains '_', it will be split on XAML x:Name element targets as well.</param>
        /// <param name="commaSeparatedTargets">XAML x:Name element targets separated by comma.</param>
        public ViewXNameAliasAttribute(string aliasName, string commaSeparatedTargets = "")
        {
            var aliasname = aliasName.TrimStart().TrimEnd();
            if (!string.IsNullOrEmpty(aliasname))
            {
                _alias = aliasname;
                if (!string.IsNullOrEmpty(_alias))
                {
                    var targets = commaSeparatedTargets.Split(',');
                    foreach (var item in targets)
                    {
                        var name = item.TrimStart().TrimEnd();
                        if (!string.IsNullOrEmpty(name))
                        {
                            if (BindHelper.ValidateElementName(name))
                            {
                                _targets.Add(name);
                            }
                        }
                    }
                }
            }
#if DEBUG
            if (String.IsNullOrEmpty(_alias) || _targets.Count == 0)
            {
                throw new InvalidOperationException("aliasName or commaSeparatedTargets was not defined.");
            }
#endif
        }
    }

    /// <summary>
    /// Defines the mapping attribute that marks a method or property name (or x:Name candidate)
    /// with set name targets for View XAML x:Name element.
    /// </summary>
    [AttributeUsageAttribute(AttributeTargets.Method | AttributeTargets.Property, AllowMultiple = true)]
    public class ViewTargetAttribute : Attribute
    {
        List<string> _targets = new List<string>();
        /// <summary>
        /// Contains the collection of the XAML x:Name element targets.
        /// </summary>
        public List<string> Targets
        {
            get { return _targets; }
        }

        /// <summary>
        /// Constructs the attribute for x:Name target mapping.
        /// </summary>
        /// <param name="commaSeparatedTargets">XAML x:Name element targets separated by comma.</param>
        public ViewTargetAttribute(string commaSeparatedTargets)
        {
            var targets = commaSeparatedTargets.Split(',');
            foreach (var item in targets)
            {
                var name = item.TrimStart().TrimEnd();
                if (!string.IsNullOrEmpty(name))
                {
                    if (BindHelper.ValidateElementName(name))
                    {
                        _targets.Add(name);
                    }
                }
            }
#if DEBUG
            if (_targets.Count == 0)
            {
                throw new InvalidOperationException("Targets was not defined.");
            }
#endif
        }
    }


    /// <summary>
    /// Defines the mapping attribute that marks the field of the System.Object type 
    /// where the reference to XAML x:Named element will be set to.
    /// </summary>
    [AttributeUsageAttribute(AttributeTargets.Field, AllowMultiple = true)]
    public class ViewXNameSourceObjectMappingAttribute : Attribute
    {
        List<string> _sources = new List<string>();
        /// <summary>
        /// Contains the collection of the XAML x:Name source names.
        /// </summary>
        public List<string> Sources
        {
            get { return _sources; }
        }

        /// <summary>
        /// Constructs the attribute for XAML x:Named element mapping.
        /// </summary>
        /// <param name="commaSeparatedXNames">x:Names separated by comma.
        /// If x:Name contains symbols '_', it will be cut at it.</param>
        public ViewXNameSourceObjectMappingAttribute(string commaSeparatedXNames)
        {
            var targets = commaSeparatedXNames.Split(',');
            foreach (var item in targets)
            {
                var name = item.TrimStart().TrimEnd();
                if (!string.IsNullOrEmpty(name))
                {
                    if (BindHelper.ValidateElementName(name))
                    {
                        _sources.Add(name);
                    }
                }
            }
#if DEBUG
            if (_sources.Count == 0)
            {
                throw new InvalidOperationException("Targets was not defined.");
            }
#endif
        }
    }

    /// <summary>
    /// The mapping attribute that marks a field reference to ViewXNameSourceTarget type for
   ///  a View XAML x:Name element. This class is used to access to properties or events of the View XAML element.
    /// </summary>
    [AttributeUsageAttribute(AttributeTargets.Field, AllowMultiple = true)]
    public class ViewXNameSourceTargetMappingAttribute : Attribute
    {
        string _alias = string.Empty;
        /// <summary>
        /// Contains a collection of the XAML x:Name aliases.
        /// </summary>
        public string AliasName
        {
            get { return _alias; }
        }

        List<string> _targets = new List<string>();
        /// <summary>
        /// Contains the collection of the XAML x:Name element targets.
        /// </summary>
        public List<string> Targets
        {
            get { return _targets; }
        }

        /// <summary>
        /// Constructs the attribute for x:Name element and  target mapping.
        /// </summary>
        /// <param name="aliasName">Alias x:Name string. If it contains '_', it will be split on XAML x:Name element targets as well.</param>
        /// <param name="commaSeparatedTargets">XAML x:Name element targets separated by comma.</param>
        public ViewXNameSourceTargetMappingAttribute(string aliasName, string commaSeparatedTargets = "")
        {

            var aliasname = aliasName.TrimStart().TrimEnd();
            if (!string.IsNullOrEmpty(aliasname))
            {
                _alias = aliasname;
                if (!string.IsNullOrEmpty(_alias))
                {
                    var targets = commaSeparatedTargets.Split(',');
                    foreach (var item in targets)
                    {
                        var name = item.TrimStart().TrimEnd();
                        if (!string.IsNullOrEmpty(name))
                        {
                            if (BindHelper.ValidateElementName(name))
                            {
                                _targets.Add(name);
                            }
                        }
                    }
                }
            }
#if DEBUG
            if (String.IsNullOrEmpty(_alias) || _targets.Count == 0)
            {
                throw new InvalidOperationException("aliasName or commaSeparatedTargets was not defined.");
            }
#endif
        }
    }


    /// <summary>
    /// Defines the mapping attribute that appends(extends) the bindings list candidates with the reference type object members.
    /// Value type, boxed value type and types started with "System" .. "MicroSoft" will be ignored.
    /// The members are appended to a list, they are not inserted in the middle of the contained attribute class.
    /// They have a low priority in the lookup. Recursive view model appending is not supported.
    /// </summary>
    [AttributeUsageAttribute(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class AppendViewModelAttribute : Attribute
    {
        ///// <summary>
        ///// Feature is not supported in the current version.
        ///// </summary>
        // public bool Recursive { get; set; }
    }

}
