using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using TerraLeague.Buffs;
using TerraLeague.Items.Weapons.Abilities;

namespace TerraLeague.Items.Weapons
{
    public class ArcaneEnergy : AbilityItem
    {
        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(5, 4));
            DisplayName.SetDefault("Arcane Energy");
            Tooltip.SetDefault("");
        }

        public override string GetWeaponTooltip()
        {
            return "Gains 100% extra damage and range for each second you charge it" +
                    "\nMax of 1000%";
        }

        public override string GetQuote()
        {
            return "Behold my power";
        }

        public override void SetDefaults()
        {
            item.damage = 75;
            item.width = 32;
            item.height = 32;
            item.magic = true;
            item.useAnimation = 32;
            item.useTime = 32;
            item.useStyle = ItemUseStyleID.HoldingUp;
            item.knockBack = 5;
            item.value = 40000;
            item.rare = ItemRarityID.LightRed;
            item.shootSpeed = 12f;
            item.shoot = ProjectileType<ArcaneEnergy_PulseControl>();
            item.useTurn = true;
            item.noUseGraphic = true;
            item.mana = 40;
            item.autoReuse = false;
            item.channel = true;
            item.noMelee = true;

            Abilities[2] = new ShockingOrb(this);
            Abilities[3] = new RiteOfTheArcane(this);
        }

        public override bool CanUseItem(Player player)
        {
            return true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            item.noMelee = true;
            return true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<Sunstone>(), 20);
            recipe.AddIngredient(ItemID.Chain, 4);
            recipe.AddIngredient(ItemID.SoulofMight, 10);
            recipe.AddIngredient(ItemID.FallenStar, 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
