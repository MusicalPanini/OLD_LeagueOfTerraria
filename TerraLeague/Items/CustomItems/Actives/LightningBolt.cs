using Microsoft.Xna.Framework;
using System;
using TerraLeague.Projectiles;
using Terraria;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CustomItems.Actives
{
    public class LightningBolt : Active
    {
        int baseDamage;
        int magicMinionScaling;
        int cooldown;

        public LightningBolt(int BaseDamage, int MagicMinionScaling, int Cooldown)
        {
            baseDamage = BaseDamage;
            magicMinionScaling = MagicMinionScaling;
            cooldown = Cooldown;
        }

        public override string Tooltip(Player player, LeagueItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            string scaleText;
            if (modPlayer.SUM > modPlayer.MAG)
                scaleText = TerraLeague.CreateScalingTooltip(DamageType.SUM, modPlayer.SUM, magicMinionScaling);
            else
                scaleText = TerraLeague.CreateScalingTooltip(DamageType.MAG, modPlayer.MAG, magicMinionScaling);

            return TooltipName("FROST BOLT") + TerraLeague.CreateColorString(ActiveSecondaryColor, "Target an enemy to deal ") + baseDamage + scaleText + TerraLeague.CreateColorString(ActiveSecondaryColor, " magic damage and apply 'Slowed' to them")
                + "\n" + TerraLeague.CreateColorString(ActiveSubColor, (int)(cooldown * modPlayer.cdrLastStep) + " second cooldown. Damage scales with either MAG or SUM");
        }

        public override void DoActive(Player player, LeagueItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            if (modItem.GetStatOnPlayer(player) <= 0)
            {
                int npc = TerraLeague.NPCMouseIsHovering();
                if (npc != -1)
                {
                    int damage = baseDamage + (int)(Math.Max(modPlayer.SUM, modPlayer.MAG) * magicMinionScaling / 100d);
                    Projectile.NewProjectile(player.Center, Vector2.Zero, ProjectileType<Item_LightningBolt>(), damage, 0, player.whoAmI, npc);
                    modPlayer.FindAndSetActiveStat(this, (int)(cooldown * modPlayer.Cdr * 60));
                }
            }
        }

        public override void PostPlayerUpdate(Player player, LeagueItem modItem)
        {
            AddStat(player, modItem, cooldown * 60, -1, true);
            base.PostPlayerUpdate(player, modItem);
        }
    }
}

