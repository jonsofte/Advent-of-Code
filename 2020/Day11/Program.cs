using System;
using System.IO;
using System.Linq;

var seats = File.ReadAllLines("input.txt").Select(l => l.ToCharArray()).ToArray();
ShiftSeats(seats, withVisibleSeat: false);
ShiftSeats(seats, withVisibleSeat: true);

void ShiftSeats(char[][] seats, bool withVisibleSeat)
{
   int numberOfMoves = int.MaxValue;
   while (numberOfMoves > 0) (numberOfMoves,seats) = MovePassengers(seats, withVisibleSeat);
   Console.WriteLine(seats.ToList().SelectMany(x => x).Where(c => c == '#').Count());
}

(int numberOfMoves, char[][] newArrangement) MovePassengers(char[][] seats, bool withVisibleSeat)
{
   int numberOfMoves = 0;
   var newSeats = seats.Select(r => r.ToArray()).ToArray();
   foreach (int r in Enumerable.Range(0, seats.Length))
   {
      foreach (int s in Enumerable.Range(0, seats[r].Length))
      {
         if (seats[r][s] == 'L' && adjacentPassengers(r, s, seats, withVisibleSeat) == 0)
         {
            numberOfMoves++;
            newSeats[r][s] = '#';
         }
         else if (seats[r][s] == '#' && adjacentPassengers(r, s, seats, withVisibleSeat) >= (withVisibleSeat ? 5 : 4))
         {
            numberOfMoves++;
            newSeats[r][s] = 'L';
         }
      }
   }
   return (numberOfMoves, newSeats);
}

int adjacentPassengers(int r, int s, char[][] seats, bool withVisibleSeat)
{
   bool ValidPosition(int rr, int ss) => 0 <= ss && ss < seats[r].Length && 0 <= rr && rr < seats.Length;
   int occupiedSeats = 0;
   foreach (int dr in new int[] { -1, 0, 1 })
   {
      foreach (int ds in new int[] { -1, 0, 1 })
      {
         if (!(dr == 0 && ds == 0))
         {
            int rr = r + dr;
            int ss = s + ds;
            while (withVisibleSeat && ValidPosition(rr, ss) && seats[rr][ss] == '.')
            {
               rr += dr;
               ss += ds;
            }
            if (ValidPosition(rr, ss) && seats[rr][ss] == '#') occupiedSeats++;
         }
      }
   }
   return occupiedSeats;
}