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
        public bool GetHareWin()
        {
            if (Spaces[1, 0] == Board.SpaceState.Hare)
            {
                return true;
            }
            var ret = false;
            var keepGoing = true;
            for (int y = 0; keepGoing && y < 5; y++)
            {
                keepGoing = !Enumerable.Range(0, 3).Select(x => StateAtAddress(Spaces, (x, y))).Contains(dog);
                ret = Enumerable.Range(0, 3).Select(x => StateAtAddress(Spaces, (x, y))).Contains(rabbit);
            }
            return ret;
        }
        public readonly bool HoundTurn = true;
        public Board(SpaceState[,] spaces, bool houndTurn) => (this.spaces, this.HoundTurn) = (spaces, houndTurn);
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
        private SpaceState turn => (HoundTurn ? dog : rabbit);
        public IEnumerable<(int x, int y)> GetAllAddresses()
            => Enumerable.Range(0, 3).SelectMany(x => Enumerable.Range(0, 5).Select(y => (x, y)));
        public SpaceState[,] Spaces { get => spaces; }

        public IEnumerable<Board> NextStates()
        {
            IEnumerable<(int x, int y)> enumerable = GetAllAddresses()
                            .Where(position => (StateAtAddress(Spaces, position) == turn)).ToArray();
            return enumerable
                            .SelectMany(address => MovesFromHere(address));
        }
        private IEnumerable<(int x, int y)> AddressesAround((int x, int y) position)
        {
            var addressList = new List<(int, int)>();
            var xList = new List<int>();
            var yList = new List<int>();
            (var x, var y) = position;
            if (x > 0) { xList.Add(x - 1); }
            if (x < 2) { xList.Add(x + 1); }
            if (y > 0) { yList.Add(y - 1); }
            if (y < 4) { yList.Add(y + 1); }
            addressList.AddRange(xList.Select(newX => (newX, y)));
            addressList.AddRange(yList.Select(newY => (x, newY)));
            if ((x + y) % 2 == 1)
            {
                addressList.AddRange(xList.SelectMany(newX => yList.Select(newY => (newX, newY))));
            }
            return addressList;
        }
        private IEnumerable<Board> MovesFromHere((int x, int y) position)
        {
            var nextSpots = AddressesAround(position).Where(n => n != position);
            var validMoves = nextSpots.Where(s => StateAtAddress(Spaces, s) == blank);
            foreach (var move in validMoves)
            {
                SpaceState[,] nextBoardState = Spaces.Clone() as SpaceState[,];
                SpaceState[,] spaces1 = Swap(nextBoardState, position, move);
                yield return new Board(spaces1, !HoundTurn);
            }
        }
        private static SpaceState[,] Swap(SpaceState[,] nextBoardState, (int x, int y) position, (int x, int y) move)
        {
            (nextBoardState[move.x, move.y]
            , nextBoardState[position.x, position.y])
                = (nextBoardState[position.x, position.y]
                , nextBoardState[move.x, move.y]);
            return nextBoardState;

        }
        private static SpaceState StateAtAddress(SpaceState[,] spaces, (int x, int y) position)
            => spaces[position.x, position.y];
        public string Print()
        {
            var ret = new StringBuilder();
            var rows = GetAllAddresses().GroupBy(e => e.x);
            foreach (var row in rows)
            {
                ret.AppendLine(string.Join(null, row.Select(e => "[" + print[Spaces[e.x, e.y]] + "]")));
            }
            ret.AppendLine((HoundTurn ? "Hound" : "Hare") + "'s Turn");
            return ret.ToString();
        }
        public override string ToString()
            => (HoundTurn ? print[dog] : print[rabbit])
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