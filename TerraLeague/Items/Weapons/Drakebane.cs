using Microsoft.Xna.Framework;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class Drakebane : AbilityItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Drakebane");
            Tooltip.SetDefault("");
        }

        public override string GetQuote()
        {
            return "Righteous retribution!";
        }

        public override string GetAbilityName(AbilityType type)
        {
            if (type == AbilityType.E)
                return "Demacian Standard";
            else
                return base.GetAbilityName(type);
        }

        public override string GetIconTexturePath(AbilityType type)
        {
            if (type == AbilityType.E)
                return "AbilityImages/DemacianStandard";
            else
                return base.GetIconTexturePath(type);
        }

        public override string GetAbilityTooltip(AbilityType type)
        {
            if (type == AbilityType.E)
            {
                return "Drop a Demacian flag from the sky that gives near by allies attack speed";
            }
            else
            {
                return base.GetAbilityTooltip(type);
            }
        }

        public override int GetAbilityBaseDamage(Player player, AbilityType type)
        {
            if (type == AbilityType.E)
                return (int)(2 * item.damage);
            else
                return base.GetAbilityBaseDamage(player, type);
        }

        public override int GetBaseManaCost(AbilityType type)
        {
            if (type == AbilityType.E)
                return 12;
            else
                return base.GetBaseManaCost(type);
        }

        public override string GetDamageTooltip(Player player, AbilityType type)
        {
            if (type == AbilityType.E)
                return GetAbilityBaseDamage(player, type) + " + " + GetScalingTooltip(player, type, DamageType.MAG) + " magic damage";
            else
                return base.GetDamageTooltip(player, type);
        }

        public override string GetTooltip(AbilityType type)
        {
            return base.GetTooltip(type);
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
                return 0;
            else
                return base.GetRawCooldown(type);
        }

        public override void DoEffect(Player player, AbilityType type)
        {
            if (type == AbilityType.W)
            {
                if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetScaledManaCost(type), true))
                {
                    PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

                    player.AddBuff(BuffID.Swiftness, 360);
                    modPlayer.AddShield(GetAbilityBaseDamage(player, type), 360, new Color(181,77,177), ShieldType.Basic);
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
            item.damage = 26;
            item.width = 32;
            item.height = 32;
            item.melee = true;
            item.useTime = 30;
            item.useAnimation = 30;
            item.useStyle = 5;
            item.knockBack = 2;
            item.value = 10000;
            item.rare = 2;
            item.UseSound = new LegacySoundStyle(2, 101);
            item.shootSpeed = 1f;
            item.shoot = ProjectileType<DrakebaneProj>();
            item.noMelee = true;
            item.noUseGraphic = true;
        }

        public override bool CanUseItem(Player player)
        {
            item.damage = 26;
            if (player.ownedProjectileCounts[ProjectileType<DrakebaneProj>()] < 1)
                return base.CanUseItem(player);
            return false;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.AntlionMandible, 6);
            recipe.AddIngredient(ItemID.Amethyst, 1);
            recipe.AddIngredient(ItemType<Sunstone>(), 10);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override bool GetIfAbilityExists(AbilityType type)
        {
            if (type == AbilityType.E)
                return false;
            return base.GetIfAbilityExists(type);
        }

        public override void Efx(Player player, AbilityType type)
        {
            base.Efx(player, type);
        }
    }
}
