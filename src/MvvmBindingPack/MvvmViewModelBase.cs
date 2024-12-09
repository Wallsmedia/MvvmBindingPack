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
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Navigation;

namespace MvvmBindingPack;

/// <summary>
/// Base class for all view models
/// </summary>
abstract public class MvvmViewModelBase : NotifyPropertyChangedBase
{

    #region View - Model Notifications
    /// <summary>
    /// Occurs when a FrameworkElement has been constructed and added to the object tree, and is ready for interaction.
    /// </summary>
    /// <param name="sender">The Event Sender object.</param>
    /// <param name="e">Event arguments.</param>
    virtual public void Loaded(object sender, RoutedEventArgs e)
    {
    }

    /// <summary>
    /// Occurs when this object is no longer connected to the main object tree.
    /// </summary>
    /// <param name="sender">The Event Sender object.</param>
    /// <param name="e">Event arguments.</param>
    virtual public void Unloaded(object sender, RoutedEventArgs e)
    {
    }

    /// <summary>
    /// Invoked when the Page is loaded and becomes the current source of a parent Frame.
    /// </summary>
    /// <param name="sender">The Event Sender object.</param>
    /// <param name="e">Event data that can be examined by overriding code. The event data is representative of the pending navigation that will load the current Page. </param>
    virtual public void NavigatedTo(object sender, NavigationEventArgs e)
    {
    }

    /// <summary>
    /// Invoked immediately after the Page is unloaded and is no longer the current source of a parent Frame.
    /// </summary>
    /// <param name="sender">The Event Sender object.</param>
    /// <param name="e">Event data that can be examined by overriding code. The event data is representative of the navigation that has unloaded the current Page.</param>
    virtual public void NavigatedFrom(object sender, NavigationEventArgs e)
    {
    }

    /// <summary>
    /// Invoked immediately before the Page is unloaded and is no longer the current source of a parent Frame.
    /// </summary>
    /// <param name="sender">The Event Sender object.</param>
    /// <param name="e">Event data that can be examined by overriding code. The event data is representative of the navigation that will unload the current Page unless canceled. The navigation can potentially be canceled by setting Cancel.</param>
    virtual public void NavigatingFrom(object sender, NavigatingCancelEventArgs e)
    {
    }

    #endregion  View - Model Notifications
}
