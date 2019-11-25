using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Shapes;
using System.Windows.Threading;
using Factory.Domain;
using Factory.DAL;

namespace Factory
{
    public partial class MainWindow : Window
    {
        private IMainFormState _mainFormState;
        public Dictionary<Button, Machine> automaticMachines;
        public Dictionary<Line, Transporter> transporters;
        private DispatcherTimer _timer;
        private Dictionary<Button, EntityOnTransposter> _entitiesButtons;
        public Database database;

        public MainWindow()
        {
            InitializeComponent();

            ItemDialogCloseButton.Click += DeactivateItemDialog;
            automaticMachines = new Dictionary<Button, Machine>();
            transporters = new Dictionary<Line, Transporter>();
            _entitiesButtons = new Dictionary<Button, EntityOnTransposter>();

            database = new Database();

            ChangeMode(new ItemManagementState());
            StartStateUpdate();
            comboBox.SelectedIndex = 0;
        }

        private void StartStateUpdate()
        {
            _timer = new DispatcherTimer();
            _timer.Interval = new TimeSpan(10);
            _timer.Tick += new EventHandler(Update);
            _timer.Start();
        }
        private void Update(object o, EventArgs e)
        {
            _mainFormState.Update();
        }

        private void ItemManagementButton_Click(object sender, RoutedEventArgs e)
        {
            var app = Application.Current;
            var expenseReport = (EditorData)app.FindResource("EditorData");

            expenseReport.DetailTypes.Add(new DetailData() { Name = "TOP SECRET"});

            //ChangeMode(new ItemManagementState());
            var dlg = new Editor { Owner = this };
            dlg.Show();
            dlg.Closed += (d, b) => DebugCollections();
        }

        private void DebugCollections()
        {
            var app = Application.Current;
            var expenseReport = (EditorData)app.FindResource("EditorData");
            List<DetailType> detailTypes= new List<DetailType>();
            foreach (var item in expenseReport.DetailTypes)
            {
                detailTypes.Add(new DetailType(item.Name));
            }
            
        }


        private void ChangeMode(IMainFormState state)
        {
            if (_mainFormState != null)
            {
                _mainFormState.Deactivate();
            }
            _mainFormState = state;
            state.Activate(this);
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (comboBox.SelectedIndex)
            {
                case 0:
                    ChangeMode(new MainFormBuildState());
                    break;
                case 1:
                    ChangeMode(new MainFormConnectState());
                    break;
                case 2:
                    ChangeMode(new ActiveMainFormState());
                    break;

            }
        }

        public void ActivateItemDialog(string typeName, List<string> properties)
        {
            ItemDialog.Visibility = System.Windows.Visibility.Visible;
            ItemDialogItemTypeName.Content = typeName;
            ItemDialogItemPropertiesV.Children.Clear();
            for (int i = 0; i < properties.Count; i++)
            {
                ParserContext context = new ParserContext();
                context.XmlnsDictionary.Add("", "http://schemas.microsoft.com/winfx/2006/xaml/presentation");
                context.XmlnsDictionary.Add("x", "http://schemas.microsoft.com/winfx/2006/xaml");

                string xaml = String.Format(@"<StackPanel Background='Azure' Orientation ='Horizontal'>
                        <Border BorderThickness = '2' BorderBrush = 'LightSteelBlue'>
                            <Label Content = '{1}' Width = '72'/>
                        </Border>
                        <Border BorderThickness = '2' BorderBrush = 'LightSteelBlue'>
                            <TextBox Name = 'Property{0}' Width = '136'/>
                        </Border>
                    </StackPanel> ", i, properties[i]);
                UIElement element = (UIElement)XamlReader.Parse(xaml, context);
                ItemDialogItemPropertiesV.Children.Add(element);
            }
        }

        public void ActivateItemDialog(string typeName, Dictionary<string, string> properties)
        {
            ItemDialog.Visibility = System.Windows.Visibility.Visible;
            ItemDialogItemTypeName.Content = typeName;
            ItemDialogItemPropertiesV.Children.Clear();
            for (int i = 0; i < properties.Count; i++)
            {
                ParserContext context = new ParserContext();
                context.XmlnsDictionary.Add("", "http://schemas.microsoft.com/winfx/2006/xaml/presentation");
                context.XmlnsDictionary.Add("x", "http://schemas.microsoft.com/winfx/2006/xaml");

                string xaml = String.Format(@"<StackPanel Background='Azure' Orientation ='Horizontal'>
                        <Border BorderThickness = '2' BorderBrush = 'LightSteelBlue'>
                            <Label Content = '{1}' Width = '72'/>
                        </Border>
                        <Border BorderThickness = '2' BorderBrush = 'LightSteelBlue'>
                            <TextBox Name = 'Property{0}' Text = '{2}' Width = '136'/>
                        </Border>
                    </StackPanel> ", i, properties.ElementAt(i).Key, properties.ElementAt(i).Value);
                UIElement element = (UIElement)XamlReader.Parse(xaml, context);
                ItemDialogItemPropertiesV.Children.Add(element);
            }
        }

        public void DeactivateItemDialog(object sender, RoutedEventArgs e)
        {
            ItemDialog.Visibility = System.Windows.Visibility.Hidden;
        }
    }
}
