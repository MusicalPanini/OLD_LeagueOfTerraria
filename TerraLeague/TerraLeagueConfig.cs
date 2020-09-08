﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using Terraria.ModLoader.Config.UI;
using Terraria.UI;

namespace TerraLeague
{
    [BackgroundColor(4, 74, 26)]
    [Label("Config")]
    public class TerraLeagueConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [Increment(5)]
        [Range(20, 100)]
        [DefaultValue(50)]
        [Slider]
        [BackgroundColor(186,33,55)]
        [Label("How much life per marker on the health bar")]
        public int healthBarDividerSpacing;

        [Increment(5)]
        [Range(20, 100)]
        [DefaultValue(50)]
        [Slider]
        [BackgroundColor(51, 106, 183)]
        [Label("How much mana per marker on the mana bar")]
        public int manaBarDividerSpacing;

        //[DefaultValue(true)]
        //[Label("Toggle the mods custom resource bars")]
        //public bool useModdedHealthBar;

        [DefaultValue(true)]
        [BackgroundColor(19, 122, 113)]
        [Slider]
        [Label("Toggle the custom resource bar")]
        public bool disableModResourceBar;

        [DefaultValue(1)]
        [Range(0, 1)]
        [BackgroundColor(0, 96, 29)]
        [Slider]
        [Label("Set the intensity of the Black Mist effect")]
        public float drawMist;

        public override void OnChanged()
        {
            UI.ResourceBar.healthBarDividerDistance = healthBarDividerSpacing;
            UI.ResourceBar.manaBarDividerDistance = manaBarDividerSpacing;
            TerraLeague.fogIntensity = drawMist;
            TerraLeague.disableModResourceBar = disableModResourceBar;

            base.OnChanged();
        }
    }
}