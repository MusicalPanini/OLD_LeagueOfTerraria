using Microsoft.Xna.Framework;
using TerraLeague.Items.Weapons.Abilities;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class TrueIceFlail : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("True Ice Flail");
            Tooltip.SetDefault("");
        }

        string GetWeaponTooltip()
        {
            return "Chance to slow enemies on hit." +
                "\nIf enemy is already slow, freeze them instead." +
                "\nMelee attacks shatter frozen enemies, dealing 10% of their max life as extra damage (Max 200)" +
                "\nShattered enemies cannot be frozen again for 5 seconds";
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.value = 160000;
            item.rare = ItemRarityID.Pink;
            item.noMelee = true;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.useAnimation = 40;
            item.useTime = 40;
            item.knockBack = 6F;
            item.damage = 54;
            item.scale = 1;
            item.noUseGraphic = true;
            item.UseSound = SoundID.Item1;
            item.shootSpeed = 13F;
            item.melee = true;
            item.channel = true;
            item.shoot = ProjectileType<TrueIceFlail_Ball>();

            AbilityItemGLOBAL abilityItem = item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.R, new GlacialPrison(this));
            abilityItem.ChampQuote = "The cold does not forgive";
            abilityItem.getWeaponTooltip = GetWeaponTooltip;
            abilityItem.IsAbilityItem = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<TrueIceChunk>(), 4);
            recipe.AddIngredient(ItemID.Sunfury, 1);
            recipe.AddIngredient(ItemID.FrostCore, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
