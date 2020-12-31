// 
//  MVVM-WPF-NetCore© Markup Binding Extensions.
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
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#if WINDOWS_UWP
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Controls.Primitives;
using System.Reflection;

#else
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Markup;
using System.Windows;
using System.Reflection;
#endif

namespace MvvmBindingPack
{
    /// <summary>
    /// The class supports an initialization/injection pattern. It's based on attached dependency property behaviors.
    /// </summary>
    public static partial class BindXAML
    {

#if WINDOWS_UWP
        internal static readonly DependencyProperty BindToCommandProperty = DependencyProperty.RegisterAttached("BindToCommand", typeof(object), typeof(BindXAML), new PropertyMetadata(null));
#else
        internal static readonly DependencyProperty BindToCommandProperty = DependencyProperty.RegisterAttached("BindToCommand.Shadow", typeof(object), typeof(BindXAML), new PropertyMetadata(null));
#endif
#if WINDOWS_UWP
        /// <summary>
        /// Sets a new value for the attached dependency property.
        /// Case WPF  - if an added object implements the MarkupExtension abstract class, the ProvideValue method will be called. The result 
        /// of this method will be passed as a value. Otherwise the object instance will be passed as a value.
        /// Case WinRt - Nothing will be called and an object instance will be passed as a value.
        /// </summary>
        /// <param name="dependencyObject">The target dependency object for an attached property.</param>
        /// <param name="value">The value to set for a attached dependency property.</param>
        /// <returns>WinRt should return a result an object for the attached property set type operation.</returns>
        public static object SetBindToCommand(DependencyObject dependencyObject, object value)
        {
            if (BindHelper.IsInDesignModeStatic)
            {
                // Cannot correctly set in the design mode.
                return null;
            }
#else
        /// <summary>
        /// Sets a new value for the attached dependency property.
        /// Case WPF  - if an added object implements the MarkupExtension abstract class, the ProvideValue method will be called. The result 
        /// of this method will be passed as a value. Otherwise the object instance will be passed as a value.
        /// Case WinRt - Nothing will be called and an object instance will be passed as a value.
        /// </summary>
        /// <param name="dependencyObject">The target dependency object for an attached property.</param>
        /// <param name="value">The value to set for a attached dependency property.</param>
        /// <returns>WinRt should return a result an object for the attached property set type operation.</returns>
        public static void SetBindToCommand(DependencyObject dependencyObject, object value)
        {
            if (BindHelper.IsInDesignModeStatic)
            {
                // Cannot correctly set in the design mode.
                return;
            }
#endif

            // For WPF, the BindCommandBase class returns the object instance of  the BindCommandBase class when the method ProvideValue has been called.
            object result = null;
            var frameworkElement = dependencyObject as FrameworkElement;
            var commandBase = value as BindCommandBase;
            if (commandBase == null)
            {
                throw new ArgumentException("BindToCommand accepts only BindCommand type objects");
            }
            string targetPropertyName = "Command";
            if (frameworkElement != null)
            {
                // CLR property
                PropertyInfo propInfo = dependencyObject.GetPropertyInfo(targetPropertyName);

                // WPF Dependency Property
                DependencyProperty depdInfo = BindHelper.LocateDependencyProperty(targetPropertyName, dependencyObject);

                if ((propInfo == null) && (depdInfo == null))
                {
                    throw new ArgumentException("AssignToProperties cannot resolve TargetPropertyName = " + targetPropertyName);
                }

                ProvideValueTarget provideValueTarget;
                if (propInfo != null)
                {
                    // CLR
                    provideValueTarget = new ProvideValueTarget(frameworkElement, propInfo);
                    ServiceProvider serviceProvider = new ServiceProvider(provideValueTarget);
                    // For WPF we have to call in the second time in order to get a correct value.
                    result = commandBase.ProvideValue(serviceProvider);
                    propInfo.SetValue(frameworkElement, result, null);
                }
                else
                {
                    // WPF
                    provideValueTarget = new ProvideValueTarget(frameworkElement, depdInfo);
                    ServiceProvider serviceProvider = new ServiceProvider(provideValueTarget);
                    result = commandBase.ProvideValue(serviceProvider);
                    dependencyObject.SetValue(depdInfo, result);
                }
            }
#if WINDOWS_UWP
            return result;
#endif
        }

    }
}
