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

// ReSharper disable RedundantUsingDirective
using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Input;


#if WINDOWS_UWP
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

#else
using System.Windows;
using System.Windows.Media;

#endif
// ReSharper restore RedundantUsingDirective

namespace MvvmBindingPack
{

    /// <summary>
    /// Proxy class that provides the proxy implementation of <see cref="ICommand"/> interface for case of command binding in markup extensions.
    /// </summary>
    public partial class CommandHandlerProxy : ICommand
    {
        /// <summary>
        /// The delegate that relays Execute method.
        /// </summary>
        protected virtual Action<object> ExecuteDelegate { get; set; }

        /// <summary>
        /// The delegate that relays CanExecute method.
        /// </summary>
        protected virtual Func<object, bool> CanExecuteDelegate { get; set; }

        /// <summary>
        /// The outer property that set on with notification delegate to void NotifyCanExecuteChanged();
        /// </summary>
        readonly PropertyInfo _outerPropertyInfo;
        readonly object _outerPropertyTarget;

        /// <summary>
        /// The outer event that subscribes with notification delegate to void NotifyCanExecuteChanged();
        /// </summary>
        readonly EventInfo _outerEventInfo;
        readonly object _outerEventTarget;

        /// <summary>
        /// The outer delegates that used to subscribes with notification delegate to void NotifyCanExecuteChanged();
        /// </summary>
        readonly Action<Action> _setupDelegateAction;
        readonly Action<Action> _removeDelegateAction;

        /// <summary>
        /// The outer delegates that used to subscribes with notification delegate to void NotifyCanExecuteChanged(object sender, EventArgs e);
        /// </summary>
        readonly Action<EventHandler> _setupDelegateEventHandler;
        readonly Action<EventHandler> _removeDelegateEventHandler;

        /// <summary>
        /// The outer Boolean property info and the target object that refers to
        /// a flag which should be return by  <see cref="ICommand.CanExecute"/> method.
        /// </summary>
        readonly PropertyInfo _outerBooleanPropertyInfo;
        readonly object _propertyBooleanTarget;

        EventHandler _canExecuteChanged;


        /// <summary>
        ///  Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public virtual event EventHandler CanExecuteChanged
        {
            add
            {
                if (_canExecuteChanged == null)
                {
                    SetupOuterNotifications();
                }

                _canExecuteChanged += value;
            }
            remove
            {
                if (_canExecuteChanged != null)
                {
                    // ReSharper disable DelegateSubtraction
                    _canExecuteChanged -= value;
                    // ReSharper restore DelegateSubtraction
                }

                if (_canExecuteChanged == null)
                {
                    RemoveOuterNotifications();
                }
            }
        }

        /// <summary>
        /// Setup outer notification interfaces for engaging the <see cref="ICommand"/> event notification.
        /// </summary>
        protected void SetupOuterNotifications()
        {
            if (_outerBooleanPropertyInfo != null)
            {
                // ReSharper disable SuggestUseVarKeywordEvident
                INotifyPropertyChanged notify = _propertyBooleanTarget as INotifyPropertyChanged;
                // ReSharper restore SuggestUseVarKeywordEvident
                if (notify != null)
                {
                    notify.PropertyChanged += PropertyChanged;
                }
            }

            if (_outerEventInfo != null)
            {
#if WINDOWS_UWP
                if (_outerEventInfo.EventHandlerType == typeof(Action) || _outerEventInfo.EventHandlerType.GetTypeInfo().IsSubclassOf(typeof(Action)))
#else
                if (_outerEventInfo.EventHandlerType == typeof(Action) || _outerEventInfo.EventHandlerType.IsSubclassOf(typeof(Action)))
#endif
                {
                    _outerEventInfo.AddEventHandler(_outerEventTarget, new Action(NotifyCanExecuteChanged));
                }
#if WINDOWS_UWP
                else if (_outerEventInfo.EventHandlerType == typeof(EventHandler) || _outerEventInfo.EventHandlerType.GetTypeInfo().IsSubclassOf(typeof(EventHandler)))
#else
                else if (_outerEventInfo.EventHandlerType == typeof(EventHandler) || _outerEventInfo.EventHandlerType.IsSubclassOf(typeof(EventHandler)))
#endif
                {
                    _outerEventInfo.AddEventHandler(_outerEventTarget, new EventHandler(NotifyCanExecuteChanged));
                }
            }

            if (_outerPropertyInfo != null)
            {
#if WINDOWS_UWP
                if (_outerPropertyInfo.PropertyType == typeof(Action) || _outerPropertyInfo.PropertyType.GetTypeInfo().IsSubclassOf(typeof(Action)))
#else
                if (_outerPropertyInfo.PropertyType == typeof(Action) || _outerPropertyInfo.PropertyType.IsSubclassOf(typeof(Action)))
#endif
                {
                    _outerPropertyInfo.SetValue(_outerPropertyTarget, new Action(NotifyCanExecuteChanged), null);
                }
#if WINDOWS_UWP
                else if (_outerPropertyInfo.PropertyType == typeof(EventHandler) || _outerPropertyInfo.PropertyType.GetTypeInfo().IsSubclassOf(typeof(EventHandler)))
#else
                else if (_outerPropertyInfo.PropertyType == typeof(EventHandler) || _outerPropertyInfo.PropertyType.IsSubclassOf(typeof(EventHandler)))
#endif
                {
                    _outerPropertyInfo.SetValue(_outerPropertyTarget, new EventHandler(NotifyCanExecuteChanged), null);
                }
            }

            if (_setupDelegateAction != null)
            {
                _setupDelegateAction(NotifyCanExecuteChanged);
            }
            if (_setupDelegateEventHandler != null)
            {
                _setupDelegateEventHandler(NotifyCanExecuteChanged);
            }
        }

