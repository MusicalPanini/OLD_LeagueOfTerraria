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
            string text = "";

            if (modItem.item.type == ItemType<Maw>())
            {
                text = "[c/0099cc:Passive: LIFELINE -] [c/99e6ff:Negate the next projectile damage you take while below 30% life, and summon a 200 Magic Shield]" +
                    "\n[c/007399:" + (int)(cooldown * modPlayer.cdrLastStep) + " second cooldown]" +
                    "\n[c/007399:Triggering LIFELINE grants LIFEGRIP]" +
                    "\n[c/0099cc:Passive: LIFEGRIP -] [c/99e6ff:Grants 5% life steal, melee and ranged damage, and 5 resist]";

            }
            else if (modItem.item.type == ItemType<Steraks>())
            {
                text = "[c/0099cc:Passive: LIFELINE -] [c/99e6ff:Negate the next damage you take while below 30% life, and summon a 50% Max Life (" + modPlayer.GetRealHeathWithoutShield(true) / 2 + ") Shield]" +
                    "\n[c/007399:" + (int)(cooldown * modPlayer.cdrLastStep) + " second cooldown]" +
                    "\n[c/007399:Triggering LIFELINE grants STERAK'S FURY]" +
                    "\n[c/0099cc:Passive: STERAK'S FURY -] [c/99e6ff:Grants 20 defence and immunity to most debuffs]";

            }
            else if (modItem.item.type == ItemType<Hexdrinker>())
            {
                text = "[c/0099cc:Passive: LIFELINE -] [c/99e6ff:Negate the next projectile damage you take while below 30% life and summon an 80 Magic Shield]" +
                     "\n[c/007399:" + (int)(cooldown * modPlayer.cdrLastStep) + " second cooldown]";

            }
            else if (modItem.item.type == ItemType<PhantomDancer>())
            {
                text = "[c/0099cc:Passive: LIFELINE -] [c/99e6ff:Negate the next damage you take while below 30% life and summon a 100 Shield]" +
                    "\n[c/007399:" + (int)(cooldown * modPlayer.cdrLastStep) + " second cooldown]";
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
            if (Main.netMode == 1)
                PacketHandler.SendPassiveEfx(-1, player.whoAmI, player.whoAmI, modItem.item.type, FindIfPassiveIsSecondary(modItem));
        }

        public void TriggerLifeLine(Player player)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<Steraks>()) != -1)
            {
                if (modPlayer.lifeLineCooldown <= 0)
                {
                    modPlayer.AddShieldAttachedToBuff((int)(modPlayer.GetRealHeathWithoutShield(true) / 2 * modPlayer.healPower), BuffType<Buffs.SteraksFury>(), new Color(114, 18, 111), ShieldType.Basic);
                    player.AddBuff(BuffType<Buffs.SteraksFury>(), 720);
                }
            }
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<Maw>()) != -1)
            {
                if (modPlayer.lifeLineCooldown <= 0)
                {
                    modPlayer.AddShieldAttachedToBuff((int)(200 * modPlayer.healPower), BuffType<Buffs.LifeGrip>(), Color.Purple, ShieldType.Magic);
                    player.AddBuff(BuffType<Buffs.LifeGrip>(), 720);
                }
            }
            else if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<Hexdrinker>()) != -1)
            {
                if (modPlayer.lifeLineCooldown <= 0)
                {
                    modPlayer.AddShield((int)(80 * modPlayer.healPower), 720, Color.Purple, ShieldType.Magic);
                }
            }
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<PhantomDancer>()) != -1)
            {
                if (modPlayer.lifeLineCooldown <= 0)
                {
                    modPlayer.AddShield((int)(100 * modPlayer.healPower), 720, new Color(108, 203, 185), ShieldType.Basic);
                }
            }
        }

        override public void Efx(Player User)
        {
            Main.PlaySound(new LegacySoundStyle(2, 29).WithPitchVariance(-0.5f), User.position);
        }
    }
}
