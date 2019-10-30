using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Buffs
{
    public class PetricitePlating : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Petricite Plating");
            Description.SetDefault("Your petricite armor is absorbing magic!");
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;

        }
        public override void Update(Player player, ref int buffIndex)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            Main.buffNoTimeDisplay[Type] = false;

            if (player.armor[0].type == ItemType<Items.Armor.PetriciteHelmet>() && player.armor[1].type == ItemType<Items.Armor.PetriciteBreastplate>() && player.armor[2].type == ItemType<Items.Armor.PetriciteLeggings>())
            {
                if (Main.LocalPlayer.whoAmI == player.whoAmI)
                {
                    for (int i = 0; i < modPlayer.Shields.Count; i++)
                    {
                        if (modPlayer.Shields[i].AssociatedBuff == Type)
                        {
                            if (modPlayer.Shields[i].ShieldAmount < 50)
                            {
                                Main.buffNoTimeDisplay[Type] = false;

                                if (player.buffTime[buffIndex] <= 60)
                                {
                                    modPlayer.AddShieldAttachedToBuff(10, Type, Color.MediumPurple, ShieldType.Magic);
                                    player.buffTime[buffIndex] = 60 * 6;
                                }
                            }
                            else
                            {
                                Main.buffNoTimeDisplay[Type] = true;
                                player.buffTime[buffIndex] = 60 * 6;
                            }

                            break;
                        }

                        if (i == modPlayer.Shields.Count - 1)
                        {
                            if (player.buffTime[buffIndex] <= 60)
                            {
                                modPlayer.AddShieldAttachedToBuff(10, Type, Color.MediumPurple, ShieldType.Magic);
                                player.buffTime[buffIndex] = 60 * 6;
                            }
                        }
                    }

                    if (modPlayer.Shields.Count == 0)
                    {
                        if (player.buffTime[buffIndex] <= 60)
                        {
                            modPlayer.AddShieldAttachedToBuff(10, Type, Color.MediumPurple, ShieldType.Magic);
                            player.buffTime[buffIndex] = 60 * 6;
                        }
                    }
                }
            }
            else
            {
                player.ClearBuff(Type);
            }
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
        }
    }
}
