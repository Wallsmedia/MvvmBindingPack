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
using System.Diagnostics;
using System.Reflection;
#if WINDOWS_UWP
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
#else
using System.Windows;
using System.Windows.Controls;
#endif

namespace MvvmBindingPack;

/// <summary>
/// Represents the class that holds the proxy method that will handle various routed events. 
/// </summary>
class DataContextProxyRoutedEventHandler
{
    /// <summary>
    /// Creates the instance of the class and returns the <see cref="RoutedEventHandler"/> type delegate to the proxy method.
    /// </summary>
    /// <param name="desiredDelegateType">Desired type of a delegate.</param>
    /// <param name="methodName">The method name.</param>
    /// <param name="propertyName">Property type.</param>
    /// <returns>Delegate of the <see cref="RoutedEventHandler"/> type.</returns>
    public static object CreateProxyDelegate(Type desiredDelegateType, string methodName, string propertyName = null)
    {
        var delInvoke = desiredDelegateType.GetMethodInfo("Invoke");
        var parms = delInvoke.GetParameters();
        Type typeGenerate = parms[1].ParameterType;
        Type classType = typeof(DataContextProxyRoutedEventHandlerGen<>);
        classType = classType.MakeGenericType(typeGenerate);
        var inst = Activator.CreateInstance(classType, methodName, propertyName);
        return BindHelper.EventHandlerDelegateFromMethod("RoutedEventHandlerProxy", inst, desiredDelegateType);
    }

}

class DataContextProxyRoutedEventHandlerGen<T>
{
    delegate void ProxyEventHandler(object sender, T e);

    private readonly String _methodName;

    private readonly String _propertyName;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="methodName"></param>
    /// <param name="propertyName"></param>
    public DataContextProxyRoutedEventHandlerGen(string methodName, string propertyName)
    {
        _methodName = methodName;
        _propertyName = propertyName;
    }


    private ProxyEventHandler _handler;
    private bool _errorInHandlerSetup;


    /// <summary>
    /// Represents the proxy method that will handle various routed events that do not 
    /// have specific event data beyond the data that is common for all routed events.
    /// </summary>
    /// <param name="sender">The object which is the originator of the event.</param>
    /// <param name="e">The event data.</param>
    public void RoutedEventHandlerProxy(object sender, T e)
    {
        if ((_handler == null) && (!_errorInHandlerSetup))
        {
            // Try to get a proper handler
            var element = sender as DependencyObject;
            if (element != null)
            {
                var sourceBaseProvided = BindHelper.LocateValidDependencyPropertyByAllTrees(element, FrameworkElement.DataContextProperty, _methodName, _propertyName);
                if (sourceBaseProvided != null)
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(_methodName))
                        {
                            _handler = (ProxyEventHandler)BindHelper.EventHandlerDelegateFromMethod(_methodName, sourceBaseProvided, typeof(ProxyEventHandler));
                        }
                        if (!string.IsNullOrEmpty(_propertyName))
                        {
                            _handler = (ProxyEventHandler)BindHelper.EventHandlerDelegateFromProperty(_propertyName, sourceBaseProvided, typeof(ProxyEventHandler));
                        }
                    }
#if DEBUG
                    catch (Exception ex)
                    {
                        // Should cause an exception if binding was not resolved.
                        Debug.WriteLine($"Cannot create delegate: {_methodName}|{_propertyName} for {sourceBaseProvided} Exception: {ex.Message}");
#else
                    catch
                    {
#endif
                        _errorInHandlerSetup = true;
                    }
                }
            }
        }
        if (_handler != null)
        {
            _handler(sender, e);
        }
    }

}