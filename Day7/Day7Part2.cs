
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AOC2023
{
    internal class Day7Part2
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

            Console.WriteLine("Part 2 Result: " + result);
        }


        public enum CardRank
        {
            Jack = 1,
            Two = 2,
            Three = 3,
            Four = 4,
            Five = 5,
            Six = 6,
            Seven = 7,
            Eight = 8,
            Nine = 9,
            Ten = 10,
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
            Pair = 2,
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
                    return HandValue.Pair;
                }
                else
                {
                    return HandValue.HighCard;
                }
            }

            private int Jacks => Cards.Count(x => x.Rank == CardRank.Jack);
            private IEnumerable<IGrouping<CardRank,Card>> NonWildCardsByRank => Cards.Where(c => c.Rank != CardRank.Jack).GroupBy(c => c.Rank);

            private bool IsFiveOfAKind()
            {
                if (Jacks == 5)
                {
                    return true;
                }
                return NonWildCardsByRank.Any(g => g.Count() + Jacks == 5);

            }

            private bool IsFourOfAKind()
            {

                return NonWildCardsByRank.Any(g => g.Count() + Jacks == 4);
                
            }

            private bool IsFullHouse()
            {
                //check without Jacks
                bool hasThreeOfAKind = NonWildCardsByRank.Any(g => g.Count() == 3);
                bool hasPair = NonWildCardsByRank.Any(g => g.Count() == 2);

                //check with Jacks
                hasThreeOfAKind = hasThreeOfAKind || NonWildCardsByRank.Any(g => g.Count() + Jacks == 3);
                hasPair = hasPair || NonWildCardsByRank.Any(g => g.Count() + Jacks == 2);
                var x = NonWildCardsByRank.ToList();

                if (hasThreeOfAKind && hasPair)
                {
                    if (Jacks > 0)
                    {
                        //have to ensure that the jacks aren't reused
                        var threeRank = NonWildCardsByRank.Where(g => g.Count() + Jacks == 3).ToList()[0].Key;

                        var jacksUsed = 3 - Cards.Count(x => x.Rank == threeRank);
                        var jacksRemaining = Jacks - jacksUsed;
                        hasPair = Cards.Where(c => c.Rank != CardRank.Jack && c.Rank != threeRank).GroupBy(c => c.Rank).Any(g => g.Count() + jacksRemaining == 2);
                    }
                }

                return hasThreeOfAKind && hasPair;
            }

            private bool IsThreeOfAKind()
            {

                return NonWildCardsByRank.Any(g => g.Count() + Jacks == 3);

            
            }

            private bool IsTwoPair()
            {
                var ranks = Cards.Select(c => c.Rank).Distinct();
                int pairCount = 0;

                foreach (var rank in ranks)
                {
                    if (Cards.Count(c => c.Rank == rank) >= 2)
                    {
                        pairCount++;
                    }
                }

                return pairCount >= 2;
            }

            private bool IsOnePair()
            {
                return NonWildCardsByRank.Any(g => g.Count() + Jacks == 2);

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
