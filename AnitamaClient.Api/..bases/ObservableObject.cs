﻿using GalaSoft.MvvmLight.Threading;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace AnitamaClient
{
    public class ObservableObject : INotifyPropertyChanged
    {
        protected bool Set<TProp>(ref TProp field, TProp value, [CallerMemberName]string propertyName = null)
        {
            if(Equals(field, value))
                return false;
            ForceSet(ref field, value, propertyName);
            return true;
        }

        protected bool Set<TProp>(string addtionalPropertyName, ref TProp field, TProp value, [CallerMemberName]string propertyName = null)
        {
            if(Equals(field, value))
                return false;
            ForceSet(addtionalPropertyName, ref field, value, propertyName);
            return true;
        }

        protected bool Set<TProp>(string addtionalPropertyName0, string addtionalPropertyName1, ref TProp field, TProp value, [CallerMemberName]string propertyName = null)
        {
            if(Equals(field, value))
                return false;
            ForceSet(addtionalPropertyName0, addtionalPropertyName1, ref field, value, propertyName);
            return true;
        }

        protected bool Set<TProp>(IEnumerable<string> addtionalPropertyNames, ref TProp field, TProp value, [CallerMemberName]string propertyName = null)
        {
            if(Equals(field, value))
                return false;
            ForceSet(addtionalPropertyNames, ref field, value, propertyName);
            return true;
        }

        protected void ForceSet<TProp>(ref TProp field, TProp value, [CallerMemberName]string propertyName = null)
        {
            field = value;
            RaisePropertyChanged(propertyName);
        }

        protected void ForceSet<TProp>(string addtionalPropertyName, ref TProp field, TProp value, [CallerMemberName]string propertyName = null)
        {
            field = value;
            RaisePropertyChanged(propertyName, addtionalPropertyName);
        }

        protected void ForceSet<TProp>(string addtionalPropertyName0, string addtionalPropertyName1, ref TProp field, TProp value, [CallerMemberName]string propertyName = null)
        {
            field = value;
            RaisePropertyChanged(propertyName, addtionalPropertyName0, addtionalPropertyName1);
        }

        protected void ForceSet<TProp>(IEnumerable<string> addtionalPropertyNames, ref TProp field, TProp value, [CallerMemberName]string propertyName = null)
        {
            field = value;
            RaisePropertyChanged(addtionalPropertyNames.Concat(Enumerable.Repeat(propertyName, 1)));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged([CallerMemberName]string propertyName = null)
        {
            var temp = PropertyChanged;
            if(temp == null)
                return;
            DispatcherHelper.CheckBeginInvokeOnUI(() => temp(this, new PropertyChangedEventArgs(propertyName)));
        }

        protected void RaisePropertyChanged(string propertyName0, string propertyName1)
        {
            var temp = PropertyChanged;
            if(temp == null)
                return;
            DispatcherHelper.CheckBeginInvokeOnUI(() =>
            {
                temp(this, new PropertyChangedEventArgs(propertyName0));
                temp(this, new PropertyChangedEventArgs(propertyName1));
            });
        }

        protected void RaisePropertyChanged(string propertyName0, string propertyName1, string propertyName2)
        {
            var temp = PropertyChanged;
            if(temp == null)
                return;
            DispatcherHelper.CheckBeginInvokeOnUI(() =>
            {
                temp(this, new PropertyChangedEventArgs(propertyName0));
                temp(this, new PropertyChangedEventArgs(propertyName1));
                temp(this, new PropertyChangedEventArgs(propertyName2));
            });
        }

        protected void RaisePropertyChanged(params string[] propertyNames)
        {
            this.RaisePropertyChanged((IEnumerable<string>)propertyNames);
        }

        protected void RaisePropertyChanged(IEnumerable<string> propertyNames)
        {
            var temp = PropertyChanged;
            if(temp == null)
                return;
            DispatcherHelper.CheckBeginInvokeOnUI(() =>
            {
                foreach(var item in propertyNames)
                {
                    temp(this, new PropertyChangedEventArgs(item));
                }
            });
        }
    }
}
