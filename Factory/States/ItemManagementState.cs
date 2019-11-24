using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using Factory.DAL;
using Factory.Domain;

namespace Factory
{
    class ItemManagementState : IMainFormState
    {
        MainWindow mainWindow;

        Dictionary<Button, DetailType> detailsTypesButtons;
        public void Activate(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            detailsTypesButtons = new Dictionary<Button, DetailType>();
            mainWindow.ItemManagementMenu.Visibility = System.Windows.Visibility.Visible;
            mainWindow.ItemManagementCloseButton.Click += Deactivate;
            mainWindow.detailButton.Click += OnDetailsButton;
            mainWindow.machineTypesButton.Click += OnMachineTypeButton;
            
        }

        public void Deactivate()
        {
            mainWindow.ItemManagementMenu.Visibility = System.Windows.Visibility.Hidden;
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

        public void ActivateDetailTypesDialog(object sender, RoutedEventArgs e)
        {
            mainWindow.ActivateItemDialog("Detail", new List<string>() { "Name" });
        }

        public void ActivateMachineTypesDialog(object sender, RoutedEventArgs e)
        {
            mainWindow.ActivateItemDialog("Detail", new List<string>() { "MachineName" });
        }

        public void OnDetailsButton(object sender, RoutedEventArgs e)
        {
            mainWindow.AddTypeButton.Click += ActivateDetailTypesDialog;
            mainWindow.AddTypeButton.Click -= ActivateMachineTypesDialog;
            foreach (var detailType in mainWindow.database.GetAllDetailsTypes())
            {
                Button btn = new Button();
                btn.Content = detailType.Name;
                btn.Height = 20;
                btn.Background = System.Windows.Media.Brushes.Azure;
                detailsTypesButtons[btn] = detailType;
                mainWindow.itemManagementMenuStackPanel.Children.Add(btn);
            }
        }

        public void OnMachineTypeButton(object sender, RoutedEventArgs e)
        {
            mainWindow.AddTypeButton.Click += ActivateMachineTypesDialog;
            mainWindow.AddTypeButton.Click -= ActivateDetailTypesDialog;
            foreach (var detailType in mainWindow.database.GetAllDetailsTypes())
            {
                Button btn = new Button();
                btn.Content = detailType.Name;
                btn.Height = 20;
                btn.Background = System.Windows.Media.Brushes.Azure;
                detailsTypesButtons[btn] = detailType;
                mainWindow.itemManagementMenuStackPanel.Children.Add(btn);
            }
        }
    }
}
