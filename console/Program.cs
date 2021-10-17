using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using src;

namespace runner
{
    public class Comp : IComparer<int>
    {
        public int Compare(int x, int y)
        {
            return x - y;
        }
    }
    
    class Program
    {
        static void Main(string[] args)
        {
            for (int l = 0; l < 10; ++l)
            {
                Task.Run(() =>
                    {
                        for (int k = 0; k < 100000; ++k)
                        {
                            Console.WriteLine(k);
                            Thread.Sleep(1);
                            var rnd = new Random();

                            if (k % 1000 == 0)
                            {
                                GC.Collect();
                            }

                            int N = rnd.Next(10,500);

                            var a = new int[N];

                            for (var i = 0; i < N; ++i)
                            {
                                a[i] = i;
                            }

                            //a = a[0..(N/2)].OrderBy(x => rnd.Next()).ToArray()
                            //    .Concat(
                            //        a[(N/2)..N].OrderBy(x => rnd.Next()).ToArray()
                            //    ).ToArray();
                            //a[40] = 101;

                            a = a.OrderBy(x => rnd.Next()).ToArray();

                            var copyA = (int[])a.Clone();

                            var copySortedA = (int[])a.Clone();
                            Array.Sort(copySortedA);

                            TimSort.DoTimSort(ref a, new Comp());

                            if (a.SequenceEqual(copySortedA)) continue;

                            Console.WriteLine("before");
                            Console.WriteLine(string.Join(',', copyA));

                            Console.WriteLine("\n\nafter");
                            Console.WriteLine(string.Join(',', a));
                            Console.WriteLine("need");
                            Console.WriteLine(string.Join(',', copySortedA));
                            Console.WriteLine("equal? " + (a.SequenceEqual(copySortedA) ? "yes" : "no"));

                            throw new Exception("fuck");
                            break;
                        }
                        Console.WriteLine("done");
                    }
                );
            }

            Thread.Sleep(86400 * 1000);
        }
    }
}

/*

minrun=50
insertion sort: from 0 to 50
insertion sort: from 51 to 99
total runs: 2
(0, 50) 
0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,42,43,44,45,46,47,48,49,50

(51, 99) 
51,52,53,54,55,56,57,58,59,60,61,62,63,64,65,66,67,68,69,70,71,72,73,74,75,76,77,78,79,80,81,82,83,84,85,86,87,89,92,93,96,97,88,90,91,94,95,98,99


merge
newRun 3: size=100; startIndex=0
total runs: 1
(0, 99) 
0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,42,43,44,45,46,47,48,49,50,51,52,53,54,55,56,57,58,59,60,61,62,63,64,65,66,67,68,69,70,71,72,73,74,75,76,77,78,79,80,81,82,83,84,85,86,87,89,92,93,96,97,88,90,91,94,95,98,99




after
0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,42,43,44,45,46,47,48,49,50,51,52,53,54,55,56,57,58,59,60,61,62,63,64,65,66,67,68,69,70,71,72,73,74,75,76,77,78,79,80,81,82,83,84,85,86,87,89,92,93,96,97,88,90,91,94,95,98,99

*/