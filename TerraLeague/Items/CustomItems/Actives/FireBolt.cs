using Microsoft.Xna.Framework;
using System;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.Audio;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CustomItems.Actives
{
    public class FireBolt : Active
    {
        int baseDamage;
        int magicMinionScaling;
        int cooldown;

        public FireBolt(int BaseDamage, int MagicMinionScaling, int Cooldown)
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

            return "[c/ff4d4d:Active: FIRE BOLT -] [c/ff8080:Launch yourself towards the curser, while firing 7 bolts in a cone and 1 from behined dealing] " + baseDamage + " + " + scaleText + " [c/ff8080:magic damage]" +
                "\n[c/cc0000:" + (int)(cooldown * modPlayer.cdrLastStep) + " second cooldown. Damage scales with either MAG or SUM]";
        }

        public override void DoActive(Player player, LeagueItem modItem)
        {
            if (modItem.GetStatOnPlayer(player) <= 0)
            {
                PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

                player.velocity = TerraLeague.CalcVelocityToMouse(player.Center, 18f);

                Vector2 position = player.Center;
                Vector2 velocity = TerraLeague.CalcVelocityToMouse(position, 20f);
                int projType = ProjectileType<ProtobeltShot>();
                int damage = baseDamage + (int)(Math.Max(modPlayer.SUM, modPlayer.MAG) * magicMinionScaling / 100d);
                int knockback = 1;
                int numberProjectiles = 7;
                float startingAngle = 27;
                for (int i = 0; i < numberProjectiles; i++)
                {
                    Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.ToRadians(startingAngle));
                    Projectile.NewProjectileDirect(position, perturbedSpeed, projType, damage, knockback, player.whoAmI);
                    startingAngle -= 9f;
                }
                Projectile.NewProjectileDirect(position, -velocity, projType, damage, knockback, player.whoAmI);


                Efx(player);
                if (Main.netMode == 1)
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

