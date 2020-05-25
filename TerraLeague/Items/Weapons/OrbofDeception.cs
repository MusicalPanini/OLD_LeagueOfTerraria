using Microsoft.Xna.Framework;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class OrbofDeception : AbilityItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Orb of Deception");
            Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(8, 4));

            Tooltip.SetDefault("");
        }

        public override string GetWeaponTooltip()
        {
            return "Send out a returning orb of magic that crits on the way back";
        }

        public override string GetQuote()
        {
            return "Let's have some real fun";
        }

        public override string GetAbilityName(AbilityType type)
        {
            if (type == AbilityType.W)
                return "Fox-Fire";
            else
                return base.GetAbilityName(type);
        }

        public override string GetIconTexturePath(AbilityType type)
        {
            if (type == AbilityType.W)
                return "AbilityImages/FoxFire";
            else
                return base.GetIconTexturePath(type);
        }

        public override string GetAbilityTooltip(AbilityType type)
        {
            if (type == AbilityType.W)
            {
                return "Summon 3 spectral flames that orbit around you";
            }
            else
            {
                return base.GetAbilityTooltip(type);
            }
        }

        public override int GetAbilityBaseDamage(Player player, AbilityType type)
        {
            if (type == AbilityType.W)
                return (int)System.Math.Round(item.damage * 2.5);
            else
                return base.GetAbilityBaseDamage(player, type);
        }

        public override int GetAbilityScalingAmount(Player player, AbilityType type, DamageType dam)
        {
            if (type == AbilityType.Q)
            {
                if (dam == DamageType.MAG)
                    return 30;
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
                return GetAbilityBaseDamage(player, type) + " + " + GetScalingTooltip(player, type, DamageType.MAG) + " magic damage";
            else
                return base.GetDamageTooltip(player, type);
        }

        public override bool CanBeCastWhileUsingItem(AbilityType type)
        {
            if (type == AbilityType.W)
                return true;
            else
                return false;
        }

        public override int GetRawCooldown(AbilityType type)
        {
            if (type == AbilityType.W)
                return 10;
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
                    Vector2 velocity = Vector2.Zero;
                    int projType = ProjectileType<OrbofDeception_FoxFire>();
                    int damage = GetAbilityBaseDamage(player, type) + GetAbilityScalingDamage(player, type, DamageType.MAG);

                    int knockback = 1;

                    for (int i = 1; i < 4; i++)
                    {
                        Projectile.NewProjectile(position, velocity, projType, damage, knockback, player.whoAmI, ((MathHelper.TwoPi * i) / 3));
                    }
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
            item.damage = 28;
            item.width = 30;
            item.height = 30;
            item.magic = true;
            item.useTime = 25;
            item.useAnimation = 25;
            item.useStyle = ItemUseStyleID.Stabbing;
            item.knockBack = 1;
            item.mana = 20;
            item.value = 54000;
            item.rare = ItemRarityID.Orange;
            item.UseSound = new LegacySoundStyle(2, 8, Terraria.Audio.SoundType.Sound);
            item.shootSpeed = 15f;
            item.shoot = ProjectileType<OrbofDeception_Orb>();
            item.noMelee = true;
            item.useTurn = true;
            item.autoReuse = true;
            item.noUseGraphic = true;
        }

        public override bool CanUseItem(Player player)
        {


            return player.ownedProjectileCounts[ProjectileType<OrbofDeception_Orb>()] < 1;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            return true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<ManaBar>(), 16);
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
            {

            }
        }
    }
}
