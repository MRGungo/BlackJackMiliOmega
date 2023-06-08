using System;
using System.Collections.Generic;
using System.Numerics;

namespace BlackJackC3cVylita
{
    public class Program
    {
        private static List<Player> playerList = new List<Player>();
        private static List<Hand> hands = new List<Hand>();
        private static bool active = false;

    

 static void Main(string[] args)
        {
            GetUserLoginOption();

            do
            {
                PromptNewRound();
                PromptBet();
                hands[0] = Game.ProvideStartingHand(hands[0]);
                PromptAction();
            } while (playerList[0].Bankroll >= 5);

            Print.Exit();
        }

        private static void SignUpNewPlayer()
        {
            Console.WriteLine();
            Console.Write("Please write your name: ");
            string inputName = CollectInput();

            while (Player.PlayerNameExists(inputName))
            {
                Console.WriteLine();
                Console.Write("Sorry, that name is taken. Try again: ");
                inputName = CollectInput();
            }

            playerList.Add(new Player(Player.GeneratePlayerId(), inputName));
            Player.SavePlayer(playerList[0]);
        }

        private static void LogIn()
        {
            Console.WriteLine();
            Console.Write("Please write your name: ");
            string inputName = CollectInput();

            while (!Player.PlayerNameExists(inputName))
            {
                Console.WriteLine();
                Console.Write("Sorry, no player by that name was found. Try again: ");
                inputName = CollectInput();
            }
            Player player = Player.GetPlayerByName(inputName);
            playerList.Add(player);
        }


        private static void TerminateGame()
        {
            for (int i = 0; i < hands.Count; i++)
            {
                double result = Game.CompareCards(hands[i]);

                hands[i].TransactionAmount = result == 1 ? hands[i].Bet + hands[i].Insurance : result * hands[i].Bet;
                playerList[0].Bankroll += hands[i].TransactionAmount;

                playerList[0].Hands.Add(hands[i]);
                Player.SaveData(playerList[0]);

                Print.ShowInfo(hands, playerList[0]);
                var color = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;

                if (hands.Count > 1)
                {
                    Console.Write($"Hand {i + 1}: ");
                }

                switch (result)
                {
                    case 0: Console.WriteLine("You lose."); break;
                    case 2: Console.WriteLine("You win!"); break;
                    case 2.5: Console.WriteLine("Blackjack!"); break;
                    case 1:
                        {
                            if (hands[i].Insurance > 0 && hands[0].DealerHand.Count == 2 && hands[0].DealerHandSoftValue == 21)
                            {
                                Console.WriteLine("Insurance pays out.");
                            }
                            else if (hands[i].Insurance > 0 && hands[i].PlayerHand.Count == 2 && hands[i].PlayerHandSoftValue == 21)
                            {
                                Console.WriteLine("Insurance pays out.");
                            }
                            else
                            {
                                Console.WriteLine("It's a draw!");
                            };
                            break;
                        }
                }
                Console.ForegroundColor = color;

                if (hands.Count > 1)
                {
                    Console.ReadKey();
                }
            }

            active = false;
        }

        private static void PromptNewRound()
        {
            Print.MenuForNewGame();

            switch (CollectInput().ToLower())
            {
                case "q": Print.Exit(); break;
                default: active = true; break;
            }
        }

        private static void PromptBet()
        {
            int bet = 0;
            hands.Clear();
            Hand hand = new Hand();

            Print.ShowInfo(hands, playerList[0]);
            Print.ChoicesForBetting(playerList[0]);

            switch (CollectInput().ToLower())
            {
                case "a": bet = 5; break;
                case "b": bet = 10; break;
                case "c": bet = 20; break;
                case "d": bet = 40; break;
                case "e": bet = 80; break;
                default: bet = 5; break;
            }

            hand.Bet = bet;
            playerList[0].Bankroll -= bet;
            hands.Add(hand);
            Player.SaveData(playerList[0]);
        }

        private static void PromptAction()
        {
            hands = Game.CheckHands(hands);

            do
            {
                int count = hands.Count;

                for (int i = 0; i < count; i++)
                {
                    do
                    {
                        Print.ShowInfo(hands, playerList[0]);

                        if (hands.Count > 1)
                        {
                            Console.WriteLine($"Hand {i + 1}");
                        }

                        Print.MenuForAction(hands[i]);

                        if (hands[i].PlayerHandValue >= 21 || hands[i].PlayerHandSoftValue == 21)
                        {
                            hands[i].Stand = true;
                        }
                        else
                        {
                            switch (CollectInput().ToLower())
                            {
                                case "h": hands[i].PlayerHand = Game.GiveCard(hands[i].PlayerHand, 1); break;
                                case "s": hands[i].Stand = true; break;
                                case "d": hands[i] = Game.DoubleBet(hands[i]); playerList[0].Bankroll -= hands[i].Bet; break;
                                case "p": Game.ApplySplit(hands, playerList[0]); playerList[0].Bankroll -= hands[1].Bet; break;
                                case "i": hands[i] = Game.ApplyInsurance(hands[i]); playerList[0].Bankroll -= hands[i].Insurance; break;
                                default: hands[i].Stand = true; break;
                            }
                        }

                        hands = Game.CheckHands(hands);

                    } while (!hands[i].Stand && hands.Count == count);
                }

                if (Game.LosesGame(hands))
                {
                    TerminateGame();
                }
                if (active && Game.StandsGame(hands))
                {
                    hands = Game.PlayDealerRound(hands, playerList[0]);
                    TerminateGame();
                }

            } while (active);
        }

        private static void GetUserLoginOption()
        {
            do
            {
                Print.Title();
                Print.LoginMenu();

                string input = CollectInput().ToLower();

                switch (input)
                {
                    case "r": SignUpNewPlayer(); break;
                    case "l": LogIn(); break;
                    case "s": Print.DisplayRules(); break;
                }
            } while (playerList.Count < 1);
        }

        private static string CollectInput()
        {
            string input = string.Empty;
            bool success = false;

            do
            {
                try
                {
                    input = Console.ReadLine();
                    success = true;
                }
                catch (Exception)
                {
                    Console.WriteLine($"Please try again.");
                }
            } while (!success);

            return input;
        }

    }
}