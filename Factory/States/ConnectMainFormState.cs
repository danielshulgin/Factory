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
                //Console.Write("OK");
                Transporter transporter = new Transporter(6f, 2f, true);
                //TODO
                Line line = new Line();

                line.Stroke = Brushes.LightSteelBlue;

                line.X1 = (int)first.Margin.Left;
                line.Y1 = (int)first.Margin.Top;
                line.X2 = (int)second.Margin.Left;
                line.Y2 = (int)second.Margin.Top;
                line.StrokeThickness = 2;

                mainWindow.transporters.Add(line, transporter);
                mainWindow.myCanvas.Children.Add(line);

                //mainWindow.automaticMachines[first].TryConnectOutput(transporter);
                //transporter.TryConnectInput(mainWindow.automaticMachines[first]);

                //mainWindow.automaticMachines[second].TryConnectInput(transporter);
                //transporter.TryConnectOutput(mainWindow.automaticMachines[second]);

                first = null;
                second = null;
            }
        }

        public void Update()
        {

        }
    }
}
