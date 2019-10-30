using Microsoft.Xna.Framework;
using TerraLeague.Buffs;
using Terraria;
using Terraria.Audio;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.SummonerSpells
{
    public class BarrierRune : SummonerSpell
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Barrier Rune");
            Tooltip.SetDefault("");
            base.SetStaticDefaults();
        }
        public override string GetIconTexturePath()
        {
            return "Items/SummonerSpells/Barrier";
        }

        public override string GetSpellName()
        {
            return "Barrier";
        }

        public override int GetRawCooldown()
        {
            return 120;
        }
        public override string GetTooltip()
        {
            return "You gain a shield that protects from " + (int)(200 * Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().healPowerLastStep) + " damage for 10 seconds";
        }

        public override void DoEffect(Player player, int spellSlot)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            player.AddBuff(BuffType<Barrier>(), 600);
            modPlayer.AddShieldAttachedToBuff((int)(200 * modPlayer.healPower), BuffType<Barrier>(), Color.Orange, ShieldType.Basic);

            Efx(player);
            PacketHandler.SendBarrier(-1, player.whoAmI, player.whoAmI);

            SetCooldowns(player, spellSlot);
        }

        static public void Efx(Player player)
        {
            Main.PlaySound(new LegacySoundStyle(2, 29).WithPitchVariance(-0.5f), player.Center);
        }

        static public void ShieldBreak(Player player)
        {

        }
    }
}
