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
using System.Reflection;
using System.Diagnostics;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
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
using System.Windows.Markup;
using System.Diagnostics;
using System.Windows.Data;
#endif

namespace MvvmBindingPack;


/// <summary>
/// Public Class container.
/// Holds the resolved View XAML-defined element target metadata.
/// It may be a property or event metadata of the x:Named View XAML-defined element target.
/// </summary>
[DebuggerDisplay("XName = {XName},TargetName = {TargetName}, IsProperty = {IsProperty}, IsEvent = {IsEvent}")]
public class ViewXNameSourceTarget
{
    /// <summary>
    /// The target XAML object name.
    /// </summary>
    public string XName { get; set; }
    /// <summary>
    /// The target nameof the property or event.
    /// </summary>
    public string TargetName { get; set; }
    /// <summary>
    /// The target object source.
    /// </summary>
    public Object TargetObject { get; set; }
    /// <summary>
    /// The target object type.
    /// </summary>
    public Type TargetType { get; set; }
    /// <summary>
    /// The CLR property metadata.
    /// </summary>
    public PropertyInfo PropertyInfo { get; set; }
    /// <summary>
    /// The dependency property metadata.
    /// </summary>
    public DependencyProperty DependencyProperty { get; set; }
#if !WINDOWS_UWP
    /// <summary>
    /// The readonly dependency property metadata.
    /// </summary>
    public DependencyPropertyKey DependencyPropertyKey { get; set; }

    /// <summary>
    /// The readonly dependency property metadata.
    /// </summary>
    public DependencyPropertyDescriptor DependencyPropertyDescriptor
    {
        get
        {
            if ((TargetObject as DependencyObject != null) && (DependencyProperty != null) && (TargetObject != null))
            {
                return DependencyPropertyDescriptor.FromProperty(DependencyProperty, TargetType);
            }
            return null;
        }
    }
#endif
    /// <summary>
    /// The CLR event metadata.
    /// </summary>
    public EventInfo EventInfo { get; set; }
    /// <summary>
    /// The routing event metadata.
    /// </summary>
    public RoutedEvent RoutedEvent { get; set; }
#if !WINDOWS_UWP
    /// <summary>
    /// Retruns true if the property is readonly.
    /// </summary>
    public bool IsReadOnlyDpProperty { get { return DependencyPropertyKey != null; } }
#endif
    /// <summary>
    /// Returns true if the target is a property.
    /// </summary>
    public bool IsProperty { get { return PropertyInfo != null || DependencyProperty != null; } }
    /// <summary>
    /// Returns true if the target is an event.
    /// </summary>
    public bool IsEvent { get { return EventInfo != null || RoutedEvent != null; } }

    /// <summary>
    ///  Returns the current effective value of a dependency property on this instance
    ///  of a System.Windows.DependencyObject, or returns the property value of a specified object.
    /// </summary>
    /// <returns>The property value.</returns>
    public Object GetPropertyValue()
    {
        if (IsProperty && TargetObject != null)
        {
            try
            {
                if (DependencyProperty != null)
                {
                    DependencyObject dp = TargetObject as DependencyObject;
                    if (dp != null)
                    {
                        return dp.GetValue(DependencyProperty);
                    }
                }
                else if (PropertyInfo != null)
                {
                    return PropertyInfo.GetValue(TargetObject, null);
                }
            }
            catch
            {
                // ignored
            }
        }
        return null;
    }

#if !WINDOWS_UWP
    /// <summary>
    /// Sets the local value of a read-only dependency property, specified by its dependency
    /// property key identifier.
    /// </summary>
    /// <param name="obj">The new property value.</param>
    public void SetReadOnlyDpPropertyValue(object obj)
    {
        if (IsReadOnlyDpProperty && TargetObject != null)
        {
            try
            {
                if (DependencyPropertyKey != null)
                {
                    DependencyObject dp = TargetObject as DependencyObject;
                    if (dp != null)
                    {
                        dp.SetValue(DependencyPropertyKey, obj);
                    }
                }
            }
            catch { }
        }
    }
#endif

    /// <summary>
    /// Sets the local value of a dependency property, specified by its dependency
    /// property identifier, or sets the property value of a specified object.
    /// </summary>
    /// <param name="obj">The new property value.</param>
    public void SetPropertyValue(object obj)
    {
        if (IsProperty && TargetObject != null)
        {
            try
            {
                if (DependencyProperty != null)
                {
                    DependencyObject dp = TargetObject as DependencyObject;
                    if (dp != null)
                    {
                        dp.SetValue(DependencyProperty, obj);
                    }
                }
                else if (PropertyInfo != null)
                {
                    PropertyInfo.SetValue(TargetObject, obj, null);
                }
            }
            catch
            {
                // ignored
            }
        }
    }

    /// <summary>
    /// Adds a routed event handler for a specified routed event, adding the handler
    ///  to the handler collection on the current element. Specify handledEventsToo
    ///  as true to have the provided handler be invoked for routed event that had
    ///  already been marked as handled by another element along the event route, or  adds an event handler to an event source.
    /// </summary>
    /// <param name="del"> A reference to the handler implementation; it encapsulates a method or methods to be invoked when the event is raised by the target.</param>
    /// <param name="handledEventsToo"> true to register the handler such that it is invoked even when the routed
    ///  event is marked handled in its event data; false to register the handler
    ///  with the default condition that it will not be invoked if the routed event
    ///  is already marked handled. The default is false.Do not routinely ask to re-handle
    ///  a routed event.</param>
    public void AddEventHandler(Delegate del, bool handledEventsToo = false)
    {
        if (IsEvent && TargetObject != null)
        {
            try
            {
                if (RoutedEvent != null)
                {
                    UIElement uie = TargetObject as UIElement;
                    if (uie != null)
                    {
                        uie.AddHandler(RoutedEvent, del, handledEventsToo);
                    }
                }
                else if (EventInfo != null)
                {
                    EventInfo.AddEventHandler(TargetObject, del);
                }
            }
            catch
            {
                // ignored
            }
        }
    }

    /// <summary>
    /// Removes the specified routed event handler from this element, or removes an event handler from an event source.
    /// </summary>
    /// <param name="del">A reference to the handler implementation.</param>
    public void RemoveEventHandler(Delegate del)
    {
        if (IsEvent && TargetObject != null)
        {
            try
            {
                if (RoutedEvent != null)
                {
                    UIElement uie = TargetObject as UIElement;
                    if (uie != null)
                    {
                        uie.RemoveHandler(RoutedEvent, del);
                    }
                }
                else if (EventInfo != null)
                {
                    EventInfo.RemoveEventHandler(TargetObject, del);
                }
            }
            catch
            {
                // ignored
            }
        }
    }

}
