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
using System.Windows.Input;
#if WINDOWS_UWP

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using System.IO;

#else
using System.Windows;
using System.Windows.Controls;
using System.Linq;
using System.Windows.Media;
using System.Collections.Generic;
using System.Reflection;
using System.ComponentModel;


#endif

namespace MvvmBindingPack;

/// <summary>
///  Application Binder Helper.
/// </summary>
public static partial class BindHelper
{
    /// <summary>
    /// Locate properties and fields that marked with attribute <see cref="AppendViewModelAttribute"/>
    /// </summary>
    /// <param name="obj">Object to scan</param>
    /// <returns>Returns the list of located objects.</returns>
    static public List<Tuple<Type, object>> LocateAppendedViewModels(object obj)
    {
        List<Tuple<Type, object>> res = new List<Tuple<Type, object>>();
        var lookUpObject = obj;
        var lookUpType = obj.GetType();

#if WINDOWS_UWP
        IEnumerable<FieldInfo> info;
#else
        FieldInfo[] info;
#endif
#if WINDOWS_UWP
            info = lookUpType.GetTypeInfo().DeclaredFields;
#else
        info = lookUpType.GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
#endif
        foreach (var item in info)
        {
#if !WINDOWS_UWP
            var attribR = Attribute.GetCustomAttributes(item, typeof(AppendViewModelAttribute));
            var fType = item.FieldType;
#else
           var attribR = item.GetCustomAttributes(typeof(AppendViewModelAttribute));
           var fType = item.FieldType.GetTypeInfo();
#endif

            if (!fType.IsValueType && !BindHelper.CheckKnownTypePrefix(fType.FullName))
            {
                foreach (Attribute att in attribR)
                {
                    object value = item.GetValue(lookUpObject);
                    if (value != null)
                    {
                        res.Add(new Tuple<Type, object>(item.FieldType, value));
                    }
                }
            }
        }

#if WINDOWS_UWP
        IEnumerable<PropertyInfo> propertyInfos;
        propertyInfos = lookUpType.GetTypeInfo().DeclaredProperties;
#else
        PropertyInfo[] propertyInfos;
        propertyInfos = lookUpType.GetProperties(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
#endif
        foreach (var item in propertyInfos)
        {

#if !WINDOWS_UWP
            var attribR = Attribute.GetCustomAttributes(item, typeof(AppendViewModelAttribute));
            var fType = item.PropertyType;
#else
           var attribR = item.GetCustomAttributes(typeof(AppendViewModelAttribute));
           var fType = item.PropertyType.GetTypeInfo();
#endif

            if (!fType.IsValueType && !BindHelper.CheckKnownTypePrefix(fType.FullName))
            {
                foreach (Attribute att in attribR)
                {
                    object value = item.GetValue(lookUpObject, null);
                    if (value != null)
                    {
                        res.Add(new Tuple<Type, object>(item.PropertyType, value));
                    }
                }
            }
        }

        return res;
    }
    /// <summary>
    /// Find an element in resource directories. The page resource directory will be 
    /// check before the application resource directory.
    /// </summary>
    /// <param name="frameworkElement">XAML object element that contains the intended page content.</param>
    /// <param name="resourceKey">A key object that referenced to an element in the resource directory.</param>/// Locate the Resource element.
    /// <returns>Resolved resource element.</returns>
    public static object LocateResource(FrameworkElement frameworkElement, object resourceKey)
    {
        object resolvedResource = null;
#if !WINDOWS_UWP
        if (frameworkElement != null)
        {
            resolvedResource = frameworkElement.TryFindResource(resourceKey);
        }
#endif

        if (resolvedResource == null)
        {
            resolvedResource = LocateResourceFromLogicalTree(frameworkElement, resourceKey);
        }
        if (resolvedResource == null)
        {
            resolvedResource = LocateResourceFromVisualTree(frameworkElement, resourceKey);
        }

#if WINDOWS_UWP
        if (resolvedResource == null)
        {
            var rootFrameworkElement = Window.Current.Content as FrameworkElement;
            if (rootFrameworkElement != null)
            {
                resolvedResource = LocateResourceFromLogicalTree(rootFrameworkElement, resourceKey);
            }
        }
#endif
        if (resolvedResource == null)
        {
#if WINDOWS_UWP
            if ((Application.Current != null) && (Application.Current.Resources.Keys.Contains(resourceKey)))
            {
                resolvedResource = Application.Current.Resources[resourceKey];
            }
#else
            if (Application.Current != null)
            {
                resolvedResource = Application.Current.Resources[resourceKey];
            }
#endif
        }
        return resolvedResource;
    }


    /// <summary>
    /// Find an element in resource directories. The logical tree will be scanning.
    /// </summary>
    /// <param name="frameworkElement">XAML object element that contains the intended page content.</param>
    /// <param name="resourceKey">A key object that referenced to an element in the resource directory.</param>/// Locate the Resource element.
    /// <returns>Resolved resource element.</returns> 
    public static object LocateResourceFromLogicalTree(FrameworkElement frameworkElement, object resourceKey)
    {
        object resolvedResource = null;
        if (frameworkElement != null)
        {
            FrameworkElement currentElement = frameworkElement;
            while (currentElement != null)
            {

#if WINDOWS_UWP
                if (currentElement.Resources.Keys.Contains(resourceKey))
                {
                    resolvedResource = currentElement.Resources[resourceKey];
                    break;
                }
#else
                resolvedResource = currentElement.Resources[resourceKey];
                if (resolvedResource != null)
                {
                    break;
                }

#endif

#if WINDOWS_UWP
                currentElement = currentElement.Parent as FrameworkElement;
#else
                currentElement = LogicalTreeHelper.GetParent(currentElement) as FrameworkElement;
#endif

            }
        }
        return resolvedResource;
    }


    /// <summary>
    /// Find an element in resource directories. The visual tree will be scanning.
    /// </summary>
    /// <param name="frameworkElement">XAML object element that contains the intended page content.</param>
    /// <param name="resourceKey">A key object that referenced to an element in the resource directory.</param>/// Locate the Resource element.
    /// <returns>Resolved resource element.</returns>
    public static object LocateResourceFromVisualTree(FrameworkElement frameworkElement, object resourceKey)
    {
        object resolvedResource = null;
        if (frameworkElement != null)
        {
            FrameworkElement currentElement = frameworkElement;
            while (currentElement != null)
            {

#if WINDOWS_UWP
                if (currentElement.Resources.Keys.Contains(resourceKey))
                {
                    resolvedResource = currentElement.Resources[resourceKey];
                    break;
                }
#else
                resolvedResource = currentElement.Resources[resourceKey];
                if (resolvedResource != null)
                {
                    break;
                }

#endif
                currentElement = VisualTreeHelper.GetParent(currentElement) as FrameworkElement;
            }
        }
        return resolvedResource;
    }
    private static bool? _isInDesignMode;

    /// <summary>
    /// Gets a value indicating whether the running control is in design mode (running in Blend or Visual Studio).
    /// </summary>
    public static bool IsInDesignModeStatic
    {
        get
        {
            if (!_isInDesignMode.HasValue)
            {
#if WINDOWS_UWP
                _isInDesignMode = Windows.ApplicationModel.DesignMode.DesignModeEnabled;
#else
                var prop = DesignerProperties.IsInDesignModeProperty;
                _isInDesignMode = (bool)DependencyPropertyDescriptor.FromProperty(prop, typeof(FrameworkElement)).Metadata.DefaultValue;
#endif
            }
            return _isInDesignMode.Value;
        }
    }

    /// <summary>
    /// Locate the first defined value of Dependency property by walking the logical tree up to the roots.
    /// </summary>
    /// <param name="depObject">The DependencyObject object.</param>
    /// <param name="depProp">The dependency property to locate.</param>
    /// <param name="methodName">The method name in the dependency property object.
    /// The properties will be scan up over the logical tree until the  method name will be in math  to the method in the object 
    /// that represented by a property value.</param>
    /// <param name="properyName">The property name in the dependency property object.
    /// The properties will be scan up over the logical tree until the  property name will be in math  to the property in the object 
    /// that represented by a property value.</param>
    /// <param name="eventName">The event name in the dependency property object.
    /// The properties will be scan up over the logical tree until the  event name will be in math  to the event in the object 
    /// that represented by a property value.</param>
    /// <param name="matchType">The type which should match with a property or event or method name or by it self.</param>
    /// <returns>The value of located defined property or null.</returns>
    public static object LocateValidDependencyPropertyByLogicalTree(DependencyObject depObject, DependencyProperty depProp,
        string methodName = null, string properyName = null, string eventName = null, Type matchType = null)
    {
        var current = depObject;
        object fisrtLocated = null;
        while (current != null)
        {
            var value = current.GetValue(depProp);
            if (value != null)
            {
                if (fisrtLocated == null)
                {
                    fisrtLocated = value;
                }

                if (ValidateValue(value, methodName, properyName, eventName, matchType))
                {
                    return value;
                }

            }

#if WINDOWS_UWP
            current = (current as FrameworkElement) != null ? (current as FrameworkElement).Parent : null;
#else
            current = LogicalTreeHelper.GetParent(current);
#endif
        }
        return fisrtLocated;
    }

    /// <summary>
    /// Locate the first defined value of Dependency property by walking the Visual tree up to the roots.
    /// </summary>
    /// <param name="depObject">The DependencyObject object.</param>
    /// <param name="depProp">The dependency property to locate.</param>
    /// <param name="methodName">The method name in the dependency property object.
    /// The properties will be scan up over the visual tree until the  method name will be in math  to the method in the object 
    /// that represented by a property value.</param>
    /// <param name="properyName">The property name in the dependency property object.
    /// The properties will be scan up over the visual tree until the  property name will be in math  to the property in the object 
    /// that represented by a property value.</param>
    /// <param name="eventName">The event name in the dependency property object.
    /// The properties will be scan up over the visual tree until the  event name will be in math  to the event in the object 
    /// that represented by a property value.</param>
    /// <param name="matchType">The type which should match with a property or event or method name or by it self.</param>
    /// <returns>The value of the located dependency property or null.</returns>
    public static object LocateValidDependencyPropertyByVisualTree(DependencyObject depObject, DependencyProperty depProp,
        string methodName = null, string properyName = null, string eventName = null, Type matchType = null)
    {
        DependencyObject current = depObject;
        object fisrtLocated = null;
        while (current != null)
        {
            object value = current.GetValue(depProp);
            if (value != null)
            {
                if (fisrtLocated == null)
                {
                    fisrtLocated = value;
                }
                if (ValidateValue(value, methodName, properyName, eventName, matchType))
                {
                    return value;
                }
            }
            current = VisualTreeHelper.GetParent(current);
        }

        return fisrtLocated;
    }

    /// <summary>
    /// Locate the first defined value of Dependency property by walking the all trees up to the roots.
    /// </summary>
    /// <param name="depObject">The DependencyObject object.</param>
    /// <param name="depProp">The dependency property to locate.</param>
    /// <param name="methodName">The method name in the dependency property object.
    /// The properties will be scan up over the visual tree until the  method name will be in math  to the method in the object 
    /// that represented by a property value.</param>
    /// <param name="properyName">The property name in the dependency property object.
    /// The properties will be scan up over the visual tree until the  property name will be in math  to the property in the object 
    /// that represented by a property value.</param>
    /// <param name="eventName">The event name in the dependency property object.
    /// The properties will be scan up over the visual tree until the  event name will be in math  to the event in the object 
    /// that represented by a property value.</param>
    /// <param name="matchType">The type which should match with a property or event or method name or by it self.</param>
    /// <returns>The value of the located dependency property or null.</returns>
    public static object LocateValidDependencyPropertyByAllTrees(DependencyObject depObject, DependencyProperty depProp,
        string methodName = null, string properyName = null, string eventName = null, Type matchType = null)
    {
        DependencyObject current = depObject;
        object fisrtLocated = null;
        object value = null;
        while (current != null)
        {
            value = current.GetValue(depProp);
            if (value != null)
            {
                if (fisrtLocated == null)
                {
                    fisrtLocated = value;
                }
                if (ValidateValue(value, methodName, properyName, eventName, matchType))
                {
                    break;
                }
                value = null;
            }
            current = VisualTreeHelper.GetParent(current);
        }

        if (value != null)
        {
            return value;
        }

        current = depObject;
        while (current != null)
        {
            value = current.GetValue(depProp);
            if (value != null)
            {
                if (fisrtLocated == null)
                {
                    fisrtLocated = value;
                }

                if (ValidateValue(value, methodName, properyName, eventName, matchType))
                {
                    return value;
                }
            }

#if WINDOWS_UWP
            current = (current as FrameworkElement) != null ? (current as FrameworkElement).Parent : null;
#else
            current = LogicalTreeHelper.GetParent(current);
#endif

        }
        return fisrtLocated;
    }

    /// <summary>
    /// Validates an object if it is match to the type or/and members content.
    /// </summary>
    /// <param name="value">The object to validate.</param>
    /// <param name="methodName">The method name in the dependency property object.
    /// The properties will be scan up over the visual tree until the  method name will be in math  to the method in the object 
    /// that represented by a property value.</param>
    /// <param name="propertyName">The property name in the dependency property object.
    /// The properties will be scan up over the visual tree until the  property name will be in math  to the property in the object 
    /// that represented by a property value.</param>
    /// <param name="eventName">The event name in the dependency property object.
    /// The properties will be scan up over the visual tree until the  event name will be in math  to the event in the object 
    /// that represented by a property value.</param>
    /// <param name="matchType">The type which should match with a property or event or method name or by it self.</param>
    /// <returns>Returns true if it has a match.</returns>
    static public bool ValidateValue(object value, string methodName, string propertyName = null, string eventName = null, Type matchType = null)
    {
        if (string.IsNullOrEmpty(methodName) && string.IsNullOrEmpty(propertyName) && string.IsNullOrEmpty(eventName) && matchType == null)
        {
            return true;
        }

        if (string.IsNullOrEmpty(methodName) && string.IsNullOrEmpty(propertyName) && string.IsNullOrEmpty(eventName))
        {

#if WINDOWS_UWP
            return value.GetType() == matchType || value.GetType().GetTypeInfo().IsSubclassOf(matchType);
#else
            return value.GetType() == matchType || value.GetType().IsSubclassOf(matchType);
#endif
        }

        // Deep check the property object with the method name
        if (!string.IsNullOrEmpty(methodName))
        {
            MethodInfo methodInfo = value.GetMethodInfo(methodName);
            if (methodInfo == null)
            {
                List<Tuple<Type, object>> locatedViewModels = BindHelper.LocateAppendedViewModels(value);
                foreach (var model in locatedViewModels)
                {
                    methodInfo = model.Item2.GetMethodInfo(methodName);
                    if (methodInfo != null)
                    {
                        break;
                    }
                }
            }
            if (methodInfo != null)
            {
                if (matchType != null)
                {
#if WINDOWS_UWP
                    return value.GetType() == matchType || value.GetType().GetTypeInfo().IsSubclassOf(matchType);
#else
                    return value.GetType() == matchType || value.GetType().IsSubclassOf(matchType);
#endif
                }

                // Return only if it has a math in the object
                return true;
            }
        }
        // Deep check the property object with the property name
        if (!string.IsNullOrEmpty(propertyName))
        {

            PropertyInfo propertyInfo = value.GetPropertyInfo(propertyName);
            if (propertyInfo == null)
            {
                List<Tuple<Type, object>> locatedViewModels = BindHelper.LocateAppendedViewModels(value);
                foreach (var model in locatedViewModels)
                {
                    propertyInfo = model.Item2.GetPropertyInfo(propertyName);
                    if (propertyInfo != null)
                    {
                        break;
                    }
                }
            }
            if (propertyInfo != null)
            {
                if (matchType != null)
                {
#if WINDOWS_UWP
                    return value.GetType() == matchType || value.GetType().GetTypeInfo().IsSubclassOf(matchType);
#else
                    return value.GetType() == matchType || value.GetType().IsSubclassOf(matchType);
#endif
                }
                // Return only if it has a math in the object
                return true;
            }
        }
        // Deep check the property object with the event name
        if (!string.IsNullOrEmpty(eventName))
        {
            EventInfo eventInfo = value.GetEventInfo(eventName);
            if (eventInfo == null)
            {
                List<Tuple<Type, object>> locatedViewModels = BindHelper.LocateAppendedViewModels(value);
                foreach (var model in locatedViewModels)
                {
                    eventInfo = model.Item2.GetEventInfo(eventName);
                    if (eventInfo != null)
                    {
                        break;
                    }
                }
            }
            if (eventInfo != null)
            {
                if (matchType != null)
                {
#if WINDOWS_UWP
                    return value.GetType() == matchType || value.GetType().GetTypeInfo().IsSubclassOf(matchType);
#else
                    return value.GetType() == matchType || value.GetType().IsSubclassOf(matchType);
#endif
                }
                // Return only if it has a math in the object
                return true;
            }
        }

        return false;
    }


    /// <summary>
    /// Locate the first parent control that is a implementation or subclass of requested type by walking the Visual tree up to the root.
    /// </summary>
    /// <param name="depObject">The DependencyObject object.</param>
    /// <param name="typeToLocate">The type of the object to locate.</param>
    /// <returns>The value of located defined property or null.</returns>
    public static object LocateFirstParentTypeOfControlByLogicalTree(DependencyObject depObject, Type typeToLocate)
    {
        var current = depObject;
        while (current != null)
        {
#if WINDOWS_UWP
            if (current.GetType() == typeToLocate || current.GetType().GetTypeInfo().IsSubclassOf(typeToLocate))
#else
            if (current.GetType() == typeToLocate || current.GetType().IsSubclassOf(typeToLocate))
#endif
            {
                return current;
            }

#if WINDOWS_UWP
            current = (current as FrameworkElement) != null ? (current as FrameworkElement).Parent : null;
#else
            current = LogicalTreeHelper.GetParent(current);
#endif
        }
        return null;
    }

    /// <summary>
    /// Locate the first parent control that is a implementation or subclass of requested type by walking the Visual tree up to the root.
    /// </summary>
    /// <param name="depObject">The DependencyObject object.</param>
    /// <param name="typeToLocate">The type of the object to locate.</param>
    /// <returns>The value of located defined property or null.</returns>
    public static object LocateFirstParentTypeOfControlByVisualTree(DependencyObject depObject, Type typeToLocate)
    {
        DependencyObject current = depObject;
        while (current != null)
        {
#if WINDOWS_UWP
            if (current.GetType() == typeToLocate || current.GetType().GetTypeInfo().IsSubclassOf(typeToLocate))
#else
            if (current.GetType() == typeToLocate || current.GetType().IsSubclassOf(typeToLocate))
#endif
            {
                return current;
            }
            current = VisualTreeHelper.GetParent(current);
        }
        return null;
    }

    /// <summary>
    /// Locate the first parent control that is a implementation or subclass of requested type by walking the all trees up to the root.
    /// </summary>
    /// <param name="depObject">The dependency object.</param>
    /// <param name="typeToLocate">The type of the object to locate.</param>
    /// <returns>The value of located defined property or null.</returns>
    public static object LocateFirstParentTypeOfControlByAllTrees(DependencyObject depObject, Type typeToLocate)
    {
        object value = LocateFirstParentTypeOfControlByVisualTree(depObject, typeToLocate);
        if (value != null)
        {
            return value;
        }
        value = LocateFirstParentTypeOfControlByLogicalTree(depObject, typeToLocate);
        return value;
    }

    private static readonly Dictionary<string, Type> DefaultTypeAliases = new Dictionary<string, Type>
        {
            {"sbyte", typeof (sbyte)},
            {"short", typeof (short)},
            {"int", typeof (int)},
            {"integer", typeof (int)},
            {"long", typeof (long)},
            {"byte", typeof (byte)},
            {"ushort", typeof (ushort)},
            {"uint", typeof (uint)},
            {"ulong", typeof (ulong)},
            {"float", typeof (float)},
            {"single", typeof (float)},
            {"double", typeof (double)},
            {"decimal", typeof (decimal)},
            {"char", typeof (char)},
            {"bool", typeof (bool)},
            {"oolean", typeof (bool)},
            {"object", typeof (object)},
            {"string", typeof (string)},
            {"datetime", typeof (DateTime)},
            {"date", typeof (DateTime)}
        };



#if WINDOWS_UWP
    private static WeakReference _loadedTypesList;


    /// <summary>
    /// 
    /// </summary>
    /// <param name="desiredNameSpace"></param>
    /// <returns></returns>
    public static IEnumerable<Type> ResolveTypesByNameSpace(string desiredNameSpace)
    {
        IEnumerable<Type> types;
        if ((_loadedTypesList == null) || (!_loadedTypesList.IsAlive))
        {
            // loaded assemblies and parse types
            // Change context for this operation only on multithreading
            var oldSync = SynchronizationContext.Current;

            SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
            var assembliesTask = AsyncGetLoadedAssemblies();
            var assemblies = assembliesTask.Result;
            types = TypesFromAssemblies(assemblies);
            if (_loadedTypesList == null)
            {
                _loadedTypesList = new WeakReference(types);
            }
            else
            {
                _loadedTypesList.Target = types;
            }
            SynchronizationContext.SetSynchronizationContext(oldSync);
        }

        types = _loadedTypesList.Target as IEnumerable<Type>;
        if (types != null)
        {
            // find a type by full or name
            return types.Where(tp => (tp.Namespace == desiredNameSpace));
        }

        return Empty<Type>();
    }


    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <returns></returns>
    public static IEnumerable<TResult> Empty<TResult>()
    {
        return EmptyEnumerable<TResult>.Instance;
    }

    internal class EmptyEnumerable<TElement>
    {
        static volatile TElement[] _instance;

        public static IEnumerable<TElement> Instance
        {
            get
            {
                if (_instance == null) _instance = new TElement[0];
                return _instance;
            }
        }
    }


    /// <summary>
    /// WinStor Application, Resolves a Type by using the Type Name.
    /// </summary>
    /// <param name="typeName">Type name to find.</param>
    /// <returns>Resolved type.</returns>
    public static Type ResolveTypeByName(string typeName)
    {
        if (DefaultTypeAliases.ContainsKey(typeName.ToLower()))
        {
            return DefaultTypeAliases[typeName.ToLower()];
        }

        IEnumerable<Type> types;
        if ((_loadedTypesList == null) || (!_loadedTypesList.IsAlive))
        {
            // loaded assemblies and parse types
            // Change context for this operation only on multithreading
            var oldSync = SynchronizationContext.Current;

            SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
            var assembliesTask = AsyncGetLoadedAssemblies();
            var assemblies = assembliesTask.Result;
            types = TypesFromAssemblies(assemblies);
            if (_loadedTypesList == null)
            {
                _loadedTypesList = new WeakReference(types);
            }
            else
            {
                _loadedTypesList.Target = types;
            }
            SynchronizationContext.SetSynchronizationContext(oldSync);
        }

        types = _loadedTypesList.Target as IEnumerable<Type>;
        if (types != null)
        {
            // find a type by full or name
            return types.FirstOrDefault(tp => (tp.Name == typeName) || (tp.FullName == typeName));
        }
        return null;
    }


    /// <summary>
    ///  Get the list of loaded assemblies.
    /// </summary>
    /// <param name="skipOnError">When it is true, then skip errors.</param>
    /// <returns>The list of loaded assemblies.</returns>
    public static async Task<IEnumerable<Assembly>> AsyncGetLoadedAssemblies(bool skipOnError = true)
    {
        var folder = Windows.ApplicationModel.Package.Current.InstalledLocation;
        var assemblies = new List<Assembly>();

        foreach (var file in await folder.GetFilesAsync().AsTask().ConfigureAwait(false))
        {
            if (file.FileType == ".dll" || file.FileType == ".exe")
            {
                var name = new AssemblyName { Name = Path.GetFileNameWithoutExtension(file.Name) };
                Assembly assembly;
                try
                {
                    assembly = Assembly.Load(name);
                }
                catch (Exception e)
                {
                    if (!(skipOnError && (e is FileNotFoundException || e is BadImageFormatException)))
                    {
                        throw;
                    }
                    continue;
                }
                assemblies.Add(assembly);
            }
        }
        return assemblies;
    }

    /// <summary>
    /// Get all types from a list of assemblies.
    /// </summary>
    /// <param name="assemblies">The list of assemblies.</param>
    /// <param name="skipOnError">When it is true, then skip errors.</param>
    /// <returns></returns>
    public static IEnumerable<Type> TypesFromAssemblies(IEnumerable<Assembly> assemblies, bool skipOnError = true)
    {
        return assemblies.SelectMany(a =>
                {
                    IEnumerable<TypeInfo> types;
                    try
                    {
                        types = a.DefinedTypes;
                    }
                    catch (ReflectionTypeLoadException e)
                    {
                        if (!skipOnError)
                        {
                            throw;
                        }

                        types = e.Types.TakeWhile(t => t != null).Select(t => t.GetTypeInfo());
                    }
                    return types.Where(ti => ti.IsClass & !ti.IsAbstract && !ti.IsValueType/* && ti.IsVisible*/).Select(ti => ti.AsType());
                });
    }


#endif

#if !WINDOWS_UWP

    /// <summary>
    /// Windows Application, Resolves a Type by using the Type Name.
    /// </summary>
    /// <param name="typeName">Type name to find.</param>
    /// <param name="includeInterfaces">When it is true, interfaces will included in the result list.</param>
    /// <returns>Resolved type.</returns>
    public static Type ResolveTypeByName(string typeName, bool includeInterfaces = true)
    {
        if (DefaultTypeAliases.ContainsKey(typeName.ToLower()))
        {
            return DefaultTypeAliases[typeName.ToLower()];
        }

        IEnumerable<Type> types;
        var assemblies = GetLoadedAssemblies();
        types = TypesFromAssemblies(assemblies, includeInterfaces);

        if (types != null)
        {
            // find a type by full or name
            return types.FirstOrDefault(tp => (tp.Name == typeName) || (tp.FullName == typeName));
        }
        return null;
    }

    /// <summary>
    /// Windows Application, Resolves a Type by using the Type Name.
    /// </summary>
    /// <param name="nameSpace">Type name to find.</param>
    /// <param name="includeInterfaces">When it is true, interfaces will included in the result list.</param>
    /// <returns>Resolved type.</returns>
    public static IEnumerable<Type> ResolveTypesByNameSpace(string nameSpace, bool includeInterfaces = true)
    {
        IEnumerable<Type> types;
        var assemblies = GetLoadedAssemblies();
        types = TypesFromAssemblies(assemblies, includeInterfaces);
        if (types != null)
        {
            // find  types by a namespace
            return types.Where(tp => tp.Namespace == nameSpace);
        }

        return Enumerable.Empty<Type>();
    }

    /// <summary>
    ///  Get the list of loaded assemblies.
    /// </summary>
    /// <returns>The list of loaded assemblies.</returns>
    public static IEnumerable<Assembly> GetLoadedAssemblies(bool includeDynamicAssemblies = true)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        if (includeDynamicAssemblies)
        {
            return assemblies;
        }
        return assemblies.Where(a => (!a.IsDynamic));
    }

