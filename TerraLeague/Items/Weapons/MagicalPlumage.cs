using Microsoft.Xna.Framework;
using TerraLeague.Buffs;
using TerraLeague.Items.Ammo;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class MagicalPlumage : AbilityItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Magical Plumage");
            Tooltip.SetDefault("");
        }

        public override string GetWeaponTooltip()
        {
            return "";
        }

        public override string GetQuote()
        {
            return "Feathers fly!";
        }

        public override string GetAbilityName(AbilityType type)
        {
            if (type == AbilityType.W)
                return "Deadly Plumage";
            else
                return base.GetAbilityName(type);
        }

        public override string GetIconTexturePath(AbilityType type)
        {
            if (type == AbilityType.W)
                return "AbilityImages/DeadlyPlumage";
            else
                return base.GetIconTexturePath(type);
        }

        public override string GetAbilityTooltip(AbilityType type)
        {
            if (type == AbilityType.W)
            {
                return "Gain 25% movement speed and ranged rate of fire";
            }
            else
            {
                return base.GetAbilityTooltip(type);
            }
        }

        public override int GetAbilityBaseDamage(Player player, AbilityType type)
        {
            if (type == AbilityType.W)
                return 0;
            else
                return base.GetAbilityBaseDamage(player, type);
        }

        public override int GetBaseManaCost(AbilityType type)
        {
            if (type == AbilityType.W)
                return 60;
            else
                return base.GetBaseManaCost(type);
        }

        public override string GetDamageTooltip(Player player, AbilityType type)
        {
            if (type == AbilityType.W)
                return "";
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
                return 18;
            else
                return base.GetRawCooldown(type);
        }

        public override void DoEffect(Player player, AbilityType type)
        {
            if (type == AbilityType.W)
            {
                if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetScaledManaCost(type), true))
                {
                    player.AddBuff(BuffType<DeadlyPlumage>(), 60 * 5);
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
            item.damage = 15;
            item.width = 32;
            item.height = 32;
            item.ranged = true;
            item.useTime = 22;
            item.useAnimation = 22;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.knockBack = 1;
            item.value = 54000;
            item.rare = ItemRarityID.Orange;
            item.UseSound = new LegacySoundStyle(2, 19, Terraria.Audio.SoundType.Sound);
            item.shootSpeed = 20f;
            item.shoot = ProjectileID.PurificationPowder;
            item.useAmmo = ItemType<RazorFeather>();
            item.noMelee = true;
            item.useTurn = true;
            item.autoReuse = true;
            item.noUseGraphic = true;
        }

        public override bool CanUseItem(Player player)
        {
            return base.CanUseItem(player);
        }

        public override float UseTimeMultiplier(Player player)
        {
            return 1;
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
