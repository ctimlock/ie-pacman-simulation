using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace PacSim
{
    class Coordinate
    {
        public int X { get; set; } = 0;
        public int Y { get; set; } = 0;

        public Coordinate(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
    }

    class GameContainer
    {

        public int PlayerX { get; set; }
        public int PlayerY { get; set; }

        public int GridWidth { get; set; } = 5;
        public int GridHeight { get; set; } = 5;

        public bool IsPlaced { get; set; } = false;
        public bool IsRunning { get; set; } = true;

        public LinkedList<string> PlayerFacing { get; set; }

        public Dictionary<string, Coordinate> CoordDict { get; set; }

        public GameContainer()
        {
            PlayerX = 0;
            PlayerY = 0;

            PlayerFacing = new LinkedList<string>();
            CoordDict = new Dictionary<string, Coordinate>();

            CoordDict.Add("North", new Coordinate(0, 1));
            CoordDict.Add("East", new Coordinate(1, 0));
            CoordDict.Add("South", new Coordinate(0, -1));
            CoordDict.Add("West", new Coordinate(-1, 0));
            PlayerFacing.AddLast("North");
            PlayerFacing.AddLast("East");
            PlayerFacing.AddLast("South");
            PlayerFacing.AddLast("West");
        }

        public GameContainer(int width, int height)
        {
            PlayerX = 0;
            PlayerY = 0;

            GridWidth = width;
            GridHeight = height;

            PlayerFacing = new LinkedList<string>();
            CoordDict = new Dictionary<string, Coordinate>();

            CoordDict.Add("North", new Coordinate(0, 1));
            CoordDict.Add("East", new Coordinate(1, 0));
            CoordDict.Add("South", new Coordinate(0, -1));
            CoordDict.Add("West", new Coordinate(-1, 0));
            PlayerFacing.AddLast("North");
            PlayerFacing.AddLast("East");
            PlayerFacing.AddLast("South");
            PlayerFacing.AddLast("West");
        }

        public void InvalidInput()
        {
            Console.WriteLine("Invalid input. Please try again.");
        }

        public void TurnRight()
        {
            string temp = PlayerFacing.First();
            PlayerFacing.RemoveFirst();
            PlayerFacing.AddLast(temp);
        }

        public void TurnLeft()
        {
            string temp = PlayerFacing.Last();
            PlayerFacing.RemoveLast();
            PlayerFacing.AddFirst(temp);
        }

        public void Report()
        {
            Console.WriteLine(PlayerX + ", " + PlayerY + ", " + PlayerFacing.First());
        }

        public void Move()
        {
            Coordinate request = CoordDict.GetValueOrDefault(PlayerFacing.First());

            if (PlayerX + request.X >= 0 && PlayerX + request.X <= GridWidth - 1)
            {
                PlayerX += request.X;
            }

            if (PlayerY + request.Y >= 0 && PlayerY + request.Y <= GridHeight - 1)
            {
                PlayerY += request.Y;
            }
        }

        public static void Main(String[] args) 
        {
            GameContainer game = new GameContainer(5, 5);

            game.InvalidPlace();

            while (game.IsRunning)
            {
                game.GameLoop();
            }
        }

        public void Place(string input)
        {
            input = input.ToLower();
            char[] separators = new char[] { ' ', '.' };
            string[] contents = input.Split(separators, StringSplitOptions.TrimEntries);

            if (contents.Length != 4)
            {
                InvalidPlace();
                return;
            }

            int xTarget;
            int yTarget;

            try
            {
                xTarget = Convert.ToInt32(contents[1]);
                yTarget = Convert.ToInt32(contents[2]);
            } catch {
                InvalidPlace();
                return;
            };

            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            string dirTarget = textInfo.ToTitleCase(contents[3]);

            if (PlayerFacing.Contains(dirTarget) 
                && Enumerable.Range(0, GridWidth - 1).Contains(xTarget) 
                && Enumerable.Range(0, GridHeight - 1).Contains(yTarget))
            {
                PlayerX = xTarget;
                PlayerY = yTarget;
                while (PlayerFacing.First() != dirTarget)
                {
                    TurnRight();
                }

                IsPlaced = true;
            } else
            {
                InvalidPlace();
            }
        }

        public void GameMove(string input)
        {
            switch (input.Substring(0, 2).ToLower())
            {
                case "mo":
                    Move();
                    break;

                case "le":
                    TurnLeft();
                    break;

                case "re":
                    Report();
                    break;

                case "ri":
                    TurnRight();
                    break;

                case "ex":
                    IsRunning = false;
                    break;

                default:
                    InvalidInput();
                    break;
            }
        }

        public void GameLoop()
        {
            string input = Console.ReadLine();
            if (!string.IsNullOrEmpty(input))
            {
                switch (input.Substring(0,1).ToLower())
                {
                    case "p":
                        Place(input);
                        break;

                    default:
                        if (IsPlaced)
                        {
                            GameMove(input);
                        } else
                        {
                            InvalidPlace();
                        }
                        break;
                }
            } else
            {
                InvalidInput();
            }
        }

        private void InvalidPlace()
        {
            System.Console.WriteLine("Please enter a place command in the format 'Place xx, yy, facing'");
        }
    }
}