﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace MVVMUserControls
{
    public class BindableBase : INotifyPropertyChanged
    {
        //protected virtual void SetProperty<T>(ref T member, T val,
        //    [CallerMemberName] string propertyName = null)
        //{
        //    if (object.Equals(member, val)) return;

        //    member = val;
        //    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        //}

        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            this.PropertyChanged?.Invoke(this, e);
        }

        //protected virtual void OnPropertyChanged(string propertyName)
        //{
        //    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        //}
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }
}
