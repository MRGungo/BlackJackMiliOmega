using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BlackJackC3cVylita
{
    public class Print
    {
        public static void LoginMenu()
        {
            Console.WriteLine();
            Console.WriteLine("R - Register new player");
            Console.WriteLine("L - Login");
            Console.WriteLine("S - Show rules");
        }

        public static void DisplayRules()
        {
            Title();
            Console.WriteLine();
            Console.WriteLine("Rules:");
            string rules = File.ReadAllText("C:\\Users\\janvy\\source\\repos\\BlackJackC3cVylita/rules.txt");
            Console.WriteLine(rules);
            Console.ReadKey();
            Console.Clear();
        }

        public static void ShowInfo(List<Hand> hands, Player player)
        {
            Title();

            if (player != null)
            {
                PlayerInfo(player);
            }
            if (hands.Any())
            {
                AmountOfBet(hands);
                ShowHands(hands);
            }
        }

        public static void MenuForNewGame()
        {
            Console.WriteLine();
            Console.WriteLine("D - Deal a new hand");
            Console.WriteLine("Q - Quit");
        }

        public static void AmountOfBet(List<Hand> hands)
        {
            double totalBet = hands.Sum(hand => hand.Bet);

            totalBet += hands[0].Insurance;

            ConsoleColor previousColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Bet: ${totalBet}");
            Console.ForegroundColor = previousColor;
            Console.WriteLine();
        }

        public static void PlayerInfo(Player player)
        {
            ConsoleColor previousColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"Player: {player.Name}  ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Bankroll: ${player.Bankroll}");
            Console.ForegroundColor = previousColor;
        }

        public static void Title()
        {
            Console.Clear();
            ConsoleColor color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("BlackJack ONLY 18+");
            Console.WriteLine("\"Ministry of Finance warns: Gambling can be addictive!\". Prohibition of participation by persons under 18 years of age");
            Console.WriteLine("------------------");
            Console.ForegroundColor = color;
        }

        public static void MenuForAction(Hand inputHand)
        {
            Console.WriteLine("H - Hit");
            Console.WriteLine("S - Stand");

            if (inputHand.PlayerHand.Count == 2)
            {
                Console.WriteLine("D - Double");
            }
            if (Game.CanSplit(inputHand))
            {
                Console.WriteLine("P - Split");
            }
            if (Game.CanInsure(inputHand))
            {
                Console.WriteLine("I - Insurance");
            }
        }

        public static void ShowHands(List<Hand> hands)
        {
            Console.WriteLine("Dealer has:");

            for (int i = 0; i < hands[0].DealerHand.Count; i++)
            {
                Console.Write($"{hands[0].DealerHand[i].Rank} of {hands[0].DealerHand[i].Suit}");

                if (i < (hands[0].DealerHand.Count - 2))
                {
                    Console.Write(", ");
                }
                else if (i < (hands[0].DealerHand.Count - 1))
                {
                    Console.Write(" and ");
                }
            }

            Console.Write($" ({hands[0].DealerHandValue}");

            if (hands[0].DealerHandSoftValue > hands[0].DealerHandValue && hands[0].DealerHandSoftValue <= 21)
            {
                Console.Write($" or {hands[0].DealerHandSoftValue}");
            }

            Console.WriteLine(")");
            Console.WriteLine();

            var color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("You have:");

            for (int i = 0; i < hands.Count; i++)
            {
                if (hands.Count > 1)
                {
                    Console.Write($"({i + 1}) ");
                }

                for (int j = 0; j < hands[i].PlayerHand.Count; j++)
                {
                    Console.Write($"{hands[i].PlayerHand[j].Rank} of {hands[i].PlayerHand[j].Suit}");

                    if (j < (hands[i].PlayerHand.Count - 2))
                    {
                        Console.Write(", ");
                    }
                    else if (j < (hands[i].PlayerHand.Count - 1))
                    {
                        Console.Write(" and ");
                    }
                }

                Console.Write($" ({hands[i].PlayerHandValue}");

                if (hands[i].PlayerHandSoftValue > hands[i].PlayerHandValue && hands[i].PlayerHandSoftValue <= 21)
                {
                    Console.Write($" or {hands[i].PlayerHandSoftValue}");
                }

                Console.WriteLine(")");
            }
            Console.WriteLine();
            Console.ForegroundColor = color;
        }

        public static void ChoicesForBetting(Player player)
        {
            char option = 'A';

            Console.WriteLine();

            if (player.Bankroll >= 5)
            {
                Console.WriteLine("Please choose a bet amount:");
                
            }
            else
            {
                Console.WriteLine("Sorry, you're out of money.");
                Exit();
            }
        }

        public static void Exit()
        {
            Console.Clear();
            Console.WriteLine("You are leaving so early why is that ?");
            Environment.Exit(1);
        }

        public static void PromptToContinue()
        {
            Console.Write("Press any key to continue");
            Console.ReadKey();
        }
    }
}
