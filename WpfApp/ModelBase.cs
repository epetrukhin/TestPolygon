using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace WpfApp
{
    internal abstract class ModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected void RaisePropertiesChanged([NotNull] params string[] properties)
        {
            if (properties == null)
                throw new ArgumentNullException(nameof(properties));
            if (properties.Length == 0)
                throw new ArgumentException("properties is empty", nameof(properties));

            var temp = PropertyChanged;
            if (temp == null)
                return;

            for (var i = 0; i < properties.Length; i++)
            {
                temp(this, new PropertyChangedEventArgs(properties[i]));
            }
        }

        [NotifyPropertyChangedInvocator]
        protected bool SetProperty<T>([CanBeNull] ref T field, [CanBeNull] T value, [NotNull, CallerMemberName] string property = "")
        {
            if (property == null)
                throw new ArgumentNullException(nameof(property));

            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;

            field = value;
            RaisePropertiesChanged(property);
            return true;
        }
    }
}