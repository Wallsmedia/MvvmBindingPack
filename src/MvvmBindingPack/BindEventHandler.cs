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


#if WINDOWS_UWP
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Controls.Primitives;

#else
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Markup;
using System.Xaml;
#endif


namespace MvvmBindingPack;

/// <summary>
/// XAML mark-up, BindXAML.AddEvents and BindXAML.AddPropertyChangeEvents extensions; 
/// it binds a control event to a method with a compatible signature of the object which is located in DataContext referenced object.
/// </summary>

#if !WINDOWS_UWP
[MarkupExtensionReturnTypeAttribute(typeof(RoutedEventHandler))]
#endif
public class BindEventHandler : BindEventHandlerBase
{
    /// <summary>
    /// It is a "back-door" feature which allows to setup the source object. If it is not set on, by default,
    /// the markup extension will use the defined DataContext property value.
    /// It is referring to the source object which has the method or property used by the markup extension. 
    /// There may be used {IocBinding ...} or other "agnostic" mark up extension(not {Binding ...}) which provides by the independent way a source object.
    /// </summary>
#if !WINDOWS_UWP
    [ConstructorArgument("serviceType")]
#endif
    public object Source { get; set; }

    /// <summary>
    /// Default constructor.
    /// </summary>
    public BindEventHandler() { }

    /// <summary>
    /// Constructs the class with a requested source.
    /// </summary>
    /// <param name="source">The source object.</param>
    public BindEventHandler(object source) { Source = source; }

    /// <summary>
    /// Get source object for binding. 
    /// If it is not set on, by default the method will search the first defined DataContext property value.
    /// </summary>
    /// <param name="serviceProvider">An object that can provide services for the markup extension.</param>
    /// <returns>Reference to a source object.</returns>

    protected override object ObtainSourceObject(IServiceProvider serviceProvider)
    {
        if (Source == null)
        {
            // For WinRT we provide a valid targets through BindXAML class. 
            // ReSharper disable SuggestUseVarKeywordEvident
            IProvideValueTarget service = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;
            // ReSharper restore SuggestUseVarKeywordEvident
            if ((service != null) && (service.TargetObject != null))
            {
                if (DeepScanAllTrees)
                {
                    Source = BindHelper.LocateValidDependencyPropertyByAllTrees(service.TargetObject as DependencyObject, FrameworkElement.DataContextProperty, MethodName, PropertyName);
                }
                else
                {
                    Source = BindHelper.LocateValidDependencyPropertyByAllTrees(service.TargetObject as DependencyObject, FrameworkElement.DataContextProperty);
                }
            }
        }
        return Source;
    }

}
