﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TerraLeague.Items.SummonerSpells;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Items
{
    public class Sunstone : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sunstone");
            Tooltip.SetDefault("'Feels warm'");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            item.maxStack = 999;
            item.width = 20;
            item.height = 24;
            item.rare = 2;
            item.value = 5000;
        }
    }
}
