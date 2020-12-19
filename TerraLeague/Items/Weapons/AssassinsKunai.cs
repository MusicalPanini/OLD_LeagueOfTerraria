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
    public class AssassinsKunai : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Assassin's Kunai");
            Tooltip.SetDefault("");
        }

        public override void SetDefaults()
        {
            item.damage = 36;
            item.width = 18;
            item.height = 36;
            item.magic = true;
            item.useTime = 23;
            item.useAnimation = 23;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.knockBack = 2;
            item.mana = 8;
            item.value = 350000;
            item.rare = ItemRarityID.Lime;
            item.UseSound = new LegacySoundStyle(2, 19, Terraria.Audio.SoundType.Sound);
            item.shoot = ProjectileType<AssassinsKunai_Kunai>();
            item.shootSpeed = 16f;
            item.noMelee = true;
            item.useTurn = true;
            item.autoReuse = true;
            item.noUseGraphic = true;

            AbilityItemGLOBAL abilityItem = item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.W, new TwilightShroud(this));
            abilityItem.ChampQuote = "Fear the assassin with no master";
            abilityItem.IsAbilityItem = true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            int numberProjectiles = 5;
            float startingAngle = 20;
            for (int i = 0; i < numberProjectiles; i++)
            {
                Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedBy(MathHelper.ToRadians(startingAngle));
                Projectile.NewProjectile(position, perturbedSpeed, type, damage, knockBack, player.whoAmI);
                startingAngle -= 10f;
            }

            return false;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<HarmonicBar>(), 16);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
