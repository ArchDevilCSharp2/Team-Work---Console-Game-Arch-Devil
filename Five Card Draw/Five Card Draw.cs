using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class Program
{
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
        int heigth = 30;
        int width = 70;
        int cardWidth = 8;
        int cardHeight = 5;

        Console.Title = title;
        Console.WindowHeight = heigth;
        Console.WindowWidth = width;
        Console.BufferHeight = heigth;
        Console.BufferWidth = width;
        Random r = new Random();

        string text = "";// towa e za GAME OVER 
        int x = width / 2;
        int y = heigth / 2;

        // deck[r.Next(0, 4), r.Next(0, 13)] randoma ot testeto

        //welcome screen

        Console.ForegroundColor = ConsoleColor.Magenta;
        WriteAtPlace(title, (width - 18) / 2, heigth / 2 - 4);
        WriteAtPlace("how to play - type help", (width - 18) / 2, heigth / 2 - 1);
        WriteAtPlace("Press enter to play", (width - 18) / 2, heigth / 2 + 1);
        WriteAtPlace(text, (width - 18) / 2, heigth / 2 + 3);

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
            return;
            Console.WriteLine("You pressed escape");
        }

        // TOVA BIH GO IZTRIL
        //string choice = Console.ReadLine();
        //if (choice == "help")
        //{
        //    //HelpScreen();
        //}
        

        //HelpScreen - SpaceBar draws, keys 1-5 hold cards, Esc to return ot welcome screen

        //MainScreen - winnings block - coins, bet, current winnings and 5 card backs..

        //Draw - 5 cards

        //Hold and Redraw

        //Check for Winnings - CheckForWinnings(play[]) - moje i da e [,] - 6 widim

        //Game Over or Next Deal

        //malko sym testwal tuka dolu 67-82 da widq kyf razmer da e kartata i tn
        Console.WriteLine();

        Console.BackgroundColor = ConsoleColor.Black;

        Console.Clear();
        Console.WriteLine();
        Console.ResetColor();

        HomeScreen();

        Console.ReadLine();

        for (int i = 0; i < cardHeight; i++)
        {
            for (int j = 0; j < cardWidth; j++)
            {

                Console.BackgroundColor = ConsoleColor.White;
                Console.Write(' ');

            }
            Console.WriteLine();
        }
        Console.WriteLine();
        Console.ResetColor();
    }

    private static void HomeScreen()
    {
        PrintOnPosition(30, 1, "9000", ConsoleColor.Yellow);
        PrintOnPosition(67, 1, "893", ConsoleColor.Yellow);
        PrintOnPosition(63, 0, "Credits", ConsoleColor.Yellow);

        WriteFullLine("ROYAL FLUSH");

        Console.WriteLine("STRAIGHT FLUSH");
        PrintOnPosition(30, 2, "1800", ConsoleColor.Yellow);
        Console.WriteLine();

        Console.WriteLine("FOUR OF A KIND");
        PrintOnPosition(30, 3, "720", ConsoleColor.Yellow);


        Console.WriteLine();
        Console.WriteLine("FULL HOUSE");
        PrintOnPosition(67, 3, "BET", ConsoleColor.Yellow);
        PrintOnPosition(68, 4, "18", ConsoleColor.Yellow);

        PrintOnPosition(30, 4, "180", ConsoleColor.Yellow);
        Console.WriteLine();

        Console.WriteLine("FLUSH");
        PrintOnPosition(30, 5, "126", ConsoleColor.Yellow);
        Console.WriteLine();

        Console.WriteLine("STRAIGHT");
        PrintOnPosition(30, 6, "90", ConsoleColor.Yellow);
        Console.WriteLine();

        Console.WriteLine("THREE OF A KIND");
        PrintOnPosition(30, 7, "54", ConsoleColor.Yellow);
        Console.WriteLine();

        Console.WriteLine("TWO PAIR");
        PrintOnPosition(30, 8, "36", ConsoleColor.Yellow);
        Console.WriteLine();

        Console.WriteLine("HIGH PAIR");
        PrintOnPosition(30, 9, "18", ConsoleColor.Yellow);
        PrintOnPosition(40, 9, "WINNINGS:0 ", ConsoleColor.White);

        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine();
    }

    //HelpScreen - SpaceBar draws, keys 1-5 hold cards, Esc to return ot welcome screen
    static void HelpMenu()
    {
        Console.Clear();
        PrintOnPosition(20, 6, "Keys 1 to 5: hold cards", ConsoleColor.Yellow);
        PrintOnPosition(20, 8, "Spacebar: draw cards", ConsoleColor.Yellow);
        PrintOnPosition(20, 10, "You can change any card", ConsoleColor.Yellow);
        PrintOnPosition(20, 12, "Press enter to play", ConsoleColor.Yellow);

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
    
    private static void WriteAtPlace(string text, int x, int y)
    {
        Console.SetCursorPosition(x, y);
        Console.Write(text);
    }
    static void WriteFullLine(string value) // Method, which makes Royal FLush line in RED
    {
        //
        // This method writes an entire line to the console with the string.
        //
        Console.ForegroundColor = ConsoleColor.Yellow;

        Console.WriteLine(value.PadRight(Console.WindowWidth - 40));
        Console.ResetColor();
    }
    static void PrintOnPosition(int x, int y, string c, ConsoleColor color = ConsoleColor.Black)//Method, which sets the High Score position
    {
        Console.SetCursorPosition(x, y);
        Console.ForegroundColor = color;
        Console.Write(c);
    }

}
