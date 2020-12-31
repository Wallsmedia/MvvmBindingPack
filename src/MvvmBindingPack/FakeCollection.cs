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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
#if WINDOWS_UWP
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Controls.Primitives;

#else
using System.Windows;

#endif

namespace MvvmBindingPack
{
    /// <summary>
    /// The fake collection which is used to execute operations with binding events to their handlers.
    /// </summary>
    public class FakeCollection : FakeCollectionBase<object>
    {
        /// <summary>
        /// The action processing delegate. it calls when the new element
        /// has been added to collection. 
        /// </summary>
        public Action<DependencyObject, object> ExecuteAction { get; set; }
        /// <summary>
        /// The dependence object as a target to process.
        /// </summary>
        public DependencyObject DependencyObjectElement { get; set; }
#pragma warning disable 1591
        public override void Add(object item)
        {
            if ((ExecuteAction != null) && (DependencyObjectElement != null))
            {
                ExecuteAction(DependencyObjectElement, item);
            }
        }
#pragma warning restore 1591

    }

    /// <summary>
    /// The fake collection which is used to intercept the add to the collection event and execute an action delegate.
    /// </summary>
    /// <typeparam name="T">Type that is supported by collection.</typeparam>
    public class FakeCollectionBase<T> : IList<T>, IList
    {
#pragma warning disable 1591
        public int IndexOf(T item) { return 0; }

        public void Insert(int index, T item) { }

        public void RemoveAt(int index) { }

        public T this[int index] { get { return default(T); } set { } }

        public virtual void Add(T item) { }

        public void Clear() { }

        public bool Contains(T item) { return false; }

        public void CopyTo(T[] array, int arrayIndex) { }

        public int Count { get { return 0; } }

        public bool IsReadOnly { get { return false; } }

        public bool Remove(T item) { return true; }

        public IEnumerator<T> GetEnumerator() { return Enumerable.Empty<T>().GetEnumerator(); }

        IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }

        int IList.Add(object value) { T valT = (T)value; Add(valT); return 0; }

        void IList.Clear() { }

        bool IList.Contains(object value) { return false; }

        int IList.IndexOf(object value) { return 0; }

        void IList.Insert(int index, object value) { }

        bool IList.IsFixedSize { get { return false; } }

        bool IList.IsReadOnly { get { return false; } }

        void IList.Remove(object value) { }

        void IList.RemoveAt(int index) { }

        object IList.this[int index] { get { return null; } set { } }

        void ICollection.CopyTo(Array array, int index) { }

        int ICollection.Count { get { return 0; } }

        bool ICollection.IsSynchronized { get { return false; } }

        object ICollection.SyncRoot { get { return this; } }
#pragma warning restore 1591
    }

}