using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
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
            return "Damage all nearby enemies for " + GetDamageStat() + " and heal " + TerraLeague.CreateScalingTooltip(DamageType.NONE, Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().ScaleValueWithHealPower(10, true), 100, true) + " life for each enemy hit" +
                "\nDamage scales thoughout the game";
        }

        public override void DoEffect(Player player, int spellSlot)
        {
            List<int> npcs = TerraLeague.GetAllNPCsInRange(player.MountedCenter, 600);

            if (npcs.Count != 0)
            {
                Projectile.NewProjectileDirect(player.MountedCenter, Vector2.Zero, ProjectileType<Summoner_SyphonVisuals>(), 0, 0, player.whoAmI);
                for (int i = 0; i < npcs.Count; i++)
                {
                    Projectile.NewProjectile(Main.npc[npcs[i]].Center, Vector2.Zero, ProjectileType<Summoner_Syphon>(), GetDamageStat(), 0, player.whoAmI, npcs[i]);
                    SetCooldowns(player, spellSlot);
                }
            }
        }
    }
}
