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
            var rnd = new Random();

            //var N = 50;//rnd.Next(10,500);

            //var a = new int[N];
            int[] a = { 46,30,24,37,13,19,10,26,16,12,32,33,17,5,27,21,38,43,45,2,18,4,1,9,34,0,11,48,44,14,35,41,31,42,29,36,28,6,15,25,22,39,49,20,23,8,47,40,7,3 };

            //for (var i = 0; i < N; ++i)
            //{
                //a[i] = i;
            //}
            
            //a = a.OrderBy(x => rnd.Next()).ToArray();

            var copyA = (int[])a.Clone();

            var copySortedA = (int[])a.Clone();
            Array.Sort(copySortedA);

            TimSort.DoTimSort(ref a, new Comp());
            
            Console.WriteLine("before");
            Console.WriteLine(string.Join(',', copyA));

            Console.WriteLine("\n\nafter");
            Console.WriteLine(string.Join(',', a));
            Console.WriteLine("need");
            Console.WriteLine(string.Join(',', copySortedA));
            Console.WriteLine("equal? " + (a.SequenceEqual(copySortedA) ? "yes" : "no"));
        }
    }
}