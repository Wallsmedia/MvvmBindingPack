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

#if WINDOWS_UWP
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Controls.Primitives;

#else
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Markup;

#endif

namespace MvvmBindingPack
{
#if WINDOWS_UWP

    /// <summary>
    ///  Provides a base class for XAML markup extension implementations that can
    ///  be supported by .NET Framework XAML Services and other XAML readers and XAML
    ///  writers.
    /// </summary>
    public abstract class MarkupExtension
    {

        /// <summary>
        /// When implemented in a derived class, returns an object that is provided as
        /// the value of the target property for this markup extension.
        /// </summary>
        /// <param name="serviceProvider"> A service provider helper that can provide services for the markup extension.</param>
        /// <returns>The object value to set on the property where the extension is applied.</returns>
        public abstract object ProvideValue(IServiceProvider serviceProvider);
    }

    /// <summary>
    ///  Defines a mechanism for retrieving a service object; that is, an object that
    ///     provides custom support to other objects.
    /// </summary>
    public interface IServiceProvider
    {
        /// <summary>
        /// Gets the service object of the specified type.
        /// </summary>
        /// <param name="serviceType"> An object that specifies the type of service object to get.</param>
        /// <returns>A service object of type serviceType.-or- null if there is no service object
        /// of type serviceType.</returns>
        object GetService(Type serviceType);
    }

    /// <summary>
    /// Represents a service that reports situational object-property relationships
    /// for markup extension evaluation.
    /// </summary>
    public interface IProvideValueTarget
    {
        ///<summary>
        /// Gets the target object being reported.
        ///</summary>
        ///<returns>
        /// The target object being reported.
        ///</returns>
        object TargetObject { get; set; }

        ///<summary>
        /// Gets an identifier for the target property being reported.
        ///</summary>
        ///<returns>
        /// An identifier for the target property being reported.
        ///</returns>
        object TargetProperty { get; set; }
    }
#endif

    /// <summary>
    /// Proxy class simulation. Defines a mechanism for retrieving a service object; that is, an object that
    /// provides custom support to other objects.
    /// </summary>
    public class ServiceProvider : IServiceProvider
    {
        IProvideValueTarget _iProvideValueTarget;
        /// <summary>
        /// constructs the proxy class provider simulation. 
        /// </summary>
        /// <param name="iProvideValueTarget"> Represents a service that reports situational object-property relationships
        /// for markup extension evaluation.</param>
        public ServiceProvider(IProvideValueTarget iProvideValueTarget)
        {
            _iProvideValueTarget = iProvideValueTarget;
        }

        /// <summary>
        /// Gets the service object of the specified type.
        /// </summary>
        /// <param name="serviceType"> An object that specifies the type of service object to get.</param>
        /// <returns>
        /// A service object of type serviceType.-or- null if there is no service object
        /// of type serviceType.
        ///</returns>
        public object GetService(Type serviceType)
        {
            if (serviceType == typeof(IProvideValueTarget))
            {
                return _iProvideValueTarget;
            }
            return null;
        }
    }

    /// <summary>
    /// POCO class implementation. It represents a service that reports situational object-property relationships
    /// for markup extension evaluation.
    /// </summary>
    public class ProvideValueTarget : IProvideValueTarget
    {
        /// <summary>
        /// Constructs a class container.
        /// </summary>
        /// <param name="target">The target object being reported.</param>
        /// <param name="property"> The target property being reported.</param>
        public ProvideValueTarget(object target, object property)
        {
            TargetObject = target;
            TargetProperty = property;
        }
        /// <summary>
        ///The target object being reported.
        /// </summary>
        public object TargetObject { get; set; }

        /// <summary>
        /// The target property being reported.
        /// </summary>
        public object TargetProperty { get; set; }
    }
}