    /// <summary>
    /// Get all types from a list of assemblies.
    /// </summary>
    /// <param name="assemblies">The list of assemblies.</param>
    /// <param name="includeInterfaces">When it is true, interfaces will included in the result list.</param>
    /// <param name="skipOnError">When it is true, then skip errors.</param>
    /// <returns>The list discovered types from the list of assemblies.</returns>
    public static IEnumerable<Type> TypesFromAssemblies(IEnumerable<Assembly> assemblies, bool includeInterfaces = true, bool skipOnError = true)
    {
        return assemblies.SelectMany(a =>
        {
            IEnumerable<Type> types;
            try
            {
                types = a.GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                if (!skipOnError)
                {
                    throw;
                }

                types = e.Types.TakeWhile(t => t != null);
            }
            if (includeInterfaces)
            {
                return types.Where(ti => (ti.IsClass || ti.IsInterface) && !ti.IsValueType /*&& ti.IsVisible*/);
            }
            return types.Where(ti => ti.IsClass && !ti.IsValueType /*&& ti.IsVisible*/);

        });
    }
#endif

    internal static object ProvideValueIoc(Type serviceType, string serviceKey)
    {
        return ObtainIocValue(serviceType, serviceKey);
    }

    /// <summary>
    ///  Extension method that helps to resolve a service reference from the DI container.
    /// </summary>
    /// <param name="serviceType">Service type.</param>
    /// <param name="serviceKey">Service key.</param>
    /// <returns>The requested service instance or has an exception.</returns>
    static public object ObtainIocValue(Type serviceType, String serviceKey = null)
    {

