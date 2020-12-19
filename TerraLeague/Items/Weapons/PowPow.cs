using Microsoft.Xna.Framework;
using TerraLeague.Items.Weapons.Abilities;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class PowPow : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pow Pow");
            Tooltip.SetDefault("");
        }

        string GetWeaponTooltip()
        {
            return "Kills grant 'EXCITED!'" +
                "\nEXCITED increases firerate and damage";
        }

        public override void SetDefaults()
        {
            item.damage = 15;
            item.ranged = true;
            item.width = 76;
            item.height = 46;
            item.useAnimation = 12;
            item.useTime = 4;
            item.reuseDelay = 14;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.noMelee = true;
            item.knockBack = 0;
            item.value = 60000;
            item.rare = ItemRarityID.Pink;
            item.UseSound = new Terraria.Audio.LegacySoundStyle(2, 31);
            item.shoot = ProjectileID.PurificationPowder;
            item.autoReuse = true;
            item.shootSpeed = 13f;
            item.useAmmo = AmmoID.Bullet;

            AbilityItemGLOBAL abilityItem = item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.E, new Zap(this));
            abilityItem.ChampQuote = "SAY HELLO TO MY FRIENDS OF VARYING SIZES!";
            abilityItem.getWeaponTooltip = GetWeaponTooltip;
            abilityItem.IsAbilityItem = true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY + 6)) * 25f;
            
            position += muzzleOffset;

            Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(3));

            speedX = perturbedSpeed.X;
            speedY = perturbedSpeed.Y;

            return true;
        }

        public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
        {
            if (player.GetModPlayer<PLAYERGLOBAL>().excited)
                mult = 2;

            base.ModifyWeaponDamage(player, ref add, ref mult, ref flat);
        }

        public override float UseTimeMultiplier(Player player)
        {
            if (player.GetModPlayer<PLAYERGLOBAL>().excited)
            {
                return base.UseTimeMultiplier(player) * 1.3f;
            }
            else
            {
                return base.UseTimeMultiplier(player);
            }
        }

        public override bool ConsumeAmmo(Player player)
        {
            int num = (int)(item.useAnimation * UseTimeMultiplier(player)) - item.useAnimation;

                return !(player.itemAnimation < (player.itemAnimationMax) - 2);
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-20, 8);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.IllegalGunParts, 1);
            recipe.AddIngredient(ItemID.Minishark, 1);
            recipe.AddIngredient(ItemID.ClockworkAssaultRifle, 1);
            recipe.AddIngredient(ItemID.SoulofSight, 10);
            recipe.AddIngredient(ItemID.PinkPaint, 3);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
