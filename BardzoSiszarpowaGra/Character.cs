using System;
using System.Collections.Generic;
using System.Text;

namespace BardzoSiszarpowaGra
{
	enum Proffesion
	{
		PROFF_KNIGHT, PROFF_ARCHER, PROFF_BARD, PROFF_DARK_KNIGHT, PROFF_CLERIC, PROFF_MAGE, PROFF_MONSTER
	};

	class Level
	{
		private int level;
		private int currentEXP;
		private int thresholdEXP;
		public void setLevel(int lvl)
        {
			level = lvl;
        }
		public int getCurrentEXP()
        {
			return currentEXP;
        }
		public int getLevel()
        {
			return level;
        }
		public int getLevelThreshold(int lvl)
        {
			if (lvl == 1)
				return 50;
			else
				return (lvl * 10) + getLevelThreshold(lvl - 1);
		}
		public void levelUp()
        {
			level++;
			thresholdEXP = getLevelThreshold(level);
			currentEXP = 0;
		}
		public bool checkLevelUp()
        {
			if (currentEXP >= thresholdEXP)
			{
				levelUp();
				return true;
			}
			else
				return false;
		}
		public void addEXP(int exp)
        {
			currentEXP += exp;
		}
		public Level(int lvl = 1)
        {
			level = lvl;
			currentEXP = 0;
			thresholdEXP = getLevelThreshold(lvl);
		}
	};

	abstract class Character
	{
		protected string name;
		protected Proffesion CharClass;
		protected bool sex; //true - kobieta, false - mezczyzna
		protected Statistics special;
		protected Level lvl;
		protected int gold;
		//Currently unused
		//List<Weapon> weapon_eq;
		//List<Armor> armor_eq;
		//List<Equipment> equipment_eq;

		public Character(string setName, bool setSex, Statistics stats, int lvls = 1)
        {
			name = setName;
			sex = setSex;
			special = stats;
			//Add skills!
			lvl.setLevel(lvls);
		}
		public Statistics getStats()
		{
			return special;
		}

		public Proffesion getProffesion()
		{
			return CharClass;
		}

		public int getLevel()
		{
			return lvl.getLevel();
		}


		public string getName()
		{
			return name;
		}
		public string getProffesionName()
		{
			switch (CharClass)
			{
				case Proffesion.PROFF_KNIGHT:
					return "rycerz";
				case Proffesion.PROFF_ARCHER:
					return "lucznik";
				case Proffesion.PROFF_BARD:
					return "bard";
				case Proffesion.PROFF_CLERIC:
					return "kleryk";
				case Proffesion.PROFF_DARK_KNIGHT:
					return "upadly rycerz";
				case Proffesion.PROFF_MAGE:
					return "mag";
				default:
					return "nieznana";
			}
		}
		public bool getSex()
		{
			return sex;
		}

		public void checkLevelUp()
		{
			if (lvl.checkLevelUp())
			{
				Console.WriteLine((sex ? "Awansowałaś" : "Awansowałeś") + " na poziom" + lvl.getLevel() + "!");
				for (int i = 5; i > 0;)
				{
					Console.WriteLine("Wybierz statystyki, które chcesz ulepszyć: ");
					Console.WriteLine("Dostępne punkty: " + i);
					int choice = 0;
					Console.WriteLine("1. Siła");
					Console.WriteLine("2. Percepcja");
					Console.WriteLine("3. Wytzymałość");
					Console.WriteLine("4. Charyzma");
					Console.WriteLine("5. Inteligencja");
					Console.WriteLine("6. Zręczność");
					Console.WriteLine("7. Szczęście");
					try
					{
						choice = Convert.ToInt32(Console.ReadLine());
					}
					catch (InvalidCastException)
					{
						choice = 0;
					}
					switch (choice)
					{
						case 1:
							special.advanceStatsByChar('s', 1);
							i--;
							break;
						case 2:
							special.advanceStatsByChar('p', 1);
							i--;
							break;
						case 3:
							special.advanceStatsByChar('e', 1);
							i--;
							break;
						case 4:
							special.advanceStatsByChar('c', 1);
							i--;
							break;
						case 5:
							special.advanceStatsByChar('i', 1);
							i--;
							break;
						case 6:
							special.advanceStatsByChar('a', 1);
							i--;
							break;
						case 7:
							special.advanceStatsByChar('l', 1);
							i--;
							break;
						default:
							Console.WriteLine("Niepoprawny wybór! Wybierz ponownie.");
							break;
					}
				}
			}
		}
		public void addEXP(int exp)
        {
			lvl.addEXP(exp);
			checkLevelUp();
		} //sprawdza automatycznie, czy próg został osiągnięty
		public void writeStatistics()
        {
			Console.WriteLine("Sila: " + special.strength);
			Console.WriteLine("Percepcja: " + special.perception);
			Console.WriteLine("Wytrzymalosc: " + special.endurance);
			Console.WriteLine("Charyzma: " + special.charisma);
			Console.WriteLine("Inteligencja: " + special.intelligence);
			Console.WriteLine("Zrecznosc: " + special.agility);
			Console.WriteLine("Szczescie: " + special.luck);
		}
		public void drawCharacterCard()
        {
			Console.WriteLine("Imie: " + name);
			Console.WriteLine("Klasa: " + getProffesionName());
			Console.WriteLine("Poziom: " + lvl.getLevel());
			Console.WriteLine("Doświadczenie: " + lvl.getCurrentEXP() + "/" + lvl.getLevelThreshold(lvl.getLevel()));
			Console.WriteLine("Plec: " + (sex ? "kobieta" : "mezczyzna"));
			writeStatistics();
		}
		public abstract void randomizeStatistics();
		public int getGold()
        {
			return gold;
        }
		public void modifyGold(int modified)
        {
			gold += modified;
        }
	};

