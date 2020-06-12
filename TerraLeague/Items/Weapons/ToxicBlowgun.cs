using Microsoft.Xna.Framework;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class ToxicBlowgun : AbilityItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Toxic Blowgun");
            Tooltip.SetDefault("");
        }

        public override string GetWeaponTooltip()
        {
            return "";
        }

        public override string GetQuote()
        {
            return "Never underestimate the power of the Scout's code";
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
                return (int)(50);
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
                    Vector2 position = player.MountedCenter;
                    Vector2 velocity = TerraLeague.CalcVelocityToMouse(position, 8f);
                    int projType = ProjectileType<ToxicBlowgun_NoxiousTrap>();
                    int damage = GetAbilityBaseDamage(player, type) + GetAbilityScalingDamage(player, type, DamageType.SUM);
                    int knockback = 0;

                    Projectile.NewProjectile(position, velocity, projType, damage, knockback, player.whoAmI);
                    Projectile.NewProjectile(position, velocity * 1.5f, projType, damage, knockback, player.whoAmI);
                    Projectile.NewProjectile(position, velocity * 0.5f, projType, damage, knockback, player.whoAmI);

                    SetAnimation(player, 20, 20, position + velocity);
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
            item.CloneDefaults(ItemID.Blowgun);
            item.damage = 24;
            item.ranged = true;
            item.width = 24;
            item.height = 54;
            item.useAnimation = 35;
            item.useTime = 35;
            item.shootSpeed = 10f;
            item.noMelee = true;
            item.knockBack = 1;
            item.value = 200000;
            item.rare = ItemRarityID.Lime;
            item.autoReuse = true;
            item.shoot = ProjectileID.PurificationPowder;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            position = new Vector2(position.X, position.Y - 6);

            return true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Blowgun, 1);
            recipe.AddIngredient(ItemID.VialofVenom, 10);
            recipe.AddIngredient(ItemID.Mushroom, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override bool GetIfAbilityExists(AbilityType type)
        {
            if (type == AbilityType.R || type == AbilityType.E)
                return true;
            return base.GetIfAbilityExists(type);
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(6, -10);
        }

        public override void Efx(Player player, AbilityType type)
        {
            if (type == AbilityType.R)
                Main.PlaySound(SoundID.Item1, player.Center);
        }
    }
}
