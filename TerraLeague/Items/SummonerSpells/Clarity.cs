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
                    PacketHandler.SendClarity(-1, player.whoAmI, target.whoAmI, false);
                    Efx(target, false);
                }
            }

            player.ManaEffect((int)(player.statManaMax2));
            player.statMana += player.statManaMax2;
            Efx(player, true);
            PacketHandler.SendClarity(-1, player.whoAmI, player.whoAmI, true);

            SetCooldowns(player, spellSlot);
        }

        static public void Efx(Player player, bool drawRing)
        {
            Main.PlaySound(new LegacySoundStyle(2, 29), player.Center);

            float starRad = 96 * 1.5f;

            Vector2 PointA = player.MountedCenter + new Vector2(0, -starRad);
            Vector2 PointB = player.MountedCenter + new Vector2(0, -starRad).RotatedBy(MathHelper.Pi * 2 /5f);
            Vector2 PointC = player.MountedCenter + new Vector2(0, -starRad).RotatedBy(MathHelper.Pi * 4 / 5f);
            Vector2 PointD = player.MountedCenter + new Vector2(0, -starRad).RotatedBy(MathHelper.Pi * 6 / 5f);
            Vector2 PointE = player.MountedCenter + new Vector2(0, -starRad).RotatedBy(MathHelper.Pi * 8 / 5f);

            TerraLeague.DustLine(PointA, PointC, 113, 1, 2f, default, true, 0, 0);
            TerraLeague.DustLine(PointC, PointE, 113, 1, 2f, default, true, 0, 0);
            TerraLeague.DustLine(PointE, PointB, 113, 1, 2f, default, true, 0, 0);
            TerraLeague.DustLine(PointB, PointD, 113, 1, 2f, default, true, 0, 0);
            TerraLeague.DustLine(PointD, PointA, 113, 1, 2f, default, true, 0, 0);

            if (drawRing)
            {
                TerraLeague.DustRing(261, player, new Color(0, 0, 255, 0));
                TerraLeague.DustBorderRing(effectRadius, player.MountedCenter, 261, new Color(0, 0, 255, 0), 2);
            }
        }
    }
}
