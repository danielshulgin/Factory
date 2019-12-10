using Factory.Domain;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Media;

namespace Factory
{
    class MainFormConnectState : IMainFormState
    {
        private Button first;
        private Button second;
        private MainWindow mainWindow;
        public List<EventHandler> eventHandlers = new List<EventHandler>();

        public void Activate(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            foreach (var button in mainWindow.automaticMachines.Keys)
            {
                EventHandler eventHandler = new EventHandler(OnButtonClick);
                button.Click += OnButtonClick;
                eventHandlers.Add(eventHandler);
            }
        }

        public void Deactivate()
        {
            foreach (var button in mainWindow.automaticMachines.Keys)
            {
                button.Click -= OnButtonClick;
            }
        }

        public void OnButtonClick(object o, EventArgs e)
        {
            Button button = (Button)o;
            if (first == null)
            {
                first = button;
            }
            else if (first != null && second == null)
            {
                second = button;
                mainWindow.CreateTransporter(first.Margin.Left, first.Margin.Top, second.Margin.Left, second.Margin.Top);
                first = null;
                second = null;
            }
        }

        public void Update()
        {

        }
    }
}
