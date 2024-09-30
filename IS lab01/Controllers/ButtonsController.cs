using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using CryptLogic;

namespace Controllers
{
    public class ButtonsController
    {
        private ListBox ListBoxLab2;
        private ListBox ListBox2Lab2;
        private RSA _rsa = new RSA();
        public ButtonsController(ListBox  ListBox1, ListBox ListBox2) 
        { 
            ListBoxLab2 = ListBox1;
            ListBox2Lab2 = ListBox2;
        }
        public void ButtonChooseFileLab2_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDlg = new OpenFileDialog();

            Nullable<bool> result = openFileDlg.ShowDialog();
            if (result == true)
            {
                var input = File.ReadAllText(openFileDlg.FileName);
                string encryptedMessage = _rsa.Encrypt(input);
                string decryptedMessage = _rsa.Decrypt(encryptedMessage);
                var encryptedFilePath = System.IO.Path.ChangeExtension(openFileDlg.FileName, "_ENCRYPT.txt");
                var decryptedFilePath = System.IO.Path.ChangeExtension(openFileDlg.FileName, "_DECRYPT.txt");
                File.WriteAllText(decryptedFilePath, decryptedMessage);
                ListBoxLab2.Items.Add($"P: {_rsa.P}");
                ListBoxLab2.Items.Add($"Q: {_rsa.Q}");
                ListBoxLab2.Items.Add($"Public key (e, n): ({_rsa.E}, {_rsa.N})");
                ListBoxLab2.Items.Add($"Private key (d, y): ({_rsa.D}, {_rsa.Y})");
                ListBoxLab2.Items.Add($"Encrypted file: {encryptedFilePath}");
                ListBoxLab2.Items.Add($"Decrypted file: {decryptedFilePath}");
                ListBox2Lab2.Items.Add($"Encrypted text: {encryptedMessage}");
                ListBox2Lab2.Items.Add($"Decrypted text: {decryptedMessage}");
            }

        }
    }
}
