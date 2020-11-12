using Microsoft.Xna.Framework;
using TerraLeague.Buffs;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items
{
    public class RegenHeart : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.ItemIconPulse[item.type] = true;
            ItemID.Sets.ItemNoGravity[item.type] = true;
            DisplayName.SetDefault("Rejuvenation");
            
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            item.maxStack = 1;
            item.rare = ItemRarityID.Orange;
            item.width = 18;
            item.height = 18;
            base.SetDefaults();
        }

        public override void GrabRange(Player player, ref int grabRange)
        {
            
        }

        public override bool GrabStyle(Player player)
        {
            Vector2 vectorItemToPlayer = player.Center - item.Center;
            Vector2 movement = vectorItemToPlayer.SafeNormalize(default(Vector2));
            item.velocity = item.velocity + movement;
            item.velocity = Collision.TileCollision(item.position, item.velocity, item.width, item.height);
            return true;
        }

        public override void PostUpdate()
        {
            Lighting.AddLight(item.Center, Color.Orange.ToVector3() * Main.essScale);
        }

        public override bool OnPickup(Player player)
        {
            Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 29), player.Center);
            //for (int k = 0; k < 20; k++)
            //{
            //    Dust.NewDust(player.position, player.width, player.height, 56, 0, -2, 0, default(Color), 1.2f);
            //}
            player.AddBuff(BuffType<Rejuvenation>(), 2 * 60);
            return false;
        }
    }
}
