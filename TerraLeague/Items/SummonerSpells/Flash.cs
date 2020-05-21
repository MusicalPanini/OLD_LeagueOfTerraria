using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Items.SummonerSpells
{
    public class FlashRune : SummonerSpell
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Flash Rune");
            Tooltip.SetDefault("");
            base.SetStaticDefaults();
        }
        public override string GetIconTexturePath()
        {
            return "Items/SummonerSpells/Flash";
        }

        public override string GetSpellName()
        {
            return "Flash";
        }

        public override int GetRawCooldown()
        {
            return 90;
        }
        public override string GetTooltip()
        {
            return "Blink to your cursor";
        }

        public override void DoEffect(Player player, int spellSlot)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            bool pathBlocked = false;
            for (int x = (int)((Main.mouseX + Main.screenPosition.X) / 16) - 1; x < (int)((Main.mouseX + Main.screenPosition.X) / 16) + 1; x++)
            {
                for (int y = (int)((Main.mouseY + Main.screenPosition.Y) / 16) - 1; y <= (int)((Main.mouseY + Main.screenPosition.Y) / 16) + 1; y++)
                {
                    if (Main.tile[x, y].collisionType > 0 || Main.tile[x, y].wall == WallID.LihzahrdBrickUnsafe && !NPC.downedPlantBoss)
                    {
                        pathBlocked = true;
                        break;
                    }
                }
            }
            if (!pathBlocked)
            {
                player.Teleport(new Vector2((int)(Main.mouseX + Main.screenPosition.X - 16), (int)(Main.mouseY + Main.screenPosition.Y - 24)), 1, 0);
                NetMessage.SendData(MessageID.Teleport, -1, -1, null, 0, player.whoAmI, (int)(Main.mouseX + Main.screenPosition.X), (int)(Main.mouseY + Main.screenPosition.Y), 1, 0, 0);

                SetCooldowns(player, spellSlot);
            }
        }
    }
}
