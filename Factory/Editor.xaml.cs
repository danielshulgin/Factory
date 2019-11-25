using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Linq;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using Factory.DAL;

namespace Factory
{
    /// <summary>
    /// Interaction logic for Editor.xaml
    /// </summary>
    public partial class Editor : Window
    {
        public Editor()
        {
            InitializeComponent();
            var app = Application.Current;
            //MachineTypeCollection machineTypeCollection = (MachineTypeCollection)app.FindResource("MachineTypeCollection");
            //machineTypeCollection.lineItems = (EditorData)app.FindResource("EditorData").;
            //var expenseReport = (EditorData)app.FindResource("EditorData");
        }

        private void addExpenseButton_Click(object sender, RoutedEventArgs e)
        {
            var app = Application.Current;
            var expenseReport = (EditorData)app.FindResource("EditorData");
            expenseReport?.LineItems.Add(new DetailData());
        }

        private void Grid_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            DataGridRow row = ItemsControl.ContainerFromElement((DataGrid)sender, e.OriginalSource as DependencyObject) as DataGridRow;
        }

        private void RemoveDetailDataButton_Click(object sender, RoutedEventArgs e)
        {
            var app = Application.Current;
            object Name = ((Button)sender).CommandParameter;
            var expenseReport = (EditorData)app.FindResource("EditorData");
            expenseReport?.LineItems.Remove(Name.ToString());
        }

        private void addMachineDataButton_Click(object sender, RoutedEventArgs e)
        {
            var app = Application.Current;
            var expenseReport = (EditorData)app.FindResource("EditorData");
            expenseReport?.Machines.Add(new MachineData());
        }

        private void addDetailToMachineButton_Click(object sender, RoutedEventArgs e)
        {
            var app = Application.Current;
            var expenseReport = (EditorData)app.FindResource("EditorData");
            if (expenseReport?.SelectedMachine != null)
            {
                var dlg = new MachineDetailsEditor { Owner = this };
                dlg.Show();
            }
        }

        private void selectMachineButton_Click(object sender, RoutedEventArgs e)
        {
            var app = Application.Current;
            object Name = ((Button)sender).CommandParameter;
            var expenseReport = (EditorData)app.FindResource("EditorData");
            expenseReport.SelectedMachine = (expenseReport.Machines as ObservableCollection<MachineData>)
                .Where(i => i.Name == Name.ToString()).First();
            var dlg = new MachineDetailsEditor { Owner = this };
            dlg.Show();
        }

        private void RemoveMachineDataButton_Click(object sender, RoutedEventArgs e)
        {
            var app = Application.Current;
            object Name = ((Button)sender).CommandParameter;
            var expenseReport = (EditorData)app.FindResource("EditorData");
            expenseReport?.Machines.Remove(Name.ToString());
        }
    }
}
