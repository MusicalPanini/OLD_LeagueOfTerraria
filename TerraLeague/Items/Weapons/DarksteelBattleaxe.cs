﻿using Microsoft.Xna.Framework;
using TerraLeague.Buffs;
using TerraLeague.Items.Weapons.Abilities;
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

            Abilities[(int)AbilityType.Q] = new Decimate(this);
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
    }
}
