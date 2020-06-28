using Microsoft.VisualBasic;
using System;
using System.Runtime.CompilerServices;

namespace BardzoSiszarpowaGra
{
    class Program
    {
        static void Main(string[] args)
        {
            Archer player = new Archer("Hazel", false);
            player.getStats().agility = 100;
            Map mapa = new Map(player);
            while (true)
            {
                mapa.draw();
                mapa.move();
            }
        }
    }
}
