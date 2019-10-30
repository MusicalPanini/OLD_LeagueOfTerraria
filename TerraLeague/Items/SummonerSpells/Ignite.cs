using Microsoft.Xna.Framework;
using TerraLeague.Projectiles;
using Terraria;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.SummonerSpells
{
    public class IgniteRune : SummonerSpell
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ignite Rune");
            Tooltip.SetDefault("");
            base.SetStaticDefaults();
        }
        public override string GetIconTexturePath()
        {
            return "Items/SummonerSpells/Ignite";
        }

        public override string GetSpellName()
        {
            return "Ignite";
        }

        public override int GetRawCooldown()
        {
            return 90;
        }
        public override string GetTooltip()
        {
            return "Set enemy on fire and apply 'Grievous Wounds' at you cursor for 4 seconds" +
                "\nThe damage increases throughout the game";
        }

        public override void DoEffect(Player player, int spellSlot)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            int npc = TerraLeague.NPCMouseIsHovering();
            if (npc != -1)
            {
                Projectile.NewProjectile(player.Center, Vector2.Zero, ProjectileType<IgniteProj>(), 1, 0, player.whoAmI, npc);

                SetCooldowns(player, spellSlot);
            }
        }
    }
}
