using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class Program
{
    static Random r = new Random();

    const int royalFlushPoints = 500;
    const int streetFlushPoints = 100;
    const int fourOfAKindPoints = 40;
    const int fullHousePoints = 12;
    const int flushPoint = 7;
    const int straightPoint = 5;
    const int threeOfAKindPoint = 3;
    const int twoPairPoint = 2;
    const int highCardPoint = 1;
    const int creditsPoints = 42;
    const int betPoints = 42;

    static void Main()
    {

        string[,] deck =
            {
                {"2\u2660","3\u2660","4\u2660","5\u2660","6\u2660","7\u2660","8\u2660","9\u2660","10\u2660","J\u2660","Q\u2660","K\u2660","A\u2660"},
                {"2\u2665","3\u2665","4\u2665","5\u2665","6\u2665","7\u2665","8\u2665","9\u2665","10\u2665","J\u2665","Q\u2665","K\u2665","A\u2665"},
                {"2\u2665","3\u2666","4\u2666","5\u2666","6\u2666","7\u2666","8\u2666","9\u2666","10\u2666","J\u2666","Q\u2666","K\u2666","A\u2666"},
                {"2\u2663","3\u2663","4\u2663","5\u2663","6\u2663","7\u2663","8\u2663","9\u2663","10\u2663","J\u2663","Q\u2663","K\u2663","A\u2663"}
            };

        string title = "ARCH DEVIL's POKER";
        int heigth = 25;
        int width = 60;
        int cardWidth = 8;
        int cardHeight = 7;
        int winnings = 0;
        int bet = 1;
        int coins = 100;

        Console.CursorVisible = false;
        Console.Title = title;
        Console.WindowHeight = heigth;
        Console.WindowWidth = width;
        Console.BufferHeight = heigth;
        Console.BufferWidth = width;

        //welcome screen - OK

        WelcomeScreen(title, heigth, width);

        //HelpScreen - SpaceBar draws, keys 1-5 hold cards, Esc to return ot welcome screen

        //MainScreen - winnings block - coins, bet, current winnings and 5 card backs..

        while (coins > 0)
        {
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.BackgroundColor = ConsoleColor.DarkGreen;

            Console.WriteLine();
            Console.WriteLine(" ROYAL FLUSH");
            PrintOnPosition(30, 1, "x" + royalFlushPoints.ToString());
            PrintOnPosition(width - 10, 2, coins.ToString().PadLeft(10));
            PrintOnPosition(width - 5, 1, "Coins");



            Console.WriteLine(" STRAIGHT FLUSH");
            PrintOnPosition(30, 2, "x" + streetFlushPoints.ToString());
            Console.WriteLine();

            Console.WriteLine(" 4 OF A KIND");
            PrintOnPosition(30, 3, "x" + fourOfAKindPoints.ToString());


            Console.WriteLine();
            Console.WriteLine(" FULL HOUSE");
            PrintOnPosition(width - 3, 5, "BET");
            PrintOnPosition(width - 2, 6, betPoints.ToString());

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
            Console.WriteLine();
            Console.WriteLine();



            for (int i = 0; i < (cardWidth + 3) * 5; i += 11)
            {
                CardBack(cardHeight, cardWidth, i + 4, 14);
            }



            //Draw - 5 cards - OK
            var drawedCards = new List<string>();

            var playCards = DrawCards(deck, r, drawedCards);

            //Hold

            //Redraw

            //Check for Winnings - CheckForWinnings(play[]) - moje i da e [,] - 6 widim

            //Game Over or Next Deal



            Console.ReadLine();
        }
        
    }

    private static void WelcomeScreen(string title, int heigth, int width)
    {
        Console.ForegroundColor = ConsoleColor.Magenta;
        PrintOnPosition((width - 18) / 2, heigth / 2 - 4, title);
        PrintOnPosition((width - 18) / 2, heigth / 2 - 1, "how to play - h");
        PrintOnPosition((width - 18) / 2, heigth / 2 + 1, "press ENTER to play");

        // Joro

        ConsoleKeyInfo startKey;
        startKey = Console.ReadKey();

        if (startKey.Key == ConsoleKey.H)
        {
            // Console.WriteLine("You pressed H");
            HelpMenu();
        }
        if (startKey.Key == ConsoleKey.Enter)
        {
            Console.WriteLine("You pressed enter");
        }
        if (startKey.Key == ConsoleKey.Escape)
        {
            Console.WriteLine("You pressed escape");
        }
    }

    static void CardBack(int height, int width, int x, int y)
    {
        for (int i = 0; i < width; i++) //cadrBAck start
        {
            for (int j = 0; j < height; j++)
            {
                PrintOnPosition(x + i, y + j, "*");
            }
        }   //cardBack end
    }


    //HelpScreen - SpaceBar draws, keys 1-5 hold cards, Esc to return ot welcome screen
    static void HelpMenu()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Yellow;
        //Console.BackgroundColor = ConsoleColor.Yellow;
        PrintOnPosition(20, 6, "Keys 1 to 5: hold cards");
        PrintOnPosition(20, 8, "Spacebar: draw cards");
        PrintOnPosition(20, 10, "You can change any card");
        PrintOnPosition(20, 12, "Press enter to play");


        while (true)
        {
            ConsoleKeyInfo key;
            key = Console.ReadKey();
            if (key.Key == ConsoleKey.Enter)
            {
                break;
            }
        }
    }
   
    static void PrintOnPosition(int x, int y, string c)
    {
        Console.SetCursorPosition(x, y);
        Console.Write(c);
    }

    private static string[] DrawCards(string[,] deck, Random r, List<string> drawedCards)
    {
        var playCards = new string[5];

        for (int i = 0; i < 5; i++)
        {
            playCards[i] = deck[r.Next(0, 4), r.Next(0, 13)];
            if (drawedCards.Contains(playCards[i]))
            {
                i--;
                continue;
            }
            else
            {
                drawedCards.Add(playCards[i]);
            }

        }
        return playCards;
    }

}
