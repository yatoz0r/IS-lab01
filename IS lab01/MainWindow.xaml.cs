using System.Windows;
using Controllers;

namespace IS_lab01
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ButtonsController _buttonsControllerLab2;
        private ButtonsController _buttonsControllerLab3;
        private ButtonsController _buttonsControllerLab4;
        private ButtonsController _buttonsControllerLab5;
        private ButtonsController _buttonsControllerLab6;
        private TextBoxController _textBoxController;
        public MainWindow()
        {
            InitializeComponent();
            var listKCeaser = new List<int>()
            {
                1, 2, 3, 4, 5, 6, 7, 8, 9, 10
            };
            _buttonsControllerLab2 = new ButtonsController(ListBoxLab2, ListBox2Lab2);
            _buttonsControllerLab3 = new ButtonsController(ListBoxLab3, ListBox2Lab3);
            _buttonsControllerLab4 = new ButtonsController(ListBoxLab4, ListBox2Lab4);
            _buttonsControllerLab5 = new ButtonsController(RichTextBox1);
            _buttonsControllerLab6 = new ButtonsController(ListBoxLab6);
            _textBoxController = new TextBoxController(ComboBox1, InputTextBoxLab1, EncryptedString, DecryptedString);
            ComboBox1.ItemsSource = listKCeaser;
            InputTextBoxLab1.KeyDown += _textBoxController.InputTextBoxLab1KeyDown;
            ButtonChooseFileLab2.Click += _buttonsControllerLab2.ButtonChooseFileLab2_Click;
            ButtonChooseFileLab3.Click += _buttonsControllerLab3.ButtonChooseFileLab3_Click;
            ButtonChooseFileLab4.Click += _buttonsControllerLab4.ButtonChooseFileLab4_Click;
            ButtonToExecuteLab5.Click += _buttonsControllerLab5.ButtonToExecuteLab5_Click;
            ButtonToExecuteLab6.Click += _buttonsControllerLab6.ButtonToExecuteLab6_Click;
        }
    }
}