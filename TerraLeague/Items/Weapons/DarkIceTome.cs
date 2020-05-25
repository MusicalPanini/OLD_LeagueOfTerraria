using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using TerraLeague.Projectiles;
using Microsoft.Xna.Framework;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class DarkIceTome : AbilityItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dark Ice Tome");
            Tooltip.SetDefault("");
        }

        public override string GetWeaponTooltip()
        {
            return "Launch a shard of ice that splits on hit";
        }

        public override string GetQuote()
        {
            return "I will bury the world in ice";
        }

        public override string GetAbilityName(AbilityType type)
        {
            if (type == AbilityType.W)
                return "Ring of Frost";
            return base.GetAbilityName(type);
        }

        public override string GetIconTexturePath(AbilityType type)
        {
            if (type == AbilityType.W)
                return "AbilityImages/RingOfFrost";
            return base.GetIconTexturePath(type);
        }

        public override string GetAbilityTooltip(AbilityType type)
        {
            if (type == AbilityType.W)
            {
                return "Slow all nearby enemies in a cold burst";
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
            PLAYERGLOBAL modPlayer = Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>();
            if (type == AbilityType.W)
            {
                if (dam == DamageType.MAG)
                    return 50;
            }
            return base.GetAbilityScalingAmount(player, type, dam);
        }

        public override int GetBaseManaCost(AbilityType type)
        {
            if (type == AbilityType.W)
                return 75;
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
                return 12;
            else
                return base.GetRawCooldown(type);
        }

        public override void DoEffect(Player player, AbilityType type)
        {
            if (type == AbilityType.W)
            {
                if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetScaledManaCost(type), true))
                {
                    Vector2 position = player.Center;
                    Vector2 velocity = Vector2.Zero;
                    int projType = ProjectileType<DarkIceTome_FrostRing>();
                    int damage = GetAbilityBaseDamage(player, type) + GetAbilityScalingDamage(player, type, DamageType.MAG);
                    int knockback = 0;

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
            item.damage = 45;
            item.noMelee = true;
            item.magic = true;
            item.mana = 14;
            item.rare = ItemRarityID.LightRed;
            item.value = 72000;
            item.width = 28;
            item.height = 32;
            item.useTime = 45;
            item.useAnimation = 45;
            item.knockBack = 2;
            item.UseSound = new Terraria.Audio.LegacySoundStyle(2,8);
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.shootSpeed = 16;
            item.shoot = ProjectileType<DarkIceTome_IceShard>();
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            return base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<TrueIceChunk>(), 4);
            recipe.AddIngredient(ItemID.DemonScythe, 1);
            recipe.AddTile(TileID.DemonAltar);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override bool GetIfAbilityExists(AbilityType type)
        {
            if (type == AbilityType.W)
                return true;
            return base.GetIfAbilityExists(type);
        }
    }
}
