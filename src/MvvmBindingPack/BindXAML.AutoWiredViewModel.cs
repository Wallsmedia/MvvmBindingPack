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
using System.Windows;
using System.Windows.Input;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;


#if WINDOWS_UWP

using Windows.UI.Xaml;

#else
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
#if BEHAVIOR
using System.Windows.Interactivity;
#endif
using System.Windows.Threading;
#endif

namespace MvvmBindingPack
{

    /// <summary>
    /// Partial class contains a collection of attached properties. 
    /// </summary>
    public static partial class BindXAML
    {

        /// <summary>
        /// Defines the "AutoWiredViewModel" inherited attached dependency property identifier.
        /// </summary>
#if WINDOWS_UWP
        public static readonly DependencyProperty AutoWiredViewModelProperty = DependencyProperty.RegisterAttached("AutoWiredViewModel", typeof(object), typeof(BindXAML), new PropertyMetadata(null));
#else
        public static readonly DependencyProperty AutoWiredViewModelProperty = DependencyProperty.RegisterAttached("AutoWiredViewModel", typeof(object), typeof(BindXAML), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));
#endif

        /// <summary>
        /// Attached property "AutoWiredViewModel" set accessor implementation.
        /// Sets the local value of a dependency property, specified by its attached dependency property identifier.
        /// </summary>
        /// <param name="obj">The dependency object.</param>
        /// <param name="value">The value to set to dependency property.</param>
        public static void SetAutoWiredViewModel(DependencyObject obj, object value)
        {
            obj.SetValue(AutoWiredViewModelProperty, value);
        }

        /// <summary>
        /// Attached property "AutoWiredViewModel" get accessor implementation.
        /// Gets the local value of a dependency property, specified by its attached dependency property identifier.
        /// </summary>
        /// <param name="obj">The dependency object.</param>
        /// <returns>Returns the current effective value.</returns>
        public static object GetAutoWiredViewModel(DependencyObject obj)
        {
            return obj.GetValue(AutoWiredViewModelProperty);
        }

    }

}
