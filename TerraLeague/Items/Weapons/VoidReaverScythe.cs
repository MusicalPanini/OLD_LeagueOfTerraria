using Microsoft.Xna.Framework;
using TerraLeague.Buffs;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class VoidReaverScythe : AbilityItem
    {
        public override bool OnlyShootOnSwing => false;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Voidreaver Scythe");
            Tooltip.SetDefault("");
        }

        public override string GetWeaponTooltip()
        {
            return "";
        }

        public override string GetQuote()
        {
            return "Fear the Void";
        }

        public override string GetAbilityName(AbilityType type)
        {
            if (type == AbilityType.E)
                return "Evolved Wings";
            else
                return base.GetAbilityName(type);
        }

        public override string GetIconTexturePath(AbilityType type)
        {
            if (type == AbilityType.E)
                return "AbilityImages/EvolvedWings";
            else
                return base.GetIconTexturePath(type);
        }

        public override string GetAbilityTooltip(AbilityType type)
        {
            if (type == AbilityType.E)
            {
                return "Gain wings and 50% increased melee damage for 4 seconds";
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
                return 50;
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
                return 22;
            else
                return base.GetRawCooldown(type);
        }

        public override void DoEffect(Player player, AbilityType type)
        {
            if (type == AbilityType.E)
            {
                if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetScaledManaCost(type), true))
                {
                    player.AddBuff(BuffType<EvolvedWings>(), 240);
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
            item.damage = 32;
            item.width = 46;          
            item.height = 44;         
            item.melee = true;        
            item.useTime = 52;        
            item.useAnimation = 26;
            item.useStyle = 1;          
            item.knockBack = 3;    
            item.value = 5400;
            item.rare = 3; 
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.shoot = ProjectileType<VoidSpike>();
            item.shootSpeed = 18;
            item.scale = 1.3f;
        }

        public override void OnHitNPC(Player player, NPC target, int damage, float knockback, bool crit)
        {
            item.useTurn = false;

        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 velocity = new Vector2(speedX, speedY);

            Projectile.NewProjectile(position, velocity, type, damage, knockBack, player.whoAmI);
            Projectile.NewProjectile(position, velocity.RotatedBy(0.3f), type, damage, knockBack, player.whoAmI);
            Projectile.NewProjectile(position, velocity.RotatedBy(-0.3f), type, damage, knockBack, player.whoAmI);



            return false;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<VoidFragment>(), 64);
            recipe.AddTile(TileID.DemonAltar);
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
                Main.PlaySound(SoundID.NPCDeath1, player.MountedCenter);

                for (int i = 0; i < 10; i++)
                {
                    Dust dust = Dust.NewDustDirect(player.position, player.width, player.height, 97, -3 * player.direction, -2);
                    dust.scale = 2;
                }
            }
        }
    }
}
