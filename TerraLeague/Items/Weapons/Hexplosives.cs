using Microsoft.Xna.Framework;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class Hexplosives : AbilityItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hexplosives");
            Tooltip.SetDefault("");
        }

        public override string GetWeaponTooltip()
        {
            return "";
        }

        public override string GetQuote()
        {
            return "This'll be a blast!";
        }

        public override string GetAbilityName(AbilityType type)
        {
            if (type == AbilityType.Q)
                return "Bouncing Bomb";
            else if (type == AbilityType.E)
                return "Hexsplosive Mine Field";
            else
                return base.GetAbilityName(type);
        }

        public override string GetIconTexturePath(AbilityType type)
        {
            if (type == AbilityType.Q)
                return "AbilityImages/BouncingBomb";
            else if (type == AbilityType.E)
                return "AbilityImages/MineField";
            else
                return base.GetIconTexturePath(type);
        }

        public override string GetAbilityTooltip(AbilityType type)
        {
            if (type == AbilityType.Q)
            {
                return "Throw a bouncing explosive that detonates after 3 bounces";
            }
            else if (type == AbilityType.E)
            {
                return "Toss a bunch of small bombs that create a minefield";
            }
            else
            {
                return base.GetAbilityTooltip(type);
            }
        }

        public override int GetAbilityBaseDamage(Player player, AbilityType type)
        {
            if (type == AbilityType.Q)
                return (int)(item.damage * 1.5);
            else if (type == AbilityType.E)
                return (int)(item.damage * 0.8);
            else
                return base.GetAbilityBaseDamage(player, type);
        }

        public override int GetAbilityScalingAmount(Player player, AbilityType type, DamageType dam)
        {
            PLAYERGLOBAL modPlayer = Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>();
            if (type == AbilityType.Q)
            {
                if (dam == DamageType.MAG)
                    return 65;
            }
            else if (type == AbilityType.E)
            {
                if (dam == DamageType.MAG)
                    return 20;
            }
            return base.GetAbilityScalingAmount(player, type, dam);
        }

        public override int GetBaseManaCost(AbilityType type)
        {
            if (type == AbilityType.Q)
                return 20;
            else if (type == AbilityType.E)
                return 20;
            else
                return base.GetBaseManaCost(type);
        }

        public override string GetDamageTooltip(Player player, AbilityType type)
        {
            if (type == AbilityType.Q)
                return GetAbilityBaseDamage(player, type) + " + " + GetScalingTooltip(player, type, DamageType.MAG) + " magic damage";
            else if (type == AbilityType.E)
                return GetAbilityBaseDamage(player, type) + " + " + GetScalingTooltip(player, type, DamageType.MAG) + " magic damage per mine";
            else
                return base.GetDamageTooltip(player, type);
        }

        public override bool CanBeCastWhileUsingItem(AbilityType type)
        {
            return false;
        }

        public override int GetRawCooldown(AbilityType type)
        {
            if (type == AbilityType.Q)
                return 6;
            else if (type == AbilityType.E)
                return 12;
            else
                return base.GetRawCooldown(type);
        }

        public override void DoEffect(Player player, AbilityType type)
        {
            if (type == AbilityType.Q)
            {
                if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetScaledManaCost(type), true))
                {
                    Vector2 position = player.Center;
                    Vector2 velocity = TerraLeague.CalcVelocityToMouse(position, 12);
                    int projType = ProjectileType<BouncingBomb>();
                    int damage = GetAbilityBaseDamage(player, type) + GetAbilityScalingDamage(player, type, DamageType.MAG);
                    int knockback = 7;

                    SetAnimation(player, item.useTime, item.useAnimation, position + velocity);
                    DoEfx(player, type);
                    Projectile.NewProjectile(position, velocity, projType, damage, knockback, player.whoAmI);
                    SetCooldowns(player, type);
                }
            }
            else if (type == AbilityType.E)
            {
                if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetBaseManaCost(type), true))
                {
                    Vector2 position = player.Center;
                    Vector2 velocity = TerraLeague.CalcVelocityToMouse(position, 10);
                    int projType = ProjectileType<Minefield>();
                    int damage = GetAbilityBaseDamage(player, type) + GetAbilityScalingDamage(player, type, DamageType.MAG);
                    int knockback = 0;

                    SetAnimation(player, item.useTime, item.useAnimation, position + velocity);
                    DoEfx(player, type);
                    Projectile.NewProjectile(position, velocity, projType, damage, knockback, player.whoAmI);
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
            item.damage = 55;
            item.width = 32;
            item.height = 32;
            item.magic = true;
            item.useAnimation = 32;
            item.useTime = 32;
            item.useStyle = 1;
            item.knockBack = 5;
            item.value = 40000;
            item.rare = 4;
            item.UseSound = new LegacySoundStyle(2, 19, Terraria.Audio.SoundType.Sound);
            item.shootSpeed = 12f;
            item.shoot = ProjectileType<HexplosiveShot>();
            item.mana = 8;
            item.noMelee = true;
            item.useTurn = true;
            item.autoReuse = true;
            item.noUseGraphic = true;
        }

        public override bool CanUseItem(Player player)
        {
            return true;
        }

        public override float UseTimeMultiplier(Player player)
        {
            return 1;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            return true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<HextechCore>(), 2);
            recipe.AddIngredient(ItemID.Bomb, 20);
            recipe.AddRecipeGroup("TerraLeague:Tier3Bar", 6);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override bool GetIfAbilityExists(AbilityType type)
        {
            if (type == AbilityType.Q || type == AbilityType.E)
                return true;
            return base.GetIfAbilityExists(type);
        }

        public override void Efx(Player player, AbilityType type)
        {
            if (type == AbilityType.Q || type == AbilityType.E)
                Main.PlaySound(new LegacySoundStyle(2, 19, Terraria.Audio.SoundType.Sound), player.Center);
        }
    }
}