        if (serviceType == null)
        {
            // ReSharper disable NotResolvedInText
            throw new ArgumentNullException("ObtainIocValue - ServiceType cannot be null.");
            // ReSharper restore NotResolvedInText
        }

        object obj = null;
        if (AutoWireVmDataContext.ServiceProvider != null)
        {

            try { obj = AutoWireVmDataContext.ServiceProvider.GetService(serviceType); } catch { }
            if (obj == null)
            {
                try { obj = Activator.CreateInstance(serviceType); } catch { }
            }
        }

        return obj;
    }
    static readonly char[] InvalidCharacters = {/* '.', '_',',', */'@', '#', '$', '%', '^', '&', '*', '(', ')', '+', '=', '-', '[', ']', '\'', ';', ',', '/', '{', '}', '|', ':', '<', '>', '?', '~', ';' };
    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static bool ValidateElementName(string name)
    {
        if (!string.IsNullOrEmpty(name))
        {
            return name.IndexOfAny(InvalidCharacters) == -1;
        }
        return false;
    }

    /// <summary>
    /// Creates the delegate with requested type from a method.
    /// </summary>
    /// <param name="methodName">The method name that can be used as a delegate.</param>
    /// <param name="sourceObject">The source object that contains a method.</param>
    /// <param name="desiredDelegateType">The delegate type.</param>
    /// <returns></returns>
    public static Object EventHandlerDelegateFromMethod(string methodName, object sourceObject, Type desiredDelegateType)
    {
        MethodInfo methodInfo = sourceObject.GetMethodInfo(methodName);
        if (methodInfo == null)
        {
            List<Tuple<Type, object>> locatedViewModels = BindHelper.LocateAppendedViewModels(sourceObject);
            foreach (var model in locatedViewModels)
            {
                methodInfo = model.Item2.GetMethodInfo(methodName);
                if (methodInfo != null)
                {
                    sourceObject = model.Item2;
                    break;
                }
            }
        }
        if (methodInfo == null)
        {
            throw new ArgumentException("DelegateFromMethod - cannot resolve method  " + methodName);
        }
        return EventHandlerDelegateFromMethodInfo(sourceObject, desiredDelegateType, methodInfo);
    }

