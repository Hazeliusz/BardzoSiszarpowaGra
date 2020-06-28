using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BardzoSiszarpowaGra
{
	class Map
	{

		private Character player;
		private char[,] wholeMap;
		private int playerX;
		private int playerY;
		private int width;
		private int height;

		public Map(Character cPlayer)
        {
			player = cPlayer;
			string[] lines = File.ReadAllLines("Whole.txt");
			for (int i = 0; i < lines.Length; i++)
				lines[i] = lines[i].Replace(" ", String.Empty);
			width = lines[0].Length;
			height = lines.Length;
			wholeMap = new char[width, height];
			for(int x = 0; x < width; x++)
            {
				for(int y = 0; y < height; y++)
                {
					wholeMap[x, y] = lines[y].ToCharArray()[x];
                }
            }
			playerX = 1;
			playerY = 37;
			wholeMap[playerX, playerY] = 'U';
        }

		public void drawAll()
        {
			for(int y = 0; y < height; y++)
            {
				for(int x = 0; x < width; x++)
                {
					Console.Write(wholeMap[x, y] + " ");
                }
				Console.WriteLine("");
            }
        }
		
		public void draw()
        {
			Console.Clear();
			int viewXStart = playerX - 8;
			int viewYStart = playerY - 8;
			while (viewXStart < 0)
				viewXStart++;
			while (viewXStart + 15 > width)
				viewXStart--;
			while (viewYStart < 0)
				viewYStart++;
			while (viewYStart + 15 > height)
				viewYStart--;
			for(int y = viewYStart; y < viewYStart + 15; y++)
            {
				for(int x = viewXStart; x < viewXStart + 15; x++)
                {
					Console.Write(wholeMap[x, y] + " ");
                }
				Console.WriteLine("");
            }
        }

		public void move()
        {
			char move = Console.ReadKey().KeyChar;
			if (!check(move))
				return;
            switch (move)
            {
				case 'w':
				case 'W':
					wholeMap[playerX, playerY] = '-';
					playerY--;
					wholeMap[playerX, playerY] = 'U';
					break;
				case 's':
				case 'S':
					wholeMap[playerX, playerY] = '-';
					playerY++;
					wholeMap[playerX, playerY] = 'U';
					break;
				case 'd':
				case 'D':
					wholeMap[playerX, playerY] = '-';
					playerX++;
					wholeMap[playerX, playerY] = 'U';
					break;
				case 'a':
				case 'A':
					wholeMap[playerX, playerY] = '-';
					playerX--;
					wholeMap[playerX, playerY] = 'U';
					break;
				default:
					break;
            }
        }

		public bool check(char direction)
        {
			Random rand = new Random();
			bool checkForInteractable(char interaction, int x, int y)
            {
				switch (interaction)
				{
					case '1':
					case '2':
					case '3':
					case '4':
					case '5':
					case '6':
					case '7':
					case '8':
					case '9':
					case '!':
						Monster enemy = new Monster((int)(interaction - '0'));
						if (interaction == '!')
							enemy = new Monster(10);
						CharactersInteractions fight = new CharactersInteractions(player, enemy);
						if (fight.MonsterFight() == 2)
							return true;
						else
							return false;
					case 'G':
						player.modifyGold(rand.Next(1, 200));
						return true;
					case 'N':
						NPCInteraction.startConversation(player, x, y);
						return false;
					default:
                        return false;
					
				}
			}
            switch (direction)
            {
				case 'w':
				case 'W':
					if (wholeMap[playerX, playerY - 1] == '-')
						return true;
					else
						return checkForInteractable(wholeMap[playerX, playerY - 1], playerX, playerY - 1);
				case 's':
				case 'S':
					if (wholeMap[playerX, playerY + 1] == '-')
						return true;
					else
						return checkForInteractable(wholeMap[playerX, playerY + 1], playerX, playerY + 1);
				case 'd':
				case 'D':
					if (wholeMap[playerX + 1, playerY] == '-')
						return true;
					else
						return checkForInteractable(wholeMap[playerX + 1, playerY], playerX + 1, playerY); ;
				case 'a':
				case 'A':
					if (wholeMap[playerX - 1, playerY] == '-')
						return true;
					else
						return checkForInteractable(wholeMap[playerX - 1, playerY], playerX - 1, playerY); ;
				default:
					return false;
			}
        }
		
	};
}
