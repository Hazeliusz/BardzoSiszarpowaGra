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

	class Skill
    {
		private string name;
		private int currentCooldown;
		private int cooldownTime;
		public Action<Character, Character> action;
		public Skill(string cName, int cooldown, Action<Character, Character> defAction)
        {
			name = cName;
			currentCooldown = 0;
			cooldownTime = cooldown;
			action = defAction;
		}
		public void setCurrentCooldown(int cooldown)
        {
			currentCooldown = cooldown;
        }
		public int getCurrentCooldown()
        {
			return currentCooldown;
        }
		public void use(Character user, Character target)
        {
			action(user, target);
			currentCooldown = cooldownTime;
        }
    };

	abstract class Character
	{
		protected string name;
		protected Proffesion CharClass;
		protected bool sex; //true - kobieta, false - mezczyzna
		protected Statistics special;
		protected Level lvl;
		protected int hp;
		protected int gold;
		protected List<Skill> skills;
		//Currently unused
		//List<Weapon> weapon_eq;
		//List<Armor> armor_eq;
		//List<Equipment> equipment_eq;

		public Character(string setName, bool setSex, int lvls = 1)
        {
			name = setName;
			sex = setSex;
			special = new Statistics();
			randomizeStatistics();
			hp = special.endurance * 10;
			skills = new List<Skill>();
			skills.Add(new Skill("Atak", 0,(Character user, Character target) => {
				Random rand = new Random();
				int dmg = Convert.ToInt32(user.getStats().strength * 0.5) +
				rand.Next(0, user.getStats().luck);
				target.modifyHP(-dmg);
				Console.WriteLine(user.name + (user.getSex() ? " zaatakowała" : " zaatakował") + " zadając " + dmg + " obrażeń.");
			}));
			skills.Add(new Skill("Atak magiczny", 0, (Character user, Character target) => {
				Random rand = new Random();
				int dmg = Convert.ToInt32(user.getStats().intelligence * 0.5) +
				rand.Next(0, user.getStats().luck);
				target.modifyHP(-dmg);
				Console.WriteLine(user.name + " rzuca czary zadając " + dmg + " obrażeń.");
			}));
			lvl = new Level();
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
		public List<Skill> getSkills()
        {
			return skills;
        }
		public int getHP()
        {
			return hp;
        }
		public void modifyHP(int modifier)
        {
			hp += modifier;
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
		public Knight(string n, bool g) : base(n, g)
		{
			CharClass = Proffesion.PROFF_KNIGHT;
			skills.Add(new Skill("Mocny cios", 5, (Character user, Character target) =>
			{
				Random rand = new Random();
				int dmg = user.getStats().strength + 35 + rand.Next(0, user.getStats().luck);
				target.modifyHP(-dmg);
				Console.WriteLine(user.getName() + " zadaje potężny cios za " + dmg + " obrażeń.");
			}));
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
		public Archer(string n, bool g) : base(n, g)
		{
			CharClass = Proffesion.PROFF_ARCHER;
			skills.Add(new Skill("Strzał z łuku", 2, (Character user, Character target) =>
			{
				Random rand = new Random();
				int dmg = Convert.ToInt32(user.getStats().agility * 1.5) + rand.Next(0, user.getStats().luck);
				target.modifyHP(-dmg);
				Console.WriteLine(target.getName() + (target.getSex() ? " została postrzelona za " : " został postrzelony za ") + dmg + " obrażeń.");
			}));
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
		public Bard(string n, bool g) : base(n, g)
		{
			CharClass = Proffesion.PROFF_BARD;
			skills.Add(new Skill("Pieśń Apolla", 3, (Character user, Character target) =>
			{
				Random rand = new Random();
				int dmg = Convert.ToInt32(target.getStats().strength * 0.5) + rand.Next(0, user.getStats().luck);
				target.modifyHP(-dmg);
				Console.WriteLine(target.getName() + (target.getSex() ? " zaatakowała sama " : " zaatakował sam ") + "siebie za " + dmg + " obrażeń.");
			}));
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
		public DarkKnight(string n, bool g) : base(n, g)
		{
			CharClass = Proffesion.PROFF_DARK_KNIGHT;
			skills.Add(new Skill("Bluźniercze leczenie", 4, (Character user, Character target) =>
			{
				Random rand = new Random();
				int dmg = Convert.ToInt32(target.getStats().intelligence * 0.7) + rand.Next(0, user.getStats().luck);
				target.modifyHP(-dmg);
				user.modifyHP(dmg);
				Console.WriteLine(user.getName() + (user.getSex() ? " ukradła " : " ukradł ") + dmg + " HP przeciwnikowi.");
			}));
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
		public Cleric(string n, bool g) : base(n, g)
		{
			CharClass = Proffesion.PROFF_CLERIC;
			skills.Add(new Skill("Leczenie", 3, (Character user, Character target) =>
			{
				Random rand = new Random();
				int heal = target.getStats().intelligence + rand.Next(0, user.getStats().luck);
				user.modifyHP(heal);
				Console.WriteLine(user.getName() + (user.getSex() ? " uleczyła " : " uleczył ") + "się za " + heal + " HP.");
			}));
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
		public Mage(string n, bool g) : base(n, g)
		{
			CharClass = Proffesion.PROFF_MAGE;
			skills.Add(new Skill("Kula ognia", 4, (Character user, Character target) =>
			{
				Random rand = new Random();
				int dmg = Convert.ToInt32(target.getStats().intelligence * 1.5) + rand.Next(0, user.getStats().luck);
				target.modifyHP(dmg);
				Console.WriteLine(target.getName() + (target.getSex() ? " została trafiona " : " został trafiony ") + " za " + dmg + " HP.");
			}));
		}
	};

}