	class Knight : Character {
		public override void randomizeStatistics()
        {
			Random rand = new Random();
			special.strength = rand.Next(10, 20);
			special.perception = rand.Next(5, 15);
			special.endurance = rand.Next(20, 30);
			special.charisma = rand.Next(5, 10);
			special.intelligence = rand.Next(7, 12);
			special.agility = rand.Next(5, 9);
			special.luck = rand.Next(1, 21);
		}
		public Knight(string n, bool g, Statistics stats) : base(n, g, stats)
		{
			CharClass = Proffesion.PROFF_KNIGHT;
			//add skill
		}
	};

	class Archer : Character
	{
		public override void randomizeStatistics()
		{
			Random rand = new Random();
			special.strength = rand.Next(5, 10);
			special.perception = rand.Next(15, 25);
			special.endurance = rand.Next(10, 20);
			special.charisma = rand.Next(7, 12);
			special.intelligence = rand.Next(7, 12);
			special.agility = rand.Next(25, 35);
			special.luck = rand.Next(1, 21);
		}
		public Archer(string n, bool g, Statistics stats) : base(n, g, stats)
		{
			CharClass = Proffesion.PROFF_ARCHER;
			//add skill
		}
	};

	class Bard : Character
	{
		public override void randomizeStatistics()
		{
			Random rand = new Random();
			special.strength = rand.Next(5, 10);
			special.perception = rand.Next(5, 15);
			special.endurance = rand.Next(10, 20);
			special.charisma = rand.Next(25, 35);
			special.intelligence = rand.Next(15, 20);
			special.agility = rand.Next(3, 8);
			special.luck = rand.Next(1, 21);
		}
		public Bard(string n, bool g, Statistics stats) : base(n, g, stats)
		{
			CharClass = Proffesion.PROFF_BARD;
			//add skill
		}
	};

	class DarkKnight : Character
	{
		public override void randomizeStatistics()
		{
			Random rand = new Random();
			special.strength = rand.Next(30, 40);
			special.perception = rand.Next(5, 15);
			special.endurance = rand.Next(15, 20);
			special.charisma = 1;
			special.intelligence = rand.Next(10, 15);
			special.agility = rand.Next(10, 15);
			special.luck = 1;
		}
		public DarkKnight(string n, bool g, Statistics stats) : base(n, g, stats)
		{
			CharClass = Proffesion.PROFF_DARK_KNIGHT;
			//add skill
		}
	};

	class Cleric : Character
	{
		public override void randomizeStatistics()
		{
			Random rand = new Random();
			special.strength = rand.Next(5, 10);
			special.perception = rand.Next(5, 15);
			special.endurance = rand.Next(10, 15);
			special.charisma = rand.Next(15, 25);
			special.intelligence = rand.Next(25, 30);
			special.agility = rand.Next(3, 8);
			special.luck = rand.Next(1, 21);
		}
		public Cleric(string n, bool g, Statistics stats) : base(n, g, stats)
		{
			CharClass = Proffesion.PROFF_CLERIC;
			//add skill
		}
	};

	class Mage : Character
	{
		public override void randomizeStatistics()
		{
			Random rand = new Random();
			special.strength = rand.Next(5, 10);
			special.perception = rand.Next(5, 15);
			special.endurance = rand.Next(10, 20);
			special.charisma = rand.Next(10, 20);
			special.intelligence = rand.Next(25, 35);
			special.agility = rand.Next(3, 8);
			special.luck = rand.Next(1, 21);
		}
		public Mage(string n, bool g, Statistics stats) : base(n, g, stats)
		{
			CharClass = Proffesion.PROFF_MAGE;
			//add skill
		}
	};

}
