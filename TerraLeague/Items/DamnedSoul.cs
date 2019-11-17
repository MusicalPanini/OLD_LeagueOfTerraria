﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items
{
    public class DamnedSoul : ModItem
    {
        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(8, 4));
            DisplayName.SetDefault("Damned Soul");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            item.maxStack = 999;
            item.width = 16;
            item.height = 16;
            item.uniqueStack = false;
            item.rare = 2;
            item.value = 1000;
        }

        public override bool GrabStyle(Player player)
        {
            Vector2 vectorItemToPlayer = player.Center - item.Center;
            Vector2 movement = vectorItemToPlayer.SafeNormalize(default(Vector2));
            item.velocity = item.velocity + movement;
            return true;
        }

        public override void PostUpdate()
        {
            Lighting.AddLight(item.Center, Color.DarkSeaGreen.ToVector3());
        }
    }
}
