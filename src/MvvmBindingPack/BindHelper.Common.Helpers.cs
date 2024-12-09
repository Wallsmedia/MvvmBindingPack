// 
//  Control Adorners and other extensions.
//  Copyright © 2013-2021 Alexander Paskhin /paskhin@hotmail.co.uk/ All rights reserved.
// 

using System;
using System.Windows;
using System.Windows.Input;
using System.Collections.Generic;
using System.ComponentModel;

#if WINDOWS_UWP

using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using System.Reflection;

#else
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
#if BEHAVIOR
using System.Windows.Interactivity;
#endif
using System.Windows.Markup.Primitives;
using System.Reflection;

#endif


namespace MvvmBindingPack;

/// <summary>
/// 
/// </summary>
public class XClassTypeNameElement
{
    /// <summary>
    /// Dependency object "x:Name"; it used for wiring to View Model.
    /// It will not match to type name.
    /// </summary>
    public string XNameForXClass;
    /// <summary>
    /// The type that owners this dependency object.
    /// </summary>
    public Type XClassType;
    /// <summary>
    /// Dependency object 
    /// </summary>
    public DependencyObject XClassDependencyObject;
    /// <summary>
    /// The full type Name
    /// </summary>
    public string FullxClassTypeName { get { return XClassType.FullName; } }
    /// <summary>
    /// The type namespace
    /// </summary>
    public string XClassTypeNamespace { get { return XClassType.Namespace; } }
    /// <summary>
    /// The type name
    /// </summary>
    public string XClassTypeName { get { return XClassType.Name; } }
}

/// <summary>
/// Partial class contains a collection of helpers. 
/// </summary>
public static partial class BindHelper
{
    /// <summary>
    /// Returns true if the DI container has been setup.
    /// </summary>
    static public bool IsIocContainerActive
    {
        get
        {
            return AutoWireVmDataContext.ServiceProvider != null;
        }
    }

    // Common language runtime

