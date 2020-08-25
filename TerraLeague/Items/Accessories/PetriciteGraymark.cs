using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TerraLeague.NPCs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Accessories
{
    public class PetriciteGraymark : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Petricite Graymark");
            Tooltip.SetDefault("Gain 10 resist after taking damage from a projectile for 4 seconds");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            item.width = 26;
            item.height = 26;
            item.rare = ItemRarityID.LightRed;
            item.value = 6400 * 5;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            modPlayer.greymark = true;

            if (modPlayer.greymarkBuff)
                modPlayer.resist += 10;
        }

        public override void AddRecipes()
        {
            //ModRecipe recipe = new ModRecipe(mod);
            //recipe.AddIngredient(ItemType<SilversteelBar>(), 12);
            //recipe.AddTile(TileID.MythrilAnvil);
            //recipe.SetResult(this);
            //recipe.AddRecipe();
        }

        public class GraymarkNPC : GlobalNPC
        {
            public override void NPCLoot(NPC npc)
            {
                float rnd = Main.rand.NextFloat();
                if ((npc.netID == NPCID.GreekSkeleton || npc.netID == NPCID.Medusa) && (rnd <= 0.0133 || (Main.expertMode && rnd <= 0.0266f)) && !npc.SpawnedFromStatue)
                {
                    Item.NewItem(npc.getRect(), ItemType<PetriciteGraymark>(), 1);
                }

                base.NPCLoot(npc);
            }
        }
    }
}
