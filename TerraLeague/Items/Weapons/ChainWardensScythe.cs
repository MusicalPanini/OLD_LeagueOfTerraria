using Microsoft.Xna.Framework;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class ChainWardensScythe : AbilityItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Chain Warden's Scythe");
            Tooltip.SetDefault("");
        }

        public override string GetWeaponTooltip()
        {
            return "";
        }

        public override string GetQuote()
        {
            return "What delightful agony we shall inflict";
        }

        public override string GetAbilityName(AbilityType type)
        {
            if (type == AbilityType.W)
                return "Dark Passage";
            else
                return base.GetAbilityName(type);
        }

        public override string GetIconTexturePath(AbilityType type)
        {
            if (type == AbilityType.W)
                return "AbilityImages/DarkPassage";
            else
                return base.GetIconTexturePath(type);
        }

        public override string GetAbilityTooltip(AbilityType type)
        {
            if (type == AbilityType.W)
            {
                return "Throw a lantern at the target location and shield all near by players." +
                    "Ally may also grab the lantern and be brought to your location.";
            }
            else
            {
                return base.GetAbilityTooltip(type);
            }
        }

        public override int GetAbilityBaseDamage(Player player, AbilityType type)
        {
            if (type == AbilityType.W)
                return (int)(item.damage);
            else
                return base.GetAbilityBaseDamage(player, type);
        }

        public override int GetAbilityScalingAmount(Player player, AbilityType type, DamageType dam)
        {
            if (type == AbilityType.W)
            {
                if (dam == DamageType.MEL)
                    return 20;
            }
            return base.GetAbilityScalingAmount(player, type, dam);
        }

        public override int GetBaseManaCost(AbilityType type)
        {
            if (type == AbilityType.W)
                return 40;
            else
                return base.GetBaseManaCost(type);
        }

        public override string GetDamageTooltip(Player player, AbilityType type)
        {
            if (type == AbilityType.W)
                return GetAbilityBaseDamage(player, type) + " + " + GetScalingTooltip(player, type, DamageType.MEL) + " shielding";
            else
                return base.GetDamageTooltip(player, type);
        }

        public override int GetRawCooldown(AbilityType type)
        {
            if (type == AbilityType.W)
                return 17;
            else
                return base.GetRawCooldown(type);
        }

        public override void DoEffect(Player player, AbilityType type)
        {
            if (type == AbilityType.W)
            {
                if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetScaledManaCost(type), true))
                {
                    Vector2 position = Main.MouseWorld;
                    
                    if (player.Distance(position) > 700)
                    {
                        Vector2 temp = (Main.MouseWorld - player.MountedCenter);
                        temp.Normalize();
                        position = (temp * 700) + player.MountedCenter;
                    }

                    Vector2 velocity = Vector2.Zero;
                    int projType = ProjectileType<Lantern>();
                    int damage = GetAbilityBaseDamage(player, type) + GetAbilityScalingDamage(player, type, DamageType.MEL);
                    int knockback = 0;

                    SetAnimation(player, 10, 10, position);
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
            item.width = 50;
            item.height = 46;
            item.value = 54000;
            item.rare = 3;

            item.noMelee = true;
            item.useStyle = 5;
            item.useAnimation = 40;
            item.useTime = 40;
            item.knockBack = 6F;
            item.damage = 30;
            item.scale = 1;
            item.noUseGraphic = true;
            item.UseSound = SoundID.Item1;
            item.shootSpeed = 17F;
            item.melee = true;
            item.shoot = ProjectileType<ChainWardensScytheProj>();
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            return base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<DamnedSoul>(), 50);
            recipe.AddRecipeGroup("TerraLeague:DemonGroup", 16);
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
                Main.PlaySound(SoundID.Item1, player.Center);
        }
    }
}
