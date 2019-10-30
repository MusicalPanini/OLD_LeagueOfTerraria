using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.PetrifiedWood
{
    public class PetBow : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Petrified Wood Bow");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            item.damage = 7;
            item.ranged = true;
            item.noMelee = true;
            item.width = 16;
            item.height = 32;
            item.useTime = 27;
            item.useAnimation = 27;
            item.useStyle = 5;
            item.knockBack = 0f;
            item.value = 100;
            item.rare = 0;
            item.shootSpeed = 6.6f;
            item.UseSound = SoundID.Item5;
            item.useAmmo = AmmoID.Arrow;
        }

        public override bool CanUseItem(Player player)
        {
            //Main.tile[(int)player.position.X / 16, (int)(player.position.Y / 16) + 5].type = (ushort)TileType("MarbleGrass");
            //Main.NewText(Main.tile[(int)player.position.X / 16, (int)(player.position.Y / 16) + 5] + " " + (ushort)TileType("Limestone"));
            item.shoot = player.inventory.Where(x => x.ammo == AmmoID.Arrow).First().shoot;
            return base.CanUseItem(player);
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            
            return true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<PetWood>(), 10);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
            base.AddRecipes();
        }
    }
}
