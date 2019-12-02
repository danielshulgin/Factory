using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace Factory.DAL
{
    public class MachineCollectionViewModel : ObservableCollection<MachineViewModel>
    {
        public event EventHandler LineItemCostChanged;

        public DetailCollectionViewModel lineItems;
        public new void Add(MachineViewModel item)
        {
            if (item != null)
            {
                item.PropertyChanged += LineItemPropertyChanged;
            }
            if (this.Where(machine => machine.Name == item.Name).Count() == 0)
            {
                base.Add(item);
            }
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
            base.Remove((this as ObservableCollection<MachineViewModel>).Where(i => i.Name == Name).First());
        }
    }
}