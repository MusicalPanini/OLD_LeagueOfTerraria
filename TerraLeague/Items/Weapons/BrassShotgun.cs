using Microsoft.Xna.Framework;
using System.Linq;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class BrassShotgun : AbilityItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Destiny");
            Tooltip.SetDefault("");
        }

        public override string GetQuote()
        {
            return "Dead man walkin'";
        }

        public override string GetAbilityName(AbilityType type)
        {
            if (type == AbilityType.Q)
                return "End of the Line";
            else
                return base.GetAbilityName(type);
        }

        public override string GetIconTexturePath(AbilityType type)
        {
            if (type == AbilityType.Q)
                return "AbilityImages/EndoftheLine";
            else
                return base.GetIconTexturePath(type);
        }

        public override string GetAbilityTooltip(AbilityType type)
        {
            if (type == AbilityType.Q)
            {
                return "Fire an explosive round that detonates on contact with the ground." +
                    "\nThe explosions will home back to you";
            }
            else
            {
                return base.GetAbilityTooltip(type);
            }
        }

        public override int GetAbilityBaseDamage(Player player, AbilityType type)
        {
            if (type == AbilityType.Q)
                return (int)(2 * item.damage);
            else
                return base.GetAbilityBaseDamage(player, type);
        }

        public override int GetAbilityScalingAmount(Player player, AbilityType type, DamageType dam)
        {
            if (type == AbilityType.Q)
            {
                if (dam == DamageType.RNG)
                    return 100;
            }
            return base.GetAbilityScalingAmount(player, type, dam);
        }

        public override int GetBaseManaCost(AbilityType type)
        {
            if (type == AbilityType.Q)
                return 20;
                return base.GetBaseManaCost(type);
        }

        public override string GetDamageTooltip(Player player, AbilityType type)
        {
            if (type == AbilityType.Q)
                return GetAbilityBaseDamage(player, type) + " + " + GetScalingTooltip(player, type, DamageType.RNG) + " ranged damage";
            else
                return base.GetDamageTooltip(player, type);
        }

        public override int GetRawCooldown(AbilityType type)
        {
            if (type == AbilityType.Q)
                return 10;
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
                    Vector2 position = player.MountedCenter;
                    Vector2 velocity = TerraLeague.CalcVelocityToMouse(position, 12);
                    int projType = ProjectileType<EndoftheLine>();
                    int damage = GetAbilityBaseDamage(player, type) + GetAbilityScalingDamage(player, type, DamageType.RNG);
                    int knockback = 3;

                    SetAnimation(player, 20, 20, position + velocity);
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
            item.damage = 8;
            item.ranged = true;
            item.width = 50;
            item.height = 24;
            item.useAnimation = 60;
            item.useTime = 60;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.noMelee = true;
            item.knockBack = 6;
            item.value = 3500;
            item.rare = ItemRarityID.Green;
            item.UseSound = new LegacySoundStyle(2,36);
            item.shoot = ProjectileID.PurificationPowder;
            item.shootSpeed = 6f;
            item.useAmmo = AmmoID.Bullet;
            
        }

        public override bool CanUseItem(Player player)
        {
            return base.CanUseItem(player);
        }
        
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
            int numberProjectiles = 4;
            for (int i = 0; i < numberProjectiles; i++)
            {
                Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(12));
                Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
            }
            return false;
		}

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(0, 0);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<BrassBar>(), 14);
            recipe.AddTile(TileID.Anvils);
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
