using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class Monoalphabetic : ICryptographicTechnique<string, string>
    {
        public string Analyse(string plainText, string cipherText)
        {
           

        }

        public string Decrypt(string cipherText, string key)
        {
            key = key.ToUpper();
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
            string output = "";
            output += cipher[maxIndex];
            for(int i=1; i<5; i++)
            {

                output = (cipher[maxIndex] - i) + output;
            }
            


        }
    }
}