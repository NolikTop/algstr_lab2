using System;
using System.Collections.Generic;
using System.Linq;
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
            const int N = 100;
            
            int[] a = new int[N];
            for (int i = 0; i < N; ++i)
            {
                a[i] = i;
            }
            /*int[] a =
            {
                33, 22, 27, 25, 30, 80, 11, 96, 95, 13, 45, 35, 8, 56, 5, 77, 1, 49, 44, 88, 76, 92, 98, 38, 26, 91, 10,
                51, 0, 16, 7, 3, 34, 48, 29, 32, 58, 15, 50, 72, 2, 70, 99, 82, 55, 64, 81, 62, 20, 42, 66, 19, 93, 9,
                74, 83, 87, 71, 41, 78, 57, 31, 59, 68, 65, 85, 4, 63, 47, 52, 37, 18, 54, 69, 17, 39, 43, 79, 28, 21,
                53, 67, 12, 23, 97, 61, 89, 6, 94, 60, 14, 73, 75, 24, 86, 84, 36, 90, 40, 46
            };*/

            var copySortedA = (int[])a.Clone();
            Array.Sort(copySortedA);
            
            var rnd = new Random();
            a = a[0..(N/2)].OrderBy(x => rnd.Next()).ToArray()
                .Concat(
                    a[(N/2)..N].OrderBy(x => rnd.Next()).ToArray()
                ).ToArray();  
            a = a.OrderBy(x => rnd.Next()).ToArray();
            
            Console.WriteLine("before");
            Console.WriteLine(string.Join(',', a));

            TimSort.Sort(ref a, new Comp());
            
            Console.WriteLine("\n\nafter");
            Console.WriteLine(string.Join(',', a));
            Console.WriteLine("need");
            Console.WriteLine(string.Join(',', copySortedA));
            Console.WriteLine("equal? " + (a.SequenceEqual(copySortedA) ? "yes" : "no"));
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