        /// <summary>
        /// Remove outer notification interfaces for engaging the <see cref="ICommand"/> event notification.
        /// </summary>
        protected void RemoveOuterNotifications()
        {
            if (_outerBooleanPropertyInfo != null)
            {
                // ReSharper disable SuggestUseVarKeywordEvident
                INotifyPropertyChanged notify = _propertyBooleanTarget as INotifyPropertyChanged;
                // ReSharper restore SuggestUseVarKeywordEvident
                if (notify != null)
                {
                    notify.PropertyChanged -= PropertyChanged;
                }
            }

            if (_outerEventInfo != null)
            {
#if WINDOWS_UWP
                if (_outerEventInfo.EventHandlerType == typeof(Action) || _outerEventInfo.EventHandlerType.GetTypeInfo().IsSubclassOf(typeof(Action)))
#else
                if (_outerEventInfo.EventHandlerType == typeof(Action) || _outerEventInfo.EventHandlerType.IsSubclassOf(typeof(Action)))
#endif
                {
                    _outerEventInfo.RemoveEventHandler(_outerEventTarget, new Action(NotifyCanExecuteChanged));
                }
#if WINDOWS_UWP
                else if (_outerEventInfo.EventHandlerType == typeof(EventHandler) || _outerEventInfo.EventHandlerType.GetTypeInfo().IsSubclassOf(typeof(EventHandler)))
#else
                else if (_outerEventInfo.EventHandlerType == typeof(EventHandler) || _outerEventInfo.EventHandlerType.IsSubclassOf(typeof(EventHandler)))
#endif
                {
                    _outerEventInfo.RemoveEventHandler(_outerEventTarget, new EventHandler(NotifyCanExecuteChanged));
                }
            }

            if (_outerPropertyInfo != null)
            {
                _outerPropertyInfo.SetValue(_outerPropertyTarget, null, null);
            }

            if (_removeDelegateAction != null)
            {
                _removeDelegateAction(NotifyCanExecuteChanged);
            }
            if (_removeDelegateEventHandler != null)
            {
                _removeDelegateEventHandler(NotifyCanExecuteChanged);
            }
        }

        /// <summary>
        /// Constructs the proxy class implementation of <see cref="ICommand"/> interface with using 
        /// delegates of CanExecute and Execute methods. CanExecute delegate defines the method that determines whether the command can execute in its
        /// current state. Execute delegate Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="execute">The delegate that presents <see cref="ICommand.Execute"/> logic.</param>
        /// <param name="canExecute">The delegate that presents <see cref="ICommand.CanExecute"/> logic.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CommandHandlerProxy(Action<object> execute, Func<object, bool> canExecute = null)
        {
            if (execute == null)
            {
                throw new ArgumentException("CommandHandlerProxy - execute delegate cannot be null");
            }

            // ReSharper disable DoNotCallOverridableMethodsInConstructor
            ExecuteDelegate = execute;
            CanExecuteDelegate = canExecute;
            // ReSharper restore DoNotCallOverridableMethodsInConstructor
        }

