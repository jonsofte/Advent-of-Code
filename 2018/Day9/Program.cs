using System;
using System.Collections.Generic;
using System.Linq;

namespace Day9
{
    class Program
    {
        static void Main(string[] args)
        {
            int numberOfPlayers = 452;
            int numberOfMarbles = 70784;
            CalculateScore(numberOfPlayers, numberOfMarbles);
            numberOfMarbles = 7078400;
            CalculateScore(numberOfPlayers, numberOfMarbles);
            Console.ReadKey();
        }

        private static void CalculateScore(int numberOfPlayers, int numberOfMarbles)
        {
            Marbles marbles = new Marbles();
            List<long> score = new List<long>();

            foreach (int i in Enumerable.Repeat(0, numberOfPlayers + 1))
            {
                score.Add((long)i);
            }

            int currentPlayerID = 1;
            foreach (int i in Enumerable.Range(1, numberOfMarbles))
            {
                score[currentPlayerID] += marbles.InsertMarble(i);
                currentPlayerID = (currentPlayerID >= numberOfPlayers) ? 1 : currentPlayerID + 1;
            }
            Console.WriteLine(score.Max());
        }
    }

    class Marbles
    {
        private const int multiple = 23;
        public LinkedList<long> List { get; set; }
        private LinkedListNode<long> currentMarbleNode;

        public Marbles()
        {
            List = new LinkedList<long>();
            currentMarbleNode = List.AddLast(0);
        }
        
        public long InsertMarble(long number)
        {
            if (number%multiple == 0)
            {
                LinkedListNode<long> node = currentMarbleNode;
                for (int i=0;i<7;i++) { 
                    node = node.Previous ?? List.Last;
                }
                currentMarbleNode = node.Next ?? List.First;
                List.Remove(node);
                return number + node.Value;
            }

            var insertNode = currentMarbleNode.Next ?? List.First;
            currentMarbleNode = List.AddAfter(insertNode,number);
            return 0;
        }
    }
}
