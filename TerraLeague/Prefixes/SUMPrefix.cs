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
    public class SUMPrefix : ModPrefix
    {
        public override PrefixCategory Category { get { return PrefixCategory.Accessory; } }
        public byte SUM;

        public SUMPrefix()
        {
        }

        public SUMPrefix(byte sum)
        {
            SUM = sum;
        }

        public override float RollChance(Item item)
        {
            switch (SUM)
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
                mod.AddPrefix("Summoner's", new SUMPrefix(1));
                mod.AddPrefix("Master's", new SUMPrefix(2));
            }
            return false;
        }

        public override void Apply(Item item)
        {
            item.GetGlobalItem<TerraLeaguePrefixGLOBAL>().SUM = SUM;
            if (SUM == 2)
                item.rare += 2;
            else
                item.rare += 1;
        }

    }
}
