using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ConfigTool.ConfigCore
{
    public abstract class ConfigBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {   // 2025-6-4
            if (EqualityComparer<T>.Default.Equals(field, value))
            {
                return false;
            }
            T oldValue = field;
            field = value;
            OnPropertyChanged(propertyName);

            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Property changed [{propertyName.PadRight(15, ' ')}] : {oldValue} \u27A4 {value}");//2025-10-11 这个方法竟然一直没有使用
            return true;
        }
    }
}
