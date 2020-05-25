using Microsoft.Xna.Framework;
using System;
using TerraLeague.Projectiles;
using Terraria;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.SummonerSpells
{
    public class SyphonRune : SummonerSpell
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Syphon Rune");
            Tooltip.SetDefault("");
            base.SetStaticDefaults();
        }

        public override string GetIconTexturePath()
        {
            return "Items/SummonerSpells/Syphon";
        }

        public override string GetSpellName()
        {
            return "Syphon";
        }

        public override int GetRawCooldown()
        {
            return 150;
        }

        public int GetDamageStat()
        {
            if (NPC.downedGolemBoss)
                return 250;
            else if (NPC.downedPlantBoss)
                return 125;
            else if (NPC.downedMechBossAny)
                return 75;
            else if (Main.hardMode)
                return 50;
            else if (NPC.downedBoss2)
                return 25;
            else
                return 15;
        }

        public override string GetTooltip()
        {
            return "Damage all nearby enemies for " + GetDamageStat() + " and heal " + (int)(10 * Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().healPowerLastStep) + " life for each enemy hit" +
                "\nDamage scales thoughout the game";
        }

        public override void DoEffect(Player player, int spellSlot)
        {
            for (int i = 0; i < Main.npc.Length; i++)
            {
                float distance = 700;
                if (!Main.npc[i].townNPC && Main.npc[i].active && !Main.npc[i].immortal && !Main.npc[i].dontTakeDamage)
                {
                    Vector2 newMove = Main.npc[i].Center - player.Center;
                    float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
                    if (player.Distance(Main.npc[i].Center) < distance)
                    {
                        Projectile.NewProjectile(player.Center, Vector2.Zero, ProjectileType<Summoner_Syphon>(), GetDamageStat(), 0, player.whoAmI, i);
                        SetCooldowns(player, spellSlot);
                    }
                }
            }
        }
    }
}
