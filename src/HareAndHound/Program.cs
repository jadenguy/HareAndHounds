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
            var someoneWins = new List<Board>();
            var q = new Queue<Board>(new Board[] { new Board() });
            while (q.Count > 0)
            {
                var board = q.Dequeue();
                // board.Print().WriteHost();
                if (board.GetHareWin())
                {
                    // someoneWins.Add(board);
                    moveList[board] = new Board[] { };
                }
                else
                {
                    var nextState = board.NextStates().ToArray();
                    if (nextState.Length == 0)
                    {
                        someoneWins.Add(board);
                        moveList[board] = new Board[] { };
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
            var iCanWin = FindMyWins(moveList, someoneWins);
            var iWillLose = FindMyForcedLosses(moveList, iCanWin.Union(someoneWins));
            iWillLose.First().Print().WriteHost();

            // var playerOptions = moveList.GroupBy(k => k.Key.HoundTurn).ToDictionary(k => k.Key, v => v.ToDictionary());
            // (var houndOptions, var hareOptions) = (playerOptions[houndTurn], playerOptions[hareTurn]);
            // var houndLosses = FindMyForcedLosses(houndOptions, hareWin);
            // var houndLossCount = houndLosses.Count();
            // var keepGoing = true;
            // var i = 0;
            // while (keepGoing)
            // {
            //     var hareForcesWin = FindMyWins(hareOptions, houndLosses);
            //     houndLosses = houndLosses
            //         .Union(FindMyForcedLosses(houndOptions, hareForcesWin))
            //         .Distinct()
            //         .ToArray();
            //     keepGoing = houndLosses.Length > houndLossCount;
            //     houndLossCount = houndLosses.Length;
            //     i++;
            // }
            // i.WriteHost();
            return 0;
        }
        private static Board[] FindMyWins(Dictionary<Board, Board[]> myOptions, IEnumerable<Board> theirLosses)
        {
            return myOptions.Where(o => o.Value.Any(m => theirLosses.Contains(m))).Select(h => h.Key).ToArray();
        }

        private static Board[] FindMyForcedLosses(Dictionary<Board, Board[]> myOptions, IEnumerable<Board> theirWins)
        {
            IEnumerable<KeyValuePair<Board, Board[]>> enumerable = myOptions
                            .Where(o => o.Value.All(m => theirWins.Contains(m)));
            Board[] boards = enumerable
                                        .Select(h => h.Key)
                                        .ToArray();
            return boards;
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
    static class DictExtension
    {
        public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> enumerable)
        {
            return enumerable.ToDictionary(k => k.Key, v => v.Value);
        }
    }
}
