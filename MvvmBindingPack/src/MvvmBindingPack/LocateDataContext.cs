// 
//  MVVM-WPF-NetCore Markup, Binding and other Extensions.
//  Copyright © 2013-2019 Alexander Paskhin /paskhin@hotmail.co.uk/ All rights reserved.
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
    /// XAML mark-up and BindXAML.AssignProperties extensions; it finds among DataContext objects, the first,
   ///  which contains the exact method or property. It comes through the parent elements of logical and visual trees.
    /// </summary>
#if !WINDOWS_UWP
    [MarkupExtensionReturnTypeAttribute(typeof(Object))]
#endif
    public class LocateDataContext : MarkupExtension
    {
        /// <summary>
        /// Used for late binding to dependency property
        /// </summary>
        protected object TargetObject;

        /// <summary>
        /// Used for late binding to dependency property
        /// </summary>
        protected object TargetProperty;

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
        /// Default constructor.
        /// </summary>
        public LocateDataContext() { }

        /// <summary>
        /// Constructs the class with a requested service type.
        /// </summary>
        /// <param name="dataContextType">Type or type name of the requested object.</param>
        public LocateDataContext(object dataContextType)
        {
            _dataContextType = dataContextType;
        }

        object _dataContextType;
        /// <summary>
        /// The type (System.Type) or the type name (System.String) of the required DataContext object. If it is not set, only a method or property name will be used to locate.
        /// </summary>
#if !WINDOWS_UWP
        [ConstructorArgument("DataContextType")]
#endif
        public object DataContextType
        {
            get { return _dataContextType; }
            set { _dataContextType = value; }
        }

        /// <summary>
        /// The method name of the source instance that has RoutedEventHandler delegate signature.
        /// The method name is used to search in DataContext object methods.
        /// It's priority versus 'PropertyName'.
        /// </summary>
        public String MethodName { get; set; }

        /// <summary>
        /// The property name is used to search in DataContext object properties.
        /// It doesn't has a priority versus 'MethodName'. 
        /// </summary>
        public String PropertyName { get; set; }


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
            object obj = null;
            Type typeToResolve;

            if (_dataContextType == null)
            {
                typeToResolve = typeof(object);
            }
            else if (_dataContextType is Type)
            {
                typeToResolve = (Type)_dataContextType;
            }
            else if (_dataContextType is String)
            {
#if !WINDOWS_UWP
                // ReSharper disable SuggestUseVarKeywordEvident
                IXamlTypeResolver service = serviceProvider.GetService(typeof(IXamlTypeResolver)) as IXamlTypeResolver;
                // ReSharper restore SuggestUseVarKeywordEvident

                typeToResolve = BindHelper.ResolveTypeByName((string)_dataContextType);

                if ((service != null) && (typeToResolve == null))
                {
                    typeToResolve = service.Resolve((string)_dataContextType);
                }
#else
                typeToResolve = BindHelper.ResolveTypeByName((string)_dataContextType);
#endif
            }
            else
            {
                throw new ArgumentNullException("LocateDataContext - DataContextType can be 'Type' or 'String' or empty.");
            }

            // ReSharper disable SuggestUseVarKeywordEvident
            IProvideValueTarget serviceProvideValueTarget = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;
            // ReSharper restore SuggestUseVarKeywordEvident
            if ((serviceProvideValueTarget != null) && (serviceProvideValueTarget.TargetObject != null))
            {
                // Save targets for case delayed processing
                TargetObject = serviceProvideValueTarget.TargetObject;
                TargetProperty = serviceProvideValueTarget.TargetProperty;

                if (!(serviceProvideValueTarget.TargetObject is DependencyObject))
                {
                    throw new AggregateException("LocateDataContext - Target of the markup extension must be the DependencyObject type.");
                }
                obj = BindHelper.LocateValidDependencyPropertyByAllTrees(serviceProvideValueTarget.TargetObject as DependencyObject, FrameworkElement.DataContextProperty, MethodName, PropertyName, null, typeToResolve);
                if (obj == null)
                {
                    var frameworkElement = TargetObject as FrameworkElement;
                    if (frameworkElement != null)
                    {
                        frameworkElement.Loaded += DelayLocateDataContext;
                    }
                }
            }
            return obj;
        }

        void DelayLocateDataContext(object obj, RoutedEventArgs args)
        {
            var frameworkElement = TargetObject as FrameworkElement;
            if (frameworkElement != null)
            {
                frameworkElement.Loaded -= DelayLocateDataContext;
                object dataContext;
                dataContext = BindHelper.LocateValidDependencyPropertyByAllTrees(TargetObject as DependencyObject, FrameworkElement.DataContextProperty, MethodName, PropertyName);
                if (dataContext != null)
                {
#if !WINDOWS_UWP
                    if (TargetProperty is DependencyProperty)
                    {
                        frameworkElement.SetValue((DependencyProperty)TargetProperty, dataContext);
                    }
#else
                    if (TargetProperty as PropertyInfo != null)
                    {
                        ((PropertyInfo)TargetProperty).SetValue(TargetObject, dataContext);
                    }
#endif
                }

                frameworkElement.Loaded -= DelayLocateDataContext;
            }

        }
    }
}
