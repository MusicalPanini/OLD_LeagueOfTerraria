using Microsoft.Xna.Framework;
using System.Collections.Generic;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class XanCrestBlades : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Xan Crest Blades");

            Tooltip.SetDefault("Manipulate flowing blades that deal damage based on their speed" +
                "\nCan stack up to 12" +
                "\n[c/cc9900:'Cut them down!']");
        }

        public override void SetDefaults()
        {
            item.damage = 34;
            item.width = 62;
            item.height = 26;
            item.melee = true;
            item.useTime = 25;
            item.useAnimation = 25;
            item.useStyle = ItemUseStyleID.Stabbing;
            item.knockBack = 0;
            item.value = 10000;
            item.rare = ItemRarityID.Orange;
            item.UseSound = new LegacySoundStyle(2, 82, Terraria.Audio.SoundType.Sound);
            item.shootSpeed = 15f;
            item.shoot = ProjectileType<XanCrestBlades_DancingBlade>();
            item.noMelee = true;
            item.useTurn = true;
            item.autoReuse = false;
            item.noUseGraphic = true;
            item.maxStack = 12;
        }

        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[ProjectileType<XanCrestBlades_DancingBlade>()] < 1;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            for (int i = 0; i < item.stack; i++)
            {
                Projectile.NewProjectileDirect(position, Vector2.Zero, ProjectileType<XanCrestBlades_DancingBlade>(), damage, knockBack, player.whoAmI, i).Center = player.Center;
            }

            return false;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<SilversteelBar>(), 3);
            recipe.AddIngredient(ItemID.HallowedBar, 3);
            recipe.AddIngredient(ItemID.SoulofMight, 3);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
