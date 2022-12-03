namespace Advent_of_Code.Challenge_Solutions.Year_2020
{
    internal class ChallengeSolution12 : ChallengeSolution
    {
        private string[] actions;
        
        public ChallengeSolution12()
        {
            actions = File.ReadAllLines(Utilities.GetFileString("input", 2020, 12));
        }

        public void SolveFirstPart()
        {
            horizontal = vertical = 0;
            direction = EAST;

            foreach (string action in actions)
            {
                MovePlane(action);
            }

            Console.WriteLine(Math.Abs(horizontal) + Math.Abs(vertical));
        }

        public void SolveSecondPart()
        {
            planeHorizontal = planeVertical = 0;
            wpHorizontal = 10;
            wpVertical = 1;

            foreach (string action in actions)
            {
                DoAction(action);
            }
            Console.WriteLine(Math.Abs(planeVertical) + Math.Abs(planeHorizontal));
        }

        private int horizontal, vertical;
        private int planeHorizontal, planeVertical, wpHorizontal, wpVertical;

        public const int EAST = 0, SOUTH = 1, WEST = 2, NORTH = 3;

        private int direction;

        private void DoAction(string action)
        {
            char command = action[0];
            int amount = Convert.ToInt32(action.Substring(1));

            if (command == 'N')
            {
                wpVertical += amount;
            }
            else if (command == 'S')
            {
                wpVertical -= amount;
            }
            else if (command == 'W')
            {
                wpHorizontal -= amount;
            }
            else if (command == 'E')
            {
                wpHorizontal += amount;
            }
            else if (command == 'F')
            {
                planeVertical += amount * wpVertical;
                planeHorizontal += amount * wpHorizontal;
            }
            else
            {
                if (amount == 180)
                {
                    wpVertical = -wpVertical;
                    wpHorizontal = -wpHorizontal;
                }
                else
                {
                    if (command == 'R')
                    {
                        if (amount == 90)
                        {
                            int aux = wpVertical;
                            wpVertical = -wpHorizontal;
                            wpHorizontal = aux;
                        }
                        else if (amount == 270)
                        {
                            int aux = wpVertical;
                            wpVertical = wpHorizontal;
                            wpHorizontal = -aux;
                        }
                    }
                    else if (command == 'L')
                    {
                        if (amount == 90)
                        {
                            int aux = wpVertical;
                            wpVertical = wpHorizontal;
                            wpHorizontal = -aux;
                        }
                        else if (amount == 270)
                        {
                            int aux = wpVertical;
                            wpVertical = -wpHorizontal;
                            wpHorizontal = aux;
                        }
                    }
                }
            }
        }

        private void TurnPlane(char dir, int amount)
        {
            if (amount == 180)
            {
                switch (direction)
                {
                    case NORTH:
                        direction = SOUTH;
                        break;
                    case WEST:
                        direction = EAST;
                        break;
                    case SOUTH:
                        direction = NORTH;
                        break;
                    case EAST:
                        direction = WEST;
                        break;
                }
            }
            else if (dir == 'L')
            {
                switch (amount)
                {
                    case 90:
                        switch (direction)
                        {
                            case NORTH:
                                direction = WEST;
                                break;
                            case WEST:
                                direction = SOUTH;
                                break;
                            case SOUTH:
                                direction = EAST;
                                break;
                            case EAST:
                                direction = NORTH;
                                break;
                        }
                        break;
                    case 270:
                        switch (direction)
                        {
                            case NORTH:
                                direction = EAST;
                                break;
                            case WEST:
                                direction = NORTH;
                                break;
                            case SOUTH:
                                direction = WEST;
                                break;
                            case EAST:
                                direction = SOUTH;
                                break;
                        }
                        break;
                }
            }
            else if (dir == 'R')
            {
                switch (amount)
                {
                    case 90:
                        switch (direction)
                        {
                            case NORTH:
                                direction = EAST;
                                break;
                            case EAST:
                                direction = SOUTH;
                                break;
                            case SOUTH:
                                direction = WEST;
                                break;
                            case WEST:
                                direction = NORTH;
                                break;
                        }
                        break;
                    case 270:
                        switch (direction)
                        {
                            case NORTH:
                                direction = WEST;
                                break;
                            case WEST:
                                direction = SOUTH;
                                break;
                            case SOUTH:
                                direction = EAST;
                                break;
                            case EAST:
                                direction = NORTH;
                                break;
                        }
                        break;
                }
            }
        }

        private void MovePlane(string action)
        {
            char dir = action[0];
            int amount = Convert.ToInt32(action.Substring(1));

            if (dir == 'N')
            {
                vertical += amount;
            }
            else if (dir == 'S')
            {
                vertical -= amount;
            }
            else if (dir == 'W')
            {
                horizontal -= amount;
            }
            else if (dir == 'E')
            {
                horizontal += amount;
            }
            else if (dir == 'F')
            {
                switch (direction)
                {
                    case EAST:
                        horizontal += amount;
                        break;
                    case WEST:
                        horizontal -= amount;
                        break;
                    case NORTH:
                        vertical += amount;
                        break;
                    case SOUTH:
                        vertical -= amount;
                        break;
                }
            }
            else
                TurnPlane(dir, amount);
        }
    }
}
