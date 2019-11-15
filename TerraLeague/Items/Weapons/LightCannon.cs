﻿using Microsoft.Xna.Framework;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class LightCannon : AbilityItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Light Cannon");
            Tooltip.SetDefault("");
        }

        public override string GetWeaponTooltip()
        {
            return "Deals an additional [c/" + TerraLeague.RNGColor + ":" + Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().RNG/4 + "] damage";
        }

        public override string GetQuote()
        {
            return "I don't carry this to compromise";
        }

        public override string GetAbilityName(AbilityType type)
        {
            if (type == AbilityType.R)
                return "Noxious Trap";
            else if (type == AbilityType.E)
                return "Toxic Shot";
            else
                return base.GetAbilityName(type);
        }

        public override string GetIconTexturePath(AbilityType type)
        {
            if (type == AbilityType.R)
                return "AbilityImages/NoxiousTrap";
            if (type == AbilityType.E)
                return "AbilityImages/ToxicShot";
            else
                return base.GetIconTexturePath(type);
        }

        public override string GetAbilityTooltip(AbilityType type)
        {
            if (type == AbilityType.R)
            {
                return "Toss 3 mushroom traps that will rupture and releases clouds of venom when an enemy is near";
            }
            else if (type == AbilityType.E)
            {
                return "Your ranged attacks apply 'Venom' and deal additional On Hit damage for 5 seconds";
            }
            else
            {
                return base.GetAbilityTooltip(type);
            }
        }

        public override int GetAbilityBaseDamage(Player player, AbilityType type)
        {
            if (type == AbilityType.R)
                return (int)System.Math.Round(item.damage * 1.5);
            else if (type == AbilityType.E)
                return (int)(60);
            else
                return base.GetAbilityBaseDamage(player, type);
        }

        public override int GetAbilityScalingAmount(Player player, AbilityType type, DamageType dam)
        {
            if (type == AbilityType.R)
            {
                if (dam == DamageType.SUM)
                    return 12;
            }
            else if (type == AbilityType.E)
            {
                if (dam == DamageType.SUM)
                    return 30;
            }
            return base.GetAbilityScalingAmount(player, type, dam);
        }

        public override int GetBaseManaCost(AbilityType type)
        {
            if (type == AbilityType.R)
                return 30;
            else if (type == AbilityType.E)
                return 50;
            else
                return base.GetBaseManaCost(type);
        }

        public override string GetDamageTooltip(Player player, AbilityType type)
        {
            if (type == AbilityType.R)
                return GetAbilityBaseDamage(player, type) + " + " + GetScalingTooltip(player, type, DamageType.SUM) + " minion damage";
            else if (type == AbilityType.E)
                return GetAbilityBaseDamage(player, type) + " + " + GetScalingTooltip(player, type, DamageType.SUM) + " ranged On Hit damage";
            else
                return base.GetDamageTooltip(player, type);
        }

        public override int GetRawCooldown(AbilityType type)
        {
            if (type == AbilityType.R)
                return 30;
            else if (type == AbilityType.E)
                return 15;
            else
                return base.GetRawCooldown(type);
        }

        public override bool CanBeCastWhileUsingItem(AbilityType type)
        {
            if (type == AbilityType.E)
                return true;
            else
                return false;
        }

        public override void DoEffect(Player player, AbilityType type)
        {
            if (type == AbilityType.R)
            {
                if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetScaledManaCost(type), true))
                {
                    Vector2 position = player.Center;
                    Vector2 velocity = TerraLeague.CalcVelocityToMouse(position, 8f);
                    int projType = ProjectileType<NoxiousTrap>();
                    int damage = GetAbilityBaseDamage(player, type) + GetAbilityScalingDamage(player, type, DamageType.SUM);
                    int knockback = 0;

                    Projectile.NewProjectile(position, velocity, projType, damage, knockback, player.whoAmI);
                    Projectile.NewProjectile(position, velocity * 1.5f, projType, damage, knockback, player.whoAmI);
                    Projectile.NewProjectile(position, velocity * 0.5f, projType, damage, knockback, player.whoAmI);
                    DoEfx(player, type);
                    SetCooldowns(player, type);
                }
            }
            else if (type == AbilityType.E)
            {
                if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetScaledManaCost(type), true))
                {
                    player.AddBuff(BuffType<Buffs.ToxicShot>(), 300);
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
            item.damage = 100;
            item.ranged = true;
            item.useStyle = 5;
            item.width = 108;
            item.height = 28;
            item.channel = true;
            item.useAnimation = 40;
            item.useTime = 40;
            item.shootSpeed = 10f;
            item.noMelee = true;
            item.knockBack = 1;
            item.value = 6000;
            item.rare = 4;
            item.shoot = ProjectileType<LightCannonProj>();
            item.UseSound = new Terraria.Audio.LegacySoundStyle(2, 13);
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Projectile proj = Projectile.NewProjectileDirect(player.Center, Vector2.Zero, type, damage + player.GetModPlayer<PLAYERGLOBAL>().RNG / 4, knockBack, player.whoAmI);
            proj.rotation = new Vector2(speedX, speedY).ToRotation();

            return false;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<TrueIceChunk>(), 4);
            recipe.AddIngredient(ItemID.Blowgun, 1);
            recipe.AddIngredient(ItemID.VialofVenom, 10);
            recipe.AddIngredient(ItemID.Mushroom, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override bool GetIfAbilityExists(AbilityType type)
        {
            item.useAnimation = 60;
            item.useTime = 60;
            if (type == AbilityType.R || type == AbilityType.E)
                return false;
            return base.GetIfAbilityExists(type);
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-16, 4);
        }

        public override void Efx(Player player, AbilityType type)
        {
            if (type == AbilityType.R)
                Main.PlaySound(SoundID.Item1, player.Center);
        }
    }
}