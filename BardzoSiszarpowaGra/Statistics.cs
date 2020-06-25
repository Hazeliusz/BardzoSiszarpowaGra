using System;
using System.Collections.Generic;
using System.Text;

namespace BardzoSiszarpowaGra
{
    class Statistics
    {
		public int strength;
		public int perception;
		public int endurance;
		public int charisma;
		public int intelligence;
		public int agility;
		public int luck;

		public Statistics(int s = 0, int p = 0, int e = 0, int c = 0, int i = 0, int a = 0, int l = 0)
		{
			strength = s;
			perception = p;
			endurance = e;
			charisma = c;
			intelligence = i;
			agility = a;
			luck = l;
		}

		public int getByChar(char stat)
		{
			switch (stat)
			{
				case 's':
					return strength;
				case 'p':
					return perception;
				case 'e':
					return endurance;
				case 'c':
					return charisma;
				case 'i':
					return intelligence;
				case 'a':
					return agility;
				case 'l':
					return luck;
				default:
					return -1;
			}
		}
		public void advanceStatsByChar(char stat, int number)
		{
			switch (stat)
			{
				case 's':
					strength += number;
					break;
				case 'p':
					perception += number;
					break;
				case 'e':
					endurance += number;
					break;
				case 'c':
					charisma += number;
					break;
				case 'i':
					intelligence += number;
					break;
				case 'a':
					agility += number;
					break;
				case 'l':
					luck += number;
					break;
			}
		}
	}
}
