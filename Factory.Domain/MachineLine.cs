using System;
using System.Collections.Generic;
using System.Text;

namespace Factory.Domain
{
    public class MachineLine
    {
        private List<Machine> _machines;
        private List<Transporter> _transporters;

        public IReadOnlyCollection<Machine> Machines => _machines;
        public IReadOnlyCollection<Machine> Transporter => _transporters;

        public MachineLine(List<Machine> machines, List<Transporter> transporters)
        {
            this._machines = machines;
            this._transporters = transporters;
        }

        public void Optimaize()
        {

        }
    }
}
