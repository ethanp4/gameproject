using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gameproject
{
    public class Skill
    {
        public string Name { get; private set; }
        public int MPCost { get; private set; }
        public Action Effect { get; private set; }

        public Skill(string name, int mpCost, Action effect)
        {
            Name = name;
            MPCost = mpCost;
            Effect = effect;
        }

        public void Use()
        {
            Effect?.Invoke();
        }

    }
}