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
            //return 32; // remove
            
            var r = 0;
            
            while (n >= 64)
            {
                r |= n & 1;
                n >>= 1;
            }
            
            Console.WriteLine("minrun=" + (n + r));

            return n + r;
        }

        private static void DumpRuns<T>(ref T[] array, Stack<(int StartIndex, int Size)> runs)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("total runs: " + runs.Count);
            Console.ResetColor();
            foreach(var run in runs.Reverse())
            {
                DumpRun(ref array, run);
            }
            Console.WriteLine("");
        }

        private static void DumpRun<T>(ref T[] array, (int StartIndex, int Size) run)
        {
            var (startIndex, size) = run;
            
            Console.WriteLine("[" + startIndex + ", " + (startIndex + size - 1) + "] ");
            Console.WriteLine(string.Join(',', array[startIndex..(startIndex + size)]));
            Console.WriteLine();
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
            
            DumpRuns(ref array, runs);

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("START MERGING!!!");
            Console.ResetColor();

            while (runs.Count >= 3)
            {
                var x = runs.Pop();
                var y = runs.Pop();
                var z = runs.Pop();
                
                Console.Write("x = ");
                RunRangeDump(x);
                Console.WriteLine();
                
                Console.Write("y = ");
                RunRangeDump(y);
                Console.WriteLine();
                
                Console.Write("z = ");
                RunRangeDump(z);
                Console.WriteLine();

                if (z.Size > x.Size + y.Size && y.Size > x.Size)
                {
                    runs.Push(z);
                    runs.Push(y);
                    runs.Push(x);

                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine("break;");
                    Console.ResetColor();
                    
                    DumpRuns(ref array, runs);
                    break;
                }

                if (z.Size >= x.Size + y.Size)
                {
                    (int StartIndex, int Size) newRun;
                    
                    if (z.Size > x.Size)
                    {
                        Console.WriteLine("push x");
                        runs.Push(x);    

                        Console.WriteLine("merge z and y");
                        newRun = Merge(ref array, z.StartIndex, z.Size, y.StartIndex, y.Size, comparer);

                    }
                    else
                    {
                        Console.WriteLine("push z");
                        runs.Push(z);    

                        Console.WriteLine("merge x and y");
                        newRun = Merge(ref array, x.StartIndex, x.Size, y.StartIndex, y.Size, comparer);
                    }
                    
                    Console.WriteLine("newRun: size=" + newRun.Size + "; startIndex=" + newRun.StartIndex);
                    
                    runs.Push(
                        newRun
                    );
   
                    DumpRuns(ref array, runs);
                }
                else
                {
                    
                    Console.WriteLine("merge y and x");
                    var newRun = Merge(ref array, y.StartIndex, y.Size, x.StartIndex, x.Size, comparer);
                    Console.WriteLine("newRun 2: size=" + newRun.Size + "; startIndex=" + newRun.StartIndex);
                    runs.Push(
                        newRun
                    );
                    
                    Console.WriteLine("push z");
                    runs.Push(z);

                    DumpRuns(ref array, runs);
                }
            }
            
            while (runs.Count >= 2)
            {
                var x = runs.Pop();
                var y = runs.Pop();

                var newRun = Merge(ref array, x.StartIndex, x.Size, y.StartIndex, y.Size, comparer);
                
                Console.WriteLine("newRun 3: size=" + newRun.Size + "; startIndex=" + newRun.StartIndex);

                runs.Push(
                    newRun    
                );
                
                DumpRuns(ref array, runs);
            }
        }

        private static void RunRangeDump((int StartIndex, int Size) run)
        {
            var (startIndex, size) = run;

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("[" + startIndex + "; " + (startIndex + size - 1) + "]");
            Console.ResetColor();
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

            var n = toIndex - fromIndex + 1;
            for (var i = 1; i < n; ++i) {
                var key = array[fromIndex + i];
                var j = i - 1;
 
                while (j >= 0 && comparer.Compare(array[fromIndex + j], key) > 0) {
                    array[fromIndex + j + 1] = array[fromIndex + j];
                    j--;
                }
                array[fromIndex + j + 1] = key;
            }

            var copy = (int[])array[fromIndex..(toIndex + 1)].Clone();
            Array.Sort(copy);
            Console.WriteLine("Real sort: \n" + string.Join(",", copy));

        }

        //todo test
        public static void Reverse<T>(ref T[] array, int fromIndex, int toIndex)
        {
            var n = (toIndex - fromIndex) / 2;
            for (var i = 0; i <= n; ++i)
            {
                (array[fromIndex + i], array[toIndex - i]) = (array[toIndex - i], array[fromIndex + i]);
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
                Console.WriteLine("not enough for minrun: " + (minrun - size) + "; current size=" + size);
                
                while (currentIndex++ < array.Length && size++ < minrun) {}

                if (currentIndex - 1 == array.Length)
                {
                    size++;
                }
                size--;
                
                Console.WriteLine("currentIndex=" + currentIndex);
                Console.WriteLine("size=" + size);
            }

            currentIndex = startIndex + size;
            
            Console.WriteLine("cur index at end=" + currentIndex);
            
            DumpRun(ref array, (startIndex, size));

            InsertionSort(ref array, startIndex, startIndex + size - 1, comparer);
            
            DumpRun(ref array, (startIndex, size));

            return (startIndex, size);
        }

    }
}