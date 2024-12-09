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
using System.Windows.Input;

#if WINDOWS_UWP
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Controls.Primitives;
#else
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Markup;
using System.ComponentModel;
#endif

namespace MvvmBindingPack;

/// <summary>
/// WPF and WinRt XAML mark-up extension.
/// Binds Command property to <see cref="ICommand"/> interface via a proxy class to a class member of a source object.
/// </summary>
#if !WINDOWS_UWP
[MarkupExtensionReturnTypeAttribute(typeof(ICommand))]
#endif
public class BindCommand : BindCommandBase, ICommand
{

    /// <summary>
    /// It is a "back-door" feature which allows to setup the source object. If it is not set on, by default,
    /// the markup extension will use the defined DataContext property value.
    /// It is referring to the source object which has the method or property used by the markup extension. 
    /// There may be used {IocBinding ...} or other "agnostic" mark up extension(not {Binding ...}) which provides by the independent way a source object.
    /// </summary>
#if !WINDOWS_UWP
    [ConstructorArgument("source")]
#endif
    public object Source { get; set; }

    /// <summary>
    /// Default constructor.
    /// </summary>
    public BindCommand() { }

    /// <summary>
    /// Constructs the class with a requested source.
    /// </summary>
    /// <param name="source">The source object.</param>
    public BindCommand(object source) { Source = source; }

    /// <summary>
    /// Get a source object for binding. 
    /// If it is not set on, by default the method will search the first defined DataContext property value.
    /// </summary>
    /// <param name="serviceProvider">Object that can provide services for the markup extension.</param>
    /// <returns>Reference to a source object.</returns>
    protected override object ObtainSourceObject(IServiceProvider serviceProvider)
    {
        // For WinRT we provide a valid targets through BindXAML class. 
        if (Source == null)
        {
            // ReSharper disable SuggestUseVarKeywordEvident
            IProvideValueTarget service = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;
            // ReSharper restore SuggestUseVarKeywordEvident

            if ((service != null) && (service.TargetObject != null))
            {
                if (DeepScanAllTrees)
                {
                    return BindHelper.LocateValidDependencyPropertyByAllTrees(service.TargetObject as DependencyObject, FrameworkElement.DataContextProperty, ExecuteMethodName, ExecutePropertyName);
                }
                else
                {
                    return BindHelper.LocateValidDependencyPropertyByAllTrees(service.TargetObject as DependencyObject, FrameworkElement.DataContextProperty);
                }
            }
        }

        if (Source is string)
        {

        }

        return Source;
    }

    bool ICommand.CanExecute(object parameter)
    {
        return false;
    }

#if !WINDOWS_UWP
    void LateBiningOfCommandInterface(object obj, EventArgs args)
#else
    void LateBiningOfCommandInterface(object obj, RoutedEventArgs args)
#endif
    {
        object dataContext;
        if (DeepScanAllTrees)
        {
            dataContext = BindHelper.LocateValidDependencyPropertyByAllTrees(TargetObject as DependencyObject, FrameworkElement.DataContextProperty, ExecuteMethodName, ExecutePropertyName);
        }
        else
        {
            dataContext = BindHelper.LocateValidDependencyPropertyByAllTrees(TargetObject as DependencyObject, FrameworkElement.DataContextProperty);
        }

        var frameworkElement = TargetObject as FrameworkElement;
        if (frameworkElement == null)
        {
            return;
        }
        // Remove handler back

#if !WINDOWS_UWP
        DependencyPropertyDescriptor dppDescr = DependencyPropertyDescriptor.FromProperty(FrameworkElement.DataContextProperty, FrameworkElement.DataContextProperty.OwnerType);
        dppDescr.RemoveValueChanged(frameworkElement, LateBiningOfCommandInterface);
#else
        frameworkElement.Loaded -= LateBiningOfCommandInterface;
#endif

        if (dataContext != null)
        {
            var replaceCommand = ResolvePropertyValues(dataContext);

#if !WINDOWS_UWP
            if (TargetProperty is DependencyProperty)
            {
                frameworkElement.SetValue((DependencyProperty)TargetProperty, replaceCommand);
            }

#else
            if (TargetProperty as PropertyInfo != null)
            {
                ((PropertyInfo)TargetProperty).SetValue(TargetObject, replaceCommand);
            }
#endif

        }
    }

    event EventHandler ICommand.CanExecuteChanged
    {
        add
        {
            var frameworkElement = TargetObject as FrameworkElement;
            if (frameworkElement != null)
            {
#if !WINDOWS_UWP
                DependencyPropertyDescriptor dppDescr = DependencyPropertyDescriptor.FromProperty(FrameworkElement.DataContextProperty, FrameworkElement.DataContextProperty.OwnerType);
                dppDescr.AddValueChanged(frameworkElement, LateBiningOfCommandInterface);
#else
                frameworkElement.Loaded += LateBiningOfCommandInterface;
#endif
            }


        }
        remove
        {
            var frameworkElement = TargetObject as FrameworkElement;
            if (frameworkElement != null)
            {
#if !WINDOWS_UWP
                DependencyPropertyDescriptor dppDescr = DependencyPropertyDescriptor.FromProperty(FrameworkElement.DataContextProperty, FrameworkElement.DataContextProperty.OwnerType);
                dppDescr.RemoveValueChanged(frameworkElement, LateBiningOfCommandInterface);
#else
                frameworkElement.Loaded -= LateBiningOfCommandInterface;
#endif
            }

        }
    }

    void ICommand.Execute(object parameter)
    {
    }
}
