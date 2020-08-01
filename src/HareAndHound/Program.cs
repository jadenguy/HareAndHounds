using System;
using System.Collections.Generic;
using System.Linq;

namespace HareAndHound
{
    class Program
    {
        static int Main(string[] args)
        {
            var moveList = new Dictionary<Board, Board[]>();
            var hareLoss = new List<Board>();
            var hareWin = new List<Board>();
            var q = new Queue<Board>(new Board[] { new Board() });
            while (q.Count > 0)
            {
                var board = q.Dequeue();
                // board.Print().WriteHost();
                if (board.GetHareWin())
                {
                    hareWin.Add(board);
                    moveList[board] = null;
                }
                else
                {
                    var next = board.NextStates().ToArray();
                    if (next.Length == 0)
                    {
                        hareLoss.Add(board);
                    }
                    moveList[board] = next;
                    foreach (var nextBoard in next.Where(n => !moveList.ContainsKey(n) && !q.Contains(n)))
                    {
                        q.Enqueue(nextBoard);
                    }
                }
            }
            // moveList.Count().WriteHost();
            return 0;
        }
    }
    static class WriterExtension
    {
        public static void WriteHost(this object message)
        {
            // return;
            System.Diagnostics.Debug.WriteLine(message);
            System.Console.WriteLine(message);
        }
        public static void WriteHost(this object message, object title)
        {
            // return;
            var mString = message.ToString();
            var tString = title.ToString();
            var mOut = tString + ": " + mString;
            mOut.WriteHost();
        }
    }
}
