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
            item.damage = 32;           //The damage of your weapon
            item.width = 46;            //Weapon's texture's width
            item.height = 44;           //Weapon's texture's height
            item.melee = true;          //Is your weapon a melee weapon?
            item.useTime = 52;          //The time span of using the weapon. Remember in terraria, 60 frames is a second.
            item.useAnimation = 26;
            item.useStyle = 1;          //The use style of weapon, 1 for swinging, 2 for drinking, 3 act like shortsword, 4 for use like life crystal, 5 for use staffs or guns
            item.knockBack = 6;         //The force of knockback of the weapon. Maximum is 20
            item.value = 5400;           //The value of the weapon
            item.rare = 3;              //The rarity of the weapon, from -1 to 13
            item.UseSound = SoundID.Item1;      //The sound when the weapon is using
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
