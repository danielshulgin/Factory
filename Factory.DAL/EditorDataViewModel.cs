using Factory;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Factory.DAL
{
    public class EditorDataViewModel : INotifyPropertyChanged
    {
        public DetailCollectionViewModel DetailTypes { get; set; }
        public MachineCollectionViewModel Machines { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private MachineViewModel _selectedMachine;

        public MachineViewModel SelectedMachine { 
            get { return _selectedMachine; }
            set
            {
                _selectedMachine = value;
                OnPropertyChanged("SelectedMachine");
            }
        }
        

        public EditorDataViewModel()
        {
            DetailTypes = new DetailCollectionViewModel();
            Machines = new MachineCollectionViewModel();
        }

        private void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
