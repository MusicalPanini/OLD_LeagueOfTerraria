using Microsoft.Xna.Framework;
using TerraLeague.Buffs;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class MouthoftheAbyss : AbilityItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mouth of the Abyss");
            Tooltip.SetDefault("");
        }

        public override string GetWeaponTooltip()
        {
            return "33% chance to not consume ammo";
        }

        public override string GetQuote()
        {
            return "Hunger never sleep";
        }

        public override string GetAbilityName(AbilityType type)
        {
            if (type == AbilityType.W)
                return "Bio-Arcane Barrage";
            else
                return base.GetAbilityName(type);
        }

        public override string GetIconTexturePath(AbilityType type)
        {
            if (type == AbilityType.W)
                return "AbilityImages/Bio-ArcaneBarrage";
            else
                return base.GetIconTexturePath(type);
        }

        public override string GetAbilityTooltip(AbilityType type)
        {
            if (type == AbilityType.W)
            {
                return "Your ranged attacks deal On Hit damage based on targets max life for 3 seconds.";
            }
            else
            {
                return base.GetAbilityTooltip(type);
            }
        }

        public override int GetAbilityBaseDamage(Player player, AbilityType type)
        {
            return base.GetAbilityBaseDamage(player, type);
        }

        public override int GetAbilityScalingAmount(Player player, AbilityType type, DamageType dam)
        {
            if (type == AbilityType.W)
            {
                if (dam == DamageType.MAG)
                    return 5;
            }
            return base.GetAbilityScalingAmount(player, type, dam);
        }

        public override int GetBaseManaCost(AbilityType type)
        {
            if (type == AbilityType.W)
                return 40;
            else
                return base.GetBaseManaCost(type);
        }

        public override bool CanBeCastWhileUsingItem(AbilityType type)
        {
            if (type == AbilityType.W)
                return true;
            return base.CanBeCastWhileUsingItem(type);
        }

        public override string GetDamageTooltip(Player player, AbilityType type)
        {
            TerraLeague.CreateScalingTooltip(DamageType.MAG, player.GetModPlayer<PLAYERGLOBAL>().MAG, 5, false, "%");

            if (type == AbilityType.W)
                return "4% + " + TerraLeague.CreateScalingTooltip(DamageType.MAG, player.GetModPlayer<PLAYERGLOBAL>().MAG, 5, false, "%") + " of targets max life On Hit" +
                    "\nMax: 20 + " + TerraLeague.CreateScalingTooltip(DamageType.MAG, player.GetModPlayer<PLAYERGLOBAL>().MAG, 50) + " damage";
            else
                return base.GetDamageTooltip(player, type);
        }

        public override int GetRawCooldown(AbilityType type)
        {
            if (type == AbilityType.W)
                return 20;
            else
                return base.GetRawCooldown(type);
        }

        public override void DoEffect(Player player, AbilityType type)
        {
            if (type == AbilityType.W)
            {
                if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetScaledManaCost(type), true))
                {
                    player.AddBuff(BuffType<BioArcaneBarrage>(), 60 * 3);
                    DoEfx(player, type);
                    SetCooldowns(player, type);
                }
            }
            else
            {
                base.DoEffect(player, type);
            }
        }

        public override void SetDefaults()
        {
            item.damage = 26;
            item.ranged = true;
            item.width = 44;
            item.height = 52;
            item.useAnimation = 8;
            item.useTime = 8;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.noMelee = true;
            item.knockBack = 1;
            item.value = 120000;
            item.rare = ItemRarityID.Lime;
            item.UseSound = new Terraria.Audio.LegacySoundStyle(2, 11);
            item.shoot = ProjectileID.PurificationPowder;
            item.autoReuse = true;
            item.shootSpeed = 13f;
            item.useAmmo = AmmoID.Bullet;
        }

        public override bool CanUseItem(Player player)
        {
            return true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            position.Y += 8;
            
            int numberProjectiles = Main.rand.Next(0,4) != 0 ? 0 : Main.rand.Next(1, 4);
            for (int i = 0; i < numberProjectiles; i++)
            {
                Vector2 perturbedSpeed = new Vector2(speedX + Main.rand.NextFloat(-2, 2), speedY + Main.rand.NextFloat(-1, 1)).RotatedByRandom(MathHelper.ToRadians(8));
                Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, ProjectileType<MouthoftheAbyss_AcidBlob>(), damage, knockBack, player.whoAmI);
            }

            return true;
        }

        public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
        {
            base.ModifyWeaponDamage(player, ref add, ref mult, ref flat);
        }

        public override bool ConsumeAmmo(Player player)
        {
            return Main.rand.Next(0, 100) < 33;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(0, 0);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<VoidBar>(), 14);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override bool GetIfAbilityExists(AbilityType type)
        {
            if (type == AbilityType.W)
                return true;
            return base.GetIfAbilityExists(type);
        }

        public override void Efx(Player player, AbilityType type)
        {
            if (type == AbilityType.W)
            {
                TerraLeague.PlaySoundWithPitch(player.MountedCenter, 2, 95, -1f);
            }
        }

        public static int GetMaxOnHit(PLAYERGLOBAL player)
        {
            return (int)(20 + (player.MAG * 0.05 * 10));
        }
    }
}
