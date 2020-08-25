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
            return "Give all players vision of treasure, traps, and NPC's for 5 seconds";
        }

        public override void DoEffect(Player player, int spellSlot)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            Efx(player);
            player.AddBuff(BuffType<Buffs.Clairvoyance>(), 300);

            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                for (int i = 0; i < Main.player.Length; i++)
                {
                    Player target = Main.player[i];
                    if (target.active)
                    {
                        Efx(target);
                        PacketHandler.SendClairvoyance(-1, player.whoAmI, i);
                        modPlayer.SendBuffPacket(BuffType<Buffs.Clairvoyance>(), 300, i, -1, player.whoAmI);
                    }
                }
            }

            SetCooldowns(player, spellSlot);
        }

        static public void Efx(Player player)
        {
            Main.PlaySound(new LegacySoundStyle(2, 29), player.Center);

            TerraLeague.DustRing(261, player, new Color(0, 0, 255, 0));
        }
    }
}
