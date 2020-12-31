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
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace MvvmBindingPack
{
    /// <summary>
    /// Class implements the basic case of : INotifyDataErrorInfo + INotifyPropertyChanged
    /// </summary>
    public class NotifyChangesBase : NotifyPropertyChangedBase, INotifyDataErrorInfo
    {
        /// <summary>
        /// Occurs when the validation errors have changed for a property or for the
        /// entire entity.
        /// </summary>
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        private Lazy<Dictionary<string, List<string>>> errorsCollection = new Lazy<Dictionary<string, List<string>>>();

        /// <summary>
        /// Gets the validation errors for a specified property or for the entire entity.
        /// </summary>
        /// <param name="propertyName">The name of the property to retrieve validation errors for; or null or System.String.Empty, to retrieve entity-level errors.</param>
        /// <returns> The validation errors for the property or entity.</returns>
        public System.Collections.IEnumerable GetErrors(string propertyName)
        {
            if (errorsCollection.IsValueCreated)
            {
                if (string.IsNullOrEmpty(propertyName))
                {
                    return errorsCollection.Value;
                }
                if (errorsCollection.Value.Keys.Contains(propertyName))
                {
                    return errorsCollection.Value[propertyName];
                }
            }
            return Enumerable.Empty<object>();
        }

        /// <summary>
        /// Gets a value that indicates whether the entity has validation errors.
        /// Returns: true if the entity currently has validation errors; otherwise, false.
        /// </summary>
        public bool HasErrors
        {
            get
            {
                if (errorsCollection.IsValueCreated)
                {
                    return errorsCollection.Value.Keys.Count > 0;
                }
                return false;
            }
        }

#if !NET40
        /// <summary>
        /// Notifies of changing the property.
        /// </summary>
        /// <param name="propertyName">The property name.</param>
        protected void NotifyErrorsChanged([CallerMemberName] String propertyName = "")
#else
        /// <summary>
        /// Notifies of changing the property.
        /// </summary>
        /// <param name="propertyName">The property name.</param>
        protected void NotifyErrorsChanged(String propertyName)
#endif
        {
            if (ErrorsChanged != null)
                ErrorsChanged(this, new DataErrorsChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Removes all registered errors.
        /// </summary>
        /// <param name="notify">The flag tells to send notification after clearing of errors.</param>
        protected void ClearAllErrors(bool notify = true)
        {
            if (errorsCollection.IsValueCreated)
            {
                if (notify)
                {
                    foreach (var item in errorsCollection.Value.Keys)
                    {
                        NotifyErrorsChanged(item);
                    }
                }
                errorsCollection.Value.Clear();
            }
        }

        /// <summary>
        /// Marks property as a valid.
        /// </summary>
        /// <param name="propertyName">The property name.</param>
        /// <param name="notify">The flag tells to send notification.</param>
        protected void ValidProperty(String propertyName, bool notify = true)
        {
            if (errorsCollection.IsValueCreated)
            {
                if (errorsCollection.Value.Keys.Contains(propertyName))
                {
                    errorsCollection.Value.Remove(propertyName);
                    if (notify)
                    {
                        NotifyErrorsChanged(propertyName);
                    }
                }
            }
        }

        /// <summary>
        /// Marks property as invalid.
        /// </summary>
        /// <param name="propertyName">The property name.</param>
        /// <param name="notify">The flag tells to send notification.</param>
        /// <param name="errors">The errors messages.</param>
        protected void InValidProperty(String propertyName, bool notify, params String[] errors)
        {
            List<string> err = new List<string>(errors);
            errorsCollection.Value[propertyName] = err;
            if (notify)
            {
                NotifyErrorsChanged(propertyName);
            }
        }

    }
}