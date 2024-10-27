using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using IS_lab01.CryptLogics;
using System;

namespace Controllers
{
    public class ButtonsController
    {
        private ListBox ListBox1;
        private ListBox ListBox2;
        private RichTextBox _richTextBox1;
        private RSA _rsa = new RSA();
        private DiffieHellman _diffhell = new DiffieHellman();
        private BlockEncrypt _blockEncrypt = new BlockEncrypt();
        private ErrorCorrection _errorCorrection = new ErrorCorrection();
        private RLE _rle = new RLE();
        private Huffman _huffman = new Huffman();

        public ButtonsController(RichTextBox RTB1)
        {
            this._richTextBox1 = RTB1;
        }
        
        public ButtonsController(ListBox LB1)
        {
            this.ListBox1 = LB1;
        }
        
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
                ListBox1.Items.Add($"N: {_diffhell.N}");
                ListBox1.Items.Add($"Q: {_diffhell.Q}");
                ListBox1.Items.Add($"Public key A: ({_diffhell.PublicKeyA})");
                ListBox1.Items.Add($"Public key B: ({_diffhell.PublicKeyB})");
                ListBox1.Items.Add($"X: ({_diffhell.X})");
                ListBox1.Items.Add($"Y: ({_diffhell.Y})");
                ListBox1.Items.Add($"Client Private A - {_diffhell.PrivateKeyA}");
                ListBox1.Items.Add($"Client Private B - {_diffhell.PrivateKeyB}");
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

        public void ButtonToExecuteLab5_Click(object sender, RoutedEventArgs e)
        {
            int numberOfTests = 10; // Number of random tests
            for (int i = 0; i < numberOfTests; i++)
            {
                int data = _errorCorrection.GenerateRandomData(6); // Generate random 6-bit data
                int encoded = _errorCorrection.Encode(data);
                _richTextBox1.AppendText($"Original data: {Convert.ToString(data, 2).PadLeft(6, '0')}" + Environment.NewLine);
                _richTextBox1.AppendText($"Encoded data: {Convert.ToString(encoded, 2).PadLeft(_errorCorrection.CodeLength, '0')}" + Environment.NewLine);

                // Randomly decide whether to introduce an error
                if (_errorCorrection.Random.Next(2) == 0) // 50% chance to introduce an error
                {
                    int received = _errorCorrection.IntroduceRandomError(encoded);
                    _richTextBox1.AppendText($"Received data with error: {Convert.ToString(received, 2).PadLeft(_errorCorrection.CodeLength, '0')}" + Environment.NewLine);
                    DecodeResult result = _errorCorrection.Decode(received);
                    _richTextBox1.AppendText(result.Message + Environment.NewLine);
                    _richTextBox1.AppendText($"Corrected data: {Convert.ToString(result.CorrectedData, 2).PadLeft(6, '0')}" + Environment.NewLine);
                }
                else
                {
                    _richTextBox1.AppendText($"Received data without error: {Convert.ToString(encoded, 2).PadLeft(_errorCorrection.CodeLength, '0')}" + Environment.NewLine);
                    DecodeResult result = _errorCorrection.Decode(encoded);
                    _richTextBox1.AppendText(result.Message + Environment.NewLine);
                    _richTextBox1.AppendText($"Original data: {Convert.ToString(result.CorrectedData, 2).PadLeft(6, '0')}" + Environment.NewLine);
                }
                _richTextBox1.AppendText(Environment.NewLine);
            }
        }

        public void ButtonToExecuteLab6_Click(object sender, RoutedEventArgs e)
        {
            string inputFile = "C:\\Users\\yatoz\\source\\repos\\IS lab01\\text files\\123.txt";
            string compressedFile = "C:\\Users\\yatoz\\source\\repos\\IS lab01\\text files\\output_rlehuff.txt";
            string decompressedFile = "C:\\Users\\yatoz\\source\\repos\\IS lab01\\text files\\decompressed.txt";

            // Чтение исходного файла
            Console.WriteLine("Чтение исходного файла...");
            byte[] originalData = File.ReadAllBytes(inputFile);
            ListBox1.Items.Add($"Размер исходного файла: {_huffman.FormatFileSize(originalData.Length)}");
            
            // Этап 1: RLE сжатие
            ListBox1.Items.Add("\n--- Этап 1: RLE сжатие ---");
            byte[] rleCompressed = _rle.RLECompress(originalData);
            ListBox1.Items.Add("Файл успешно сжат");

            // Этап 2: Хаффман сжатие
            ListBox1.Items.Add("\n--- Этап 2: Сжатие Хаффмана ---");
            byte[] huffmanCompressed = _huffman.HuffmanCompress(rleCompressed);
            File.WriteAllBytes(compressedFile, huffmanCompressed);
            ListBox1.Items.Add($"Финальный размер сжатого файла: {_huffman.FormatFileSize(huffmanCompressed.Length)}");
            ListBox1.Items.Add($"Общая степень сжатия: {(double)originalData.Length / huffmanCompressed.Length:F2}x");

            // Этап 3: Хаффман расжатие
            ListBox1.Items.Add("\n--- Этап 3: Расжатие Хаффмана ---");
            byte[] huffmanDecompressed = _huffman.HuffmanDecompress(huffmanCompressed);
            ListBox1.Items.Add($"Файл расжат без ошибок");

            // Этап 4: RLE расжатие
            ListBox1.Items.Add("\n--- Этап 4: RLE расжатие ---");
            byte[] finalDecompressed = _rle.RLEDecompress(huffmanDecompressed);
            File.WriteAllBytes(decompressedFile, finalDecompressed);
            ListBox1.Items.Add($"Финальный размер расжатого файла: {_huffman.FormatFileSize(finalDecompressed.Length)}");

            // Проверка корректности
            bool success = _huffman.CompareFiles(originalData, finalDecompressed);
            ListBox1.Items.Add("\n=== Результаты ===");
            ListBox1.Items.Add($"Сжатие и расжатие выполнено {(success ? "успешно" : "с ошибками")}");
            ListBox1.Items.Add($"Исходный файл: {inputFile}");
            ListBox1.Items.Add($"Сжатый файл: {compressedFile}");
            ListBox1.Items.Add($"Расжатый файл: {decompressedFile}");
        }
    }
}