    /// <summary>
    /// Gets all CLR events.
    /// </summary>
    /// <param name="obj">Object instance.</param>
    /// <returns>Returns System.Collections.Generic.Dictionary(string, PropertyInfo) object.</returns>
    public static Dictionary<string, EventInfo> GetClrEvents(object obj)
    {
        Dictionary<string, EventInfo> result = new Dictionary<string, EventInfo>();
        if (obj != null)
        {
#if WINDOWS_UWP
            IEnumerable<EventInfo> info;
#else
            EventInfo[] info;
#endif
            Type type = obj.GetType();
            var lookUpType = type;
            while (lookUpType != null)
            {
#if WINDOWS_UWP
                info = lookUpType.GetTypeInfo().DeclaredEvents;
#else
                info = lookUpType.GetEvents(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
#endif
                foreach (var item in info)
                {
                    string name = lookUpType.Name + "." + item.Name;
                    result[item.Name] = item;
                    result[name] = item;
                }
#if WINDOWS_UWP
                lookUpType = lookUpType.GetTypeInfo().BaseType;
#else
                lookUpType = lookUpType.BaseType;
#endif
            }
        }
        return result;
    }

    /// <summary>
    /// Locates a CLR event.
    /// </summary>
    /// <param name="eventName">Property name may be in format OwnerType.PropertyName' .</param>
    /// <param name="obj">Object instance.</param>
    /// <returns> Returns  object or null.</returns>
    public static EventInfo LocateClrEvent(string eventName, object obj)
    {
        if (obj != null)
        {
#if WINDOWS_UWP
            IEnumerable<EventInfo> info;
#else
            EventInfo[] info;
#endif
            Type type = obj.GetType();
            var lookUpType = type;
            while (lookUpType != null)
            {
#if WINDOWS_UWP
                info = lookUpType.GetTypeInfo().DeclaredEvents;
#else
                info = lookUpType.GetEvents(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
#endif
                foreach (var item in info)
                {
                    if ((item.Name == eventName) || (lookUpType.Name + "." + item.Name == eventName))
                    {
                        return item;
                    }
                }
#if WINDOWS_UWP
                lookUpType = lookUpType.GetTypeInfo().BaseType;
#else
                lookUpType = lookUpType.BaseType;
#endif
            }
        }
        return null;

    }

    /// <summary>
    /// Gets all CLR properties.
    /// </summary>
    /// <param name="obj">Object instance.</param>
    /// <returns>Returns System.Collections.Generic.Dictionary(string, PropertyInfo) object.</returns>
    public static Dictionary<string, PropertyInfo> GetClrProperties(object obj)
    {
        Dictionary<string, PropertyInfo> result = new Dictionary<string, PropertyInfo>();
        if (obj != null)
        {
#if WINDOWS_UWP
            IEnumerable<PropertyInfo> info;
#else
            PropertyInfo[] info;
#endif
            Type type = obj.GetType();
            var lookUpType = type;
            while (lookUpType != null)
            {
#if WINDOWS_UWP
                info = lookUpType.GetTypeInfo().DeclaredProperties;
#else
                info = lookUpType.GetProperties(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
#endif
                foreach (var item in info)
                {
                    string name = lookUpType.Name + "." + item.Name;
                    result[item.Name] = item;
                    result[name] = item;
                }
#if WINDOWS_UWP
                lookUpType = lookUpType.GetTypeInfo().BaseType;
#else
                lookUpType = lookUpType.BaseType;
#endif
            }
        }
        return result;
    }

    /// <summary>
    /// Locates a CLR property.
    /// </summary>
    /// <param name="propertyName">Property name may be in format OwnerType.PropertyName' .</param>
    /// <param name="obj">Object instance.</param>
    /// <returns> Returns  object or null.</returns>
    public static PropertyInfo LocateClrProperty(string propertyName, object obj)
    {
        if (obj != null)
        {
#if WINDOWS_UWP
            IEnumerable<PropertyInfo> info;
#else
            PropertyInfo[] info;
#endif
            Type type = obj.GetType();
            var lookUpType = type;
            while (lookUpType != null)
            {
#if WINDOWS_UWP
                info = lookUpType.GetTypeInfo().DeclaredProperties;
#else
                info = lookUpType.GetProperties(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
#endif
                foreach (var item in info)
                {
                    if ((item.Name == propertyName) || (lookUpType.Name + "." + item.Name == propertyName))
                    {
                        return item;
                    }
                }
#if WINDOWS_UWP
                lookUpType = lookUpType.GetTypeInfo().BaseType;
#else
                lookUpType = lookUpType.BaseType;
#endif
            }
        }
        return null;

    }

    // Work flow presentation foundation 

    /// <summary>
    /// Gets all dependency properties that have set values from any kind of source.
    /// </summary>
    /// <param name="obj">Object instance.</param>
    /// <returns>Returns System.Collections.Generic.Dictionary(string, DependencyProperty) object.</returns>
    public static Dictionary<string, RoutedEvent> GetDefinedInTypeRoutedEvents(DependencyObject obj)
    {
        Dictionary<string, RoutedEvent> result = new Dictionary<string, RoutedEvent>();
        if (obj == null)
        {
            return result;
        }
        Type type = obj.GetType();
#if WINDOWS_UWP
        // Wins store apps has a props but not fields like WPF
        IEnumerable<PropertyInfo> info;
        var lookUpType = type;
        while (lookUpType != null)
        {
            info = lookUpType.GetTypeInfo().DeclaredProperties;
            if (info != null)
            {
                foreach (var item in info)
                {
                    if (item.PropertyType == typeof(RoutedEvent))
                    {
                        var value = item.GetValue(obj);
                        if (value != null)
                        {
                            string name = lookUpType.Name + "." + item.Name.Replace("Event", "");
                            result.Add(name, (RoutedEvent)value);
                        }
                    }
                }
            }
            lookUpType = lookUpType.GetTypeInfo().BaseType;
        }
#else
        // Static fields for WPF
        FieldInfo[] info;
        var lookUpType = type;
        while (lookUpType != null)
        {
            info = lookUpType.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (var item in info)
            {
                if (item.FieldType == typeof(RoutedEvent))
                {
                    var value = item.GetValue(obj);
                    if (value != null)
                    {
                        RoutedEvent prop = (RoutedEvent)value;
                        string name = prop.OwnerType + "." + item.Name.Replace("Event", "");
                        result.Add(name, prop);
                    }
                }
            }
            lookUpType = lookUpType.BaseType;
        }
#endif
        return result;

    }

    /// <summary>
    /// Locates a dependency property defined in a type and including attached property.
    /// </summary>
    /// <param name="eventName">Property name may be in format OwnerType.PropertyName' .</param>
    /// <param name="obj">Object instance.</param>
    /// <returns> Returns System.Windows.DependencyProperty object or null.</returns>
    public static RoutedEvent LocateRoutedEvent(string eventName, DependencyObject obj)
    {
        if (obj != null)
        {
#if WINDOWS_UWP
            var Info = obj.GetPropertyInfo(eventName + "Event");
            if (Info != null)
            {
                if (Info.PropertyType == typeof(RoutedEvent))
                {
                    return (RoutedEvent)Info.GetValue(null);
                }
            }
#else
            var info = obj.GetFieldInfo(eventName + "Event");
            if (info != null)
            {
                if (info.FieldType == typeof(RoutedEvent) && info.IsStatic)
                {
                    return (RoutedEvent)info.GetValue(null);
                }
            }
#endif
            //
            var splitName = eventName.Split('.');
            if (splitName.Length > 1)
            {
                return LocateRoutedEventFromTypeHive(splitName[1], splitName[0]);
            }
        }
        return null;
    }

    /// <summary>
    /// Gets all dependency properties that have set values from any kind of source.
    /// </summary>
    /// <param name="obj">Object instance.</param>
    /// <returns>Returns System.Collections.Generic.Dictionary(string, DependencyProperty) object.</returns>
    public static Dictionary<string, DependencyProperty> GetDefinedInTypeDependencyProperties(DependencyObject obj)
    {
        Dictionary<string, DependencyProperty> result = new Dictionary<string, DependencyProperty>();
        if (obj == null)
        {
            return result;
        }
        Type type = obj.GetType();
#if WINDOWS_UWP
        // Wins store apps has a props but not fields like WPF
        IEnumerable<PropertyInfo> info;
        var lookUpType = type;
        while (lookUpType != null)
        {
            info = lookUpType.GetTypeInfo().DeclaredProperties;
            if (info != null)
            {
                foreach (var item in info)
                {
                    if (item.PropertyType == typeof(DependencyProperty))
                    {
                        var value = item.GetValue(obj);
                        if (value != null)
                        {
                            string name = lookUpType.Name + "." + item.Name.Replace("Property", "");
                            result.Add(name, (DependencyProperty)value);
                        }
                    }
                }
            }
            lookUpType = lookUpType.GetTypeInfo().BaseType;
        }
#else
        // Static fields for WPF
        var lookUpType = type;
        while (lookUpType != null)
        {
            var info = lookUpType.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (var item in info)
            {
                if (item.FieldType == typeof(DependencyProperty))
                {
                    var value = item.GetValue(obj);
                    if (value != null)
                    {
                        DependencyProperty prop = (DependencyProperty)value;
                        string name = prop.OwnerType + "." + item.Name.Replace("Property", "");
                        result.Add(name, prop);
                    }
                }
            }
            lookUpType = lookUpType.BaseType;
        }
#endif
        return result;
    }

    /// <summary>
    /// Locates a dependency property defined in a type and including attached property.
    /// </summary>
    /// <param name="propertyName">Property name may be in format OwnerType.PropertyName' .</param>
    /// <param name="obj">Object instance.</param>
    /// <returns> Returns System.Windows.DependencyProperty object or null.</returns>
    public static DependencyProperty LocateDependencyProperty(string propertyName, DependencyObject obj)
    {
        if (obj != null)
        {
#if WINDOWS_UWP
            var Info = obj.GetPropertyInfo(propertyName + "Property");
            if (Info != null)
            {
                if (Info.PropertyType == typeof(DependencyProperty))
                {
                    return (DependencyProperty)Info.GetValue(null);
                }
            }
#else
            var info = obj.GetFieldInfo(propertyName + "Property");
            if (info != null)
            {
                if (info.FieldType == typeof(DependencyProperty) && info.IsStatic)
                {
                    return (DependencyProperty)info.GetValue(null);
                }
            }
#endif
            //
            var splitName = propertyName.Split('.');
            if (splitName.Length > 1)
            {
                return LocateDependencyPropertyFromTypeHive(splitName[1], splitName[0]);
            }
        }
        return null;
    }

    /// <summary>
    /// Locates a dependency by looking of the proper name of static filed in located type in loaded assemblies.
    /// </summary>
    /// <param name="propertyName">Property name.</param>
    /// <param name="typeName"></param>
    /// <returns> Returns System.Windows.DependencyProperty object or null.</returns>
    public static DependencyProperty LocateDependencyPropertyFromTypeHive(string propertyName, string typeName)
    {
        /* locate the type from current assemblies */
        var located = ResolveTypeByName(typeName);
        if (located != null)
        {
#if WINDOWS_UWP
            // Win store apps use properties but not static fields
            var Info = located.GetPropertyInfo(propertyName + "Property");
            if (Info != null)
            {

                if (Info.PropertyType == typeof(DependencyProperty))
                {
                    return (DependencyProperty)Info.GetValue(null);
                }
            }
#else
            var info = located.GetFieldInfo(propertyName + "Property");
            if (info != null)
            {

                if (info.FieldType == typeof(DependencyProperty) && info.IsStatic)
                {
                    return (DependencyProperty)info.GetValue(null);
                }
            }
#endif
        }
        return null;
    }


#if !WINDOWS_UWP
    /// <summary>
    /// Locates a dependency property defined in a type and including attached property.
    /// </summary>
    /// <param name="propertyName">Property name may be in format OwnerType.PropertyName' .</param>
    /// <param name="obj">Object instance.</param>
    /// <returns> Returns System.Windows.DependencyProperty object or null.</returns>
    public static DependencyPropertyKey LocateDependencyPropertyKey(string propertyName, DependencyObject obj)
    {
        if (obj != null)
        {
#if WINDOWS_UWP
            var Info = obj.PrInfo(propertyName + "PropertyKey");
            if (Info != null)
            {
                if (Info.PropertyType == typeof(DependencyProperty))
                {
                    return (DependencyProperty)Info.GetValue(null);
                }
            }
#else
            var info = obj.GetFieldInfo(propertyName + "PropertyKey");
            if (info != null)
            {
                if (info.FieldType == typeof(DependencyProperty) && info.IsStatic)
                {
                    return (DependencyPropertyKey)info.GetValue(null);
                }
            }
#endif
            //
            var splitName = propertyName.Split('.');
            if (splitName.Length > 1)
            {
                return LocateDependencyPropertyKeyFromTypeHive(splitName[1], splitName[0]);
            }
        }
        return null;
    }

    /// <summary>
    /// Locates a dependency by looking of the proper name of static filed in located type in loaded assemblies.
    /// </summary>
    /// <param name="propertyName">Property name.</param>
    /// <param name="typeName"></param>
    /// <returns> Returns System.Windows.DependencyProperty object or null.</returns>
    public static DependencyPropertyKey LocateDependencyPropertyKeyFromTypeHive(string propertyName, string typeName)
    {
        /* locate the type from current assemblies */
        var located = ResolveTypeByName(typeName);
        if (located != null)
        {
#if WINDOWS_UWP
            // Win store apps use properties but not static fields
            var Info = located.PrInfo(propertyName + "PropertyKey");
            if (Info != null)
            {

                if (Info.PropertyType == typeof(DependencyPropertyKey))
                {
                    return (DependencyPropertyKey)Info.GetValue(null);
                }
            }
#else
            var info = located.GetFieldInfo(propertyName + "PropertyKey");
            if (info != null)
            {

                if (info.FieldType == typeof(DependencyPropertyKey) && info.IsStatic)
                {
                    return (DependencyPropertyKey)info.GetValue(null);
                }
            }
#endif
        }
        return null;
    }
#endif

    /// <summary>
    /// Locates a routed event by looking of the proper name of static filed in located type in loaded assemblies.
    /// </summary>
    /// <param name="eventName">Event name.</param>
    /// <param name="typeName"></param>
    /// <returns> Returns System.Windows.DependencyProperty object or null.</returns>
    public static RoutedEvent LocateRoutedEventFromTypeHive(string eventName, string typeName)
    {
        /* locate the type from current assemblies */
        var located = ResolveTypeByName(typeName);
        if (located != null)
        {
#if WINDOWS_UWP
            // Win store apps use properties but not static fields
            var Info = located.GetPropertyInfo(eventName + "Event");
            if (Info != null)
            {

                if (Info.PropertyType == typeof(RoutedEvent))
                {
                    return (RoutedEvent)Info.GetValue(null);
                }
            }
#else
            var info = located.GetFieldInfo(eventName + "Event");
            if (info != null)
            {

                if (info.FieldType == typeof(RoutedEvent) && info.IsStatic)
                {
                    return (RoutedEvent)info.GetValue(null);
                }
            }
#endif
        }
        return null;
    }

    /// <summary>
    /// Locates a visual child element by dedicated type;
    /// </summary>
    /// <typeparam name="T">Type to locate.</typeparam>
    /// <param name="obj">Object which visual children will examined.</param>
    /// <returns></returns>
    public static T FindVisualChildByType<T>(DependencyObject obj) where T : DependencyObject
    {
        for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
        {
            var child = VisualTreeHelper.GetChild(obj, i);
            if (child != null && child is T)
            {
                return (T)child;
            }
            else
            {
                T descendant = FindVisualChildByType<T>(child);
                if (descendant != null)
                {
                    return descendant;
                }
            }
        }
        return null;
    }

    /// <summary>
    /// Splits the name into  parts by a capital case and '_'.
    /// The character '_' doesn't include into the name part.
    /// </summary>
    /// <param name="pascalCasedName"></param>
    /// <returns>The list of parts.</returns>
    public static List<string> SplitNameByCase(string pascalCasedName)
    {
        List<string> result = new List<string>();
        if (string.IsNullOrEmpty(pascalCasedName))
        {
            return result;
        }

        int startIndex = 0;
        int length;
        for (int i = 0; i < pascalCasedName.Length; i++)
        {
            if (((pascalCasedName[i] >= 'A') && (pascalCasedName[i] <= 'Z')) || pascalCasedName[i] == '_')
            {
                length = i - startIndex;
                if (length != 0)
                {
                    result.Add(pascalCasedName.Substring(startIndex, length));
                }
                if (pascalCasedName[i] == '_')
                {
                    startIndex = i + 1;
                }
                else
                {
                    startIndex = i;
                }
            }
        }

        if (startIndex < pascalCasedName.Length)
        {
            length = pascalCasedName.Length - startIndex;
            if (length != 0)
            {
                result.Add(pascalCasedName.Substring(startIndex, pascalCasedName.Length - startIndex));
            }
        }
        return result;
    }

    private static List<string> _knownPrefixExclude = new List<string> { "System", "Windows", "Microsoft" };
    /// <summary>
    /// Known system types prefixes.
    /// </summary>
    public static List<string> KnownPrefixExclude
    {
        get { return _knownPrefixExclude; }
    }


    /// <summary>
    /// Finds the list of non system types by walking the logical tree up to the roots.
    /// </summary>
    /// <param name="obj">Start dependency object in the logical tree.</param>
    /// <param name="useTheFirstOne">If it is true, the list is limited by the first found type.</param>
    /// <returns></returns>
    public static List<XClassTypeNameElement> FindNonSystemParentClassNames(DependencyObject obj, bool useTheFirstOne)
    {
        List<XClassTypeNameElement> result = new List<XClassTypeNameElement>();
        var current = obj;
        string xName;
        while (current != null)
        {
            string fullName = current.GetType().FullName;
            bool check = CheckKnownTypePrefix(fullName);
            if (!check)
            {
                xName = current.GetValue(FrameworkElement.NameProperty) as string;
                var res = new XClassTypeNameElement() { XClassType = current.GetType(), XClassDependencyObject = current, XNameForXClass = xName };
                result.Add(res);
                if (useTheFirstOne)
                {
                    // get the first found to use only
                    break;
                }
            }
#if WINDOWS_UWP
            current = (current as FrameworkElement) != null ? (current as FrameworkElement).Parent : null;
#else
            current = LogicalTreeHelper.GetParent(current);
#endif
        }

        return result;
    }

    /// <summary>
    /// Checks if the method names starts from the specifics prefixes.
    /// </summary>
    /// <param name="fullName">The method name.</param>
    /// <returns>True if it starts from the specific prefix.</returns>
    public static bool CheckKnownTypePrefix(string fullName)
    {
        bool check = false;
        foreach (var str in _knownPrefixExclude)
        {
            if (fullName.StartsWith(str))
            {
                check = true;
                break;
            }
        }
        return check;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="splitCaseNameCandidate"></param>
    /// <param name="splitCaselName"></param>
    /// <returns></returns>
    public static int CalculateMatchingRank(List<string> splitCaseNameCandidate, List<string> splitCaselName)
    {
        int rank = 0;
        for (int i = 0; i < splitCaseNameCandidate.Count && i < splitCaselName.Count; i++)
        {
            if (String.Compare(splitCaseNameCandidate[i], splitCaselName[i], StringComparison.OrdinalIgnoreCase) == 0)
            {
                rank++;
            }
            else
            {
                break;
            }
        }
        return rank;
    }

#if !WINDOWS_UWP
    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static Dictionary<string, DependencyProperty> WpfGetTypeDescriptorDependencyProperties(DependencyObject obj)
    {

        Dictionary<string, DependencyProperty> result = new Dictionary<string, DependencyProperty>();
        if (obj != null)
        {
            var properties = TypeDescriptor.GetProperties(obj, new Attribute[] { new PropertyFilterAttribute(PropertyFilterOptions.All) });
            foreach (PropertyDescriptor pd in properties)
            {
                DependencyPropertyDescriptor dpd = DependencyPropertyDescriptor.FromProperty(pd);
                if ((dpd != null) && (dpd.DependencyProperty != null))
                {
                    DependencyProperty depProp = dpd.DependencyProperty;
                    string name = depProp.OwnerType.Name + "." + depProp.Name;
                    result[depProp.Name] = depProp;
                    result[name] = depProp;
                }
            }
        }
        return result;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="propertyName">Property name may be in format OwnerType.PropertyName' .</param>
    /// <param name="obj">Object instance.</param>
    /// <returns> Returns  object or null.</returns>
    public static DependencyProperty WpfLocateTypeDescriptorDependencyProperty(string propertyName, DependencyObject obj)
    {
        if (obj != null)
        {
            var properties = TypeDescriptor.GetProperties(obj, new Attribute[] { new PropertyFilterAttribute(PropertyFilterOptions.All) });
            foreach (PropertyDescriptor pd in properties)
            {
                DependencyPropertyDescriptor dpd = DependencyPropertyDescriptor.FromProperty(pd);
                if ((dpd != null) && (dpd.DependencyProperty != null))
                {
                    DependencyProperty depProp = dpd.DependencyProperty;
                    var nameArr = depProp.Name.Split('.'); // ignore the Shadow suffix
                    var propName = nameArr[0]; // remove shadow name decorators
                    if ((propName == propertyName) || (depProp.OwnerType.Name + "." + propName == propertyName))
                    {
                        return depProp;
                    }
                }
            }
        }
        return null;
    }

    /// <summary>
    /// Gets all dependency properties that have local values set.
    /// </summary>
    /// <param name="obj">Object instance.</param>
    /// <returns>Returns System.Collections.Generic.Dictionary(string, DependencyProperty) object.</returns>
    public static Dictionary<string, DependencyProperty> WpfLocalOnlyDependencyProperties(DependencyObject obj)
    {
        Dictionary<string, DependencyProperty> result = new Dictionary<string, DependencyProperty>();
        if (obj != null)
        {

            LocalValueEnumerator lve = obj.GetLocalValueEnumerator();
            while (lve.MoveNext())
            {
                DependencyProperty depProp = lve.Current.Property;
                string name = depProp.OwnerType.Name + "." + depProp.Name;
                result[depProp.Name] = depProp;
                result[name] = depProp;
            }
        }
        return result;
    }

    /// <summary>
    /// Locates a dependency property that has a local value set.
    /// </summary>
    /// <param name="propertyName">Property name may be in format OwnerType.PropertyName' .</param>
    /// <param name="obj">Object instance.</param>
    /// <returns> Returns System.Windows.DependencyProperty object or null.</returns>
    public static DependencyProperty WpfLocateLocalOnlyDependencyProperty(string propertyName, DependencyObject obj)
    {
        if (obj != null)
        {
            LocalValueEnumerator lve = obj.GetLocalValueEnumerator();
            while (lve.MoveNext())
            {
                DependencyProperty depProp = lve.Current.Property;
                var nameArr = depProp.Name.Split('.');// ignore the Shadow suffix
                var propName = nameArr[0]; // remove shadow name decorators
                if ((propName == propertyName) || (depProp.OwnerType.Name + "." + propName == propertyName))
                {
                    return depProp;
                }
            }
        }
        return null;
    }

    //public static IEnumerable<DependencyProperty> EnumerateDependencyProperties(object element)
    //{
    //    if (element != null)
    //    {
    //        MarkupObject markupObject = MarkupWriter.GetMarkupObjectFor(element);
    //        if (markupObject != null)
    //        {
    //            foreach (MarkupProperty mp in markupObject.Properties)
    //            {
    //                if (mp.DependencyProperty != null)
    //                    yield return mp.DependencyProperty;
    //            }
    //        }
    //    }
    //}

    //public static IEnumerable<DependencyProperty> EnumerateAttachedProperties(object element)
    //{
    //    if (element != null)
    //    {
    //        MarkupObject markupObject = MarkupWriter.GetMarkupObjectFor(element);
    //        if (markupObject != null)
    //        {
    //            foreach (MarkupProperty mp in markupObject.Properties)
    //            {
    //                if (mp.IsAttached)
    //                    yield return mp.DependencyProperty;
    //            }
    //        }
    //    }
    //}

#endif

}


