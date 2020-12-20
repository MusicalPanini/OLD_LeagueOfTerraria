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
    public class HealPrefix : ModPrefix
    {
        public override PrefixCategory Category { get { return PrefixCategory.Accessory; } }
        public byte Heal;

        public HealPrefix()
        {
        }

        public HealPrefix(byte heal)
        {
            Heal = heal;
        }
        
        public override float RollChance(Item item)
        {
            switch (Heal)
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
                mod.AddPrefix("Calming", new HealPrefix(1));
                mod.AddPrefix("Medicinal", new HealPrefix(2));
                mod.AddPrefix("Regenerative", new HealPrefix(3));
            }
            return false;
        }

        public override void Apply(Item item)
        {
            item.GetGlobalItem<TerraLeaguePrefixGLOBAL>().HealPower = Heal;
            if (Heal == 3)
                item.rare += 2;
            else
                item.rare += 1;
        }

    }
}
