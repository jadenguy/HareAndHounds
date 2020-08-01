using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HareAndHound
{
    public class Board
    {
        private static Dictionary<State, char> print = new Dictionary<State, char>() {
            { State.Empty, '_' },
            { State.Invalid, 'X' },
            { State.Hound, 'D' },
            { State.Hare, 'r' } };
        public enum State { Empty, Invalid, Hound, Hare }
        private const State corner = State.Invalid;
        private const State dog = State.Hound;
        private const State rabbit = State.Hare;
        private const State space = State.Empty;
        private State[,] spaces = new State[3, 5];
        private bool houndTurn = true;
        public Board(State[,] spaces, bool houndTurn) => (this.spaces, this.houndTurn) = (spaces, houndTurn);
        public Board()
        {
            Spaces[0, 0] = corner;
            Spaces[2, 0] = corner;
            Spaces[0, 4] = corner;
            Spaces[2, 4] = corner;
            Spaces[0, 1] = dog;
            Spaces[1, 0] = dog;
            Spaces[2, 1] = dog;
            Spaces[1, 4] = rabbit;
        }
        private State turn => (houndTurn ? dog : rabbit);
        public IEnumerable<(int x, int y)> GetAllAddresses()
            => Enumerable.Range(0, 3).SelectMany(x => Enumerable.Range(0, 5).Select(y => (x, y)));
        public State[,] Spaces { get => spaces; }

        public IEnumerable<Board> NextStates()
            => GetAllAddresses().SelectMany((Func<(int x, int y), IEnumerable<Board>>)(address => MovesFromHere((State[,])this.Spaces, address)));
        private IEnumerable<(int, int)> addressesAround((int, int) position)
        {
            (var x, var y) = position;
            int lookLeft = (houndTurn ? 0 : 1);
            var xMin = Math.Max(x - lookLeft, 0);
            var xSize = (x < 4 ? 2 : 1) + (xMin > 0 ? lookLeft : 0);
            var yMin = Math.Max(y - 1, 0);
            var ySize = y == 1 ? 3 : 2;
            var xRange = Enumerable.Range(xMin, xSize);
            var yRange = Enumerable.Range(yMin, ySize);
            return xRange.Join(yRange, x => true, y => true, (x, y) => (x, y));
        }
        private IEnumerable<Board> MovesFromHere(State[,] spaces, (int x, int y) position)
        {
            State state = turn;
            if (stateAtAddress(spaces, position) != state) { yield break; }
            var g = addressesAround(position).ToArray();
            yield return new Board(spaces, !houndTurn);
        }
        private static State stateAtAddress(State[,] spaces, (int x, int y) position)
            => spaces[position.x, position.y];
        public string Print()
        {
            var ret = new StringBuilder();
            var rows = GetAllAddresses().GroupBy(e => e.x);
            foreach (var row in rows)
            {
                ret.AppendLine(string.Join(null, row.Select(e => "[" + print[Spaces[e.x, e.y]] + "]")));
            }
            ret.AppendLine((houndTurn ? "Hound" : "Hare") + "'s Turn");
            return ret.ToString();
        }
        public override string ToString()
            => (houndTurn ? print[dog] : print[rabbit])
                + string.Join(null, Spaces
                        .Cast<State>()
                        .Select(e => print[e]));
        public override bool Equals(object obj)
            => (obj as Board) is null
                ? false
                : obj.ToString() == this.ToString();
        public override int GetHashCode() => this.ToString().GetHashCode();
    }
}