using TerraLeague.Buffs;
using Terraria;
using Terraria.Audio;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.SummonerSpells
{
    public class GhostRune : SummonerSpell
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ghost Rune");
            Tooltip.SetDefault("");
            base.SetStaticDefaults();
        }
        public override string GetIconTexturePath()
        {
            return "Items/SummonerSpells/Ghost";
        }

        public override string GetSpellName()
        {
            return "Ghost";
        }

        public override int GetRawCooldown()
        {
            return 60;
        }
        public override string GetTooltip()
        {
            return "Gain movement speed and knockback immunity for 10 seconds";
        }

        public override void DoEffect(Player player, int spellSlot)
        {
            player.AddBuff(BuffType<Ghost>(), 600);
            Efx(player);
            PacketHandler.SendGhost(-1, player.whoAmI, player.whoAmI);
            
            SetCooldowns(player, spellSlot);
        }

        static public void Efx(Player player)
        {
            Main.PlaySound(new LegacySoundStyle(2, 117).WithPitchVariance(0.8f), player.Center);
        }
    }
}
