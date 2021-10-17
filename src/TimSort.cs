using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace src
{
    public static class TimSort 
    {
        public const int GalopSize = 7; 
        
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

                    break;
                }

                if (z.Size >= x.Size + y.Size)
                {
                    (int StartIndex, int Size) newRun;
                    
                    if (z.Size > x.Size)
                    {
                        runs.Push(x);    

                        newRun = Merge(ref array, z.StartIndex, z.Size, y.StartIndex, y.Size, comparer);

                    }
                    else
                    {
                        runs.Push(z);    

                        newRun = Merge(ref array, x.StartIndex, x.Size, y.StartIndex, y.Size, comparer);
                    }
                    
                    
                    runs.Push(
                        newRun
                    );
   
                }
                else
                {
                    var newRun = Merge(ref array, y.StartIndex, y.Size, x.StartIndex, x.Size, comparer);
                    runs.Push(
                        newRun
                    );
                    
                    runs.Push(z);
                }
            }
            
            while (runs.Count >= 2)
            {
                var x = runs.Pop();
                var y = runs.Pop();

                var newRun = Merge(ref array, x.StartIndex, x.Size, y.StartIndex, y.Size, comparer);

                runs.Push(
                    newRun    
                );
            }
        }

        public static (int StartIndex, int Size) Merge<T>(ref T[] array, int startIndex1, int size1, int startIndex2, int size2, IComparer<T> comparer)
        {
            if (startIndex1 > startIndex2) // startIndex1 должен быть перед startIndex2
            {
                (startIndex1, startIndex2) = (startIndex2, startIndex1);
                (size1, size2) = (size2, size1);
            }
            
            var tempAr = new T[size1];
            Array.Copy(array, startIndex1, tempAr, 0, size1);

            var current1 = 0;
            var current2 = startIndex2;

            var lastFromLeft = false;
            var count = 0;

            var until = startIndex2 + size2 - 1;
            
            for(var currentAr = startIndex1; currentAr <= until; ++currentAr)
            {
                if (current1 == size1)
                {
                    if (lastFromLeft)
                    {
                        lastFromLeft = false;
                        count = 0;
                    }
                    
                    array[currentAr] = array[current2];
                    current2++;
                }
                else if (current2 == startIndex2 + size2)
                {
                    if (!lastFromLeft)
                    {
                        lastFromLeft = true;
                        count = 0;
                    }
                    
                    array[currentAr] = tempAr[current1];
                    current1++;
                }
                else
                {
                    if (comparer.Compare(tempAr[current1], array[current2]) > 0)
                    {
                        if (lastFromLeft)
                        {
                            lastFromLeft = false;
                            count = 0;
                        }

                        array[currentAr] = array[current2++];
                    }
                    else
                    {
                        if (!lastFromLeft)
                        {
                            lastFromLeft = true;
                            count = 0;
                        }
                        
                        array[currentAr] = tempAr[current1++];
                    }

                }
                
                count++;

                if (count != GalopSize) continue;

                currentAr++;
                
                
                //Console.WriteLine("left run: ");
                DumpRun(ref array, (currentAr, array.Length - currentAr));

                if (lastFromLeft)
                {
                    //Console.WriteLine("right run: ");
                    DumpRun(ref tempAr, (current1, tempAr.Length - current1));
                    
                    //Console.WriteLine("left");
                    if (current2 == startIndex2 + size2)
                    {
                        count = 0;
                        currentAr--;
                        continue;
                    }
                    
                    Gallop(ref array, ref tempAr, ref currentAr, ref current1, array[current2], comparer);
                }
                else
                {
                    //Console.WriteLine("right run: ");
                    DumpRun(ref array, (current2, array.Length - current2));
                    
                    //Console.WriteLine("right");
                    if (current1 == size1)
                    {
                        count = 0;
                        currentAr--;
                        continue;
                    }

                    Gallop(ref array, ref array, ref currentAr, ref current2, tempAr[current1], comparer);
                }
                lastFromLeft = !lastFromLeft;
                count = 0;
            }

            return (startIndex1, size1 + size2);
        }

        private static void Gallop<T>(ref T[] array, ref T[] source, ref int currentAr, ref int currentSource, T currentOpposite, IComparer<T> comparer)
        {
            //Console.WriteLine("Enter gallop at " + currentSource + " [" + source[currentSource] + "]");

            var startIndex = currentSource;
            
            var i = 0;
            while (currentSource < source.Length && comparer.Compare(source[currentSource], currentOpposite) < 0)
            {
                //Console.WriteLine("currentSource=" + currentSource);

                i++;
                currentSource += 1 << i;
            }
            
            if (i == 0){
                currentAr--;
                //Console.WriteLine("Nothing changed");
                return; 
            }
            
            currentSource -= 1 << i;

            //Console.WriteLine("made gallop from " + startIndex + " to " + currentSource);
            
            Array.Copy(source, startIndex, array, currentAr, currentSource - startIndex + 1);
            
            currentAr += currentSource - startIndex;

            currentSource++;
        }

        public static void InsertionSort<T>(ref T[] array, int fromIndex, int toIndex, IComparer<T> comparer)
        {
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
        }

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
                while (currentIndex++ < array.Length && size++ < minrun) {}

                if (currentIndex - 1 == array.Length)
                {
                    size++;
                }
                size--;
            }

            currentIndex = startIndex + size;

            InsertionSort(ref array, startIndex, startIndex + size - 1, comparer);

            return (startIndex, size);
        }

        private static void DumpRuns<T>(ref T[] array, Stack<(int StartIndex, int Size)> runs)
        {
            //Console.ForegroundColor = ConsoleColor.Magenta;
            //Console.WriteLine("total runs: " + runs.Count);
            //Console.ResetColor();
            foreach(var run in runs.Reverse())
            {
                DumpRun(ref array, run);
            }
            //Console.WriteLine("");
        }

        private static void DumpRun<T>(ref T[] array, (int StartIndex, int Size) run)
        {
            var (startIndex, size) = run;
            
            //Console.WriteLine("[" + startIndex + ", " + (startIndex + size - 1) + "] ");
            //Console.WriteLine(string.Join(',', array[startIndex..(startIndex + size)]));
            //Console.WriteLine();
        }
    }
}