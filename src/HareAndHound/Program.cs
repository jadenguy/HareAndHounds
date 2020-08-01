using System;
using System.Linq;

namespace HareAndHound
{
    class Program
    {
        static void Main(string[] args)
        {
            var board = new Board();
            board.Print().WriteHost();
            var next = board.NextStates().ToArray();
            next.Count().WriteHost();
            next.Distinct().Count().WriteHost();
        }
    }
    static class WriterExtension
    {
        public static void WriteHost(this object message)
        {
            System.Diagnostics.Debug.WriteLine(message);
            System.Console.WriteLine(message);
        }
        public static void WriteHost(this object message, object title)
        {
            var mString = message.ToString();
            var tString = title.ToString();
            var mOut = tString + ": " + mString;
            mOut.WriteHost();
        }
    }
}
