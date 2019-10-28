using Factory.Domain;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace Factory2
{
    class MainFormBuildState : IMainFormState
    {
        MainWindow mainWindow;
        Button currentButton;
        List<Button> applied;

        public void Activate(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            applied = new List<Button>();
            mainWindow.MouseDown += Form1_MouseClick;
            mainWindow.button1.Click += Button1_Click;
        }

        public void Deactivate()
        {
            mainWindow.button1.Click -= Button1_Click;
            mainWindow.MouseDown -= Form1_MouseClick;
            foreach (var button in applied)
            {
                mainWindow.automaticMachines.Add(button, new Machine(4f, true, new List<Detail>() { new Detail("TEST_DETAIL")}));
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
            if (currentButton == null)
            {
                currentButton = new Button();
                currentButton.Margin = new Thickness(0, 0, 0, 0);
                currentButton.Height = 40;
                currentButton.Width = 40;

                currentButton.Click += Form1_MouseClick;
                mainWindow.myCanvas.Children.Add(currentButton);
                applied.Add(currentButton);
            }
            else
            {
                DeleteCurrentButton();
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