    /// <summary>
    /// Creates the delegate with requested type from a method info.
    /// </summary>
    /// <param name="info">The method info that can be used as a delegate.</param>
    /// <param name="sourceObject">The source object that contains a method.</param>
    /// <param name="desiredDelegateType">The delegate type.</param>
    /// <returns></returns>
    public static object EventHandlerDelegateFromMethodInfo(object sourceObject, Type desiredDelegateType, MethodInfo info)
    {
        if (info.IsGenericMethod)
        {
            // Substitute type into generic method
            var delInvoke = desiredDelegateType.GetMethodInfo("Invoke");
            var parms = delInvoke.GetParameters();
            info = info.MakeGenericMethod(parms[1].ParameterType);
        }

        object handler;
        if (info.IsStatic)
        {
#if WINDOWS_UWP
            handler = info.CreateDelegate(desiredDelegateType);
#else
            handler = Delegate.CreateDelegate(desiredDelegateType, info);
#endif
        }
        else
        {
#if WINDOWS_UWP
            handler = info.CreateDelegate(desiredDelegateType, sourceObject);
#else
            handler = Delegate.CreateDelegate(desiredDelegateType, sourceObject, info);
#endif
        }
        return handler;
    }

    /// <summary>
    /// Obtains the property value that expected to be a delegate.
    /// </summary>
    /// <param name="propertyName">The property name.</param>
    /// <param name="sourceObject">The source object that contains a method</param>
    /// <param name="desiredDelegateType"></param>
    /// <returns></returns>
    public static Object EventHandlerDelegateFromProperty(string propertyName, object sourceObject, Type desiredDelegateType)
    {
        var infoProp = sourceObject.GetPropertyInfo(propertyName);
        if (infoProp == null)
        {
            List<Tuple<Type, object>> locatedViewModels = LocateAppendedViewModels(sourceObject);
            foreach (var model in locatedViewModels)
            {
                infoProp = model.Item2.GetPropertyInfo(propertyName);
                if (infoProp != null)
                {
                    sourceObject = model.Item2;
                    break;
                }
            }
        }
        if (infoProp == null)
        {
            throw new ArgumentException("DelegateFromProperty - cannot resolve property  " + propertyName);
        }
        var obj = infoProp.GetValue(sourceObject, null);
        if (obj is MethodInfo)
        {
            // create the proper delegate
            var info = obj as MethodInfo;
            if (info.IsStatic)
            {
#if WINDOWS_UWP
                obj = info.CreateDelegate(desiredDelegateType);
#else
                obj = Delegate.CreateDelegate(desiredDelegateType, info);
#endif
            }
            else
            {
#if WINDOWS_UWP
                obj = info.CreateDelegate(desiredDelegateType, sourceObject);
#else
                obj = Delegate.CreateDelegate(desiredDelegateType, sourceObject, info);
#endif
            }
        }
        if (obj is Delegate)
        {
#if WINDOWS_UWP
            MethodInfo delMtInfo = ((Delegate)obj).GetMethodInfo();
#else
            MethodInfo delMtInfo = ((Delegate)obj).Method;
#endif
            if (delMtInfo.IsStatic)
            {
#if WINDOWS_UWP
                obj = delMtInfo.CreateDelegate(desiredDelegateType);
#else
                obj = Delegate.CreateDelegate(desiredDelegateType, delMtInfo);
#endif
            }
            else
            {
#if WINDOWS_UWP
                obj = delMtInfo.CreateDelegate(desiredDelegateType, ((Delegate)obj).Target);
#else
                obj = Delegate.CreateDelegate(desiredDelegateType, ((Delegate)obj).Target, delMtInfo);
#endif
            }

        }
        return obj;
    }

