using System.Windows;
using Controllers;

namespace IS_lab01
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ButtonsController _buttonsController;
        private TextBoxController _textBoxController;
        public MainWindow()
        {
            InitializeComponent();
            var listKCeaser = new List<int>()
            {
                1, 2, 3, 4, 5, 6, 7, 8, 9, 10
            };
            _buttonsController = new ButtonsController(ListBoxLab2, ListBox2Lab2);
            _textBoxController = new TextBoxController(ComboBox1, InputTextBoxLab1, EncryptedString, DecryptedString);
            ComboBox1.ItemsSource = listKCeaser;
            InputTextBoxLab1.KeyDown += _textBoxController.InputTextBoxLab1KeyDown;
            ButtonChooseFileLab2.Click += _buttonsController.ButtonChooseFileLab2_Click;
        }
    }
}