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
    public class StarForgersCore : AbilityItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Star Forger");
            Tooltip.SetDefault("");
            base.SetStaticDefaults();
        }

        public override string GetWeaponTooltip()
        {
            return "Become the center of the Universe!";
        }

        public override string GetQuote()
        {
            return "Now we're playing with star fire!";
        }

        public override string GetAbilityName(AbilityType type)
        {
            if (type == AbilityType.W)
                return "Celestial Expansion";
            else
                return base.GetAbilityName(type);
        }

        public override string GetIconTexturePath(AbilityType type)
        {
            if (type == AbilityType.W)
                return "AbilityImages/CelestialExpansion";
            else
                return base.GetIconTexturePath(type);
        }

        public override string GetAbilityTooltip(AbilityType type)
        {
            if (type == AbilityType.W)
            {
                return "Expands the range and size of your stars and increases all minion damage output";
            }
            else
            {
                return base.GetAbilityTooltip(type);
            }
        }

        public override int GetAbilityBaseDamage(Player player, AbilityType type)
        {
            if (type == AbilityType.W)
                return (int)20;
            else
                return base.GetAbilityBaseDamage(player, type);
        }

        public override int GetAbilityScalingAmount(Player player, AbilityType type, DamageType dam)
        {
            if (type == AbilityType.W)
            {
                if (dam == DamageType.SUM)
                    return 15;
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
                return GetAbilityBaseDamage(player, type) + " + " + GetScalingTooltip(player, type, DamageType.SUM) + " flat increased minion damage";
            else
                return base.GetDamageTooltip(player, type);
        }

        public override string GetTooltip(AbilityType type)
        {
            return (GetDamageTooltip(Main.LocalPlayer, type) == "" ? "" : GetDamageTooltip(Main.LocalPlayer, type) + "\n") +
            (GetBaseManaCost(type) == 0 ? "" : "Uses " + GetScaledManaCost(type) + " mana + 30 per second\n") +
            (GetAbilityTooltip(type) == "" ? "" : GetAbilityTooltip(type));
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
                return 1;
            else
                return base.GetRawCooldown(type);
        }

        public override void DoEffect(Player player, AbilityType type)
        {
            if (type == AbilityType.W)
            {
                if (CheckIfNotOnCooldown(player, type) && player.HasBuff(BuffType<CelestialExpansion>()))
                {
                    player.ClearBuff(BuffType<CelestialExpansion>());
                    DoEfx(player, type);
                    SetCooldowns(player, type);
                }
                else if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetScaledManaCost(type), true))
                {
                    player.AddBuff(BuffType<CelestialExpansion>(), 61);
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
            item.summon = true;
            item.mana = 20;
            item.width = 40;
            item.height = 42;
            item.useTime = 36;
            item.useAnimation = 36;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.noMelee = true;
            item.knockBack = 1;
            item.value = 40000;
            item.rare = ItemRarityID.Blue;
            item.UseSound = new LegacySoundStyle(2, 113);
            item.shoot = ProjectileType<ForgedStar>();
        }

        public override bool AltFunctionUse(Player player)
        {
            return false;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            player.AddBuff(BuffType<CenterOfTheUniverse>(), 3);
            position = player.MountedCenter;
            Projectile.NewProjectileDirect(position, Vector2.Zero, type, damage, knockBack, player.whoAmI, player.ownedProjectileCounts[type] + 1);

            return false;
        }

        public override bool UseItem(Player player)
        {

            return base.UseItem(player);
        }

        public override bool CanUseItem(Player player)
        {
            return base.CanUseItem(player);
        }

        public override bool GetIfAbilityExists(AbilityType type)
        {
            if (type == AbilityType.W)
                return true;
            return base.GetIfAbilityExists(type);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<CelestialBar>(), 12);
            recipe.AddIngredient(ItemID.FallenStar, 12);
            recipe.AddIngredient(ItemID.SoulofLight, 6);
            recipe.AddIngredient(ItemID.SoulofNight, 6);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void Efx(Player player, AbilityType type)
        {
            if (type == AbilityType.W)
            {
                var efx = Main.PlaySound(new LegacySoundStyle(2, 45), player.Center);
                if (efx != null)
                    efx.Pitch = -0.5f;
            }
        }
    }
}

