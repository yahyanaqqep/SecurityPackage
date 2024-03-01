using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class PlayFair : ICryptographic_Technique<string, string>
    {
        public string Decrypt(string cipherText, string key)
        {
            cipherText = cipherText.ToLower();
            key = key.ToLower();
            string plain = "";
            string[,] keymat = constructKeyMatrix(key);
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    System.Console.WriteLine(keymat[i, j]);
                }

            }
            for (int i = 0; i < cipherText.Length; i++)
            {
                System.Console.WriteLine(cipherText[i].ToString());
                System.Console.WriteLine(itemIndex(keymat, cipherText[i].ToString())[0, 0]);
                System.Console.WriteLine(itemIndex(keymat, cipherText[i].ToString())[0, 1]);
                int[,] ind1 = itemIndex(keymat, cipherText[i].ToString());
                int[,] ind2 = itemIndex(keymat, cipherText[i + 1].ToString());
                if (ind1[0, 0] == ind2[0, 0])
                {
                    int[,] newletterind1 = { { ind1[0, 0], (ind1[0, 1] - 1)<0?4: (ind1[0, 1] - 1) } };
                    int[,] newletterind2 = { { ind2[0, 0], (ind2[0, 1] - 1)<0?4: (ind2[0, 1] - 1) } };

                    if (keymat[newletterind1[0, 0], newletterind1[0, 1]] == "i/j")
                    {
                        plain += "i";
                    }
                    else
                    {
                        plain += keymat[newletterind1[0, 0], newletterind1[0, 1]];
                    }
                    if (keymat[newletterind2[0, 0], newletterind2[0, 1]] == "i/j")
                    {
                        plain += "i";
                    }
                    else
                    {
                        plain += keymat[newletterind2[0, 0], newletterind2[0, 1]];
                    }

                }
                else if (ind1[0, 1] == ind2[0, 1])
                {
                    int[,] newletterind1 = { { (ind1[0, 0] - 1)<0?4: (ind1[0, 0] - 1), ind1[0, 1] } };
                    int[,] newletterind2 = { { (ind2[0, 0] - 1)<0?4: (ind2[0, 0] - 1), ind2[0, 1] } };

                    if (keymat[newletterind1[0, 0], newletterind1[0, 1]] == "i/j")
                    {
                        plain += "i";
                    }
                    else
                    {
                        plain += keymat[newletterind1[0, 0], newletterind1[0, 1]];
                    }
                    if (keymat[newletterind2[0, 0], newletterind2[0, 1]] == "i/j")
                    {
                        plain += "i";
                    }
                    else
                    {
                        plain += keymat[newletterind2[0, 0], newletterind2[0, 1]];
                    }
                }
                else
                {
                    int[,] newletterind1 = { { ind1[0, 0], ind2[0, 1] } };
                    int[,] newletterind2 = { { ind2[0, 0], ind1[0, 1] } };
                    if (keymat[newletterind1[0, 0], newletterind1[0, 1]] == "i/j")
                    {
                        plain += "i";
                    }
                    else
                    {
                        plain += keymat[newletterind1[0, 0], newletterind1[0, 1]];
                    }
                    if (keymat[newletterind2[0, 0], newletterind2[0, 1]] == "i/j")
                    {
                        plain += "i";
                    }
                    else
                    {
                        plain += keymat[newletterind2[0, 0], newletterind2[0, 1]];
                    }
                }
                i++;
            }
            if (plain.Contains("x"))
            {
                for (int i = 0; i < plain.Length; i++)
                {
                    while (plain[i] != 'x' && i < plain.Length - 1)
                    {
                        i++;
                    }
                    if (i == plain.Length - 1)
                    {
                        if (plain[i] == 'x')
                        {
                            plain = plain.Remove(i, 1);
                        }
                    }
                    else if (plain[i - 1] == plain[i + 1] )
                    {
                        if(plain[i - 1] == 'l' && plain[i + 1] == 'l')
                        {
                            continue;
                        }
                        plain = plain.Remove(i, 1);
                    }
                }
            }

            System.Console.WriteLine(plain);
            return plain;
        }

        public string Encrypt(string plainText, string key)
        {
            plainText = plainText.ToLower();
            key = key.ToLower();
            string cipher = "";
            string[,] keymat = constructKeyMatrix(key);
            
            
            for (int i=0; i<5; i++)
            {
                for(int j=0; j<5; j++)
                {
                    System.Console.WriteLine(keymat[i, j]);
                }
                
            }
            for (int i = 0; i < plainText.Length; i++)
            {
                System.Console.WriteLine(plainText[i].ToString());
                System.Console.WriteLine(itemIndex(keymat, plainText[i].ToString())[0,0]);
                System.Console.WriteLine(itemIndex(keymat, plainText[i].ToString())[0, 1]);
                int[,] ind1 = itemIndex(keymat, plainText[i].ToString());
                int[,] ind2 = { { 0,0} };
                if (i + 1 >= plainText.Length)
                {
                    ind2 = itemIndex(keymat, "x");
                }
                else if (i + 1 < plainText.Length && plainText[i] == plainText[i + 1])
                {
                    plainText = plainText.Insert(i + 1, "x");
                    ind2 = itemIndex(keymat, plainText[i + 1].ToString());
                }
                else
                {
                    ind2 = itemIndex(keymat, plainText[i + 1].ToString());
                }
                if (ind1[0, 0] == ind2[0, 0])
                {
                    int[,] newletterind1 = { { ind1[0, 0], (ind1[0, 1] + 1)%5 } };
                    int[,] newletterind2 = { { ind2[0, 0], (ind2[0, 1] + 1)%5 } };
                    
                    if (keymat[newletterind1[0, 0], newletterind1[0, 1]] == "i/j")
                    {
                        cipher += "i";
                    }
                    else
                    {
                        cipher += keymat[newletterind1[0, 0], newletterind1[0, 1]];
                    }
                    if (keymat[newletterind2[0, 0], newletterind2[0, 1]] == "i/j")
                    {
                        cipher += "i";
                    }
                    else
                    {
                        cipher += keymat[newletterind2[0, 0], newletterind2[0, 1]];
                    }

                }
                else if (ind1[0,1] == ind2[0, 1])
                {
                    int[,] newletterind1 = { { (ind1[0, 0]+1)%5, ind1[0, 1] } };
                    int[,] newletterind2 = { { (ind2[0, 0]+1)%5, ind2[0, 1] } };
                    
                    if (keymat[newletterind1[0, 0], newletterind1[0, 1]] == "i/j")
                    {
                        cipher += "i";
                    }
                    else
                    {
                        cipher += keymat[newletterind1[0, 0], newletterind1[0, 1]];
                    }
                    if (keymat[newletterind2[0, 0], newletterind2[0, 1]] == "i/j")
                    {
                        cipher += "i";
                    }
                    else
                    {
                        cipher += keymat[newletterind2[0, 0], newletterind2[0, 1]];
                    }
                }
                else
                {
                    int[,] newletterind1 = { { ind1[0, 0] , ind2[0, 1] } };
                    int[,] newletterind2 = { { ind2[0, 0] , ind1[0, 1] } };
                    if (keymat[newletterind1[0, 0], newletterind1[0, 1]] == "i/j")
                    {
                        cipher += "i";
                    }
                    else
                    {
                        cipher += keymat[newletterind1[0, 0], newletterind1[0, 1]];
                    }
                    if (keymat[newletterind2[0, 0], newletterind2[0, 1]] == "i/j")
                    {
                        cipher += "i";
                    }
                    else
                    {
                        cipher += keymat[newletterind2[0, 0], newletterind2[0, 1]];
                    }
                }
                i++;
            }
            System.Console.WriteLine(cipher);
            return cipher;
            

        }
        public bool contains(string[,] arr, string element)
        {
            if (element == "i" || element == "j")
            {
                for (int i = 0; i < arr.GetLength(0); i++)
                {
                    for (int j = 0; j < arr.GetLength(1); j++)
                    {
                        if (arr[i, j] == "i/j")
                        {
                            return true;
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < arr.GetLength(0); i++)
                {
                    for (int j = 0; j < arr.GetLength(1); j++)
                    {
                        if (arr[i, j] == element)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        public int[,] itemIndex(string[,] arr, string element)
        {
            int[,] ret = new int[1, 2];
            if (element == "i" || element == "j")
            {
                for (int i = 0; i < arr.GetLength(0); i++)
                {
                    bool found = false;
                    for (int j = 0; j < arr.GetLength(1); j++)
                    {
                        if (arr[i, j] == "i/j")
                        {
                            ret[0, 0] = i;
                            ret[0, 1] = j;
                            found = true;
                            break;
                        }
                    }
                    if (found) { break; }
                }
            }
            else
            {
                for (int i = 0; i < arr.GetLength(0); i++)
                {
                    bool found = false;
                    for (int j = 0; j < arr.GetLength(1); j++)
                    {
                        if (arr[i, j] == element)
                        {
                            ret[0, 0] = i;
                            ret[0, 1] = j;
                            found = true;
                            break;
                        }
                    }
                    if (found) { break; }
                }
            }
            return ret;
        }
        public string[,] constructKeyMatrix(string key)
        {
            string[,] keymat = new string[5, 5];
            int keyind = 0;
            string currletter = "a";
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (keyind < key.Length)
                    {
                        if ((key[keyind] == 'i' || key[keyind] == 'j') && !contains(keymat, "i/j"))
                        {
                            keymat[i, j] = "i/j";
                        }
                        else if (key[keyind] == 'i' && contains(keymat, "i/j"))
                        {
                            if (j == 0)
                            {
                                i--;
                                j = 4;
                            }
                            else { j--; }
                            keyind++;
                        }
                        else if (key[keyind] == 'j' && contains(keymat, "i/j"))
                        {
                            if (j == 0)
                            {
                                i--;
                                j = 4;
                            }
                            else { j--; }
                            keyind++;
                        }
                        else
                        {
                            if (!contains(keymat, key[keyind].ToString())) { keymat[i, j] = key[keyind].ToString(); }
                            else
                            {
                                if (j == 0) { i--; j = 4; }
                                else { j--; }
                            }
                            keyind += 1;
                        }
                    }
                    else
                    {

                        while (contains(keymat, currletter) && currletter.ToCharArray()[0] < 'z')
                        {
                            char c = currletter.ToCharArray()[0];
                            c = (char)(c + 1);
                            currletter = c.ToString();

                        }
                        if (currletter == "i" || currletter == "j")
                        {
                            currletter = "i/j";
                            keymat[i, j] = currletter;
                            currletter = "k";
                        }
                        else
                        {
                            keymat[i, j] = currletter;
                        }
                    }
                }
            }
            return keymat;
        }
    } 
}