// 
//  MVVM-WPF-NetCore Markup, Binding and other Extensions.
//  Copyright © 2013-2019 Alexander Paskhin /paskhin@hotmail.co.uk/ All rights reserved.
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
using System.Reflection;
using System.Windows.Input;


#if WINDOWS_UWP
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
#else
using System.Windows;
using System.Windows.Media;
#endif

namespace MvvmBindingPack
{

    /// <summary>
    /// Proxy class that provides the weak referenced implementation of <see cref="ICommand"/> interface for case of command binding.
    /// </summary>
    public class WeakCommandHandlerProxy : CommandHandlerProxy
    {
        /// <summary>
        /// Constructs the weak proxy class implementation of <see cref="ICommand"/> interface with using 
        /// delegates of CanExecute and Execute methods. CanExecute delegate defines the method that determines whether the command can execute in its
        /// current state. Execute delegate Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="execute">The delegate that presents <see cref="ICommand.Execute"/> logic.</param>
        /// <param name="canExecute">The delegate that presents <see cref="ICommand.CanExecute"/> logic.</param>
        public WeakCommandHandlerProxy(Action<object> execute, Func<object, bool> canExecute = null)
            : base(execute, canExecute)
        {
        }

        /// <summary>
        /// Constructs the weak proxy class implementation of <see cref="ICommand"/> interface with using 
        /// delegates of CanExecute and Execute methods. CanExecute delegate defines the method that determines whether the command can execute in its
        /// current state. Execute delegate Defines the method to be called when the command is invoked.
        /// Event notifications via CanExecuteChanged, when changes occur that affect whether or not the command should execute, are pushed through the subscription 
        /// to the outer event.
        /// </summary>
        /// <param name="execute">The delegate that presents <see cref="ICommand.Execute"/> logic.</param>
        /// <param name="canExecute">The delegate that presents <see cref="ICommand.CanExecute"/> logic.</param>
        /// <param name="outerEventInfo">EventInfo type information of an event class member to which will be added a delegate that rises the proxy event <see cref="ICommand.CanExecuteChanged"/>.
        /// Supported event types are <see cref="System.Action"/> and <see cref="System.EventHandler"/>.</param>
        /// <param name="outerEventTarget">The instance of the object that holds the event member(null for static).</param>
        public WeakCommandHandlerProxy(Action<object> execute, Func<object, bool> canExecute, EventInfo outerEventInfo, object outerEventTarget = null)
            : base(execute, canExecute, outerEventInfo, outerEventTarget)
        {
        }

        /// <summary>
        /// Constructs the weak proxy class implementation of <see cref="ICommand"/> interface with using 
        /// delegates of CanExecute and Execute methods. CanExecute delegate defines the method that determines whether the command can execute in its
        /// current state. Execute delegate Defines the method to be called when the command is invoked.
        /// Event notifications via CanExecuteChanged, when changes occur that affect whether or not the command should execute, are pushed through the providing/placing
        /// the delegate on property.
        /// </summary>
        /// <param name="execute">The delegate that presents <see cref="ICommand.Execute"/> logic.</param>
        /// <param name="canExecute">The delegate that presents <see cref="ICommand.CanExecute"/> logic.</param>
        /// <param name="outerPropertyInfo">PropertyInfo type information of property class member which will be set with a delegate that rises the proxy event <see cref="ICommand.CanExecuteChanged"/>.
        /// Supported property types are <see cref="System.Action"/> and <see cref="System.EventHandler"/>.</param>
        /// <param name="outerPropertyTarget">The instance of the object that hold a property of a delegate(null for static).</param>
        public WeakCommandHandlerProxy(Action<object> execute, Func<object, bool> canExecute, PropertyInfo outerPropertyInfo, object outerPropertyTarget = null)
            : base(execute, canExecute, outerPropertyInfo, outerPropertyTarget)
        {
        }

        /// <summary>
        /// Constructs the weak proxy class implementation of <see cref="ICommand"/> interface with using 
        /// delegates of CanExecute and Execute methods. CanExecute delegate defines the method that determines whether the command can execute in its
        /// current state. Execute delegate Defines the method to be called when the command is invoked.
        /// Event notifications via CanExecuteChanged, when changes occur that affect whether or not the command should execute, are provided by
        /// a pair of the delegates(setup/remove). These delegates are called with <see cref="System.Action"/> type delegate which provides a notification call.
        /// </summary>
        /// <param name="execute">The delegate that presents <see cref="ICommand.Execute"/> logic.</param>
        /// <param name="canExecute">The delegate that presents <see cref="ICommand.CanExecute"/> logic.</param>
        /// <param name="setupDelegate">The delegate that accepts as a parameter the delegate <see cref="System.Action"/> to setup.</param>
        /// <param name="removeDelegate">The delegate that accepts as a parameter the delegate <see cref="System.Action"/> to remove.</param>
        public WeakCommandHandlerProxy(Action<object> execute, Func<object, bool> canExecute, Action<Action> setupDelegate, Action<Action> removeDelegate = null)
            : base(execute, canExecute, setupDelegate, removeDelegate)
        {
        }

