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
using System.Windows.Input;
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
    /// The abstract <see cref="ICommand"/> binding class of WPF XAML mark-up extension.
    /// Base class for implementing of binding to <see cref="ICommand"/> interface via CommandHandlerProxy or WeakCommandHandlerProxy class.
    /// </summary>
#if !WINDOWS_UWP
    [MarkupExtensionReturnTypeAttribute(typeof(Object))]
#endif
    public abstract class BindCommandBase : MarkupExtension
    {
        /// <summary>
        /// Used for late binding to dependency property
        /// </summary>
        protected object TargetObject { get; set; }

        /// <summary>
        /// Used for late binding to dependency property
        /// </summary>
        protected object TargetProperty { get; set; }

        /// <summary>
        /// THe ICommand interface proxy class hat was provided as a resulted.
        /// </summary>
        protected object ResolvedCommand { get; set; }

        /// <summary>
        /// Get a source object for binding.
        /// </summary>
        /// <param name="serviceProvider">Object that can provide services for the markup extension.</param>
        /// <returns></returns>
        protected abstract object ObtainSourceObject(IServiceProvider serviceProvider);

        /// <summary>
        /// If set on true, it will be used weak references when binding.
        /// This mode will provide weak referenced 'event CanExecuteChanged' for <see cref="ICommand"/> interface.
        /// </summary>
        public bool WeakBinding { get; set; }

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
        /// The method name of a source instance that performs as <see cref="ICommand.Execute"/> method.
        /// It's mutually exclusive versus 'ExecutePropertyName'.
        /// </summary>
        public String ExecuteMethodName { get; set; }

        /// <summary>
        /// The property name of a source instance that has a type of <see cref="System.Action"/> delegate
        /// that performs as <see cref="ICommand.Execute"/> method.
        /// It's mutually exclusive versus 'ExecuteMethodName'.
        /// </summary>
        public String ExecutePropertyName { get; set; }

        /// <summary>
        /// The property name of a source instance that refers to Boolean property that would be return 
        /// by <see cref="ICommand.CanExecute"/> method. INotifyPropertyChanged interface will be subscribed
        /// to trigger event "event EventHandler <see cref="ICommand.CanExecuteChanged"/>".
        /// It's mutually exclusive versus CanExecuteMethodName, CanExecutePropertyName,
        /// EventToInvokeCanExecuteChanged and PropertyActionCanExecuteChanged.
        /// </summary>
        public String CanExecuteBooleanPropertyName { get; set; }

        /// <summary>
        /// The method name of a source instance that performs as <see cref="ICommand.CanExecute"/> method.
        /// It's mutually exclusive versus 'CanExecutePropertyName'.
        /// </summary>
        public String CanExecuteMethodName { get; set; }

        /// <summary>
        /// The property name of a source instance that has a type of System.Func &lt; object, bool &gt; delegate
        /// that performs as <see cref="ICommand.CanExecute"/> method.
        /// It's mutually exclusive versus 'CanExecuteMethodName'.
        /// </summary>
        public String CanExecutePropertyName { get; set; }

        /// <summary>
        /// Name of an event class member to which will be added a delegate for rising an event in the proxy class, "event EventHandler <see cref="ICommand.CanExecuteChanged"/>".
        /// Notification delegate of types <see cref="System.Action"/> or <see cref="EventHandler"/> will be added or removed synchronously when the event handler will be add or removed in 
        /// "event EventHandler <see cref="ICommand.CanExecuteChanged"/>".
        /// It's mutually exclusive versus 'PropertyActionCanExecuteChanged'.
        /// </summary>
        public String EventToInvokeCanExecuteChanged { get; set; }

        /// <summary>
        /// Name of a property that will accept a delegate of <see cref="System.Action"/> type that can be used for rising an event in the proxy class, "event EventHandler <see cref="ICommand.CanExecuteChanged"/>".
        /// Notification delegate of types <see cref="System.Action"/> or <see cref="EventHandler"/> will be set or cleared synchronously when the event handler will be add or removed in 
        /// "event EventHandler <see cref="ICommand.CanExecuteChanged"/>".
        /// It's mutually exclusive versus 'EventToInvokeCanExecuteChanged'.
        /// </summary>
        public String PropertyActionCanExecuteChanged { get; set; }

        /// <summary>
        /// Returns an object that should be set on the property which this extension is set on.
        /// </summary>
        /// <param name="serviceProvider">An object that can provide services for the markup extension.</param>
        /// <returns>The object value to set on the property where the markup extension provided value is evaluated.</returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            // ReSharper disable SuggestUseVarKeywordEvident
            IProvideValueTarget service = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;
            // ReSharper restore SuggestUseVarKeywordEvident

            if ((service != null) && (service.TargetObject != null) && (service.TargetProperty != null))
            {
                // Save targets for case delayed processing
                TargetObject = service.TargetObject;
                TargetProperty = service.TargetProperty;

                if (!(service.TargetProperty is MethodInfo))
                {
#if !WINDOWS_UWP
                    // In WPF the target will be provided by calling environment.
                    if ((service.TargetProperty is DependencyProperty) && (((DependencyProperty)service.TargetProperty).PropertyType != typeof(ICommand)))
                    {
                        throw new ArgumentException("BindCommandBase - Target should be 'ICommand' type class member");
                    }
#else
                    // In WinRt the target property will be provided via BindXAML interface.
                    if ((service.TargetProperty as PropertyInfo != null) && (((PropertyInfo)service.TargetProperty).PropertyType != typeof(ICommand)))
                    {
                        throw new ArgumentException("BindCommandBase - Target should be 'ICommand' type class member");
                    }
#endif
                }
            }

            if (BindHelper.IsInDesignModeStatic)
            {
                // Cannot correctly resolve in the design mode the source 
                // TODO: Create empty for a nice view during design time
                return null;
            }

#if !WINDOWS_UWP

            //
            // WPF - Single Attached property special case.
            // 
            if ((service != null) && (service.TargetObject != null) && (service.TargetProperty != null) && (service.TargetProperty is MethodInfo))
            {
                // The WPF special support for a single attached property case. MethodInfo (attached property static method)
                // is the target property object. 
                // For WPF BindCommandBase, it returns the object instance when the method ProvideValue has been called.
                var mf = service.TargetProperty as MethodInfo;
                if (mf.DeclaringType == typeof(BindXAML))
                {
                    return this;
                }
            }
#endif

            object sourceBaseProvided = ObtainSourceObject(serviceProvider);
            if (sourceBaseProvided != null)
            {
                return ResolvePropertyValues(sourceBaseProvided);
            }
            else
            {
                // Delay DataContext binding for BindCommand class.
                if (this is BindCommand)
                {
                    if (((BindCommand)this).Source == null)
                    {
                        //Return the instance of BindCommand which provides delay binding for a DataContext property.
                        return this;
                    }
                }
            }
            return null;
        }

        internal object ResolvePropertyValues(object sourceBaseProvided)
        {
            if (sourceBaseProvided == null)
            {
                return null;
            }
            EventInfo outerEvent = null;
            PropertyInfo outerProperty = null;
            Action<object> executeDelegate = null;
            Func<object, bool> canExecuteDelegate = null;

            Type type = sourceBaseProvided.GetType();

            if ((!String.IsNullOrEmpty(ExecuteMethodName)) && (!String.IsNullOrEmpty(ExecutePropertyName)))
            {
                throw new ArgumentException("BindCommandBase - Should be set only one property 'ExecuteMethodName' or 'ExecutePropertyName'");
            }

            if ((!String.IsNullOrEmpty(CanExecuteBooleanPropertyName)) &&
                (!String.IsNullOrEmpty(CanExecuteMethodName)
                || !String.IsNullOrEmpty(CanExecutePropertyName)
                || !String.IsNullOrEmpty(EventToInvokeCanExecuteChanged)
                || !String.IsNullOrEmpty(PropertyActionCanExecuteChanged)))
            {
                throw new ArgumentException("BindCommandBase - Should be set only one property 'CanExecuteBooleanPropertyName'");
            }

            if ((!String.IsNullOrEmpty(CanExecuteMethodName)) && (!String.IsNullOrEmpty(CanExecutePropertyName)))
            {
                throw new ArgumentException("BindCommandBase - Should be set only one property 'CanExecuteMethodName' or 'CanExecutePropertyName'");
            }

            if ((!String.IsNullOrEmpty(EventToInvokeCanExecuteChanged)) && (!String.IsNullOrEmpty(PropertyActionCanExecuteChanged)))
            {
                throw new ArgumentException("BindCommandBase - Should be set only one property 'EventToInvokeCanExecuteChanged' or 'PropertyActionCanExecuteChanged'");
            }

            if (!String.IsNullOrEmpty(ExecuteMethodName))
            {
                MethodInfo executeMethod = type.GetMethodInfo(ExecuteMethodName);
                if (executeMethod == null)
                {
                    List<Tuple<Type, object>> locatedViewModels = BindHelper.LocateAppendedViewModels(sourceBaseProvided);
                    foreach (var model in locatedViewModels)
                    {
                        executeMethod = model.Item2.GetMethodInfo(ExecuteMethodName);
                        if (executeMethod != null)
                        {
                            sourceBaseProvided = model.Item2;
                            type = model.Item1;
                            break;
                        }
                    }
                }
                if (executeMethod == null)
                {
                    throw new ArgumentException("BindCommandBase - cannot resolve 'ExecuteMethodName' method  " + ExecuteMethodName);
                }

                if (executeMethod.IsStatic)
                {
#if WINDOWS_UWP
                    executeDelegate = (Action<object>)executeMethod.CreateDelegate(typeof(Action<object>));
#else
                    executeDelegate = (Action<object>)Delegate.CreateDelegate(typeof(Action<object>), executeMethod);
#endif
                }
                else
                {
#if WINDOWS_UWP
                    executeDelegate = (Action<object>)executeMethod.CreateDelegate(typeof(Action<object>), sourceBaseProvided);
#else
                    executeDelegate = (Action<object>)Delegate.CreateDelegate(typeof(Action<object>), sourceBaseProvided, executeMethod);
#endif
                }
            }
            else if (!String.IsNullOrEmpty(ExecutePropertyName))
            {
                // Get the property info and check the property type. 
                PropertyInfo methodProperty = type.GetPropertyInfo(ExecutePropertyName);

                if (methodProperty == null)
                {
                    List<Tuple<Type, object>> locatedViewModels = BindHelper.LocateAppendedViewModels(sourceBaseProvided);
                    foreach (var model in locatedViewModels)
                    {
                        methodProperty = model.Item2.GetPropertyInfo(ExecutePropertyName);
                        if (methodProperty != null)
                        {
                            sourceBaseProvided = model.Item2;
                            type = model.Item1;
                            break;
                        }
                    }
                }
                if ((methodProperty == null) || (methodProperty.PropertyType != typeof(Action<object>)))
                {
                    throw new ArgumentException("BindCommandBase - cannot resolve 'ExecutePropertyName' property " + ExecuteMethodName);
                }
                executeDelegate = (Action<object>)methodProperty.GetValue(sourceBaseProvided, null);
            }


            if (!String.IsNullOrEmpty(CanExecuteBooleanPropertyName))
            {
                PropertyInfo booleanProperty = type.GetPropertyInfo(CanExecuteBooleanPropertyName);
                if (booleanProperty == null)
                {
                    List<Tuple<Type, object>> locatedViewModels = BindHelper.LocateAppendedViewModels(sourceBaseProvided);
                    foreach (var model in locatedViewModels)
                    {
                        booleanProperty = model.Item2.GetPropertyInfo(CanExecuteBooleanPropertyName);
                        if (booleanProperty != null)
                        {
                            sourceBaseProvided = model.Item2;
                            type = model.Item1;
                            break;
                        }
                    }
                }
                if ((booleanProperty == null) || (booleanProperty.PropertyType != typeof(Boolean)))
                {
                    throw new ArgumentException("BindCommandBase - cannot resolve 'CanExecuteBooleanPropertyName' property  " + CanExecuteBooleanPropertyName);
                }
                ResolvedCommand = WeakBinding ? new WeakCommandHandlerProxy(executeDelegate, booleanProperty, sourceBaseProvided)
                : new CommandHandlerProxy(executeDelegate, booleanProperty, sourceBaseProvided);

                return ResolvedCommand;
            }
            if (!String.IsNullOrEmpty(CanExecuteMethodName))
            {
                MethodInfo canExecuteMethod = type.GetMethodInfo(CanExecuteMethodName);
                if (canExecuteMethod == null)
                {
                    List<Tuple<Type, object>> locatedViewModels = BindHelper.LocateAppendedViewModels(sourceBaseProvided);
                    foreach (var model in locatedViewModels)
                    {
                        canExecuteMethod = model.Item2.GetMethodInfo(CanExecuteMethodName);
                        if (canExecuteMethod != null)
                        {
                            sourceBaseProvided = model.Item2;
                            type = model.Item1;
                            break;
                        }
                    }
                }

                if (canExecuteMethod == null)
                {
                    throw new ArgumentException("BindCommandBase - cannot resolve 'CanExecuteMethodName' method " + CanExecuteMethodName);
                }
                if (canExecuteMethod.IsStatic)
                {
#if WINDOWS_UWP
                    canExecuteDelegate = (Func<object, bool>)canExecuteMethod.CreateDelegate(typeof(Func<object, bool>));
#else
                    canExecuteDelegate = (Func<object, bool>)Delegate.CreateDelegate(typeof(Func<object, bool>), canExecuteMethod);
#endif

                }
                else
                {
#if WINDOWS_UWP
                    canExecuteDelegate = (Func<object, bool>)canExecuteMethod.CreateDelegate(typeof(Func<object, bool>), sourceBaseProvided);
#else
                    canExecuteDelegate = (Func<object, bool>)Delegate.CreateDelegate(typeof(Func<object, bool>), sourceBaseProvided, canExecuteMethod);
#endif
                }
            }
            else if (!String.IsNullOrEmpty(CanExecutePropertyName))
            {
                PropertyInfo methodProperty = type.GetPropertyInfo(CanExecutePropertyName);
                if (methodProperty == null)
                {
                    List<Tuple<Type, object>> locatedViewModels = BindHelper.LocateAppendedViewModels(sourceBaseProvided);
                    foreach (var model in locatedViewModels)
                    {
                        methodProperty = model.Item2.GetPropertyInfo(CanExecutePropertyName);
                        if (methodProperty != null)
                        {
                            sourceBaseProvided = model.Item2;
                            type = model.Item1;
                            break;
                        }
                    }
                }
                if ((methodProperty == null) || (methodProperty.PropertyType != typeof(Func<object, bool>)))
                {
                    throw new ArgumentException("BindCommandBase - cannot resolve 'CanExecutePropertyName' property  " + CanExecuteMethodName);
                }
                canExecuteDelegate = (Func<object, bool>)methodProperty.GetValue(sourceBaseProvided, null);
            }

            if (!String.IsNullOrEmpty(EventToInvokeCanExecuteChanged))
            {
                outerEvent = type.GetEventInfo(EventToInvokeCanExecuteChanged);
                if (outerEvent == null)
                {
                    List<Tuple<Type, object>> locatedViewModels = BindHelper.LocateAppendedViewModels(sourceBaseProvided);
                    foreach (var model in locatedViewModels)
                    {
                        outerEvent = model.Item2.GetEventInfo(EventToInvokeCanExecuteChanged);
                        if (outerEvent != null)
                        {
                            sourceBaseProvided = model.Item2;
                            type = model.Item1;
                            break;
                        }
                    }
                }

                if (outerEvent == null)
                {
                    throw new ArgumentException("BindCommandBase - cannot resolve 'EventToInvokeCanExecuteChanged' event " + EventToInvokeCanExecuteChanged);
                }
            }

            if (!String.IsNullOrEmpty(PropertyActionCanExecuteChanged))
            {
                outerProperty = type.GetPropertyInfo(PropertyActionCanExecuteChanged);
                if (outerProperty == null)
                {
                    List<Tuple<Type, object>> locatedViewModels = BindHelper.LocateAppendedViewModels(sourceBaseProvided);
                    foreach (var model in locatedViewModels)
                    {
                        outerProperty = model.Item2.GetPropertyInfo(PropertyActionCanExecuteChanged);
                        if (outerProperty != null)
                        {
                            sourceBaseProvided = model.Item2;
                            type = model.Item1;
                            break;
                        }
                    }
                }

                if (outerProperty == null)
                {
                    throw new ArgumentException("BindCommandBase - cannot resolve 'PropertyActionCanExecuteChanged' property " + PropertyActionCanExecuteChanged);
                }
            }

            if ((outerEvent != null) || (outerProperty != null))
            {
                if (outerEvent != null)
                {
                    ResolvedCommand = WeakBinding ? new WeakCommandHandlerProxy(executeDelegate, canExecuteDelegate, outerEvent, sourceBaseProvided)
                              : new CommandHandlerProxy(executeDelegate, canExecuteDelegate, outerEvent, sourceBaseProvided);
                }
                else if (outerProperty != null)
                {
                    ResolvedCommand = WeakBinding ? new WeakCommandHandlerProxy(executeDelegate, canExecuteDelegate, outerProperty, sourceBaseProvided)
                              : new CommandHandlerProxy(executeDelegate, canExecuteDelegate, outerProperty, sourceBaseProvided);
                }

                return ResolvedCommand;
            }

            ResolvedCommand = WeakBinding ? new WeakCommandHandlerProxy(executeDelegate, canExecuteDelegate)
                       : new CommandHandlerProxy(executeDelegate, canExecuteDelegate);

            return ResolvedCommand;
        }

    }
}
