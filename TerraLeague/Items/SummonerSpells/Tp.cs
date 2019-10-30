using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Items.SummonerSpells
{
    public class TeleportRune : SummonerSpell
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Teleport Rune");
            Tooltip.SetDefault("");
            base.SetStaticDefaults();
        }
        public override string GetIconTexturePath()
        {
            return "Items/SummonerSpells/Tp";
        }

        public override string GetSpellName()
        {
            return "Teleport";
        }

        public override int GetRawCooldown()
        {
            return 180;
        }

        public override string GetTooltip()
        {
            return "Teleport to a random location";
        }

        public override void DoEffect(Player player, int spellSlot)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            player.TeleportationPotion();

            SetCooldowns(player, spellSlot);
        }
    }
}
