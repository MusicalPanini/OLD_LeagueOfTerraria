using Microsoft.Xna.Framework;
using TerraLeague.Items.CompleteItems;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CustomItems.Passives
{
    public class Cleave : Passive
    {
        int baseMeleeDamage;

        public Cleave(int AdditionMeleeDamage)
        {
            baseMeleeDamage = AdditionMeleeDamage;
        }

        public override string Tooltip(Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>();

            string text = "";

            if (modItem.item.type == ItemType<RavenousHydra>())
                text = "[c/99e6ff:deal] [c/" + TerraLeague.MELColor + ":" + (int)(modPlayer.MEL * baseMeleeDamage / 100d) + "] [c/99e6ff:melee damage to near by enemies]" +
                    "\n[c/99e6ff:Heal for 10% of the damage]";
            else if (modItem.item.type == ItemType<TitanicHydra>())
                text = "[c/99e6ff:deal] [c/" + TerraLeague.MELColor + ":" + (int)(modPlayer.MEL * baseMeleeDamage / 100d) + "] [c/99e6ff:+ 5% of Max Life (" + (int)(Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().maxLifeLastStep * 0.05) + ") melee damage to near by enemies]";
            else
                text = "[c/99e6ff:deal] [c/" + TerraLeague.MELColor + ":" + (int)(modPlayer.MEL * baseMeleeDamage / 100d) + "] [c/99e6ff:melee damage to near by enemies]";

            return "[c/0099cc:Passive: CLEAVE -] [c/99e6ff:Your melee attacks will periodically] " + text;
        }

        public override void UpdateAccessory(Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            if (modItem.item.type == ItemType<RavenousHydra>())
            {
                modPlayer.ravenous = true;
                if (modPlayer.cleaveCooldown != 0)
                    modPlayer.cleaveCooldown--;
            }
            else if (modItem.item.type == ItemType<TitanicHydra>())
            {
                modPlayer.titanic = true;
                if (modPlayer.cleaveCooldown != 0)
                    modPlayer.cleaveCooldown--;
            }
            else
            {
                modPlayer.tiamat = true;
                if (modPlayer.cleaveCooldown != 0)
                    modPlayer.cleaveCooldown--;
            }

            base.UpdateAccessory(player, modItem);
        }

        static public void Efx(int user, int type)
        {
            Player player = Main.player[user];

            if (type == 1)
            {
                Main.PlaySound(new LegacySoundStyle(2, 71).WithPitchVariance(-0.2f), player.Center);
                TerraLeague.DustRing(261, player, new Color(255, 170, 0, 0));
                TerraLeague.DustBorderRing(200, player.MountedCenter, 261, new Color(255, 170, 0, 0), 2);
            }
            else if (type == 2)
            {
                Main.PlaySound(new LegacySoundStyle(2, 71).WithPitchVariance(-0.2f), player.Center);
                TerraLeague.DustRing(261, player, new Color(255, 170, 0, 0));
                TerraLeague.DustBorderRing(200, player.MountedCenter, 261, new Color(255, 170, 0, 0), 2);
            }
            else
            {
                Main.PlaySound(new LegacySoundStyle(2, 71).WithPitchVariance(-0.2f), player.Center);
                TerraLeague.DustRing(261, player, new Color(255, 255, 0, 0));
                TerraLeague.DustBorderRing(200, player.MountedCenter, 261, new Color(255, 255, 0, 0), 2);
            }

        }
    }
}
