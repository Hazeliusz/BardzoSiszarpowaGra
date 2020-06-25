using System;
using System.Runtime.CompilerServices;

namespace BardzoSiszarpowaGra
{
    class Program
    {
        static void Main(string[] args)
        {
            Mage player = new Mage("Hazel", false);
            Bard wrog = new Bard("Worg", true);
            Console.WriteLine(wrog.getHP());
            player.getSkills()[2].use(player, wrog);
            Console.WriteLine(wrog.getHP());
        }
    }
}
