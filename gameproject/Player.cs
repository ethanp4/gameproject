using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gameproject
{
    public class Player
    {
        public int Health { get; private set; }
        public int MaxHealth { get; private set; }
        public int MP { get; private set; }
        public int MaxMP { get; private set; }

        public Player(int maxHealth, int maxMP)
        {
            MaxHealth = maxHealth;
            MaxMP = maxMP;
            Health = maxHealth;
            MP = maxMP;
        }

        public void TakeDamage(int damage)
        {
            Health -= damage;
            if (Health < 0) Health = 0; 
        }

        public void Heal(int amount)
        {
            Health += amount;
            if (Health > MaxHealth) Health = MaxHealth; // Cap health at max
        }

        public void UseMP(int amount)
        {
            MP -= amount;
            if (MP < 0) MP = 0; 
        }

        public void RegenerateMP(int amount)
        {
            MP += amount;
            if (MP > MaxMP) MP = MaxMP; // Cap MP at max
        }
    }
}