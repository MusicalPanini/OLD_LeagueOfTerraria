﻿using Microsoft.Xna.Framework;
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
            Tooltip.SetDefault("'Gift from the gods'");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            item.maxStack = 999;
            item.width = 14;
            item.height = 18;
            item.rare = 9;
            item.value = 100000;
        }

        public override void PostUpdate()
        {
            Lighting.AddLight(item.Center, Color.Blue.ToVector3());
        }
    }
}