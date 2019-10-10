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
using System.Reflection;
using System.Windows;


#if WINDOWS_UWP
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Controls.Primitives;

#else
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Markup;

#endif

namespace MvvmBindingPack
{
    /// <summary>
    /// WPF and WinRt XAML mark-up extension of Inversion of Controls binding.
    /// Binds an object to a target property by resolving it through the generic Service Locator interface.
    /// </summary>
#if !WINDOWS_UWP
    [MarkupExtensionReturnTypeAttribute(typeof(Object))]
#endif
    public class IocBinding : MarkupExtension
    {

        /// <summary>
        /// Default constructor.
        /// </summary>
        public IocBinding() { }

        /// <summary>
        /// Constructs the class with a requested service type.
        /// </summary>
        /// <param name="serviceType">Type or type name of the requested object.</param>
        public IocBinding(object serviceType)
        {
            _serviceType = serviceType;
        }

        object _serviceType;
        /// <summary>
        /// The type (System.Type) or type name (System.String) of the requested object. 
        /// </summary>
#if !WINDOWS_UWP
        [ConstructorArgument("serviceType")]
#endif
        public object ServiceType
        {
            get { return _serviceType; }
            set { _serviceType = value; }
        }

        String _serviceKey;
        /// <summary>
        /// The key of the requested object.
        /// </summary>
        public String ServiceKey
        {
            get { return _serviceKey; }
            set { _serviceKey = value; }
        }

        String _propertyName;
        /// <summary>
        /// The Object property name.
        /// </summary>
        public String PropertyName
        {
            get { return _propertyName; }
            set { _propertyName = value; }
        }

        String _targetPropertyName;
        /// <summary>
        /// The Target property name.
        /// </summary>
        public String TargetPropertyName
        {
            get { return _targetPropertyName; }
            set { _targetPropertyName = value; }
        }

        /// <summary>
        /// Value that was resulted
        /// </summary>
        protected Object ResolvedResult { get; set; }


        /// <summary>
        /// Returns an object that should be set on the property where this extension is applied.
        /// </summary>
        /// <param name="serviceProvider">Object that can provide services for the markup extension.</param>
        /// <returns>The object value to set on the property where the markup extension provided
        /// value is evaluated.</returns>
        /// <exception cref="System.InvalidOperationException">serviceProvider was null, or failed to 
        /// implement a required service.</exception>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
            {
                throw new ArgumentNullException("serviceProvider");
            }
          
            if (_serviceType == null)
            {
                // ReSharper disable NotResolvedInText
                throw new ArgumentNullException("IocBinding - ServiceType cannot be null.");
                // ReSharper restore NotResolvedInText
            }


            Type typeToResolve;
            // ReSharper disable CanBeReplacedWithTryCastAndCheckForNull
            if (_serviceType is Type)
            // ReSharper restore CanBeReplacedWithTryCastAndCheckForNull
            {
                typeToResolve = (Type)_serviceType;
            }
            // ReSharper disable CanBeReplacedWithTryCastAndCheckForNull
            else if (_serviceType is string)
            // ReSharper restore CanBeReplacedWithTryCastAndCheckForNull
            {
#if !WINDOWS_UWP
                // ReSharper disable SuggestUseVarKeywordEvident
                IXamlTypeResolver service = serviceProvider.GetService(typeof(IXamlTypeResolver)) as IXamlTypeResolver;
                // ReSharper restore SuggestUseVarKeywordEvident

                typeToResolve = BindHelper.ResolveTypeByName((string)_serviceType);

                if ((service != null) && (typeToResolve == null))
                {
                    typeToResolve = service.Resolve((string)_serviceType);
                }
#else
                typeToResolve = BindHelper.ResolveTypeByName((string)_serviceType);
#endif

                if (typeToResolve == null)
                {
                    throw new InvalidOperationException("IocBinding invalid  cannot resolve type - " + (string)_serviceType);
                }
            }
            else
            {
                // ReSharper disable NotResolvedInText
                throw new ArgumentNullException("IocBinding - ServiceType can be 'Type' or 'String'.");
                // ReSharper restore NotResolvedInText
            }
            if (AutoWireVmDataContext.ServiceProvider != null)
            {

                try { ResolvedResult = AutoWireVmDataContext.ServiceProvider.GetService(typeToResolve); } catch { }
                if (ResolvedResult == null)
                {
                    try { ResolvedResult = Activator.CreateInstance(typeToResolve); } catch { }
                }
            }

#if !WINDOWS_UWP
            // ReSharper disable SuggestUseVarKeywordEvident
            IProvideValueTarget valueTarget = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;
            // ReSharper restore SuggestUseVarKeywordEvident

            //Attached collection support
            if ((valueTarget != null) && (valueTarget.TargetObject != null) && (valueTarget.TargetObject is FakeCollection) && (valueTarget.TargetProperty == null))
            {
                return this;
            }

            if ((valueTarget != null) && (valueTarget.TargetObject != null) && (valueTarget.TargetProperty == BindXAML.AssignPropertiesProperty))
            {
                throw new ArgumentException("IocBinding - Coding Error Direct access to attached property XAML Parser.");
            }

            //if ((valueTarget != null) && (valueTarget.TargetObject != null) && (valueTarget.TargetProperty != null) && (valueTarget.TargetProperty as MethodInfo != null))
            //{
            //    // Support for a single attached property for WPF
            //    // XAML WinRt doesn't call ProvideValue() but WPF always does call ProvideValue()
            //    // In this case if use the single attached property we have to demonstrate 
            //    return this; // That should be done like that
            //}
#endif
            if (!String.IsNullOrEmpty(PropertyName))
            {
                if (ResolvedResult != null)
                {
                    var info = ResolvedResult.GetPropertyInfo(PropertyName);
                    if (info == null)
                    {
                        throw new ArgumentException("IocBinding - cannot resolve 'TargetPropertyName' property  " + PropertyName);
                    }
                    ResolvedResult = info.GetValue(ResolvedResult, null);
                }
            }
            return ResolvedResult;
        }
    }

}
