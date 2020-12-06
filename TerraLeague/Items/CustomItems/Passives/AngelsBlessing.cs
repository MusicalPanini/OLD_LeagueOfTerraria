using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Items.CustomItems.Passives
{
    public class AngelsBlessing : Passive
    {
        public AngelsBlessing()
        {
            passiveCooldown = 300;
        }

        public AngelsBlessing(int Cooldown)
        {
            passiveCooldown = Cooldown;
        }

        public override string Tooltip(Player player, ModItem modItem)
        {
            return TooltipName("REBIRTH") + TerraLeague.CreateColorString(PassiveSecondaryColor, "Upon taking fatal damage, heal for 50% of your max life\nYou will gain 'Cursed' for a short period after")
                + "\n" + TerraLeague.CreateColorString(PassiveSubColor, GetScaledCooldown(player) + " second cooldown");
        }

        public override int PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource, Player player, ModItem modItem)
        {
            if (cooldownCount <= 0)
            {
                Efx(player);
                SendEfx(player, modItem);

                player.HealEffect((int)(player.statLifeMax2 * 0.5));
                player.statLife += (int)(player.statLifeMax2 * 0.5);
                player.AddBuff(BuffID.Cursed, 360);
                SetCooldown(player);

                return 0;
            }
            return -1;
        }

        public override void Efx(Player user)
        {
            Main.PlaySound(new LegacySoundStyle(2, 29).WithPitchVariance(-0.5f), user.position);
            TerraLeague.DustRing(43, user, new Microsoft.Xna.Framework.Color(255, 255, 255));
        }
    }
}
