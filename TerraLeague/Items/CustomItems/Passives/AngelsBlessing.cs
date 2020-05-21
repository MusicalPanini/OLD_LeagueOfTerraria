using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Items.CustomItems.Passives
{
    public class AngelsBlessing : Passive
    {
        int cooldown;

        public AngelsBlessing()
        {
            cooldown = 300;
        }

        public AngelsBlessing(int Cooldown)
        {
            cooldown = Cooldown;
        }

        public override string Tooltip(Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            return "[c/0099cc:Passive: ANGEL'S BLESSING -] [c/99e6ff:Upon taking fatal damage, heal for 50% of your max life]" +
                 "\n[c/99e6ff:You will gain 'Cursed' for a short period after]" +
                 "\n[c/007399:" + (int)(cooldown * modPlayer.cdrLastStep) + " second cooldown]";
        }

        public override void UpdateAccessory(Player player, ModItem modItem)
        {

            base.UpdateAccessory(player, modItem);
        }

        public override void PostPlayerUpdate(Player player, ModItem modItem)
        {
            AddStat(player, modItem, cooldown * 60, -1, true);
            base.PostPlayerUpdate(player, modItem);
        }

        public override int PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource, Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            if (modPlayer.accessoryStat[TerraLeague.FindAccessorySlotOnPlayer(player, modItem)] <= 0)
            {
                Efx(player);
                if (Main.netMode == NetmodeID.MultiplayerClient)
                    PacketHandler.SendPassiveEfx(-1, player.whoAmI, player.whoAmI, modItem.item.type, FindIfPassiveIsSecondary(modItem));

                player.HealEffect((int)(player.statLifeMax2 * 0.5));
                player.statLife += (int)(player.statLifeMax2 * 0.5);
                player.AddBuff(BuffID.Cursed, 360);
                modPlayer.accessoryStat[TerraLeague.FindAccessorySlotOnPlayer(player, modItem)] = (int)(cooldown * modPlayer.Cdr * 60);

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
