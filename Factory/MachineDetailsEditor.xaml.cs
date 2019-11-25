using Factory.DAL;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Factory
{
    /// <summary>
    /// Interaction logic for MachineDetailsEditor.xaml
    /// </summary>
    public partial class MachineDetailsEditor : Window
    {
        public MachineDetailsEditor()
        {
            InitializeComponent();
        }

        private void AddDetailToMachineButton_Click(object sender, RoutedEventArgs e)
        {
            var app = Application.Current;
            var expenseReport = (EditorData)app.FindResource("EditorData");
            expenseReport?.SelectedMachine.Add(new DetailInMachine());
        }
    }
}
