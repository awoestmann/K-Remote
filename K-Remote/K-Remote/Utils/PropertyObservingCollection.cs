using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K_Remote.Utils
{
    class PropertyObservingCollection<T> : ObservableCollection<T> where T : INotifyPropertyChanged
    {
        public PropertyObservingCollection() : base() { }

        public PropertyObservingCollection(List<T> list) : base(list)
        {
            foreach(T item in list)
            {
                item.PropertyChanged += RaisePropertyChanged;
            }
        }

        public PropertyObservingCollection(T[] array) : base(array)
        {
            foreach (T item in array)
            {
                item.PropertyChanged += new PropertyChangedEventHandler(RaisePropertyChanged);
            }
        }

        private void RaisePropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            OnPropertyChanged(args);
        }

        public new void Add(T item)
        {
            item.PropertyChanged += new PropertyChangedEventHandler(RaisePropertyChanged);
            base.Add(item);
        }

        public new void Insert(int index, T item)
        {
            item.PropertyChanged += new PropertyChangedEventHandler(RaisePropertyChanged);
            base.Insert(index, item);
        }

        public new void Clear()
        {
            foreach(T item in Items)
            {
                item.PropertyChanged -= new PropertyChangedEventHandler(RaisePropertyChanged);
            }
            base.Clear();
        }
    }
}
