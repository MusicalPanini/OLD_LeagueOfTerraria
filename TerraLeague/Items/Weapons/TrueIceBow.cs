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
    public class TrueIceBow : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("True Ice Bow");
            Tooltip.SetDefault("");
        }

        string GetWeaponTooltip()
        {
            return "Fires a flurry of slowing arrows after every shot that deal 30% of the original arrow's damage";
        }

        public override void SetDefaults()
        {
            item.damage = 30;
            item.ranged = true;
            item.width = 24;
            item.height = 54;
            item.useAnimation = 25;
            item.useTime = 5;
            item.reuseDelay = 20;
            item.shootSpeed = 10f;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.noMelee = true;
            item.knockBack = 1;
            item.value = 160000;
            item.rare = ItemRarityID.Pink;
            item.UseSound = SoundID.Item5;
            item.autoReuse = true;
            item.shoot = ProjectileID.PurificationPowder;
            item.useAmmo = AmmoID.Arrow;

            AbilityItemGLOBAL abilityItem = item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.W, new Volley(this));
            abilityItem.ChampQuote = "Right between the eyes";
            abilityItem.getWeaponTooltip = GetWeaponTooltip;
            abilityItem.IsAbilityItem = true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (player.itemAnimation != player.itemAnimationMax - 1)
            {
                type = ProjectileType<TrueIceBow_Flurry>();
                damage = (int)(damage * 0.3);
                knockBack = 0;
                //int numberProjectiles = 5;
                //    int distance = 24;
                //    for (int i = 0; i < numberProjectiles; i++)
                //    {
                //        Vector2 relPosition = new Vector2(0 - (distance * 2) + (i * distance), 0).RotatedBy(TerraLeague.CalcAngle(player.Center, Main.MouseWorld) + MathHelper.PiOver2);
                //        Vector2 position = new Vector2(player.Center.X + relPosition.X, player.Center.Y + relPosition.Y);
                //        Vector2 velocity = TerraLeague.CalcVelocityToMouse(position, 15f);

                //        Projectile.NewProjectile(position, velocity, projType, damage, knockback, player.whoAmI);
                //    }
            }

            return true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<TrueIceChunk>(), 4);
            recipe.AddIngredient(ItemID.HellwingBow, 1);
            recipe.AddIngredient(ItemID.FrostCore, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override bool ConsumeAmmo(Player player)
        {
            int num = (int)(item.useAnimation * UseTimeMultiplier(player)) - item.useAnimation;

            return !(player.itemAnimation < (player.itemAnimationMax) - 2);
        }
    }
}
