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
using System.Windows;
using System.Windows.Input;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;


#if WINDOWS_UWP

using Windows.UI.Xaml;
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
using System.Reflection;
#endif


namespace MvvmBindingPack
{

    /// <summary>
    /// The class supports an VM auto wiring/initialization/injection pattern. It's based on attached dependency property behaviors.
    /// Partial class contains a collection of attached properties. 
    /// </summary>
    public static partial class BindXAML
    {

#if WINDOWS_UWP
        /// <summary>
        /// XAML attached property, a fake collection, that used for processing extensions: AutoWireVmDataContext, AutoWireViewConrols
        /// </summary>
        public static readonly DependencyProperty ProcessMvvmExtensionsProperty = DependencyProperty.RegisterAttached("ProcessMvvmExtensions", typeof(FakeCollection), typeof(BindXAML), new PropertyMetadata(null));
#else
        // ".Shadow" suffix is used in WPF for preventing XAML optimization, access directly dependency property.
        /// <summary>
        /// XAML attached property, a fake collection, that used for processing extensions: AutoWireVmDataContext, AutoWireViewConrols
        /// </summary>
        public static readonly DependencyProperty ProcessMvvmExtensionsProperty = DependencyProperty.RegisterAttached("ProcessMvvmExtensions.Shadow", typeof(FakeCollection), typeof(BindXAML), new PropertyMetadata(null));
#endif
        /// <summary>
        /// The attached dependency property returns the fake collection; it's used to add events to a FrameWorkElement object.
        /// It provides the fake collection which executes the the delegate "ProcessAddEventItems" 
        /// when "Add" a new item to the collection.
        /// </summary>
        /// <param name="dependencyObject">The target object for an attached dependency property.</param>
        /// <returns>The fake collection for processing elements.</returns>
        public static FakeCollection GetProcessMvvmExtensions(DependencyObject dependencyObject)
        {
            var fakecol = new FakeCollection();
            fakecol.DependencyObjectElement = dependencyObject;
            fakecol.ExecuteAction = ProcessAddMvvmExtensions;
            return fakecol;
        }

        /// <summary>
        /// The method which will be called for the attached dependency property when  the new element is added to a fake collection.
        /// </summary>
        /// <param name="dependencyObject">The target FrameworkElement.</param>
        /// <param name="item">The new object to add.</param>
        internal static void ProcessAddMvvmExtensions(DependencyObject dependencyObject, object item)
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

            //Item has 'void Execute(DependencyObject dependencyObject)' or 'void Execute(object frameworkElement)'  method
            MethodInfo executeMethodInfo = item.GetMethodInfo("Execute");
            if (executeMethodInfo != null)
            {
                //Execute the enhancer to dependency object element
                executeMethodInfo.Invoke(item, new object[] { dependencyObject });
            }

        }
    }
}
