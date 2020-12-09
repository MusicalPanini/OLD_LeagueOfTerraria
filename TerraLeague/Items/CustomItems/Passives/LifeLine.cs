using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerraLeague.Items.AdvItems;
using TerraLeague.Items.CompleteItems;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CustomItems.Passives
{
    public class Lifeline : Passive
    {
        int cooldown;

        public Lifeline(int Cooldown)
        {
            cooldown = Cooldown;
        }

        public override string Tooltip(Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            string text = TooltipName("LIFELINE");

            if (modItem.item.type == ItemType<Maw>())
            {
                text += TerraLeague.CreateColorString(PassiveSecondaryColor, "Negate the next projectile damage you take while below 30% life, and summon a ") + TerraLeague.CreateScalingTooltip(DamageType.NONE, 200, 100, true) + TerraLeague.CreateColorString(PassiveSecondaryColor, " Magic Shield") +
                    "\n" + TerraLeague.CreateColorString(PassiveSubColor, (int)(cooldown * modPlayer.ItemCdrLastStep) + " second cooldown\nTriggering LIFELINE grants LIFEGRIP") +
                    "\n" + TooltipName("LIFEGRIP") + TerraLeague.CreateColorString(PassiveSecondaryColor, "Grants 5% life steal, melee and ranged damage, and 5 resist");
            }
            else if (modItem.item.type == ItemType<Steraks>())
            {
                text += TerraLeague.CreateColorString(PassiveSecondaryColor, "Negate the next damage you take while below 30% life, and summon a ") + TerraLeague.CreateScalingTooltip(UI.HealthbarUI.RedHealthColor.Hex3(), "LIFE", modPlayer.GetRealHeathWithoutShield(true), 50, true) + TerraLeague.CreateColorString(PassiveSecondaryColor," Shield") +
                    "\n" + TerraLeague.CreateColorString(PassiveSubColor, (int)(cooldown * modPlayer.ItemCdrLastStep) + " second cooldown\nTriggering LIFELINE grants STERAK'S FURY") +
                    "\n" + TooltipName("STERAK'S FURY") + TerraLeague.CreateColorString(PassiveSecondaryColor, "Grants 20 defence and immunity to most debuffs");

            }
            else if (modItem.item.type == ItemType<Hexdrinker>())
            {
                text += TerraLeague.CreateColorString(PassiveSecondaryColor, "Negate the next projectile damage you take while below 30% life and summon a ") + TerraLeague.CreateScalingTooltip(DamageType.NONE, 80, 100, true) + TerraLeague.CreateColorString(PassiveSecondaryColor, " Magic Shield") +
                    "\n" + TerraLeague.CreateColorString(PassiveSubColor, (int)(cooldown * modPlayer.ItemCdrLastStep) + " second cooldown");

            }
            else if (modItem.item.type == ItemType<PhantomDancer>())
            {
                text += TerraLeague.CreateColorString(PassiveSecondaryColor, "Negate the next damage you take while below 30% life and summon a ") + TerraLeague.CreateScalingTooltip(DamageType.NONE, 100, 100, true) + TerraLeague.CreateColorString(PassiveSecondaryColor, "Shield") +
                    "\n" + TerraLeague.CreateColorString(PassiveSubColor, (int)(cooldown * modPlayer.ItemCdrLastStep) + " second cooldown");
            }

            return text;
        }

        public override void UpdateAccessory(Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            modPlayer.accessoryStat[TerraLeague.FindAccessorySlotOnPlayer(player, modItem)] = modPlayer.lifeLineCooldown;
            base.UpdateAccessory(player, modItem);
        }

        public override void PostPlayerUpdate(Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            if (modPlayer.lifeLineCooldown <= 0)
            {
                if (TerraLeague.FindAccessorySlotOnPlayer(player, modItem) != -1)
                {
                    if (modItem.item.type == ItemType<Maw>() || modItem.item.type == ItemType<Hexdrinker>())
                    {
                        player.armorEffectDrawShadow = true;
                        if (Main.rand.Next(0, 5) == 0)
                        {
                            Dust dust = Dust.NewDustDirect(player.position, player.width, player.height, 16, 0, 0, 0, new Color(255, 0, 255, 150));
                            dust.velocity.X = 0;
                            dust.velocity.Y = -Math.Abs(dust.velocity.Y);
                            dust.noGravity = true;
                        }
                    }
                }
            }

            base.PostPlayerUpdate(player, modItem);
        }

        public override void OnHitByNPC(NPC npc, ref int damage, ref bool crit, Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            if (player.statLifeMax2 / 3 >= modPlayer.GetRealHeathWithoutShield())
            {
                if (modPlayer.lifeLineCooldown <= 0)
                {
                    if (TerraLeague.FindAccessorySlotOnPlayer(player, modItem) != -1)
                    {
                        if (modItem.item.type == ItemType<Steraks>())
                            DoEffect(player, modItem);
                        if (modItem.item.type == ItemType<PhantomDancer>())
                            DoEffect(player, modItem);
                    }
                }
            }

            base.OnHitByNPC(npc, ref damage, ref crit, player, modItem);
        }

        public override void OnHitByProjectile(NPC npc, ref int damage, ref bool crit, Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            if (player.statLifeMax2 / 3 >= modPlayer.GetRealHeathWithoutShield())
            {
                if (modPlayer.lifeLineCooldown <= 0)
                {
                    if (TerraLeague.FindAccessorySlotOnPlayer(player, modItem) != -1)
                    {
                        if (modItem.item.type == ItemType<Steraks>())
                            DoEffect(player, modItem);
                        if (modItem.item.type == ItemType<PhantomDancer>())
                            DoEffect(player, modItem);
                        if (modItem.item.type == ItemType<Maw>())
                            DoEffect(player, modItem);
                        if (modItem.item.type == ItemType<Hexdrinker>())
                            DoEffect(player, modItem);
                    }
                }
            }
                
            base.OnHitByProjectile(npc, ref damage, ref crit, player, modItem);
        }

        public override void OnHitByProjectile(Projectile proj, ref int damage, ref bool crit, Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            if (player.statLifeMax2 / 3 >= modPlayer.GetRealHeathWithoutShield())
            {
                if (modPlayer.lifeLineCooldown <= 0)
                {
                    if (TerraLeague.FindAccessorySlotOnPlayer(player, modItem) != -1)
                    {
                        if (modItem.item.type == ItemType<Steraks>())
                            DoEffect(player, modItem);
                        if (modItem.item.type == ItemType<PhantomDancer>())
                            DoEffect(player, modItem);
                        if (modItem.item.type == ItemType<Maw>())
                            DoEffect(player, modItem);
                        if (modItem.item.type == ItemType<Hexdrinker>())
                            DoEffect(player, modItem);
                    }
                }
            }

            base.OnHitByProjectile(proj, ref damage, ref crit, player, modItem);
        }

        public void DoEffect(Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            player.endurance = 1;
            TriggerLifeLine(player);
            modPlayer.lifeLineCooldown = (int)(cooldown * modPlayer.Cdr * 60);
            Efx(player);
            SendEfx(player, modItem);
        }

        public void TriggerLifeLine(Player player)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<Steraks>()) != -1)
            {
                if (modPlayer.lifeLineCooldown <= 0)
                {
                    modPlayer.AddShieldAttachedToBuff(modPlayer.ScaleValueWithHealPower(modPlayer.GetRealHeathWithoutShield(true) / 2), BuffType<Buffs.SteraksFury>(), new Color(114, 18, 111), ShieldType.Basic);
                    player.AddBuff(BuffType<Buffs.SteraksFury>(), 720);
                }
            }
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<Maw>()) != -1)
            {
                if (modPlayer.lifeLineCooldown <= 0)
                {
                    modPlayer.AddShieldAttachedToBuff(modPlayer.ScaleValueWithHealPower(200), BuffType<Buffs.LifeGrip>(), Color.Purple, ShieldType.Magic);
                    player.AddBuff(BuffType<Buffs.LifeGrip>(), 720);
                }
            }
            else if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<Hexdrinker>()) != -1)
            {
                if (modPlayer.lifeLineCooldown <= 0)
                {
                    modPlayer.AddShield(modPlayer.ScaleValueWithHealPower(80), 720, Color.Purple, ShieldType.Magic);
                }
            }
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<PhantomDancer>()) != -1)
            {
                if (modPlayer.lifeLineCooldown <= 0)
                {
                    modPlayer.AddShield(modPlayer.ScaleValueWithHealPower(100), 720, new Color(108, 203, 185), ShieldType.Basic);
                }
            }
        }

        override public void Efx(Player User)
        {
            Main.PlaySound(new LegacySoundStyle(2, 29).WithPitchVariance(-0.5f), User.position);
        }
    }
}
