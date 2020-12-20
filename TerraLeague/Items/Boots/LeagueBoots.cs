using System.Collections.Generic;
using System.Linq;
using TerraLeague.Items.BasicItems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Boots
{
    public abstract class LeagueBoot : ModItem
    {
        public override void SetStaticDefaults()
        {
        }

        public string BuildFullTooltip(bool withUpgradeText = true)
        {
            if (Main.hardMode)
            {
                return TerraLeague.CreateColorString(TerraLeague.ConvertHexToColor("F892F8"), "Tier 5: Fastest Sprint + Rocket Boots + Ice Skates") + "\n8% increased movement speed\n" + Tier5Tip();
            }
            else if (NPC.downedBoss3)
            {
                return TerraLeague.CreateColorString(TerraLeague.ConvertHexToColor("F49090"), "Tier 4: Faster Sprint + Rocket Boots") + "\n8% increased movement speed\n" + Tier4Tip() + (withUpgradeText ? "\nDefeat The Wall of Flesh to upgrade" : "");
            }
            else if (NPC.downedBoss2)
            {
                return TerraLeague.CreateColorString(TerraLeague.ConvertHexToColor("E8B688"), "Tier 3: Fast Sprint + Rocket Boots") + "\n" + Tier3Tip() + (withUpgradeText ? "\nDefeat Skeletron to upgrade" : "");
            }
            else if (NPC.downedBoss1)
            {
                return TerraLeague.CreateColorString(TerraLeague.ConvertHexToColor("92F892"), "Tier 2: Fast Sprint") + "\n" + Tier2Tip() + (withUpgradeText ? (Main.ActiveWorldFileData.HasCorruption ? "\nDefeat The Eater of Worlds to upgrade" : "\nDefeat The Brain of Cthulhu to upgrade") : "");
            }
            else
            {
                return TerraLeague.CreateColorString(TerraLeague.ConvertHexToColor("8686E5"), "Tier 1: Slow Sprint") + "\n" + Tier1Tip() + (withUpgradeText ? "\nDefeat The Eye of Cthulhu to upgrade" : "");
            }
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            TooltipLine tt = tooltips.FirstOrDefault(x => x.Name == "Tooltip0" && x.mod == "Terraria");
            Player player = Main.LocalPlayer;
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            if (tt != null)
            {
                tt.text = BuildFullTooltip();
                if (Main.hardMode)
                {
                    //item.value = 350000;
                    item.rare = ItemRarityID.Pink;
                }
                else if (NPC.downedBoss3)
                {
                    //item.value = 240000;
                    item.rare = ItemRarityID.LightRed;
                }
                else if (NPC.downedBoss2)
                {
                    //item.value = 100000;
                    item.rare = ItemRarityID.Orange;
                }
                else if (NPC.downedBoss1)
                {
                    //item.value = 50000;
                    item.rare = ItemRarityID.Green;
                }
                else
                {
                    //item.value = 25000;
                    item.rare = ItemRarityID.Blue;
                }
            }
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.accessory = true;
            item.value = 100000;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            Update(player);
        }

        public override void UpdateInventory(Player player)
        {
            Update(player);
            base.UpdateInventory(player);
        }

        public void Update(Player player)
        {
            //item.value = 100000;
            if (Main.hardMode)
            {
                player.GetModPlayer<PLAYERGLOBAL>().T5Boots = true;
                //item.value = 350000;
                item.rare = ItemRarityID.Pink;
                Tier5Update(player);
                player.rocketBoots = 3;
                player.moveSpeed += 0.08f;
                player.iceSkate = true;
            }
            else if (NPC.downedBoss3)
            {
                player.GetModPlayer<PLAYERGLOBAL>().T4Boots = true;
                //item.value = 240000;
                item.rare = ItemRarityID.LightRed;
                Tier4Update(player);
                player.rocketBoots = 2;
                player.moveSpeed += 0.08f;
            }
            else if (NPC.downedBoss2)
            {
                player.GetModPlayer<PLAYERGLOBAL>().T3Boots = true;
                //item.value = 100000;
                item.rare = ItemRarityID.Orange;
                Tier3Update(player);
                player.rocketBoots = 2;
            }
            else if (NPC.downedBoss1)
            {
                player.GetModPlayer<PLAYERGLOBAL>().T2Boots = true;
                //item.value = 50000;
                item.rare = ItemRarityID.Green;
                Tier2Update(player);
            }
            else
            {
                player.GetModPlayer<PLAYERGLOBAL>().T1Boots = true;
                //item.value = 25000;
                item.rare = ItemRarityID.Blue;
                Tier1Update(player);
            }
        }

        virtual public void Tier1Update(Player player)
        {
        }

        virtual public void Tier2Update(Player player)
        {
        }

        virtual public void Tier3Update(Player player)
        {
        }

        virtual public void Tier4Update(Player player)
        {
        }

        virtual public void Tier5Update(Player player)
        {
        }

        virtual public string Tier1Tip()
        {
            return "";
        }

        virtual public string Tier2Tip()
        {
            return "";
        }

        virtual public string Tier3Tip()
        {
            return "";
        }

        virtual public string Tier4Tip()
        {
            return "";
        }

        virtual public string Tier5Tip()
        {
            return "";
        }
    }
}
