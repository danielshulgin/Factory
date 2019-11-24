// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace Factory
{
    public class DetailDataCollection : ObservableCollection<DetailData>
    {
        public event EventHandler LineItemCostChanged;

        public DetailDataCollection lineItems;

        public object Message { get; private set; }

        public new void Add(DetailData item)
        {
            if (item != null)
            {
                item.PropertyChanged += LineItemPropertyChanged;
            }
            base.Add(item);
        }

        private void LineItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Cost")
            {
                OnLineItemCostChanged(this, new EventArgs());
            }
        }

        private void OnLineItemCostChanged(object sender, EventArgs args)
        {
            LineItemCostChanged?.Invoke(sender, args);
        }

        public void Remove(string Name)
        {
            base.Remove((this as ObservableCollection<DetailData>).Where(i => i.Name == Name).First());
        }
    }
}
