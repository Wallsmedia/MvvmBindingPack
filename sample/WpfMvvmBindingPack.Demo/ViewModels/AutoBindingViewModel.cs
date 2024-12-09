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

using MvvmBindingPack;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace WpfMvvmBindingPackDemo.ViewModels;

public class AutoBindingViewModel : NotifyChangesBase
{
    [ViewXNameAlias("TextOfTitle", "Content")]
    private const string _textOfTitle = "AutoBindingViewModel-Title binding Example from const field";


    [AppendViewModel]
    AppendAutoBindingViewModel AppendAutoBindingViewModel { get; }


    public AutoBindingViewModel(AppendAutoBindingViewModel appendAutoBindingViewModel)
    {
        AppendAutoBindingViewModel = appendAutoBindingViewModel;
    }

    bool _state = true;

    [ViewXNameAlias("IncCounterClick", "IsEnabled")]
    [ViewXNameAlias("IncCounterCmd", "IsEnabled")]
    public bool AllFuncButtonsState
    {
        get => _state;
        set { _state = value; NotifyPropertyChanged(); }
    }

    //[ViewXNameAlias("EnableDisable","Click")] - attribute equivalent of the name convention
    public void EnableDisable_Click(object o, EventArgs e)
    {
        AllFuncButtonsState = !AllFuncButtonsState;
    }

    [ViewXNameAlias("IncCounterClick", "Click")]
    public void IncCounterClickMethod(object o, EventArgs e)
    {
        CountClick_Text++;
    }

    [ViewXNameAlias("IncCounterCmd", "Click")]
    public void IncCounterCmdMethod(object o, EventArgs e)
    {
        CountCmd++;
    }

    [ViewXNameAlias("IncCounterCmdEx", "Command.Execute")]
    public void IncCounterCmdExMethod(object o)
    {
        CntCmdEx++;
    }

    [ViewXNameAlias("IncCounterCmdEx", "Command.CanExecute")]
    public bool IncCounterCmdCnExMethod(object o)
    {
        return _state;
    }

    int _countClick;
    int _countCmd;
    int _countCmdEx;
    private string _actionPerson;

    //[ViewXNameAlias("CountClick", "Text")]
    public int CountClick_Text
    {
        get => _countClick; set { _countClick = value; NotifyPropertyChanged(); }
    }

    [ViewTarget("Text")]
    public int CountCmd
    {
        get => _countCmd; set { _countCmd = value; NotifyPropertyChanged(); }
    }

    [ViewXNameAlias("CountCmdEx", "Text")]
    public int CntCmdEx
    {
        get => _countCmdEx; set { _countCmdEx = value; NotifyPropertyChanged(); }
    }


    [ViewXNameAlias("ListViewItemsSource", "ItemsSource")]
        public ObservableCollection<Person> People { get; } = new ObservableCollection<Person>()
    {
        new Person("Michael", "Alexander", "Bellevue"),
        new Person("Jeff", "Hay", "Redmond"),
        new Person("Christina", "Lee", "Kirkland"),
        new Person("Samantha", "Smith", "Seattle")
    };

    public string ActionPerson
    {
        get => _actionPerson; set { _actionPerson = value; NotifyPropertyChanged(); }
    }
    public void ActionClickColumnMethod(object o, EventArgs e)
    {
        var person = ((Button)o).Tag as Person;
        ActionPerson = $"Action for person: {person.FirstName} {person.LastName} {person.HomeTown}";
    }
}