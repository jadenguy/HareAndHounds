using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HareAndHound
{
    public class Board
    {
        private char[,] spaces = new char[3, 5];
        private bool houndTurn = true;
        public Board(char[,] spaces) => this.spaces = spaces;
        public Board()
        {
            IEnumerable<(int, int)> alladdresses = GetAllAddresses();
            foreach ((var x, var y) in alladdresses) { spaces[x, y] = '_'; }
            spaces[0, 0] = 'X';
            spaces[2, 0] = 'X';
            spaces[0, 4] = 'X';
            spaces[2, 4] = 'X';
            spaces[0, 1] = 'D';
            spaces[1, 0] = 'D';
            spaces[2, 1] = 'D';
            spaces[1, 4] = 'B';
        }

        public IEnumerable<(int, int)> GetAllAddresses()
            => Enumerable.Range(0, 3).SelectMany(x => Enumerable.Range(0, 5).Select(y => (x, y)));
        public char[,] Spaces { get => spaces; }
        public IEnumerable<Board> NextStates()
        {
            foreach ((var x, var y) in GetAllAddresses())
            {
                foreach (var move in (houndTurn ? HoundMoves(Spaces, x, y) : HareMoves(Spaces, x, y)))
                { yield return move; }
            }
        }
        private IEnumerable<Board> HareMoves(char[,] spaces, int x, int y)
        {
            yield return new Board(spaces);
        }
        private IEnumerable<Board> HoundMoves(char[,] spaces, int x, int y)
        {
            yield return new Board(spaces);
        }
        public string Print()
        {
            var ret = new StringBuilder();
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 5; y++)
                {
                    ret.Append("[" + spaces[x, y] + "]");
                }
                ret.AppendLine();
            }
            ret.Append(houndTurn ? "Hound" : "Hare");
            ret.AppendLine("'s Turn");
            return ret.ToString();
        }
        public override string ToString()
        {
            return spaces.ToString();
        }
    }
}