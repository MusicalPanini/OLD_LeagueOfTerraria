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
    public class FishBones : AbilityItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fish Bones");
            Tooltip.SetDefault("");
        }

        public override string GetWeaponTooltip()
        {
            return "Kills grant 'EXCITED!'" +
                "\nEXCITED increase firerate";
        }

        public override string GetQuote()
        {
            return "BYE BYE";
        }

        public override string GetAbilityName(AbilityType type)
        {
            if (type == AbilityType.R)
                return "Super Mega Death Rocket!";
            else
                return base.GetAbilityName(type);
        }

        public override string GetIconTexturePath(AbilityType type)
        {
            if (type == AbilityType.R)
                return "AbilityImages/SMDR";
            else
                return base.GetIconTexturePath(type);
        }

        public override string GetAbilityTooltip(AbilityType type)
        {
            if (type == AbilityType.R)
            {
                return "Fire a giant rocket that gains damage the longer it flys";
            }
            else
            {
                return base.GetAbilityTooltip(type);
            }
        }

        public override int GetAbilityBaseDamage(Player player, AbilityType type)
        {
            if (type == AbilityType.R)
                return (int)(Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().rocketDamageLastStep * item.damage * 5);
            else
                return base.GetAbilityBaseDamage(player, type);
        }

        public override int GetAbilityScalingAmount(Player player, AbilityType type, DamageType dam)
        {
            if (type == AbilityType.R)
            {
                if (dam == DamageType.RNG)
                    return 150;
            }
            return base.GetAbilityScalingAmount(player, type, dam);
        }

        public override int GetBaseManaCost(AbilityType type)
        {
            if (type == AbilityType.R)
                return 150;
            else
                return base.GetBaseManaCost(type);
        }

        public override string GetDamageTooltip(Player player, AbilityType type)
        {
            if (type == AbilityType.R)
                return "Up to " + GetAbilityBaseDamage(player, type) + " + " + GetScalingTooltip(player, type, DamageType.RNG) + " ranged damage";
            else
                return base.GetDamageTooltip(player, type);
        }

        public override int GetRawCooldown(AbilityType type)
        {
            if (type == AbilityType.R)
                return 30;
            else
                return base.GetRawCooldown(type);
        }

        public override void DoEffect(Player player, AbilityType type)
        {
            if (type == AbilityType.R)
            {
                if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetScaledManaCost(type), true))
                {
                    Vector2 position = player.MountedCenter;
                    Vector2 velocity = TerraLeague.CalcVelocityToMouse(position, 2.5f);
                    int projType = ProjectileType<SMDR>();
                    int damage = GetAbilityBaseDamage(player, type) + GetAbilityScalingDamage(player, type, DamageType.RNG);
                    int knockback = 6;

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
            item.width = 32;
            item.height = 32;
            item.damage = 35;
            item.ranged = true;
            item.useAnimation = 30;
            item.useTime = 30;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.noMelee = true;
            item.knockBack = 3;
            item.value = 120000;
            item.rare = ItemRarityID.Yellow;
            item.UseSound = SoundID.Item11;
            item.autoReuse = true;
            item.shoot = ProjectileID.RocketI;
            item.shootSpeed = 6;
            item.useAmmo = AmmoID.Rocket;
        }

        public override bool CanUseItem(Player player)
        {
            item.damage = 35;
            item.useAnimation = 30;
            item.useTime = 30;
            return true;
        }

        
        
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            type = item.shoot + player.inventory.Where(x => x.ammo == AmmoID.Rocket).First().shoot;

            position.Y -= 8;

            return true;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-65, -15);
        }

        public void SetStatsPostShoot(Player player)
        {
            if (player.GetModPlayer<PLAYERGLOBAL>().excited)
            {
                item.damage = (int)(15 * 1.5);
                item.useAnimation = 15;
                item.useTime = 15;
            }
            else
            {
                item.useAnimation = 35;
                item.useTime = 35;
                item.damage = (int)(15);
            }
        }

        public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
        {
            if (player.GetModPlayer<PLAYERGLOBAL>().excited)
                mult = 1.5f;

            base.ModifyWeaponDamage(player, ref add, ref mult, ref flat);
        }

        public override float UseTimeMultiplier(Player player)
        {
            if (player.GetModPlayer<PLAYERGLOBAL>().excited)
            {
                return base.UseTimeMultiplier(player) * 1.5f;
            }
            else
            {
                return base.UseTimeMultiplier(player);
            }
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.IllegalGunParts, 1);
            recipe.AddIngredient(ItemID.Megashark, 1);
            recipe.AddIngredient(ItemID.RocketLauncher, 1);
            recipe.AddIngredient(ItemID.SharkFin, 5);
            recipe.AddIngredient(ItemID.SoulofMight, 20);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override bool GetIfAbilityExists(AbilityType type)
        {
            if (type == AbilityType.R)
                return true;
            return base.GetIfAbilityExists(type);
        }

        public override void Efx(Player player, AbilityType type)
        {
            if (type == AbilityType.R)
                Main.PlaySound(new LegacySoundStyle(2, 11), player.Center);
        }
    }
}
