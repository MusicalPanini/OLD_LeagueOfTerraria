﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerraLeague.Items;
using Terraria;
using Terraria.ModLoader;

namespace TerraLeague.Prefixes
{
    public class RNGPrefix : ModPrefix
    {
        public override PrefixCategory Category { get { return PrefixCategory.Accessory; } }
        public byte RNG;

        public RNGPrefix()
        {
        }

        public RNGPrefix(byte rng)
        {
            RNG = rng;
        }

        public override float RollChance(Item item)
        {
            switch (RNG)
            {
                case 1:
                    return 1;
                case 2:
                    return 1;
                default:
                    break;
            }

            return base.RollChance(item);
        }

        public override bool Autoload(ref string name)
        {
            if (base.Autoload(ref name))
            {
                mod.AddPrefix("Ranger's", new RNGPrefix(1));
                mod.AddPrefix("Marksmen's", new RNGPrefix(2));
            }
            return false;
        }

        public override void Apply(Item item)
        {
            item.GetGlobalItem<ITEMGLOBAL>().RNG = RNG;
            if (RNG == 2)
                item.rare += 2;
            else
                item.rare += 1;
        }

    }
}
