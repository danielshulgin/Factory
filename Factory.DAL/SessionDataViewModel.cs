using Factory.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Factory.DAL
{
    public class SessionDataViewModel
    {
        public List<Machine> machines;
        public List<Transporter> transporters;
        public EditorDataViewModel editorDataViewModel;

        public SessionDataViewModel(List<Machine> machines, List<Transporter> transporters, EditorDataViewModel editorDataViewModel)
        {
            this.machines = machines;
            this.transporters = transporters;
            this.editorDataViewModel = editorDataViewModel;
        }
    }
}
