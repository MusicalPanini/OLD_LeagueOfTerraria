using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader.IO;
using Terraria.Utilities;
using Terraria.ModLoader;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using System.Linq;
using TerraLeague.Items.Armor;
using TerraLeague.Buffs;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items
{
    public class TerraLeagueITEMGLOBAL : GlobalItem
    {
        //public bool meleeProjCooldown = false;

        public override void SetDefaults(Item item)
        {
            if (item.type == ItemID.StoneBlock || item.type == ItemID.EbonstoneBlock || item.type == ItemID.CrimstoneBlock || item.type == ItemID.PearlstoneBlock)
            {
                item.ammo = ItemType<BlackIceChunk>();
            }

            if (item.type == ItemID.PirateHat)
            {
                item.vanity = false;
                item.defense = 3;
            }
            if (item.type == ItemID.PirateShirt)
            {
                item.vanity = false;
                item.defense = 4;
            }
            if (item.type == ItemID.PiratePants)
            {
                item.vanity = false;
                item.defense = 4;
            }

            if (item.type == ItemID.BuccaneerBandana)
            {
                item.SetNameOverride("Cannoneer Bandana");
                item.vanity = false;
                item.defense = 9;
                item.rare = ItemRarityID.LightRed;
            }
            if (item.type == ItemID.BuccaneerShirt)
            {
                item.SetNameOverride("Cannoneer Shirt");
                item.vanity = false;
                item.defense = 12;
                item.rare = ItemRarityID.LightRed;
            }
            if (item.type == ItemID.BuccaneerPants)
            {
                item.SetNameOverride("Cannoneer Pants");
                item.vanity = false;
                item.defense = 9;
                item.rare = ItemRarityID.LightRed;
            }
            base.SetDefaults(item);
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (item.summon)
            {
                TooltipLine tt2 = tooltips.FirstOrDefault(x => x.Name == "Damage" && x.mod == "Terraria");
                if (tt2 != null)
                {
                    string[] splitText = tt2.text.Split(' ');
                    int damageValue = Convert.ToInt32(splitText.First());
                    string damageWord = splitText.Last();
                    tt2.text = Math.Round(item.damage * Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().minionDamageLastStep) + " minion damage";
                }
            }

            if (item.type == ItemID.PirateHat)
            {
                if (tooltips.FirstOrDefault(x => x.Name == "SocialDesc" && x.mod == "Terraria") == null)
                    tooltips.Insert(3,new TooltipLine(mod, "Tooltip0", "5% increased ranged damage"));
            }
            if (item.type == ItemID.PirateShirt)
            {
                if (tooltips.FirstOrDefault(x => x.Name == "SocialDesc" && x.mod == "Terraria") == null)
                    tooltips.Insert(3,new TooltipLine(mod, "Tooltip0", "3% increased ranged crit chance"));
            }
            if (item.type == ItemID.PiratePants)
            {
                if (tooltips.FirstOrDefault(x => x.Name == "SocialDesc" && x.mod == "Terraria") == null)
                    tooltips.Insert(3,new TooltipLine(mod, "Tooltip0", "10% chance to not consume ammo"));
            }

            if (item.type == ItemID.BuccaneerBandana)
            {
                if (tooltips.FirstOrDefault(x => x.Name == "SocialDesc" && x.mod == "Terraria") == null)
                    tooltips.Insert(3, new TooltipLine(mod, "Tooltip0", "15% increased ranged damage"));
            }
            if (item.type == ItemID.BuccaneerShirt)
            {
                if (tooltips.FirstOrDefault(x => x.Name == "SocialDesc" && x.mod == "Terraria") == null)
                    tooltips.Insert(3, new TooltipLine(mod, "Tooltip0", "7% increased ranged crit chance"));
            }
            if (item.type == ItemID.BuccaneerPants)
            {
                if (tooltips.FirstOrDefault(x => x.Name == "SocialDesc" && x.mod == "Terraria") == null)
                    tooltips.Insert(3, new TooltipLine(mod, "Tooltip0", "15% chance to not consume ammo"));
            }

            base.ModifyTooltips(item, tooltips);
        }

        public override float UseTimeMultiplier(Item item, Player player)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            float num = base.UseTimeMultiplier(item, player);

            if (modPlayer.forDemacia)
                num += 0.1f;
            if (modPlayer.windPower)
                num += 0.1f;
            if (modPlayer.chargerBlessing)
            {
                num += 0.1f;
                if (modPlayer.bottleOfStardust)
                    num += 0.1f;
            }

            if (item.ranged)
            {
                num *= 0.8f * (float)modPlayer.rangedAttackSpeed;
            }

            return num;
        }

        public override void UseStyle(Item item, Player player)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            if (modPlayer.cannonTimer <= 0 && modPlayer.cannonSet && Main.rand.Next(0, 1) == 0 && item.ranged && player.whoAmI == Main.LocalPlayer.whoAmI)
            {
                modPlayer.cannonTimer = 90;
                Projectile.NewProjectileDirect(player.Center, new Vector2(-15, 0).RotatedBy(player.AngleFrom(Main.MouseWorld)), ProjectileID.CannonballFriendly, (int)(modPlayer.RNG * 1.5), 7, player.whoAmI);
            }

            base.UseStyle(item, player);
        }

        

        public override bool CanUseItem(Item item, Player player)
        {
            if (player.GetModPlayer<PLAYERGLOBAL>().umbralTrespassing)
                return false;
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile proj = Main.projectile[i];
                if (proj.owner == player.whoAmI)
                {
                    if (proj.active && proj.GetGlobalProjectile<Projectiles.PROJECTILEGLOBAL>().channelProjectile)
                    {
                        return false;
                    }
                }
            }

            return base.CanUseItem(item, player);
        }

        public override bool UseItem(Item item, Player player)
        {
            return base.UseItem(item, player);
        }

        public override bool ConsumeAmmo(Item item, Player player)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            return Main.rand.NextFloat() - 0.01f >= modPlayer.ConsumeAmmoChance;
        }

        public override void OnHitNPC(Item item, Player player, NPC target, int damage, float knockBack, bool crit)
        {
            base.OnHitNPC(item, player, target, damage, knockBack, crit);
        }

        public override void UpdateEquip(Item item, Player player)
        {
            // Pirate set
            if (item.type == ItemID.PirateHat)
            {
                player.rangedDamage += 0.05f;
            }
            if (item.type == ItemID.PirateShirt)
            {
                player.rangedCrit += 3;
            }
            if (item.type == ItemID.PiratePants)
            {
                player.GetModPlayer<PLAYERGLOBAL>().ConsumeAmmoChance += 0.1;
            }

            // Cannoneer set
            if (item.type == ItemID.BuccaneerBandana)
            {
                player.rangedDamage += 0.15f;
            }
            if (item.type == ItemID.BuccaneerShirt)
            {
                player.rangedCrit += 7;
            }
            if (item.type == ItemID.BuccaneerPants)
            {
                player.GetModPlayer<PLAYERGLOBAL>().ConsumeAmmoChance += 0.15;
            }
            base.UpdateEquip(item, player);
        }

        public override string IsArmorSet(Item head, Item body, Item legs)
        {
            if (head.type == ItemID.PirateHat && body.type == ItemID.PirateShirt && legs.type == ItemID.PiratePants)
            {
                return "Pirate";
            }
            if (head.type == ItemID.BuccaneerBandana && body.type == ItemID.BuccaneerShirt && legs.type == ItemID.BuccaneerPants)
            {
                return "Cannoneer";
            }
            if (legs.type == ItemType<SpiritualLeggings>() && head.type == ItemType<SpiritualHelmet>() && body.type == ItemType<SpiritualBreastplate>())
            {
                return "Spiritual";
            }
            if (legs.type == ItemType<PetriciteLeggings>() && head.type == ItemType<PetriciteHelmet>() && body.type == ItemType<PetriciteBreastplate>())
            {
                return "Petricite";
            }
            else
            {
                return base.IsArmorSet(head, body, legs);
            }
        }

        public override void UpdateArmorSet(Player player, string set)
        {
            if (set == "Pirate")
            {
                player.GetModPlayer<PLAYERGLOBAL>().pirateSet = true;
                player.GetModPlayer<PLAYERGLOBAL>().rangedOnHit += 5;
                player.setBonus = "+5 ranged On Hit damage" +
                    "\nRanged damage cause enemies to drop coins";
            }
            if (set == "Cannoneer")
            {
                player.GetModPlayer<PLAYERGLOBAL>().cannonSet = true;
                player.setBonus = "Ranged weapons periodically fire a cannon ball dealing " + TerraLeague.CreateScalingTooltip(DamageType.RNG, player.GetModPlayer<PLAYERGLOBAL>().RNG, 150) + " ranged damage";
            }
            if (set == "Spiritual")
            {
                if (player.GetModPlayer<PLAYERGLOBAL>().GetRealHeathWithoutShield() <= player.GetModPlayer<PLAYERGLOBAL>().maxLifeLastStep / 2)
                    player.AddBuff(BuffType<GatheringFire>(), 2);
            }
            if (set == "Petricite")
            {
                if (!player.HasBuff(BuffType<PetricitePlating>()))
                    player.AddBuff(BuffType<PetricitePlating>(), 60 * 6);
            }
            base.UpdateArmorSet(player, set);
        }

        public override bool Shoot(Item item, Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            //if (player.GetModPlayer<PLAYERGLOBAL>().meleeProjCooldown)
            //{
            //    return false;
            //}
            //else if (meleeProjCooldown && !player.GetModPlayer<PLAYERGLOBAL>().meleeProjCooldown)
            //{
            //    player.GetModPlayer<PLAYERGLOBAL>().meleeProjCooldown = true;
            //}

            return base.Shoot(item, player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
        }

        public override bool PreOpenVanillaBag(string context, Player player, int arg)
        {
            if (context == "crate" && arg == ItemID.WoodenCrate && Main.rand.Next(8) == 0)
                player.QuickSpawnItem(ItemType<BrassBar>(), Main.rand.Next(8, 13));

            if (context == "crate" && arg == ItemID.IronCrate && Main.rand.Next(4) == 0)
                player.QuickSpawnItem(ItemType<BrassBar>(), Main.rand.Next(12, 19));

            if (context == "crate" && arg == ItemID.GoldenCrate &&  Main.rand.Next(3) == 0)
                player.QuickSpawnItem(ItemType<BrassBar>(), Main.rand.Next(18, 25));

            return base.PreOpenVanillaBag(context, player, arg);
        }

        public override void ExtractinatorUse(int extractType, ref int resultType, ref int resultStack)
        {
            if (extractType == 3347)
            {
                if (Main.rand.NextFloat() < 0.035259f)
                {
                    resultStack = 1;
                    resultType = ItemType<Sunstone>();
                }
            }
        }
    }
}
