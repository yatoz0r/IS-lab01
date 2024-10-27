using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using IS_lab01.CryptLogics;

namespace Controllers
{
    public class ButtonsController
    {
        private readonly ListBox? ListBox1;
        private readonly ListBox? ListBox2;
        private readonly RichTextBox? _richTextBox1;
        private readonly TextBox? _textBox1;
        private readonly TextBox? _textBox2;
        private readonly TextBox? _textBox3;
        private readonly TextBox? _textBox4;
        private readonly RSA _rsa = new RSA();
        private readonly DiffieHellman _diffhell = new DiffieHellman();
        private readonly BlockEncrypt _blockEncrypt = new BlockEncrypt();
        private readonly ErrorCorrection _errorCorrection = new ErrorCorrection();
        private readonly RLE _rle = new RLE();
        private readonly Huffman _huffman = new Huffman();
        private readonly PasswordHasher _hasher = new PasswordHasher();
        private readonly GammaEncryption _gamma = new GammaEncryption();

        public ButtonsController(RichTextBox rtb1) => this._richTextBox1 = rtb1;

        public ButtonsController(TextBox tb1) => this._textBox1 = tb1;

        public ButtonsController(ListBox lb1) => this.ListBox1 = lb1;

        public ButtonsController(ListBox  lb1, ListBox lb2) 
        { 
            this.ListBox1 = lb1;
            this.ListBox2 = lb2;
        }

        public ButtonsController(TextBox tb1, TextBox tb2, TextBox tb3, TextBox tb4)
        {
            this._textBox1 = tb1;
            this._textBox2 = tb2;
            this._textBox3 = tb3;
            this._textBox4 = tb4;
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
                ListBox1?.Items.Add($"P: {_rsa.P}");
                ListBox1?.Items.Add($"Q: {_rsa.Q}");
                ListBox1?.Items.Add($"Public key (e, n): ({_rsa.E}, {_rsa.N})");
                ListBox1?.Items.Add($"Private key (d, y): ({_rsa.D}, {_rsa.Y})");
                ListBox1?.Items.Add($"Encrypted file: {encryptedFilePath}");
                ListBox1?.Items.Add($"Decrypted file: {decryptedFilePath}");
                ListBox2?.Items.Add($"Encrypted text: {encryptedMessage}");
                ListBox2?.Items.Add($"Decrypted text: {decryptedMessage}");
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
                ListBox1?.Items.Add($"N: {_diffhell.N}");
                ListBox1?.Items.Add($"Q: {_diffhell.Q}");
                ListBox1?.Items.Add($"Public key A: ({_diffhell.PublicKeyA})");
                ListBox1?.Items.Add($"Public key B: ({_diffhell.PublicKeyB})");
                ListBox1?.Items.Add($"X: ({_diffhell.X})");
                ListBox1?.Items.Add($"Y: ({_diffhell.Y})");
                ListBox1?.Items.Add($"Client Private A - {_diffhell.PrivateKeyA}");
                ListBox1?.Items.Add($"Client Private B - {_diffhell.PrivateKeyB}");
                ListBox1?.Items.Add($"Private key: ({_diffhell.PrivateKey})");
                ListBox1?.Items.Add($"Encrypted file: {encryptedFilePath}");
                ListBox1?.Items.Add($"Decrypted file: {decryptedFilePath}");
                ListBox2?.Items.Add($"Encrypted text: {encryptedMessage}");
                ListBox2?.Items.Add($"Decrypted text: {decryptedMessage}");
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
                ListBox1?.Items.Add($"Key(string): {_blockEncrypt.GetKey()}");
                ListBox1?.Items.Add($"Key(Hex): {BitConverter.ToString(_blockEncrypt.Key).Replace("-", " ")}");
                ListBox1?.Items.Add($"Key(bin): {_blockEncrypt.ByteArrayToBinaryString(_blockEncrypt.Key)}");
                ListBox1?.Items.Add($"Encrypted file: {encryptedFilePath}");
                ListBox1?.Items.Add($"Decrypted file: {decryptedFilePath}");
                ListBox2?.Items.Add($"Encrypted text(string): {encryptedMessage}");
                ListBox2?.Items.Add($"Encrypted text(Hex): {BitConverter.ToString(_blockEncrypt.Encrypted).Replace("-", " ")}");
                ListBox2?.Items.Add($"Encrypted text(bin): {_blockEncrypt.ByteArrayToBinaryString(_blockEncrypt.Encrypted)}");
                ListBox2?.Items.Add($"Decrypted text: {decryptedMessage}");
            }
        }

