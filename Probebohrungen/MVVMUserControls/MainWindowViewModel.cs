﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MVVMUserControls
{
    internal class MainWindowViewModel : BindableBase
    {
        private ObservableCollection<IconInfo> _SourceIconInfos;
        private string _Prompt;

        public MainWindowViewModel()
        {
            LoadCommand = new RelayCommand(OnLoadIcons);
            //ClickHandlerCommand = new RelayCommand<string>(OnIconClicked, OnIconClicked_CanExecute);
        }
        public RelayCommand LoadCommand { get; set; }
        //public RelayCommand<string> ClickHandlerCommand { get; set; }

        public RelayCommand<string> ClickHandlerCommand { get { return new RelayCommand<string>(OnIconClicked, OnIconClicked_CanExecute); } }


        public ObservableCollection<IconInfo> SourceIconInfos
        {
            get { return _SourceIconInfos; }
            set { _SourceIconInfos = value;
                RaisePropertyChanged("SourceIconInfos");
            }
        }

        public string Prompt
        {
            get { return _Prompt; }
            set { _Prompt = value;
                RaisePropertyChanged("Prompt");
            }
        }

        private void OnIconClicked(string msg)
        {
            Prompt = msg + " Clicked";
        }

        bool OnIconClicked_CanExecute(string msg)
        {
            return false;
        }

        private void OnLoadIcons()
        {
            SourceIconInfos = new ObservableCollection<IconInfo>
            {
                new IconInfo
                {
                    Label = "3D Shape 01",
                    ResourcePath ="/MVVMUserControls;component/Images/3D-Shape-01.png",
                    Command = this.ClickHandlerCommand
                },
                new IconInfo
                {
                    Label = "3D Shape 02",
                    ResourcePath ="/MVVMUserControls;component/Images/3D-Shape-02.png",
                    Command = this.ClickHandlerCommand
                },
                new IconInfo
                {
                    Label = "Astrologer",
                    ResourcePath ="/MVVMUserControls;component/Images/Astrologer.png",
                    Command = this.ClickHandlerCommand
                },
                new IconInfo
                {
                    Label = "Balance",
                    ResourcePath ="/MVVMUserControls;component/Images/Balance-02.png",
                    Command = this.ClickHandlerCommand
                },
                new IconInfo
                {
                    Label = "Candle",
                    ResourcePath ="/MVVMUserControls;component/Images/Candle.png",
                    Command = this.ClickHandlerCommand
                },
                new IconInfo
                {
                    Label = "Clapper-Board",
                    ResourcePath ="/MVVMUserControls;component/Images/Clapper-Board-02.png",
                    Command = this.ClickHandlerCommand
                },
                new IconInfo
                {
                    Label = "Container",
                    ResourcePath ="/MVVMUserControls;component/Images/Container.png",
                    Command = this.ClickHandlerCommand
                },
                new IconInfo
                {
                    Label = "Cooler",
                    ResourcePath ="/MVVMUserControls;component/Images/Cooler-01.png",
                    Command = this.ClickHandlerCommand
                },
                new IconInfo
                {
                    Label = "Crown-King",
                    ResourcePath ="/MVVMUserControls;component/Images/Crown-King.png",
                    Command = this.ClickHandlerCommand
                },
                new IconInfo
                {
                    Label = "Digital Signage",
                    ResourcePath ="/MVVMUserControls;component/Images/DigitalSignage.png",
                    Command = this.ClickHandlerCommand
                },
            };
        }
    }
}
