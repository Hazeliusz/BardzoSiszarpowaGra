using Microsoft.VisualBasic;
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
            CharactersInteractions interact = new CharactersInteractions(player, wrog);
            interact.MonsterFight();
        }
    }
}
