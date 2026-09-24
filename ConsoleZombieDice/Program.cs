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
            List<Die> dice = new List<Die>
            {
                new GreenDie(),
                new GreenDie(),
                new GreenDie(),
                new GreenDie(),
                new GreenDie(),
                new GreenDie(),
                new YellowDie(),
                new YellowDie(),
                new YellowDie(),
                new YellowDie(),
                new RedDie(),
                new RedDie(),
                new RedDie(),
            };

            Random.Shared.Shuffle(CollectionsMarshal.AsSpan(dice));

            // Roll the dice and display the results
            foreach (var die in dice)
            {
                die.Roll();
                die.ShowDieResult();
            }
        }

        



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
