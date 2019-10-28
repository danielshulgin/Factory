using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Factory2
{
    interface IMainFormState
    {
        void Activate(MainWindow mainWindow);
        void Deactivate();
        void Update();
    }
}
