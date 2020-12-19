using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System.Linq;
using Microsoft.Xna.Framework;
using TerraLeague.Projectiles;
using Terraria.Audio;
using static Terraria.ModLoader.ModContent;
using TerraLeague.Items.Weapons.Abilities;

namespace TerraLeague.Items.Weapons
{
    public class EyeOfTheVoid : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Eye of the Void");
            Tooltip.SetDefault("");
        }

        string GetWeaponTooltip()
        {
            return "Fire a lifeform disintegration ray!";
        }

        public override void SetDefaults()
        {
            item.damage = 16;
            item.noMelee = true;
            item.magic = true;
            item.channel = true;
            item.mana = 6;
            item.rare = ItemRarityID.Orange;
            item.value = 5400;
            item.width = 28;
            item.height = 32;
            item.useTime = 20;
            item.useAnimation = 20;
            item.UseSound = new LegacySoundStyle(2, 15);
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.shootSpeed = 10;
            item.shoot = ProjectileType<EyeOfTheVoid_Lazer>();

            AbilityItemGLOBAL abilityItem = item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.Q, new PlasmaFission(this));
            abilityItem.ChampQuote = "Knowledge through... disintegration";
            abilityItem.getWeaponTooltip = GetWeaponTooltip;
            abilityItem.IsAbilityItem = true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            return base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
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
