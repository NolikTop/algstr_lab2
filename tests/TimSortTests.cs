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
    }
}