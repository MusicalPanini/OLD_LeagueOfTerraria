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
            return "Deals an additional " + TerraLeague.CreateScalingTooltip(DamageType.RNG, Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().RNG, 50) + " damage";
        }

        public override string GetQuote()
        {
            return "I don't carry this to compromise";
        }

        public override string GetAbilityName(AbilityType type)
        {
            if (type == AbilityType.Q)
                return "Piercing Darkness";
            else
                return base.GetAbilityName(type);
        }

        public override string GetIconTexturePath(AbilityType type)
        {
            if (type == AbilityType.Q)
                return "AbilityImages/PiercingDarkness";
            else
                return base.GetIconTexturePath(type);
        }

        public override string GetAbilityTooltip(AbilityType type)
        {
            if (type == AbilityType.Q)
            {
                return "Fire a spectral laser damaging enemies and healing allies";
            }
            else
            {
                return base.GetAbilityTooltip(type);
            }
        }

        public override int GetAbilityBaseDamage(Player player, AbilityType type)
        {
            PLAYERGLOBAL modPlayer = Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>();
            if (type == AbilityType.Q)
                return (int)item.damage * 2;
            else if (type == AbilityType.W)
                return (int)((item.damage/4));
            else
                return base.GetAbilityBaseDamage(player, type);
        }

        public override int GetAbilityScalingAmount(Player player, AbilityType type, DamageType dam)
        {
            PLAYERGLOBAL modPlayer = Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>();
            if (type == AbilityType.Q)
            {
                if (dam == DamageType.RNG)
                    return 75;
            }
            if (type == AbilityType.W)
            {
                if (dam == DamageType.RNG)
                    return (int)(40);
                else if (dam == DamageType.MAG)
                    return (int)(25);
            }
            return base.GetAbilityScalingAmount(player, type, dam);
        }

        public override int GetBaseManaCost(AbilityType type)
        {
            if (type == AbilityType.Q)
                return 80;
            else
                return base.GetBaseManaCost(type);
        }

        public override string GetDamageTooltip(Player player, AbilityType type)
        {
            if (type == AbilityType.Q)
                return GetAbilityBaseDamage(player, type) + " + " + GetScalingTooltip(player, type, DamageType.RNG) + " range damage" +
                    "\n" + TerraLeague.CreateScalingTooltip(DamageType.NONE, GetAbilityBaseDamage(player, AbilityType.W), 100, true) + " + " + GetScalingTooltip(player, AbilityType.W, DamageType.RNG, true) + " + " + GetScalingTooltip(player, AbilityType.W, DamageType.MAG, true) + " healing";
            else
                return base.GetDamageTooltip(player, type);
        }

        public override int GetRawCooldown(AbilityType type)
        {
            if (type == AbilityType.Q)
                return 24;
            else
                return base.GetRawCooldown(type);
        }

        public override bool CanBeCastWhileUsingItem(AbilityType type)
        {
            return false;
        }

        public override void DoEffect(Player player, AbilityType type)
        {
            if (type == AbilityType.Q)
            {
                if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetScaledManaCost(type), true))
                {
                    PLAYERGLOBAL modPlayer = Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>();
                    Vector2 position = player.MountedCenter;
                    Vector2 velocity = TerraLeague.CalcVelocityToMouse(position, 8f);
                    int projType = ProjectileType<LightCannon_PiercingDarkness>();
                    int damage = GetAbilityBaseDamage(player, type) + GetAbilityScalingDamage(player, type, DamageType.RNG);
                    int healing = modPlayer.ScaleValueWithHealPower(GetAbilityBaseDamage(player, AbilityType.W) + GetAbilityScalingDamage(player, AbilityType.W, DamageType.RNG) + GetAbilityScalingDamage(player, AbilityType.W, DamageType.MAG));
                    int knockback = 0;

                    Projectile proj = Projectile.NewProjectileDirect(position, velocity, projType, damage, knockback, player.whoAmI, healing);

                    SetAnimation(player, 30, 30, position + velocity);
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
            item.damage = 150;
            item.ranged = true;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.width = 108;
            item.height = 28;
            item.channel = true;
            item.useAnimation = 60;
            item.useTime = 60;
            item.shootSpeed = 10f;
            item.noMelee = true;
            item.knockBack = 7;
            item.value = 160000;
            item.rare = ItemRarityID.Pink;
            item.shoot = ProjectileType<LightCannon_BeamControl>();
            item.UseSound = new Terraria.Audio.LegacySoundStyle(2, 13);
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Projectile proj = Projectile.NewProjectileDirect(player.Center, Vector2.Zero, type, damage + player.GetModPlayer<PLAYERGLOBAL>().RNG / 2, knockBack, player.whoAmI);
            proj.rotation = new Vector2(speedX, speedY).ToRotation();

            return false;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<DamnedSoul>(), 100);
            recipe.AddIngredient(ItemID.HallowedBar, 16);
            recipe.AddIngredient(ItemID.Marble, 100);
            recipe.AddIngredient(ItemID.IllegalGunParts, 1);
            recipe.AddIngredient(ItemID.SoulofMight, 10);
            recipe.AddIngredient(ItemID.SoulofLight, 5);
            recipe.AddIngredient(ItemID.SoulofNight, 5);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override bool GetIfAbilityExists(AbilityType type)
        {
            if (type == AbilityType.Q)
                return true;
            return base.GetIfAbilityExists(type);
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-16, 4);
        }

        public override void Efx(Player player, AbilityType type)
        {
            if (type == AbilityType.Q)
            {
                TerraLeague.PlaySoundWithPitch(player.MountedCenter, 2, 13, 1f);
            }
        }
    }
}
