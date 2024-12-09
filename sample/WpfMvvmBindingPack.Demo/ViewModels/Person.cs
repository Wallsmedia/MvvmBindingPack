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

using System.ComponentModel;

namespace WpfMvvmBindingPackDemo.ViewModels;

public class Person : INotifyPropertyChanged
{
    private string _firstname;
    private string _hometown;
    private string _lastname;

    public Person()
    {
    }

    public Person(string first, string last, string town)
    {
        _firstname = first;
        _lastname = last;
        _hometown = town;
    }

    public string FirstName
    {
        get { return _firstname; }
        set
        {
            _firstname = value;
            OnPropertyChanged("FirstName");
        }
    }

    public string LastName
    {
        get { return _lastname; }
        set
        {
            _lastname = value;
            OnPropertyChanged("LastName");
        }
    }

    public string HomeTown
    {
        get { return _hometown; }
        set
        {
            _hometown = value;
            OnPropertyChanged("HomeTown");
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    public override string ToString() => _firstname;

    protected void OnPropertyChanged(string info)
    {
        var handler = PropertyChanged;
        handler?.Invoke(this, new PropertyChangedEventArgs(info));
    }
}