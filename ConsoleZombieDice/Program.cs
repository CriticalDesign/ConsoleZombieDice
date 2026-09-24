using System.Runtime.InteropServices;

namespace ConsoleZombieDice
{
    internal class Program
    {

        public enum DiceSide
        {
            Brain,
            Shotgun,
            Footsteps
        }

        public enum DieColor
        {
            Green,
            Yellow,
            Red
        }


        static void Main(string[] args)
        {

            List<Die> gameDice = new List<Die>();
            ReloadGameDice(gameDice);

            Random.Shared.Shuffle(CollectionsMarshal.AsSpan(gameDice));

            int player1Score = 0;
            int player2Score = 0;
            int currentPlayer = 1;

            List<Die> playerCurrentDice = new List<Die>();
            List<Die> playerCurrentShotguns = new List<Die>();
            List<Die> playerCurrentBrains = new List<Die>();
            bool keepGoing = true;

            do
            {
                int currentDiceCount = playerCurrentDice.Count;
                for (int i = 0; i < 3 - currentDiceCount; i++)
                {
                    playerCurrentDice.Add(gameDice[i]);
                    gameDice.RemoveAt(i);
                }


                foreach (var die in playerCurrentDice)
                {
                    die.Roll();
                    die.ShowDieResult();

                    if (die.GetSideUp() == DiceSide.Brain)
                    {
                        playerCurrentBrains.Add(die);
                    }
                    else if (die.GetSideUp() == DiceSide.Shotgun)
                    {
                        playerCurrentShotguns.Add(die);
                    }
                }


                Console.WriteLine("Player " + currentPlayer + " Brains: " + playerCurrentBrains.Count + ", Shotgun blasts: " + playerCurrentShotguns.Count);
                
                if (playerCurrentShotguns.Count >= 3)
                {
                    Console.WriteLine("You have been shotgunned! You lose all your brains.");
                    playerCurrentBrains.Clear();
                    playerCurrentShotguns.Clear();
                    playerCurrentDice.Clear();
                    ReloadGameDice(gameDice);
                    if(currentPlayer == 1)
                    {
                        currentPlayer = 2;
                    }
                    else
                    {
                        currentPlayer = 1;
                    }
                }
                else
                {
                    Console.WriteLine("Do you want to roll again? (y/n)");
                    string input = Console.ReadLine();
                    if (input.ToLower() != "y")
                    {
                        playerCurrentBrains.Clear();
                        playerCurrentShotguns.Clear();
                        playerCurrentDice.Clear();
                        ReloadGameDice(gameDice);
                        //keepGoing = false;
                        if (currentPlayer == 1)
                        {
                            AddBrainsToPlayerScore(ref player1Score, playerCurrentBrains.Count);   //convert this to a numPlayers array
                            currentPlayer = 2;
                        }
                        else
                        {
                            AddBrainsToPlayerScore(ref player2Score, playerCurrentBrains.Count);
                            currentPlayer = 1;
                        }
                    }
                }

            } while (keepGoing && !(playerCurrentShotguns.Count >= 3));
        }

        public static void AddBrainsToPlayerScore(ref int playerScore, int brains)
        {
            playerScore += brains;
        }

        public static void ReloadGameDice(List<Die> dice)
        {
            dice.Clear();
            dice.AddRange(new List<Die>
            {
                new GreenDie(), new GreenDie(), new GreenDie(), new GreenDie(), new GreenDie(), new GreenDie(), new YellowDie(), new YellowDie(), new YellowDie(), new YellowDie(), new RedDie(), new RedDie(), new RedDie(),
            });
        }




        //Object classes to support the game.

        internal abstract class Die
        {
            protected DiceSide[] Sides;
            protected DiceSide SideUp;
            protected Die()
            {
                Sides = new DiceSide[6];
                SideUp = Sides[0];
            }
            public void Roll()
            {
                Random rand = new Random();
                int rollIndex = rand.Next(0, 6);
                SideUp = Sides[rollIndex];
            }

            public DiceSide GetSideUp()
            {
                return SideUp;
            }

            public abstract void ShowDieResult();
            public abstract DieColor GetDieColor();
        }

        internal class GreenDie : Die
        {
            public GreenDie()
            {
                Sides = new DiceSide[6]
                {
                    DiceSide.Brain,
                    DiceSide.Brain,
                    DiceSide.Brain,
                    DiceSide.Shotgun,
                    DiceSide.Footsteps,
                    DiceSide.Footsteps
                };
            }
            public override void ShowDieResult()
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(SideUp);
                Console.ResetColor();
            }
            public override DieColor GetDieColor()
            {
                return DieColor.Green;
            }
        }

        internal class YellowDie : Die
        {
            public YellowDie()
            {
                Sides = new DiceSide[6]
                {
                    DiceSide.Brain,
                    DiceSide.Brain,
                    DiceSide.Shotgun,
                    DiceSide.Shotgun,
                    DiceSide.Footsteps,
                    DiceSide.Footsteps
                };
            }

            public override void ShowDieResult()
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(SideUp);
                Console.ResetColor();
            }
            public override DieColor GetDieColor()
            {
                return DieColor.Yellow;
            }
        }

        internal class RedDie : Die
        {
            public RedDie()
            {
                Sides = new DiceSide[6]
                {
                    DiceSide.Brain,
                    DiceSide.Shotgun,
                    DiceSide.Shotgun,
                    DiceSide.Shotgun,
                    DiceSide.Footsteps,
                    DiceSide.Footsteps
                };
            }

            public override void ShowDieResult()
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(SideUp);
                Console.ResetColor();
            }
            public override DieColor GetDieColor()
            {
                return DieColor.Red;
            }
        }



    }
}
