using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class Monoalphabetic : ICryptographicTechnique<string, string>
    {
        public string Analyse(string plainText, string cipherText)
        {
            plainText = plainText.ToUpper();
            cipherText = cipherText.ToUpper();
            SortedDictionary<char, char> parameters = new SortedDictionary<char, char>()
            {
                {'A',' ' },{'B', ' ' },{'C',' ' }, {'D', ' ' }, {'E', ' '}, {'F', ' ' }, {'G',' ' },
                {'H', ' ' }, {'I', ' ' }, {'J', ' ' }, {'K', ' ' }, {'L', ' ' }, {'M', ' ' }, {'N', ' ' },
                {'O', ' ' }, {'P', ' ' }, {'Q', ' ' }, {'R', ' ' }, {'S', ' ' },{'T', ' ' }, {'U', ' ' }, {'V', ' ' },
                {'W', ' ' }, {'X', ' ' },{'Y', ' ' }, {'Z', ' ' }
            };
            char[] freqList = {
                'E', 'T', 'A', 'O', 'L', 'N', 'S', 'R', 'H', 'L',
                'D', 'C', 'U', 'M', 'F', 'P', 'G', 'W', 'Y', 'B',
                'V', 'K', 'X', 'J', 'Q', 'Z'
            };
            char[] alphabet =
           {
                'A','B','C','D','E','F','G',
                'H', 'I','J','K','L','M','N','O',
                'P','Q','R','S','T','U','V','W',
                'X','Y','Z'
            };
            for (int  i=0; i<plainText.Length; i++)
            {
                parameters[plainText[i]] = cipherText[i];
            }
            foreach(char c in alphabet)
            {
                if (parameters[c] == ' ')
                {
                    foreach(char c2 in freqList)
                    {
                        if (!parameters.ContainsValue(c2))
                        {
                            parameters[c] = c2;
                        }
                    }
                }
            }
            System.Console.WriteLine(parameters.Values.ToArray());
            Regex regex = new Regex("isy.k.{2}ux.{2}zqmct.lofn.{3}a.");
            System.Console.WriteLine(regex.Match(new string(parameters.Values.ToArray()).ToLower()).Success);
            return new string(parameters.Values.ToArray()).ToLower();
        }

    public string Decrypt(string cipherText, string key)
        {
            key = key.ToLower();
            cipherText = cipherText.ToLower();
            char[] smallAlphabet =
          {
                'a','b','c','d','e','f','g',
                'h', 'i','j','k','l','m','n','o',
                'p','q','r','s','t','u','v','w',
                'x','y','z'
            };
            string output = "";
            foreach(var c in cipherText)
            {
                int index = key.IndexOf(c);
                output+=smallAlphabet[index];
            }
            return output;


        }

        public string Encrypt(string plainText, string key)
        {
            char[] smallAlphabet =
          {
                'a','b','c','d','e','f','g',
                'h', 'i','j','k','l','m','n','o',
                'p','q','r','s','t','u','v','w',
                'x','y','z'
            };
            string output = "";
            foreach(var c in plainText)
            {
                int index = Array.IndexOf(smallAlphabet, c);
                output += key[index];
            }
            return output;

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
        /// </summary>
        /// <param name="cipher"></param>
        /// <returns>Plain text</returns>
        /// 

        public string AnalyseUsingCharFrequency(string cipher)
        {
            cipher = cipher.ToLower();
            char[] smallAlphabet =
          {
                'a','b','c','d','e','f','g',
                'h', 'i','j','k','l','m','n','o',
                'p','q','r','s','t','u','v','w',
                'x','y','z'
            };
            int[] freqList = new int[26];
            foreach(var c in cipher)
            {
                int index = Array.IndexOf(smallAlphabet, c);
                freqList[index]++;
            }
            int maxIndex = 0;
            for (int i = 0; i < freqList.Length; i++)
            {
                if (freqList[i] > freqList[maxIndex])
                {
                    maxIndex = i;
                }
            }
            string key = "";
            key += smallAlphabet[maxIndex];

            System.Console.WriteLine(maxIndex);

            for(int i=1; i<5; i++)
            {
                //System.Console.WriteLine(i);
                int ind = maxIndex - i;
                //System.Console.WriteLine(ind);
                if (ind < 0)
                {
                    ind += 26;
                }
                //System.Console.WriteLine(ind);
                key = smallAlphabet[ind] + key;
            }
            for (int i = 1; i < 22; i++)
            {
                System.Console.WriteLine(i);
                int ind = maxIndex + i;
                System.Console.WriteLine(ind);
                if (ind > 25)
                {
                    ind %= 26;
                }
                System.Console.WriteLine(ind);
                key = key + smallAlphabet[ind] ;
            }

            string output = Decrypt(cipher, key);
            System.Console.WriteLine(output);
            return output;
            


        }
    }
}