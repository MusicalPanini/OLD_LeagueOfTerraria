using Microsoft.Xna.Framework;
using TerraLeague.Buffs;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class AtlasGauntlets : AbilityItem
    {
        public override bool OnlyShootOnSwing => false;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Atlas Gauntlets");
            Tooltip.SetDefault("");
        }

        public override string GetWeaponTooltip()
        {
            return "";
        }

        public override string GetQuote()
        {
            return "Here comes the punch line!";
        }

        public override string GetAbilityName(AbilityType type)
        {
            if (type == AbilityType.E)
                return "Excessive Force";
            else
                return base.GetAbilityName(type);
        }

        public override string GetIconTexturePath(AbilityType type)
        {
            if (type == AbilityType.E)
                return "AbilityImages/ExcessiveForce";
            else
                return base.GetIconTexturePath(type);
        }

        public override string GetAbilityTooltip(AbilityType type)
        {
            if (type == AbilityType.E)
            {
                return "Your next melee attack will deal 300% more damage and splash outward";
            }
            else
            {
                return base.GetAbilityTooltip(type);
            }
        }

        public override int GetAbilityBaseDamage(Player player, AbilityType type)
        {
            if (type == AbilityType.E)
                return (int)0;
            else
                return base.GetAbilityBaseDamage(player, type);
        }

        public override int GetBaseManaCost(AbilityType type)
        {
            if (type == AbilityType.E)
                return 30;
            else
                return base.GetBaseManaCost(type);
        }

        public override string GetDamageTooltip(Player player, AbilityType type)
        {
            if (type == AbilityType.E)
                return "";
            else
                return base.GetDamageTooltip(player, type);
        }

        public override bool CanBeCastWhileUsingItem(AbilityType type)
        {
            if (type == AbilityType.E)
                return true;
            else
                return false;
        }

        public override int GetRawCooldown(AbilityType type)
        {
            if (type == AbilityType.E)
                return 16;
            else
                return base.GetRawCooldown(type);
        }

        public override void DoEffect(Player player, AbilityType type)
        {
            if (type == AbilityType.E)
            {
                if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetScaledManaCost(type), true))
                {
                    player.AddBuff(BuffType<ExcessiveForce>(), 240);
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
            item.damage = 70;
            item.width = 52;          
            item.height = 30;         
            item.melee = true;        
            item.useTime = 10;
            item.useAnimation = 10;
            item.useStyle = ItemUseStyleID.HoldingOut;          
            item.knockBack = 6;    
            item.value = 100000;
            item.rare = ItemRarityID.Pink; 
            item.shoot = ProjectileType<AtlasGauntlets_Right>();
            item.shootSpeed = 10;
            item.noUseGraphic = true;
            item.noMelee = true;
            item.autoReuse = true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.ownedProjectileCounts[ProjectileType<AtlasGauntlets_Left>()] > 0 && player.ownedProjectileCounts[ProjectileType<AtlasGauntlets_Right>()] > 0)
                return false;
            return base.CanUseItem(player);
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            item.knockBack = 6;
            if (player.ownedProjectileCounts[ProjectileType<AtlasGauntlets_Right>()] == 0)
            {
                type = ProjectileType<AtlasGauntlets_Right>();
                return true;
            }
            else if (player.ownedProjectileCounts[ProjectileType<AtlasGauntlets_Left>()] == 0)
            {
                type = ProjectileType<AtlasGauntlets_Left>();
                return true;
            }

            return false;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<PerfectHexCore>());
            recipe.AddRecipeGroup("TerraLeague:Tier3Bar", 14);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override bool GetIfAbilityExists(AbilityType type)
        {
            if (type == AbilityType.E)
                return true;
            return base.GetIfAbilityExists(type);
        }

        public override void Efx(Player player, AbilityType type)
        {
            if (type == AbilityType.E)
            {
                Microsoft.Xna.Framework.Audio.SoundEffectInstance sound = Main.PlaySound(new Terraria.Audio.LegacySoundStyle(42, 24), player.Center);
                if (sound != null)
                    sound.Pitch = -0.5f;

                for (int j = 0; j < 10; j++)
                {
                    Dust dust = Dust.NewDustDirect(player.position, player.width, player.height, DustID.Fire, 0, -10);
                    dust.velocity.X *= 0;
                    dust.velocity.Y -= 4;
                    dust.noGravity = true;
                    dust.scale = 2;
                }
            }
        }
    }
}
