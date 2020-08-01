using System;
using System.Linq;

namespace HareAndHound
{
    class Program
    {
        static void Main(string[] args)
        {
            var board = new Board();
            Console.WriteLine(board.Print());
            var next = board.NextStates().ToArray();
        }
    }
}
