using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace TerraLeague.Items
{
    public class TerraLeaguePrefixGLOBAL : GlobalItem
    {
        public bool Transedent;
        public byte Armor;
        public byte Resist;
        public byte HealPower;
        public byte MEL;
        public byte RNG;
        public byte MAG;
        public byte SUM;

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

        public TerraLeaguePrefixGLOBAL()
        {
            Transedent = false;
            Armor = 0;
            Resist = 0;
            HealPower = 0;
            MEL = 0;
            RNG = 0;
            MAG = 0;
            SUM = 0;
        }

        public override bool NewPreReforge(Item item)
        {
            Transedent = false;
            Armor = 0;
            Resist = 0;
            HealPower = 0;
            MEL = 0;
            RNG = 0;
            MAG = 0;
            SUM = 0;

            return base.NewPreReforge(item);
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (!item.social && item.prefix > 0)
            {
                if (Transedent)
                {
                    TooltipLine line = new TooltipLine(mod, "PrefixCDR", "+5 ability, item, and summoner spell haste")
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
                if (MEL > 0)
                {
                    TooltipLine line = new TooltipLine(mod, "PrefixMEL", "+" + (10 * MEL) + " MEL")
                    {
                        isModifier = true
                    };
                    tooltips.Add(line);
                }
                if (RNG > 0)
                {
                    TooltipLine line = new TooltipLine(mod, "PrefixRNG", "+" + (10 * RNG) + " RNG")
                    {
                        isModifier = true
                    };
                    tooltips.Add(line);
                }
                if (MAG > 0)
                {
                    TooltipLine line = new TooltipLine(mod, "PrefixMAG", "+" + (10 * MAG) + " MAG")
                    {
                        isModifier = true
                    };
                    tooltips.Add(line);
                }
                if (SUM > 0)
                {
                    TooltipLine line = new TooltipLine(mod, "PrefixSUM", "+" + (10 * SUM) + " SUM")
                    {
                        isModifier = true
                    };
                    tooltips.Add(line);
                }
            }

            base.ModifyTooltips(item, tooltips);
        }

        public override void UpdateEquip(Item item, Player player)
        {
            // Prefixes
            if (Transedent)
            {
                player.GetModPlayer<PLAYERGLOBAL>().abilityHaste += 5;
                player.GetModPlayer<PLAYERGLOBAL>().itemHaste += 5;
                player.GetModPlayer<PLAYERGLOBAL>().summonerHaste += 5;
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
            else if (MEL > 0)
            {
                player.GetModPlayer<PLAYERGLOBAL>().BonusMEL += 10 * MEL;
            }
            else if (RNG > 0)
            {
                player.GetModPlayer<PLAYERGLOBAL>().BonusRNG += 10 * RNG;
            }
            else if (MAG > 0)
            {
                player.GetModPlayer<PLAYERGLOBAL>().BonusMAG += 10 * MAG;
            }
            else if (SUM > 0)
            {
                player.GetModPlayer<PLAYERGLOBAL>().BonusSUM += 10 * SUM;
            }

            base.UpdateEquip(item, player);
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
            else if (MEL > 0)
            {
                if (MEL == 1)
                    reforgePrice = (int)(reforgePrice * 1.3225);
                else if (MEL == 2)
                    reforgePrice = (int)(reforgePrice * 1.44);
            }
            else if (RNG > 0)
            {
                if (RNG == 1)
                    reforgePrice = (int)(reforgePrice * 1.3225);
                else if (RNG == 2)
                    reforgePrice = (int)(reforgePrice * 1.44);
            }
            else if (MAG > 0)
            {
                if (MAG == 1)
                    reforgePrice = (int)(reforgePrice * 1.3225);
                else if (MAG == 2)
                    reforgePrice = (int)(reforgePrice * 1.44);
            }
            else if (SUM > 0)
            {
                if (SUM == 1)
                    reforgePrice = (int)(reforgePrice * 1.3225);
                else if (SUM == 2)
                    reforgePrice = (int)(reforgePrice * 1.44);
            }

            return base.ReforgePrice(item, ref reforgePrice, ref canApplyDiscount);
        }

        public override GlobalItem Clone(Item item, Item itemClone)
        {
            TerraLeaguePrefixGLOBAL myClone = (TerraLeaguePrefixGLOBAL)base.Clone(item, itemClone);
            myClone.Transedent = Transedent;
            myClone.Armor = Armor;
            myClone.Resist = Resist;
            myClone.HealPower = HealPower;
            myClone.MEL = MEL;
            myClone.RNG = RNG;
            myClone.MAG = MAG;
            myClone.SUM = SUM;
            return myClone;
        }

        public override void NetSend(Item item, BinaryWriter writer)
        {
            writer.Write(Transedent);
            writer.Write(Armor);
            writer.Write(Resist);
            writer.Write(HealPower);
            writer.Write(MEL);
            writer.Write(RNG);
            writer.Write(MAG);
            writer.Write(SUM);
        }

        public override void NetReceive(Item item, BinaryReader reader)
        {
            Transedent = reader.ReadBoolean();
            Armor = reader.ReadByte();
            Resist = reader.ReadByte();
            HealPower = reader.ReadByte();
            MEL = reader.ReadByte();
            RNG = reader.ReadByte();
            MAG = reader.ReadByte();
            SUM = reader.ReadByte();
        }
    }
}
