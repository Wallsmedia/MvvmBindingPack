// 
//  MVVM-WPF Markup Dependency Injection Binding Extensions
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
#endif

namespace MvvmBindingPack;

/// <summary>
/// WPF and WinRt XAML mark-up extension.
/// Binds <see cref="EventHandler"/> property type to a target property by resolving it through the generic Service Locator interface.
/// </summary>
#if !WINDOWS_UWP
[MarkupExtensionReturnTypeAttribute(typeof(RoutedEventHandler))]
#endif
public class BindEventHandlerIoc : BindEventHandlerBase
{

    readonly IocBinding _iocSource;
    /// <summary>
    /// Type or type name (string) of the requested object.
    /// </summary>
#if !WINDOWS_UWP
    [ConstructorArgument("serviceType")]
#endif
    public object ServiceType
    {
        get { return _iocSource.ServiceType; }
        set { _iocSource.ServiceType = value; }
    }

    /// <summary>
    /// Key (string) of the requested object.
    /// </summary>
    [Obsolete("ServiceKey not supported and ignored")]
    public string ServiceKey
    {
        get;set;
    }

    /// <summary>
    /// Default constructor.
    /// </summary>
    public BindEventHandlerIoc()
    {
        _iocSource = new IocBinding();
    }

    /// <summary>
    /// Constructs the class with a requested service type.
    /// </summary>
    /// <param name="serviceType">Type or type name of the requested object.</param>
    public BindEventHandlerIoc(object serviceType)
    {
        _iocSource = new IocBinding(serviceType);
    }

    /// <summary>
    /// Get source object for binding
    /// </summary>
    /// <param name="serviceProvider">Object that can provide services for the markup extension</param>
    /// <returns></returns>
    protected override object ObtainSourceObject(IServiceProvider serviceProvider)
    {
        return _iocSource.ProvideValue(serviceProvider);
    }

}
