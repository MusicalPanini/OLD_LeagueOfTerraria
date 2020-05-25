using Microsoft.Xna.Framework;
using TerraLeague.Buffs;
using TerraLeague.NPCs;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class DarksteelBattleaxe : AbilityItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Darksteel Battleaxe");
            Tooltip.SetDefault("");
        }

        public override string GetWeaponTooltip()
        {
            return "Struck enemies will start to 'Hemorrhage'" +
                "\nHemorrage stacks up to 5 times";
        }

        public override string GetQuote()
        {
            return "Death by my hand";
        }

        public override string GetAbilityName(AbilityType type)
        {
            if (type == AbilityType.Q)
                return "Decimate";
            else
                return base.GetAbilityName(type);
        }

        public override string GetIconTexturePath(AbilityType type)
        {
            if (type == AbilityType.Q)
                return "AbilityImages/Decimate";
            else
                return base.GetIconTexturePath(type);
        }

        public override string GetAbilityTooltip(AbilityType type)
        {
            if (type == AbilityType.Q)
            {
                return "Prepare your axe then spin it with great speed" +
                    "\nHeal " + (int)(7 * Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().healPowerLastStep) + " per enemy hit";
            }
            else
            {
                return base.GetAbilityTooltip(type);
            }
        }

        public override int GetAbilityBaseDamage(Player player, AbilityType type)
        {
            if (type == AbilityType.Q)
                return (int)(item.damage);
            else
                return base.GetAbilityBaseDamage(player, type);
        }

        public override int GetAbilityScalingAmount(Player player, AbilityType type, DamageType dam)
        {
            PLAYERGLOBAL modPlayer = Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>();
            if (type == AbilityType.Q)
            {
                if (dam == DamageType.MEL)
                    return 120;
            }
            return base.GetAbilityScalingAmount(player, type, dam);
        }

        public override int GetBaseManaCost(AbilityType type)
        {
            if (type == AbilityType.Q)
                return 30;
            else
                return base.GetBaseManaCost(type);
        }

        public override string GetDamageTooltip(Player player, AbilityType type)
        {
            if (type == AbilityType.Q)
                return GetAbilityBaseDamage(player, type) + " + " + GetScalingTooltip(player, type, DamageType.MEL) + " melee damage";
            else
                return base.GetDamageTooltip(player, type);
        }

        public override int GetRawCooldown(AbilityType type)
        {
            if (type == AbilityType.Q)
                return 8;
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
                    Vector2 velocity = Vector2.Zero;
                    int projType = ProjectileType<DarksteelBattleaxe_Decimate>();
                    int damage = GetAbilityBaseDamage(player, type) + GetAbilityScalingDamage(player, type, DamageType.MEL);
                    int knockback = 4;

                    Projectile.NewProjectile(position, velocity, projType, damage, knockback, player.whoAmI, player.direction);
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
            item.damage = 28;
            item.width = 54;
            item.height = 44;
            item.melee = true;
            item.useTime = 34;
            item.useAnimation = 34;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.knockBack = 5;
            item.value = 6000;
            item.rare = ItemRarityID.Orange;
            item.axe = 30;
            item.UseSound = SoundID.Item1;
            item.scale = 1.2f;
            item.autoReuse = true;
        }

        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            int stacks = target.GetGlobalNPC<NPCsGLOBAL>().HemorrhageStacks;

            if (stacks < 4)
            {
                target.GetGlobalNPC<NPCsGLOBAL>().HemorrhageStacks++;

                if (Main.netMode == NetmodeID.MultiplayerClient)
                    target.GetGlobalNPC<NPCsGLOBAL>().PacketHandler.SendSyncStats(Main.LocalPlayer.whoAmI, -1, 4, target.whoAmI, target.GetGlobalNPC<NPCsGLOBAL>().HemorrhageStacks);
            }

            target.AddBuff(BuffType<Hemorrhage>(), 300);
            base.OnHitNPC(player, target, damage, knockBack, crit);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<DarksteelBar>(), 16);
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
