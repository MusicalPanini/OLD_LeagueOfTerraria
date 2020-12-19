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
    public class PlasmaFission : Ability
    {
        public PlasmaFission(Terraria.ModLoader.ModItem item)
        {
            abilityItem = item;
        }

        public override string GetAbilityName()
        {
            return "Plasma Fission";
        }

        public override string GetIconTexturePath()
        {
            return "AbilityImages/PlasmaFission";
        }

        public override string GetAbilityTooltip()
        {
            return "Fire a projectile that splits in 2 on collision." +
                "\nRecast to split early." +
                "\nHit enemies will take 10% more magic damage.";
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
            return 8;
        }

        public override int GetBaseManaCost()
        {
            return 35;
        }

        public override string GetDamageTooltip(Player player)
        {
            return GetAbilityBaseDamage(player) + " + " + GetScalingTooltip(player, DamageType.MAG) + " magic damage";
        }

        public override bool CanBeCastWhileUsingItem()
        {
            return CurrentlyHasSpecialCast(Main.LocalPlayer);
        }

        public override bool CanBeCastWhileCCd()
        {
            return CurrentlyHasSpecialCast(Main.LocalPlayer);
        }

        public override bool CurrentlyHasSpecialCast(Player player)
        {
            int pos = GetPositionOfAbilityInArray(abilityItem);
            if (pos != -1)
                return (player.ownedProjectileCounts[ProjectileType<EyeOfTheVoid_Plasma>()] > 0 && player.GetModPlayer<PLAYERGLOBAL>().AbilityCooldowns[pos] <= GetCooldown() * 60 - 10);
            return false;
        }

        public override void DoEffect(Player player, AbilityType type)
        {
            if (CurrentlyHasSpecialCast(player))
            {
                Main.projectile.Where(x => x.type == ProjectileType<EyeOfTheVoid_Plasma>() && x.owner == player.whoAmI).FirstOrDefault().Kill();
            }
            else if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetScaledManaCost(), true))
            {
                Vector2 position = player.MountedCenter;
                Vector2 velocity = TerraLeague.CalcVelocityToMouse(position, 12);
                int projType = ProjectileType<EyeOfTheVoid_Plasma>();
                int damage = GetAbilityBaseDamage(player) + GetAbilityScaledDamage(player, DamageType.MAG);
                int knockback = 1;

                Projectile.NewProjectile(position, velocity, projType, damage, knockback, player.whoAmI);

                SetAnimation(player, 20, 20, position + velocity);
                DoEfx(player, type);
                SetCooldowns(player, type);
            }
        }

        public override void Efx(Player player)
        {
            TerraLeague.PlaySoundWithPitch(player.MountedCenter, 2, 93, -1f);
            TerraLeague.PlaySoundWithPitch(player.MountedCenter, 2, 43, -0.5f);
        }
    }
}
