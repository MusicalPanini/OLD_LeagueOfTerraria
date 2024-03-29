﻿using Microsoft.Xna.Framework;
using TerraLeague.Buffs;
using Terraria;
using Terraria.Audio;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.SummonerSpells
{
    public class CleanseRune : SummonerSpell
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cleanse Rune");
            Tooltip.SetDefault("");
            base.SetStaticDefaults();
        }
        public override string GetIconTexturePath()
        {
            return "Items/SummonerSpells/Cleanse";
        }

        public override string GetSpellName()
        {
            return "Cleanse";
        }

        public override int GetRawCooldown()
        {
            return 90;
        }
        public override string GetTooltip()
        {
            return "Become immune to all debuffs for 10 seconds" +
                "\nYou gain 'Swiftness'";
        }

        public override void DoEffect(Player player, int spellSlot)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            player.AddBuff(BuffType<Cleanse>(), 600);
            Efx(player);
            PacketHandler.SendCleanse(-1, player.whoAmI, player.whoAmI);

            SetCooldowns(player, spellSlot);
        }

        static public void Efx(Player player)
        {
            Main.PlaySound(new LegacySoundStyle(2, 29).WithPitchVariance(-0.3f), player.Center);
            for (int j = 0; j < 18; j++)
            {
                int num2 = Dust.NewDust(new Vector2(Main.rand.Next((int)player.position.X - 8, (int)player.position.X + 8), player.position.Y + 16), player.width, player.height, 261, 0, -Main.rand.Next(6, 18), 0, new Color(0, 255, 255, 0), Main.rand.Next(Main.rand.Next(2, 3)));
                Main.dust[num2].noGravity = true;
            }
        }
    }
}
