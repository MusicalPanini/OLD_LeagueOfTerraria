using Microsoft.Xna.Framework;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class DebugGun : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Debug Gun");
            Tooltip.SetDefault("");
        }

        string GetWeaponTooltip()
        {
            return "Does whatever I say it does";
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

            AbilityItemGLOBAL abilityItem = item.GetGlobalItem<AbilityItemGLOBAL>();
            //abilityItem.SetAbility(AbilityType.Q, new Defile(this));
            //abilityItem.SetAbility(AbilityType.W, new Defile(this));
            //abilityItem.SetAbility(AbilityType.E, new Defile(this));
            //abilityItem.SetAbility(AbilityType.R, new Requiem(this));
            abilityItem.ChampQuote = "You probably shouldn't have this";
            abilityItem.getWeaponTooltip = GetWeaponTooltip;
            abilityItem.IsAbilityItem = true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            position = Main.MouseWorld;
            speedX = 0;
            speedY = 0;
            item.channel = false;
            return true;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(0, 0);
        }
    }
}
