using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace src
{
    public static class TimSort 
    {
        private static int GetMinrun(int n)
        {
            var r = 0;
            
            while (n >= 64)
            {
                r |= n & 1;
                n >>= 1;
            }

            return n + r;
        }

        private static void DumpRuns(Stack<(int StartIndex, int Size)> runs)
        {
            Console.WriteLine("total runs: " + runs.Count);
            foreach(var run in runs.Reverse())
            {
                Console.Write("(" + run.StartIndex + ", " + (run.StartIndex + run.Size - 1) + ") ");
            }
            Console.WriteLine("");
        }
        
        public static void Sort<T>(ref T[] array, IComparer<T> comparer)
        {
            Stack<(int StartIndex, int Size)> runs = new ();

            var minrun = GetMinrun(array.Length);
            var currentIndex = 0;

            (int StartIndex, int Size)? run;
            while ((run = NextRun(ref array, ref currentIndex, minrun, comparer)) != null)
            {
                runs.Push(((int StartIndex, int Size))run);
            }
            
            DumpRuns(runs);

            while (runs.Count >= 3)
            {
                var x = runs.Pop();
                var y = runs.Pop();
                var z = runs.Pop();

                if (z.Size > x.Size + y.Size && y.Size > x.Size)
                {
                    runs.Push(z);
                    runs.Push(y);
                    runs.Push(x);
                    
                    DumpRuns(runs);
                    break;
                }

                if (y.Size <= x.Size)
                {
                    runs.Push(z);
                        
                    runs.Push(
                        Merge(ref array, x.StartIndex, x.Size, y.StartIndex, y.Size, comparer)    
                    );
                    
                    DumpRuns(runs);
                }
                else if (z.Size <= x.Size + y.Size)
                {
                    var minSizedRunSize = x.Size > z.Size ? z.Size : x.Size;
                    var minSizedRunStart = x.Size > z.Size ? z.StartIndex : x.StartIndex;
                        
                    runs.Push(x.Size > z.Size ? x : z);
                        
                    runs.Push(
                        Merge(ref array, y.StartIndex, y.Size, minSizedRunStart, minSizedRunSize, comparer)
                    );
                    
                    DumpRuns(runs);
                }
            }
            
            while (runs.Count >= 2)
            {
                var x = runs.Pop();
                var y = runs.Pop();

                if (y.Size > x.Size)
                {
                    runs.Push(y);
                    runs.Push(x);
                    
                    Console.WriteLine("fuck");
                    
                    DumpRuns(runs);
                    break;
                }
                
                runs.Push(
                    Merge(ref array, x.StartIndex, x.Size, y.StartIndex, y.Size, comparer)    
                );
                
                DumpRuns(runs);
            }
        }

        public static (int StartIndex, int Size) Merge<T>(ref T[] array, int startIndex1, int size1, int startIndex2, int size2, IComparer<T> comparer)
        {
            if (startIndex1 > startIndex2) // startIndex1 должен быть перед startIndex2
            {
                (startIndex1, startIndex2) = (startIndex2, startIndex1);
                (size1, size2) = (size2, size1);
            }
            
            Console.WriteLine("merge");
            
            var tempAr = new T[size1];
            Array.Copy(array, startIndex1, tempAr, 0, size1);

            var current1 = 0;
            var current2 = startIndex2;

            for(var currentAr = startIndex1; currentAr <= startIndex2 + size2 - 1; ++currentAr)
            {
                if (current1 == size1)
                {
                    array[currentAr] = array[current2];
                    current2++;
                }
                else if (current2 == startIndex2 + size2)
                {
                    array[currentAr] = tempAr[current1];
                    current1++;
                }
                else
                {

                    if (comparer.Compare(tempAr[current1], array[current2]) > 0)
                    {
                        array[currentAr] = array[current2++];
                    }
                    else
                    {
                        array[currentAr] = tempAr[current1++];
                    }

                }

            }
            
            return (startIndex1, size1 + size2);
        }

        public static void InsertionSort<T>(ref T[] array, int fromIndex, int toIndex, IComparer<T> comparer)
        {
            Console.WriteLine("insertion sort: from " + fromIndex + " to " + toIndex);

            T val;
            bool flag; 
            for (var i = fromIndex + 1; i <= toIndex; i++) {
                val = array[i];
                flag = false;
                for (var j = i - 1; j >= 0 && !flag; ) {
                    if (comparer.Compare(array[j], val) > 0) {
                        array[j + 1] = array[j];
                        j--;
                        array[j + 1] = val;
                    }
                    else flag = true;
                }
            }

        }

        //todo test
        public static void Reverse<T>(ref T[] array, int fromIndex, int toIndex)
        {
            var n = (toIndex + fromIndex) / 2;
            for (var i = fromIndex; i <= n; ++i)
            {
                (array[i], array[toIndex - i]) = (array[toIndex - i], array[i]);
            }
        }

        private static (int StartIndex, int Size)? NextRun<T>(ref T[] array, ref int currentIndex, int minrun, IComparer<T> comparer)
        {
            var diff = array.Length - currentIndex;
            if (diff < 0)
            {
                return null;
            }
            if (diff == 0)
            {
                return null;
            }
            if(diff == 1)
            {
                return (currentIndex, 1);
            }

            var startIndex = currentIndex;

            var el1 = array[currentIndex++];
            var el2 = array[currentIndex++];

            var size = 2;

            var invert = comparer.Compare(el1, el2) < 0; // >= 0 значит el1 >= el2

            if (invert)
            {
                for(
                    var prev = el2; 
                    currentIndex < array.Length &&
                    comparer.Compare(prev, array[currentIndex]) < 0; 
                    prev = array[currentIndex++], size++) 
                {}
                
                Reverse(ref array, startIndex, currentIndex-1);
            }
            else
            {
                for(
                    var prev = el2; 
                    currentIndex < array.Length &&
                    comparer.Compare(prev, array[currentIndex]) >= 0; 
                    prev = array[currentIndex++], size++) 
                {}
            }

            if (size < minrun)
            {
                while (currentIndex++ < array.Length && size++ < minrun) {}
            }

            InsertionSort(ref array, startIndex, startIndex + size - 1, comparer);

            return (startIndex, size);
        }

    }
}