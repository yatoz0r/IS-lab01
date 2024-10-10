using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using IS_lab01.CryptLogics;

namespace Controllers
{
    public class ButtonsController
    {
        private ListBox ListBox1;
        private ListBox ListBox2;
        private RSA _rsa = new RSA();
        private DiffieHellman _diffhell = new DiffieHellman();
        private BlockEncrypt _blockEncrypt = new BlockEncrypt();
        public ButtonsController(ListBox  LB1, ListBox LB2) 
        { 
            this.ListBox1 = LB1;
            this.ListBox2 = LB2;
        }
        public void ButtonChooseFileLab2_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDlg = new OpenFileDialog();

            bool? result = openFileDlg.ShowDialog();
            if (result == true)
            {
                var input = File.ReadAllText(openFileDlg.FileName);
                string encryptedMessage = _rsa.Encrypt(input);
                string decryptedMessage = _rsa.Decrypt(encryptedMessage);
                var encryptedFilePath = System.IO.Path.ChangeExtension(openFileDlg.FileName, "_ENCRYPTRSA.txt");
                var decryptedFilePath = System.IO.Path.ChangeExtension(openFileDlg.FileName, "_DECRYPTRSA.txt");
                File.WriteAllText(encryptedFilePath, encryptedMessage);
                File.WriteAllText(decryptedFilePath, decryptedMessage);
                ListBox1.Items.Add($"P: {_rsa.P}");
                ListBox1.Items.Add($"Q: {_rsa.Q}");
                ListBox1.Items.Add($"Public key (e, n): ({_rsa.E}, {_rsa.N})");
                ListBox1.Items.Add($"Private key (d, y): ({_rsa.D}, {_rsa.Y})");
                ListBox1.Items.Add($"Encrypted file: {encryptedFilePath}");
                ListBox1.Items.Add($"Decrypted file: {decryptedFilePath}");
                ListBox2.Items.Add($"Encrypted text: {encryptedMessage}");
                ListBox2.Items.Add($"Decrypted text: {decryptedMessage}");
            }
        }

        public void ButtonChooseFileLab3_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDlg = new OpenFileDialog();

            bool? result = openFileDlg.ShowDialog();
            if (result == true)
            {
                var input = File.ReadAllText(openFileDlg.FileName);
                string encryptedMessage = _diffhell.Encrypt(input);
                string decryptedMessage = _diffhell.Decrypt(encryptedMessage);
                var encryptedFilePath = System.IO.Path.ChangeExtension(openFileDlg.FileName, "_ENCRYPTDIFFHELL.txt");
                var decryptedFilePath = System.IO.Path.ChangeExtension(openFileDlg.FileName, "_DECRYPTDIFFHELL.txt");
                File.WriteAllText(encryptedFilePath, encryptedMessage);
                File.WriteAllText(decryptedFilePath, decryptedMessage);
                ListBox1.Items.Add($"X: {_diffhell.X}");
                ListBox1.Items.Add($"Y: {_diffhell.Y}");
                ListBox1.Items.Add($"Public key A: ({_diffhell.PublicKeyA})");
                ListBox1.Items.Add($"Public key A: ({_diffhell.PublicKeyB})");
                ListBox1.Items.Add($"Private key: ({_diffhell.PrivateKey})");
                ListBox1.Items.Add($"Encrypted file: {encryptedFilePath}");
                ListBox1.Items.Add($"Decrypted file: {decryptedFilePath}");
                ListBox2.Items.Add($"Encrypted text: {encryptedMessage}");
                ListBox2.Items.Add($"Decrypted text: {decryptedMessage}");
            }
        }

        public void ButtonChooseFileLab4_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDlg = new OpenFileDialog();

            bool? result = openFileDlg.ShowDialog();
            if(result == true)
            {
                var input = File.ReadAllText(openFileDlg.FileName);
                _blockEncrypt.GenerateKey();
                string encryptedMessage = _blockEncrypt.Encrypt(input);
                string decryptedMessage = _blockEncrypt.Decrypt(encryptedMessage);
                var encryptedFilePath = System.IO.Path.ChangeExtension(openFileDlg.FileName, "_ENCRYPTBLOCK.bin");
                var decryptedFilePath = System.IO.Path.ChangeExtension(openFileDlg.FileName, "_DECRYPTBLOCK.bin");
                File.WriteAllText(encryptedFilePath, encryptedMessage);
                File.WriteAllText(decryptedFilePath, decryptedMessage);
                ListBox1.Items.Add($"Key(string): {_blockEncrypt.GetKey()}");
                ListBox1.Items.Add($"Key(Hex): {BitConverter.ToString(_blockEncrypt.Key).Replace("-", " ")}");
                ListBox1.Items.Add($"Key(bin): {_blockEncrypt.ByteArrayToBinaryString(_blockEncrypt.Key)}");
                ListBox1.Items.Add($"Encrypted file: {encryptedFilePath}");
                ListBox1.Items.Add($"Decrypted file: {decryptedFilePath}");
                ListBox2.Items.Add($"Encrypted text(string): {encryptedMessage}");
                ListBox2.Items.Add($"Encrypted text(Hex): {BitConverter.ToString(_blockEncrypt.Encrypted).Replace("-", " ")}");
                ListBox2.Items.Add($"Encrypted text(bin): {_blockEncrypt.ByteArrayToBinaryString(_blockEncrypt.Encrypted)}");
                ListBox2.Items.Add($"Decrypted text: {decryptedMessage}");
            }
        }
    }
}
