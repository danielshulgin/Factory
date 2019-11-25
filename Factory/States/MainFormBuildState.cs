using Factory.Domain;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
using System.Windows.Input;
using System.Collections.ObjectModel;
using Factory.DAL;

namespace Factory
{
    class MainFormBuildState : IMainFormState
    {
        MainWindow mainWindow;
        Button currentButton;
        Dictionary<Button, Machine> applied;

        public void Activate(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            applied = new Dictionary<Button, Machine>();
            mainWindow.MouseDown += Form1_MouseClick;
            mainWindow.button1.Click += Button1_Click;
        }

        public void Deactivate()
        {
            mainWindow.button1.Click -= Button1_Click;
            mainWindow.MouseDown -= Form1_MouseClick;
            foreach (var button in applied.Keys)
            {
                
                mainWindow.automaticMachines.Add(button, applied[button]);
            }
        }

        public void Update()
        {
            if (currentButton != null)
            {
                System.Windows.Point position = Mouse.GetPosition(mainWindow.myCanvas);
                currentButton.Margin = new Thickness(position.X, position.Y, 0, 0);
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (mainWindow.machineTypeComboBox.SelectedIndex != -1)
            {
                if (currentButton == null)
                {
                    currentButton = new Button();
                    currentButton.Margin = new Thickness(0, 0, 0, 0);
                    currentButton.Height = 40;
                    currentButton.Width = 40;

                    currentButton.Click += Form1_MouseClick;
                    mainWindow.myCanvas.Children.Add(currentButton);
                    var machineData = (mainWindow.EditorData.Machines.Where(machine => 
                    machine.Name == mainWindow.machineTypeComboBox.SelectedItem.ToString()).First() as MachineData);
                    var addDetails = machineData.Select(detail => new Detail(detail.Name, Guid.NewGuid())).ToList();
                    if (mainWindow.machineGeneralComboBox.SelectedIndex == 0)
                    {
                        applied[currentButton] = new MachineSource(machineData.TimeToCreateDetail, true, new List<Detail>());
                    }
                    else
                    {
                        applied[currentButton] = new Machine(machineData.TimeToCreateDetail, true, addDetails);
                    }
                }
                else
                {
                    DeleteCurrentButton();
                }
            }
        }

        private void DeleteCurrentButton()
        {
            mainWindow.myCanvas.Children.Remove(currentButton);
            applied.Remove(currentButton);
            
            currentButton = null;
        }

        private void Form1_MouseClick(object sender, EventArgs e)
        {
            currentButton = null;
        }

    }
}
