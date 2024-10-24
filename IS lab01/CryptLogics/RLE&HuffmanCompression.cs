using System;
using System.IO;
using System.Text;
using System.Collections;

namespace IS_lab01.CryptLogics
{
    public class RLE
    {
        public string RLECompress(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;

            StringBuilder compressed = new StringBuilder();
            char currentChar = input[0];
            int count = 1;

            for (int i = 1; i < input.Length; i++)
            {
                if (input[i] == currentChar)
                {
                    count++;
                }
                else
                {
                    compressed.Append(currentChar);
                    compressed.Append(count);
                    currentChar = input[i];
                    count = 1;
                }
            }
            compressed.Append(currentChar);
            compressed.Append(count);

            return compressed.ToString();
        }

        public string RLEDecompress(string input)
        {
            StringBuilder decompressed = new StringBuilder();
            for (int i = 0; i < input.Length; i++)
            {
                char currentChar = input[i];
                i++;
                int count = int.Parse(input[i].ToString());
                decompressed.Append(new string(currentChar, count));
            }
            return decompressed.ToString();
        }
    }

    public class HuffmanNode
    {
        public char Character;
        public int Frequency;
        public HuffmanNode Left, Right;

        public HuffmanNode(char character, int frequency)
        {
            Character = character;
            Frequency = frequency;
        }
    }

    public class Huffman
    {
        public static Dictionary<char, string> BuildHuffmanTree(string input)
        {
            var frequency = input.GroupBy(c => c).ToDictionary(g => g.Key, g => g.Count());
            var priorityQueue = new SortedSet<HuffmanNode>(Comparer<HuffmanNode>.Create((a, b) => a.Frequency == b.Frequency ? a.Character.CompareTo(b.Character) : a.Frequency.CompareTo(b.Frequency)));

            foreach (var kvp in frequency)
            {
                priorityQueue.Add(new HuffmanNode(kvp.Key, kvp.Value));
            }

            while (priorityQueue.Count > 1)
            {
                var left = priorityQueue.Min;
                priorityQueue.Remove(left);
                var right = priorityQueue.Min;
                priorityQueue.Remove(right);

                var newNode = new HuffmanNode('\0', left.Frequency + right.Frequency) { Left = left, Right = right };
                priorityQueue.Add(newNode);
            }

            var root = priorityQueue.Min;

            var codes = new Dictionary<char, string>();
            BuildCodes(root, "", codes);
            return codes;
        }

        private static void BuildCodes(HuffmanNode node, string code, Dictionary<char, string> codes)
        {
            if (node == null) return;

            if (node.Left == null && node.Right == null)
            {
                codes[node.Character] = code;
            }

            BuildCodes(node.Left, code + "0", codes);
            BuildCodes(node.Right, code + "1", codes);
        }

        public static string HuffmanCompress(string input)
        {
            var codes = BuildHuffmanTree(input);
            var compressed = new StringBuilder();

            foreach (var character in input)
            {
                compressed.Append(codes[character]);
            }

            return compressed.ToString();
        }

        public static string HuffmanDecompress(string compressed, Dictionary<char, string> codes)
        {
            var reverseCodes = codes.ToDictionary(kvp => kvp.Value, kvp => kvp.Key);
            var currentCode = new StringBuilder();
            var decompressed = new StringBuilder();

            foreach (var bit in compressed)
            {
                currentCode.Append(bit);
                if (reverseCodes.TryGetValue(currentCode.ToString(), out char character))
                {
                    decompressed.Append(character);
                    currentCode.Clear();
                }
            }

            return decompressed.ToString();
        }
    }
}