using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerraLeague.Buffs;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons.Abilities
{
    public class Contaminate : Ability
    {
        public Contaminate(Terraria.ModLoader.ModItem item)
        {
            abilityItem = item;
        }

        public override string GetAbilityName()
        {
            return "Contaminate";
        }

        public override string GetIconTexturePath()
        {
            return "AbilityImages/Contaminate";
        }

        public override string GetAbilityTooltip()
        {
            return "Damage all nearby enemies inflicted with 'Deadly Venom'";
        }

        public override int GetAbilityBaseDamage(Player player)
        {
            return (int)(7 * Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().rangedDamageLastStep);
        }

        public override int GetAbilityScalingAmount(Player player, DamageType dam)
        {
            switch (dam)
            {
                case DamageType.RNG:
                    return 35;
                case DamageType.MAG:
                    return 20;
                default:
                    return 0;
            }
        }

        public override int GetRawCooldown()
        {
            return 14;
        }

        public override int GetBaseManaCost()
        {
            return 5;
        }

        public override string GetDamageTooltip(Player player)
        {
            return "Enemies with 'Deadly Venom' take " + GetAbilityBaseDamage(player) + " + " + GetScalingTooltip(player, DamageType.RNG) + " + " + GetScalingTooltip(player, DamageType.MAG) + " ranged damage per stack";
        }

        public override bool CanBeCastWhileUsingItem()
        {
            return true;
        }

        public override bool CanCurrentlyBeCast(Player player)
        {
            return TerraLeague.IsThereAnNPCInRange(player.MountedCenter, 700, BuffType<DeadlyVenom>());
        }

        public override void DoEffect(Player player, AbilityType type)
        {
            if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetScaledManaCost()))
            {
                player.CheckMana(GetBaseManaCost(), true);
                var npcs = TerraLeague.GetAllNPCsInRange(player.MountedCenter, 700, true);

                for (int i = 0; i < npcs.Count; i++)
                {
                    NPC npc = Main.npc[npcs[i]];
                    if (npc.HasBuff(BuffType<DeadlyVenom>()))
                    {
                        SetCooldowns(player, type);
                        Projectile.NewProjectileDirect(player.Center, Vector2.Zero, ProjectileType<ChemCrossbow_Contaminate>(), GetAbilityBaseDamage(player) + GetAbilityScaledDamage(player, DamageType.RNG) + GetAbilityScaledDamage(player, DamageType.MAG), 0, player.whoAmI, npcs[i]);
                    }
                }
            }
        }

        public override void Efx(Player player)
        {
            TerraLeague.PlaySoundWithPitch(player.MountedCenter, 2, 91, -1f);
            TerraLeague.PlaySoundWithPitch(player.MountedCenter, 2, 43, -0.5f);
        }
    }
}