        public void ButtonToExecuteLab5_Click(object sender, RoutedEventArgs e)
        {
            int numberOfTests = 10; // Number of random tests
            for (int i = 0; i < numberOfTests; i++)
            {
                int data = _errorCorrection.GenerateRandomData(6); // Generate random 6-bit data
                int encoded = _errorCorrection.Encode(data);
                _richTextBox1?.AppendText($"Original data: {Convert.ToString(data, 2)
                    .PadLeft(6, '0')}" + Environment.NewLine);
                _richTextBox1?.AppendText($"Encoded data: {Convert.ToString(encoded, 2)
                    .PadLeft(_errorCorrection.CodeLength, '0')}" + Environment.NewLine);

                // Randomly decide whether to introduce an error
                if (_errorCorrection.Random.Next(2) == 0) // 50% chance to introduce an error
                {
                    int received = _errorCorrection.IntroduceRandomError(encoded);
                    _richTextBox1?.AppendText($"Received data with error: {Convert.ToString(received, 2)
                        .PadLeft(_errorCorrection.CodeLength, '0')}" + Environment.NewLine);
                    DecodeResult result = _errorCorrection.Decode(received);
                    _richTextBox1?.AppendText(result.Message + Environment.NewLine);
                    _richTextBox1?.AppendText($"Corrected data: {Convert.ToString(result.CorrectedData, 2)
                        .PadLeft(6, '0')}" + Environment.NewLine);
                }
                else
                {
                    _richTextBox1?.AppendText($"Received data without error: {Convert.ToString(encoded, 2)
                        .PadLeft(_errorCorrection.CodeLength, '0')}" + Environment.NewLine);
                    DecodeResult result = _errorCorrection.Decode(encoded);
                    _richTextBox1?.AppendText(result.Message + Environment.NewLine);
                    _richTextBox1?.AppendText($"Original data: {Convert.ToString(result.CorrectedData, 2)
                        .PadLeft(6, '0')}" + Environment.NewLine);
                }
                _richTextBox1?.AppendText(Environment.NewLine);
            }
        }

        public void ButtonToExecuteLab6_Click(object sender, RoutedEventArgs e)
        {
            string inputFile = "C:\\Users\\yatoz\\source\\repos\\IS lab01\\text files\\123.txt";
            string compressedFile = "C:\\Users\\yatoz\\source\\repos\\IS lab01\\text files\\output_rlehuff.txt";
            string decompressedFile = "C:\\Users\\yatoz\\source\\repos\\IS lab01\\text files\\decompressed.txt";

            Console.WriteLine("Чтение исходного файла...");
            byte[] originalData = File.ReadAllBytes(inputFile);
            ListBox1?.Items.Add($"Размер исходного файла: {_huffman.FormatFileSize(originalData.Length)}");
            
            ListBox1?.Items.Add("\n--- Этап 1: RLE сжатие ---");
            byte[] rleCompressed = _rle.RLECompress(originalData);
            ListBox1?.Items.Add("Файл успешно сжат");

            ListBox1?.Items.Add("\n--- Этап 2: Сжатие Хаффмана ---");
            byte[] huffmanCompressed = _huffman.HuffmanCompress(rleCompressed);
            File.WriteAllBytes(compressedFile, huffmanCompressed);
            ListBox1?.Items.Add($"Финальный размер сжатого файла: {_huffman.FormatFileSize(huffmanCompressed.Length)}");
            ListBox1?.Items.Add($"Общая степень сжатия: {(double)originalData.Length / huffmanCompressed.Length:F2}x");

            ListBox1?.Items.Add("\n--- Этап 3: Расжатие Хаффмана ---");
            byte[] huffmanDecompressed = _huffman.HuffmanDecompress(huffmanCompressed);
            ListBox1?.Items.Add($"Файл расжат без ошибок");

            ListBox1?.Items.Add("\n--- Этап 4: RLE расжатие ---");
            byte[] finalDecompressed = _rle.RLEDecompress(huffmanDecompressed);
            File.WriteAllBytes(decompressedFile, finalDecompressed);
            ListBox1?.Items.Add($"Финальный размер расжатого файла: {_huffman.FormatFileSize(finalDecompressed.Length)}");

            bool success = _huffman.CompareFiles(originalData, finalDecompressed);
            ListBox1?.Items.Add("\n=== Результаты ===");
            ListBox1?.Items.Add($"Сжатие и расжатие выполнено {(success ? "успешно" : "с ошибками")}");
            ListBox1?.Items.Add($"Исходный файл: {inputFile}");
            ListBox1?.Items.Add($"Сжатый файл: {compressedFile}");
            ListBox1?.Items.Add($"Расжатый файл: {decompressedFile}");
        }

        public void ButtonConfirmPassword1_Click(object sender, RoutedEventArgs e)
        {
            string hashedPassword = _hasher.HashPassword(_textBox1.Text);
            _textBox2.Text = hashedPassword;
        }

        public void ButtonConfirmPassword2_Click(object sender, RoutedEventArgs e)
        {
            bool isValid = _hasher.VerifyPassword(_textBox3.Text,_textBox2.Text);
            _textBox4.Text = isValid ? "Пароль верный" : "Пароль неверный";
        }

        public void ButtonLab8_Click(object sender, RoutedEventArgs e)
        {
            string inputFile = "C:\\Users\\yatoz\\source\\repos\\IS lab01\\text files\\123.txt";
            string encryptedFile = "C:\\Users\\yatoz\\source\\repos\\IS lab01\\text files\\encryptedGamma.txt";
            string encryptedFileGamma = "C:\\Users\\yatoz\\source\\repos\\IS lab01\\text files\\encryptedGamma.txt.gamma";
            string decryptedFile = "C:\\Users\\yatoz\\source\\repos\\IS lab01\\text files\\decryptedGamma.txt";
            _gamma.EncryptFile(inputFile, encryptedFile);
            _gamma.DecryptFile(encryptedFile, decryptedFile, encryptedFileGamma);
            _textBox1.Text = $"Файл {inputFile} зашифрован \n\n" +
                "Созданы файлы: \n" +
                $"Зашифрованный файл - {encryptedFile}\n\n" +
                $"Зашифрованный гамма файл - {encryptedFileGamma}\n\n" +
                $"Расшифрованный файл - {decryptedFile}";
        }
    }
}
