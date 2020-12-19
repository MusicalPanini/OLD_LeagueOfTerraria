using Microsoft.Xna.Framework;
using TerraLeague.Items.Weapons.Abilities;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
	public class StoneweaversStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stoneweaver's Staff");
            Tooltip.SetDefault("");
        }

        string GetWeaponTooltip()
        {
            return "Uses Stone as ammo";
        }

        public override void SetDefaults()
        {
            item.damage = 17;
            item.mana = 4;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.magic = true;
            item.useTime = 35;
            item.useAnimation = 35;
            item.noMelee = true;
            item.knockBack = 2;
            item.value = 10000;
            item.rare = ItemRarityID.Green;
            item.UseSound = SoundID.Item8;
            item.shoot = ProjectileType<StoneweaversStaff_WeaversStone>();
            item.shootSpeed = 12f;
            item.autoReuse = true;
            item.useAmmo = ItemType<BlackIceChunk>();

            AbilityItemGLOBAL abilityItem = item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.Q, new ThreadedVolley(this));
            abilityItem.ChampQuote = "Throw another rock!";
            abilityItem.getWeaponTooltip = GetWeaponTooltip;
            abilityItem.IsAbilityItem = true;
        }

        public override bool CanUseItem(Player player)
        {
            return player.HasAmmo(item, false);
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(0, -6);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddRecipeGroup("TerraLeague:GoldGroup", 10);
            recipe.AddIngredient(ItemID.Sandstone, 50);
            recipe.AddIngredient(ItemType<Sunstone>(), 10);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
