using Microsoft.Xna.Framework;
using TerraLeague.Buffs;
using TerraLeague.Items.Weapons.Abilities;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class NetherBladeofHorok : ModItem
    {
        public override bool OnlyShootOnSwing => true;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Nether Blade of Horok");
            Tooltip.SetDefault("");
        }
        public override void SetDefaults()
        {
            item.damage = 74;
            item.width = 40;
            item.height = 40;
            item.melee = true;
            item.useTime = 32;
            item.useAnimation = 32;
            item.scale = 1.3f;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.knockBack = 5;
            item.value = 120000;
            item.rare = ItemRarityID.Lime;
            item.shoot = ProjectileType<NetherBladeofHorok_NullSphere>();
            item.autoReuse = true;
            item.UseSound = new LegacySoundStyle(2, 15);
            item.shootSpeed = 7;
            //item.GetGlobalItem<TerraLeagueITEMGLOBAL>().meleeProjCooldown = true;

            AbilityItemGLOBAL abilityItem = item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.R, new Riftwalk(this));
            abilityItem.ChampQuote = "You are null and void";
            abilityItem.IsAbilityItem = true;
        }

        

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            item.useTime = 32;
            Projectile.NewProjectileDirect(player.MountedCenter, new Vector2(speedX, speedY), type, damage, 0, player.whoAmI, -1);
            return false;
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            Dust dust = Dust.NewDustDirect(hitbox.TopLeft(), hitbox.Width, hitbox.Height, 112, 0,0, 255, new Color(59, 0, 255), 1f);
            dust.noGravity = true;
            dust.noLight = true;

            base.MeleeEffects(player, hitbox);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<VoidBar>(), 14);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