    /// <summary>
    /// Constructs the proxy class implementation of <see cref="ICommand"/> interface.
    /// </summary>
    /// <param name="executeMethod">The method name of a source instance that performs as <see cref="ICommand.Execute"/>.</param>
    /// <param name="sourceBase">The instance of the source object.</param>
    /// <returns>The proxy class that provides the implementation of <see cref="ICommand"/> interface for case of command binding.</returns>
    public static Action<object> CommandExecuteDelegate(string executeMethod, object sourceBase)
    {
        Action<object> executeDelegate;
        MethodInfo executeMethodInfo = sourceBase.GetMethodInfo(executeMethod);

        if (executeMethodInfo == null)
        {
            throw new ArgumentException("GetExecuteDelegate - cannot resolve method  " + executeMethod);
        }

        if (executeMethodInfo.IsStatic)
        {
#if WINDOWS_UWP
            executeDelegate = (Action<object>)executeMethodInfo.CreateDelegate(typeof(Action<object>));
#else
            executeDelegate = (Action<object>)Delegate.CreateDelegate(typeof(Action<object>), executeMethodInfo);
#endif
        }
        else
        {
#if WINDOWS_UWP
            executeDelegate = (Action<object>)executeMethodInfo.CreateDelegate(typeof(Action<object>), sourceBase);
#else
            executeDelegate = (Action<object>)Delegate.CreateDelegate(typeof(Action<object>), sourceBase, executeMethodInfo);
#endif
        }
        return executeDelegate;
    }

