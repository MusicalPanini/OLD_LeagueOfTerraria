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
    public class MELPrefix : ModPrefix
    {
        public override PrefixCategory Category { get { return PrefixCategory.Accessory; } }
        public byte MEL;

        public MELPrefix()
        {
        }

        public MELPrefix(byte mel)
        {
            MEL = mel;
        }

        public override float RollChance(Item item)
        {
            switch (MEL)
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
                mod.AddPrefix("Bruser's", new MELPrefix(1));
                mod.AddPrefix("Slayer's", new MELPrefix(2));
            }
            return false;
        }

        public override void Apply(Item item)
        {
            item.GetGlobalItem<TerraLeaguePrefixGLOBAL>().MEL = MEL;
            if (MEL == 2)
                item.rare += 2;
            else
                item.rare += 1;
        }

    }
}
