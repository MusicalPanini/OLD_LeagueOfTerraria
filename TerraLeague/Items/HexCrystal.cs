﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items
{
    public class HexCrystal : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hex Crystal");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            item.maxStack = 99;
            item.width = 22;
            item.height = 18;
            item.value = 40000;
            item.rare = ItemRarityID.Pink;
        }

        public override void PostUpdate()
        {
            Lighting.AddLight(item.Center, Color.Pink.ToVector3());
        }
    }
}
