using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using src;

namespace tests
{
    public class Comp : IComparer<int>
    {
        public int Compare(int x, int y)
        {
            return x - y;
        }
    }
    
    public class Tests
    {
        public int[] Array1;
        public int[] Array2;
        public int[] CopyArray1;
        public int[] CopyArray2;
        public readonly Comp Comp = new();

        [SetUp]
        public void Setup()
        {
            Array1 = new[] { 1, 3, 2, 53, 5236, 52, 52, 66, 2,2 ,6, 6, 2, 4 };
            Array2 = new[] { 1, 4, 6, 2, 602, 50, 2, 51, 0 };
            CopyArray1 = (int[])Array1.Clone();
            CopyArray2 = (int[])Array2.Clone();
        }

        private void SortCopyArrays()
        {
            var a = CopyArray1.ToList();
            a.Sort(Comp);
            CopyArray1 = a.ToArray();
            
            a = CopyArray2.ToList();
            a.Sort(Comp);
            CopyArray2 = a.ToArray();
        }

        [Test]
        public void Reverse()
        {
            TimSort.Reverse(ref Array1, 0, Array1.Length - 1);
            Assert.AreEqual(CopyArray1.Reverse().ToArray(), Array1);
            
            TimSort.Reverse(ref Array2, 0, Array2.Length - 1);
            Assert.AreEqual(CopyArray2.Reverse().ToArray(), Array2);
        }

        [Test]
        public void InsertionSort()
        {
            TimSort.InsertionSort(ref Array1, 0, Array1.Length - 1, Comp);
            TimSort.InsertionSort(ref Array2, 0, Array2.Length - 1, Comp);
            SortCopyArrays();
            
            Assert.AreEqual(CopyArray1, Array1);
            Assert.AreEqual(CopyArray2, Array2);
        }

        [Test]
        public void Merge()
        {
            SortCopyArrays();
            var n = CopyArray1.Concat(CopyArray2).ToArray();
            var nList = n.ToList();
            nList.Sort(Comp);
            var sortedN = nList.ToArray();
            
            TimSort.Merge(ref n, 0, CopyArray1.Length, CopyArray1.Length, CopyArray2.Length, Comp);
            
            Assert.AreEqual(n, sortedN);
            
            Console.WriteLine("before: " + string.Join(',', CopyArray1.Concat(CopyArray2).ToArray()));
            Console.WriteLine("after: " + string.Join(',', n));
        }

        [Test]
        public void DoTimSort()
        {
            int[] a = { // очень длинный массив
                78,195,77,39,151,46,83,72,146,152,218,95,53,134,186,14,13,196,139,36,100,67,54,41,37,214,93,179,197,131,138,193,31,108,164,79,19,43,119,145,165,61,103,208,22,94,111,52,133,9,56,194,47,184,168,170,40,189,200,85,190,121,154,132,162,75,163,113,124,110,202,148,175,11,81,68,169,90,141,156,3,211,1,167,215,20,5,59,64,44,142,23,136,204,116,128,49,58,172,63,123,173,28,217,219,38,147,88,199,70,118,26,174,17,29,27,0,198,87,80,35,157,112,57,25,117,181,97,65,15,73,96,62,33,32,135,91,18,50,106,69,115,149,21,120,51,187,137,144,122,206,143,12,153,107,6,207,76,161,182,140,10,150,185,201,71,212,130,92,48,192,66,171,82,98,34,213,178,16,45,183,30,99,109,155,114,105,4,60,176,160,89,129,191,2,180,8,158,55,102,205,104,127,125,166,84,74,24,42,188,101,86,209,7,203,210,126,216,159,177
            };
            var copyA = (int[])a.Clone();
            Array.Sort(copyA, Comp);
            
            TimSort.DoTimSort(ref a, Comp);
            Assert.AreEqual(copyA, a);
            
            a = new[]
            { // длинный массив, но поменьше
                113,102,85,73,4,12,74,81,110,3,68,93,122,47,2,49,61,96,98,0,59,8,29,21,57,66,26,108,114,82,54,14,32,31,121,38,119,75,77,115,94,64,53,42,88,15,97,44,95,60,51,118,34,24,91,104,30,43,67,11,89,40,101,48,7,62,107,84,111,69,13,100,78,16,72,33,37,19,52,65,112,50,1,5,105,36,79,86,58,41,117,70,35,39,99,27,18,22,45,9,90,106,76,83,56,17,10,116,25,6,80,92,23,87,120,109,55,103,46,20,71,28,63
            };
            copyA = (int[])a.Clone();
            Array.Sort(copyA, Comp);
            
            TimSort.DoTimSort(ref a, Comp);
            Assert.AreEqual(copyA, a);
            
            a = new[]
            { // массив размером <64, TimSort должен будет превратиться в обычный InsertionSort
                46,30,24,37,13,19,10,26,16,12,32,33,17,5,27,21,38,43,45,2,18,4,1,9,34,0,11,48,44,14,35,41,31,42,29,36,28,6,15,25,22,39,49,20,23,8,47,40,7,3
            };
            copyA = (int[])a.Clone();
            Array.Sort(copyA, Comp);
            
            TimSort.DoTimSort(ref a, Comp);
            Assert.AreEqual(copyA, a);
        }
    }
}