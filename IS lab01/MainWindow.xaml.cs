using Microsoft.Win32;
using System.Formats.Tar;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IS_lab01
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CryptLogic _cryptLogic = new CryptLogic();
        public MainWindow()
        {
            InitializeComponent();
            var listKCeaser = new List<int>()
            {
                1, 2, 3, 4, 5, 6, 7, 8, 9, 10
            };
            ComboBox1.ItemsSource = listKCeaser;
            InputTextBoxLab1.KeyDown += InputTextBoxLab1KeyDown;
            ButtonChooseFileLab2.Click += ButtonChooseFileLab2_Click;
        }

        private void ButtonChooseFileLab2_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDlg = new OpenFileDialog();

            Nullable<bool> result = openFileDlg.ShowDialog();
            if (result == true)
            {
                var input = File.ReadAllText(openFileDlg.FileName);
                string encryptedMessage = _cryptLogic.Encrypt(input);
                string decryptedMessage = _cryptLogic.Decrypt(encryptedMessage);
                var encryptedFilePath = System.IO.Path.ChangeExtension(openFileDlg.FileName, "_ENCRYPT.txt");
                var decryptedFilePath = System.IO.Path.ChangeExtension(openFileDlg.FileName, "_DECRYPT.txt");
                File.WriteAllText(decryptedFilePath, decryptedMessage);
                ListBoxLab2.Items.Add($"P: {_cryptLogic.P}");
                ListBoxLab2.Items.Add($"Q: {_cryptLogic.Q}");
                ListBoxLab2.Items.Add($"Public key (e, n): ({_cryptLogic.E}, {_cryptLogic.N})");
                ListBoxLab2.Items.Add($"Private key (d, y): ({_cryptLogic.D}, {_cryptLogic.Y})");
                ListBoxLab2.Items.Add($"Encrypted file: {encryptedFilePath}");
                ListBoxLab2.Items.Add($"Decrypted file: {decryptedFilePath}");
                ListBox2Lab2.Items.Add($"Encrypted text: {encryptedMessage}");
                ListBox2Lab2.Items.Add($"Decrypted text: {decryptedMessage}");
            }
        
        }
        private void InputTextBoxLab1KeyDown(object sender, KeyEventArgs key)
        {
            if (key.Key == Key.Enter)
            {
                var keyComboBox = (int)ComboBox1.SelectedItem;
                var stringTextBox = InputTextBoxLab1.Text;
                EncryptedString.Text = _cryptLogic.CaesarCipher(stringTextBox, keyComboBox);
                DecryptedString.Text = _cryptLogic.CaesarCipher(EncryptedString.Text, -keyComboBox);
            }
        }
    }
}