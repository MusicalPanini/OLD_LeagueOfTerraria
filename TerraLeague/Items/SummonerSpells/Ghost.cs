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
            int spacing = 48;
            int type = 111;

            TerraLeague.DustLine(player.MountedCenter + new Microsoft.Xna.Framework.Vector2(24, 0), player.MountedCenter + new Microsoft.Xna.Framework.Vector2(-24, 0), type, 0.5f, 2);

            TerraLeague.DustLine(player.MountedCenter + new Microsoft.Xna.Framework.Vector2(48, 0), player.MountedCenter + new Microsoft.Xna.Framework.Vector2(0, 0), type, 0.5f, 2, default, true, -10, 0);
            TerraLeague.DustLine(player.MountedCenter + new Microsoft.Xna.Framework.Vector2(-48, 0), player.MountedCenter + new Microsoft.Xna.Framework.Vector2(0, 0), type, 0.5f, 2, default, true, 10, 0);

            TerraLeague.DustLine(player.MountedCenter + new Microsoft.Xna.Framework.Vector2(16, spacing), player.MountedCenter + new Microsoft.Xna.Framework.Vector2(0, spacing), type, 0.5f, 2, default, true, -5, 0);
            TerraLeague.DustLine(player.MountedCenter + new Microsoft.Xna.Framework.Vector2(-32, spacing), player.MountedCenter + new Microsoft.Xna.Framework.Vector2(0, spacing), type, 0.5f, 2, default, true, 5, 0);

            TerraLeague.DustLine(player.MountedCenter + new Microsoft.Xna.Framework.Vector2(32, -spacing), player.MountedCenter + new Microsoft.Xna.Framework.Vector2(0, -spacing), type, 0.5f, 2, default, true, -5, 0);
            TerraLeague.DustLine(player.MountedCenter + new Microsoft.Xna.Framework.Vector2(-16, -spacing), player.MountedCenter + new Microsoft.Xna.Framework.Vector2(0, -spacing), type, 0.5f, 2, default, true, 5, 0);

            Main.PlaySound(new LegacySoundStyle(2, 117).WithPitchVariance(0.8f), player.Center);
        }
    }
}
