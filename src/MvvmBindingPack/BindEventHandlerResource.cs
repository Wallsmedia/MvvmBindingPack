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


namespace MvvmBindingPack
{
    /// <summary>
    /// WPF and WinRt XAML mark-up extension.
    /// Binds control's <see cref="EventHandler"/> property type to a source class member of an object located in resource dictionaries.
    /// </summary>

#if !WINDOWS_UWP
    [MarkupExtensionReturnTypeAttribute(typeof(RoutedEventHandler))]
#endif
    public class BindEventHandlerResource : BindEventHandlerBase
    {

#if  !WINDOWS_UWP
        readonly StaticResourceExtension _resourceSource;
#endif

#if !WINDOWS_UWP
        /// <summary>
        ///  Gets or sets the key value passed by a static resource reference. The
        ///  key is used to return the object matching that key in resource dictionaries.
        /// </summary>
        [ConstructorArgument("resourceKey")]
        public object ResourceKey
        {
            get { return _resourceSource.ResourceKey; }
            set { _resourceSource.ResourceKey = value; }
        }
#else
        /// <summary>
        ///  Gets or sets the key value passed by a static resource reference. The
        ///  key is used to return the object matching that key in resource dictionaries.
        /// </summary>
        public object ResourceKey { get; set; }

#endif

        /// <summary>
        /// Get a source object for binding, an object matching the key in resource dictionaries.
        /// </summary>
        /// <param name="serviceProvider">Object that can provide services for the markup extension.</param>
        /// <returns></returns>
        protected override object ObtainSourceObject(IServiceProvider serviceProvider)
        {

#if WINDOWS_UWP
            // ReSharper disable SuggestUseVarKeywordEvident
            IProvideValueTarget service = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;
            // ReSharper restore SuggestUseVarKeywordEvident
            // For WinRT we provide a valid targets through BindXAML class. 
            object obj = BindHelper.LocateResource(service.TargetObject as FrameworkElement, ResourceKey);
            return obj;
#else
            // For WPF we have got a StaticResourceExtension to get a Resource
            return _resourceSource.ProvideValue(serviceProvider);
#endif
        }

#if !WINDOWS_UWP
        /// <summary>
        /// Default constructor.
        /// </summary>
        public BindEventHandlerResource()
        {
            _resourceSource = new StaticResourceExtension();
        }

        /// <summary>
        /// Constructs the class with a requested resource key.
        /// </summary>
        /// <param name="resourceKey">Requested resource key.</param>
        public BindEventHandlerResource(object resourceKey)
        {
            _resourceSource = new StaticResourceExtension(resourceKey);
        }
#endif

#if WINDOWS_UWP

        /// <summary>
        /// Default constructor.
        /// </summary>
        public BindEventHandlerResource()
        {
        }

        /// <summary>
        /// Constructs the class with a requested resource key.
        /// </summary>
        /// <param name="resourceKey">Requested resource key.</param>
        public BindEventHandlerResource(object resourceKey)
        {
            ResourceKey = resourceKey;
        }

#endif

    }
}
