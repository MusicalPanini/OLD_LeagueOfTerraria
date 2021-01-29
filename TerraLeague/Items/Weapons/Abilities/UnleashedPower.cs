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
    public class UnleashedPower : Ability
    {
        public UnleashedPower(Terraria.ModLoader.ModItem item)
        {
            abilityItem = item;
        }

        public override string GetAbilityName()
        {
            return "Unleashed Power";
        }

        public override string GetIconTexturePath()
        {
            return "AbilityImages/UnleashedPower";
        }

        public override string GetAbilityTooltip()
        {
            return "Launch " + TerraLeague.CreateScalingTooltip(TerraLeague.MINIONMAXColor, "MINIONS", (int)Main.LocalPlayer.maxMinions, 200) + " dark spheres at a targeted enemy";
        }

        public override int GetAbilityBaseDamage(Player player)
        {
            return (int)(abilityItem.item.damage * 2.5);
        }

        public override int GetAbilityScalingAmount(Player player, DamageType dam)
        {
            switch (dam)
            {
                case DamageType.SUM:
                    return 100;
                default:
                    return 0;
            }
        }

        public override int GetRawCooldown()
        {
            return 75;
        }

        public override int GetBaseManaCost()
        {
            return 100;
        }

        public override string GetDamageTooltip(Player player)
        {
            return GetAbilityBaseDamage(player) + " + " + GetScalingTooltip(player, DamageType.SUM) + " summon damage per sphere";
        }

        public override bool CanBeCastWhileUsingItem()
        {
            return true;
        }

        public override void DoEffect(Player player, AbilityType type)
        {
            int target = TerraLeague.NPCMouseIsHovering();
            if (target != -1)
            {
                if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetScaledManaCost(), true))
                {
                    DoEfx(player, type);
                    int projType = ProjectileType<DarkSovereignsStaff_UnleashedPower>();
                    int damage = GetAbilityBaseDamage(player) + GetAbilityScaledDamage(player, DamageType.SUM);
                    int knockback = 3;

                    int numberProjectiles = player.maxMinions * 2;
                    for (int i = 0; i < numberProjectiles; i++)
                    {
                        Vector2 position = player.MountedCenter;
                        Vector2 velocity = Vector2.Zero;

                        Projectile.NewProjectile(position, velocity, projType, damage, knockback, player.whoAmI, target, i);
                    }
                    SetCooldowns(player, type);
                }
            }
        }

        public override void Efx(Player player)
        {
            //Main.PlaySound(new LegacySoundStyle(2, 11), player.Center);
        }
    }
}
