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
            return "Fully restore yours and all near by players mana";
        }

        public override void DoEffect(Player player, int spellSlot)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                for (int k = 0; k < 200; k++)
                {
                    if (Main.player[k].active)
                    {
                        if (player.Distance(Main.player[k].Center) < 700 && k != player.whoAmI)
                        {
                            modPlayer.SendManaPacket(Main.player[k].statManaMax2, k, -1, player.whoAmI);
                            PacketHandler.SendClarity(-1, player.whoAmI, k);
                            Efx(Main.player[k]);
                        }
                    }
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

            for (int j = 0; j < 18; j++)
            {
                Dust dust = Dust.NewDustDirect(new Vector2(Main.rand.Next((int)player.position.X - 8, (int)player.position.X + 8), player.position.Y + 16), player.width, player.height, 261, 0, -Main.rand.Next(6, 18), 0, new Color(0, 0, 255, 0), Main.rand.Next(Main.rand.Next(2, 3)));
                dust.noGravity = true;
            }
        }
    }
}
