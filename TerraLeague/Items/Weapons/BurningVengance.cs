using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System.Collections.Generic;
using System.Linq;
using TerraLeague.NPCs;
using TerraLeague.Projectiles;
using Microsoft.Xna.Framework;
using TerraLeague.Buffs;
using Terraria.Audio;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class BurningVengance : AbilityItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Burning Vengance");
            Tooltip.SetDefault("");
        }

        public override string GetWeaponTooltip()
        {
            return "";
        }

        public override string GetQuote()
        {
            return "The inferno begins";
        }

        public override string GetAbilityName(AbilityType type)
        {
            if (type == AbilityType.W)
                return "Pillar of Flame";
            else if (type == AbilityType.R)
                return "Pyroclasm";
            else
                return base.GetAbilityName(type);
        }

        public override string GetIconTexturePath(AbilityType type)
        {
            if (type == AbilityType.W)
                return "AbilityImages/PillarOfFlame";
            else if (type == AbilityType.R)
                return "AbilityImages/Pyroclasm";
            else
                return base.GetIconTexturePath(type);
        }

        public override string GetAbilityTooltip(AbilityType type)
        {
            if (type == AbilityType.W)
            {
                return "Create a rising pillar of magic fire." +
                    "\nIf a target is on fire, set them 'Ablaze' instead";
            }
            else if (type == AbilityType.R)
            {
                return "Launch a homing fireball at a target that bounces between enemies" +
                    "\nCrits enemies who are 'Ablaze' and causes an explosion";
            }
            else
            {
                return base.GetAbilityTooltip(type);
            }
        }

        public override int GetAbilityBaseDamage(Player player, AbilityType type)
        {
            if (type == AbilityType.W)
                return (int)(2 * item.damage);
            else if (type == AbilityType.R)
                return (int)(4 * item.damage);
            else
                return base.GetAbilityBaseDamage(player, type);
        }

        public override int GetAbilityScalingAmount(Player player, AbilityType type, DamageType dam)
        {
            if (type == AbilityType.W)
            {
                if (dam == DamageType.MAG)
                    return 60;
            }
            else if (type == AbilityType.R)
            {
                if (dam == DamageType.MAG)
                    return 25;
            }
            return base.GetAbilityScalingAmount(player, type, dam);
        }

        public override int GetBaseManaCost(AbilityType type)
        {
            if (type == AbilityType.W)
                return 60;
            else if (type == AbilityType.R)
                return 100;
            else
                return base.GetBaseManaCost(type);
        }

        public override string GetDamageTooltip(Player player, AbilityType type)
        {
            if (type == AbilityType.W)
                return GetAbilityBaseDamage(player, type) + " + " + GetScalingTooltip(player, type, DamageType.MAG) + " magic damage";
            else if (type == AbilityType.R)
                return GetAbilityBaseDamage(player, type) + " + " + GetScalingTooltip(player, type, DamageType.MAG) + " magic damage";
            else
                return base.GetDamageTooltip(player, type);
        }

        public override int GetRawCooldown(AbilityType type)
        {
            if (type == AbilityType.W)
                return 12;
            else if (type == AbilityType.R)
                return 40;
            else
                return base.GetRawCooldown(type);
        }

        public override bool CanBeCastWhileUsingItem(AbilityType type)
        {
            if (type == AbilityType.W || type == AbilityType.R)
                return true;
            else
                return false;
        }

        public override void DoEffect(Player player, AbilityType type)
        {
            if (type == AbilityType.W)
            {
                if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetScaledManaCost(type), true))
                {
                    Vector2 position = new Vector2(Main.MouseWorld.X, player.position.Y + 600);
                    Vector2 velocity = new Vector2(0, -1.25f);
                    int projType = ProjectileType<BurningVengance_PillarOfFlame>();
                    int damage = GetAbilityBaseDamage(player, type) + GetAbilityScalingDamage(player, type, DamageType.MAG);
                    int knockback = 0;

                    SetAnimation(player, 10, 10, position + velocity);
                    Projectile.NewProjectile(position, velocity, projType, damage, knockback, player.whoAmI);
                    SetCooldowns(player, type);
                }
            }
            else if (type == AbilityType.R)
            {
                if (TerraLeague.NPCMouseIsHovering(30, true) != -1)
                {
                    if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetScaledManaCost(type), true))
                    {
                        Vector2 position = player.position;
                        Vector2 velocity = new Vector2(0, 0);
                        int projType = ProjectileType<BurningVengance_Pyroclasm>();
                        int damage = GetAbilityBaseDamage(player, type) + GetAbilityScalingDamage(player, type, DamageType.MAG);
                        int knockback = 0;

                        SetAnimation(player, 10, 10, position + velocity);
                        Projectile.NewProjectile(position, velocity, projType, damage, knockback, player.whoAmI, TerraLeague.NPCMouseIsHovering(30, true), -1);
                        SetCooldowns(player, type);
                    }
                }
            }
            else
            {
                base.DoEffect(player, type);
            }
        }

        public override void SetDefaults()
        {
            item.damage = 23;
            item.noMelee = true;
            item.magic = true;
            item.useTime = 5;
            item.useAnimation = 10;
            item.shootSpeed = 14;
            item.mana = 6;
            item.rare = ItemRarityID.Pink;
            item.value = 300000;
            item.width = 28;
            item.height = 32;
            item.knockBack = 1;
            item.UseSound = new LegacySoundStyle(2, 34);
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.shoot = ProjectileType<BurningVengance_Flame>();
            item.autoReuse = true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            return base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.SpellTome, 1);
            recipe.AddIngredient(ItemID.LivingFireBlock, 50);
            recipe.AddIngredient(ItemID.SoulofFright, 10);
            recipe.AddTile(TileID.Bookcases);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override bool GetIfAbilityExists(AbilityType type)
        {
            if (type == AbilityType.W || type == AbilityType.R)
                return true;
            return base.GetIfAbilityExists(type);
        }

        public override void Efx(Player player, AbilityType type)
        {
        }
    }
}
