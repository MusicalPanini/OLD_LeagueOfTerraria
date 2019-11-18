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
    public class ITEMGLOBAL : GlobalItem
    {
        public double HealingPowerLastStep = 0;
        public bool Transedent;
        public byte Armor;
        public byte Resist;
        public byte HealPower;

        public ITEMGLOBAL()
        {
            Transedent = false;
            Armor = 0;
            Resist = 0;
        }

        public override bool NewPreReforge(Item item)
        {
            Transedent = false;
            Armor = 0;
            Resist = 0;
            HealPower = 0;

            return base.NewPreReforge(item);
        }

        public override bool InstancePerEntity
        {
            get
            {
                return true;
            }
        }

        public override bool CloneNewInstances
        {
            get
            {
                return true;
            }
        }

        public override void SetDefaults(Item item)
        {
            if (item.type == ItemID.StoneBlock || item.type == ItemID.EbonstoneBlock || item.type == ItemID.CrimstoneBlock || item.type == ItemID.PearlstoneBlock)
            {
                // Need an itemid from the mod to not conflict with other mods
                item.ammo = ItemType<BlackIceChunk>();
            }

            if (item.type == ItemID.PirateHat)
            {
                item.vanity = false;
                item.defense = 4;
            }
            if (item.type == ItemID.PirateShirt)
            {
                item.vanity = false;
                item.defense = 5;
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
                item.defense = 5;
                item.rare = 3;
            }
            if (item.type == ItemID.BuccaneerShirt)
            {
                item.SetNameOverride("Cannoneer Shirt");
                item.vanity = false;
                item.defense = 5;
                item.rare = 3;
            }
            if (item.type == ItemID.BuccaneerPants)
            {
                item.SetNameOverride("Cannoneer Pants");
                item.vanity = false;
                item.defense = 5;
                item.rare = 3;
            }
            base.SetDefaults(item);
        }

        public override bool OnPickup(Item item, Player player)
        {
            if (item.type == 184 || item.type == 1735 || item.type == 1868)
            {
                PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
                if (modPlayer.manaCharge && modPlayer.manaChargeStacks < 750)
                    modPlayer.manaChargeStacks++;
            }

            return base.OnPickup(item, player);
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

            if (!item.social && item.prefix > 0)
            {
                if (Transedent)
                {
                    TooltipLine line = new TooltipLine(mod, "PrefixCDR", "+5% cooldown reduction")
                    {
                        isModifier = true
                    };
                    tooltips.Add(line);
                }
                if (Armor > 0)
                {
                    TooltipLine line = new TooltipLine(mod, "PrefixArmor", "+" + (2 * Armor) + " armor")
                    {
                        isModifier = true
                    };
                    tooltips.Add(line);
                }
                if (Resist > 0)
                {
                    TooltipLine line = new TooltipLine(mod, "PrefixResist", "+" + (2 * Resist) + " resist")
                    {
                        isModifier = true
                    };
                    tooltips.Add(line);
                }
                if (HealPower > 0)
                {
                    TooltipLine line = new TooltipLine(mod, "PrefixHealPower", "+" + (3 * HealPower) + "% healing power")
                    {
                        isModifier = true
                    };
                    tooltips.Add(line);
                }
            }
            base.ModifyTooltips(item, tooltips);
        }

        public override float UseTimeMultiplier(Item item, Player player)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            float num = 1;

            if (modPlayer.forDemacia)
                num *= 1.1f;
            if (modPlayer.deadlyPlumage && item.ranged)
                num *= 1.25f;
            if (modPlayer.windPower && item.ranged)
                num *= 1.1f;

            return num;
        }

        public override void UseStyle(Item item, Player player)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            if (modPlayer.cannonTimer <= 0 && modPlayer.cannonSet && Main.rand.Next(0, 1) == 0 && item.ranged)
            {
                modPlayer.cannonTimer = 90;
                Projectile.NewProjectileDirect(player.Center, new Vector2(-15, 0).RotatedBy(player.AngleFrom(Main.MouseWorld)), ProjectileID.CannonballFriendly, 50, 7, player.whoAmI);
            }

            base.UseStyle(item, player);
        }

        public override void UpdateInventory(Item item, Player player)
        {

            if (item.healLife > 0)
            {
                int stack = item.stack;
                bool fav = item.favorited;

                item.SetDefaults(item.type);

                item.stack = stack;
                item.favorited = fav;

                if (player.GetModPlayer<PLAYERGLOBAL>().hasSpiritualRestorationLastStep)
                    item.healLife = (int)(item.healLife * 1.3);

                item.healLife = (int)(item.healLife * player.GetModPlayer<PLAYERGLOBAL>().healPowerLastStep);
            }

            base.UpdateInventory(item, player);
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
            // Prefixes
            if (Transedent)
            {
                player.GetModPlayer<PLAYERGLOBAL>().Cdr -= 0.05;
            }
            else if (Armor > 0)
            {
                player.GetModPlayer<PLAYERGLOBAL>().armor += 2 * Armor;
            }
            else if (Resist > 0)
            {
                player.GetModPlayer<PLAYERGLOBAL>().resist += 2 * Resist;
            }
            else if (HealPower > 0)
            {
                player.GetModPlayer<PLAYERGLOBAL>().healPower += 0.03 * HealPower;
            }

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

        public override bool ReforgePrice(Item item, ref int reforgePrice, ref bool canApplyDiscount)
        {
            // Prefixes
            if (Transedent)
            {
                reforgePrice = (int)(reforgePrice * 1.21);
            }
            else if (Armor > 0)
            {
                if (Armor == 1)
                    reforgePrice = (int)(reforgePrice * 1.21);
                else if (Armor == 2)
                    reforgePrice = (int)(reforgePrice * 1.3225);
                else if (Armor == 3)
                    reforgePrice = (int)(reforgePrice * 1.44);
            }
            else if (Resist > 0)
            {
                if (Resist == 1)
                    reforgePrice = (int)(reforgePrice * 1.21);
                else if (Resist == 2)
                    reforgePrice = (int)(reforgePrice * 1.3225);
                else if (Resist == 3)
                    reforgePrice = (int)(reforgePrice * 1.44);
            }
            else if (HealPower > 0)
            {
                if (HealPower == 1)
                    reforgePrice = (int)(reforgePrice * 1.21);
                else if (HealPower == 2)
                    reforgePrice = (int)(reforgePrice * 1.3225);
                else if (HealPower == 3)
                    reforgePrice = (int)(reforgePrice * 1.44);
            }

            return base.ReforgePrice(item, ref reforgePrice, ref canApplyDiscount);
        }

        public override void UpdateArmorSet(Player player, string set)
        {
            if (set == "Pirate")
            {
                player.GetModPlayer<PLAYERGLOBAL>().pirateSet = true;
                player.setBonus = "Ranged damage cause enemies to drop coins";
            }
            if (set == "Cannoneer")
            {
                player.GetModPlayer<PLAYERGLOBAL>().cannonSet = true;
                player.setBonus = "Ranged weapons periodically fire a cannon ball";
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
            return base.Shoot(item, player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
        }

        public override bool PreOpenVanillaBag(string context, Player player, int arg)
        {
            if (context == "crate" && arg == ItemID.WoodenCrate && Main.rand.Next(Main.hardMode ? 14 : 8) == 0)
                player.QuickSpawnItem(ItemType<BrassBar>(), Main.rand.Next(8, 13));

            if (context == "crate" && arg == ItemID.IronCrate && Main.rand.Next(Main.hardMode ? 12 : 4) == 0)
                player.QuickSpawnItem(ItemType<BrassBar>(), Main.rand.Next(12, 19));

            if (context == "crate" && arg == ItemID.GoldenCrate &&  Main.rand.Next(Main.hardMode ? 9 : 3) == 0)
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

        public override GlobalItem Clone(Item item, Item itemClone)
        {
            ITEMGLOBAL myClone = (ITEMGLOBAL)base.Clone(item, itemClone);
            myClone.Transedent = Transedent;
            return myClone;
        }

        public override void NetSend(Item item, BinaryWriter writer)
        {
            writer.Write(Transedent);
            writer.Write(Armor);
            writer.Write(Resist);
            writer.Write(HealPower);
        }

        public override void NetReceive(Item item, BinaryReader reader)
        {
            Transedent = reader.ReadBoolean();
            Armor = reader.ReadByte();
            Resist = reader.ReadByte();
            HealPower = reader.ReadByte();
        }
    }
}
