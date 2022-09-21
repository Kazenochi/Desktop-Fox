using CommunityToolkit.Mvvm.ComponentModel;
using Desktop_Fox;
using Desktop_Fox.MVVM.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Desktop_Fox
{
    public class MainWindowVM
    {
        MainWindowModel MainWindowModel { get; set; }
        public MainWindowVM()
        {
            MainWindowModel = new MainWindowModel();
        }
    }
}
