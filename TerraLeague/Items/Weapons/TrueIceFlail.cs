using Microsoft.Xna.Framework;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class TrueIceFlail : AbilityItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("True Ice Flail");
            Tooltip.SetDefault("");
        }

        public override string GetWeaponTooltip()
        {
            return "Chance to slow enemies on hit." +
                "\nIf enemy is already slow, freeze them instead." +
                "\nMelee attacks shatter frozen enemies, dealing 10% of their max life as extra damage (Max 200)" +
                "\nShattered enemies cannot be frozen again for 5 seconds";
        }

        public override string GetQuote()
        {
            return "The cold does not forgive";
        }

        public override string GetAbilityName(AbilityType type)
        {
            if (type == AbilityType.R)
                return "Glacial Prison";
            else
                return base.GetAbilityName(type);
        }

        public override string GetIconTexturePath(AbilityType type)
        {
            if (type == AbilityType.R)
                return "AbilityImages/GlacialPrison";
            else
                return base.GetIconTexturePath(type);
        }

        public override string GetAbilityTooltip(AbilityType type)
        {
            if (type == AbilityType.R)
            {
                return "Throw a True Ice bola that creates a storm and freezes the hit enemy." +
                    "\nCan even freeze recently shattered enemies" +
                    "\nAfter 2 seconds, the storm erupts.";
            }
            else
            {
                return base.GetAbilityTooltip(type);
            }
        }

        public override int GetAbilityBaseDamage(Player player, AbilityType type)
        {
            if (type == AbilityType.R)
                return (int)(item.damage * 2);
            else
                return base.GetAbilityBaseDamage(player, type);
        }

        public override int GetAbilityScalingAmount(Player player, AbilityType type, DamageType dam)
        {
            if (type == AbilityType.R)
            {
                if (dam == DamageType.MAG)
                    return 40;
            }
            return base.GetAbilityScalingAmount(player, type, dam);
        }

        public override int GetBaseManaCost(AbilityType type)
        {
            if (type == AbilityType.R)
                return 100;
            else
                return base.GetBaseManaCost(type);
        }

        public override string GetDamageTooltip(Player player, AbilityType type)
        {
            if (type == AbilityType.R)
                return GetAbilityBaseDamage(player, type) + " + " + GetScalingTooltip(player, type, DamageType.MAG) + " magic damage";
            else
                return base.GetDamageTooltip(player, type);
        }

        public override int GetRawCooldown(AbilityType type)
        {
            if (type == AbilityType.R)
                return 90;
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
                    Vector2 velocity = TerraLeague.CalcVelocityToMouse(position, 15f);
                    int projType = ProjectileType<GlacialPrisonBola>();
                    int damage = GetAbilityBaseDamage(player, type) + GetAbilityScalingDamage(player, type, DamageType.MAG);
                    int knockback = 0;

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
            item.width = 32;
            item.height = 32;
            item.value = 72000;
            item.rare = ItemRarityID.LightRed;

            item.noMelee = true;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.useAnimation = 40;
            item.useTime = 40;
            item.knockBack = 6F;
            item.damage = 30;
            item.scale = 1;
            item.noUseGraphic = true;
            item.UseSound = SoundID.Item1;
            item.shootSpeed = 13F;
            item.melee = true;
            item.channel = true;
            item.shoot = ProjectileType<TrueIceFlailP>();
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            return base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<TrueIceChunk>(), 4);
            recipe.AddIngredient(ItemID.Sunfury, 1);
            recipe.AddTile(TileID.Anvils);
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
                Main.PlaySound(SoundID.Item1, player.Center);
        }
    }
}
