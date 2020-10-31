using Microsoft.Xna.Framework;
using TerraLeague.Items.Weapons.Abilities;
using Terraria;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class SunsteelBroadsword : AbilityItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sunsteel Broadsword");
            Tooltip.SetDefault("");
        }

        public override string GetWeaponTooltip()
        {
            return "";
        }

        public override string GetQuote()
        {
            return "DEMACIA!";
        }

        public override void SetDefaults()
        {
            item.damage = 10;
            item.width = 32;
            item.height = 32;
            item.melee = true;
            item.useTime = 32;
            item.useAnimation = 32;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.knockBack = 4;
            item.value = 1000;
            item.rare = ItemRarityID.Blue;
            item.UseSound = SoundID.Item1;

            Abilities[(int)AbilityType.Q] = new DecisiveStrike(this);
            Abilities[(int)AbilityType.W] = new Courage(this);
        }
    }
}
