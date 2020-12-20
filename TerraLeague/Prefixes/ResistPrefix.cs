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
    public class ResistPrefix : ModPrefix
    {
        public override PrefixCategory Category { get { return PrefixCategory.Accessory; } }
        public byte Resist;

        public ResistPrefix()
        {
        }

        public ResistPrefix(byte resist)
        {
            Resist = resist;
        }

        public override float RollChance(Item item)
        {
            switch (Resist)
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
                mod.AddPrefix("Mirrored", new ResistPrefix(1));
                mod.AddPrefix("Absorbing", new ResistPrefix(2));
                mod.AddPrefix("Nullifying", new ResistPrefix(3));
            }
            return false;
        }

        public override void Apply(Item item)
        {
            item.GetGlobalItem<TerraLeaguePrefixGLOBAL>().Resist = Resist;
            if (Resist == 3)
                item.rare += 2;
            else
                item.rare += 1;
        }

    }
}