    /// <summary>
    /// Constructs the proxy class implementation of <see cref="ICommand"/> interface.
    /// </summary>
    /// <param name="canExecuteMethod">The method name of a source instance that performs as <see cref="ICommand.CanExecute"/>.</param>
    /// <param name="sourceBase">The instance of the source object.</param>
    /// <returns>The proxy class that provides the implementation of <see cref="ICommand"/> interface for case of command binding.</returns>
    public static Func<object, bool> CommandCanExecuteDelegate(string canExecuteMethod, object sourceBase)
    {
        Func<object, bool> canExecuteDelegate;
        MethodInfo executeMethodInfo = sourceBase.GetMethodInfo(canExecuteMethod);

        if (executeMethodInfo == null)
        {
            throw new ArgumentException("GetCanExecuteDelegate - cannot resolve method  " + canExecuteMethod);
        }

        if (executeMethodInfo.IsStatic)
        {
#if WINDOWS_UWP
            canExecuteDelegate = (Func<object, bool>)executeMethodInfo.CreateDelegate(typeof(Func<object, bool>));
#else
            canExecuteDelegate = (Func<object, bool>)Delegate.CreateDelegate(typeof(Func<object, bool>), executeMethodInfo);
#endif
        }
        else
        {
#if WINDOWS_UWP
            canExecuteDelegate = (Func<object, bool>)executeMethodInfo.CreateDelegate(typeof(Func<object, bool>), sourceBase);
#else
            canExecuteDelegate = (Func<object, bool>)Delegate.CreateDelegate(typeof(Func<object, bool>), sourceBase, executeMethodInfo);
#endif
        }
        return canExecuteDelegate;
    }

}



