using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HareAndHound
{
    public class Board
    {
        private static Dictionary<SpaceState, char> print = new Dictionary<SpaceState, char>() {
            { SpaceState.Empty, '_' },
            { SpaceState.Invalid, 'X' },
            { SpaceState.Hound, 'D' },
            { SpaceState.Hare, 'r' } };
        public enum SpaceState { Empty, Invalid, Hound, Hare }
        private const SpaceState corner = SpaceState.Invalid;
        private const SpaceState dog = SpaceState.Hound;
        private const SpaceState rabbit = SpaceState.Hare;
        private const SpaceState blank = SpaceState.Empty;
        private readonly SpaceState[,] spaces = new SpaceState[3, 5];
        private bool houndTurn = true;
        public Board(SpaceState[,] spaces, bool houndTurn) => (this.spaces, this.houndTurn) = (spaces, houndTurn);
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
        private SpaceState turn => (houndTurn ? dog : rabbit);
        public IEnumerable<(int x, int y)> GetAllAddresses()
            => Enumerable.Range(0, 3).SelectMany(x => Enumerable.Range(0, 5).Select(y => (x, y)));
        public SpaceState[,] Spaces { get => spaces; }

        public IEnumerable<Board> NextStates()
        {
            IEnumerable<(int x, int y)> enumerable = GetAllAddresses()
                            .Where(position => (stateAtAddress(Spaces, position) == turn)).ToArray();
            return enumerable
                            .SelectMany(address => MovesFromHere(address));
        }
        private IEnumerable<(int x, int y)> addressesAround((int x, int y) position)
        {
            (var x, var y) = position;
            int lookLeft = (houndTurn && x != 0 ? 0 : 1);
            var xMin = Math.Max(x - 1, 0);
            var xSize = x == 1 ? 3 : 2;
            var xRange = Enumerable.Range(xMin, xSize);
            var yMin = Math.Max(y - lookLeft, 0);
            var ySize = y < 4 ? 2 + lookLeft : 1 + lookLeft;
            var yRange = Enumerable.Range(yMin, ySize);
            return xRange.Join(yRange, x => true, y => true, (x, y) => (x, y));
        }
        private IEnumerable<Board> MovesFromHere((int x, int y) position)
        {
            var nextSpots = addressesAround(position).Where(n => n != position);
            // var view = nextSpots.Select(q =>
            //     {
            //         var g = new State[3, 5];
            //         g[q.x, q.y] = turn;
            //         return new Board(g, houndTurn);
            //     });
            // foreach (var v in view) { v.Print().WriteHost(); }
            // "".WriteHost();
            // "".WriteHost();
            // "Options".WriteHost();
            // "".WriteHost();
            // "".WriteHost();
            var validMoves = nextSpots.Where(s => stateAtAddress(Spaces, s) == blank);
            foreach (var move in validMoves)
            {
                SpaceState[,] nextBoardState = Spaces.Clone() as SpaceState[,];
                SpaceState[,] spaces1 = swap(nextBoardState, position, move);
                yield return new Board(spaces1, !houndTurn);
            }
        }
        private static SpaceState[,] swap(SpaceState[,] nextBoardState, (int x, int y) position, (int x, int y) move)
        {
            (nextBoardState[move.x, move.y]
            , nextBoardState[position.x, position.y])
                = (nextBoardState[position.x, position.y]
                , nextBoardState[move.x, move.y]);
            return nextBoardState;

        }
        private static SpaceState stateAtAddress(SpaceState[,] spaces, (int x, int y) position)
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
                        .Cast<SpaceState>()
                        .Select(e => print[e]));
        public override bool Equals(object obj)
            => (obj as Board) is null
                ? false
                : obj.ToString() == this.ToString();
        public override int GetHashCode() => this.ToString().GetHashCode();
    }
}