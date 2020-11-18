using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Day13
{

    class Program
    {
        static void Main(string[] args)
        {

            var grid = File.ReadAllText("input.txt")
                .Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.ToCharArray().Select(c => c).ToList()).ToList();

            // Part 1
            TracksHandler tracks = new TracksHandler(grid);
            while (!tracks.FirstCrash) tracks.MoveCarts();
            Console.WriteLine($"Part 1: Crash at {tracks.CrashPosition.x},{tracks.CrashPosition.y}");

            // Part 2 
            while (tracks.CartsLeft() > 1) tracks.MoveCarts();
            Console.WriteLine($"Part 2: Last Cart at {tracks.GetLastCartPosition.x},{tracks.GetLastCartPosition.y}");

            Console.Read();
        }
    }

    internal class Cart
    {
        public int ID { get; set; }
        public (int y, int x) Pos;
        public char Direction { get; set; }
        public char NextMove { get; set; }
        public bool Crashed { get; set; }

        public Cart()
        {
            NextMove = 'l';
            Crashed = false;
        }

        public void MoveCart(char GridValue)
        {
            if (Direction == '>')
            {
                Pos.x++;
                if (GridValue == '+')
                {
                    if (NextMove == 'l') { Direction = '^'; NextMove = 's'; }
                    else if (NextMove == 's') { Direction = '>'; NextMove = 'r'; }
                    else if (NextMove == 'r') { Direction = 'v'; NextMove = 'l'; }
                }
                if (GridValue == '\\') Direction = 'v';
                if (GridValue == '/') Direction = '^';
                if (GridValue == '-') Direction = '>';
            }
            else if (Direction == '<')
            {
                Pos.x--;
                if (GridValue == '+')
                {
                    if (NextMove == 'l') { Direction = 'v'; NextMove = 's'; }
                    else if (NextMove == 's') { Direction = '<'; NextMove = 'r'; }
                    else if (NextMove == 'r') { Direction = '^'; NextMove = 'l'; }
                }
                if (GridValue == '\\') Direction = '^';
                if (GridValue == '/') Direction = 'v';
                if (GridValue == '-') Direction = '<';
            }
            else if (Direction == '^')
            {
                Pos.y--;
                if (GridValue == '+')
                {
                    if (NextMove == 'l') { Direction = '<'; NextMove = 's'; }
                    else if (NextMove == 's') { Direction = '^'; NextMove = 'r'; }
                    else if (NextMove == 'r') { Direction = '>'; NextMove = 'l'; }
                }
                if (GridValue == '\\') Direction = '<';
                if (GridValue == '/') Direction = '>';
                if (GridValue == '-') Direction = '^';
            }
            else if (Direction == 'v')
            {
                Pos.y++;
                if (GridValue == '+')
                {
                    if (NextMove == 'l') { Direction = '>'; NextMove = 's'; }
                    else if (NextMove == 's') { Direction = 'v'; NextMove = 'r'; }
                    else if (NextMove == 'r') { Direction = '<'; NextMove = 'l'; }
                }
                if (GridValue == '\\') Direction = '>';
                if (GridValue == '/') Direction = '<';
                if (GridValue == '-') Direction = 'v';
            }
        }
    }

    internal class TracksHandler
    {
        private List<List<char>> grid;
        private (int y, int x) MaxValues;
        private List<Cart> carts = new List<Cart>();
        public bool FirstCrash = false;
        public (int y, int x) CrashPosition;
        public int CartsLeft() => carts.Count(x => !x.Crashed);
        public (int y, int x) GetLastCartPosition => carts.Where(x => !x.Crashed).First().Pos;

        public TracksHandler(List<List<char>> grid)
        {
            int i = 1;
            foreach (List<char> y in grid)
            {
                foreach (char c in y)
                {
                    if (c == '<' || c == '>' || c == 'v' | c == '^')
                    {
                        carts.Add(new Cart()
                        {
                            ID = i++,
                            Pos = (grid.IndexOf(y), y.IndexOf(c)),
                            Direction = c
                        });
                    }
                }
            }

            this.grid = grid;
            MaxValues = (y: grid.Count, x: grid[0].Count);
            FixExistingValueForCart();
        }

        private void FixExistingValueForCart()
        {
            foreach (var cart in carts)
            {
                char top = (cart.Pos.y > 0) ? grid[cart.Pos.y - 1][cart.Pos.x] : ' ';
                char bottom = (cart.Pos.y < MaxValues.y) ? grid[cart.Pos.y + 1][cart.Pos.x] : ' ';
                char right = (cart.Pos.x < MaxValues.x) ? grid[cart.Pos.y][cart.Pos.x + 1] : ' ';
                char left = (cart.Pos.x > 0) ? grid[cart.Pos.y][cart.Pos.x - 1] : ' ';

                if (cart.Direction == '>')
                {
                    if ((top == '\\' || top == '|' || top == '/') && (bottom == '\\' || bottom == '|' || bottom == '/')) grid[cart.Pos.y][cart.Pos.x] = '+';
                    else if (top == '|' && right == '-') grid[cart.Pos.y][cart.Pos.x] = '\\';
                    else if (bottom == '|' && right == '-') grid[cart.Pos.y][cart.Pos.x] = '/';
                    else grid[cart.Pos.y][cart.Pos.x] = '-';
                }
                if (cart.Direction == '<')
                {
                    if ((top == '\\' || top == '|' || top == '/') && (bottom == '\\' || bottom == '|' || bottom == '/')) grid[cart.Pos.y][cart.Pos.x] = '+';
                    else if (top == '|' && right == '-') grid[cart.Pos.y][cart.Pos.x] = '/';
                    else if (bottom == '|' && right == '-') grid[cart.Pos.y][cart.Pos.x] = '\\';
                    else grid[cart.Pos.y][cart.Pos.x] = '-';
                }
                if (cart.Direction == '^')
                {
                    if ((top == '\\' || top == '-' || top == '/') && (bottom == '\\' || bottom == '-' || bottom == '/')) grid[cart.Pos.y][cart.Pos.x] = '+';
                    else if (top == '|' && right == '-') grid[cart.Pos.y][cart.Pos.x] = '\\';
                    else if (bottom == '|' && right == '-') grid[cart.Pos.y][cart.Pos.x] = '/';
                    else grid[cart.Pos.y][cart.Pos.x] = '|';
                }
                if (cart.Direction == 'v')
                {
                    if ((top == '\\' || top == '-' || top == '/') && (bottom == '\\' || bottom == '-' || bottom == '/')) grid[cart.Pos.y][cart.Pos.x] = '+';
                    else if (top == '|' && right == '-') grid[cart.Pos.y][cart.Pos.x] = '\\';
                    else if (bottom == '|' && right == '-') grid[cart.Pos.y][cart.Pos.x] = '/';
                    else grid[cart.Pos.y][cart.Pos.x] = '|';
                }
            }
        }

        public void MoveCarts()
        {
            foreach (var cart in carts.Where(x => !x.Crashed))
            {
                if (cart.Direction == '>') cart.MoveCart(grid[cart.Pos.y][cart.Pos.x + 1]);
                else if (cart.Direction == '<') cart.MoveCart(grid[cart.Pos.y][cart.Pos.x - 1]);
                else if (cart.Direction == '^') cart.MoveCart(grid[cart.Pos.y - 1][cart.Pos.x]);
                else if (cart.Direction == 'v') cart.MoveCart(grid[cart.Pos.y + 1][cart.Pos.x]);
                if (!FirstCrash) CheckIfFirstCrash(cart);
                else DisableCartsIfCrashed(cart);
            }
        }

        private void DisableCartsIfCrashed(Cart cart)
        {
            foreach (Cart c in carts.Where(x => (x.ID != cart.ID) && !x.Crashed))
            {
                if ((c.Pos.x == cart.Pos.x) && ((c.Pos.y == cart.Pos.y)))
                {
                    cart.Crashed = true;
                    c.Crashed = true;
                }
            }
        }

        private void CheckIfFirstCrash(Cart cart)
        {
            foreach (Cart c in carts.Where( x => x.ID != cart.ID))
            {
                if ((c.Pos.x == cart.Pos.x) && ((c.Pos.y == cart.Pos.y)))
                {
                    FirstCrash = true;
                    CrashPosition = cart.Pos;
                    cart.Crashed = true;
                    c.Crashed = true;
                }
            }
        }
    }
}
