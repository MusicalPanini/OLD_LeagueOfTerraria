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

        public FrostBolt(int BaseDamage, int MagicMinionScaling, int Cooldown)
        {
            baseDamage = BaseDamage;
            magicMinionScaling = MagicMinionScaling;
            activeCooldown = Cooldown;
        }

        public override string Tooltip(Player player, LeagueItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            string scaleText;
            if (modPlayer.SUM > modPlayer.MAG)
                scaleText = TerraLeague.CreateScalingTooltip(DamageType.SUM, modPlayer.SUM, magicMinionScaling);
            else
                scaleText = TerraLeague.CreateScalingTooltip(DamageType.MAG, modPlayer.MAG, magicMinionScaling);

            return TooltipName("FROST BOLT") + TerraLeague.CreateColorString(ActiveSecondaryColor, "Fire 10 frost projectiles in a cone that deal ") + baseDamage + " + " + scaleText + TerraLeague.CreateColorString(ActiveSecondaryColor, " magic damage and apply 'Slowed'")
                + "\n" + TerraLeague.CreateColorString(ActiveSubColor, GetScaledCooldown(player) + " second cooldown. Damage scales with either MAG or SUM");
        }

        public override void DoActive(Player player, LeagueItem modItem)
        {
            if (cooldownCount <= 0)
            {
                PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
                Vector2 position = player.Center;
                Vector2 velocity = TerraLeague.CalcVelocityToMouse(position, 14f);
                int projType = ProjectileType<Item_FrostBolt>();
                int damage = baseDamage + (int)(Math.Max(modPlayer.SUM, modPlayer.MAG) * magicMinionScaling / 100d);
                int knockback = 1;
                int numberProjectiles = 10;
                float startingAngle = 20;
                for (int i = 0; i < numberProjectiles; i++)
                {
                    Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.ToRadians(startingAngle));
                    Projectile proj = Projectile.NewProjectileDirect(position, perturbedSpeed, projType, damage, knockback, player.whoAmI);
                    startingAngle -= 4f;
                }

                Efx(player);
                if (Main.netMode == NetmodeID.MultiplayerClient)
                    PacketHandler.SendActiveEfx(-1, player.whoAmI, player.whoAmI, modItem.item.type);

                SetCooldown(player);
            }
        }

        public override void PostPlayerUpdate(Player player, LeagueItem modItem)
        {
            base.PostPlayerUpdate(player, modItem);
        }

        public override void Efx(Player user)
        {
            Main.PlaySound(new LegacySoundStyle(2, 11), user.Center);
            TerraLeague.PlaySoundWithPitch(user.MountedCenter, 2, 28, -0f);
        }
    }
}

