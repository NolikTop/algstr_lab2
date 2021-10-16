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
            
            var rnd = new Random();
            a = a.OrderBy(x => rnd.Next()).ToArray();  
            
            Console.WriteLine("before");
            Console.WriteLine(string.Join(',', a));

            TimSort.Sort(ref a, new Comp());
            
            Console.WriteLine("\n\nafter");
            Console.WriteLine(string.Join(',', a));
        }
    }
}

/*

before
76,65,0,12,99,84,72,30,44,90,55,7,88,54,48,14,75,94,3,16,81,11,38,13,40,6,66,74,36,33,57,73,60,24,17,87,78,98,1,79,93,69,23,64,28,41,63,85,39,97,9,83,68,47,4,50,19,82,70,71,29,20,61,51,89,95,53,25,27,56,2,37,22,26,96,62,18,34,43,67,92,35,80,31,15,10,59,86,46,42,58,21,45,77,49,52,32,5,8,91
insertion sort: from 0 to 50
insertion sort: from 51 to 99
total runs: 2
(0, 50) (51, 99) 
merge
total runs: 1
(0, 99) 


after
0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,42,43,44,45,46,47,48,49,50,51,52,53,54,55,56,57,58,59,60,61,62,63,64,65,66,67,68,69,70,71,72,73,74,75,76,77,78,79,80,81,82,84,85,87,88,90,93,94,97,98,99,83,86,89,91,92,95,96



*/