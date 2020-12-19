using Microsoft.Xna.Framework;
using TerraLeague.Items.Weapons.Abilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class PetriciteCrossbow : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Petricite Crossbow");
            Tooltip.SetDefault("");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            item.damage = 14;
            item.ranged = true;
            item.noMelee = true;
            item.width = 56;
            item.height = 24;
            item.useTime = 24;
            item.useAnimation = 24;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.knockBack = 1f;
            item.value = 2400;
            item.rare = ItemRarityID.Green;
            item.shootSpeed = 10f;
            item.autoReuse = true;
            item.UseSound = SoundID.Item5;
            item.shoot = ProjectileID.PurificationPowder;
            item.useAmmo = AmmoID.Arrow;

            AbilityItemGLOBAL abilityItem = item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.W, new HeightenedSenses(this));
            abilityItem.ChampQuote = "Valor, to me!";
            abilityItem.IsAbilityItem = true;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, 0);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<Petricite>(), 16);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
