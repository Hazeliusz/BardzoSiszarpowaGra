using System;
using System.Collections.Generic;
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
}
