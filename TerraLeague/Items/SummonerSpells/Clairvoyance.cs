using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.SummonerSpells
{
    public class ClairvoyanceRune : SummonerSpell
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Clairvoyance Rune");
            Tooltip.SetDefault("");
            base.SetStaticDefaults();
        }
        public override string GetIconTexturePath()
        {
            return "Items/SummonerSpells/Clairvoyance";
        }

        public override string GetSpellName()
        {
            return "Clairvoyance";
        }

        public override int GetRawCooldown()
        {
            return 60;
        }
        public override string GetTooltip()
        {
            return "Give all allies vision of treasure, traps, and NPC's for 5 seconds";
        }

        public override void DoEffect(Player player, int spellSlot)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            Efx(player);
            PacketHandler.SendClairvoyance(-1, player.whoAmI, player.whoAmI);
            player.AddBuff(BuffType<Buffs.Clairvoyance>(), 300);

            // For Server
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                var players = TerraLeague.GetAllPlayersInRange(player.MountedCenter, 999999, player.whoAmI, player.team);

                for (int i = 0; i < players.Count; i++)
                {
                    Player target = Main.player[players[i]];
                    Efx(target);
                    PacketHandler.SendClairvoyance(-1, player.whoAmI, target.whoAmI);
                    modPlayer.SendBuffPacket(BuffType<Buffs.Clairvoyance>(), 300, target.whoAmI, -1, player.whoAmI);
                }
            }

            SetCooldowns(player, spellSlot);
        }

        static public void Efx(Player player)
        {
            Main.PlaySound(new LegacySoundStyle(2, 29), Main.LocalPlayer.MountedCenter);

            TerraLeague.DustElipce(1.5f, 0.66f, 0, player.MountedCenter, 113, new Color(0, 0, 255), 1.5f, 180, true, 10);
            TerraLeague.DustElipce(0.76f, 0.66f, 0, player.MountedCenter, 113, new Color(0, 0, 255), 1.5f, 180, true, 10);
            TerraLeague.DustElipce(0, 0.45f, 0, player.MountedCenter, 113, new Color(0, 0, 255), 1.5f, 180, true, 10);


            //TerraLeague.DustRing(261, player, new Color(0, 0, 255, 0));
        }
    }
}
