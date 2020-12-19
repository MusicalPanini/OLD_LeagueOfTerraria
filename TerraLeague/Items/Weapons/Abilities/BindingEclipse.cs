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
    public class BindingEclipse : Ability
    {
        public BindingEclipse(Terraria.ModLoader.ModItem item)
        {
            abilityItem = item;
        }

        public override string GetAbilityName()
        {
            return "Binding Eclipse";
        }

        public override string GetIconTexturePath()
        {
            return "AbilityImages/BindingEclipse";
        }

        public override string GetAbilityTooltip()
        {
            return "Expunge all marked enemies, stunning and dealing damage to them";
        }

        public override int GetAbilityBaseDamage(Player player)
        {
            return (int)(abilityItem.item.damage * 2);
        }

        public override int GetAbilityScalingAmount(Player player, DamageType dam)
        {
            switch (dam)
            {
                case DamageType.MAG:
                    return 80;
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
            return 40;
        }

        public override string GetDamageTooltip(Player player)
        {
            return GetAbilityBaseDamage(player) + " + " + GetScalingTooltip(player, DamageType.MAG) + " magic damage"
                + "\nUses 10% Gravitum Ammo";
        }

        public override bool CanBeCastWhileUsingItem()
        {
            return false;
        }

        public override bool CanCurrentlyBeCast(Player player)
        {
            if (player.GetModPlayer<PLAYERGLOBAL>().gravitumAmmo >= 10)
                return TerraLeague.IsThereAnNPCInRange(player.MountedCenter, 999999, BuffType<GravitumMark>());

            return false;
        }

        public override void DoEffect(Player player, AbilityType type)
        {
            if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetBaseManaCost(), true))
            {
                player.GetModPlayer<PLAYERGLOBAL>().gravitumAmmo -= 10;
                player.CheckMana(GetBaseManaCost(), true);
                var npcs = TerraLeague.GetAllNPCsInRange(player.MountedCenter, 999999, true, true);

                for (int i = 0; i < npcs.Count; i++)
                {
                    NPC npc = Main.npc[npcs[i]];
                    if (npc.HasBuff(BuffType<GravitumMark>()))
                    {
                        SetCooldowns(player, type);
                        Projectile.NewProjectileDirect(npc.Center, Vector2.Zero, ProjectileType<Gravitum_BindingEclipse>(), GetAbilityBaseDamage(player) + GetAbilityScaledDamage(player, DamageType.MAG), 0, player.whoAmI, npcs[i]);
                    }
                }
            }
        }

        public override void Efx(Player player)
        {
            TerraLeague.PlaySoundWithPitch(player.MountedCenter, 2, 13, -1f);
        }
    }
}
