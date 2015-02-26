using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;


class Program
{
    static Random r = new Random();

    const int royalFlushPoints = 500;
    const int straightFlushPoints = 100;
    const int fourOfAKindPoints = 40;
    const int fullHousePoints = 12;
    const int flushPoint = 7;
    const int straightPoint = 5;
    const int threeOfAKindPoint = 3;
    const int twoPairPoint = 2;
    const int highCardPoint = 1;


    static void Main()
    {
        string[,] deck =
            {
                {"2\u2660","3\u2660","4\u2660","5\u2660","6\u2660","7\u2660","8\u2660","9\u2660","T\u2660","J\u2660","Q\u2660","K\u2660","A\u2660"},
                {"2\u2663","3\u2663","4\u2663","5\u2663","6\u2663","7\u2663","8\u2663","9\u2663","T\u2663","J\u2663","Q\u2663","K\u2663","A\u2663"},
                {"2\u2665","3\u2665","4\u2665","5\u2665","6\u2665","7\u2665","8\u2665","9\u2665","T\u2665","J\u2665","Q\u2665","K\u2665","A\u2665"},
                {"2\u2666","3\u2666","4\u2666","5\u2666","6\u2666","7\u2666","8\u2666","9\u2666","T\u2666","J\u2666","Q\u2666","K\u2666","A\u2666"}
            };

        string title = "ARCH DEVIL's POKER";
        int heigth = 23;
        int width = 60;
        int cardWidth = 8;
        int cardHeight = 7;
        int winnings = 0;
        int bet = 1;
        int maxBet = 10;
        int coins = 100;
        bool[] holdCards = new bool[5];

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
            Array.Clear(holdCards, 0, holdCards.Length);
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine();
            Console.WriteLine(" ROYAL FLUSH");
            PrintOnPosition(30, 1, "x" + royalFlushPoints.ToString());
            PrintOnPosition(width - 10, 2, coins.ToString().PadLeft(10, ' '));
            PrintOnPosition(width - 5, 1, "Coins");

            Console.WriteLine(" STRAIGHT FLUSH");
            PrintOnPosition(30, 2, "x" + straightFlushPoints.ToString());
            Console.WriteLine();

            Console.WriteLine(" 4 OF A KIND");
            PrintOnPosition(30, 3, "x" + fourOfAKindPoints.ToString());

            
            Console.WriteLine();
            Console.WriteLine(" FULL HOUSE");
            PrintOnPosition(width - 3, 5, "BET");
            PrintOnPosition(width - 3, 6, bet.ToString().PadLeft(3, ' '));

            PrintOnPosition(30, 4, "x" + fullHousePoints.ToString());
            Console.WriteLine();

            Console.WriteLine(" FLUSH");
            PrintOnPosition(30, 5, "x" + flushPoint.ToString());
            Console.WriteLine();

            Console.WriteLine(" STRAIGHT");
            PrintOnPosition(30, 6, "x" + straightPoint.ToString());
            Console.WriteLine();

            Console.WriteLine(" 3 OF A KIND");
            PrintOnPosition(30, 7, "x" + threeOfAKindPoint.ToString());
            Console.WriteLine();

            Console.WriteLine(" TWO PAIR");
            PrintOnPosition(30, 8, "x" + twoPairPoint.ToString());
            Console.WriteLine();

            Console.WriteLine(" HIGH PAIR");
            PrintOnPosition(30, 9, "x" + highCardPoint.ToString());
            PrintOnPosition(40, 9, "WINNINGS: " + winnings);

            Console.WriteLine();
            Console.Write(string.Empty.PadLeft(width, '_'));
            Console.WriteLine(); //here write guidances what to press at this step - have to get the y of this line - later
            Console.Write(string.Empty.PadLeft(width, '_'));



            for (int i = 0, j = 0; j < 5; i += 11, j++)
            {
                CardBack(cardHeight, cardWidth, i + 4, 14);
                Thread.Sleep(130);
            }

            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.ForegroundColor = ConsoleColor.Yellow;

            // Bet
            bet = Bet(bet, width, heigth, maxBet);

            if (coins > bet)
            {
                coins -= bet;
                PrintOnPosition(width - 10, 2, coins.ToString().PadLeft(10, ' '));
            }
            else
            {
                bet = coins;
                coins = 0;
                PrintOnPosition(width - 10, 2, "ALL IN".PadLeft(10, ' '));
                PrintOnPosition(width - 2, 6, bet.ToString().PadLeft(2, ' '));
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

            int wining = CheckForWinnings(deck, playCards);
            coins += wining * bet;
           
            
            //Game Over or Next Deal



            Console.ReadKey();
        }
    }


    // Read me: мисля си че за проверката на кво се печели трябва да се направи първо в един голям метод. и тови метод да извиква последователно маклки методи
    // които да да за проверка на отделните възможни комбинации от карти
    // сортиранията по скоро OrderBy ще помогне на PlayCards за после като обхожваме матрицата от карти.
    // също съм разменил два реда от матрицата, за да може и тя да е подредена, за да може като я обхождаме да си е подредена. ебати изречението :)
    // после тва да го изтрием. в 2:30am не ми е никак лесно

