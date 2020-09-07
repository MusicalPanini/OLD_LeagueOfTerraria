using Microsoft.Xna.Framework;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class BrackernStinger : AbilityItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Brackern Stinger");
            Tooltip.SetDefault("");
        }

        public override string GetQuote()
        {
            return "Feel my sting!";
        }

        public override string GetAbilityName(AbilityType type)
        {
            if (type == AbilityType.W)
                return "Crystalline Exoskeleton";
            else
                return base.GetAbilityName(type);
        }

        public override string GetIconTexturePath(AbilityType type)
        {
            if (type == AbilityType.W)
                return "AbilityImages/CrystallineExoskeleton";
            else
                return base.GetIconTexturePath(type);
        }

        public override string GetAbilityTooltip(AbilityType type)
        {
            if (type == AbilityType.W)
            {
                return "Gain 'Swiftness' and 10% of your max life as a shield for 6 seconds";
            }
            else
            {
                return base.GetAbilityTooltip(type);
            }
        }

        public override int GetAbilityBaseDamage(Player player, AbilityType type)
        {
            if (type == AbilityType.W)
                return (int)((player.GetModPlayer<PLAYERGLOBAL>().maxLifeLastStep/10));
            else
                return base.GetAbilityBaseDamage(player, type);
        }

        public override int GetBaseManaCost(AbilityType type)
        {
            if (type == AbilityType.W)
                return 45;
            else
                return base.GetBaseManaCost(type);
        }

        public override string GetDamageTooltip(Player player, AbilityType type)
        {
            if (type == AbilityType.W)
                return TerraLeague.CreateScalingTooltip(UI.HealthbarUI.RedHealthColor.Hex3(), "LIFE", GetAbilityBaseDamage(player, type), 100, true) + " shielding";
            else
                return base.GetDamageTooltip(player, type);
        }

        public override string GetTooltip(AbilityType type)
        {
            return base.GetTooltip(type);
        }

        public override bool CanBeCastWhileUsingItem(AbilityType type)
        {
            if (type == AbilityType.W)
                return true;
            else
                return false;
        }

        public override int GetRawCooldown(AbilityType type)
        {
            if (type == AbilityType.W)
                return 12;
            else
                return base.GetRawCooldown(type);
        }

        public override void DoEffect(Player player, AbilityType type)
        {
            if (type == AbilityType.W)
            {
                if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetScaledManaCost(type), true))
                {
                    PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

                    player.AddBuff(BuffID.Swiftness, 360);
                    modPlayer.AddShield((int)(GetAbilityBaseDamage(player, type) * modPlayer.healPowerLastStep), 360, new Color(181,77,177), ShieldType.Basic);
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
            item.damage = 9;
            item.width = 32;
            item.height = 32;
            item.melee = true;
            item.useTime = 45;
            item.useAnimation = 45;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.knockBack = 2;
            item.value = 10000;
            item.rare = ItemRarityID.Green;
            item.UseSound = new LegacySoundStyle(2, 101);
            item.shootSpeed = 1f;
            item.shoot = ProjectileType<BrackernStinger_Whip>();
            item.noMelee = true;
            item.noUseGraphic = true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.ownedProjectileCounts[ProjectileType<BrackernStinger_Whip>()] < 1)
                return base.CanUseItem(player);
            return false;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {


            Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI, 0,
                Main.rand.Next(-100, 100) * 0.001f * player.gravDir);
            return false;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.AntlionMandible, 6);
            recipe.AddRecipeGroup("TerraLeague:GoldGroup", 10);
            recipe.AddIngredient(ItemID.Amethyst, 1);
            recipe.AddIngredient(ItemType<Sunstone>(), 10);
            recipe.AddTile(TileID.Anvils);
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
                TerraLeague.PlaySoundWithPitch(player.MountedCenter, 2, 27, -0.5f);
            }
            base.Efx(player, type);
        }
    }
}
