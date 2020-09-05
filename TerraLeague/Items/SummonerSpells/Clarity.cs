using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Items.SummonerSpells
{
    public class ClarityRune : SummonerSpell
    {
        static int effectRadius = 700;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Clarity Rune");
            Tooltip.SetDefault("");
            base.SetStaticDefaults();
        }
        public override string GetIconTexturePath()
        {
            return "Items/SummonerSpells/Clarity";
        }

        public override string GetSpellName()
        {
            return "Clarity";
        }

        public override int GetRawCooldown()
        {
            return 60;
        }
        public override string GetTooltip()
        {
            return "Fully restore yours and all nearby allies mana";
        }

        public override void DoEffect(Player player, int spellSlot)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            // For Server
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                var players = TerraLeague.GetAllPlayersInRange(player.MountedCenter, effectRadius, player.whoAmI, player.team);

                for (int i = 0; i < players.Count; i++)
                {
                    Player target = Main.player[players[i]];
                    modPlayer.SendManaPacket(target.statManaMax2, target.whoAmI, -1, player.whoAmI);
                    PacketHandler.SendClarity(-1, player.whoAmI, target.whoAmI);
                    Efx(target);
                }
            }

            player.ManaEffect((int)(player.statManaMax2));
            player.statMana += player.statManaMax2;
            Efx(player);
            PacketHandler.SendClarity(-1, player.whoAmI, player.whoAmI);

            SetCooldowns(player, spellSlot);
        }

        static public void Efx(Player player)
        {
            Main.PlaySound(new LegacySoundStyle(2, 29), player.Center);
            TerraLeague.DustRing(261, player, new Color(0, 0, 255, 0));
            TerraLeague.DustBorderRing(effectRadius, player.MountedCenter, 261, new Color(0, 0, 255, 0), 2);
        }
    }
}
