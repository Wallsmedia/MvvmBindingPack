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
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace MvvmBindingPack;

/// <summary>
/// Class implements the basic case of : INotifyPropertyChanged
/// </summary>
public class NotifyPropertyChangedBase : INotifyPropertyChanged
{
    /// <summary>
    /// Implementation of INotifyPropertyChanged interface
    /// </summary>
    public event PropertyChangedEventHandler PropertyChanged;

    /// <summary>
    /// This method is called by the Set accessory of each property.
    /// </summary>
    /// <param name="propertyName">The CallerMemberName attribute that is applied to the optional 
    /// propertyNameparameter causes the property name of the caller to be substituted as an argument.
    /// </param>
#if !NET40
    protected void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
#else
    protected void NotifyPropertyChanged(String propertyName)
#endif
    {
        if (PropertyChanged != null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    /// <summary>
    /// Implementation of INotifyPropertyChanged interface
    /// </summary>
    /// <param name="exp">Lambda expression.</param>
    public void NotifyPropertyChanged(Expression<Func<object>> exp)
    {
        MemberExpression body = exp.Body as MemberExpression;

        if (body == null)
        {
            UnaryExpression ubody = (UnaryExpression)exp.Body;
            body = ubody.Operand as MemberExpression;
        }
        NotifyPropertyChanged(body.Member.Name);
    }
}