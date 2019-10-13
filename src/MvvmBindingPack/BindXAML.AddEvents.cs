// 
//  MVVM-WPF-NetCore© Markup Binding Extensions.
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
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#if WINDOWS_UWP
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Controls.Primitives;

#else
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Markup;
using System.Windows;
#endif

namespace MvvmBindingPack
{
    /// <summary>
    /// The class supports an initialization/injection pattern. It's based on attached dependency property behaviors.
    /// </summary>
    public static partial class BindXAML
    {
#if WINDOWS_UWP
        internal static readonly DependencyProperty AddEventsProperty = DependencyProperty.RegisterAttached("AddEvents", typeof(FakeCollection), typeof(BindXAML), new PropertyMetadata(null));
#else
        // "Shadow" prefix is used in WPF for preventing XAML optimization, access directly dependency property.
        internal static readonly DependencyProperty AddEventsProperty = DependencyProperty.RegisterAttached("AddEvents.Shadow", typeof(FakeCollection), typeof(BindXAML), new PropertyMetadata(null));
#endif
        /// <summary>
        /// The attached dependency property returns the fake collection; it's used to add events to a FrameWorkElement object.
        /// It provides the fake collection which executes the the delegate "ProcessAddEventItems" 
        /// when "Add" a new item to the collection.
        /// </summary>
        /// <param name="dependencyObject">The target object for an attached dependency property.</param>
        /// <returns>The fake collection for processing elements.</returns>
        public static FakeCollection GetAddEvents(DependencyObject dependencyObject)
        {
            var fakecol = new FakeCollection
            {
                DependencyObjectElement = dependencyObject,
                ExecuteAction = ProcessAddEventItems
            };
            return fakecol;
        }

        /// <summary>
        /// The method which will be called for the attached dependency property when  the new element is added to a fake collection.
        /// </summary>
        /// <param name="dependencyObject">The target FrameworkElement.</param>
        /// <param name="item">The new object to add.</param>
        public static void ProcessAddEventItems(DependencyObject dependencyObject, object item)
        {
            if ((item == null) || (dependencyObject == null))
            {
                return;
            }

            UIElement uiElement = dependencyObject as UIElement;
            if (uiElement == null)
            {
                return;
            }
            if (BindHelper.IsInDesignModeStatic)
            {
                // Cannot correctly set in the design mode.
                return;
            }
          
            BindEventHandlerBase bindItem = item as BindEventHandlerBase;
            if (bindItem == null)
            {
                throw new ArgumentException("AddEvents accepts only BindEventHandlers Type instances ");
            }

            if (String.IsNullOrEmpty(bindItem.TargetEventName))
            {
                throw new ArgumentException("AddEvents accepts only BindEventHandlers with set TargetEventName");
            }

            var evntInfo = uiElement.GetEventInfo(bindItem.TargetEventName);
            if (evntInfo == null)
            {
                throw new ArgumentException("AddEvents cannot resolve TargetEventName = " + bindItem.TargetEventName);
            }

            ProvideValueTarget provideValueTarget = new ProvideValueTarget(uiElement, evntInfo);
            ServiceProvider serviceProvider = new ServiceProvider(provideValueTarget);
            var result = bindItem.ProvideValue(serviceProvider);
            if (result != null)
            {
#if WINDOWS_UWP
                evntInfo.AddMethod.Invoke(uiElement, new object[] {result});
#else
                evntInfo.AddEventHandler(uiElement, (Delegate)result);
#endif
            }
        }

    }
}
