
using System;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AOC2023
{
    internal class Day7Part1
    {

        public static void Solve()
        {

            var hands = new List<PokerHand>();
            var lines = File.ReadAllLines("day7Input.txt");
            foreach (var line in lines)
            {

                var sa = line.Split(" ");
                var bid = int.Parse(sa[1]);

                var cards = new List<Card>();

                foreach (var c in sa[0])
                {
                    CardRank cardRank;
                    switch (c)
                    {
                        case '2':
                            cardRank = CardRank.Two;
                            break;
                        case '3':
                            cardRank = CardRank.Three;
                            break;
                        case '4':
                            cardRank = CardRank.Four;
                            break;
                        case '5':
                            cardRank = CardRank.Five;
                            break;
                        case '6':
                            cardRank = CardRank.Six;
                            break;
                        case '7':
                            cardRank = CardRank.Seven;
                            break;
                        case '8':
                            cardRank = CardRank.Eight;
                            break;
                        case '9':
                            cardRank = CardRank.Nine;
                            break;
                        case 'T':
                            cardRank = CardRank.Ten;
                            break;
                        case 'J':
                            cardRank = CardRank.Jack;
                            break;
                        case 'K':
                            cardRank = CardRank.King;
                            break;
                        case 'Q':
                            cardRank = CardRank.Queen;
                            break;
                        case 'A':
                            cardRank = CardRank.Ace;
                            break;
                        default:
                            throw new Exception($"Invalid Card Rank: {c}");
                    }
                    cards.Add(new Card(cardRank, c.ToString()));
                }

                hands.Add(new PokerHand(cards, bid));
            }

            var result = 0;

            var rankIndex = 1;
            var sortedHands = hands.OrderBy(x => x).ToList();
            foreach (var hand in sortedHands)
            {
                result += (hand.Bid * rankIndex);
                rankIndex++;
            }

            Console.WriteLine("Part 1 Result: " + result);
        }


        public enum CardRank
        {
            Two = 2,
            Three = 3,
            Four = 4,
            Five = 5,
            Six = 6,
            Seven = 7,
            Eight = 8,
            Nine = 9,
            Ten = 10,
            Jack = 11,
            Queen = 12,
            King = 13,
            Ace = 14
        }

        public class Card : IComparable<Card>
        {
            public string ID { get; set; }

            public CardRank Rank { get; set; }

            public int FaceValue => (int)Rank;
            public Card(CardRank rank, string id)
            {
                Rank = rank;
                ID = id;
            }

            public override string ToString()
            {
                return $"{ID}";
            }

            public int CompareTo(Card other)
            {
                return FaceValue.CompareTo(other.FaceValue);
            }
        }

        public enum HandValue
        {
            HighCard = 1,
            OnePair = 2,
            TwoPair = 3,
            ThreeOfAKind = 4,
            FullHouse = 5,
            FourOfAKind = 6,
            FiveOfAKind = 7
        }

        public class PokerHand : IComparable<PokerHand>
        {
            public int Bid { get; set; }

            public List<Card> Cards { get; set; }

            public PokerHand(List<Card> cards, int bid)
            {
                if (cards.Count != 5)
                {
                    throw new ArgumentException("A poker hand must contain exactly 5 cards.");
                }

                Cards = cards;
                Bid = bid;
            }

            public HandValue GetHandValue()
            {
                if (IsFiveOfAKind())
                {
                    return HandValue.FiveOfAKind;
                }
                else if (IsFourOfAKind())
                {
                    return HandValue.FourOfAKind;
                }
                else if (IsFullHouse())
                {
                    return HandValue.FullHouse;
                }
                else if (IsThreeOfAKind())
                {
                    return HandValue.ThreeOfAKind;
                }
                else if (IsTwoPair())
                {
                    return HandValue.TwoPair;
                }
                else if (IsOnePair())
                {
                    return HandValue.OnePair;
                }
                else
                {
                    return HandValue.HighCard;
                }
            }

            private bool IsFiveOfAKind()
            {
                return Cards.GroupBy(c => c.Rank).Any(g => g.Count() == 5);
            }

            private bool IsFourOfAKind()
            {
                return Cards.GroupBy(c => c.Rank).Any(g => g.Count() == 4);
            }

            private bool IsFullHouse()
            {
                return Cards.GroupBy(c => c.Rank).Any(g => g.Count() == 3) &&
                       Cards.GroupBy(c => c.Rank).Any(g => g.Count() == 2);
            }

            private bool IsThreeOfAKind()
            {
                return Cards.GroupBy(c => c.Rank).Any(g => g.Count() == 3);
            }

            private bool IsTwoPair()
            {
                return Cards.GroupBy(c => c.Rank).Count(g => g.Count() == 2) == 2;
            }

            private bool IsOnePair()
            {
                return Cards.GroupBy(c => c.Rank).Any(g => g.Count() == 2);
            }

            public int CompareTo(PokerHand? other)
            {
                if (GetHandValue().CompareTo(other.GetHandValue()) != 0)
                {
                    return GetHandValue().CompareTo(other.GetHandValue());
                }
                else
                {
                    for (int i = 0; i < Cards.Count; i++)
                    {
                        if (Cards[i].CompareTo(other.Cards[i]) != 0)
                        {
                            return Cards[i].CompareTo(other.Cards[i]);
                        }
                    }

                    return 0;
                }
            }

            public override string ToString()
            {
                return $"{string.Join(' ', Cards.Select(x => x.ID)).Replace(" ", "")} {GetHandValue()}";
            }



        }

    }
}