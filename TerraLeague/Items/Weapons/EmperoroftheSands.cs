﻿using Microsoft.Xna.Framework;
using TerraLeague.Buffs;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class EmperoroftheSands : AbilityItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Emperor of the Sands");
            Tooltip.SetDefault("");
            base.SetStaticDefaults();
        }

        public override string GetWeaponTooltip()
        {
            return "Create a solder of sand to fight for you!";
        }

        public override string GetQuote()
        {
            return "Your emperor shall return";
        }

        public override string GetAbilityName(AbilityType type)
        {
            if (type == AbilityType.W)
                return "Hextech Micro-Rockets";
            else if (type == AbilityType.E)
                return "CH-2 Electron Storm Grenade";
            else
                return base.GetAbilityName(type);
        }

        public override string GetIconTexturePath(AbilityType type)
        {
            if (type == AbilityType.W)
                return "AbilityImages/MicroRockets";
            else if (type == AbilityType.E)
                return "AbilityImages/StormGrenade";
            else
                return base.GetIconTexturePath(type);
        }

        public override string GetAbilityTooltip(AbilityType type)
        {
            if (type == AbilityType.W)
            {
                return "Fire 5 rockets in a cone towards your cursor";
            }
            else if (type == AbilityType.E)
            {
                return "Throw a granade that stuns hit enemies";
            }
            else
            {
                return base.GetAbilityTooltip(type);
            }
        }

        public override int GetAbilityBaseDamage(Player player, AbilityType type)
        {
            if (type == AbilityType.W)
                return (int)(0.6 * Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().rocketDamageLastStep * item.damage);
            else if (type == AbilityType.E)
                return (int)(1.5 * item.damage);
            else
                return base.GetAbilityBaseDamage(player, type);
        }

        public override int GetAbilityScalingAmount(Player player, AbilityType type, DamageType dam)
        {
            PLAYERGLOBAL modPlayer = Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>();
            if (type == AbilityType.W)
            {
                if (dam == DamageType.SUM)
                    return 25;
            }
            else if (type == AbilityType.E)
            {
                if (dam == DamageType.SUM)
                    return 60;
            }
            return base.GetAbilityScalingAmount(player, type, dam);
        }

        public override int GetBaseManaCost(AbilityType type)
        {
            if (type == AbilityType.W)
                return 20;
            else if (type == AbilityType.E)
                return 20;
            else
                return base.GetBaseManaCost(type);
        }

        public override string GetDamageTooltip(Player player, AbilityType type)
        {
            if (type == AbilityType.W)
                return GetAbilityBaseDamage(player, type) + " + " + GetScalingTooltip(player, type, DamageType.SUM) + " summon damage per rocket";
            else if (type == AbilityType.E)
                return GetAbilityBaseDamage(player, type) + " + " + GetScalingTooltip(player, type, DamageType.SUM) + " summon damage";
            else
                return base.GetDamageTooltip(player, type);
        }

        public override bool CanBeCastWhileUsingItem(AbilityType type)
        {
            if (type == AbilityType.W || type == AbilityType.E)
                return true;
            else
                return false;
        }

        public override int GetRawCooldown(AbilityType type)
        {
            if (type == AbilityType.W)
                return 8;
            else if (type == AbilityType.E)
                return 10;
            else
                return base.GetRawCooldown(type);
        }

        public override void DoEffect(Player player, AbilityType type)
        {
            if (type == AbilityType.W)
            {
                if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetScaledManaCost(type), true))
                {
                    DoEfx(player, type);
                    int projType = ProjectileType<HextechWrench_MicroRocket>();
                    int damage = GetAbilityBaseDamage(player, type) + GetAbilityScalingDamage(player, type, DamageType.SUM);
                    int knockback = 1;

                    int numberProjectiles = 5;
                    int distance = 24;
                    for (int i = 0; i < numberProjectiles; i++)
                    {
                        Vector2 relPosition = new Vector2(0 - (distance * 2) + (i * distance), 0).RotatedBy(TerraLeague.CalcAngle(player.Center, Main.MouseWorld) + MathHelper.PiOver2);
                        Vector2 position = new Vector2(player.MountedCenter.X + relPosition.X, player.MountedCenter.Y + relPosition.Y);
                        Vector2 velocity = TerraLeague.CalcVelocityToMouse(position, 15f);

                        Projectile.NewProjectile(position, velocity, projType, damage, knockback, player.whoAmI);
                    }
                    SetCooldowns(player, type);
                }
            }
            else if (type == AbilityType.E)
            {
                if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetScaledManaCost(type), true))
                {
                    Vector2 position = player.MountedCenter;
                    Vector2 velocity = TerraLeague.CalcVelocityToMouse(position, 16f);
                    int projType = ProjectileType<HextechWrench_StormGrenade>();
                    int damage = GetAbilityBaseDamage(player, type) + GetAbilityScalingDamage(player, type, DamageType.SUM);
                    int knockback = 0;

                    
                    Projectile.NewProjectile(position, velocity, projType, damage, knockback, player.whoAmI);
                    DoEfx(player, type);
                    SetCooldowns(player, type);
                }
            }
            else
            {
                base.DoEffect(player, type);
            }
        }

        public override void SetDefaults()
        {
            item.damage = 9;
            item.summon = true;
            item.mana = 20;
            item.width = 48;
            item.height = 48;
            item.useTime = 36;
            item.useAnimation = 36;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.noMelee = true;
            item.knockBack = 1;
            item.value = 10000;
            item.rare = ItemRarityID.Green;
            item.UseSound = new LegacySoundStyle(2, 113);
            item.shoot = ProjectileType<EmperoroftheSands_SandSolder>();
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            position = Main.MouseWorld;

            if (player.altFunctionUse != 2)
            {
                bool pathBlocked = false;
                for (int x = (int)((Main.mouseX + Main.screenPosition.X) / 16) - 1; x < (int)((Main.mouseX + Main.screenPosition.X) / 16) + 1; x++)
                {
                    for (int y = (int)((Main.mouseY + Main.screenPosition.Y) / 16) - 1; y <= (int)((Main.mouseY + Main.screenPosition.Y) / 16) + 1; y++)
                    {
                        if (Main.tile[x, y].collisionType > 0)
                        {
                            pathBlocked = true;
                            break;
                        }
                    }
                }

                if (!pathBlocked)
                {
                    return true;
                }
            }

            return false;
        }

        public override bool UseItem(Player player)
        {

            return base.UseItem(player);
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {

            }
            else
            {
                player.AddBuff(BuffType<SandSolder>(), 2);
            }
            return base.CanUseItem(player);
        }

        public override bool GetIfAbilityExists(AbilityType type)
        {
            if (type == AbilityType.W || type == AbilityType.E)
                return false;
            return base.GetIfAbilityExists(type);
        }

        public override void Efx(Player player, AbilityType type)
        {
            if (type == AbilityType.W)
                Main.PlaySound(new LegacySoundStyle(2, 11), player.Center);
            else if (type == AbilityType.E)
                Main.PlaySound(new LegacySoundStyle(2, 11), player.Center);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Sapphire, 4);
            recipe.AddIngredient(ItemID.SandBlock, 100);
            recipe.AddRecipeGroup("TerraLeague:GoldGroup", 10);
            recipe.AddIngredient(ItemType<Sunstone>(), 10);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}