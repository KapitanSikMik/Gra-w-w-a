using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

class Program
{
    static void Main()
    {
        Console.WindowHeight = 16;
        Console.WindowWidth = 32;
        int screenWidth = Console.WindowWidth;
        int screenHeight = Console.WindowHeight;
        Random random = new Random();

        Pixel head = new Pixel
        {
            XPos = screenWidth / 2,
            YPos = screenHeight / 2,
            Color = ConsoleColor.Green
        };

        string movement = "RIGHT";
        List<Pixel> snake = new List<Pixel>();
        int score = 0;

        Pixel food = new Pixel
        {
            XPos = random.Next(1, screenWidth),
            YPos = random.Next(1, screenHeight),
            Color = ConsoleColor.Cyan
        };

        // Dodanie ścian
        List<Pixel> walls = new List<Pixel>();
        for (int i = 0; i < screenWidth; i++)
        {
            walls.Add(new Pixel { XPos = i, YPos = 0, Color = ConsoleColor.White }); // Górna ściana
            walls.Add(new Pixel { XPos = i, YPos = screenHeight - 1, Color = ConsoleColor.White }); // Dolna ściana
        }
        for (int i = 1; i < screenHeight - 1; i++)
        {
            walls.Add(new Pixel { XPos = 0, YPos = i, Color = ConsoleColor.White }); // Lewa ściana
            walls.Add(new Pixel { XPos = screenWidth - 1, YPos = i, Color = ConsoleColor.White }); // Prawa ściana
        }

        ConsoleKeyInfo keyInfo;
        bool gameover = false;

        while (!gameover)
        {
            Console.Clear();

            // Rysowanie ścian
            foreach (Pixel wall in walls)
            {
                Console.ForegroundColor = wall.Color;
                Console.SetCursorPosition(wall.XPos, wall.YPos);
                Console.Write("#");
            }

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.SetCursorPosition(food.XPos, food.YPos);
            Console.Write("*");

            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(0, 0);
            Console.Write($"Score: {score}");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.SetCursorPosition(head.XPos, head.YPos);
            Console.Write("■");

            foreach (Pixel pixel in snake)
            {
                Console.SetCursorPosition(pixel.XPos, pixel.YPos);
                Console.Write("■");
            }

            Thread.Sleep(100);

            keyInfo = Console.ReadKey(true);

            switch (keyInfo.Key)
            {
                case ConsoleKey.UpArrow:
                    if (movement != "DOWN")
                        movement = "UP";
                    break;

                case ConsoleKey.DownArrow:
                    if (movement != "UP")
                        movement = "DOWN";
                    break;

                case ConsoleKey.LeftArrow:
                    if (movement != "RIGHT")
                        movement = "LEFT";
                    break;

                case ConsoleKey.RightArrow:
                    if (movement != "LEFT")
                        movement = "RIGHT";
                    break;
            }

            switch (movement)
            {
                case "UP":
                    head.YPos--;
                    break;

                case "DOWN":
                    head.YPos++;
                    break;

                case "LEFT":
                    head.XPos--;
                    break;

                case "RIGHT":
                    head.XPos++;
                    break;
            }

            if (head.XPos == food.XPos && head.YPos == food.YPos)
            {
                score++;
                food.XPos = random.Next(1, screenWidth);
                food.YPos = random.Next(1, screenHeight);
            }

            // Sprawdzenie kolizji z ścianami
            if (head.XPos == 0 || head.XPos == screenWidth - 1 || head.YPos == 0 || head.YPos == screenHeight - 1)
                gameover = true;

            // Sprawdzenie kolizji z ciałem węża
            foreach (Pixel pixel in snake)
            {
                if (head.XPos == pixel.XPos && head.YPos == pixel.YPos)
                    gameover = true;
            }

            snake.Add(new Pixel { XPos = head.XPos, YPos = head.YPos });

            while (snake.Count > score + 1)
                snake.RemoveAt(0);
        }

        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Red;
        Console.SetCursorPosition(screenWidth / 5, screenHeight / 2);
        Console.WriteLine("Game Over");
        Console.SetCursorPosition(screenWidth / 5, screenHeight / 2 + 1);
        Console.WriteLine($"Your Score is: {score}");
        Console.SetCursorPosition(screenWidth / 5, screenHeight / 2 + 2);
    }
}

public class Pixel
{
    public int XPos { get; set; }
    public int YPos { get; set; }
    public ConsoleColor Color { get; set; }
}