    private static int CheckForWinnings(string[,] deck, string[] playCards)
    {

        string win = string.Empty;
        bool isMatch = false;

        isMatch = RoyalFlush(deck,playCards);
        if (isMatch)
        {
            win = "ROYAL FLUSH";
            PrintMatch(1, 1, win);
            return royalFlushPoints;
        }

        isMatch = StraightFlush(deck, playCards);
        if (isMatch)
        {
            win = "STRAIGHT FLUSH";
            PrintMatch(1, 2, win);

            return straightFlushPoints;
        }

        isMatch = FourOfAKind(deck, playCards);
        if (isMatch)
        {
            win = "4 OF A KIND";
            PrintMatch(1, 3, win);

            return fourOfAKindPoints;
        }

        isMatch = FullHouse(deck, playCards);
        if (isMatch)
        {
            win = "FULL HOUSE";
            PrintMatch(1, 4, win);

            return fullHousePoints;
        }

        isMatch = Flush(deck, playCards);
        if (isMatch)
        {
            win = "FLUSH";
            PrintMatch(1, 5, win);

            return flushPoint;
        }

        isMatch = Straight(deck, playCards);
        if (isMatch)
        {
            win = "STRAIGHT";
            PrintMatch(1, 6, win);

            return straightPoint;
        }

        isMatch = ThreeOfAKind(deck, playCards);
        if (isMatch)
        {
            win = "3 OF A KIND";
            PrintMatch(1, 7, win);

            return threeOfAKindPoint;
        }

        isMatch = TwoPair(deck, playCards);
        if (isMatch)
        {
            win = "TWO PAIR";
            PrintMatch(1, 8, win);

            return twoPairPoint;
        }

        isMatch = HighPair(deck, playCards);
        if (isMatch)
        {
            // this is for the console to print in black-yellow-black-yellow what you win.
            win = "HIGHT PAIR";
            PrintMatch(1, 9, win);
            
            // return point to colect in the coins
            return highCardPoint;
        }
        return 0;
    }

    private static bool RoyalFlush(string[,] deck, string[] playCards)
    {
        return false;
    }

    private static bool StraightFlush(string[,] deck, string[] playCards)
    {
        Array.Sort(playCards);

        for (int i = 0; i < deck.GetLength(0) - 1; i++)
        {
            for (int j = 0; j < deck.GetLength(1) - 5; j++)
            {
                if (playCards[0] == deck[i, j] &&
                    playCards[1] == deck[i, j + 1] &&
                    playCards[2] == deck[i, j + 2] &&
                    playCards[3] == deck[i, j + 3] &&
                    playCards[4] == deck[i, j + 4])
                {
                    return true;
                }
            }
        }
        return false;
    }

    private static bool FourOfAKind(string[,] deck, string[] playCards)
    {
        return false;
    }

    private static bool FullHouse(string[,] deck, string[] playCards)
    {
        return false;
    }

    private static bool Flush(string[,] deck, string[] playCards)
    {
        return false;
    }

    private static bool Straight(string[,] deck, string[] playCards)
    {
        return false;
    }

    private static bool ThreeOfAKind(string[,] deck, string[] playCards)
    {
        return false;
    }

    private static bool TwoPair(string[,] deck, string[] playCards)
    {
        return false;
    }

    private static bool HighPair(string[,] deck, string[] playCards)
    {
        Array.Sort(playCards);

        for (int k = 0; k < 4; k++)
        {
            for (int i = 0; i < deck.GetLength(1); i++)
            {
                if ((playCards[k] == deck[0, i] && playCards[k + 1] == deck[1, i]) ||
                    (playCards[k] == deck[0, i] && playCards[k + 1] == deck[2, i]) ||
                    (playCards[k] == deck[0, i] && playCards[k + 1] == deck[3, i]) ||
                    (playCards[k] == deck[1, i] && playCards[k + 1] == deck[2, i]) ||
                    (playCards[k] == deck[1, i] && playCards[k + 1] == deck[3, i]) ||
                    (playCards[k] == deck[2, i] && playCards[k + 1] == deck[3, i]))
                {
                    return true;
                }
            }
        }
        return false;

    }

    // this is for the console to print in black-yellow-black-yellow what you win.
    private static void PrintMatch(int p1, int p2, string win)
    {
        Console.ForegroundColor = ConsoleColor.Black;
        PrintOnPosition(p1, p2, win);
        Thread.Sleep(400);
        Console.ForegroundColor = ConsoleColor.Yellow;
        PrintOnPosition(p1, p2, win);
        Thread.Sleep(400);
        Console.ForegroundColor = ConsoleColor.Black;
        PrintOnPosition(p1, p2, win);
        Thread.Sleep(400);
        Console.ForegroundColor = ConsoleColor.Yellow;
        PrintOnPosition(p1, p2, win);
        Thread.Sleep(400);
        Console.ForegroundColor = ConsoleColor.Black;
        PrintOnPosition(p1, p2, win);
        Thread.Sleep(400);
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

    static int Bet(int bet, int width, int heigth, int maxBet)
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
                PrintOnPosition(width - 2, 6, bet.ToString().PadLeft(2, ' '));
            }
            if (keyPressed.Key == ConsoleKey.DownArrow && bet > 1)
            {
                // sound for betting down
                Console.Beep(424, 100);
                Console.Beep(364, 100);

                bet--;
                PrintOnPosition(width - 2, 6, bet.ToString().PadLeft(2, ' '));
            }

        } while (keyPressed.Key != ConsoleKey.Spacebar);

        // sound for you bet
        Console.Beep(1800, 200);
        Console.Beep(1800, 200);

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


        // delete sign hold form the cards (they are already hold)
        PrintOnPosition(6, 21, "    ");
        PrintOnPosition(17, 21, "    ");
        PrintOnPosition(28, 21, "    ");
        PrintOnPosition(39, 21, "    ");
        PrintOnPosition(50, 21, "    ");

        Console.Beep(1800, 200);
        Console.Beep(1800, 200);

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
        Console.Write(c);
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
