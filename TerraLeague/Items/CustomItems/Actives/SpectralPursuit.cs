using Microsoft.Xna.Framework;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CustomItems.Actives
{
    public class SpectralPursuit : Active
    {
        int baseDamage;
        int minionScaling;

        public SpectralPursuit(int BaseDamage, int MinionScaling, int Cooldown)
        {
            baseDamage = BaseDamage;
            minionScaling = MinionScaling;
            activeCooldown = Cooldown;
        }

        public override string Tooltip(Player player, LeagueItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            return TooltipName("SPECTURAL PURSUIT") + TerraLeague.CreateColorString(ActiveSecondaryColor, "Send out ") + TerraLeague.CreateScalingTooltip(TerraLeague.MINIONMAXColor, "MINIONS", (int)modPlayer.maxMinionsLastStep, 100) + TerraLeague.CreateColorString(ActiveSecondaryColor, " spooky ghosts that track down a nearby enemy" +
                "\nThey deal ") + TerraLeague.CreateScalingTooltip(DamageType.NONE, baseDamage, 100) + " + " + TerraLeague.CreateScalingTooltip(DamageType.SUM, modPlayer.SUM, minionScaling) + TerraLeague.CreateColorString(ActiveSecondaryColor, " minion damage and apply 'Slowed'") +
                 "\n" + TerraLeague.CreateColorString(ActiveSubColor, GetScaledCooldown(player) + " second cooldown");
        }

        public override void DoActive(Player player, LeagueItem modItem)
        {
            if (cooldownCount <= 0)
            {
                PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

                Efx(player);
                SetCooldown(player);

                if (player.whoAmI == Main.myPlayer)
                {
                    for (int i = 0; i < modPlayer.maxMinionsLastStep; i++)
                    {
                        float hSpeed = 5;
                        if (modPlayer.maxMinionsLastStep > 1)
                            hSpeed = 10 * ((i) / (float)(modPlayer.maxMinionsLastStep - 1));


                        Projectile.NewProjectile(player.position.X, player.position.Y, hSpeed-5, -4, ProjectileType<Item_SpookyGhost>(), baseDamage + (int)(modPlayer.SUM * minionScaling / 100d), 0, player.whoAmI);
                    }
                }

                // For Server
                if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    PacketHandler.SendActiveEfx(-1, player.whoAmI, player.whoAmI, modItem.item.type);
                }
            }
        }

        public override void PostPlayerUpdate(Player player, LeagueItem modItem)
        {
            base.PostPlayerUpdate(player, modItem);
        }

        override public void Efx(Player user)
        {
            TerraLeague.DustRing(261, user, new Color(0, 255, 255, 0));
            Main.PlaySound(new LegacySoundStyle(3, 54).WithPitchVariance(-0.2f), user.position);
        }
    }
}

