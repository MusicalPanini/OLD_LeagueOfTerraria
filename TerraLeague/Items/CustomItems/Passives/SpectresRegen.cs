using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerraLeague.Buffs;
using TerraLeague.Items.AdvItems;
using TerraLeague.Items.CompleteItems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Items.CustomItems.Passives
{
    public class SpectresRegen : Passive
    {
        public override string Tooltip(Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            return TooltipName("Incorporeal") + TerraLeague.CreateColorString(PassiveSecondaryColor, "Gain 'Regeneration' for 4 seconds after taking damage");
        }

        public override void OnHitByNPC(NPC npc, ref int damage, ref bool crit, Player player, ModItem modItem)
        {
                player.AddBuff(BuffID.Regeneration, 240);

            base.OnHitByNPC(npc, ref damage, ref crit, player, modItem);
        }

        public override void OnHitByProjectile(NPC npc, ref int damage, ref bool crit, Player player, ModItem modItem)
        {
                player.AddBuff(BuffID.Regeneration, 240);

            base.OnHitByProjectile(npc, ref damage, ref crit, player, modItem);
        }

        public override void OnHitByProjectile(Projectile proj, ref int damage, ref bool crit, Player player, ModItem modItem)
        {
                player.AddBuff(BuffID.Regeneration, 240);

            base.OnHitByProjectile(proj, ref damage, ref crit, player, modItem);
        }
    }
}
