using System.Numerics;
using System.Runtime.InteropServices;

namespace ConsoleZombieDice
{
    internal class Program
    {

        //enumeration to keep track of the sides of the dice.
        public enum DiceSide
        {
            Brain,
            Shotgun,
            Footsteps
        }

        //keep track of the color of the die for scoring purposes.
        public enum DieColor
        {
            Green,
            Yellow,
            Red
        }


        static void Main(string[] args)
        {
            //create a list of dice to represent the cup of dice for the game.
            List<Die> diceCup = new List<Die>();
            ReloadGameDice(diceCup);    //call the reload method to fill the cup with the standard Zombie Dice set up.

            //some variables to keep track of the player's score, number of games played, total score, and number of deaths.
            int playerScore = 0;
            int numGames = 0;
            int totalScore = 0;
            int deaths = 0; 

            Random _rng = new Random(); //beloved RNG

            //three lists to keep track of the player's current dice, shotguns, and brains. Footsteps will stay in the players hand.
            List<Die> playerCurrentDice = new List<Die>();
            List<Die> playerCurrentShotguns = new List<Die>();
            List<Die> playerCurrentBrains = new List<Die>();


            do
            {
                Console.WriteLine("Number of dice in the cup: " + diceCup.Count + ". Taking " + (3 - playerCurrentDice.Count) + ".");

                int currentDiceCount = playerCurrentDice.Count;     //how many dice to the player currently have in their hand.

                //if there are not enough dice to get the player to 3 dice, put the brains back in the cup - but keep the shotguns.
                if (currentDiceCount + diceCup.Count < 3) {         
                    Console.WriteLine("Not enough dice to get to 3. Putting braiiiiiins back in the cup.");
                    for (int i = playerCurrentBrains.Count - 1; i >= 0; i--)
                    {
                        diceCup.Add(playerCurrentBrains[i]);    //add brains to the "dice" cup.
                        playerCurrentBrains.RemoveAt(i);        //remove from brains. Don't worry, we have the number of brains store in playerScore.
                    }
                }

                //now that we are certain there are enough dice to get the player to 3, load up to 3.
                while(playerCurrentDice.Count < 3)      //reload to 3 if necessary.
                {
                    playerCurrentDice.Add(diceCup[diceCup.Count - 1]);              //add dice from the cup to the player's hand.
                    diceCup.RemoveAt(diceCup.Count - 1);                            //remove the dice from the cup.
                }


                //this is the core game loop
                for(int i = playerCurrentDice.Count - 1; i >= 0; i--)  //for each die in the player's hand, roll it and check the result.
                {
                    Die die = playerCurrentDice[i]; //get the die from the player's hand.

                    die.Roll();                     //roll it
                    die.ShowDieResult();            //show the result

                    if (die.GetSideUp() == DiceSide.Brain)  //it's a brain!
                    {
                        playerCurrentBrains.Add(die);   //add it to the player's brain list. (like setting it to the side)
                        playerCurrentDice.Remove(die);  //remove the die from the player's hand since it is now a brain.
                        playerScore++;                  //add to the players possible score.
                    }
                    else if (die.GetSideUp() == DiceSide.Shotgun)       //oh no! It's a shotgun blast!
                    {
                        playerCurrentShotguns.Add(die); //add it to the player's shotgun list. (like setting it to the side)
                        playerCurrentDice.Remove(die);  //remove the die from the player's hand since it is now a shotgun.
                    }
                }


                //some helpful output.
                Console.WriteLine("Brains: " + playerScore + ", Shotgun blasts: " + playerCurrentShotguns.Count);
                Console.WriteLine("Number of dice in player hand: " + playerCurrentDice.Count);


                //player has received 3 shotgun blasts. Turn must end. No brains scored.
                if (playerCurrentShotguns.Count >= 3)  
                {
                    Console.WriteLine("You have been shotgunned! You lose all your brains. Starting over.....");
                    Console.WriteLine("-------------------------------------------------------------------------------------");
                    playerCurrentBrains.Clear();    //reset the current brains from the round, not stored brains from previous rounds.
                    playerCurrentShotguns.Clear();  //reset shotguns for the next round.
                    playerCurrentDice.Clear();      //reset player dice - whatever was left over from the roll this round.
                    ReloadGameDice(diceCup);        //reload the game dice for the next round.
                    playerScore = 0;                //reset the score for the next round.
                    numGames++;                     //increment the number of games played.
                    deaths++;                       //increment the number of deaths for the player.
                }
                else
                {
                    Console.Write("Do you want to roll again? (y/n) ");
                    string input = Console.ReadLine();  //comment this out when you are ready to test your AI.

                    //AI - your AI should be deciding to re-roll or not re-roll. So, all decisions need to end with a y or n value for
                    //variable input.

                    //You can keep track of your shotguns with playerCurrentShotguns.Count and your brains with playerScore.
                    //Footsteps will be kept in your hand playerCurrentDice.Count. You can also check the number of dice left in the cup with diceCup.Count.

                    //YOU ARE NOT ALLOWED TO PEEK DIRECTLY IN THE CUP TO SEE WHAT COLOURS OF DICE ARE LEFT. This is considered cheating. You can 
                    //keep track of this by looking at the dice stored in playerCurrentBrains, playerCurrentShotguns, and playerCurrentDice and doing the math 
                    //to figure out what's left in the cup.




                    //Sample "AI" code - be sure to comment out the line above that reads user input before uncommenting this.
                    /*
                    string input = "";
                    if(_rng.Next(0,2) == 0)
                    {
                        input = "y";
                    }
                    else
                    {
                        input = "n";
                    }

                    Console.WriteLine(input);
                    */






                    //If the player or AI decides to stop rolling, we score the brains and start a new round.
                    if (input.ToLower() == "n")
                    {
                        totalScore += playerScore;      //add the current score to the total score for all games played.
                        numGames++;                     //start a new game
                        
                        Console.WriteLine("Final score = " + playerScore + " Starting over.....");  //tell the player.
                        
                        //clear all three dice lists and reload the game dice.
                        playerCurrentBrains.Clear();
                        playerCurrentShotguns.Clear();
                        playerCurrentDice.Clear();
                        ReloadGameDice(diceCup);

                        //set player score back to 0.
                        playerScore = 0;
                    }

                }

 
            } while (numGames < 1000);  //change this to 1000 for your final version.
            
            //display the run stats.
            Console.WriteLine("Num Games: " + numGames + " Average score: " + (totalScore / (float)numGames) + " Deaths: " + deaths);
        }

        //method to reload the game dice when a new round starts.
        public static void ReloadGameDice(List<Die> dice)
        {
            //make sure the dice list is empty before we reload it.
            dice.Clear();
            
            //standard Zombie Dice set up.
            dice.AddRange(new List<Die>
            {
                new GreenDie(), new GreenDie(), new GreenDie(), new GreenDie(), new GreenDie(), new GreenDie(), new YellowDie(), new YellowDie(), new YellowDie(), new YellowDie(), new RedDie(), new RedDie(), new RedDie(),
            });

            //Shuffle the dice in the cup.
            Random.Shared.Shuffle(CollectionsMarshal.AsSpan(dice));

        }




        //Object classes to support the game.  You can review these or ignore them as needed.
        //If you make any changes, clear them with me first.

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
                Console.ResetColor();
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
                Console.ResetColor();
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
                Console.ResetColor();
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
