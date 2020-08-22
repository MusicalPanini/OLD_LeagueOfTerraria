﻿using Microsoft.Xna.Framework;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CustomItems.Passives
{
    public class SummonedBlade : Passive
    {
        int minionScaling;

        public SummonedBlade(int MinionScaling)
        {
            minionScaling = MinionScaling;
        }

        public override string Tooltip(Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            return "[c/0099cc:Passive: SUMMONED BLADE -] [c/99e6ff:SPELLBLADE summons " + modPlayer.maxMinionsLastStep * 2 + " etheral blades that rapidily orbit around where the enemy was struck dealing] [c/" + TerraLeague.SUMColor + ":" + (int)(modPlayer.SUM * minionScaling / 100d) + "] [c/99e6ff:damage]";
        }

        public override void UpdateAccessory(Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
                    modPlayer.summonedBlade = true;
            base.UpdateAccessory(player, modItem);
        }

        public override void NPCHit(Item item, NPC target, ref int damage, ref float knockback, ref bool crit, ref int OnHitDamage, Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            if (modPlayer.spellBladeBuff)
            {
                if (modPlayer.summonedBlade)
                {
                    int totalSwords = player.maxMinions;
                    for (int i = 0; i < totalSwords; i++)
                    {
                        Projectile.NewProjectile(player.Center, Vector2.Zero, ProjectileType<Item_SummonedSwordA>(), (int)(modPlayer.SUM * minionScaling / 100d), 1, player.whoAmI, ((MathHelper.TwoPi * i) / totalSwords), target.whoAmI);
                        Projectile.NewProjectile(player.Center, Vector2.Zero, ProjectileType<Item_SummonedSwordB>(), (int)(modPlayer.SUM * minionScaling / 100d), 1, player.whoAmI, ((MathHelper.TwoPi * i) / totalSwords), target.whoAmI);
                    }
                    modPlayer.spellBladeBuff = false;
                }
            }
            base.NPCHit(item, target, ref damage, ref knockback, ref crit, ref OnHitDamage, player, modItem);
        }

        public override void PostPlayerUpdate(Player player, ModItem modItem)
        {
            base.PostPlayerUpdate(player, modItem);
        }
    }
}
