namespace IS_lab01.CryptLogics
{
    public class RLE
    {
        public byte[] RLECompress(byte[] data)
        {
            if (data == null || data.Length == 0)
                return new byte[0];

            List<byte> compressed = new List<byte>();
            int count = 1;
            byte current = data[0];

            for (int i = 1; i < data.Length; i++)
            {
                if (data[i] == current && count < 255)
                {
                    count++;
                }
                else
                {
                    // Добавляем текущую последовательность
                    compressed.Add((byte)count);
                    compressed.Add(current);
                    // Начинаем новую последовательность
                    current = data[i];
                    count = 1;
                }
            }

            // Добавляем последнюю последовательность
            compressed.Add((byte)count);
            compressed.Add(current);

            return compressed.ToArray();
        }

        public byte[] RLEDecompress(byte[] compressed)
        {
            if (compressed == null || compressed.Length == 0)
                return new byte[0];

            List<byte> decompressed = new List<byte>();

            for (int i = 0; i < compressed.Length - 1; i += 2)
            {
                int count = compressed[i];
                byte value = compressed[i + 1];

                for (int j = 0; j < count; j++)
                {
                    decompressed.Add(value);
                }
            }
            return decompressed.ToArray();
        }
    }
    
    

    public class Huffman
    {
        private class HuffmanNode
        {
            public byte Symbol { get; set; }
            public int Frequency { get; set; }
            public HuffmanNode Left { get; set; }
            public HuffmanNode Right { get; set; }
        }

        public byte[] HuffmanCompress(byte[] data)
        {
            if (data == null || data.Length == 0)
                return new byte[0];

            // Calculate frequencies
            Dictionary<byte, int> frequencies = new Dictionary<byte, int>();
            foreach (byte b in data)
            {
                if (!frequencies.ContainsKey(b))
                    frequencies[b] = 0;
                frequencies[b]++;
            }

            // Build Huffman tree
            var priorityQueue = new SortedDictionary<int, List<HuffmanNode>>();
            foreach (var pair in frequencies)
            {
                var node = new HuffmanNode { Symbol = pair.Key, Frequency = pair.Value };
                if (!priorityQueue.ContainsKey(pair.Value))
                    priorityQueue[pair.Value] = new List<HuffmanNode>();
                priorityQueue[pair.Value].Add(node);
            }

            while (priorityQueue.Count > 1 || priorityQueue.First().Value.Count > 1)
            {
                // Get two nodes with lowest frequencies
                var first = GetNextNode(priorityQueue);
                var second = GetNextNode(priorityQueue);

                // Create new internal node
                var newNode = new HuffmanNode
                {
                    Frequency = first.Frequency + second.Frequency,
                    Left = first,
                    Right = second
                };

                // Add new node to queue
                if (!priorityQueue.ContainsKey(newNode.Frequency))
                    priorityQueue[newNode.Frequency] = new List<HuffmanNode>();
                priorityQueue[newNode.Frequency].Add(newNode);
            }

            // Build encoding table
            var encodingTable = new Dictionary<byte, List<bool>>();
            if (priorityQueue.Count > 0)
            {
                var root = priorityQueue.First().Value[0];
                BuildEncodingTable(root, new List<bool>(), encodingTable);
            }

            // Compress data
            var compressed = new List<byte>();
            var bits = new List<bool>();

            // Add frequency table
            compressed.Add((byte)frequencies.Count);
            foreach (var pair in frequencies)
            {
                compressed.Add(pair.Key);
                compressed.AddRange(BitConverter.GetBytes(pair.Value));
            }

            // Add encoded data
            foreach (byte b in data)
            {
                bits.AddRange(encodingTable[b]);
            }

            // Convert bits to bytes
            for (int i = 0; i < bits.Count; i += 8)
            {
                byte value = 0;
                for (int j = 0; j < 8 && i + j < bits.Count; j++)
                {
                    if (bits[i + j])
                        value |= (byte)(1 << (7 - j));
                }
                compressed.Add(value);
            }

            return compressed.ToArray();
        }

