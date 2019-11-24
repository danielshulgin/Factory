using Factory;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Factory
{
    public class EditorData : INotifyPropertyChanged
    {
        private string _alias;
        private string _costCenter;
        private string _employeeNumber;
        private int _totalExpenses;
        private MachineData _selectedMachine;
        public MachineData SelectedMachine { 
            get { return _selectedMachine; }
            set
            {
                _selectedMachine = value;
                OnPropertyChanged("SelectedMachine");
                OnPropertyChanged("machineDetails");
                details = machineDetails;
            }
        }
        private MachineDetails details;
public MachineDetails machineDetails
        {
            get
            { 
                return details;
            }
        }

        public EditorData()
        {
            //SelectedMachine = new MachineData();
            details = new MachineDetails();
            LineItems = new DetailDataCollection();
            Machines = new MachineDataCollection();
            LineItems.LineItemCostChanged += OnLineItemCostChanged;
        }

        public string Alias
        {
            get { return _alias; }
            set
            {
                _alias = value;
                OnPropertyChanged("Alias");
            }
        }

        public string CostCenter
        {
            get { return _costCenter; }
            set
            {
                _costCenter = value;
                OnPropertyChanged("CostCenter");
            }
        }

        public string EmployeeNumber
        {
            get { return _employeeNumber; }
            set
            {
                _employeeNumber = value;
                OnPropertyChanged("EmployeeNumber");
            }
        }

        public int TotalExpenses
        {
            // calculated property, no setter
            get
            {
                RecalculateTotalExpense();
                return _totalExpenses;
            }
        }

        public DetailDataCollection LineItems { get; set; }
        public MachineDataCollection Machines { get; set; }
        public List<SData> list = new List<SData>() {new SData("Name"), new SData("Name1") };
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnLineItemCostChanged(object sender, EventArgs e)
        {
            OnPropertyChanged("TotalExpenses");
        }

        private void RecalculateTotalExpense()
        {
            _totalExpenses = 0;
            foreach (var item in LineItems)
                _totalExpenses += item.Cost;
        }

        private void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }

    public class SData
    {
        public string Name;
        public SData(string Name)
        {
            this.Name = Name;
        }
    }

}
