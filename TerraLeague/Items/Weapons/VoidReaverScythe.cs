using Microsoft.Xna.Framework;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class VoidReaverScythe : ModItem
    {
        public override bool OnlyShootOnSwing => true;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Voidreaver Scythe");

        }

        public override void SetDefaults()
        {
            item.damage = 32;        
            item.width = 46;          
            item.height = 44;         
            item.melee = true;        
            item.useTime = 52;        
            item.useAnimation = 26;
            item.useStyle = 1;          
            item.knockBack = 6;    
            item.value = 5400;
            item.rare = 3; 
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.shoot = ProjectileType<VoidSpike>();
            item.shootSpeed = 18;
            item.scale = 1.3f;
        }

        public override void OnHitNPC(Player player, NPC target, int damage, float knockback, bool crit)
        {

        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            player.ChangeDir(player.position.X > Main.MouseWorld.X ? -1 : 1);

            return base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<VoidFragment>(), 64);
            recipe.AddTile(TileID.DemonAltar);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
