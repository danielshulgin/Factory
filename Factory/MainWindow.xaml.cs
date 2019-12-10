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
using System.Windows.Media;
using Newtonsoft.Json;
using System.IO;

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

        private EditorDataViewModel _editorData;

        public EditorDataViewModel EditorData
        {
            get
            {
                if (_editorData == null)
                {
                    _editorData = GetEditorData();
                }
                return _editorData;
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            ItemDialogCloseButton.Click += DeactivateItemDialog;
            automaticMachines = new Dictionary<Button, Machine>();
            transporters = new Dictionary<Line, Transporter>();
            _entitiesButtons = new Dictionary<Button, EntityOnTransposter>();

            database = new Database();

            StartStateUpdate();
            comboBox.SelectedIndex = 0;
            machineGeneralComboBox.ItemsSource = Enum.GetValues(typeof(MachineType));
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
            var dlg = new Editor { Owner = this };
            dlg.Show();
            dlg.Closed += (d, b) => ApplyEditorData();
        }

        private void ApplyEditorData()
        {
            var app = Application.Current;
            var expenseReport = (EditorDataViewModel)app.FindResource("EditorData");
            List<DetailType> detailTypes= new List<DetailType>();
            foreach (var item in expenseReport.Machines)
            {
                detailTypes.Add(new DetailType(item.Name));
            }
            machineTypeComboBox.ItemsSource = expenseReport.Machines.Select(machine => machine.Name).ToList();
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

        public Button CreateButtonForMachine(double x = 0d, double y = 0d)
        {
            var button = new Button();
            button.Margin = new Thickness(x, y, 0, 0);
            button.Height = 40;
            button.Width = 40;
            myCanvas.Children.Add(button);
            return button;
        }

        public Transporter CreateTransporter(double x1, double y1, double x2, double y2)
        {
            Transporter transporter = new Transporter(6f, 2f, true, (x1 + x2) / 2, (x1 + x2) / 2, x1, y1, x2, y2);
            CreateTransporterRenderer(x1, y1, x2, y2, transporter);
            return transporter;
        }

        public void CreateTransporterRenderer(double x1, double y1, double x2, double y2, Transporter transporter)
        {
            Line line = new Line();
            line.Stroke = Brushes.LightSteelBlue;
            line.X1 = (int)x1;
            line.Y1 = (int)y1;
            line.X2 = (int)x2;
            line.Y2 = (int)y2;
            line.StrokeThickness = 2;
            transporters.Add(line, transporter);
            myCanvas.Children.Add(line);
        }
        public void Save(object sender, RoutedEventArgs e)
        {
            var machines = automaticMachines.Values.ToList();
            var transportersdata = transporters.Values.ToList();
            var db = new Database();
            db.transporters = transportersdata;
            db.machines = machines;
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            };
            string machinesJson = JsonConvert.SerializeObject(db, settings);
            string File_name = "E:\\Labas\\db.json";
            if (System.IO.File.Exists(File_name) == true)
            {
                FileStream fs = new FileStream(File_name, FileMode.Create, FileAccess.Write);
                StreamWriter objWrite = new StreamWriter(fs);
                objWrite.Write(machinesJson);
                objWrite.Close();
            }
            else
            {
                FileStream fs = new FileStream(File_name, FileMode.Create, FileAccess.Write);
                StreamWriter objWrite = new StreamWriter(fs);
                objWrite.Write(machinesJson);
                objWrite.Close();
            }
            //Load(null, null);
        }
        public void Load(object sender, RoutedEventArgs e)
        {
            automaticMachines = new Dictionary<Button, Machine>();
            transporters = new Dictionary<Line, Transporter>();
            string File_name = "E:\\Labas\\db.json";
            string transportersJson = "";
            if (System.IO.File.Exists(File_name) == true)
            {
                System.IO.StreamReader objReader;
                objReader = new StreamReader(File_name);
                transportersJson = objReader.ReadToEnd();
                objReader.Close();
            }
            else
            {
                MessageBox.Show("File Not Exist");
            }
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            };
            var transportersLoaded = JsonConvert.DeserializeObject<Database>(transportersJson, settings);
            Load(transportersLoaded.machines, transportersLoaded.transporters);
        }
        public void Load(List<Machine> machines, List<Transporter> transporters)
        {
            foreach (var machine in machines)
            {
                var button = CreateButtonForMachine(machine.X, machine.Y);
                //automaticMachines[button] = machine;
                automaticMachines.Add(button, machine);
            }

            foreach (var transporter in transporters)
            {
                CreateTransporterRenderer(transporter.X1, transporter.Y1, transporter.X2, transporter.Y2, transporter);
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

        private EditorDataViewModel GetEditorData()
        {
            var app = Application.Current;
            var editorData = (EditorDataViewModel)app.FindResource("EditorData");
            return editorData;
        }

        public void DeactivateItemDialog(object sender, RoutedEventArgs e)
        {
            ItemDialog.Visibility = System.Windows.Visibility.Hidden;
        }
    }

    public enum MachineType
    {
        Source, Base
    }
}
