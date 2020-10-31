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
    public class CrystalineVoidEnergy : AbilityItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Crystaline Void Energy");
            Tooltip.SetDefault("");
        }

        public override string GetWeaponTooltip()
        {
            PLAYERGLOBAL modPlayer = Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>();

            return "Shots apply stacks of 'Caustic Wounds'" +
                "\nAt 5 stacks, the enemy will take 25% of their missing life as magic damage" +
                "\n(Capped at 50 + " + TerraLeague.CreateScalingTooltip(DamageType.MAG, Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().MAG, 100) + ")";
        }

        public override string GetQuote()
        {
            return "Exploit their weakness";
        }

        public override void SetDefaults()
        {
            item.damage = 16;
            item.ranged = true;
            item.width = 24;
            item.height = 54;
            item.useAnimation = 35;
            item.useTime = 35;
            item.shootSpeed = 6f;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.noMelee = true;
            item.knockBack = 1;
            item.value = 5400;
            item.rare = ItemRarityID.Orange;
            item.UseSound =  new LegacySoundStyle(2, 75);
            item.autoReuse = true;
            item.shoot = ProjectileType<CrystalineVoidEnergy_VoidEnergy>();

            Abilities[(int)AbilityType.W] = new VoidSeeker(this);
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            position.Y = position.Y + 4;
            
            return true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<VoidFragment>(), 120);
            recipe.AddTile(TileID.DemonAltar);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-6, 0);
        }
    }
}
