using Factory;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Factory.DAL
{
    public class EditorData : INotifyPropertyChanged
    {
        
        public DetailDataCollection DetailTypes { get; set; }
        public MachineDataCollection Machines { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;


        public MachineData SelectedMachine { 
            get { return _selectedMachine; }
            set
            {
                _selectedMachine = value;
                OnPropertyChanged("SelectedMachine");
                OnPropertyChanged("machineDetails");
            }
        }
        

        public EditorData()
        {
            DetailTypes = new DetailDataCollection();
            Machines = new MachineDataCollection();
            DetailTypes.LineItemCostChanged += OnLineItemCostChanged;
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
            get
            {
                RecalculateTotalExpense();
                return _totalExpenses;
            }
        }

        private string _alias;
        private string _costCenter;
        private string _employeeNumber;
        private int _totalExpenses;
        private MachineData _selectedMachine;

        private void OnLineItemCostChanged(object sender, EventArgs e)
        {
            OnPropertyChanged("TotalExpenses");
        }

        private void RecalculateTotalExpense()
        {
            _totalExpenses = 0;
            foreach (var item in DetailTypes)
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