        /// <summary>
        /// Constructs the proxy class implementation of <see cref="ICommand"/> interface with using 
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
        public CommandHandlerProxy(Action<object> execute, Func<object, bool> canExecute, EventInfo outerEventInfo, object outerEventTarget = null)
            : this(execute, canExecute)
        {
            if (canExecute == null)
            {
                throw new ArgumentException("CommandHandlerProxy - canExecute delegate cannot be null");
            }
            if (outerEventInfo == null)
            {
                throw new ArgumentException("CommandHandlerProxy - outerEvent cannot be null");
            }

            _outerEventInfo = outerEventInfo;
            _outerEventTarget = outerEventTarget;
        }

        /// <summary>
        /// Constructs the proxy class implementation of <see cref="ICommand"/> interface with using 
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
        public CommandHandlerProxy(Action<object> execute, Func<object, bool> canExecute, PropertyInfo outerPropertyInfo, object outerPropertyTarget = null)
            : this(execute, canExecute)
        {
            if (canExecute == null)
            {
                throw new ArgumentException("CommandHandlerProxy - canExecute delegate cannot be null");
            }
            if (outerPropertyInfo == null)
            {
                throw new ArgumentException("CommandHandlerProxy - outerProperty cannot be null");
            }
            if (outerPropertyInfo.CanWrite == false)
            {
                throw new ArgumentException("CommandHandlerProxy - outerProperty cannot be write protected");
            }

            _outerPropertyInfo = outerPropertyInfo;
            _outerPropertyTarget = outerPropertyTarget;

        }

        /// <summary>
        /// Constructs the proxy class implementation of <see cref="ICommand"/> interface with using 
        /// delegates of CanExecute and Execute methods. CanExecute delegate defines the method that determines whether the command can execute in its
        /// current state. Execute delegate Defines the method to be called when the command is invoked.
        /// Event notifications via CanExecuteChanged, when changes occur that affect whether or not the command should execute, are provided by
        /// a pair of the delegates(setup/remove). These delegates are called with <see cref="System.Action"/> type delegate which provides a notification call.
        /// </summary>
        /// <param name="execute">The delegate that presents <see cref="ICommand.Execute"/> logic.</param>
        /// <param name="canExecute">The delegate that presents <see cref="ICommand.CanExecute"/> logic.</param>
        /// <param name="setupDelegate">The delegate that accepts as a parameter the delegate <see cref="System.Action"/> to setup.</param>
        /// <param name="removeDelegate">The delegate that accepts as a parameter the delegate <see cref="System.Action"/> to remove.</param>
        /// 
        public CommandHandlerProxy(Action<object> execute, Func<object, bool> canExecute, Action<Action> setupDelegate, Action<Action> removeDelegate = null)
            : this(execute, canExecute)
        {
            if (canExecute == null)
            {
                throw new ArgumentException("CommandHandlerProxy - canExecute delegate cannot be null");
            }
            _setupDelegateAction = setupDelegate;
            _removeDelegateAction = removeDelegate;
        }

        /// <summary>
        /// Constructs the proxy class implementation of <see cref="ICommand"/> interface with using 
        /// delegates of CanExecute and Execute methods. CanExecute delegate defines the method that determines whether the command can execute in its
        /// current state. Execute delegate Defines the method to be called when the command is invoked.
        /// Event notifications via CanExecuteChanged, when changes occur that affect whether or not the command should execute, are provided by
        /// a pair of the delegates(setup/remove). These delegates are called with <see cref="System.EventHandler"/> type delegate which provides a notification call.
        /// </summary>
        /// <param name="execute">The delegate that presents <see cref="ICommand.Execute"/> logic.</param>
        /// <param name="canExecute">The delegate that presents <see cref="ICommand.CanExecute"/> logic.</param>
        /// <param name="setupDelegate">The delegate that accepts as a parameter the delegate <see cref="System.EventHandler"/>.</param>
        /// <param name="removeDelegate">The delegate that accepts as a parameter the delegate <see cref="System.EventHandler"/> to remove.</param>
        public CommandHandlerProxy(Action<object> execute, Func<object, bool> canExecute, Action<EventHandler> setupDelegate, Action<EventHandler> removeDelegate = null)
            : this(execute, canExecute)
        {
            if (canExecute == null)
            {
                throw new ArgumentException("CommandHandlerProxy - canExecute delegate cannot be null");
            }
            _setupDelegateEventHandler = setupDelegate;
            _removeDelegateEventHandler = removeDelegate;
        }

