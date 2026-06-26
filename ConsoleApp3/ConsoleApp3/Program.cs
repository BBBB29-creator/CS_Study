using System.Runtime.InteropServices;

namespace ConsoleApp3
{
    class Program
    {
        static void Main(string[] args)
        {
         
        }
    }

    class player
    {
        string name;
        int id;
        int hp;
        int maxHp;
        float damage;

        void TakeDamage(float damage)
        {
            hp -= (int)Math.Max(0, hp - damage);
        }
    }
}
