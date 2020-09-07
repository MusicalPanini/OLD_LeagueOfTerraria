using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerraLeague.Buffs;
using TerraLeague.Items.AdvItems;
using TerraLeague.Items.CompleteItems;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.ModLoader;

namespace TerraLeague.Items.CustomItems.Passives
{
    public class WindsFury : Passive
    {
        bool CloneProj;

        public WindsFury(bool cloneProj)
        {
            CloneProj = cloneProj;
        }

        public override string Tooltip(Player player, ModItem modItem)
        {
            return TooltipName("WIND'S FURY") + TerraLeague.CreateColorString(PassiveSecondaryColor, "Hitting a ranged attack will fire 2 projectiles at near by enemies\nThe projectiles deal 40% of the original damage");
        }

        public override void UpdateAccessory(Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            if (CloneProj)
                modPlayer.windFuryReplicator = true;
            else
                modPlayer.windsFury = true;

            base.UpdateAccessory(player, modItem);
        }

        public override void PostPlayerUpdate(Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            if (modPlayer.windsFuryCooldown != 0)
                modPlayer.windsFuryCooldown--;

            base.PostPlayerUpdate(player, modItem);
        }

        public override void NPCHit(Item item, NPC target, ref int damage, ref float knockback, ref bool crit, ref int OnHitDamage, Player player, ModItem modItem)
        {
            base.NPCHit(item, target, ref damage, ref knockback, ref crit, ref OnHitDamage, player, modItem);
        }

        public override void NPCHitWithProjectile(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection, ref int OnHitDamage, Player player, ModItem modItem)
        {
            base.NPCHitWithProjectile(proj, target, ref damage, ref knockback, ref crit, ref hitDirection, ref OnHitDamage, player, modItem);
        }
    }
}
