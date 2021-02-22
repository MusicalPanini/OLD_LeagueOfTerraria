using Microsoft.Xna.Framework;
using TerraLeague.Items.Weapons.Abilities;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class AtlasGauntlets : ModItem
    {
        public override bool OnlyShootOnSwing => false;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Atlas Gauntlets");
            Tooltip.SetDefault("");
        }

        public override void SetDefaults()
        {
            item.damage = 70;
            item.width = 52;          
            item.height = 30;         
            item.melee = true;        
            item.useTime = 10;
            item.useAnimation = 10;
            item.useStyle = ItemUseStyleID.HoldingOut;          
            item.knockBack = 6;    
            item.value = 100000;
            item.rare = ItemRarityID.Pink; 
            item.shoot = ProjectileType<AtlasGauntlets_Right>();
            item.shootSpeed = 10;
            item.noUseGraphic = true;
            item.noMelee = true;
            item.autoReuse = true;

            AbilityItemGLOBAL abilityItem = item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.E, new ExcessiveForce(this));
            abilityItem.ChampQuote = "Here comes the punch line!";
            abilityItem.IsAbilityItem = true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.ownedProjectileCounts[ProjectileType<AtlasGauntlets_Left>()] > 0 && player.ownedProjectileCounts[ProjectileType<AtlasGauntlets_Right>()] > 0)
                return false;
            return base.CanUseItem(player);
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            item.knockBack = 6;
            if (player.ownedProjectileCounts[ProjectileType<AtlasGauntlets_Right>()] == 0)
            {
                type = ProjectileType<AtlasGauntlets_Right>();
                return true;
            }
            else if (player.ownedProjectileCounts[ProjectileType<AtlasGauntlets_Left>()] == 0)
            {
                type = ProjectileType<AtlasGauntlets_Left>();
                return true;
            }

            return false;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<PerfectHexCore>());
            recipe.AddRecipeGroup("TerraLeague:Tier3Bar", 14);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
