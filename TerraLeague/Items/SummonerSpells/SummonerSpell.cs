using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Items.SummonerSpells
{
    abstract public class SummonerSpell : ModItem
    {
        static internal SummonerSpellsPacketHandler PacketHandler = new SummonerSpellsPacketHandler(6);

        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.FallenStar);
            item.rare = ItemRarityID.Orange;
            item.width = 20;
            item.height = 26;
            item.maxStack = 1;
            item.notAmmo = true;
            item.ammo = AmmoID.None;
            item.shoot = ItemID.None;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            TooltipLine tt = tooltips.FirstOrDefault(x => x.Name == "Tooltip0" && x.mod == "Terraria");
            if (tt != null)
            {
                int pos = tooltips.IndexOf(tt);

                string text = TerraLeague.CreateColorString(TerraLeague.PassiveSubColor, "Left or Right click to replace your Left or Right Summoner Spell") +
                    "\nEffect: "+ GetTooltip() +
                    "\n" + GetCooldown() + " second cooldown";
                TooltipLine tip = new TooltipLine(TerraLeague.instance, "Tooltip0", text);
                tooltips[pos] = tip;
            }

            base.ModifyTooltips(tooltips);
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.whoAmI == Main.LocalPlayer.whoAmI)
            {
                PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

                if (modPlayer.sumSpells.Where(x => x.GetType() == this.GetType()).Count() > 0)
                    return false;
                else
                {
                    if (player.altFunctionUse == 2)
                    {
                        modPlayer.sumSpells[1] = this;
                    }
                    else
                    {
                        modPlayer.sumSpells[0] = this;
                    }
                    item.stack = 0;
                    return base.CanUseItem(player);
                }
            }
            return false;
        }

        /// <summary>
        /// Gets the tooltip
        /// </summary>
        /// <returns></returns>
        virtual public string GetTooltip()
        {
            return "This Summoner Spell has not tooltip";
        }

        /// <summary>
        /// Gets the Cooldown of the spell adjusted with cooldown reduction
        /// </summary>
        /// <returns></returns>
        virtual public float GetCooldown()
        {
            return (float)Math.Round(GetRawCooldown() * (Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().SummonerCdrLastStep), 1);
        }

        /// <summary>
        /// Performs the summoner spells action
        /// </summary>
        /// <param name="player">Player casting the spell</param>
        /// <param name="spellSlot">Which slot the spell is in</param>
        abstract public void DoEffect(Player player, int spellSlot);

        /// <summary>
        /// Sets the cooldown of the specified slot
        /// </summary>
        /// <param name="player"></param>
        /// <param name="spellSlot"></param>
        protected void SetCooldowns(Player player, int spellSlot)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            modPlayer.sumCooldowns[spellSlot - 1] = (int)(GetCooldown() * 60);
        }

        /// <summary>
        /// The path for the spells icon
        /// </summary>
        abstract public string GetIconTexturePath();

        /// <summary>
        /// Name of the summoner spell
        /// </summary>
        abstract public string GetSpellName();

        /// <summary>
        /// The spells cooldown in seconds
        /// </summary>
        abstract public int GetRawCooldown();
    }
}
