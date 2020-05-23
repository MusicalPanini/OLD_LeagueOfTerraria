using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using TerraLeague.Buffs;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class Whisper : AbilityItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Whisper");
            Tooltip.SetDefault("");
        }

        public override string GetWeaponTooltip()
        {
            return "Fire 4 shots before having to reload" +
                "\nThe 4th shot will deal 2x damage and crit";
        }

        public override string GetQuote()
        {
            return "Prepare... for your finale";
        }

        public override string GetAbilityName(AbilityType type)
        {
            if (type == AbilityType.Q)
                return "Dancing Grenade";
            else
                return base.GetAbilityName(type);
        }

        public override string GetIconTexturePath(AbilityType type)
        {
            if (type == AbilityType.Q)
                return "AbilityImages/DancingGranade";
            else
                return base.GetIconTexturePath(type);
        }

        public override string GetAbilityTooltip(AbilityType type)
        {
            if (type == AbilityType.Q)
            {
                return "Launch a bouncing grenade that gains damage for each enemy it kills";
            }
            else
            {
                return base.GetAbilityTooltip(type);
            }
        }

        public override int GetAbilityBaseDamage(Player player, AbilityType type)
        {
            if (type == AbilityType.Q)
                return (int)System.Math.Round(0.75 * item.damage * Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().rocketDamageLastStep);
            else
                return base.GetAbilityBaseDamage(player, type);
        }

        public override int GetAbilityScalingAmount(Player player, AbilityType type, DamageType dam)
        {
            if (type == AbilityType.Q)
            {
                if (dam == DamageType.RNG)
                    return 75;
                else if (dam == DamageType.MAG)
                    return 60;
            }
            return base.GetAbilityScalingAmount(player, type, dam);
        }

        public override int GetBaseManaCost(AbilityType type)
        {
            if (type == AbilityType.Q)
                return 30;
            else
                return base.GetBaseManaCost(type);
        }

        public override string GetDamageTooltip(Player player, AbilityType type)
        {
            if (type == AbilityType.Q)
                return GetAbilityBaseDamage(player, type) + " + " + GetScalingTooltip(player, type, DamageType.RNG) + " + " + GetScalingTooltip(player, type, DamageType.MAG) + " ranged damage";
            else
                return base.GetDamageTooltip(player, type);
        }

        public override int GetRawCooldown(AbilityType type)
        {
            if (type == AbilityType.Q)
                return 8;
            else
                return base.GetRawCooldown(type);
        }

        public override bool CanBeCastWhileUsingItem(AbilityType type)
        {
            if (type == AbilityType.Q)
                return true;
            else
                return false;
        }

        public override void DoEffect(Player player, AbilityType type)
        {
            if (type == AbilityType.Q)
            {
                if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetScaledManaCost(type), true))
                {
                    Vector2 position = player.MountedCenter;
                    Vector2 velocity = TerraLeague.CalcVelocityToMouse(position, 18f);
                    velocity.Y *= 0.8f;
                    int projType = ProjectileType<WhisperNade>();
                    int damage = GetAbilityBaseDamage(player, type) + GetAbilityScalingDamage(player, type, DamageType.RNG) + GetAbilityScalingDamage(player, type, DamageType.MAG);
                    int knockback = 4;

                    SetAnimation(player, 20, 20, position + velocity);
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
            item.damage = 144;
            item.ranged = true;
            item.width = 44;
            item.height = 20;
            item.useAnimation = 80;
            item.useTime = 80;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.noMelee = true; 
            item.knockBack = 4;
            item.value = 80000;
            item.rare = ItemRarityID.LightPurple;
            item.shoot = ProjectileID.PurificationPowder;
            item.shootSpeed = 1f;
            item.useAmmo = AmmoID.Bullet;
            item.autoReuse = true;
        }

        public override bool CanUseItem(Player player)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            if (modPlayer.WhisperShotsLeft == 0)
                return false;
            return base.CanUseItem(player);
        }

        public override float UseTimeMultiplier(Player player)
        {
            return 1;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            if (modPlayer.WhisperShotsLeft == 1)
            {
                type = ProjectileType<WhisperFShot>();
                damage *= 2;
            }
            else
            {
                type = ProjectileType<WhisperShot>();
            }
            SetStatsPostShoot(player);
            return true;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-7, 0);
        }

        public void SetStatsPostShoot(Player player)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            modPlayer.WhisperShotsLeft--;
            if (modPlayer.WhisperShotsLeft == 0)
                modPlayer.ReloadTimer = 160;
            else
                modPlayer.ReloadTimer = 320;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.HallowedBar, 10);
            recipe.AddIngredient(ItemID.SoulofFright, 5);
            recipe.AddIngredient(ItemID.SoulofMight, 5);
            recipe.AddIngredient(ItemID.SoulofSight, 5);
            recipe.AddIngredient(ItemID.Marble, 30);
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

        public override void Efx(Player player, AbilityType type)
        {
            if (type == AbilityType.Q)
                Main.PlaySound(new LegacySoundStyle(2, 11), player.Center);
        }
    }
}
