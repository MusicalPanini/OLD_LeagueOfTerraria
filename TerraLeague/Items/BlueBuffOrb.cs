using Microsoft.Xna.Framework;
using TerraLeague.Buffs;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items
{
    public class BlueBuffOrb : ModItem
    {
        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(5, 4));
            ItemID.Sets.ItemIconPulse[item.type] = true;
            ItemID.Sets.ItemNoGravity[item.type] = true;
            DisplayName.SetDefault("Crest of Insight");
            
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            item.maxStack = 1;
            item.rare = ItemRarityID.Blue;
            item.width = 24;
            item.height = 32;
            
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
            ItemID.Sets.ItemIconPulse[item.type] = true;

            Lighting.AddLight(item.Center, Color.Blue.ToVector3() * 0.55f * Main.essScale);
        }

        public override bool OnPickup(Player player)
        {
            Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 29), player.Center);
            for (int k = 0; k < 20; k++)
            {
                Dust.NewDust(player.position, player.width, player.height, 56, 0, -2, 0, default(Color), 1.2f);
            }
            player.AddBuff(BuffType<BlueBuff>(), 240 * 60);
            CombatText.NewText(player.Hitbox, new Color(50, 50, 255), "Blue Buff");
            return false;
        }
    }
}
