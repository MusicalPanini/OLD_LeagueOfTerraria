using Microsoft.Xna.Framework;
using TerraLeague.Items.Weapons.Abilities;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class EyeofGod : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Eye of God");
            Tooltip.SetDefault("");
        }

        string GetWeaponTooltip()
        {
            return "Summon a tentacle of Nagakabouros to fight for you";
        }

        public override void SetDefaults()
        {
            item.damage = 15;
            item.sentry = true;
            item.summon = true;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.width = 32;
            item.height = 32;
            item.useAnimation = 30;
            item.useTime = 30;
            item.mana = 10;
            item.noMelee = true;
            item.knockBack = 1;
            item.value = 3500;
            item.rare = ItemRarityID.Green;
            item.scale = 1f;
            item.shoot = ProjectileType<EyeofGod_Tentacle>();
            item.UseSound = new Terraria.Audio.LegacySoundStyle(2, 8);
            item.autoReuse = false;

            AbilityItemGLOBAL abilityItem = item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.E, new TestOfSpirit(this));
            abilityItem.ChampQuote = "There are kind and gentle gods. Mine isn't one of those";
            abilityItem.getWeaponTooltip = GetWeaponTooltip;
            abilityItem.IsAbilityItem = true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            player.FindSentryRestingSpot(item.shoot, out int xPos, out int yPos, out int yDis);
            Projectile.NewProjectile((float)xPos, (float)(yPos - yDis) - 24, 0f, 0f, type, damage, knockBack, player.whoAmI, 10, -1);
            player.UpdateMaxTurrets();

            return false;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<BrassBar>(), 14);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
