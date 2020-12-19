using Microsoft.Xna.Framework;
using TerraLeague.Items.Weapons.Abilities;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class VoidProphetsStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Void Prophets Staff");
            Tooltip.SetDefault("");
        }

        string GetWeaponTooltip()
        {
            return "Summon a gate way from the void";
        }

        public override void SetDefaults()
        {
            item.damage = 20;
            item.sentry = true;
            item.summon = true;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.width = 48;
            item.height = 48;
            item.useAnimation = 30;
            item.useTime = 30;
            item.mana = 10;
            item.noMelee = true;
            item.knockBack = 1;
            item.value = 35000;
            item.rare = ItemRarityID.Lime;
            item.scale = 1f;
            item.shoot = ProjectileType<VoidProphetsStaff_ZzrotPortal>();
            item.UseSound = new Terraria.Audio.LegacySoundStyle(2, 113);
            item.autoReuse = false;

            AbilityItemGLOBAL abilityItem = item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.E, new MaleficVisions(this));
            abilityItem.ChampQuote = "Bow to the void! Or be consumed by it!";
            abilityItem.getWeaponTooltip = GetWeaponTooltip;
            abilityItem.IsAbilityItem = true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            player.FindSentryRestingSpot(item.shoot, out int xPos, out int yPos, out int yDis);
            Projectile.NewProjectile((float)xPos, (float)(yPos - yDis), 0f, 0f, type, damage, knockBack, player.whoAmI, 10, -1);
            player.UpdateMaxTurrets();

            return false;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<VoidBar>(), 14);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
