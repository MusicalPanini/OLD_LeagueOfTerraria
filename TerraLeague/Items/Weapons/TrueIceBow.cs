using Microsoft.Xna.Framework;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class TrueIceBow : AbilityItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("True Ice Bow");
            Tooltip.SetDefault("");
        }

        public override string GetWeaponTooltip()
        {
            return "Fires a flurry of slowing arrows after every shot that deal 30% of the original arrow's damage";
        }

        public override string GetQuote()
        {
            return "Right between the eyes";
        }

        public override string GetAbilityName(AbilityType type)
        {
            if (type == AbilityType.W)
                return "Volley";
            else
                return base.GetAbilityName(type);
        }

        public override string GetIconTexturePath(AbilityType type)
        {
            if (type == AbilityType.W)
                return "AbilityImages/Volley";
            else
                return base.GetIconTexturePath(type);
        }

        public override string GetAbilityTooltip(AbilityType type)
        {
            if (type == AbilityType.W)
            {
                return "Fire 9 slowing shots in a cone";
            }
            else
            {
                return base.GetAbilityTooltip(type);
            }
        }

        public override int GetAbilityBaseDamage(Player player, AbilityType type)
        {
            if (type == AbilityType.W)
                return (int)System.Math.Round(item.damage * Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().arrowDamageLastStep * 1.25);
            else
                return base.GetAbilityBaseDamage(player, type);
        }

        public override int GetAbilityScalingAmount(Player player, AbilityType type, DamageType dam)
        {
            if (type == AbilityType.W)
            {
                if (dam == DamageType.RNG)
                    return 40;
            }
            return base.GetAbilityScalingAmount(player, type, dam);
        }

        public override int GetBaseManaCost(AbilityType type)
        {
            if (type == AbilityType.W)
                return 30;
            else
                return base.GetBaseManaCost(type);
        }

        public override string GetDamageTooltip(Player player, AbilityType type)
        {
            if (type == AbilityType.W)
                return GetAbilityBaseDamage(player, type) + " + " + GetScalingTooltip(player, type, DamageType.RNG) + " ranged damage per shot";
            else
                return base.GetDamageTooltip(player, type);
        }

        public override int GetRawCooldown(AbilityType type)
        {
            if (type == AbilityType.W)
                return 5;
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
                    Vector2 velocity = TerraLeague.CalcVelocityToMouse(position, 13f);
                    int projType = ProjectileType<TrueIceBow_Volley>();
                    int damage = GetAbilityBaseDamage(player, type) + GetAbilityScalingDamage(player, type, DamageType.RNG);
                    int knockback = 1;

                    int numberProjectiles = 9;
                    float startingAngle = 30;
                    for (int i = 0; i < numberProjectiles; i++)
                    {
                        Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.ToRadians(startingAngle));
                        Projectile.NewProjectile(position, perturbedSpeed, projType, damage, knockback, player.whoAmI);
                        startingAngle -= 7.5f;
                    }

                    SetAnimation(player, 20, 20, position + velocity);
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
            item.damage = 30;
            item.ranged = true;
            item.width = 24;
            item.height = 54;
            item.useAnimation = 25;
            item.useTime = 5;
            item.reuseDelay = 20;
            item.shootSpeed = 10f;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.noMelee = true;
            item.knockBack = 1;
            item.value = 200000;
            item.rare = ItemRarityID.LightRed;
            item.UseSound = SoundID.Item5;
            item.autoReuse = true;
            item.shoot = ProjectileID.PurificationPowder;
            item.useAmmo = AmmoID.Arrow;
        }

        public override bool CanUseItem(Player player)
        {
            return base.CanUseItem(player);
        }



        public override void PickAmmo(Item weapon, Player player, ref int type, ref float speed, ref int damage, ref float knockback)
        {
            base.PickAmmo(weapon, player, ref type, ref speed, ref damage, ref knockback);
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (player.itemAnimation != player.itemAnimationMax - 1)
            {
                type = ProjectileType<TrueIceBow_Flurry>();
                damage = (int)(damage * 0.3);
                knockBack = 0;
                //int numberProjectiles = 5;
                //    int distance = 24;
                //    for (int i = 0; i < numberProjectiles; i++)
                //    {
                //        Vector2 relPosition = new Vector2(0 - (distance * 2) + (i * distance), 0).RotatedBy(TerraLeague.CalcAngle(player.Center, Main.MouseWorld) + MathHelper.PiOver2);
                //        Vector2 position = new Vector2(player.Center.X + relPosition.X, player.Center.Y + relPosition.Y);
                //        Vector2 velocity = TerraLeague.CalcVelocityToMouse(position, 15f);

                //        Projectile.NewProjectile(position, velocity, projType, damage, knockback, player.whoAmI);
                //    }
            }

            return true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<TrueIceChunk>(), 4);
            recipe.AddIngredient(ItemID.HellwingBow, 1);
            recipe.AddIngredient(ItemID.FrostCore, 1);
            recipe.AddTile(TileID.Anvils);
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
                Main.PlaySound(new LegacySoundStyle(2, 5), player.Center);
        }

        public override bool ConsumeAmmo(Player player)
        {
            int num = (int)(item.useAnimation * UseTimeMultiplier(player)) - item.useAnimation;

            return !(player.itemAnimation < (player.itemAnimationMax) - 2);
        }
    }
}
