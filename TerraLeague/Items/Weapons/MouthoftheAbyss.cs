using Microsoft.Xna.Framework;
using TerraLeague.Items.Weapons.Abilities;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class MouthoftheAbyss : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mouth of the Abyss");
            Tooltip.SetDefault("");
        }

        string GetWeaponTooltip()
        {
            return "33% chance to not consume ammo";
        }

        public override void SetDefaults()
        {
            item.damage = 26;
            item.ranged = true;
            item.width = 44;
            item.height = 52;
            item.useAnimation = 8;
            item.useTime = 8;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.noMelee = true;
            item.knockBack = 1;
            item.value = 120000;
            item.rare = ItemRarityID.Lime;
            item.UseSound = new Terraria.Audio.LegacySoundStyle(2, 11);
            item.shoot = ProjectileID.PurificationPowder;
            item.autoReuse = true;
            item.shootSpeed = 13f;
            item.useAmmo = AmmoID.Bullet;

            AbilityItemGLOBAL abilityItem = item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.W, new BioArcaneBarrage(this));
            abilityItem.ChampQuote = "Hunger never sleep";
            abilityItem.getWeaponTooltip = GetWeaponTooltip;
            abilityItem.IsAbilityItem = true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            position.Y += 8;
            
            int numberProjectiles = Main.rand.Next(0,4) != 0 ? 0 : Main.rand.Next(1, 4);
            for (int i = 0; i < numberProjectiles; i++)
            {
                Vector2 perturbedSpeed = new Vector2(speedX + Main.rand.NextFloat(-2, 2), speedY + Main.rand.NextFloat(-1, 1)).RotatedByRandom(MathHelper.ToRadians(8));
                Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, ProjectileType<MouthoftheAbyss_AcidBlob>(), damage, knockBack, player.whoAmI);
            }

            return true;
        }

        public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
        {
            base.ModifyWeaponDamage(player, ref add, ref mult, ref flat);
        }

        public override bool ConsumeAmmo(Player player)
        {
            return Main.rand.Next(0, 100) < 33;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(0, 0);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<VoidBar>(), 14);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
