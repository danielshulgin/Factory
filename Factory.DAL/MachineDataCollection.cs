using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace Factory.DAL
{
    public class MachineDataCollection : ObservableCollection<MachineData>
    {
        public event EventHandler LineItemCostChanged;

        public DetailDataCollection lineItems;
        public new void Add(MachineData item)
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
            base.Remove((this as ObservableCollection<MachineData>).Where(i => i.Name == Name).First());
        }
    }
}