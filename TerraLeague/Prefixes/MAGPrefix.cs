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
    public class MAGPrefix : ModPrefix
    {
        public override PrefixCategory Category { get { return PrefixCategory.Accessory; } }
        public byte MAG;

        public MAGPrefix()
        {
        }

        public MAGPrefix(byte mag)
        {
            MAG = mag;
        }

        public override float RollChance(Item item)
        {
            switch (MAG)
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
                mod.AddPrefix("Mage's", new MAGPrefix(1));
                mod.AddPrefix("Sorcerer's", new MAGPrefix(2));
            }
            return false;
        }

        public override void Apply(Item item)
        {
            item.GetGlobalItem<TerraLeaguePrefixGLOBAL>().MAG = MAG;
            if (MAG == 2)
                item.rare += 2;
            else
                item.rare += 1;
        }

    }
}
