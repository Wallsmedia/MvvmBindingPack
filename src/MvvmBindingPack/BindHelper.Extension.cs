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

namespace MvvmBindingPack
{
    /// <summary>
    /// Class methods extensions for extracting info type objects from a class instance.
    /// </summary>
    public static partial class BindHelper
    {
        /// <summary>
        /// Gets property info from a class.
        /// </summary>
        /// <param name="obj">The target object which is to extract from.</param>
        /// <param name="propertyName">The name of the subject to locate.</param>
        /// <returns>Info object or null if it is not found.</returns>
        static public PropertyInfo GetPropertyInfo(this object obj, string propertyName)
        {
            return GetPropertyInfo(obj.GetType(), propertyName);
        }

        /// <summary>
        /// Gets property info from a class.
        /// </summary>
        /// <param name="type">The target type which is to extract from.</param>
        /// <param name="propertyName">The name of the subject to locate.</param>
        /// <returns>Info object or null if it is not found.</returns>
        static public PropertyInfo GetPropertyInfo(this Type type, string propertyName)
        {
#if WINDOWS_UWP
            PropertyInfo info = null;
            Type lookUpType = type;
            while (lookUpType != null)
            {
                info = lookUpType.GetTypeInfo().GetDeclaredProperty(propertyName);
                if (info != null)
                {
                    break;
                }
                lookUpType = lookUpType.GetTypeInfo().BaseType;
            }
            return info;
#else
            PropertyInfo info = null;
            var lookUpType = type;
            while (lookUpType != null)
            {
                info = lookUpType.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                if (info != null)
                {
                    break;
                }
                lookUpType = lookUpType.BaseType;
            }
            return info;
#endif
        }

        /// <summary>
        /// Gets methods info from a class.
        /// </summary>
        /// <param name="obj">The target object which is to extract from.</param>
        /// <param name="methodName">The name of the subject to locate.</param>
        /// <returns>Info object or null if it is not found.</returns>
        static public MethodInfo GetMethodInfo(this object obj, string methodName)
        {
            return GetMethodInfo(obj.GetType(), methodName);
        }

        /// <summary>
        /// Gets methods info from a class.
        /// </summary>
        /// <param name="type">The target type which is to extract from.</param>
        /// <param name="methodName">The name of the subject to locate.</param>
        /// <returns>Info object or null if it is not found.</returns>
        static public MethodInfo GetMethodInfo(this Type type, string methodName)
        {
#if WINDOWS_UWP
            MethodInfo info = null;
            var lookUpType = type;
            while (lookUpType != null)
            {
                info = lookUpType.GetTypeInfo().GetDeclaredMethod(methodName);
                if (info != null)
                {
                    break;
                }
                lookUpType = lookUpType.GetTypeInfo().BaseType;
            }
            return info;
#else
            MethodInfo info = null;
            var lookUpType = type;
            while (lookUpType != null)
            {
                info = lookUpType.GetMethod(methodName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                if (info != null)
                {
                    break;
                }
                lookUpType = lookUpType.BaseType;
            }
            return info;
#endif
        }

        /// <summary>
        /// Gets event info from a class.
        /// </summary>
        /// <param name="obj">The target object which is to extract from.</param>
        /// <param name="eventName">The name of the subject to locate.</param>
        /// <returns>Info object or null if it is not found.</returns>
        static public EventInfo GetEventInfo(this object obj, string eventName)
        {
            return GetEventInfo(obj.GetType(), eventName);
        }

        /// <summary>
        /// Gets event info from a class.
        /// </summary>
        /// <param name="type">The target type which is to extract from.</param>
        /// <param name="eventName">The name of the subject to locate.</param>
        /// <returns>Info object or null if it is not found.</returns>
        static public EventInfo GetEventInfo(this Type type, string eventName)
        {
#if WINDOWS_UWP
            EventInfo info = null;
            var lookUpType = type;
            while (lookUpType != null)
            {
                info = lookUpType.GetTypeInfo().GetDeclaredEvent(eventName);
                if (info != null)
                {
                    break;
                }
                lookUpType = lookUpType.GetTypeInfo().BaseType;
            }
            return info;
#else
            EventInfo info = null;
            var lookUpType = type;
            while (lookUpType != null)
            {
                info = lookUpType.GetEvent(eventName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                if (info != null)
                {
                    break;
                }
                lookUpType = lookUpType.BaseType;
            }
            return info;
#endif
        }


        /// <summary>
        /// Gets event info from a class.
        /// </summary>
        /// <param name="obj">The target object which is to extract from.</param>
        /// <param name="fieldName">The name of the subject to locate.</param>
        /// <returns>Info object or null if it is not found.</returns>
        static public FieldInfo GetFieldInfo(this object obj, string fieldName)
        {
            return GetFieldInfo(obj.GetType(), fieldName);
        }

        /// <summary>
        /// Gets field info from a class.
        /// </summary>
        /// <param name="type">The target type which is to extract from.</param>
        /// <param name="fieldName">The name of the subject to locate.</param>
        /// <returns>Info object or null if it is not found.</returns>
        static public FieldInfo GetFieldInfo(this Type type, string fieldName)
        {
#if WINDOWS_UWP
            FieldInfo info = null;
            var lookUpType = type;
            while (lookUpType != null)
            {
                info = lookUpType.GetTypeInfo().GetDeclaredField(fieldName);
                if (info != null)
                {
                    break;
                }
                lookUpType = lookUpType.GetTypeInfo().BaseType;
            }
            return info;
#else
            FieldInfo info = null;
            var lookUpType = type;
            while (lookUpType != null)
            {
                info = lookUpType.GetField(fieldName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                if (info != null)
                {
                    break;
                }
                lookUpType = lookUpType.BaseType;
            }
            return info;
#endif
        }




    }
}