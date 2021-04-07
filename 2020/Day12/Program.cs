using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

var input = File.ReadAllLines("input.txt").Select(x => (Action: x[0], Value: int.Parse(x[1..]))).ToList();
var directions = new List<(char heading, int degrees)>() { ('N', 0), ('E', 90), ('S', 180), ('W', 270) };

MoveShipFromDirections(input);
MoveShipWithWayPoint(input);

void MoveShipWithWayPoint(List<(char, int)> directions)
{
   (int x, int y, int heading) pos = (0, 0, 90);
   (int x, int y) waypoint = (10, 1);

   foreach (var a in input)
   {
      if (a.Action == 'N') waypoint.y += a.Value;
      if (a.Action == 'S') waypoint.y -= a.Value;
      if (a.Action == 'E') waypoint.x += a.Value;
      if (a.Action == 'W') waypoint.x -= a.Value;
      if (a.Action == 'L')
      {
         if (a.Value == 90) waypoint = (-waypoint.y, waypoint.x);
         if (a.Value == 180) waypoint = (-waypoint.x, -waypoint.y);
         if (a.Value == 270) waypoint = (waypoint.y, -waypoint.x);
      }
      if (a.Action == 'R')
      {
         if (a.Value == 90) waypoint = (waypoint.y, -waypoint.x);
         if (a.Value == 180) waypoint = (-waypoint.x, -waypoint.y);
         if (a.Value == 270) waypoint = (-waypoint.y, waypoint.x);
      }
      if (a.Action == 'F')
      {
         pos.x += a.Value * waypoint.x;
         pos.y += a.Value * waypoint.y;
      }
   }
   Console.WriteLine(Math.Abs(pos.x) + Math.Abs(pos.y));
}

void MoveShipFromDirections(List<(char, int)> directions) 
{
   (int x, int y, int heading) pos = (0, 0, 90);
   input.ForEach(i => pos = Move(i));
   Console.WriteLine(Math.Abs(pos.x) + Math.Abs(pos.y));

   (int x, int y, int heading) Move((char Action, int Value) a) => a.Action switch
   {
      'N' => (pos.x, pos.y + a.Value, pos.heading),
      'S' => (pos.x, pos.y - a.Value, pos.heading),
      'E' => (pos.x + a.Value, pos.y, pos.heading),
      'W' => (pos.x - a.Value, pos.y, pos.heading),
      'R' => (pos.x, pos.y, GetNewHeading(pos.heading, a.Value, left: false)),
      'L' => (pos.x, pos.y, GetNewHeading(pos.heading, a.Value, left: true)),
      'F' => Move((getHeadingFromDegrees(pos.heading), a.Value)),
      _ => throw new ApplicationException()
   };
}

char getHeadingFromDegrees(int degrees) => directions.Where(d => d.degrees == degrees).Select(h => h.heading).First();

int GetNewHeading(int currentHeading, int turn, bool left)
{
   var newHeading = currentHeading - ((left? 1 : -1) * turn);
   if (newHeading < 0) newHeading += 360;
   if (newHeading >= 360) newHeading -= 360;
   return newHeading;
}