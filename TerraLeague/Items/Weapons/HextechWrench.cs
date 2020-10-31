﻿using Microsoft.Xna.Framework;
using TerraLeague.Buffs;
using TerraLeague.Items.Weapons.Abilities;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class HextechWrench : AbilityItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hextech Wrench");
            Tooltip.SetDefault("");
            base.SetStaticDefaults();
        }

        public override string GetWeaponTooltip()
        {
            return "Create a H-28G Evolution Turret to fight for you!";
        }

        public override string GetQuote()
        {
            return "Stand back! I am about to do...science!";
        }

        public override void SetDefaults()
        {
            item.damage = 8;
            item.sentry = true;
            item.summon = true;
            item.mana = 20;
            item.width = 32;
            item.height = 32;
            item.useTime = 36;
            item.useAnimation = 36;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.noMelee = true;
            item.knockBack = 1;
            item.value = 1000;
            item.rare = ItemRarityID.Blue;
            item.UseSound = new LegacySoundStyle(2, 113);
            item.shoot = ProjectileType<HextechWrench_EvolutionTurret>();

            Abilities[(int)AbilityType.W] = new MicroRockets(this);
            Abilities[(int)AbilityType.E] = new StormGrenade(this);
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            player.FindSentryRestingSpot(item.shoot, out int xPos, out int yPos, out int yDis);
            Projectile.NewProjectile((float)xPos, (float)(yPos - yDis) + 3, 0f, 0f, type, damage, knockBack, player.whoAmI, 10, -1);
            player.UpdateMaxTurrets();

            return false;
        }
    }
}