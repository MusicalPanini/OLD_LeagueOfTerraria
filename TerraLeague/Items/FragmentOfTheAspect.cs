using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TerraLeague.Items.SummonerSpells;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Items
{
    public class FragmentOfTheAspect : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fragment of the Aspect");
            Tooltip.SetDefault("Disappears after the sunset" +
                "\n'Gift from the gods'");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            item.maxStack = 999;
            item.width = 14;
            item.height = 18;
            item.rare = ItemRarityID.Cyan;
            item.value = Item.buyPrice(0, 50, 0, 0);
        }

        public override void PostUpdate()
        {
            Lighting.AddLight(item.Center, Color.Blue.ToVector3());

            if (!Main.dayTime)
            {
                item.SetDefaults();

                for (int i = 0; i < 20; i++)
                {
                    Dust.NewDustDirect(item.position, item.width, item.height, 59);
                }
            }
        }
    }
}
