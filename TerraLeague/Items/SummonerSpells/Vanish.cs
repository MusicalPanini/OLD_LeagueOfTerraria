using Microsoft.Xna.Framework;
using TerraLeague.Buffs;
using Terraria;
using Terraria.Audio;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.SummonerSpells
{
    public class VanishRune : SummonerSpell
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Vanish Rune");
            Tooltip.SetDefault("");
            base.SetStaticDefaults();
        }

        public override string GetIconTexturePath()
        {
            return "Items/SummonerSpells/Vanish";
        }

        public override string GetSpellName()
        {
            return "Vanish";
        }

        public override int GetRawCooldown()
        {
            return 210;
        }

        public override string GetTooltip()
        {
            return "Become invisible and immune to all projectile damage for 5 seconds";
        }

        public override void DoEffect(Player player, int spellSlot)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            modPlayer.player.AddBuff(BuffType<Vanished>(), 300);
            Efx(player);
            PacketHandler.SendVanish(-1, player.whoAmI, player.whoAmI);

            SetCooldowns(player, spellSlot);
        }

        static public void Efx(Player player)
        {
            Main.PlaySound(new LegacySoundStyle(2, 11).WithPitchVariance(-1), player.Center);
            for (int i = 0; i < 9; i++)
            {
                Gore gore = Gore.NewGoreDirect(new Vector2(player.position.X + (float)(player.width / 2) - 24f, player.position.Y + (float)(player.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1.5f);
                gore.velocity.Y = gore.velocity.Y + 1.5f;
            }
        }
    }
}
