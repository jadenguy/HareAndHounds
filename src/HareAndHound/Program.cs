using System;
using System.Collections.Generic;
using System.Linq;

namespace HareAndHound
{
    class Program
    {
        static int Main(string[] args)
        {
            const bool hareTurn = false;
            const bool houndTurn = true;
            var moveList = new Dictionary<Board, Board[]>();
            var houndWin = new List<Board>();
            var hareWin = new List<Board>();
            var q = new Queue<Board>(new Board[] { new Board() });
            while (q.Count > 0)
            {
                var board = q.Dequeue();
                // board.Print().WriteHost();
                if (board.GetHareWin())
                {
                    hareWin.Add(board);
                    moveList[board] = new[] { board };
                }
                else
                {
                    var nextState = board.NextStates().ToArray();
                    if (nextState.Length == 0)
                    {
                        houndWin.Add(board);
                        moveList[board] = new[] { board };
                    }
                    else
                    {
                        moveList[board] = nextState;
                        foreach (var nextBoard in nextState.Where(n => !moveList.ContainsKey(n) && !q.Contains(n)))
                        {
                            q.Enqueue(nextBoard);
                        }
                    }
                }
            }
            var playerOptions = moveList.GroupBy(k => k.Key.HoundTurn).ToDictionary(k => k.Key, v => v.ToDictionary(kInner => kInner, vInner => vInner));
            (var houndOptions, var hareOptions) = (playerOptions[houndTurn], playerOptions[hareTurn]);
            var houndLosses = houndOptions.Values
                .Where(o => o.Value.Count() == 1)
                .Where(o => hareWin.Contains(o.Value.First())).ToArray();
            houndLosses.Count().WriteHost();
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
