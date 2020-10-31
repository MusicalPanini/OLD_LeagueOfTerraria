using Microsoft.Xna.Framework;
using TerraLeague.Items.Weapons.Abilities;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
	public class TideCallerStaff : AbilityItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tide Caller Staff");
            Tooltip.SetDefault("");
            Item.staff[item.type] = false;
        }

        public override string GetWeaponTooltip()
        {
            return "";
        }

        public override string GetQuote()
        {
            return "I decide what the tide will bring";
        }

        public override void SetDefaults()
        {
            item.damage = 15;
            item.mana = 4;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.magic = true;
            item.useTime = 25;
            item.useAnimation = 25;
            item.noMelee = true;
            item.knockBack = 3;
            item.value = 4000;
            item.rare = ItemRarityID.Green;
            item.UseSound = new Terraria.Audio.LegacySoundStyle(2, 21, Terraria.Audio.SoundType.Sound);
            item.shoot = ProjectileType<TideCallerStaff_WaterShot>();
            item.shootSpeed = 7f;
            item.autoReuse = true;

            Abilities[(int)AbilityType.Q] = new AquaPrison(this);
            Abilities[(int)AbilityType.W] = new EbbAndFlow(this);
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-9, 0);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddRecipeGroup("TerraLeague:GoldGroup", 10);
            recipe.AddIngredient(ItemID.Seashell, 5);
            recipe.AddIngredient(ItemID.FallenStar, 5);
            recipe.AddIngredient(ItemID.Sapphire, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
