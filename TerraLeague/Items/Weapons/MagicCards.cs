using Microsoft.Xna.Framework;
using TerraLeague.Items.Weapons.Abilities;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class MagicCards : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Card Masters Deck");
            Tooltip.SetDefault("");
        }

        string GetWeaponTooltip()
        {
            return "Has a chance to throw a special card" +
                "\n" + TerraLeague.CreateColorString("0066ff", "Blue Card") + " - Restore 25 mana on hit" +
                "\n" + TerraLeague.CreateColorString("ffff4d", "Yellow Card") + " - Applies 'Stunned'" +
                "\n" + TerraLeague.CreateColorString("ff1a1a", "Red Card") + " - Explodes on contact";
        }

        public override void SetDefaults()
        {
            item.damage = 12;
            item.width = 24;
            item.height = 24;
            item.magic = true;
            item.useTime = 28;
            item.useAnimation = 28;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.knockBack = 2;
            item.mana = 6;
            item.value = 3500;
            item.rare = ItemRarityID.Green;
            item.UseSound = new LegacySoundStyle(2, 19, Terraria.Audio.SoundType.Sound);
            item.shootSpeed = 15f;
            item.shoot = ProjectileType<MagicCards_GreenCard>();
            item.noMelee = true;
            item.useTurn = true;
            item.autoReuse = true;
            item.noUseGraphic = true;

            AbilityItemGLOBAL abilityItem = item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.Q, new WildCards(this));
            abilityItem.ChampQuote = "Lady luck is smilin'";
            abilityItem.getWeaponTooltip = GetWeaponTooltip;
            abilityItem.IsAbilityItem = true;
        }

        public override bool CanUseItem(Player player)
        {
            return base.CanUseItem(player);
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (Main.rand.Next(0, 5) == 0)
            {
                switch (Main.rand.Next(0, 3))
                {
                    case 0:
                        type = ProjectileType<MagicCards_RedCard>();
                        knockBack *= 2;
                        damage = (int)(damage * 1.25);
                        break;
                    case 1:
                        type = ProjectileType<MagicCards_BlueCard>();
                        damage = (int)(damage * 1.5);
                        break;
                    case 2:
                        type = ProjectileType<MagicCards_YellowCard>();
                        knockBack = 0;
                        break;
                    default:
                        break;
                }
            }
            return true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<BrassBar>(), 14);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
