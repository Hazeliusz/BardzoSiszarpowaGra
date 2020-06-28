using System;
using System.Collections.Generic;
using System.Text;

namespace BardzoSiszarpowaGra
{
    class Monster : Character
    {
        int monsterLevel; //1 - 9 + 10(boss)
        public override void randomizeStatistics()
        {
            Random rand = new Random();
            special.endurance = Convert.ToInt32(Math.Pow(2, (monsterLevel/2))) + rand.Next(3, 13);
            if (Convert.ToBoolean(rand.Next(0, 1)))
            {
                special.strength = Convert.ToInt32(Math.Pow(2, (monsterLevel / 2))) * 3 + rand.Next(3, 13);
                special.intelligence = 1;
            }

            else
            {
                special.intelligence = Convert.ToInt32(Math.Pow(2, (monsterLevel / 2))) * 3 + rand.Next(3, 13);
                special.strength = 1;
            }
            special.perception = 1;
            special.charisma = 1;
            special.agility = 1;
            special.luck = 1;
        }
        
        public Monster(int setLevel = 1, string setName = "x", bool setSex = true) : base(setName, setSex)
        {
            Random rand = new Random();
            monsterLevel = setLevel;
            sex = Convert.ToBoolean(rand.Next(0, 1));
            switch (monsterLevel)
            {
                case 1:
                    if (sex)
                        name = "Driada";
                    else
                        name = "Slime";
                    break;
                case 2:
                    if (sex)
                        name = "Banshee";
                    else
                        name = "Skrzat";
                    break;
                case 3:
                    if (sex)
                        name = "Drowa";
                    else
                        name = "Kobold";
                    break;
                case 4:
                    if (sex)
                        name = "Walkiria";
                    else
                        name = "Zombie";
                    break;
                case 5:
                    if (sex)
                        name = "Harpia";
                    else
                        name = "Szkielet";
                    break;
                case 6:
                    if (sex)
                        name = "Naga";
                    else
                        name = "Ork";
                    break;
                case 7:
                    if (sex)
                        name = "Gorgona";
                    else
                        name = "Żwyiołak";
                    break;
                case 8:
                    if (sex)
                        name = "Wiedźma";
                    else
                        name = "Cyklop";
                    break;
                case 9:
                    if (sex)
                        name = "Strzyga";
                    else
                        name = "Behemot";
                    break;
                case 10:
                    sex = false;
                    name = "Utrion";
                    break;
                default:
                    Console.WriteLine("Wystąpił błąd przy nazywaniu potwora!");
                    break;
            }
            gold = rand.Next(0, monsterLevel * 100);
        }
    }
}
