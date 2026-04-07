using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Shell.Fun
{
    internal class FlameCalculator
    {
        public string GetResult(string crush1, string crush2)
        {
            crush1 = Normalize(crush1);
            crush2 = Normalize(crush2);

            var netCount = GetNetLetterCountAfterRemovingTheCommonLetters(crush1, crush2);

            return GetResultantFlame(netCount);
        }

        static string Normalize(string name)
        {
            // Uppercase and remove non-alphabetic characters
            return Regex.Replace(name.ToUpper(), "[^A-Z]", "");
        }

        static int GetNetLetterCountAfterRemovingTheCommonLetters(string cursh1, string crush2)
        {
            char?[] arr1 = Array.ConvertAll(cursh1.ToCharArray(), c => (char?)c);
            char?[] arr2 = Array.ConvertAll(crush2.ToCharArray(), c => (char?)c);

            for (int i = 0; i < arr1.Length; i++)
            {
                for (int j = 0; j < arr2.Length; j++)
                {
                    if (arr1[i] != null && arr2[j] != null && arr1[i] == arr2[j])
                    {
                        arr1[i] = null;  // cancel
                        arr2[j] = null;  // cancel
                        break;
                    }
                }
            }

            // Count remaining (non-null) letters
            int count = 0;
            foreach (var c in arr1) if (c != null) count++;
            foreach (var c in arr2) if (c != null) count++;
            return count;
        }

        static string GetResultantFlame(int count)
        {
            List<char> letters = new List<char> { 'F', 'L', 'A', 'M', 'E' };
            int pos = 0;

            while (letters.Count > 1)
            {
                // Calculate the index to eliminate
                int idx = (pos + count - 1) % letters.Count;

                Console.WriteLine($"Count {count} → Eliminate '{letters[idx]}'");

                letters.RemoveAt(idx);

                // Next round starts from same index (mod new length)
                pos = idx % letters.Count;
            }

            return GetResult(letters[0]);
        }

        static string GetResult(char letter)
        {
            return letter switch
            {
                'F' => "Friendship 🤝",
                'L' => "Love ❤️",
                'A' => "Affection 🥰",
                'M' => "Marriage 💍",
                'E' => "Enemies ⚔️",
                _ => "Unknown"
            };
        }
    }
}
