using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    public class CrystalStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Crystal Staff");
            Tooltip.SetDefault("");
            base.SetStaticDefaults();
            Item.staff[item.type] = true;
        }

        public override void SetDefaults()
        {
            item.damage = 9;
            item.magic = true;
            item.noMelee = true;
            item.mana = 4;
            item.width = 16;
            item.height = 32;
            item.useTime = 37;
            item.useAnimation = 37;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.knockBack = 3.5f;
            item.value = 1000;
            item.rare = ItemRarityID.Blue;
            item.shootSpeed = 6.5f;
            item.UseSound = SoundID.Item20;
            item.autoReuse = true;
            item.shoot = ProjectileID.SapphireBolt;

            AbilityItemGLOBAL abilityItem = item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.W, new DarkMatter(this));
            abilityItem.SetAbility(AbilityType.R, new PrimordialBurst(this));
            abilityItem.ChampQuote = "You deny the darkness in your soul. You deny your power!";
            abilityItem.IsAbilityItem = true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            return true;
        }

        public override void OnCraft(Recipe recipe)
        {
            Main.LocalPlayer.QuickSpawnItem(ItemID.ManaCrystal, 1);

            base.OnCraft(recipe);
        }
    }
}
