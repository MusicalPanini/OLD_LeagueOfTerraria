using Microsoft.Xna.Framework;
using System.Linq;
using TerraLeague.Items.Weapons.Abilities;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class FishBones : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fish Bones");
            Tooltip.SetDefault("");
        }

        string GetWeaponTooltip()
        {
            return "Kills grant 'EXCITED!'" +
                "\nEXCITED increase firerate";
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.damage = 35;
            item.ranged = true;
            item.useAnimation = 30;
            item.useTime = 30;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.noMelee = true;
            item.knockBack = 3;
            item.value = 120000;
            item.rare = ItemRarityID.Yellow;
            item.UseSound = SoundID.Item11;
            item.autoReuse = true;
            item.shoot = ProjectileID.RocketI;
            item.shootSpeed = 6;
            item.useAmmo = AmmoID.Rocket;

            AbilityItemGLOBAL abilityItem = item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.R, new SuperMegaDeathRocket(this));
            abilityItem.ChampQuote = "BYE BYE";
            abilityItem.getWeaponTooltip = GetWeaponTooltip;
            abilityItem.IsAbilityItem = true;
        }

        public override bool CanUseItem(Player player)
        {
            item.damage = 35;
            item.useAnimation = 30;
            item.useTime = 30;
            return true;
        }
        
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            type = item.shoot + player.inventory.Where(x => x.ammo == AmmoID.Rocket).First().shoot;

            position.Y -= 8;

            return true;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-65, -15);
        }

        public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
        {
            if (player.GetModPlayer<PLAYERGLOBAL>().excited)
                mult = 1.5f;

            base.ModifyWeaponDamage(player, ref add, ref mult, ref flat);
        }

        public override float UseTimeMultiplier(Player player)
        {
            if (player.GetModPlayer<PLAYERGLOBAL>().excited)
            {
                return base.UseTimeMultiplier(player) * 1.5f;
            }
            else
            {
                return base.UseTimeMultiplier(player);
            }
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.IllegalGunParts, 1);
            recipe.AddIngredient(ItemID.Megashark, 1);
            recipe.AddIngredient(ItemID.RocketLauncher, 1);
            recipe.AddIngredient(ItemID.SharkFin, 5);
            recipe.AddIngredient(ItemID.SoulofMight, 20);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
