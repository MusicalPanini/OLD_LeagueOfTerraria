using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Items.Accessories
{
    public class DarkinArtifact : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Darkin Artifact");
            Tooltip.SetDefault("Take on the form of a Darkin!");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.rare = ItemRarityID.Pink;
            item.value = 200000;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            modPlayer.darkinCostume = true;
            if (hideVisual)
            {
                modPlayer.darkinCostumeHideVanity = true;
            }
        }

        public override void AddRecipes()
        {
            //ModRecipe recipe = new ModRecipe(mod);
            //recipe.AddIngredient(GetInstance("TrueIceChunk"), 2);
            //recipe.AddIngredient(ItemID.PurpleIceBlock, 32);
            //recipe.AddIngredient(ItemID.SoulofNight, 8);
            //recipe.AddTile(TileID.IceMachine);
            //recipe.SetResult(this, 2);
            //recipe.AddRecipe();
        }
    }

    public class DarkinHead : EquipTexture
    {
        public override bool DrawHead()
        {
            return false;
        }

        public override void UpdateVanity(Player player, EquipType type)
        {
            //if (Main.rand.NextBool(20))
            //{
            //    Dust.NewDust(player.position, player.width, player.height, DustType<Dusts.Sparkle>());
            //}
        }
    }

    public class DarkinBody : EquipTexture
    {
        public override bool DrawBody()
        {
            return false;
        }
    }

    public class DarkinLegs : EquipTexture
    {
        public override bool DrawLegs()
        {
            return false;
        }
    }
}
