using Microsoft.Xna.Framework;
using System;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CustomItems.Actives
{
    public class FrostBolt : Active
    {
        int baseDamage;
        int magicMinionScaling;
        int cooldown;

        public FrostBolt(int BaseDamage, int MagicMinionScaling, int Cooldown)
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
                scaleText = "[c/" + TerraLeague.SUMColor + ":" + (int)(modPlayer.SUM * magicMinionScaling / 100d) + "]";
            else
                scaleText = "[c/" + TerraLeague.MAGColor + ":" + (int)(modPlayer.MAG * magicMinionScaling / 100d) + "]";

            return "[c/ff4d4d:Active: FROST BOLT -] [c/ff8080:Fire 5 frost projectiles in a cone that deal] " + baseDamage + " + " + scaleText + " [c/ff4d4d:magic damage]" +
                "\n[c/cc0000:" + (int)(cooldown * modPlayer.cdrLastStep) + " second cooldown. Damage scales with either MAG or SUM]";
        }

        public override void DoActive(Player player, LeagueItem modItem)
        {
            if (modItem.GetStatOnPlayer(player) <= 0)
            {
                PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
                Vector2 position = player.Center;
                Vector2 velocity = TerraLeague.CalcVelocityToMouse(position, 20f);
                int projType = ProjectileType<TrueIceVolley>();
                int damage = baseDamage + (int)(Math.Max(modPlayer.SUM, modPlayer.MAG) * magicMinionScaling / 100d);
                int knockback = 1;
                int numberProjectiles = 5;
                float startingAngle = 16;
                for (int i = 0; i < numberProjectiles; i++)
                {
                    Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.ToRadians(startingAngle));
                    Projectile proj = Projectile.NewProjectileDirect(position, perturbedSpeed, projType, damage, knockback, player.whoAmI);
                    proj.ranged = false;
                    proj.magic = true;
                    proj.scale = 1.4f;
                    startingAngle -= 8f;
                }

                Efx(player);
                if (Main.netMode == NetmodeID.MultiplayerClient)
                    PacketHandler.SendActiveEfx(-1, player.whoAmI, player.whoAmI, modItem.item.type);

                modPlayer.FindAndSetActiveStat(this, (int)(cooldown * modPlayer.Cdr * 60));
            }
        }

        public override void PostPlayerUpdate(Player player, LeagueItem modItem)
        {
            AddStat(player, modItem, cooldown * 60, -1, true);
            base.PostPlayerUpdate(player, modItem);
        }

        public override void Efx(Player user)
        {
            Main.PlaySound(new LegacySoundStyle(2, 11), user.Center);
        }
    }
}

