using Microsoft.Xna.Framework;
using TerraLeague.Projectiles;
using Terraria;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.SummonerSpells
{
    public class SmiteRune : SummonerSpell
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Smite Rune");
            Tooltip.SetDefault("");
            base.SetStaticDefaults();
        }

        public override string GetIconTexturePath()
        {
            return "Items/SummonerSpells/Smite";
        }

        public override string GetSpellName()
        {
            return "Smite";
        }

        public override int GetRawCooldown()
        {
            return 60;
        }

        public int GetDamageStat()
        {
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
        public override string GetTooltip()
        {
            return "Smite an enemy at you cursor for " + GetDamageStat() + " damage" +
                "\nThe damage increases throughout the game" +
                "\nIf the enemy is a Boss or the enemy dies, you will heal 15% of your max life";
        }

        public override void DoEffect(Player player, int spellSlot)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            
            int npc = TerraLeague.NPCMouseIsHovering();
            if (npc != -1)
            {
                Projectile.NewProjectile(new Vector2(Main.npc[npc].Center.X + Main.rand.NextFloat(-400, 400), Main.npc[npc].Center.Y - 1500), Vector2.Zero, ProjectileType<Summoner_Smite>(), GetDamageStat(), 0, player.whoAmI, npc);

                SetCooldowns(player, spellSlot);
            }
        }
    }
}
