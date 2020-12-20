using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerraLeague.Items;
using Terraria;
using Terraria.ModLoader;

namespace TerraLeague.Prefixes
{
    public class ArmorPrefix : ModPrefix
    {
        public override PrefixCategory Category { get { return PrefixCategory.Accessory; } }
        public byte Armor;

        public ArmorPrefix()
        {
        }

        public ArmorPrefix(byte armor)
        {
            Armor = armor;
        }

        public override float RollChance(Item item)
        {
            switch (Armor)
            {
                case 1:
                    return 1;
                case 2:
                    return 1;
                case 3:
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
                mod.AddPrefix("Padded", new ArmorPrefix(1));
                mod.AddPrefix("Plated", new ArmorPrefix(2));
                mod.AddPrefix("Reinforced", new ArmorPrefix(3));
            }
            return false;
        }

        public override void Apply(Item item)
        {
            item.GetGlobalItem<TerraLeaguePrefixGLOBAL>().Armor = Armor;
            if (Armor == 3)
                item.rare += 2;
            else
                item.rare += 1;
        }

    }
}
