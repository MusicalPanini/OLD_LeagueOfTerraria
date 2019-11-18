using Microsoft.Xna.Framework;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class PowPow : AbilityItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pow Pow");
            Tooltip.SetDefault("");
        }

        public override string GetWeaponTooltip()
        {
            return "Kills grant 'EXCITED!'" +
                "\nEXCITED increases firerate and damage";
        }

        public override string GetQuote()
        {
            return "SAY HELLO TO MY FRIENDS OF VARYING SIZES!";
        }

        public override string GetAbilityName(AbilityType type)
        {
            if (type == AbilityType.W)
                return "ZAP!";
            else
                return base.GetAbilityName(type);
        }

        public override string GetIconTexturePath(AbilityType type)
        {
            if (type == AbilityType.W)
                return "AbilityImages/Zap";
            else
                return base.GetIconTexturePath(type);
        }

        public override string GetAbilityTooltip(AbilityType type)
        {
            if (type == AbilityType.W)
            {
                return "Fire a shock blast that slows";
            }
            else
            {
                return base.GetAbilityTooltip(type);
            }
        }

        public override int GetAbilityBaseDamage(Player player, AbilityType type)
        {
            if (type == AbilityType.W)
                return (int)(item.damage * 1.5);
            else
                return base.GetAbilityBaseDamage(player, type);
        }

        public override int GetAbilityScalingAmount(Player player, AbilityType type, DamageType dam)
        {
            if (type == AbilityType.R)
            {
                if (dam == DamageType.RNG)
                    return 160;
            }
            return base.GetAbilityScalingAmount(player, type, dam);
        }

        public override int GetBaseManaCost(AbilityType type)
        {
            if (type == AbilityType.W)
                return 15;
            else
                return base.GetBaseManaCost(type);
        }

        public override string GetDamageTooltip(Player player, AbilityType type)
        {
            if (type == AbilityType.W)
                return GetAbilityBaseDamage(player, type) + " + " + GetScalingTooltip(player, type, DamageType.RNG) + " ranged damage";
            else
                return base.GetDamageTooltip(player, type);
        }

        public override int GetRawCooldown(AbilityType type)
        {
            if (type == AbilityType.W)
                return 8;
            else
                return base.GetRawCooldown(type);
        }

        public override void DoEffect(Player player, AbilityType type)
        {
            if (type == AbilityType.W)
            {
                if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetScaledManaCost(type), true))
                {
                    Vector2 position = player.MountedCenter;
                    Vector2 velocity = TerraLeague.CalcVelocityToMouse(position, 8f);
                    int projType = ProjectileType<Zap>();
                    int damage = GetAbilityBaseDamage(player, type) + GetAbilityScalingDamage(player, type, DamageType.RNG);
                    int knockback = 0;

                    SetAnimation(player, 20, 20, position + velocity);
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
            item.damage = 15;
            item.ranged = true;
            item.width = 76;
            item.height = 46;
            item.useAnimation = 12;
            item.useTime = 4;
            item.reuseDelay = 14;
            item.useStyle = 5;
            item.noMelee = true;
            item.knockBack = 0;
            item.value = 60000;
            item.rare = 5;
            item.UseSound = new Terraria.Audio.LegacySoundStyle(2, 31);
            item.shoot = 10;
            item.autoReuse = true;
            item.shootSpeed = 13f;
            item.useAmmo = AmmoID.Bullet;
        }

        public override bool CanUseItem(Player player)
        {
            return true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY + 6)) * 25f;
            
            position += muzzleOffset;

            Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(3));

            speedX = perturbedSpeed.X;
            speedY = perturbedSpeed.Y;

            return true;
        }

        public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
        {
            if (player.GetModPlayer<PLAYERGLOBAL>().excited)
                mult = 2;

            base.ModifyWeaponDamage(player, ref add, ref mult, ref flat);
        }

        public override float UseTimeMultiplier(Player player)
        {
            if (player.GetModPlayer<PLAYERGLOBAL>().excited)
            {
                return base.UseTimeMultiplier(player) * 1.3f;
            }
            else
            {
                return base.UseTimeMultiplier(player);
            }
        }

        public override bool ConsumeAmmo(Player player)
        {
            int num = (int)(item.useAnimation * UseTimeMultiplier(player)) - item.useAnimation;

                return !(player.itemAnimation < (player.itemAnimationMax) - 2);
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-20, 8);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.IllegalGunParts, 1);
            recipe.AddIngredient(ItemID.Minishark, 1);
            recipe.AddIngredient(ItemID.ClockworkAssaultRifle, 1);
            recipe.AddIngredient(ItemID.SoulofSight, 10);
            recipe.AddIngredient(ItemID.PinkPaint, 3);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override bool GetIfAbilityExists(AbilityType type)
        {
            if (type == AbilityType.W)
                return true;
            return base.GetIfAbilityExists(type);
        }

        public override void Efx(Player player, AbilityType type)
        {
            if (type == AbilityType.W)
            {

            }
        }
    }
}