        // Метод для расжатия по алгоритму Хаффмана
        public byte[] HuffmanDecompress(byte[] compressed)
        {
            if (compressed == null || compressed.Length == 0)
                return new byte[0];

            // Читаем таблицу частот
            int symbolCount = compressed[0];
            Dictionary<byte, int> frequencies = new Dictionary<byte, int>();
            int index = 1;

            for (int i = 0; i < symbolCount; i++)
            {
                byte symbol = compressed[index++];
                int frequency = BitConverter.ToInt32(compressed, index);
                frequencies[symbol] = frequency;
                index += 4;
            }

            // Восстанавливаем дерево Хаффмана
            var priorityQueue = new SortedDictionary<int, List<HuffmanNode>>();
            foreach (var pair in frequencies)
            {
                var node = new HuffmanNode { Symbol = pair.Key, Frequency = pair.Value };
                if (!priorityQueue.ContainsKey(pair.Value))
                    priorityQueue[pair.Value] = new List<HuffmanNode>();
                priorityQueue[pair.Value].Add(node);
            }

            while (priorityQueue.Count > 1 || priorityQueue.First().Value.Count > 1)
            {
                var first = GetNextNode(priorityQueue);
                var second = GetNextNode(priorityQueue);

                var newNode = new HuffmanNode
                {
                    Frequency = first.Frequency + second.Frequency,
                    Left = first,
                    Right = second
                };

                if (!priorityQueue.ContainsKey(newNode.Frequency))
                    priorityQueue[newNode.Frequency] = new List<HuffmanNode>();
                priorityQueue[newNode.Frequency].Add(newNode);
            }

            // Декодируем данные
            var root = priorityQueue.First().Value[0];
            var decompressed = new List<byte>();
            var currentNode = root;

            for (int i = index; i < compressed.Length; i++)
            {
                byte currentByte = compressed[i];
                for (int bit = 7; bit >= 0; bit--)
                {
                    bool isOne = (currentByte & (1 << bit)) != 0;
                    currentNode = isOne ? currentNode.Right : currentNode.Left;

                    if (currentNode.Left == null && currentNode.Right == null)
                    {
                        decompressed.Add(currentNode.Symbol);
                        currentNode = root;
                    }
                }
            }

            return decompressed.ToArray();
        }

        private HuffmanNode GetNextNode(SortedDictionary<int, List<HuffmanNode>> queue)
        {
            var first = queue.First();
            var node = first.Value[0];
            first.Value.RemoveAt(0);
            if (first.Value.Count == 0)
                queue.Remove(first.Key);
            return node;
        }

        private void BuildEncodingTable(HuffmanNode node, List<bool> currentEncoding,
            Dictionary<byte, List<bool>> encodingTable)
        {
            if (node.Left == null && node.Right == null)
            {
                encodingTable[node.Symbol] = new List<bool>(currentEncoding);
                return;
            }

            if (node.Left != null)
            {
                currentEncoding.Add(false);
                BuildEncodingTable(node.Left, currentEncoding, encodingTable);
                currentEncoding.RemoveAt(currentEncoding.Count - 1);
            }

            if (node.Right != null)
            {
                currentEncoding.Add(true);
                BuildEncodingTable(node.Right, currentEncoding, encodingTable);
                currentEncoding.RemoveAt(currentEncoding.Count - 1);
            }
        }

        public string FormatFileSize(long bytes)
        {
            string[] suffixes = { "B", "KB", "MB", "GB" };
            int counter = 0;
            decimal number = bytes;
            while (Math.Round(number / 1024) >= 1)
            {
                number = number / 1024;
                counter++;
            }
            return $"{number:n1} {suffixes[counter]}";
        }

        public bool CompareFiles(byte[] original, byte[] decompressed)
        {
            if (original.Length != decompressed.Length)
                return false;

            for (int i = 0; i < original.Length; i++)
            {
                if (original[i] != decompressed[i])
                    return false;
            }
            return true;
        }
    }
}