using Microsoft.Xna.Framework;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
	public class StoneweaversStaff : AbilityItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stoneweaver's Staff");
            Tooltip.SetDefault("");
        }

        public override void SetDefaults()
        {
            item.damage = 17;
            item.mana = 4;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.magic = true;
            item.useTime = 35;
            item.useAnimation = 35;
            item.noMelee = true;
            item.knockBack = 2;
            item.value = 10000;
            item.rare = ItemRarityID.Green;
            item.UseSound = SoundID.Item8;
            item.shoot = ProjectileType<StoneweaversStaff_WeaversStone>();
            item.shootSpeed = 12f;
            item.autoReuse = true;
            item.useAmmo = ItemType<BlackIceChunk>();
        }

        public override string GetWeaponTooltip()
        {
            return "Uses Stone as ammo";
        }

        public override string GetQuote()
        {
            return "Throw another rock!";
        }

        public override string GetAbilityName(AbilityType type)
        {
            if (type == AbilityType.Q)
                return "Threaded Volley";
            else if (type == AbilityType.W)
                return "Seismic Shove";
            else
                return base.GetAbilityName(type);
        }

        public override string GetIconTexturePath(AbilityType type)
        {
            if (type == AbilityType.Q)
                return "AbilityImages/ThreadedVolley";
            else if (type == AbilityType.W)
                return "AbilityImages/SeismicShove";
            else
                return base.GetIconTexturePath(type);
        }

        public override string GetAbilityTooltip(AbilityType type)
        {
            if (type == AbilityType.Q)
            {
                return "Prepare 5 rock shards and throw them in the original cast direction";
            }
            else if (type == AbilityType.W)
            {
                return "Cause a huge rock to erupt from the ground, knocking back enemies";
            }
            else
            {
                return base.GetAbilityTooltip(type);
            }
        }

        public override int GetAbilityBaseDamage(Player player, AbilityType type)
        {
            if (type == AbilityType.Q)
                return (int)System.Math.Round(item.damage * 1.2);
            else if (type == AbilityType.W)
                return (int)System.Math.Round(item.damage * Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().magicDamageLastStep * 2);
            else
                return base.GetAbilityBaseDamage(player, type);
        }

        public override int GetAbilityScalingAmount(Player player, AbilityType type, DamageType dam)
        {
            if (type == AbilityType.Q)
            {
                if (dam == DamageType.MAG)
                    return 40;
            }
            return base.GetAbilityScalingAmount(player, type, dam);
        }

        public override string GetDamageTooltip(Player player, AbilityType type)
        {
            if (type == AbilityType.Q)
                return GetAbilityBaseDamage(player, type) + " + " + GetScalingTooltip(player, type, DamageType.MAG) + " magic damage per rock";
            else
                return base.GetDamageTooltip(player, type);
        }

        public override int GetBaseManaCost(AbilityType type)
        {
            if (type == AbilityType.Q)
                return 12;
            else if (type == AbilityType.W)
                return 20;
            else
                return base.GetBaseManaCost(type);
        }

        public override bool CanBeCastWhileUsingItem(AbilityType type)
        {
            if (type == AbilityType.Q)
                return true;
            else
                return false;
        }

        public override int GetRawCooldown(AbilityType type)
        {
            if (type == AbilityType.Q)
                return 10;
            else if (type == AbilityType.W)
                return 1;
            else
                return base.GetRawCooldown(type);
        }

        public override void DoEffect(Player player, AbilityType type)
        {
            if (type == AbilityType.Q)
            {
                if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetScaledManaCost(type), true))
                {
                    Vector2 position = player.MountedCenter;
                    Vector2 velocity = TerraLeague.CalcVelocityToMouse(player.Top, 1f);
                    int projType = ProjectileType<StoneweaversStaff_ThreadedVolley>();
                    int damage = GetAbilityBaseDamage(player, type) + GetAbilityScalingDamage(player, type, DamageType.MAG);
                    int knockback = 3;

                    Main.PlaySound(new LegacySoundStyle(2, 70), player.Center);

                    for (int i = 0; i < 5; i++)
                    {
                        Projectile.NewProjectile(position, velocity, projType, damage, knockback, player.whoAmI, i);
                    }
                    SetAnimation(player, item.useTime, item.useAnimation, position + velocity);
                    SetCooldowns(player, type);
                }
            }
            else if (type == AbilityType.W)
            {
                if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetScaledManaCost(type), true))
                {
                    bool overBlock = false;
                    Vector2 position = player.position;

                    if (Main.tile[(int)(Main.MouseWorld.X / 16), (int)(Main.MouseWorld.Y / 16)].active())
                    {
                        overBlock = Main.tileBlockLight[Main.tile[(int)(Main.MouseWorld.X / 16), (int)(Main.MouseWorld.Y / 16)].blockType()];
                    }

                    for (int i = (int)(Main.MouseWorld.Y / 16); i < Main.maxTilesY; i++)
                    {
                        if (overBlock && !Main.tileBlockLight[Main.tile[(int)(Main.MouseWorld.X / 16), i].blockType()])
                        {
                            overBlock = false;
                        }
                        if (!overBlock && Main.tile[(int)(Main.MouseWorld.X / 16), i].active())
                        {
                            if (Main.tileBlockLight[Main.tile[(int)(Main.MouseWorld.X / 16), i].blockType()])
                            {
                                position = new Vector2((int)(Main.MouseWorld.X), i * 16);
                                break;
                            }
                        }
                    }

                    Vector2 velocity = Vector2.Zero;
                    int projType = ProjectileType<StoneweaversStaff_SeismicShove>();
                    int damage = GetAbilityBaseDamage(player, type);
                    int knockback = 0;

                    Projectile.NewProjectile(position, velocity, projType, damage, knockback, player.whoAmI);
                    SetCooldowns(player, type);
                }
            }
            else
            {
                base.DoEffect(player, type);
            }
        }

        public override bool CanUseItem(Player player)
        {
            return player.HasAmmo(item, false);
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            return base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(0, -6);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddRecipeGroup("TerraLeague:GoldGroup", 10);
            recipe.AddIngredient(ItemID.Sandstone, 50);
            recipe.AddIngredient(ItemType<Sunstone>(), 10);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override bool GetIfAbilityExists(AbilityType type)
        {
            if (type == AbilityType.Q)
                return true;
            return base.GetIfAbilityExists(type);
        }
    }
}
