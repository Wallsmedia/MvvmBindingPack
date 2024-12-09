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

namespace MvvmBindingPack;


/// <summary>
/// Supports based on the attached property behavior injection pattern.
/// </summary>
public static partial class BindXAML
{
#if WINDOWS_UWP
    internal static readonly DependencyProperty AssignPropertiesProperty = DependencyProperty.RegisterAttached("AssignProperties", typeof(FakeCollection), typeof(object), new PropertyMetadata(null));
#else
    // "Shadow" prefix is used in WPF for preventing XAML optimization, access directly dependency property.
    internal static readonly DependencyProperty AssignPropertiesProperty = DependencyProperty.RegisterAttached("AssignProperties.Shadow", typeof(FakeCollection), typeof(BindXAML), new PropertyMetadata(null));
#endif

    /// <summary>
    /// Sets properties - (XAML attached property multi set pattern.)
    /// </summary>
    /// <param name="dependencyObject">The target FrameworkElement.</param>
    public static FakeCollection GetAssignProperties(DependencyObject dependencyObject)
    {
        var fakecol = new FakeCollection();
        fakecol.DependencyObjectElement = dependencyObject as FrameworkElement;
        fakecol.ExecuteAction = ProcessSetPropertyItem;
        return fakecol;
    }

    /// <summary>
    /// Provides set a property - (XAML attached property set pattern.)
    /// </summary>
    /// <param name="dependencyObject">The target FrameworkElement.</param>
    /// <param name="item">The item  object to add.</param>
    private static void ProcessSetPropertyItem(DependencyObject dependencyObject, object item)
    {
        if ((item == null) || (dependencyObject == null))
        {
            return;
        }

        if (BindHelper.IsInDesignModeStatic)
        {
            // Cannot correctly set in the design mode.
            return;
        }

        var bindItemPropInf = item.GetPropertyInfo("TargetPropertyName");
        string targetPropertyName = (string)bindItemPropInf.GetValue(item, null);

        if (String.IsNullOrEmpty(targetPropertyName))
        {
            throw new ArgumentException("AssignToProperties accepts only elements with set 'TargetPropertyName'");
        }

        // CLR property
        PropertyInfo propInfo = dependencyObject.GetPropertyInfo(targetPropertyName);

        // WPF Dependency Property
        DependencyProperty depdInfo = BindHelper.LocateDependencyProperty(targetPropertyName, dependencyObject);

        if ((propInfo == null) && (depdInfo == null))
        {
            throw new ArgumentException("AssignToProperties cannot resolve 'TargetPropertyName' = " + targetPropertyName);
        }

        ProvideValueTarget provideValueTarget;
        var methodInfo = item.GetMethodInfo("ProvideValue");
        if (methodInfo == null)
        {
            throw new ArgumentException("AssignToProperties cannot resolve ProvideValue(...) Method");
        }
        if (propInfo != null)
        {
            // CLR
            provideValueTarget = new ProvideValueTarget(dependencyObject, propInfo);
            ServiceProvider serviceProvider = new ServiceProvider(provideValueTarget);
            var result = methodInfo.Invoke(item, new object[] { serviceProvider });
            propInfo.SetValue(dependencyObject, result, null);
        }
        else
        {
            // WPF
            provideValueTarget = new ProvideValueTarget(dependencyObject, depdInfo);
            ServiceProvider serviceProvider = new ServiceProvider(provideValueTarget);
            var result = methodInfo.Invoke(item, new object[] { serviceProvider });
            dependencyObject.SetValue(depdInfo, result);
        }

    }
}