        /// <summary>
        /// Constructs the proxy class implementation of <see cref="ICommand"/> interface with using 
        /// delegates of CanExecute and Execute methods. CanExecute delegate defines the method that determines whether the command can execute in its
        /// current state. Execute delegate Defines the method to be called when the command is invoked.
        /// Event notifications via CanExecuteChanged, when changes occur that affect whether or not the command should execute, are provided by
        /// listening a Boolean property changes and providing a notification calls.
        /// </summary>
        /// <param name="execute">The delegate that presents <see cref="ICommand.Execute"/> logic.</param>
        /// <param name="outerBooleanProperty">PropertyInfo type information of property class member which will be set with Action delegate that rises the proxy event <see cref="ICommand.CanExecuteChanged"/>.</param>
        /// <param name="propertyBooleanTarget">The instance of the object that hold a property of <see cref="System.Action"/> delegate(null for static).</param>
        public CommandHandlerProxy(Action<object> execute, PropertyInfo outerBooleanProperty, object propertyBooleanTarget = null)
            : this(execute)
        {
            if (outerBooleanProperty == null)
            {
                throw new ArgumentException("CommandHandlerProxy - outerBooleanProperty cannot be null");
            }
            _outerBooleanPropertyInfo = outerBooleanProperty;
            _propertyBooleanTarget = propertyBooleanTarget;
        }

        /// <summary>
        /// Constructs the proxy class implementation of <see cref="ICommand"/> interface with using 
        /// delegates of CanExecute and Execute methods. CanExecute delegate defines the method that determines whether the command can execute in its
        /// current state. Execute delegate Defines the method to be called when the command is invoked.
        /// Event notifications via CanExecuteChanged, when changes occur that affect whether or not the command should execute, are provided by
        /// listening a Boolean property changes and providing a notification calls.
        /// </summary>
        /// <param name="execute">The delegate that presents <see cref="ICommand.Execute"/> logic.</param>
        /// <param name="targetType">Target type that contains property.</param>
        /// <param name="propertyBooleanTarget">The instance of the object that hold a property of <see cref="System.Action"/> delegate(null for static).</param>
        /// <param name="propertyBooleanName">The property name.</param>
        public CommandHandlerProxy(Action<object> execute, Type targetType, object propertyBooleanTarget, string propertyBooleanName)
            : this(execute)
        {
            PropertyInfo outerBooleanProperty;
            outerBooleanProperty = targetType.GetPropertyInfo(propertyBooleanName);
            if ((outerBooleanProperty == null) || (outerBooleanProperty.PropertyType != typeof(bool)))
            {
                throw new ArgumentException("CommandHandlerProxy - cannot resolve propertyBooleanName " + propertyBooleanName);
            }
            _outerBooleanPropertyInfo = outerBooleanProperty;
            _propertyBooleanTarget = propertyBooleanTarget;
        }

        /// <summary>
        /// Executes the delegate for <see cref="ICommand.CanExecute"/>.
        /// </summary>
        /// <param name="parameter"></param>-
        public bool CanExecute(object parameter)
        {
            if (_outerBooleanPropertyInfo != null)
            {
                var flag = _outerBooleanPropertyInfo.GetValue(_propertyBooleanTarget, null);
                return (bool)flag;
            }
            // ReSharper disable SimplifyConditionalTernaryExpression
            return CanExecuteDelegate == null ? true : CanExecuteDelegate(parameter);
            // ReSharper restore SimplifyConditionalTernaryExpression
        }

        /// <summary>
        /// Execute notification.
        /// </summary>
        protected virtual void ExecuteChangedNotify(object sender, EventArgs e)
        {
            if (_canExecuteChanged != null)
            {
                _canExecuteChanged(sender, e);
            }
        }

        /// <summary>
        /// Executes the delegate for <see cref="ICommand.Execute"/>.
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            if (ExecuteDelegate != null)
            { // check for case of weak reference
                ExecuteDelegate(parameter);
            }
        }

        /// <summary>
        /// Notifies if "can execute state" has changed.
        /// </summary>
        public void NotifyCanExecuteChanged()
        {
            ExecuteChangedNotify(this, EventArgs.Empty);
        }

        /// <summary>
        /// Notifies if "can execute state" has changed.
        /// </summary>
        public void NotifyCanExecuteChanged(object sender, EventArgs e)
        {
            ExecuteChangedNotify(sender, e);
        }

        /// <summary>
        /// Represents the method that will handle the System.ComponentModel.INotifyPropertyChanged.PropertyChanged
        /// event raised when a property is changed on a component.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">A System.ComponentModel.PropertyChangedEventArgs that contains the event data.</param>
        protected void PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == _outerBooleanPropertyInfo.Name)
            {
                NotifyCanExecuteChanged();
            }
        }

    }
}
