using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace TerraLeague.Items.CustomItems
{
    abstract public class LeagueItem : ModItem
    {
        //public int armor = 0;
        //public int resist = 0;
        //public double meleeAtkSpd = 0;
        //public double ammoConsumeChance = 0;
        //public double cdr = 0;
        //public double cdr = 0;


        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (player.GetModPlayer<PLAYERGLOBAL>().PassivesAreActive[TerraLeague.FindAccessorySlotOnPlayer(player, this) * 2])
                if (GetPrimaryPassive() != null)
                    GetPrimaryPassive().UpdateAccessory(player, this);

            if (player.GetModPlayer<PLAYERGLOBAL>().PassivesAreActive[(TerraLeague.FindAccessorySlotOnPlayer(player, this) * 2) + 1])
                if (GetSecondaryPassive() != null)
                    GetSecondaryPassive().UpdateAccessory(player, this);

            base.UpdateAccessory(player, hideVisual);
        }

        public override bool CanEquipAccessory(Player player, int slot)
        {
            if (slot >= 3 && slot <= 8)
                player.GetModPlayer<PLAYERGLOBAL>().accessoryStat[slot-3] = 0;

            return base.CanEquipAccessory(player, slot);
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            TooltipLine tt = tooltips.FirstOrDefault(x => x.Name == "Tooltip0" && x.mod == "Terraria");
            if (tt != null)
            {
                int pos = tooltips.Count;

                string text = "\n";

                if (GetActive() != null)
                    text += "\n" + GetActive().Tooltip(Main.LocalPlayer, this);
                if (GetPrimaryPassive() != null)
                    text += "\n" + GetPrimaryPassive().Tooltip(Main.LocalPlayer, this);
                if (GetSecondaryPassive() != null)
                    text += "\n" + GetSecondaryPassive().Tooltip(Main.LocalPlayer, this);

                string[] lines = text.Split('\n');

                int pos2 = -1;
                for (int i = 0; i < tooltips.Count; i++)
                {
                    if (tooltips.FirstOrDefault(x => x.Name == "Tooltip" + i && x.mod == "Terraria") == null)
                    {
                        pos2 = tooltips.FindIndex(x => x.Name == "Tooltip" + (i - 1) && x.mod == "Terraria");
                        break;
                    }

                }

                if (pos2 != -1)
                {
                    for (int i = 1; i < lines.Count(); i++)
                    {
                        tooltips.Insert(pos2 + i, new TooltipLine(TerraLeague.instance, "Tooltipzz" + i, lines[i]));
                    }
                }
                else
                {
                    for (int i = 1; i < lines.Count(); i++)
                    {
                        tooltips.Add(new TooltipLine(TerraLeague.instance, "Tooltipzz" + (i - 1), lines[i]));
                    }
                    tooltips.RemoveAt(pos);
                }
            }
        }
        virtual public double GetStatOnPlayer(Player player)
        {
            int slot = TerraLeague.FindAccessorySlotOnPlayer(player, this);

            if (slot != -1)
                return player.GetModPlayer<PLAYERGLOBAL>().accessoryStat[slot];
            else
                return 0;
        }

        virtual public void SetStatOnPlayer(Player player, double stat)
        {
            int slot = TerraLeague.FindAccessorySlotOnPlayer(player, this);

            if (slot != -1)
                player.GetModPlayer<PLAYERGLOBAL>().accessoryStat[slot] = stat;
        }

        virtual public string GetStatText()
        {
            int slot = TerraLeague.FindAccessorySlotOnPlayer(Main.LocalPlayer, this);

            if (slot != -1)
                return "";
            else
                return "";
        }

        virtual public void PostPlayerUpdate(Player player)
        {

        }

        virtual public void NPCHit(Item item, NPC target, ref int damage, ref float knockback, ref bool crit, ref int OnHitDamage, Player player)
        {
            if (GetPrimaryPassive() != null)
                GetPrimaryPassive().NPCHit(item, target, ref damage, ref knockback, ref crit, ref OnHitDamage, player, this);
            if (GetSecondaryPassive() != null)
                GetSecondaryPassive().NPCHit(item, target, ref damage, ref knockback, ref crit, ref OnHitDamage, player, this);
            if (GetActive() != null)
                GetActive().NPCHit(item, target, ref damage, ref knockback, ref crit, ref OnHitDamage, player, this);
        }

        virtual public void NPCHitWithProjectile(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection, ref int OnHitDamage, Player player)
        {
            if (GetPrimaryPassive() != null)
                GetPrimaryPassive().NPCHitWithProjectile(proj, target, ref damage, ref knockback, ref crit, ref hitDirection, ref OnHitDamage, player, this);
            if (GetSecondaryPassive() != null)
                GetSecondaryPassive().NPCHitWithProjectile(proj, target, ref damage, ref knockback, ref crit, ref hitDirection, ref OnHitDamage, player, this);
            if (GetActive() != null)
                GetActive().NPCHitWithProjectile(proj, target, ref damage, ref knockback, ref crit, ref hitDirection, ref OnHitDamage, player, this);
        }

        virtual public void OnHitByNPC(NPC npc, ref int damage, ref bool crit, Player player)
        {
            if (GetPrimaryPassive() != null)
                GetPrimaryPassive().OnHitByNPC(npc, ref damage, ref crit, player, this);
            if (GetSecondaryPassive() != null)
                GetSecondaryPassive().OnHitByNPC(npc, ref damage, ref crit, player, this);
            if (GetActive() != null)
                GetActive().OnHitByNPC(npc, ref damage, ref crit, player, this);
        }

        virtual public void OnHitByProjectile(Projectile proj, ref int damage, ref bool crit, Player player)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            if (GetPrimaryPassive() != null)
                GetPrimaryPassive().OnHitByProjectile(proj, ref damage, ref crit, player, this);
            if (GetSecondaryPassive() != null)
                GetSecondaryPassive().OnHitByProjectile(proj, ref damage, ref crit, player, this);
            if (GetActive() != null)
                GetActive().OnHitByProjectile(proj, ref damage, ref crit, player, this);
        }

        virtual public void OnHitByProjectile(NPC npc, ref int damage, ref bool crit, Player player)
        {
            if (GetPrimaryPassive() != null)
                GetPrimaryPassive().OnHitByProjectile(npc, ref damage, ref crit, player, this);
            if (GetSecondaryPassive() != null)
                GetSecondaryPassive().OnHitByProjectile(npc, ref damage, ref crit, player, this);
            if (GetActive() != null)
                GetActive().OnHitByProjectile(npc, ref damage, ref crit, player, this);
        }

        virtual public void OnKilledNPC(NPC npc, int damage, bool crit, Player player)
        {
            if (GetPrimaryPassive() != null)
                GetPrimaryPassive().OnKilledNPC(npc, damage, crit, player, this);
            if (GetSecondaryPassive() != null)
                GetSecondaryPassive().OnKilledNPC(npc, damage, crit, player, this);
            //if (GetActive() != null)
            //    GetActive().NPCHitWithProjectile(proj, target, ref damage, ref knockback, ref crit, ref hitDirection, ref OnHitDamage, player, this);
        }

        virtual public Passive GetPrimaryPassive()
        {
            return null;
        }

        virtual public Passive GetSecondaryPassive()
        {
            return null;
        }

        virtual public Active GetActive()
        {
            return null;
        }

        virtual public string GetBonusTooltips(Player player)
        {
            string text = "";
            return text;
        }

        virtual public bool OnCooldown(Player player)
        {
            return false;
        }
    }
}
