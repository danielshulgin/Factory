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
        private EditorData _editorData;
        private EditorData EditorData{
            get
            {
                if (_editorData == null)
                {
                    _editorData = GetEditorData();
                }
                return _editorData;
            } 
        }
        public Editor()
        {
            InitializeComponent();
        }

        private void AddDetailTypeButton_Click(object sender, RoutedEventArgs e)
        {
            EditorData.DetailTypes.Add(new DetailData());
        }

        private void Grid_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            DataGridRow row = ItemsControl.ContainerFromElement((DataGrid)sender, e.OriginalSource as DependencyObject) as DataGridRow;
        }

        private void RemoveDetailDataButton_Click(object sender, RoutedEventArgs e)
        {
            object Name = ((Button)sender).CommandParameter;
            EditorData.DetailTypes.Remove(Name.ToString());
        }

        private void AddMachineDataButton_Click(object sender, RoutedEventArgs e)
        {
            EditorData.Machines.Add(new MachineData());
        }

        private EditorData GetEditorData()
        {
            var app = Application.Current;
            var editorData = (EditorData)app.FindResource("EditorData");
            return editorData;
        }

        private void AddDetailToMachineButton_Click(object sender, RoutedEventArgs e)
        {
            if (EditorData.SelectedMachine != null)
            {
                var dlg = new MachineDetailsEditor { Owner = this };
                dlg.Show();
            }
        }

        private void SelectMachineButton_Click(object sender, RoutedEventArgs e)
        {
            object Name = ((Button)sender).CommandParameter;
            EditorData.SelectedMachine = (EditorData.Machines as ObservableCollection<MachineData>)
                .Where(i => i.Name == Name.ToString()).First();
            var dlg = new MachineDetailsEditor { Owner = this };
            dlg.Show();
        }

        private void RemoveMachineDataButton_Click(object sender, RoutedEventArgs e)
        {
            object Name = ((Button)sender).CommandParameter;
            EditorData.Machines.Remove(Name.ToString());
        }


    }
}
