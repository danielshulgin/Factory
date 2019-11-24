using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Factory
{
    public class MachineData : ObservableCollection<DetailInMachine>, INotifyPropertyChanged
    {
        public event EventHandler LineItemCostChanged;

        public DetailDataCollection lineItems;
        public new void Add(DetailInMachine item)
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
            base.Remove((this as ObservableCollection<DetailInMachine>).Where(i => i.Name == Name).First());
        }


        private int _cost;
        private string _description = "(Description)";
        private string _name = "(Expense type)";

        public MachineDetails details;

        public MachineData() : base()
        {
            details = new MachineDetails();
            details.CollectionChanged += (e, d) => OnPropertyChanged("details");
        }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                OnPropertyChanged("Description");
            }
        }

        public int Cost
        {
            get { return _cost; }
            set
            {
                _cost = value;
                OnPropertyChanged("Cost");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }

    public class DetailInMachine : INotifyPropertyChanged
    {
        private string _name = "Empty";
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(_name);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
