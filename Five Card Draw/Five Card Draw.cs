﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.IO;


class Program
{
    static Random r = new Random();

    static uint[] countWinnigs = new uint[9];

    static void Main()
    {
        string[,] deck =
            {
                {"2\u2660","3\u2660","4\u2660","5\u2660","6\u2660","7\u2660","8\u2660","9\u2660","T\u2660","J\u2660","Q\u2660","K\u2660","A\u2660"},
                {"2\u2663","3\u2663","4\u2663","5\u2663","6\u2663","7\u2663","8\u2663","9\u2663","T\u2663","J\u2663","Q\u2663","K\u2663","A\u2663"},
                {"2\u2665","3\u2665","4\u2665","5\u2665","6\u2665","7\u2665","8\u2665","9\u2665","T\u2665","J\u2665","Q\u2665","K\u2665","A\u2665"},
                {"2\u2666","3\u2666","4\u2666","5\u2666","6\u2666","7\u2666","8\u2666","9\u2666","T\u2666","J\u2666","Q\u2666","K\u2666","A\u2666"}
            };

        Dictionary<string, uint> points = new Dictionary<string, uint>()
        {
            {"ROYAL FLUSH", 500},
            {"STRAIGHT FLUSH", 100},
            {"4 OF A KIND", 40},
            {"FULL HOUSE", 12},
            {"FLUSH", 7},
            {"STRAIGHT", 5},
            {"3 OF A KIND", 3},
            {"TWO PAIR", 2},
            {"HIGH PAIR", 1}
        };



        string title = "ARCH DEVIL's POKER";
        int heigth = 23;
        int width = 60;
        int cardWidth = 8;
        int cardHeight = 7;
        uint winnings = 0;
        uint bet = 1;
        uint maxBet = 10;
        uint coins = 100;

        Console.CursorVisible = false;
        Console.Title = title;
        Console.WindowHeight = heigth;
        Console.WindowWidth = width;
        Console.BufferHeight = heigth;
        Console.BufferWidth = width;

        //welcome screen - OK
        bool isEscaped = false;
        isEscaped = WelcomeScreen(title, heigth, width);

        // pressed escape for exit
        if (isEscaped)
        {
            return;
        }

        //MainScreen
        while (coins > 0)
        {
            bool[] holdCards = new bool[5];
            bool[] winDisplay = new bool[points.Count];
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine();


            for (int i = 1; i <= points.Count; i++)
            {
                PrintWinningBlock(points, i);
            }

            PrintOnPosition(width - 10, 2, coins.ToString().PadLeft(10, ' '));
            PrintOnPosition(width - 5, 1, "Coins");

            PrintOnPosition(width - 3, 5, "BET");
            PrintOnPosition(width - 3, 6, bet.ToString().PadLeft(3, ' '));

            PrintOnPosition(0, 10, new string('_', width));
            PrintOnPosition(0, 12, new string('_', width));

            for (int i = 0, j = 0; j < 5; i += 11, j++)
            {
                CardBack(cardHeight, cardWidth, i + 4, 14);
                Thread.Sleep(130);
            }

            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.ForegroundColor = ConsoleColor.Yellow;

            // Bet
            bet = Bet(bet, width, heigth, maxBet);

            checked
            {
                try
                {
                    coins -= bet;
                    PrintOnPosition(width - 10, 2, coins.ToString().PadLeft(10, ' '));
                }
                catch (Exception)
                {
                    bet = coins;
                    coins = 0;
                    PrintOnPosition(width - 10, 2, "ALL IN".PadLeft(10, ' '));
                    PrintOnPosition(width - 2, 6, bet.ToString().PadLeft(2, ' '));
                    PrintOnPosition(20, 11, "YOUR BET: " + bet);
                }
            }

            //Draw - 5 cards - OK
            var drawedCards = new List<string>();
            var playCards = new string[5];
            DrawCards(deck, r, drawedCards, holdCards, playCards);

            //print card faces
            PutFaceCard(cardWidth, cardHeight, holdCards, playCards);

            //Hold
            holdCards = HoldCard(holdCards);

            for (int i = 0, j = 0; j < 5; i += 11, j++)
            {
                if (holdCards[j] == true) continue;
                CardBack(cardHeight, cardWidth, i + 4, 14);
            }
            Thread.Sleep(500);
            //redraw
            DrawCards(deck, r, drawedCards, holdCards, playCards);

            //print redrawn card faces
            PutFaceCard(cardWidth, cardHeight, holdCards, playCards);

            //Check for Winnings

            uint winningCoins = CheckForWinnings(playCards, winDisplay, points);
            coins += winningCoins * bet;

            if (winnings != 0) winnings++;
           
            for (int i = 1; i <= points.Count; i++)
            {
                if (!winDisplay[i - 1]) continue;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.BackgroundColor = ConsoleColor.Cyan;
                PrintWinningBlock(points, i);
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.BackgroundColor = ConsoleColor.DarkGreen;

            PrintOnPosition(40, 9, "WINNINGS: " + winningCoins * bet);

            //Game Over or Next Deal

            WriteInFile(countWinnigs, winnings);

            ConsoleKeyInfo button;

            button = Console.ReadKey(true);
            if (button.Key == ConsoleKey.Escape)
            {
                break;
            }

        }

        Console.Clear();

        PrintOnPosition((width - 28) / 2, heigth / 2 - 3, "GAME OVER");
        using (StreamReader reader = new StreamReader("winnings.txt"))
        {
            string line = reader.ReadLine();
            PrintOnPosition((width - 28) / 2, heigth / 2 - 1, line);

            int couterLine = 1;
            foreach (var key in points.Keys)
            {
                line = reader.ReadLine();
                PrintOnPosition((width - 28) / 2, heigth / 2 + couterLine, string.Concat(key, ": ", line));
                couterLine++;
            }
        }


    }

    private static void PrintWinningBlock(Dictionary<string, uint> points, int i)
    {
        var item = points.ElementAt(i - 1);
        var itemKey = item.Key;
        var itemValue = item.Value;
        PrintOnPosition(1, i, itemKey);
        PrintOnPosition(30, i, string.Concat("x", itemValue));
    }

    private static void WriteInFile(uint[] countWinnins, uint winnings)
    {
        using (StreamWriter win = new StreamWriter("winnings.txt"))
        {
            win.WriteLine("Winnings: {0}", winnings);
            for (int i = 0; i < 9; i++)
            {
                win.WriteLine(countWinnigs[i]);
            }
        }
    }

    private static uint CheckForWinnings(string[] playCards, bool[] winDisplay, Dictionary<string, uint> points)
    {
        var cardNumber = new int[playCards.Length];
        
        if (playCards[0][1] == playCards[1][1] && playCards[0][1] == playCards[2][1] && playCards[0][1] == playCards[3][1] && playCards[0][1] == playCards[4][1])
        {
            ReshapeCards(playCards, cardNumber);

            if (cardNumber[0] == cardNumber[1] - 1 && cardNumber[0] == cardNumber[2] - 2 && cardNumber[0] == cardNumber[3] - 3 && cardNumber[0] == cardNumber[4] - 4)
            {
                if (cardNumber[0] == 10)
                {
                    //Royal flush
                    winDisplay[0] = true;
                    return points["ROYAL FLUSH"];
                }
                else
                {
                    //straight flush
                    winDisplay[1] = true;
                    return points["STRAIGHT FLUSH"];
                }
            }
            if (cardNumber[0] == 14 && cardNumber[1] == 2 && cardNumber[2] == 3 && cardNumber[3] == 4 && cardNumber[4] == 5)
            {
                //straight flush
                winDisplay[1] = true;
                return points["STRAIGHT FLUSH"];
            }
            else
            {
                //flush
                winDisplay[4] = true;
                return points["FLUSH"];
            }
        }

        ReshapeCards(playCards, cardNumber);

        if ((cardNumber[0] == cardNumber[1] && cardNumber[0] == cardNumber[2] && cardNumber[0] == cardNumber[3]) || (cardNumber[1] == cardNumber[2] && cardNumber[1] == cardNumber[3] && cardNumber[1] == cardNumber[4]))
        {
            //4 of a Kind
            winDisplay[2] = true;
            return points["4 OF A KIND"];
        }
        if ((cardNumber[0] == cardNumber[1] && cardNumber[0] == cardNumber[2] && cardNumber[3] == cardNumber[4]) || (cardNumber[0] == cardNumber[1] && cardNumber[2] == cardNumber[3] && cardNumber[2] == cardNumber[4]))
        {
            //Full House
            winDisplay[3] = true;
            return points["FULL HOUSE"];
        }
        if ((cardNumber[0] == cardNumber[1] - 1 && cardNumber[0] == cardNumber[2] - 2 && cardNumber[0] == cardNumber[3] - 3 && cardNumber[0] == cardNumber[4] - 4) || (cardNumber[0] == 14 && cardNumber[1] == 2 && cardNumber[2] == 3 && cardNumber[3] == 4 && cardNumber[4] == 5))
        {
            // straight
            winDisplay[5] = true;
            return points["STRAIGHT"];
        }

        if ((cardNumber[0] == cardNumber[1] && cardNumber[0] == cardNumber[2]) || (cardNumber[1] == cardNumber[2] && cardNumber[1] == cardNumber[3]) || (cardNumber[2] == cardNumber[3] && cardNumber[2] == cardNumber[4]))
        {
            // 3 of a kind
            winDisplay[6] = true;
            return points["3 OF A KIND"];
        }
        if ((cardNumber[0] == cardNumber[1] && (cardNumber[2] == cardNumber[3] || cardNumber[3] == cardNumber[4])) || (cardNumber[1] == cardNumber[2] && cardNumber[3] == cardNumber[4]))
        {
            // two pair
            winDisplay[7] = true;
            return points["TWO PAIR"];
        }
        if ((cardNumber[0] == cardNumber[1] && cardNumber[0] > 10) || (cardNumber[1] == cardNumber[2] && cardNumber[1] > 10) || (cardNumber[2] == cardNumber[3] && cardNumber[2] > 10) || (cardNumber[3] == cardNumber[4] && cardNumber[3] > 10))
        {
            // high pair
            winDisplay[8] = true;
            return points["HIGH PAIR"];
        }
        return 0;
    }
    private static void ReshapeCards(string[] playCards, int[] cardNumber)
    {
        for (int i = 0; i < playCards.Length; i++)
        {
            if (playCards[i][0] == 'T') cardNumber[i] = 10;
            else if (playCards[i][0] == 'J') cardNumber[i] = 11;
            else if (playCards[i][0] == 'Q') cardNumber[i] = 12;
            else if (playCards[i][0] == 'K') cardNumber[i] = 13;
            else if (playCards[i][0] == 'A') cardNumber[i] = 14;
            else cardNumber[i] = int.Parse(playCards[i].Remove(playCards[i].Length - 1));
        }
        Array.Sort(cardNumber);
    }

    // this is for the console to print in black-yellow-black-yellow what you win.
    private static void PrintMatch(int p1, int p2, string win)
    {
        Console.ForegroundColor = ConsoleColor.Black;
        PrintOnPosition(p1, p2, win);
        Thread.Sleep(300);
        Console.ForegroundColor = ConsoleColor.Yellow;
        PrintOnPosition(p1, p2, win);
        Thread.Sleep(300);
        Console.ForegroundColor = ConsoleColor.Black;
        PrintOnPosition(p1, p2, win);
        Thread.Sleep(300);
        Console.ForegroundColor = ConsoleColor.Yellow;
        PrintOnPosition(p1, p2, win);
        Thread.Sleep(300);
        Console.ForegroundColor = ConsoleColor.Black;
        PrintOnPosition(p1, p2, win);
        Thread.Sleep(300);
        Console.ForegroundColor = ConsoleColor.Yellow;
        PrintOnPosition(p1, p2, win);
    }

    private static void PutFaceCard(int cardWidth, int cardHeight, bool[] holdCards, string[] playCards)
    {
        for (int i = 0, j = 0; j < 5; i += 11, j++)
        {
            if (holdCards[j] == true) continue;

            if (playCards[j].Contains("\u2660") || playCards[j].Contains("\u2663")) Console.ForegroundColor = ConsoleColor.Black;
            else Console.ForegroundColor = ConsoleColor.Red;

            CardFace(cardHeight, cardWidth, i + 4, 14, playCards[j].ToString());
            Thread.Sleep(200);
        }
        Console.BackgroundColor = ConsoleColor.DarkGreen;
        Console.ForegroundColor = ConsoleColor.Yellow;
    }

    static uint Bet(uint bet, int width, int heigth, uint maxBet)
    {
        ConsoleKeyInfo keyPressed;

        do
        {
            keyPressed = Console.ReadKey(true);

            if (keyPressed.Key == ConsoleKey.UpArrow && bet < maxBet)
            {
                // sound for betting up
                Console.Beep(364, 100);
                Console.Beep(424, 100);

                bet++;
            }
            if (keyPressed.Key == ConsoleKey.DownArrow && bet > 1)
            {
                // sound for betting down
                Console.Beep(424, 100);
                Console.Beep(364, 100);

                bet--;
            }

            PrintOnPosition(width - 2, 6, bet.ToString().PadLeft(2, ' '));

        } while (keyPressed.Key != ConsoleKey.Spacebar);

        // sound for you bet
        Console.Beep(700, 200);
        Console.Beep(700, 200);

        PrintOnPosition(1, 11, "YOU BET: " + bet);

        return bet;
    }

    static bool[] HoldCard(bool[] cards)
    {

        ConsoleKeyInfo keyPressed;

        do
        {
            keyPressed = Console.ReadKey(true);

            if (keyPressed.Key == ConsoleKey.NumPad1 || keyPressed.Key == ConsoleKey.D1)
            {
                if (cards[0] == false)
                {
                    Console.Beep(600, 150);

                    cards[0] = true;
                    PrintOnPosition(6, 21, "Hold");
                }
                else
                {
                    Console.Beep(260, 170);

                    cards[0] = false;
                    PrintOnPosition(6, 21, "    ");
                }
            }
            if (keyPressed.Key == ConsoleKey.NumPad2 || keyPressed.Key == ConsoleKey.D2)
            {

                if (cards[1] == false)
                {
                    Console.Beep(600, 150);

                    cards[1] = true;
                    PrintOnPosition(17, 21, "Hold");
                }
                else
                {
                    Console.Beep(260, 170);

                    cards[1] = false;
                    PrintOnPosition(17, 21, "    ");
                }
            }
            if (keyPressed.Key == ConsoleKey.NumPad3 || keyPressed.Key == ConsoleKey.D3)
            {

                if (cards[2] == false)
                {
                    Console.Beep(600, 150);

                    cards[2] = true;
                    PrintOnPosition(28, 21, "Hold");
                }
                else
                {
                    Console.Beep(260, 170);

                    cards[2] = false;
                    PrintOnPosition(28, 21, "    ");
                }
            }
            if (keyPressed.Key == ConsoleKey.NumPad4 || keyPressed.Key == ConsoleKey.D4)
            {

                if (cards[3] == false)
                {
                    Console.Beep(600, 150);

                    cards[3] = true;
                    PrintOnPosition(39, 21, "Hold");
                }
                else
                {
                    Console.Beep(260, 170);

                    cards[3] = false;
                    PrintOnPosition(39, 21, "    ");
                }
            }
            if (keyPressed.Key == ConsoleKey.NumPad5 || keyPressed.Key == ConsoleKey.D5)
            {
                if (cards[4] == false)
                {
                    Console.Beep(600, 150);

                    cards[4] = true;
                    PrintOnPosition(50, 21, "Hold");
                }
                else
                {
                    Console.Beep(260, 170);

                    cards[4] = false;
                    PrintOnPosition(50, 21, "    ");
                }
            }
        } while (keyPressed.Key != ConsoleKey.Spacebar);

        Console.Beep(700, 200);
        Console.Beep(700, 200);

        return cards;
    }

    private static void CardFace(int cardHeight, int cardWidth, int x, int y, string card)
    {
        string cardOk = card;
        if (cardOk[0] == 'T') cardOk = "10" + card[1];
        Console.BackgroundColor = ConsoleColor.White;
        for (int i = 0; i < cardHeight; i++)
        {
            for (int j = 0; j < cardWidth; j++)
            {
                PrintOnPosition(x + j, y + i, " ");
                PrintOnPosition(x, y, cardOk);
                PrintOnPosition(x + 3, y + 3, cardOk);
                PrintOnPosition(x + 5, y + 6, cardOk.PadLeft(3, ' '));
            }
        }
    }

    private static bool WelcomeScreen(string title, int heigth, int width)
    {
        Console.ForegroundColor = ConsoleColor.Magenta;
        PrintOnPosition((width - 28) / 2, heigth / 2 - 4, title);
        PrintOnPosition((width - 28) / 2, heigth / 2 - 1, "Play - press ENTER");
        PrintOnPosition((width - 28) / 2, heigth / 2 + 1, "How to play - press H");
        PrintOnPosition((width - 28) / 2, heigth / 2 + 3, "Exit - press Esc (or Ctrl + C)");


        ConsoleKeyInfo startKey;
        while (true)
        {
            startKey = Console.ReadKey(true);

            if (startKey.Key == ConsoleKey.H)
            {
                return HelpMenu();
            }
            if (startKey.Key == ConsoleKey.Enter)
            {
                break;
            }
            if (startKey.Key == ConsoleKey.Escape)
            {
                return true;
            }
        }
        return false;

    }

    static void CardBack(int height, int width, int x, int y)
    {
        Console.BackgroundColor = ConsoleColor.White;
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                PrintOnPosition(x + i, y + j, "*");
            }
        }
    }

    static bool HelpMenu()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Yellow;
        //Console.BackgroundColor = ConsoleColor.Yellow;
        PrintOnPosition(20, 6, "Keys 1 to 5: Hold cards");
        PrintOnPosition(20, 8, "Spacebar: Draw cards");
        PrintOnPosition(20, 9, "(You can change any card)");
        PrintOnPosition(20, 12, "Press ENTER to play");
        PrintOnPosition(20, 13, "Press ESC to exit");


        while (true)
        {
            ConsoleKeyInfo key;
            key = Console.ReadKey(true);
            if (key.Key == ConsoleKey.Enter)
            {
                return false;
            }
            if (key.Key == ConsoleKey.Escape)
            {
                return true;
            }
        }
    }

    static void PrintOnPosition(int x, int y, string c)
    {
        Console.SetCursorPosition(x, y);
        Console.WriteLine(c);
    }

    private static void DrawCards(string[,] deck, Random r, List<string> drawedCards, bool[] holdCards, string[] playCards)
    {
        for (int i = 0; i < 5; i++)
        {
            if (holdCards[i] == true) continue;
            playCards[i] = deck[r.Next(0, deck.GetLength(0)), r.Next(0, deck.GetLength(1))];
            if (drawedCards.Contains(playCards[i]))
            {
                i--;
                continue;
            }
            else drawedCards.Add(playCards[i]);
        }
    }
}
