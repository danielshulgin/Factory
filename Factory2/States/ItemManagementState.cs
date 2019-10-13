using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
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
            
        }

        public void Deactivate()
        {
            mainWindow.ItemManagementMenu.Visibility = System.Windows.Visibility.Hidden;
            mainWindow.AddDetailButton.Click -= ActivateDialog;
            mainWindow.ItemManagementCloseButton.Click -= Deactivate;
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
    }
}
