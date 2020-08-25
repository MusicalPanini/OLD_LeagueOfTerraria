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

        public static int debuffDuration = 8;

        static int GetScalingDamageOverEightSeconds()
        {
            //if (NPC.downedGolemBoss)
            //    return 3000;
            //else if (NPC.downedPlantBoss)
            //    return 1500;
            //else if (NPC.downedMechBossAny)
            //    return 1000;
            //else if (Main.hardMode)
            //    return 650;
            //else if (NPC.downedBoss2)
            //    return 400;
            //else
            //    return 200;

            if (NPC.downedGolemBoss)
                return 2000;
            else if (NPC.downedPlantBoss)
                return 1000;
            else if (NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3)
                return 750;
            else if (NPC.downedMechBossAny)
                return 500;
            else if (Main.hardMode)
                return 300;
            else if (NPC.downedBoss2)
                return 125;
            else
                return 75;
        }

        public static int GetDOTDamage()
        {
            return GetScalingDamageOverEightSeconds() / 4;
        }

        public override string GetTooltip()
        {
            return "Set enemy on fire and apply 'Grievous Wounds' at" +
                "\nyour cursor, dealing " + (int)(GetScalingDamageOverEightSeconds() * 1.5) + " damage over " + debuffDuration + " seconds." +
                "\nThe damage increases throughout the game";
        }

        public override void DoEffect(Player player, int spellSlot)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            int npc = TerraLeague.NPCMouseIsHovering();
            if (npc != -1)
            {
                Projectile.NewProjectile(player.Center, Vector2.Zero, ProjectileType<Summoner_Ignite>(), 1, 0, player.whoAmI, npc);

                SetCooldowns(player, spellSlot);
            }
        }
    }
}
