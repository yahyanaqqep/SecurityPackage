using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{

    public class HillCipher : ICryptographicTechnique<string, string>, ICryptographicTechnique<List<int>, List<int>>
    {

        public List<int> Analyse(List<int> plainText, List<int> cipherText)
        {
            List<int> result = new List<int>();
            result.Add(0);
            result.Add(0);
            result.Add(0);
            result.Add(0);
            for (int item1 = 0; item1<26 && !checkEqual(Encrypt(plainText, result), cipherText); item1++) 
            {
                for (int item2 = 0; item2 < 26 && !checkEqual(Encrypt(plainText, result), cipherText); item2++)
                {
                    for (int item3 = 0; item3 < 26 && !checkEqual(Encrypt(plainText, result), cipherText); item3++)
                    {
                        for (int item4 = 0; item4 < 26 && !checkEqual(Encrypt(plainText, result), cipherText); item4++)
                        {
                            result[0] = item1;
                            result[1] = item2;
                            result[2] = item3;
                            result[3] = item4;
                        }
                    }
                }


            }
            if(!checkEqual(Encrypt(plainText, result), cipherText))
            {
                throw new InvalidAnlysisException();
            }
            else
            {
                return result;
            }
        }


        public string Analyse(string plainText, string cipherText)
        {
            throw new NotImplementedException();
        }
        
        public List<int> Decrypt(List<int> cipherText, List<int> key)
        {
            double[,] keyMat = constructKeyMat(key, true);
            List<int[]> wordMats = constructWordMats(cipherText, keyMat.GetLength(0));
            keyMat = getInverseOfMatrix(keyMat);
            if (keyMat.GetLength(0) == 3)
            {
                int modularMultInv = getModularMultiplicativeInverse(Convert.ToInt32(get3by3det(constructKeyMat(key, true))), 26);
                for (int i = 0; i < keyMat.GetLength(0); i++)
                {
                    for (int j = 0; j < keyMat.GetLength(1); j++)
                    {
                        keyMat[i, j] *= modularMultInv;
                        if (keyMat[i, j] < 0)
                        {
                            while (keyMat[i, j] < 0)
                            {
                                keyMat[i, j] += 26;
                            }
                        }
                        keyMat[i, j] %= 26;

                    }
                }
            }
           
            
            List<double[]> plainMats = new List<double[]>();
            foreach (int[] wordMat in wordMats)
            {
                plainMats.Add(multiplyMatrices(keyMat, wordMat));
            }
            List<int> ret = new List<int>();
            for(int i = 0; i<plainMats.Count; i++)
            {
                for(int j = 0; j < plainMats[0].Length; j++)
                {
                    if (plainMats[i][j] < 0)
                    {
                        ret.Add((int)(26 + plainMats[i][j]));
                    }
                    else
                    {
                        ret.Add((int)plainMats[i][j]);
                    }
                }
            }
            for (int i = 0; i < ret.Count; i++)
            {
                System.Console.WriteLine(ret[i]);
            }
            return ret;

        }
        public string Decrypt(string cipherText, string key)
        {
            throw new NotImplementedException();
        }


        public List<int> Encrypt(List<int> plainText, List<int> key)
        {
            int[,]keyMatrix = constructKeyMat(key);
            List<int[]> wordMats = constructWordMats(plainText, key.Count%2==0?2:3);
            List<int[]> cipherMats = new List<int[]>();
            foreach(int[] wordMat in wordMats)
            {
                cipherMats.Add(multiplyMatrices(keyMatrix, wordMat));
            }
            List<int> ret = new List<int>();
            for(int i = 0; i < cipherMats.Count; i++)
            {
                for(int j = 0; j < cipherMats[0].Length; j++)
                {
                    ret.Add(cipherMats[i][j]);
                }
            }
            for (int i = 0; i < ret.Count; i++)
            {
                System.Console.WriteLine(ret[i]);
            }
            return ret;
        }
        public string Encrypt(string plainText, string key)
        {
            throw new NotImplementedException();
        }


        public List<int> Analyse3By3Key(List<int> plain3, List<int> cipher3)
        {

            double[,] cipherMat = constructKeyMat(cipher3, true);
            double[,] plainMat = constructKeyMat(plain3, true);
            int modmultinv = getModularMultiplicativeInverse(Convert.ToInt32(get3by3det(plainMat)), 26);
            double[,] plainMat2 = new double[3,3];
            plainMat2 = getInverseOfMatrix(constructKeyMat(plain3, true));

            for (int i = 0; i < plainMat2.GetLength(0); i++)
            {
                for(int j = 0; j<plainMat2.GetLength(1); j++)
                {
                    plainMat2[i,j]*=modmultinv;
                    plainMat2[i,j]%=26;
                }
            }
            //cipherMat = transpose(cipherMat);
            double[,] key = new double[3, 3];
            key[0, 0] = plainMat2[0, 0] * cipherMat[0, 0] + plainMat2[0, 1] * cipherMat[1, 0] + plainMat2[0, 2] * cipherMat[2, 0];
            key[0, 1] = plainMat2[1, 0] * cipherMat[0, 0] + plainMat2[1, 1] * cipherMat[1, 0] + plainMat2[1, 2] * cipherMat[2, 0];
            key[0, 2] = plainMat2[2, 0] * cipherMat[0, 0] + plainMat2[2, 1] * cipherMat[1, 0] + plainMat2[2, 2] * cipherMat[2, 0];
            key[1, 0] = plainMat2[0, 0] * cipherMat[0, 1] + plainMat2[0, 1] * cipherMat[1, 1] + plainMat2[0, 2] * cipherMat[2, 1];
            key[1, 1] = plainMat2[1, 0] * cipherMat[0, 1] + plainMat2[1, 1] * cipherMat[1, 1] + plainMat2[1, 2] * cipherMat[2, 1];
            key[1, 2] = plainMat2[2, 0] * cipherMat[0, 1] + plainMat2[2, 1] * cipherMat[1, 1] + plainMat2[2, 2] * cipherMat[2, 1];
            key[2, 0] = plainMat2[0, 0] * cipherMat[0, 2] + plainMat2[0, 1] * cipherMat[1, 2] + plainMat2[0, 2] * cipherMat[2, 2];
            key[2, 1] = plainMat2[1, 0] * cipherMat[0, 2] + plainMat2[1, 1] * cipherMat[1, 2] + plainMat2[1, 2] * cipherMat[2, 2];
            key[2, 2] = plainMat2[2, 0] * cipherMat[0, 2] + plainMat2[2, 1] * cipherMat[1, 2] + plainMat2[2, 2] * cipherMat[2, 2]; ;
            for (int i = 0; i < key.GetLength(0); i++)
            {
                for (int j = 0; j < key.GetLength(1); j++)
                {
                    if (key[i, j] < 0)
                    {
                        while (key[i, j] < 0)
                        {
                            key[i, j] += 26;
                        }
                    }
                    else
                    {
                        key[i, j] %= 26;
                    }
                }
            }
            List<int> ret = new List<int> { Convert.ToInt32(key[0, 0]), Convert.ToInt32(key[0, 1]), Convert.ToInt32(key[0, 2]), Convert.ToInt32(key[1, 0]),
                Convert.ToInt32(key[1, 1]), Convert.ToInt32(key[1, 2]), Convert.ToInt32(key[2, 0]), Convert.ToInt32(key[2, 1]), Convert.ToInt32(key[2, 2]) };
            return ret;
        }

        public string Analyse3By3Key(string plain3, string cipher3)
        {
            throw new NotImplementedException();
        }
        
        public int[,] constructKeyMat(List<int> key)
        {
            int dimensions = key.Count==9?3:2;
            int[,] keyMat = new int[dimensions, dimensions];
            int k = 0;
            for(int i = 0; i < dimensions; i++)
            {
                for(int j=0;j<dimensions; j++)
                {
                    keyMat[i, j] = key[k];
                    k++;
                }
            }
            return keyMat;
            
        }
        public double[,] constructKeyMat(List<int> key, bool returnDouble)
        {
            int dimensions = key.Count == 9 ? 3 : 2;
            double[,] keyMat = new double[dimensions, dimensions];
            int k = 0;
            for (int i = 0; i < dimensions; i++)
            {
                for (int j = 0; j < dimensions; j++)
                {
                    keyMat[i, j] = key[k];
                    k++;
                }
            }
            return keyMat;

        }
        public int[] multiplyMatrices(int[,]keyMat, int[] wordMat)
        {
            int[] ret = new int[wordMat.Length];
            for(int i = 0; i<keyMat.GetLength(0); i++) {             
                for(int j = 0; j < keyMat.GetLength(1); j++)
                {
                    ret[i]+=keyMat[i, j] * wordMat[j];
                }
                ret[i] %= 26;
            }
            return ret;
        }
        public double[] multiplyMatrices(double[,] keyMat, double[] wordMat)
        {
            double[] ret = new double[wordMat.Length];
            for (int i = 0; i < keyMat.GetLength(0); i++)
            {
                for (int j = 0; j < keyMat.GetLength(1); j++)
                {
                    ret[i] += keyMat[i, j] * wordMat[j];
                }
                ret[i] %= 26;
            }
            return ret;
        }
        public double[] multiplyMatrices(double[,] keyMat, int[] wordMat)
        {
            double[] ret = new double[wordMat.Length];
            for (int i = 0; i < keyMat.GetLength(0); i++)
            {
                for (int j = 0; j < keyMat.GetLength(1); j++)
                {
                    ret[i] += keyMat[i, j] * wordMat[j];
                }
                ret[i] %= 26;
            }
            return ret;
        }
        public List<int[]> constructWordMats(List<int> wordList, int dimensions)
        {
            List<int[]> ret = new List<int[]>();
            for(int i = 0; i<wordList.Count; i=i+dimensions)
            {
                int[] wordMat = new int[dimensions];
                if (dimensions == 2)
                {
                    wordMat[0] = wordList[i] ;
                    wordMat[1] = wordList[i+1];
                }
                else
                {
                    wordMat[0] = wordList[i];
                    wordMat[1] = wordList[i + 1];
                    wordMat[2] = wordList[i + 2];
                }
                ret.Add(wordMat);
            }
            return ret;
        }
        public int[,] multiplySquareMats(int[,] mat1, int[,] mat2)
        {
            int[,] ret = new int[mat1.GetLength(0), mat1.GetLength(0)];
            switch (mat1.GetLength(0))
            {
                case 2:
                    ret[0, 0] = (mat1[0, 0] * mat2[0, 0]) + (mat1[0, 1] * mat2[1, 0]);
                    ret[0, 1] = (mat1[0, 0] * mat2[0, 1]) + (mat1[0, 1] * mat2[1, 1]);
                    ret[1, 0] = (mat1[1, 0] * mat2[0, 0]) + (mat1[1, 1] * mat2[1, 0]);
                    ret[1, 1] = (mat1[1, 0] * mat2[0, 1]) + (mat1[1, 1] * mat2[1, 1]);
                    break;
                case 3:
                    ret[0, 0] = (mat1[0, 0] * mat2[0,0]) + (mat1[0, 1] * mat2[1, 0] )+ (mat1[0, 2] * mat2[2,0]);
                    ret[0, 1] = (mat1[0, 0] * mat2[0, 1]) + (mat1[0, 1] * mat2[1, 1]) + (mat1[0, 2] * mat2[2, 1]);
                    ret[0, 2] = (mat1[0, 0] * mat2[0, 2]) + (mat1[0, 1] * mat2[1, 2]) + (mat1[0, 2] * mat2[2, 2]);
                    ret[1, 0] = (mat1[1, 0] * mat2[0, 0]) + (mat1[1, 1] * mat2[1, 0]) + (mat1[1, 2] * mat2[2, 0]);
                    ret[1, 1] = (mat1[1, 0] * mat2[0, 1]) + (mat1[1, 1] * mat2[1, 1]) + (mat1[1, 2] * mat2[2, 1]);
                    ret[1, 2] = (mat1[1, 0] * mat2[0, 2]) + (mat1[1, 1] * mat2[1, 2]) + (mat1[1, 2] * mat2[2, 2]);
                    ret[2, 0] = (mat1[2, 0] * mat2[0, 0]) + (mat1[2, 1] * mat2[1, 0]) + (mat1[2, 2] * mat2[2, 0]);
                    ret[2, 1] = (mat1[2, 0] * mat2[0, 1]) + (mat1[2, 1] * mat2[1, 1]) + (mat1[2, 2] * mat2[2, 1]);
                    ret[2, 2] = (mat1[2, 0] * mat2[0, 2]) + (mat1[2, 1] * mat2[1, 2]) + (mat1[2, 2] * mat2[2, 2]);
                    break;

            }
            return ret;
        }
        public double[,] multiplySquareMats(double[,] mat1, double[,] mat2)
        {
            double[,] ret = new double[mat1.GetLength(0), mat1.GetLength(0)];
            switch (mat1.GetLength(0))
            {
                case 2:
                    ret[0, 0] = (mat1[0, 0] * mat2[0, 0]) + (mat1[0, 1] * mat2[1, 0]);
                    ret[0, 1] = (mat1[0, 0] * mat2[0, 1]) + (mat1[0, 1] * mat2[1, 1]);
                    ret[1, 0] = (mat1[1, 0] * mat2[0, 0]) + (mat1[1, 1] * mat2[1, 0]);
                    ret[1, 1] = (mat1[1, 0] * mat2[0, 1]) + (mat1[1, 1] * mat2[1, 1]);
                    break;
                case 3:
                    ret[0, 0] = (mat1[0, 0] * mat2[0, 0]) + (mat1[0, 1] * mat2[1, 0]) + (mat1[0, 2] * mat2[2, 0]);
                    ret[0, 1] = (mat1[0, 0] * mat2[0, 1]) + (mat1[0, 1] * mat2[1, 1]) + (mat1[0, 2] * mat2[2, 1]);
                    ret[0, 2] = (mat1[0, 0] * mat2[0, 2]) + (mat1[0, 1] * mat2[1, 2]) + (mat1[0, 2] * mat2[2, 2]);
                    ret[1, 0] = (mat1[1, 0] * mat2[0, 0]) + (mat1[1, 1] * mat2[1, 0]) + (mat1[1, 2] * mat2[2, 0]);
                    ret[1, 1] = (mat1[1, 0] * mat2[0, 1]) + (mat1[1, 1] * mat2[1, 1]) + (mat1[1, 2] * mat2[2, 1]);
                    ret[1, 2] = (mat1[1, 0] * mat2[0, 2]) + (mat1[1, 1] * mat2[1, 2]) + (mat1[1, 2] * mat2[2, 2]);
                    ret[2, 0] = (mat1[2, 0] * mat2[0, 0]) + (mat1[2, 1] * mat2[1, 0]) + (mat1[2, 2] * mat2[2, 0]);
                    ret[2, 1] = (mat1[2, 0] * mat2[0, 1]) + (mat1[2, 1] * mat2[1, 1]) + (mat1[2, 2] * mat2[2, 1]);
                    ret[2, 2] = (mat1[2, 0] * mat2[0, 2]) + (mat1[2, 1] * mat2[1, 2]) + (mat1[2, 2] * mat2[2, 2]);
                    break;

            }
            return ret;
        }
        public int get3by3det(int[,] mat)
        {
            return (mat[0, 0] * (mat[1, 1] * mat[2, 2] - mat[1, 2] * mat[2, 1])) - (mat[0, 1] * (mat[1, 0] * mat[2, 2] - mat[1, 2] * mat[2, 0])) + (mat[0, 2] * (mat[1, 0] * mat[2, 1] - mat[1, 1] * mat[2, 0]));
        }
        public double get3by3det(double[,] mat)
        {
            return (mat[0, 0] * (mat[1, 1] * mat[2, 2] - mat[1, 2] * mat[2, 1])) - (mat[0, 1] * (mat[1, 0] * mat[2, 2] - mat[1, 2] * mat[2, 0])) + (mat[0, 2] * (mat[1, 0] * mat[2, 1] - mat[1, 1] * mat[2, 0]));
        }
        public int[,] transpose(int[,] mat)
        {
            int[,] ret = new int[mat.GetLength(0), mat.GetLength(1)];
            for(int i = 0; i<mat.GetLength(0); i++)
            {
                for(int j = 0; j<mat.GetLength(1); j++)
                {
                    ret[j, i] = mat[i, j];
                }
            }
            return ret;
        }
        public double[,] transpose(double[,] mat)
        {
            double[,] ret = new double[mat.GetLength(0), mat.GetLength(1)];
            for (int i = 0; i < mat.GetLength(0); i++)
            {
                for (int j = 0; j < mat.GetLength(1); j++)
                {
                    ret[j, i] = mat[i, j];
                }
            }
            return ret;
        }
        public double[,] getInverseOfMatrix(double[,] mat)
        {
            if (mat.GetLength(0)== 2)
            {
                double[,] ret = new double[2, 2];
                double det = 1 / ((mat[0, 0] * mat[1, 1]) - (mat[0, 1] * mat[1,0]));
                ret[0, 0] = Math.Round(det * mat[1, 1]);
                ret[1, 1] = Math.Round(det * mat[0, 0]);
                ret[0, 1] =Math.Round( -(det * mat[0, 1]));
                ret[1, 0] =Math.Round(-(det * mat[1, 0]));
                double[,] identity = multiplySquareMats(ret, mat);
                if (identity[0,0] == 1 && identity[0,1] == 0 && identity[1,0] == 0 && identity[1,1] == 1)
                {
                    return ret;
                }
                else
                {
                    throw new InvalidAnlysisException();
                }

            }
            else
            {
                double[,] ret = new double[3, 3];
                double det = get3by3det(mat);
                ret[0, 0] = (mat[1, 1] * mat[2, 2] - mat[1, 2] * mat[2, 1]);
                ret[0, 1] = -(mat[1, 0] * mat[2, 2] - mat[1, 2] * mat[2, 0]);
                ret[0, 2] =  (mat[1, 0] * mat[2, 1] - mat[1, 1] * mat[2, 0]);
                ret[1, 0] = -(mat[0, 1] * mat[2, 2] - mat[0, 2] * mat[2, 1]);
                ret[1, 1] =  (mat[0, 0] * mat[2, 2] - mat[0, 2] * mat[2, 0]);
                ret[1, 2] = -(mat[0, 0] * mat[2, 1] - mat[0, 1] * mat[2, 0]);
                ret[2, 0] =  (mat[0, 1] * mat[1, 2] - mat[0, 2] * mat[1, 1]);
                ret[2, 1] = -(mat[0, 0] * mat[1, 2] - mat[0, 2] * mat[1, 0]);
                ret[2, 2] =  (mat[0, 0] * mat[1, 1] - mat[0, 1] * mat[1, 0]);
                double[,] outputMatrix = transpose(ret);
                ret = transpose(ret);
                for(int i = 0; i < ret.GetLength(0); i++)
                {
                    for(int j = 0; j < ret.GetLength(1); j++)
                    {
                        ret[i, j] = (1 / det) * ret[i, j];
                    }
                }
                double[,] identity = multiplySquareMats(ret, mat);
                for(int i = 0; i< identity.GetLength(0); i++)
                {
                    for(int j = 0; j<identity.GetLength(1); j++)
                    {
                        identity[i, j] =Math.Round(identity[i, j]);
                    }
                }
                if (identity[0,0] == 1 && identity[0,1] == 0 && identity[0,2] == 0 && identity[1,0] ==0 && identity[1,1] == 1 && identity[1,2] == 0 && identity[2,0] == 0 && identity[2,1] == 0 && identity[2,2] == 1)
                {
                    return outputMatrix;
                }
                else
                {
                    throw new InvalidAnlysisException();
                }
                
            }
        }
        public int getModularMultiplicativeInverse(int num, int m)
        {
            if (num < 0)
            {
                while (num < 0)
                {
                    num += m;
                }
            }
            int i = 1;
            while(num * i % 26 != 1 && i < m)
            {
                i++;
            }
            return i;
        }
        public bool isValid2By2Matrix(double[,] mat)
        {
            double[,] ret = new double[2, 2];
            double det = 1 / ((mat[0, 0] * mat[1, 1]) - (mat[0, 1] * mat[1, 0]));
            ret[0, 0] = det * mat[1, 1];
            ret[1, 1] = det * mat[0, 0];
            ret[0, 1] = -(det * mat[0, 1]);
            ret[1, 0] = -(det * mat[1, 0]);
            double[,] identity = multiplySquareMats(ret, mat);
            if (identity[0, 0] == 1 && identity[0, 1] == 0 && identity[1, 0] == 0 && identity[1, 1] == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public int[,] multiply2By2Matrices(int[,] mat1, int[,] mat2)
        {
            return new int[,] { { mat1[0, 0] * mat2[0, 0] + mat1[0, 1] * mat2[1, 0], mat1[0, 0] * mat2[0, 1] + mat1[0, 1] * mat2[1, 1] }, { mat1[1, 0] * mat2[0, 0] + mat1[1, 1] * mat2[1, 0], mat1[1, 0] * mat2[0, 1] + mat1[1, 1] * mat2[1, 1] } };
        }
        public double[,] multiplynBynMats(double[,] mat1, double[,] mat2)
        {
            if(mat1.GetLength(1) != mat2.GetLength(0))
            {
                throw new ArgumentException("mat1 columns not equal mat 2 rows");
            }
            double[,] ret = new double[mat1.GetLength(0), mat2.GetLength(0)];
            for(int i = 0; i<mat1.GetLength(0); i++)
            {
                for(int j = 0; j<mat2.GetLength(1); j++)
                {
                    ret[i, j] = 0;
                    for(int k = 0; k<mat2.GetLength(0); k++)
                    {
                        ret[i, j] += mat1[i, k] + mat2[k,j];

                    }
                }
            }
            return ret;
        }
        public bool checkEqual(List<int> l1, List<int> l2)
        {
            if(l1.Count != l2.Count)
            {
                return false;
            }
            for(int i = 0; i<l1.Count; i++)
            {
                if (l1[i] != l2[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}

