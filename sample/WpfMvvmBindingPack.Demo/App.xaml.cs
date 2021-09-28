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

using Microsoft.Extensions.DependencyInjection;
using MvvmBindingPack;
using System.Windows;
using System.Windows.Input;
using WpfMvvmBindingPackDemo.ViewModels;

namespace WpfDemoApplication
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable")]
    public partial class App
    {
        

        private void Application_Startup(object sender, StartupEventArgs e)
        {

            ServiceCollection services = new ServiceCollection();
            services.AddSingleton<AutoBindingViewModel>();
            services.AddSingleton<AppendAutoBindingViewModel>();
            services.AddSingleton<IocBindingViewModel>();
            services.AddSingleton<AppendCommonViewModel>();
            AutoWireVmDataContext.ServiceProvider = services.BuildServiceProvider();

        }
    }
}
