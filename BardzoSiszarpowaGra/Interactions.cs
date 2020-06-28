using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace BardzoSiszarpowaGra
{
	class CharactersInteractions
	{
		private Character character1;
		private Character character2;

	public CharactersInteractions(Character actor1, Character actor2)
		{
			character1 = actor1;
			character2 = actor2;
		}

		//Skrypt walki, zwraca wskaźnik na zwycięzcę (lub nullptr w przypadku jego braku

		public int MonsterFight()
		{ //1 - ucieczka, 2 - wygrana gracza, 0 - przegrana gracza
			Character player = character1;
			Character monster = character2;
			Random rand = new Random();
			List<Skill> playerSkills = player.getSkills();
			playerSkills.ForEach((element) =>
			{
				element.setCurrentCooldown(0);
			});

			bool turn = true;
			Console.WriteLine("Walka: " + character1.getName() + " vs. " + character2.getName());

			while (true)
			{
				if (turn)
				{
					playerSkills.ForEach((element) =>
					{
						if (element.getCurrentCooldown() > 0)
							element.reduceCurrentCooldown();
					});
					Console.WriteLine("Twój ruch!");
					Console.WriteLine("Aktualne HP: " + player.getHP());
					Console.WriteLine("HP przeciwnika: " + monster.getHP());
					int decyzja = 0;
					while (true)
					{
						Console.WriteLine("Wybierz akcje: ");
						for (int i = 0; i < playerSkills.Count; i++)
						{
							if (Convert.ToBoolean(playerSkills[i].getCurrentCooldown()))
								Console.WriteLine((i + 1) + ". " + playerSkills[i].getName() + " | Czas odnowienia: " +
									playerSkills[i].getCurrentCooldown());
							else
								Console.WriteLine((i + 1) + ". " + playerSkills[i].getName());
						}
						Console.WriteLine(0 + ". Ucieczka");
						try
						{
							decyzja = Convert.ToInt32(Console.ReadLine());
						}
                        catch (FormatException)
                        {
							decyzja = -1;
                        }
						if (decyzja == 0)
						{
							Console.WriteLine(player.getName() + (player.getSex() ? " uciekła!" : " uciekł!"));
							return 1; //ucieczka
						}
						else if (decyzja < 1 || decyzja > playerSkills.Count)
						{
							Console.WriteLine("Niepoprawna decyzja");
						}
						else
						{
							if(Convert.ToBoolean(playerSkills[decyzja - 1].getCurrentCooldown()))
								Console.WriteLine("Ta umiejętność się odnawia.");
							else
								break;
						}
					}


					playerSkills[decyzja - 1].use(player, monster);


					if (monster.getHP() <= 0)
					{
						Console.WriteLine("Wygrywasz walkę!");
						player.addEXP(Convert.ToInt32(monster.getStats().getByChar('s') * 2.5) + Convert.ToInt32(monster.getStats().getByChar('e') * 2.5));
						Console.WriteLine("Przy ciele bestii znajdujesz " + monster.getGold() + " sztuk złota!");
						player.modifyGold(monster.getGold());
						Console.ReadKey();
						Console.Clear();
						return 2;
					}
				}
				else
				{
					int dmg = 0;
					if (monster.getStats().getByChar('s') > monster.getStats().getByChar('i'))
					{
						dmg = Convert.ToInt32(monster.getStats().getByChar('s') * 1.5) - (rand.Next(0, player.getStats().getByChar('l')));
						if (dmg <= 0)
							Console.WriteLine(monster.getName() + " nie trafia!");
						else
						{
							Console.WriteLine(monster.getName() + " atakuje Cię za " + dmg + " HP!");
							player.modifyHP(-dmg);
						}
					}
					else
					{
						dmg = Convert.ToInt32(monster.getStats().getByChar('i') * 1.5) - rand.Next(0, player.getStats().getByChar('l'));
						if (dmg <= 0)
							Console.WriteLine(monster.getName() + " nie trafia!");
						else
						{
							Console.WriteLine(monster.getName() + " rzuca zaklęcie, zadając Ci " + dmg + " HP!");
							player.modifyHP(-dmg);
						}
					}
					if (player.getHP() <= 0)
					{
						Console.WriteLine("Przegrywasz walkę...");
						return 0;
					}
					Console.ReadKey();
					Console.Clear();
				}
				turn = !turn;

			}
		}

	};

	static class NPCInteraction
    {
		private static string npcX;
		private static string npcY;
		public static void startConversation(Character player, int X, int Y)
        {
			string[] chars = File.ReadAllLines("Character.txt");
			npcX = "error";
			npcY = "error";
			foreach(string character in chars)
            {
				if (character[0] == '/' && character[1] == '/')
					continue;
				string posX = Convert.ToString(character[0]) + Convert.ToString(character[1]) + Convert.ToString(character[2]);
				string posY = Convert.ToString(character[3]) + Convert.ToString(character[4]) + Convert.ToString(character[5]);
				if (Convert.ToInt32(posX) == X && Convert.ToInt32(posY) == Y)
                {
					npcX = posX;
					npcY = posY;
					break;
                }
            }
			if (npcX == "error" || npcY == "error")
				return;
			Console.Clear();
			conversation(player);
			
        }
		private static void conversation(Character player)
        {
			string[] dial;
			try
			{
				dial = File.ReadAllLines("dialogue/DIA_" + npcX + "_" + npcY + ".txt");
			}
			catch(DirectoryNotFoundException)
			{
				Console.WriteLine("Wybrany numer jest niepoprawny.");
				Console.ReadKey();
				return;
			}
			int choice = -1;
			foreach (string line in dial)
            {
				if (line[0] != '!')
					continue;
				string command = line.Split(' ')[0];
				switch (command)
                {
					case "!say":
						Console.WriteLine(line.Substring(5));
						break;
					case "!checksay":
						if (player.getStats().getByChar(line[10]) >= Convert.ToInt32(line.Split(' ')[2]))
							Console.WriteLine(line.Split(' ', 4)[3]);
						else
							continue;
						break;
					case "!ask":
						choice = -1;
						string[] choices = line.Split(';');
						choices[0] = choices[0].Substring(4).Trim();
						int highest = 0;
						Console.WriteLine(choices[0]);
						for(int i = 1; i < choices.Length; i++)
                        {
							choices[i] = choices[i].Trim();
							Console.WriteLine((i) + ". " + choices[i]);
							highest = i;
                        }
						while (!(choice >= 0 && choice < choices.Length))
						{
							try
							{
								choice = Convert.ToInt32(Console.ReadLine());
							}
							catch (FormatException)
							{
								choice = -1;
							}
							if (!(choice >= 0 && choice < choices.Length))
								Console.WriteLine("Niepoprawna deycyzja, wybierz ponownie.");
						}
						break;
					case "!choice":
						string[] answers = line.Split(' ', 3);
						if (choice == -1)
							Console.WriteLine("Chwileczkę, zanim odpowiesz daj mi zadać pytanie!");
						else if (Convert.ToInt32(answers[1]) == choice)
							Console.WriteLine(answers[2]);
						else
							continue;
						break;
                }
				Console.ReadKey(true);
            }
		}
    };
}
