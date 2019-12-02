using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Factory.DAL
{
    public class MachineViewModel : INotifyPropertyChanged
    {
        private int _timeToCreateDetail = 0;
        private string _name = "(Empty)";
        public MachineDetailsViewModel details { get; set; }

        public MachineViewModel() : base()
        {
            details = new MachineDetailsViewModel();
            details.CollectionChanged += (e, d) => OnPropertyChanged("Details");
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


        public int TimeToCreateDetail
        {
            get { return _timeToCreateDetail; }
            set
            {
                _timeToCreateDetail = value;
                OnPropertyChanged("TimeToCreateDetail");
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
        private string _name = "(Empty)";
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
