using System;
using System.Collections.Generic;
using System.Linq;

namespace Day14
{
    class Program
    {
        static void Main(string[] args)
        {
            int input = 170641;
            Part1_GetScoreAfterRecipes(RecipeAfter: input);
            Part2_GetRecipesAfterScore(AfterScore: input);
            Console.ReadKey();
        }

        private static void Part1_GetScoreAfterRecipes(int RecipeAfter)
        {
            Scores scores = GenerateScores(RecipeAfter);
            Console.WriteLine(scores.GetScoreOfTenRecipes(RecipeAfter));
        }

        // Brute force - tar tid
        private static void Part2_GetRecipesAfterScore(int AfterScore)
        {
            Scores scores = GenerateScores(AfterScore*200);
            Console.WriteLine(scores.GetNumberOfRecipes(AfterScore));
        }

        private static Scores GenerateScores(int RecipeAfter)
        {
            Scores scores = new Scores();
            Elf elf1 = new Elf(scores, 3);
            Elf elf2 = new Elf(scores, 7);

            for (int rounds = 2; scores.NumberOfRecipes < RecipeAfter + 10; rounds++)
            {
                var sum = elf1.GetCurrentScore() + elf2.GetCurrentScore();
                List<int> values = sum.ToString().ToCharArray().Select(x => Int32.Parse(x.ToString())).ToList();
                scores.InsertRecipe(values);
                elf1.Move();
                elf2.Move();
            }

            return scores;
        }
    }

    class Scores
    {
        public LinkedList<int> List { get; set; }
        public int NumberOfRecipes { get; private set; }

        public Scores()
        {
            List = new LinkedList<int>();
        }

        public LinkedListNode<int> InsertRecipe(List<int> numbers)
        {
            numbers.ForEach(x => List.AddLast(x));
            NumberOfRecipes += numbers.Count();
            return List.Last;
        }

        internal string GetScoreOfTenRecipes(int recipeAfter)
        {
            string score = "";
            LinkedListNode<int> currentNode = List.First;

            for (int i = 0; i < recipeAfter; i++)
            {
                currentNode = currentNode.Next;
            }

            for (int i = 0; i < 10; i++)
            {
                score += currentNode.Value.ToString();
                currentNode = currentNode.Next;
            }
            return score;
        }

        internal int GetNumberOfRecipes(int afterScore)
        {
            int recipeNumber = 0;
            List<int> values = afterScore.ToString().ToCharArray().Select(x => Int32.Parse(x.ToString())).ToList();
            LinkedListNode<int> currentNode = List.First;

            while (currentNode != null) {
                LinkedListNode<int> CheckNode = currentNode;
                for (int i=0;i<values.Count;i++)
                {
                    if (CheckNode == null) break;
                    if (CheckNode.Value != values[i]) break;
                    CheckNode = CheckNode.Next;
                    if (i == values.Count-1)
                    {
                        return recipeNumber;
                    }
                }
                recipeNumber++;
                currentNode = currentNode.Next;
            }
            return 0;
        }
    }

    class Elf
    {
        private Scores scores;
        private LinkedListNode<int> currentScoreNode;
        public Elf(Scores s, int r)
        {
            scores = s;
            currentScoreNode = scores.InsertRecipe(new List<int> { r });
        }
        public int GetCurrentScore()
        {
            return currentScoreNode.Value;
        }
        public void Move()
        {
            LinkedListNode<int> node = currentScoreNode;
            int numberOfMoves = 1 + node.Value;
            for (int i = 0; i < numberOfMoves; i++)
            {
                node = node.Next ?? scores.List.First;
            }
            currentScoreNode = node;
        }
    }
}
