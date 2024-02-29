using System;
using System.Collections.Generic;
using System.Linq;

namespace SecurityLibrary
{
    public class Ceaser : ICryptographicTechnique<string, int>
    {

        public string Encrypt(string plainText, int key)
        {
            plainText.ToLower();
            string[] alphabet =
            {
                "A","B","C","D","E","F","G",
                "H","I","J","K","L","M","N","O",
                "P","Q","R","S","T","U","V","W",
                "X","Y","Z"
            };
            char[] smallAlphabet =
           {
                'a','b','c','d','e','f','g',
                'h', 'i','j','k','l','m','n','o',
                'p','q','r','s','t','u','v','w',
                'x','y','z'
            };

            string output = "";
            foreach(var item in plainText)
            {
                int index = Array.IndexOf(smallAlphabet, item);
                index += key;
                index = index % 26;
                output = output.Insert(output.Length, alphabet[index]);
            }
            
            return output;

        }

        public string Decrypt(string cipherText, int key)
        {
            string[] smallAlphabet =
            {
                "a","b","c","d","e","f","g",
                "h","i","j","k","l","m","n","o",
                "p","q","r","s","t","u","v","w",
                "x","y","z"
            };
            char[] alphabet =
           {
                'A','B','C','D','E','F','G',
                'H', 'I','J','K','L','M','N','O',
                'P','Q','R','S','T','U','V','W',
                'X','Y','Z'
            };
            cipherText.ToUpper();
            string output = "";
            foreach(var item in cipherText)
            {
                int index = Array.IndexOf(alphabet, item);
                index -= key;
                if (index < 0)
                {
                    index += 26;
                    output += smallAlphabet[index];
                    continue;
                }

                index = index % 26; 
                output += smallAlphabet[index];
            }
            return output;

        }

        public int Analyse(string plainText, string cipherText)
        {
            plainText = plainText.ToLower();
            cipherText = cipherText.ToUpper();
            char[] alphabet =
           {
                'A','B','C','D','E','F','G',
                'H', 'I','J','K','L','M','N','O',
                'P','Q','R','S','T','U','V','W',
                'X','Y','Z'
            };
            char[] smallAlphabet =
          {
                'a','b','c','d','e','f','g',
                'h', 'i','j','k','l','m','n','o',
                'p','q','r','s','t','u','v','w',
                'x','y','z'
            };
            int[] frequencies = new int[26];

            // Calculate frequencies of letters in the cipher text
            foreach (char c in cipherText)
            {
                int index = Array.IndexOf(alphabet, c);
                frequencies[index]++;
            }

            // Find the most frequent letter's index
            int maxIndex = 0;
            for (int i = 1; i < frequencies.Length; i++)
            {
                if (frequencies[i] > frequencies[maxIndex])
                {
                    maxIndex = i;
                }
            }

            // Calculate the key based on the most frequent letter's shift
            int key = maxIndex - 4; // 'E' is the most frequent letter in English
            
            // Ensure the key is within the range [0, 25]
            key = (key + 26) % 26;
            if (Decrypt(cipherText, key).ToLower() != plainText)
            {
                key = 0;
                while (Decrypt(cipherText, key).ToLower() != plainText)
                {
                    key += 1;
                }
            }
            return key;
        }
    }
}

/// <summary>
/// Frequency Information:
/// E   12.51%
/// T	9.25
/// A	=
/// O	7.60
/// I	7.26
/// N	7.09
/// S	6.54
/// R	6.12
/// H	5.49
/// L	4.14
/// D	3.99
/// C	3.06
/// U	2.71
/// M	2.53
/// F	2.30
/// P	2.00
/// G	1.96
/// W	1.92
/// Y	1.73
/// B	1.54
/// V	0.99
/// K	0.67
/// X	0.19
/// J	0.16
/// Q	0.11
/// Z	0.09

