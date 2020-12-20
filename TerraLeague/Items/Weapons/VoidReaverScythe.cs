using Microsoft.Xna.Framework;
using TerraLeague.Items.Weapons.Abilities;
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
            Tooltip.SetDefault("");
        }

        public override void SetDefaults()
        {
            item.damage = 32;
            item.width = 46;          
            item.height = 44;         
            item.melee = true;        
            item.useTime = 52;        
            item.useAnimation = 26;
            item.useStyle = ItemUseStyleID.SwingThrow;          
            item.knockBack = 3;    
            item.value = 5400;
            item.rare = ItemRarityID.Orange; 
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.shoot = ProjectileType<VoidReaverScythe_VoidSpike>();
            item.shootSpeed = 18;
            item.scale = 1.3f;

            AbilityItemGLOBAL abilityItem = item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.R, new EvolvedWings(this));
            abilityItem.ChampQuote = "Fear the Void";
            abilityItem.IsAbilityItem = true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 velocity = new Vector2(speedX, speedY);

            Projectile.NewProjectile(position, velocity, type, damage, knockBack, player.whoAmI);
            Projectile.NewProjectile(position, velocity.RotatedBy(0.3f), type, damage, knockBack, player.whoAmI);
            Projectile.NewProjectile(position, velocity.RotatedBy(-0.3f), type, damage, knockBack, player.whoAmI);

            return false;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<VoidFragment>(), 120);
            recipe.AddTile(TileID.DemonAltar);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
