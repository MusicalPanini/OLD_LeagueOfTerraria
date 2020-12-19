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
    public class Hexplosives : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hexplosives");
            Tooltip.SetDefault("");
        }

        public override void SetDefaults()
        {
            item.damage = 55;
            item.width = 32;
            item.height = 32;
            item.magic = true;
            item.useAnimation = 32;
            item.useTime = 32;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.knockBack = 5;
            item.value = 40000;
            item.rare = ItemRarityID.LightRed;
            item.UseSound = new LegacySoundStyle(2, 19, Terraria.Audio.SoundType.Sound);
            item.shootSpeed = 12f;
            item.shoot = ProjectileType<Hexplosives_Bomb>();
            item.mana = 8;
            item.noMelee = true;
            item.useTurn = true;
            item.autoReuse = true;
            item.noUseGraphic = true;

            AbilityItemGLOBAL abilityItem = item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.Q, new BouncingBomb(this));
            abilityItem.SetAbility(AbilityType.E, new HexsplosiveMineField(this));
            abilityItem.ChampQuote = "This'll be a blast!";
            abilityItem.IsAbilityItem = true;
        }

        public override bool CanUseItem(Player player)
        {
            return true;
        }

        public override float UseTimeMultiplier(Player player)
        {
            return 1;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            return true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<HextechCore>(), 2);
            recipe.AddIngredient(ItemID.Bomb, 20);
            recipe.AddRecipeGroup("TerraLeague:Tier3Bar", 6);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
