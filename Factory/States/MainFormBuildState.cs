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
        System.Windows.Point lastPosition;
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
                //lastPosition = Mouse.GetPosition(mainWindow.myCanvas);
                currentButton.Margin = new Thickness(position.X, position.Y, 0, 0);
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (mainWindow.machineTypeComboBox.SelectedIndex != -1)
            {
                if (currentButton == null)
                {
                    currentButton = mainWindow.CreateButtonForMachine();
                    currentButton.Click += Form1_MouseClick;
                    
                    var machineData = (mainWindow.EditorData.Machines.Where(machine =>
                    machine.Name == mainWindow.machineTypeComboBox.SelectedItem.ToString()).First() as MachineViewModel);
                    var addDetails = machineData.details.Select(detail => new Detail(detail.Name, Guid.NewGuid())).ToList();
                    lastPosition = currentButton.TransformToAncestor(mainWindow)
                          .Transform(new System.Windows.Point(0, 0));
                    if (mainWindow.machineGeneralComboBox.SelectedIndex == 0)
                    {
                        applied[currentButton] = new MachineSource(machineData.TimeToCreateDetail, true, new List<Detail>(),
                            lastPosition.X, lastPosition.Y);
                    }
                    else
                    {
                        applied[currentButton] = new Machine(machineData.TimeToCreateDetail, true, addDetails, lastPosition.X, lastPosition.Y);
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
            if (currentButton != null)
            {
                lastPosition = currentButton.TransformToAncestor(mainWindow.myCanvas)
                          .Transform(new System.Windows.Point(0, 0));
                applied[(Button)sender].SetPosition(lastPosition.X, lastPosition.Y);
            }
            currentButton = null;
        }

    }
}
