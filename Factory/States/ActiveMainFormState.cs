﻿using Factory.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Factory
{
    public class ActiveMainFormState : IMainFormState
    {
        MainWindow mainWindow;
        private Dictionary<Button, EntityOnTransposter> entitiesButtons = new Dictionary<Button, EntityOnTransposter>();
        private MachineLine _machineLine;
        public void Activate(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            List<MachineBase> automaticMachines = mainWindow.automaticMachines.Select(pair => pair.Value as MachineBase).ToList();
            List<MachineBase> transporters = mainWindow.transporters.Select(pair => pair.Value as MachineBase).ToList();
            foreach (var t in transporters)
            {
                (t as Transporter).Reset();
            }
            List<MachineBase> machines = new List<MachineBase>();
            machines.Add(mainWindow.automaticMachines.FirstOrDefault().Value);
            for (int i = 0; i < automaticMachines.Count - 1; i++)
            {
                machines.Add(transporters[i]);
                machines.Add(automaticMachines[i + 1]);
            }
            _machineLine = new MachineLine(machines);
            foreach (var button in mainWindow.automaticMachines.Keys)
            {
                button.Click += OnMachineClick;
            }
        }

        public void Deactivate()
        {
            foreach (var button in mainWindow.automaticMachines.Keys)
            {
                button.Click -= OnMachineClick;
            }
        }

        public void Update()
        {
            //UpdateMachines();
            AddItemsOnTransporters();
            RemoveItemsFromTransporters();
            _machineLine.Update(DateTime.Now);
            UpdateItemsPositions();
        }

        private void RemoveItemsFromTransporters()
        {
            foreach (var entitieButtonPair in entitiesButtons)
            {
                bool existOnTransporter = false;
                foreach (var connection in mainWindow.transporters)
                {
                    if (connection.Value.EntitiesOnTransporter.Contains(entitieButtonPair.Value))
                    {
                        existOnTransporter = true;
                    }
                }
                if (!existOnTransporter)
                {
                    entitieButtonPair.Key.Click -= OnEntityClick;
                    mainWindow.myCanvas.Children.Remove(entitieButtonPair.Key);
                    entitiesButtons.Remove(entitieButtonPair.Key);
                }
            }
        }

        private void AddItemsOnTransporters()
        {
            var entitiesOnTransporterToAdd = new List<EntityOnTransposter>();
            foreach (var connection in mainWindow.transporters)
            {
                foreach (var entityOnTransposter in connection.Value.EntitiesOnTransporter)
                {
                    if (!entitiesButtons.Values.Contains(entityOnTransposter))
                    {
                        entitiesOnTransporterToAdd.Add(entityOnTransposter);
                    }
                }
            }

            foreach (var entityOnTransposter in entitiesOnTransporterToAdd)
            {
                AddItemButton(entityOnTransposter);
            }
        }

        private void UpdateItemsPositions()
        {
            foreach (var connection in mainWindow.transporters)
            {
                foreach (var entityOnTransporter in connection.Value.EntitiesOnTransporter)
                {
                    Button button = entitiesButtons.Keys.Where(b => entitiesButtons[b] == entityOnTransporter).FirstOrDefault();
                    if (button != null)
                    {
                        double x1 = connection.Key.X1;
                        double x2 = connection.Key.X2;
                        double y1 = connection.Key.Y1;
                        double y2 = connection.Key.Y2;
                        button.Margin = new Thickness(x1 + (x2 - x1) * entityOnTransporter.Position,
                            y1 + (y2 - y1) * entityOnTransporter.Position, 0, 0);
                    }
                    /*else
                    {
                        AddItemButton(entityOnTransporter);
                    }*/
                }
            }
        }
        //TODO machine sourse
        int count = 0;

        private void UpdateMachines()
        {
            if (mainWindow.automaticMachines.Count > 0)
            {
                /*if (mainWindow.automaticMachines.Values.First().CanHandle() && count <= 5)
                {
                    mainWindow.automaticMachines.Values.First().Accept(new Entity());
                    count++;
                }*/

                foreach (var machine in mainWindow.automaticMachines.Values)
                {
                    machine.Update(DateTime.Now);
                }
            }
        }

        private void AddItemButton(EntityOnTransposter entityOnTransposter)
        {
            ParserContext context = new ParserContext();
            context.XmlnsDictionary.Add("", "http://schemas.microsoft.com/winfx/2006/xaml/presentation");
            context.XmlnsDictionary.Add("x", "http://schemas.microsoft.com/winfx/2006/xaml");

            string xaml = String.Format(@"<Button  Width='25' Height='25'>
                <Button.Template>
                    <ControlTemplate TargetType = '{{x:Type Button}}'>
                        <Grid>
                            <Ellipse Fill = 'Red'/>
                            <ContentPresenter Content = '{{TemplateBinding Content}}' 
                                HorizontalAlignment = 'Center' VerticalAlignment = 'Center'/>
                        </Grid>
                    </ControlTemplate>
                </Button.Template>
            </Button>");
            Button button = (Button)(UIElement)XamlReader.Parse(xaml, context);
            button.Click += OnEntityClick;
            entitiesButtons[button] = entityOnTransposter;
            mainWindow.myCanvas.Children.Add(button);
        }

        private void OnEntityClick(object sender, RoutedEventArgs e)
        {
            Entity entity = entitiesButtons[(Button)sender].Entity;
            mainWindow.ActivateItemDialog("Entity", entity.DetailsNames);
        }

        public void OnMachineClick(object sender, EventArgs eventArgs)
        {
            Machine machine = mainWindow.automaticMachines[(Button)sender];
            var properties = new Dictionary<string, string>();
            properties["InProcess"] = machine.InProcessEntityNumber.ToString();
            properties["InQueue"] = machine.InQueueEntityNumber.ToString();
            mainWindow.ActivateItemDialog("Machine", properties);
        }
    }
}
