using Microsoft.Xna.Framework;
using TerraLeague.Buffs;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class DarkSovereignsStaff : AbilityItem
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.MinionTargettingFeature[item.type] = true;
            DisplayName.SetDefault("Dark Sovereigns Staff");
            Tooltip.SetDefault("");
            base.SetStaticDefaults();
        }

        public override string GetWeaponTooltip()
        {
            return "Create a sphere of pure negative emotions to fight for you";
        }

        public override string GetQuote()
        {
            return "I will not be restrained";
        }

        public override string GetAbilityName(AbilityType type)
        {
            if (type == AbilityType.R)
                return "Unleashed Power";
            else
                return base.GetAbilityName(type);
        }

        public override string GetIconTexturePath(AbilityType type)
        {
            if (type == AbilityType.R)
                return "AbilityImages/UnleashedPower";
            else
                return base.GetIconTexturePath(type);
        }

        public override string GetAbilityTooltip(AbilityType type)
        {
            if (type == AbilityType.R)
            {
                return "Launch " + Main.LocalPlayer.maxMinions * 2 + " dark spheres at a targeted enemy";
            }
            else
            {
                return base.GetAbilityTooltip(type);
            }
        }

        public override int GetAbilityBaseDamage(Player player, AbilityType type)
        {
            if (type == AbilityType.R)
                return (int)(1.5 * item.damage);
            else
                return base.GetAbilityBaseDamage(player, type);
        }

        public override int GetAbilityScalingAmount(Player player, AbilityType type, DamageType dam)
        {
            PLAYERGLOBAL modPlayer = Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>();
            if (type == AbilityType.R)
            {
                if (dam == DamageType.SUM)
                    return 45;
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
                return GetAbilityBaseDamage(player, type) + " + " + GetScalingTooltip(player, type, DamageType.SUM) + " summon damage per sphere";
            else
                return base.GetDamageTooltip(player, type);
        }

        public override bool CanBeCastWhileUsingItem(AbilityType type)
        {
            if (type == AbilityType.R)
                return true;
            else
                return false;
        }

        public override int GetRawCooldown(AbilityType type)
        {
            if (type == AbilityType.R)
                return 75;
            else
                return base.GetRawCooldown(type);
        }

        public override void DoEffect(Player player, AbilityType type)
        {
            if (type == AbilityType.R)
            {
                int target = TerraLeague.NPCMouseIsHovering();
                if (target != -1)
                {
                    if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetScaledManaCost(type), true))
                    {
                        DoEfx(player, type);
                        int projType = ProjectileType<DarkSovereignsStaff_UnleashedPower>();
                        int damage = GetAbilityBaseDamage(player, type) + GetAbilityScalingDamage(player, type, DamageType.SUM);
                        int knockback = 3;

                        int numberProjectiles = player.maxMinions * 2;
                        for (int i = 0; i < numberProjectiles; i++)
                        {
                            Vector2 position = player.MountedCenter;
                            Vector2 velocity = Vector2.Zero;

                            Projectile.NewProjectile(position, velocity, projType, damage, knockback, player.whoAmI, target, i);
                        }
                        SetCooldowns(player, type);
                    }
                }
            }
            else
            {
                base.DoEffect(player, type);
            }
        }

        public override void SetDefaults()
        {
            item.damage = 20;
            item.summon = true;
            item.mana = 20;
            item.width = 48;
            item.height = 48;
            item.useTime = 36;
            item.useAnimation = 36;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.noMelee = true;
            item.knockBack = 1;
            item.value = 100000;
            item.rare = ItemRarityID.Pink;
            item.UseSound = new LegacySoundStyle(2, 113);
            item.shoot = ProjectileType<DarkSovereignsStaff_DarkSphere>();
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            position = Main.MouseWorld;

            if (player.altFunctionUse != 2)
            {
                return true;
            }

            return false;
        }

        public override bool UseItem(Player player)
        {

            return base.UseItem(player);
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {

            }
            else
            {
                player.AddBuff(BuffType<DarkSphere>(), 2);
            }
            return base.CanUseItem(player);
        }

        public override bool GetIfAbilityExists(AbilityType type)
        {
            if (type == AbilityType.R)
                return true;
            return base.GetIfAbilityExists(type);
        }

        public override void Efx(Player player, AbilityType type)
        {
            if (type == AbilityType.W)
                Main.PlaySound(new LegacySoundStyle(2, 11), player.Center);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<HarmonicBar>(), 16);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}