        /// <summary>
        /// Constructs the weak proxy class implementation of <see cref="ICommand"/> interface with using 
        /// delegates of CanExecute and Execute methods. CanExecute delegate defines the method that determines whether the command can execute in its
        /// current state. Execute delegate Defines the method to be called when the command is invoked.
        /// Event notifications via CanExecuteChanged, when changes occur that affect whether or not the command should execute, are provided by
        /// a pair of the delegates(setup/remove). These delegates are called with <see cref="System.EventHandler"/> type delegate which provides a notification call.
        /// </summary>
        /// <param name="execute">The delegate that presents <see cref="ICommand.Execute"/> logic.</param>
        /// <param name="canExecute">The delegate that presents <see cref="ICommand.CanExecute"/> logic.</param>
        /// <param name="setupDelegate">The delegate that accepts as a parameter the delegate <see cref="System.EventHandler"/>.</param>
        /// <param name="removeDelegate">The delegate that accepts as a parameter the delegate <see cref="System.EventHandler"/> to remove.</param>
        public WeakCommandHandlerProxy(Action<object> execute, Func<object, bool> canExecute, Action<EventHandler> setupDelegate, Action<EventHandler> removeDelegate = null)
            : base(execute, canExecute, setupDelegate, removeDelegate)
        {
        }

        /// <summary>
        /// Constructs the weak proxy class implementation of <see cref="ICommand"/> interface with using 
        /// delegates of CanExecute and Execute methods. CanExecute delegate defines the method that determines whether the command can execute in its
        /// current state. Execute delegate Defines the method to be called when the command is invoked.
        /// Event notifications via CanExecuteChanged, when changes occur that affect whether or not the command should execute, are provided by
        /// listening a Boolean property changes and providing a notification calls.
        /// </summary>
        /// <param name="execute">The delegate that presents <see cref="ICommand.Execute"/> logic.</param>
        /// <param name="outerBooleanProperty">PropertyInfo type information of property class member which will be set with Action delegate that rises the proxy event <see cref="ICommand.CanExecuteChanged"/>.</param>
        /// <param name="propertyBooleanTarget">The instance of the object that hold a property of <see cref="System.Action"/> delegate(null for static).</param>
        public WeakCommandHandlerProxy(Action<object> execute, PropertyInfo outerBooleanProperty, object propertyBooleanTarget = null)
            : base(execute, outerBooleanProperty, propertyBooleanTarget)
        {
        }

        /// <summary>
        /// Constructs the proxy class implementation of <see cref="ICommand"/> interface with using 
        /// delegates of CanExecute and Execute methods. CanExecute delegate defines the method that determines whether the command can execute in its
        /// current state. Execute delegate Defines the method to be called when the command is invoked.
        /// Event notifications via CanExecuteChanged, when changes occur that affect whether or not the command should execute, are provided by
        /// listening a Boolean property changes and providing a notification calls.
        /// </summary>
        /// <param name="execute">The delegate that presents <see cref="ICommand.Execute"/> logic.</param>
        /// <param name="targetType">The target type that contains property.</param>
        /// <param name="propertyBooleanTarget">The instance of the object that hold a property of <see cref="System.Action"/> delegate(null for static).</param>
        /// <param name="propertyBooleanName">The property name.</param>
        public WeakCommandHandlerProxy(Action<object> execute, Type targetType, object propertyBooleanTarget, string propertyBooleanName)
            : base(execute, targetType,propertyBooleanTarget ,propertyBooleanName)
        {
        }

        readonly List<WeakReference> _canExecuteChanged = new List<WeakReference>();
        /// <summary>
        ///  Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public override event EventHandler CanExecuteChanged
        {
            add
            {
                if (_canExecuteChanged.Count == 0)
                {
                    SetupOuterNotifications();
                }
                _canExecuteChanged.Add(new WeakReference(value));
            }
            remove
            {
                for (int i = 0; i < _canExecuteChanged.Count; i++)
                {
                    if (_canExecuteChanged[i].IsAlive && ReferenceEquals(_canExecuteChanged[i].Target, value))
                    {
                        _canExecuteChanged.RemoveAt(i);
                        break;
                    }
                }
                if (_canExecuteChanged.Count == 0)
                {
                    RemoveOuterNotifications();
                }
            }
        }

        /// <summary>
        /// Notifies if "can execute state" has changed.
        /// </summary>
        protected override void ExecuteChangedNotify(object sender, EventArgs e)
        {
            if (_canExecuteChanged.Count > 0)
            {
                foreach (var item in _canExecuteChanged)
                {
                    if (item.IsAlive)
                    {
                        ((EventHandler)(item.Target))(sender, e);
                    }
                }
            }
        }

    }
}
