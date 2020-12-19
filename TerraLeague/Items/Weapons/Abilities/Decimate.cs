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
    public class Decimate : Ability
    {
        public Decimate(Terraria.ModLoader.ModItem item)
        {
            abilityItem = item;
        }

        public override string GetAbilityName()
        {
            return "Decimate";
        }

        public override string GetIconTexturePath()
        {
            return "AbilityImages/Decimate";
        }

        public override string GetAbilityTooltip()
        {
            return "Prepare your axe then spin it with great speed" +
                    "\nHeal " + TerraLeague.CreateScalingTooltip(DamageType.NONE, (int)(7), 100, true) + " per enemy hit";
        }

        public override int GetAbilityBaseDamage(Player player)
        {
            return (int)(abilityItem.item.damage);
        }

        public override int GetAbilityScalingAmount(Player player, DamageType dam)
        {
            switch (dam)
            {
                case DamageType.MEL:
                    return 70;
                default:
                    return 0;
            }
        }

        public override int GetRawCooldown()
        {
            return 12;
        }

        public override int GetBaseManaCost()
        {
            return 30;
        }

        public override string GetDamageTooltip(Player player)
        {
            return GetAbilityBaseDamage(player) + " + " + GetScalingTooltip(player, DamageType.MEL) + " melee damage";
        }

        public override bool CanBeCastWhileUsingItem()
        {
            return false;
        }

        public override void DoEffect(Player player, AbilityType type)
        {
            if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetBaseManaCost(), true))
            {
                Vector2 position = player.Center;
                Vector2 velocity = Vector2.Zero;
                int projType = ProjectileType<DarksteelBattleaxe_Decimate>();
                int damage = GetAbilityBaseDamage(player) + GetAbilityScaledDamage(player, DamageType.MEL);
                int knockback = 4;

                Projectile.NewProjectile(position, velocity, projType, damage, knockback, player.whoAmI, player.velocity.X > 0 ? 1 : -1);
                SetCooldowns(player, type);
            }
        }
    }
}
