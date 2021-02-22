using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using TerraLeague.Projectiles;
using Microsoft.Xna.Framework;
using static Terraria.ModLoader.ModContent;
using TerraLeague.Items.Weapons.Abilities;

namespace TerraLeague.Items.Weapons
{
    public class DarkIceTome : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dark Ice Tome");
            Tooltip.SetDefault("");
        }

        string GetWeaponTooltip()
        {
            return "Attacks slow hit enemies." +
                "\nIf the enemy dies while slowed, they will shatter into icy shrapenel." +
                "\nThe shrapenel deals 10% of the dead npcs max health as magic damage.";
        }

        public override void SetDefaults()
        {
            item.damage = 50;
            item.noMelee = true;
            item.magic = true;
            item.mana = 14;
            item.value = 160000;
            item.rare = ItemRarityID.Pink;
            item.width = 28;
            item.height = 32;
            item.useTime = 45;
            item.useAnimation = 45;
            item.knockBack = 2;
            item.UseSound = new Terraria.Audio.LegacySoundStyle(2,8);
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.shootSpeed = 16;
            item.shoot = ProjectileType<DarkIceTome_IceShard>();

            AbilityItemGLOBAL abilityItem = item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.W, new RingOfFrost(this));
            abilityItem.ChampQuote = "I will bury the world in ice";
            abilityItem.getWeaponTooltip = GetWeaponTooltip;
            abilityItem.IsAbilityItem = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<TrueIceChunk>(), 4);
            recipe.AddIngredient(ItemID.DemonScythe, 1);
            recipe.AddIngredient(ItemID.FrostCore, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
