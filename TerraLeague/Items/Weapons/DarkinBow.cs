using Microsoft.Xna.Framework;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class DarkinBow : AbilityItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Darkin Bow");
            Tooltip.SetDefault("");
        }

        public override string GetWeaponTooltip()
        {
            return "Charge an arrow";
        }

        public override string GetQuote()
        {
            return "The guilty will know agony";
        }

        public override string GetAbilityName(AbilityType type)
        {
            if (type == AbilityType.E)
                return "Rain of Arrows";
            else
                return base.GetAbilityName(type);
        }

        public override string GetIconTexturePath(AbilityType type)
        {
            if (type == AbilityType.E)
                return "AbilityImages/PiercingDarkness";
            else
                return base.GetIconTexturePath(type);
        }

        public override string GetAbilityTooltip(AbilityType type)
        {
            if (type == AbilityType.E)
            {
                return "Fire a hail of arrows";
            }
            else
            {
                return base.GetAbilityTooltip(type);
            }
        }

        public override int GetAbilityBaseDamage(Player player, AbilityType type)
        {
            PLAYERGLOBAL modPlayer = Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>();
            if (type == AbilityType.E)
                return (int)(item.damage * 0.5);
            else
                return base.GetAbilityBaseDamage(player, type);
        }

        public override int GetAbilityScalingAmount(Player player, AbilityType type, DamageType dam)
        {
            PLAYERGLOBAL modPlayer = Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>();
            if (type == AbilityType.E)
            {
                if (dam == DamageType.RNG)
                    return 50;
            }
            return base.GetAbilityScalingAmount(player, type, dam);
        }

        public override int GetBaseManaCost(AbilityType type)
        {
            if (type == AbilityType.E)
                return 30;
            else
                return base.GetBaseManaCost(type);
        }

        public override string GetDamageTooltip(Player player, AbilityType type)
        {
            if (type == AbilityType.E)
                return GetAbilityBaseDamage(player, type) + " + " + GetScalingTooltip(player, type, DamageType.RNG) + " range damage";
            else
                return base.GetDamageTooltip(player, type);
        }

        public override int GetRawCooldown(AbilityType type)
        {
            if (type == AbilityType.E)
                return 6;
            else
                return base.GetRawCooldown(type);
        }

        public override bool CanBeCastWhileUsingItem(AbilityType type)
        {
            return false;
        }

        public override void DoEffect(Player player, AbilityType type)
        {
            if (type == AbilityType.E)
            {
                if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetScaledManaCost(type), true))
                {
                    Vector2 position = player.MountedCenter;
                    int projType = ProjectileType<DarkinArrowRain>();
                    int damage = GetAbilityBaseDamage(player, type) + GetAbilityScalingDamage(player, type, DamageType.RNG);
                    int knockback = 0;

                    int arrows = Main.rand.Next(15, 19);
                    for (int i = 0; i < arrows; i++)
                    {
                        Vector2 velocity = new Vector2(Main.rand.NextFloat(-3, 0), Main.rand.NextFloat(-6, -5)*1.5f);
                        Vector2 velocity2 = new Vector2(Main.rand.NextFloat(0, 3), Main.rand.NextFloat(-6, -5)*1.5f);

                        Projectile proj = Projectile.NewProjectileDirect(position, velocity, projType, damage, knockback, player.whoAmI);
                        proj.extraUpdates = 1;
                        proj = Projectile.NewProjectileDirect(position, velocity2, projType, damage, knockback, player.whoAmI);
                        proj.extraUpdates = 1;
                    }

                    

                    SetAnimation(player, 30, 30, new Vector2(player.MountedCenter.X, player.MountedCenter.Y - 30));
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
            item.damage = 38;
            item.ranged = true;
            item.useStyle = 5;
            item.width = 24;
            item.height = 64;
            item.channel = true;
            item.useTime = 18;
            item.useAnimation = 18;
            item.shootSpeed = 10f;
            item.noMelee = true;
            item.knockBack = 1;
            item.value = 6000;
            item.rare = 4;
            item.shoot = ProjectileType<DarkinBowArrowControl>();
            item.useAmmo = AmmoID.Arrow;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            item.damage = 28;
            if (type == ProjectileID.WoodenArrowFriendly)
                type = ProjectileType<DarkinArrow>();

            Projectile proj = Projectile.NewProjectileDirect(player.Center, Vector2.Zero, ProjectileType<DarkinBowArrowControl>(), damage, knockBack, player.whoAmI, type);
            proj.rotation = new Vector2(speedX, speedY).ToRotation();

            return false;
        }

        public override void AddRecipes()
        {
            //ModRecipe recipe = new ModRecipe(mod);
            //recipe.AddIngredient(ItemType<DamnedSoul>(), 100);
            //recipe.AddIngredient(ItemID.HallowedBar, 16);
            //recipe.AddIngredient(ItemID.Marble, 100);
            //recipe.AddIngredient(ItemID.IllegalGunParts, 1);
            //recipe.AddIngredient(ItemID.SoulofMight, 10);
            //recipe.AddIngredient(ItemID.SoulofLight, 5);
            //recipe.AddIngredient(ItemID.SoulofNight, 5);
            //recipe.AddTile(TileID.MythrilAnvil);
            //recipe.SetResult(this);
            //recipe.AddRecipe();
        }

        public override bool GetIfAbilityExists(AbilityType type)
        {
            if (type == AbilityType.E)
                return true;
            return base.GetIfAbilityExists(type);
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(0, 0);
        }

        public override void Efx(Player player, AbilityType type)
        {
            if (type == AbilityType.E)
            {
                Microsoft.Xna.Framework.Audio.SoundEffectInstance sound = Main.PlaySound(SoundID.Item5, player.Center);
            }
        }
    }
}
