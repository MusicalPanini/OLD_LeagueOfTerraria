using Microsoft.Xna.Framework;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class DebugGun : AbilityItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Debug Gun");
            Tooltip.SetDefault("");
        }

        public override string GetWeaponTooltip()
        {
            return "";
        }

        public override string GetQuote()
        {
            return "You probably shouldn't have this";
        }

        public override void SetDefaults()
        {
            item.damage = 20;
            item.magic = true;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.width = 40;
            item.height = 24;
            item.useAnimation = 30;
            item.useTime = 30;
            item.shootSpeed = 12f;
            item.noMelee = true;
            item.knockBack = 0;
            item.value = 1;
            item.rare = ItemRarityID.Expert;
            item.scale = 0.9f;
            item.shoot = ProjectileType<SolariSet_SolarSigil>();
            //item.UseSound = new Terraria.Audio.LegacySoundStyle(2, 12);
            item.autoReuse = false;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            position = Main.MouseWorld;
            speedX = 0;
            speedY = 0;
            item.channel = false;
            return true;
        }

        public override bool GetIfAbilityExists(AbilityType type)
        {
            if (type == AbilityType.E)
                return false;
            return base.GetIfAbilityExists(type);
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(0, 0);
        }
    }
}
