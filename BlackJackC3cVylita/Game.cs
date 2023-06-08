using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJackC3cVylita
{
    public class Game
    {
        static Stack<Card> cardDeck = new Stack<Card>();

        public static void MixDeck()
        {
            Random random = new Random();
            List<Card> cards = new List<Card>();
            Rank cardRank = Rank.Ace;
            Suit cardSuit = Suit.Clubs;

            for (int i = 0; i < 4; i++)
            {
                switch (i)
                {
                    case 0: cardSuit = Suit.Clubs; break;
                    case 1: cardSuit = Suit.Diamonds; break;
                    case 2: cardSuit = Suit.Hearts; break;
                    case 3: cardSuit = Suit.Spades; break;
                }

                for (int j = 0; j < 13; j++)
                {
                    switch (j)
                    {
                        case 0: cardRank = Rank.Ace; break;
                        case 1: cardRank = Rank.Two; break;
                        case 2: cardRank = Rank.Three; break;
                        case 3: cardRank = Rank.Four; break;
                        case 4: cardRank = Rank.Five; break;
                        case 5: cardRank = Rank.Six; break;
                        case 6: cardRank = Rank.Seven; break;
                        case 7: cardRank = Rank.Eight; break;
                        case 8: cardRank = Rank.Nine; break;
                        case 9: cardRank = Rank.Ten; break;
                        case 10: cardRank = Rank.Jack; break;
                        case 11: cardRank = Rank.Queen; break;
                        case 12: cardRank = Rank.King; break;
                    }
                    cards.Add(new Card(cardRank, cardSuit));
                }
            }

            do
            {
                Card card = cards[random.Next(0, cards.Count)];
                cards.Remove(card);
                cardDeck.Push(card);
            } while (cards.Count > 0);
        }

        public static Hand ProvideStartingHand(Hand hand)
        {
            MixDeck();
            GiveCard(hand.DealerHand, 1);
            GiveCard(hand.PlayerHand, 2);
            return hand;
        }

        public static List<Card> GiveCard(List<Card> recipientHand, int cardCount)
        {
            for (int i = 0; i < cardCount; i++)
            {
                recipientHand.Add(cardDeck.Pop());
            }

            return recipientHand;
        }

        public static List<Hand> CheckHands(List<Hand> hands)
        {
            for (int i = 0; i < hands.Count; i++)
            {
                hands[i].PlayerHandValue = HandValue(hands[i].PlayerHand);
                hands[i].DealerHandValue = HandValue(hands[i].DealerHand);
                hands[i].PlayerHandSoftValue = hands[i].PlayerHandValue + AcesCount(hands[i].PlayerHand);
                hands[i].DealerHandSoftValue = hands[i].DealerHandValue + AcesCount(hands[i].DealerHand);
            }

            return hands;
        }


        public static int HandValue(List<Card> recipientHand)
        {
            int total = 0;

            foreach (var card in recipientHand)
            {
                total += card.Value;
            }

            return total;
        }

        public static int AcesCount(List<Card> recipientHand)
        {
            int total = 0;

            foreach (var card in recipientHand)
            {
                if (card.Rank == Rank.Ace)
                {
                    total = 10;
                }
            }

            return total;
        }



        public static Hand ApplyInsurance(Hand inputHand)
        {
            if (CanInsure(inputHand))
            {
                inputHand.Insurance = (0.5 * inputHand.Bet);
            }

            return inputHand;
        }


        public static List<Hand> ApplySplit(List<Hand> hands, Player player)
        {
            if (CanSplit(hands[0]))
            {
                Hand hand = new Hand();
                hand.PlayerHand.Add(hands[0].PlayerHand[1]);
                hands[0].PlayerHand.Remove(hands[0].PlayerHand[1]);
                hand.Bet = hands[0].Bet;

                hands.Add(hand);

                hands[0].Split = true;
                hands[1].Split = true;

                hands[0].PlayerHand = GiveCard(hands[0].PlayerHand, 1);
                hands[1].PlayerHand = GiveCard(hands[1].PlayerHand, 1);

                for (int i = 0; i < hands.Count; i++)
                {
                    if (hands[i].PlayerHand[0].Rank == Rank.Ace)
                    {
                        hands[i].Stand = true;
                    }
                }
            }

            return hands;
        }

        public static Hand DoubleBet(Hand inputHand)
        {
            if (inputHand.PlayerHand.Count == 2)
            {
                inputHand.Bet += inputHand.Bet;
                inputHand.PlayerHand = GiveCard(inputHand.PlayerHand, 1);
                inputHand.Stand = true;
            }
            return inputHand;
        }


        public static bool LosesGame(List<Hand> hands)
        {
            int lost = 0;

            for (int i = 0; i < hands.Count; i++)
            {
                lost = hands[i].PlayerHandValue > 21 ? lost + 1 : lost;
            }

            return lost == hands.Count;
        }

        public static bool StandsGame(List<Hand> hands)
        {
            int stand = 0;

            for (int i = 0; i < hands.Count; i++)
            {
                stand = hands[i].Stand == true ? stand + 1 : stand;
            }

            return stand == hands.Count;
        }

        public static List<Hand> PlayDealerRound(List<Hand> hands, Player player)
        {
            do
            {
                var newHand = GiveCard(hands[0].DealerHand, 1);

                for (int i = 0; i < hands.Count; i++)
                {
                    hands[i].DealerHand = newHand;
                }

                hands = CheckHands(hands);
                Print.ShowInfo(hands, player);
                Print.PromptToContinue();

            } while (NeedsAnotherCard(hands));

            return hands;
        }


        public static bool CanInsure(Hand inputHand)
        {
            return inputHand.PlayerHand.Count == 2 && inputHand.DealerHand.Count == 1 && inputHand.DealerHand[0].Rank == Rank.Ace && inputHand.Insurance == 0 && inputHand.Split == false;
        }

        public static bool CanSplit(Hand inputHand)
        {
            return !inputHand.Split && inputHand.PlayerHand.Count == 2 && inputHand.PlayerHand[0].Value == inputHand.PlayerHand[1].Value;
        }

        public static bool NeedsAnotherCard(List<Hand> hands)
        {
            return (hands[0].DealerHandSoftValue > hands[0].DealerHandValue && hands[0].DealerHandSoftValue <= 17) ||
                   (hands[0].DealerHandSoftValue > 21 && hands[0].DealerHandValue < 17) ||
                   (hands[0].DealerHandSoftValue == hands[0].DealerHandValue && hands[0].DealerHandValue < 17);
        }

        public static double CompareCards(Hand inputHand)
        {
            double output = 0;

            int playerHand = inputHand.PlayerHandSoftValue <= 21 ? inputHand.PlayerHandSoftValue : inputHand.PlayerHandValue;
            int dealerHand = inputHand.DealerHandSoftValue <= 21 ? inputHand.DealerHandSoftValue : inputHand.DealerHandValue;

            bool playerBlackjack = inputHand.PlayerHand.Count == 2 && playerHand == 21 && !inputHand.Split;
            bool dealerBlackjack = inputHand.DealerHand.Count == 2 && dealerHand == 21;

            if (playerHand > 21)
            {
                output = 0;
            }
            else if (dealerHand > 21)
            {
                output = 2;
            }
            else if (dealerBlackjack && !playerBlackjack && inputHand.Insurance == 0)
            {
                output = 0;
            }
            else if (dealerBlackjack && !playerBlackjack && inputHand.Insurance > 0)
            {
                output = 1;
            }
            else if (playerBlackjack && !dealerBlackjack && inputHand.Insurance == 0)
            {
                output = 2.5;
            }
            else if (playerBlackjack && inputHand.Insurance > 0)
            {
                output = 1;
            }
            else if (playerHand < dealerHand)
            {
                output = 0;
            }
            else if (playerHand > dealerHand)
            {
                output = 2;
            }
            else if (playerHand == dealerHand)
            {
                output = 1;
            }

            return output;
        }
    }
}
