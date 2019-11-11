using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Factory2
{
    class ItemManagementState : IMainFormState
    {
        MainWindow mainWindow;
        public void Activate(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            mainWindow.ItemManagementMenu.Visibility = System.Windows.Visibility.Visible;
            mainWindow.AddDetailButton.Click += ActivateDialog;
            mainWindow.ItemManagementCloseButton.Click += Deactivate;
            mainWindow.detailButton.Click += OnDetailsButton;
            mainWindow.machineTypesButton.Click += OnMachineTypeButton;
            
        }

        public void Deactivate()
        {
            mainWindow.ItemManagementMenu.Visibility = System.Windows.Visibility.Hidden;
            mainWindow.AddDetailButton.Click -= ActivateDialog;
            mainWindow.ItemManagementCloseButton.Click -= Deactivate;
            mainWindow.detailButton.Click -= OnDetailsButton;
            mainWindow.machineTypesButton.Click -= OnMachineTypeButton;
            mainWindow.DeactivateItemDialog(null, null);
        }

        public void Deactivate(object sender, RoutedEventArgs e)
        {
            Deactivate();
        }

        public void Update()
        {

        }

        public void ActivateDialog(object sender, RoutedEventArgs e)
        {
            mainWindow.ActivateItemDialog("Detail", new List<string>() { "Weight", "Size", "Id"});
        }

        public void OnDetailsButton(object sender, RoutedEventArgs e)
        {
            //for (int i = 0; i < length; i++)
            //{

            //}
            Button btn = new Button();
            btn.Content = "Dynamic Button";
            btn.Height = 20;
            btn.Background = System.Windows.Media.Brushes.Azure;
            //btn.Name = "detailsButton" + i;
            mainWindow.itemManagementMenuStackPanel.Children.Add(btn);
        }

        public void OnMachineTypeButton(object sender, RoutedEventArgs e)
        {
            mainWindow.itemManagementMenuStackPanel.Children.Clear();
        }
    }
}
