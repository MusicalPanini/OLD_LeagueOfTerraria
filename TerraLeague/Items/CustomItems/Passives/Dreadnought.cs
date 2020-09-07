using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerraLeague.Buffs;
using TerraLeague.Items.CompleteItems;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CustomItems.Passives
{
    public class Dreadnought : Passive
    {
        double statModifier;

        public Dreadnought(double StatIncreaseModifier)
        {
            statModifier = StatIncreaseModifier;
        }

        public override string Tooltip(Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            return TerraLeague.CreateColorString(PassiveSubColor, "Moving will generate charge")
                + "\n" + TooltipName("DREADNOUGHT") + TerraLeague.CreateColorString(PassiveSecondaryColor, "Gain 0.25% movespeed per 1% charge")
                + "\n" + TerraLeague.CreateColorString(PassiveSecondaryColor, "At full charge your next attack will deal ") + TerraLeague.CreateScalingTooltip(TerraLeague.ARMORColor, "ARM", modPlayer.armorLastStep + modPlayer.defenceLastStep, 200) + TerraLeague.CreateColorString(PassiveSecondaryColor, " extra damage");
        }

        public override void UpdateAccessory(Player player, ModItem modItem)
        {
            base.UpdateAccessory(player, modItem);
        }

        public override void NPCHit(Item item, NPC target, ref int damage, ref float knockback, ref bool crit, ref int OnHitDamage, Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            if (modPlayer.crushingBlows)
            {
                modPlayer.meleeFlatDamage += 2 * modPlayer.armorLastStep;

                Efx(player, target);
                if (Main.netMode == NetmodeID.MultiplayerClient)
                    PacketHandler.SendPassiveEfx(-1, player.whoAmI, player.whoAmI, modItem.item.type, FindIfPassiveIsSecondary(modItem), target.whoAmI);

                modPlayer.accessoryStat[TerraLeague.FindAccessorySlotOnPlayer(player, modItem)] = 0;
            }

            base.NPCHit(item, target, ref damage, ref knockback, ref crit, ref OnHitDamage, player, modItem);
        }

        public override void NPCHitWithProjectile(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection, ref int OnHitDamage, Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            if (modPlayer.crushingBlows)
            {
                modPlayer.meleeFlatDamage += 2 * modPlayer.armorLastStep;
                modPlayer.rangedFlatDamage += 2 * modPlayer.armorLastStep;
                modPlayer.magicFlatDamage += 2 * modPlayer.armorLastStep;
                modPlayer.minionFlatDamage += 2 * modPlayer.armorLastStep;

                Efx(player, target);
                if (Main.netMode == NetmodeID.MultiplayerClient)
                    PacketHandler.SendPassiveEfx(-1, player.whoAmI, player.whoAmI, modItem.item.type, FindIfPassiveIsSecondary(modItem));

                modPlayer.accessoryStat[TerraLeague.FindAccessorySlotOnPlayer(player, modItem)] = 0;
            }

            base.NPCHitWithProjectile(proj, target, ref damage, ref knockback, ref crit, ref hitDirection, ref OnHitDamage, player, modItem);
        }

        public override void PostPlayerUpdate(Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            double stat;

            if (player.velocity.X < 0)
                stat = -player.velocity.X * statModifier;
            else
                stat = player.velocity.X * statModifier;

            AddStat(player, modItem, 100, stat);

            player.moveSpeed += (float)modPlayer.accessoryStat[TerraLeague.FindAccessorySlotOnPlayer(player, modItem)] * 0.0025f;

            if (modPlayer.accessoryStat[TerraLeague.FindAccessorySlotOnPlayer(player, modItem)] >= 100)
            {
                player.armorEffectDrawShadow = true;
                Dust dust = Dust.NewDustDirect(player.position, player.width, player.height, 5, 0, 0, 0, new Color(255, 0, 0, 150), 1.5f);
                dust.noGravity = true;
                player.AddBuff(BuffType<CrushingBlow>(), 2);
            }
            else
                player.ClearBuff(BuffType<CrushingBlow>());

            base.PostPlayerUpdate(player, modItem);
        }

        public override void Efx(Player user, NPC effectedNPC)
        {
            Main.PlaySound(new LegacySoundStyle(2, 38).WithVolume(1.2f), effectedNPC.position);
            for (int i = 0; i < 10; i++)
            {
                Dust.NewDustDirect(effectedNPC.position, effectedNPC.width, effectedNPC.height, 5, 0, 0, 0, new Color(255, 0, 0, 150), 1.5f);
            }
        }
    }
}
