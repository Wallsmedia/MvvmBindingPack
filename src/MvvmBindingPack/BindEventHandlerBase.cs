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
using System.Reflection;
using System.Windows;
using System.Collections.Generic;


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
    /// WPF and WinRt XAML mark-up extension.
    /// The abstract RoutedEventHandler binding class of WPF XAML mark-up extension.
    /// Base class for implementing of binding to control's <see cref="EventHandler"/> property type to a class member of a source object.
    /// </summary>
#if !WINDOWS_UWP
    [MarkupExtensionReturnTypeAttribute(typeof(Object))]
#endif
    public abstract class BindEventHandlerBase : MarkupExtension
    {
        /// <summary>
        /// Design time stub for event handler
        /// </summary>
#if WINDOWS_UWP
        static internal void DesignRoutedEventHandler(object sender, RoutedEventArgs e)
        {
        }
#else
        static internal void DesignRoutedEventHandler(object sender, EventArgs e)
        {
        }
#endif

        /// <summary>
        /// Used for late binding to dependency property
        /// </summary>
        protected object TargetObject { get; set; }
        /// <summary>
        /// Used for late binding to dependency property
        /// </summary>
        protected object TargetProperty { get; set; }

        /// <summary>
        /// Event handler hat was provided as a resulted.
        /// </summary>
        protected object ResolvedEventHandler { get; set; }

        /// <summary>
        /// Get a source object for binding.
        /// </summary>
        /// <param name="serviceProvider">The object that can provide services for the markup extension.</param>
        /// <returns></returns>
        protected abstract object ObtainSourceObject(IServiceProvider serviceProvider);

        /// <summary>
        ///  If it is set to “true”, all DataContext properties in the logical tree will be scanned until the math to a property
        ///  or method name (PropertyName, MethodName). Smart feature allows to ignore the current DataContext property value and
        ///  traverse to other parent DataContext value.If set on true, it will cause to scan for the DataContext property objects
        ///  over the trees and get the first one that contains the binding property or method. It used in case when there is need 
        ///  to ignore the binding ItemsSource DataContext for the ItemsControl item, just bind a Button to a View Model for 
        ///  the item of the ListView or so on.
        /// </summary>
        public bool DeepScanAllTrees { get; set; }

        /// <summary>
        /// The property name of the source instance that contains <see cref="EventHandler"/> delegate.
        /// It's mutually exclusive versus 'MethodName'. The property conducts "one time target initialization" meaning.
        /// Further changes of the property doesn't have any affects on event handling.
        /// </summary>
        public String PropertyName { get; set; }

        /// <summary>
        /// The method name of the source instance that has RoutedEventHandler delegate signature.
        /// It's mutually exclusive versus 'PropertyName'.
        /// </summary>
        public String MethodName { get; set; }



        /// <summary>
        /// The Target property name.
        /// </summary>
        public String TargetPropertyName { get; set; }

        /// <summary>
        /// The target event name;.
        /// </summary>
        public String TargetEventName { get; set; }

        /// <summary>
        /// Returns an object that should be set on the property where this extension is applied.
        /// </summary>
        /// <param name="serviceProvider">An object that can provide services for the markup extension.</param>
        /// <returns>The object value to set on the property where the markup extension provided value is evaluated.</returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            // ReSharper disable SuggestUseVarKeywordEvident
            IProvideValueTarget valueTarget = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;
            // ReSharper restore SuggestUseVarKeywordEvident

            if ((valueTarget != null) && (valueTarget.TargetObject != null) && (valueTarget.TargetProperty != null))
            {
                TargetObject = valueTarget.TargetObject;
                TargetProperty = valueTarget.TargetProperty;
            }

            Type desiredDelegateType = typeof(RoutedEventHandler);

#if !WINDOWS_UWP
            if ((valueTarget != null) && (valueTarget.TargetObject != null) && (valueTarget.TargetObject is FakeCollection) && (valueTarget.TargetProperty == null))
            {
                // XAML WinRt doesn't call ProvideValue() but WPF always does call ProvideVlue()
                // WPF - for case of the FakeCollectionEventHandler as a target object returns the instance of the object.
                // WPF - will call the instance of this object to resolve event handler delegate
                return this;
            }

            if ((valueTarget != null) && (valueTarget.TargetObject != null) && (valueTarget.TargetProperty == BindXAML.AddEventsProperty))
            {
                throw new ArgumentException("BindEventHandlerBase - Coding Error Direct access to attached property XAML Parser.");
            }

            if ((valueTarget != null) && (valueTarget.TargetObject != null) && (valueTarget.TargetProperty != null) && (valueTarget.TargetProperty is MethodInfo))
            {
                // Support for a single attached property for WPF
                // XAML WinRt doesn't call ProvideValue() but WPF always does call ProvideVlue()
                // In this case if use the single attached property we have to demonstrate 
                // Extract desired type from add method (object, handler)
                MethodInfo mf = valueTarget.TargetProperty as MethodInfo;
                if (mf.GetParameters().Length == 2)
                {
                    var pfs = mf.GetParameters();
                    desiredDelegateType = pfs[1].ParameterType;
                }
                else
                {
                    return this;
                }
            }
            else
#endif

                if ((valueTarget != null) && (valueTarget.TargetObject != null) && (valueTarget.TargetProperty != null))
            {
                if (!(valueTarget.TargetProperty is MethodInfo))
                {
                    // Should not be check if bind in style of attached property
                    if (!(valueTarget.TargetProperty is EventInfo))
                    {
                        throw new ArgumentException("BindEventHandlerBase - Target should be 'event' type class member");
                    }
                    desiredDelegateType = (valueTarget.TargetProperty as EventInfo).EventHandlerType;
                }
            }

            if (BindHelper.IsInDesignModeStatic)
            {
                var info = this.GetMethodInfo("DesignRoutedEventHandler");
                object designProvidedValue;
                if (info.IsStatic)
                {
#if WINDOWS_UWP
                    designProvidedValue = info.CreateDelegate(desiredDelegateType);
#else
                    designProvidedValue = Delegate.CreateDelegate(desiredDelegateType, info);
#endif
                }
                else
                {
#if WINDOWS_UWP
                    designProvidedValue = info.CreateDelegate(desiredDelegateType, this);
#else
                    designProvidedValue = Delegate.CreateDelegate(desiredDelegateType, this, info);
#endif
                }
                // Cannot correctly resolve in the design mode the source.
                return designProvidedValue;
            }

            object sourceBaseProvided = ObtainSourceObject(serviceProvider);

            if ((!String.IsNullOrEmpty(PropertyName)) && (!String.IsNullOrEmpty(MethodName)))
            {
                throw new ArgumentException("BindEventHandlerBase - Should be set only one property 'MethodName' or 'PropertyName'");
            }

            if ((String.IsNullOrEmpty(PropertyName)) && (String.IsNullOrEmpty(MethodName)))
            {
                throw new ArgumentException("BindEventHandlerBase - Should be set one of properties 'MethodName' or 'PropertyName'");
            }

            if (!String.IsNullOrEmpty(PropertyName))
            {
                if (sourceBaseProvided == null)
                {
                    if (this is BindEventHandler)
                    {
                        // Set the loaded event and requested event because we don't know which is going to be called first
                        ResolvedEventHandler = DataContextProxyRoutedEventHandler.CreateProxyDelegate(desiredDelegateType, MethodName, PropertyName);
                    }
                    else
                    {
                        throw new ArgumentException("BindEventHandlerBase - cannot resolve 'PropertyName' property  " + PropertyName);
                    }
                }
                else
                {
                    var propertyInfo = sourceBaseProvided.GetPropertyInfo(PropertyName);
                    if (propertyInfo == null)
                    {
                        List<Tuple<Type, object>> locatedViewModels = BindHelper.LocateAppendedViewModels(sourceBaseProvided);
                        foreach (var model in locatedViewModels)
                        {
                            propertyInfo = model.Item2.GetPropertyInfo(PropertyName);
                            if (propertyInfo != null)
                            {
                                sourceBaseProvided = model.Item2;
                                break;
                            }
                        }
                    }
                    if (propertyInfo == null)
                    {
                        throw new ArgumentException("BindEventHandlerBase - cannot resolve 'PropertyName' property  " + PropertyName);
                    }
                    ResolvedEventHandler = propertyInfo.GetValue(sourceBaseProvided, null);
                }
            }

            if (!String.IsNullOrEmpty(MethodName))
            {
                if (sourceBaseProvided == null)
                {
                    if (this is BindEventHandler)
                    {
                        // Set the loaded event and requested event because we don't know which is going to be called first
                        ResolvedEventHandler = DataContextProxyRoutedEventHandler.CreateProxyDelegate(desiredDelegateType, MethodName, PropertyName);
                    }
                    else
                    {
                        throw new ArgumentException("BindEventHandlerBase - cannot resolve 'MethodName' property  " + MethodName);
                    }

                }
                else
                {
                    //Get all appenders
                    MethodInfo methodInfo = sourceBaseProvided.GetMethodInfo(MethodName);
                    if (methodInfo == null)
                    {
                        List<Tuple<Type, object>> locatedViewModels = BindHelper.LocateAppendedViewModels(sourceBaseProvided);
                        foreach (var model in locatedViewModels)
                        {
                            methodInfo = model.Item2.GetMethodInfo(MethodName);
                            if (methodInfo != null)
                            {
                                sourceBaseProvided = model.Item2;
                                break;
                            }
                        }
                    }

                    if (methodInfo == null)
                    {
                        throw new ArgumentException("BindEventHandlerBase - cannot resolve 'MethodName' property  " + MethodName);
                    }

                    if (methodInfo.IsStatic)
                    {
#if WINDOWS_UWP
                        ResolvedEventHandler = methodInfo.CreateDelegate(desiredDelegateType);
#else
                        ResolvedEventHandler = Delegate.CreateDelegate(desiredDelegateType, methodInfo);
#endif
                    }
                    else
                    {
#if WINDOWS_UWP
                        ResolvedEventHandler = methodInfo.CreateDelegate(desiredDelegateType, sourceBaseProvided);
#else
                        ResolvedEventHandler = Delegate.CreateDelegate(desiredDelegateType, sourceBaseProvided, methodInfo);
#endif
                    }
                }
            }


            return ResolvedEventHandler;
        }

    }
}
