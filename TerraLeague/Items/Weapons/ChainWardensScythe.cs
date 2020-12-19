using Microsoft.Xna.Framework;
using TerraLeague.Items.Weapons.Abilities;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class ChainWardensScythe : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Chain Warden's Scythe");
            Tooltip.SetDefault("");
        }

        public override void SetDefaults()
        {
            item.width = 50;
            item.height = 46;
            item.value = 54000;
            item.rare = ItemRarityID.Green;
            item.noMelee = true;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.useAnimation = 40;
            item.useTime = 40;
            item.knockBack = 6F;
            item.damage = 14;
            item.scale = 1;
            item.noUseGraphic = true;
            item.UseSound = SoundID.Item1;
            item.shootSpeed = 14F;
            item.melee = true;
            item.channel = true;
            item.shoot = ProjectileType<ChainWardensScythe_Scythe>();

            AbilityItemGLOBAL abilityItem = item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.W, new DarkPassage(this));
            abilityItem.ChampQuote = "What delightful agony we shall inflict";
            abilityItem.IsAbilityItem = true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            return base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<DamnedSoul>(), 50);
            recipe.AddRecipeGroup("TerraLeague:DemonGroup", 16);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
