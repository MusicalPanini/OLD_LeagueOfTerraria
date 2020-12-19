using Microsoft.Xna.Framework;
using TerraLeague.Items.Weapons.Abilities;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class DarksteelThrowingAxe : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Darksteel Throwing Axe");
            Tooltip.SetDefault("");
        }

        public override void SetDefaults()
        {
            item.damage = 42;
            item.width = 24;
            item.height = 24;
            item.ranged = true;
            item.useTime = 45;
            item.useAnimation = 45;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.knockBack = 3.5f;
            item.value = 6000;
            item.rare = ItemRarityID.Orange;
            item.UseSound = new Terraria.Audio.LegacySoundStyle(2, 19, Terraria.Audio.SoundType.Sound);
            item.shootSpeed = 15f;
            item.shoot = ProjectileType<DarksteelThrowingAxe_ThrowingAxe>();
            item.autoReuse = true;
            item.noMelee = true;
            item.useTurn = true;
            item.noUseGraphic = true;

            AbilityItemGLOBAL abilityItem = item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.Q, new SpinningAxe(this));
            abilityItem.ChampQuote = "Welcome to the League of Draaaaven";
            abilityItem.IsAbilityItem = true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            position.Y -= 5;

            if (player.GetModPlayer<PLAYERGLOBAL>().spinningAxe)
            {
                type = ProjectileType<DarksteelThrowingAxe_SpinningAxe>();
                AbilityItemGLOBAL abilityItem = item.GetGlobalItem<AbilityItemGLOBAL>();
                if (abilityItem.GetAbility(AbilityType.Q) != null)
                {
                    damage += abilityItem.GetAbility(AbilityType.Q).GetAbilityBaseDamage(player);
                    damage += abilityItem.GetAbility(AbilityType.Q).GetAbilityScaledDamage(player, DamageType.RNG);
                }
                Projectile.NewProjectileDirect(position, new Vector2(speedX, speedY) * 1.2f, type, damage, knockBack + 1.5f, player.whoAmI, 1, player.velocity.X);
                player.ClearBuff(BuffType<Buffs.SpinningAxe>());
                return false;
            }
            else
            {
                return true;
            }
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<DarksteelBar>(), 14);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
