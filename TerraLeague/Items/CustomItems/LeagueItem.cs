using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace TerraLeague.Items.CustomItems
{
    abstract public class LeagueItem : ModItem
    {
        public Passive[] Passives = null;
        public Active Active = null;

        // public int armor = 0;
        // public int resist = 0;
        // public double meleeAtkSpd = 0;
        // public double ammoConsumeChance = 0;
        // public double cdr = 0;
        // public double cdr = 0;


        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (Passives != null)
            {
                for (int i = 0; i < Passives.Length; i++)
                {
                    if (Passives[i].currentlyActive)
                    {
                        Passives[i].UpdateAccessory(player, this);
                    }
                }
            }
            //if (Active != null)
            //{

            //}
            base.UpdateAccessory(player, hideVisual);
        }

        public override bool CanEquipAccessory(Player player, int slot)
        {
            if (slot >= 3 && slot <= 8)
            {
                if (Passives != null)
                {
                    for (int i = 0; i < Passives.Length; i++)
                    {
                        Passives[i].passiveStat = 0;
                        Passives[i].SetCooldown(player);
                    }
                }
                if (Active != null)
                {
                    Active.activeStat = 0;
                    Active.SetCooldown(player);
                }
            }

            return base.CanEquipAccessory(player, slot);
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            TooltipLine tt = tooltips.FirstOrDefault(x => x.Name == "Tooltip0" && x.mod == "Terraria");
            if (tt != null)
            {
                int pos = tooltips.Count;
                this.SetDefaults();
                string text = "\n";

                int slot = TerraLeague.FindAccessorySlotOnPlayer(Main.LocalPlayer, this);
                if (slot != -1)
                {
                    if (Active != null)
                    {
                        if (Active.currentlyActive)
                        {
                            text += "\n" + Active.Tooltip(Main.LocalPlayer, this);
                        }
                    }
                    if (Passives != null)
                    {
                        for (int i = 0; i < Passives.Length; i++)
                        {
                            if (Passives[i].currentlyActive)
                            {
                                text += "\n" + Passives[i].Tooltip(Main.LocalPlayer, this);
                            }
                        }
                    }
                    //if (GetActive() != null && Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().ActivesAreActive[slot])
                    //    text += "\n" + GetActive().Tooltip(Main.LocalPlayer, this);
                    //if (GetPrimaryPassive() != null && Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().PassivesAreActive[slot * 2])
                    //    text += "\n" + GetPrimaryPassive().Tooltip(Main.LocalPlayer, this);
                    //if (GetSecondaryPassive() != null && Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().PassivesAreActive[(slot * 2) + 1])
                    //    text += "\n" + GetSecondaryPassive().Tooltip(Main.LocalPlayer, this);
                }
                else
                {
                    if (Active != null)
                    {
                        text += "\n" + Active.Tooltip(Main.LocalPlayer, this);
                    }
                    if (Passives != null)
                    {
                        for (int i = 0; i < Passives.Length; i++)
                        {
                            text += "\n" + Passives[i].Tooltip(Main.LocalPlayer, this);
                        }
                    }

                    //if (GetActive() != null)
                    //    text += "\n" + GetActive().Tooltip(Main.LocalPlayer, this);
                    //if (GetPrimaryPassive() != null)
                    //    text += "\n" + GetPrimaryPassive().Tooltip(Main.LocalPlayer, this);
                    //if (GetSecondaryPassive() != null)
                    //    text += "\n" + GetSecondaryPassive().Tooltip(Main.LocalPlayer, this);
                }

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

        [Obsolete]
        virtual public double GetStatOnPlayer(Player player)
        {
            int slot = TerraLeague.FindAccessorySlotOnPlayer(player, this);

            if (slot != -1)
                return player.GetModPlayer<PLAYERGLOBAL>().accessoryStat[slot];
            else
                return 0;
        }

        [Obsolete]
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
            if (Active != null)
            {
                if (Active.currentlyActive)
                {
                    Active.NPCHit(item, target, ref damage, ref knockback, ref crit, ref OnHitDamage, player, this);
                }
            }
            if (Passives != null)
            {
                for (int i = 0; i < Passives.Length; i++)
                {
                    if (Passives[i].currentlyActive)
                    {
                        Passives[i].NPCHit(item, target, ref damage, ref knockback, ref crit, ref OnHitDamage, player, this);
                    }
                }
            }

            //if (GetPrimaryPassive() != null)
            //    GetPrimaryPassive().NPCHit(item, target, ref damage, ref knockback, ref crit, ref OnHitDamage, player, this);
            //if (GetSecondaryPassive() != null)
            //    GetSecondaryPassive().NPCHit(item, target, ref damage, ref knockback, ref crit, ref OnHitDamage, player, this);
            //if (GetActive() != null)
            //    GetActive().NPCHit(item, target, ref damage, ref knockback, ref crit, ref OnHitDamage, player, this);
        }

        virtual public void NPCHitWithProjectile(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection, ref int OnHitDamage, Player player)
        {
            if (Active != null)
            {
                if (Active.currentlyActive)
                {
                    Active.NPCHitWithProjectile(proj, target, ref damage, ref knockback, ref crit, ref hitDirection, ref OnHitDamage, player, this);
                }
            }
            if (Passives != null)
            {
                for (int i = 0; i < Passives.Length; i++)
                {
                    if (Passives[i].currentlyActive)
                    {
                        Passives[i].NPCHitWithProjectile(proj, target, ref damage, ref knockback, ref crit, ref hitDirection, ref OnHitDamage, player, this);
                    }
                }
            }

            //if (GetPrimaryPassive() != null)
            //    GetPrimaryPassive().NPCHitWithProjectile(proj, target, ref damage, ref knockback, ref crit, ref hitDirection, ref OnHitDamage, player, this);
            //if (GetSecondaryPassive() != null)
            //    GetSecondaryPassive().NPCHitWithProjectile(proj, target, ref damage, ref knockback, ref crit, ref hitDirection, ref OnHitDamage, player, this);
            //if (GetActive() != null)
            //    GetActive().NPCHitWithProjectile(proj, target, ref damage, ref knockback, ref crit, ref hitDirection, ref OnHitDamage, player, this);
        }

        virtual public void OnHitByNPC(NPC npc, ref int damage, ref bool crit, Player player)
        {
            if (Active != null)
            {
                if (Active.currentlyActive)
                {
                    Active.OnHitByNPC(npc, ref damage, ref crit, player, this);
                }
            }
            if (Passives != null)
            {
                for (int i = 0; i < Passives.Length; i++)
                {
                    if (Passives[i].currentlyActive)
                    {
                        Passives[i].OnHitByNPC(npc, ref damage, ref crit, player, this);
                    }
                }
            }

            //if (GetPrimaryPassive() != null)
            //    GetPrimaryPassive().OnHitByNPC(npc, ref damage, ref crit, player, this);
            //if (GetSecondaryPassive() != null)
            //    GetSecondaryPassive().OnHitByNPC(npc, ref damage, ref crit, player, this);
            //if (GetActive() != null)
            //    GetActive().OnHitByNPC(npc, ref damage, ref crit, player, this);
        }

        virtual public void OnHitByProjectile(Projectile proj, ref int damage, ref bool crit, Player player)
        {
            if (Active != null)
            {
                if (Active.currentlyActive)
                {
                    Active.OnHitByProjectile(proj, ref damage, ref crit, player, this);
                }
            }
            if (Passives != null)
            {
                for (int i = 0; i < Passives.Length; i++)
                {
                    if (Passives[i].currentlyActive)
                    {
                        Passives[i].OnHitByProjectile(proj, ref damage, ref crit, player, this);
                    }
                }
            }

            //if (GetPrimaryPassive() != null)
            //    GetPrimaryPassive().OnHitByProjectile(proj, ref damage, ref crit, player, this);
            //if (GetSecondaryPassive() != null)
            //    GetSecondaryPassive().OnHitByProjectile(proj, ref damage, ref crit, player, this);
            //if (GetActive() != null)
            //    GetActive().OnHitByProjectile(proj, ref damage, ref crit, player, this);
        }

        virtual public void OnHitByProjectile(NPC npc, ref int damage, ref bool crit, Player player)
        {
            if (Active != null)
            {
                if (Active.currentlyActive)
                {
                    Active.OnHitByProjectile(npc, ref damage, ref crit, player, this);
                }
            }
            if (Passives != null)
            {
                for (int i = 0; i < Passives.Length; i++)
                {
                    if (Passives[i].currentlyActive)
                    {
                        Passives[i].OnHitByProjectile(npc, ref damage, ref crit, player, this);
                    }
                }
            }

            //if (GetPrimaryPassive() != null)
            //    GetPrimaryPassive().OnHitByProjectile(npc, ref damage, ref crit, player, this);
            //if (GetSecondaryPassive() != null)
            //    GetSecondaryPassive().OnHitByProjectile(npc, ref damage, ref crit, player, this);
            //if (GetActive() != null)
            //    GetActive().OnHitByProjectile(npc, ref damage, ref crit, player, this);
        }

        virtual public void OnKilledNPC(NPC npc, int damage, bool crit, Player player)
        {
            if (Passives != null)
            {
                for (int i = 0; i < Passives.Length; i++)
                {
                    if (Passives[i].currentlyActive)
                    {
                        Passives[i].OnKilledNPC(npc, damage, crit, player, this);
                    }
                }
            }

            //if (GetPrimaryPassive() != null)
            //    GetPrimaryPassive().OnKilledNPC(npc, damage, crit, player, this);
            //if (GetSecondaryPassive() != null)
            //    GetSecondaryPassive().OnKilledNPC(npc, damage, crit, player, this);
            //if (GetActive() != null)
            //    GetActive().NPCHitWithProjectile(proj, target, ref damage, ref knockback, ref crit, ref hitDirection, ref OnHitDamage, player, this);
        }

        [Obsolete]
        virtual public Passive GetPrimaryPassive()
        {
            return null;
        }
        [Obsolete]
        virtual public Passive GetSecondaryPassive()
        {
            return null;
        }
        [Obsolete]
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

        static public int GetPassivePositionInArray(LeagueItem item, Passive passiveToFind)
        {
            if (item != null)
            {
                if (item.Passives != null)
                {
                    for (int i = 0; i < item.Passives.Length; i++)
                    {
                        if (item.Passives.GetType() == passiveToFind.GetType())
                            return i;
                    }
                }
            }
            return -1;
        }

        static public void RunEnabled_PostPlayerUpdate(Player player)
        {
            for (int i = 3; i < 9; i++)
            {
                LeagueItem legItem = player.armor[i].modItem as LeagueItem;

                if (legItem != null)
                {
                    if (legItem.Passives != null)
                    {
                        for (int j = 0; j < legItem.Passives.Length; j++)
                        {
                            if (legItem.Passives[j].currentlyActive)
                                legItem.Passives[j].PostPlayerUpdate(player, legItem);
                        }
                    }
                    if (legItem.Active != null)
                    {
                        if (legItem.Active.currentlyActive)
                            legItem.Active.PostPlayerUpdate(player, legItem);
                    }
                }
            }
        }

        static public void RunEnabled_NPCHit(Player player, Item item, NPC target, ref int damage, ref float knockback, ref bool crit, ref int onhitdamage)
        {
            for (int i = 3; i < 9; i++)
            {
                LeagueItem legItem = player.armor[i].modItem as LeagueItem;

                if (legItem != null)
                {
                    if (legItem.Passives != null)
                    {
                        for (int j = 0; j < legItem.Passives.Length; j++)
                        {
                            if (legItem.Passives[j].currentlyActive)
                                legItem.Passives[j].NPCHit(item, target, ref damage, ref knockback, ref crit, ref onhitdamage, player, legItem);
                        }
                    }
                    if (legItem.Active != null)
                    {
                        if (legItem.Active.currentlyActive)
                            legItem.Active.NPCHit(item, target, ref damage, ref knockback, ref crit, ref onhitdamage, player, legItem);
                    }
                }
            }
        }

        static public void RunEnabled_NPCHitWithProjectile(Player player, Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection, ref int onhitdamage)
        {
            for (int i = 3; i < 9; i++)
            {
                LeagueItem legItem = player.armor[i].modItem as LeagueItem;

                if (legItem != null)
                {
                    if (legItem.Passives != null)
                    {
                        for (int j = 0; j < legItem.Passives.Length; j++)
                        {
                            if (legItem.Passives[j].currentlyActive)
                                legItem.Passives[j].NPCHitWithProjectile(proj, target, ref damage, ref knockback, ref crit, ref hitDirection, ref onhitdamage, player, legItem);
                        }
                    }
                    if (legItem.Active != null)
                    {
                        if (legItem.Active.currentlyActive)
                            legItem.Active.NPCHitWithProjectile(proj, target, ref damage, ref knockback, ref crit, ref hitDirection, ref onhitdamage, player, legItem);
                    }
                }
            }
        }

        static public void RunEnabled_OnHitByNPC(Player player, NPC npc, ref int damage, ref bool crit)
        {
            for (int i = 3; i < 9; i++)
            {
                LeagueItem legItem = player.armor[i].modItem as LeagueItem;

                if (legItem != null)
                {
                    if (legItem.Passives != null)
                    {
                        for (int j = 0; j < legItem.Passives.Length; j++)
                        {
                            if (legItem.Passives[j].currentlyActive)
                                legItem.Passives[j].OnHitByNPC(npc, ref damage, ref crit, player, legItem);
                        }
                    }
                    if (legItem.Active != null)
                    {
                        if (legItem.Active.currentlyActive)
                            legItem.Active.OnHitByNPC(npc, ref damage, ref crit, player, legItem);
                    }
                }
            }
        }

        static public void RunEnabled_OnHitByProjectile(Player player, Projectile proj, ref int damage, ref bool crit)
        {
            for (int i = 3; i < 9; i++)
            {
                LeagueItem legItem = player.armor[i].modItem as LeagueItem;

                if (legItem != null)
                {
                    if (legItem.Passives != null)
                    {
                        for (int j = 0; j < legItem.Passives.Length; j++)
                        {
                            if (legItem.Passives[j].currentlyActive)
                                legItem.Passives[j].OnHitByProjectile(proj, ref damage, ref crit, player, legItem);
                        }
                    }
                    if (legItem.Active != null)
                    {
                        if (legItem.Active.currentlyActive)
                            legItem.Active.OnHitByProjectile(proj, ref damage, ref crit, player, legItem);
                    }
                }
            }
        }

        static public void RunEnabled_OnHitByProjectile(Player player, NPC npc, ref int damage, ref bool crit)
        {
            for (int i = 3; i < 9; i++)
            {
                LeagueItem legItem = player.armor[i].modItem as LeagueItem;

                if (legItem != null)
                {
                    if (legItem.Passives != null)
                    {
                        for (int j = 0; j < legItem.Passives.Length; j++)
                        {
                            if (legItem.Passives[j].currentlyActive)
                                legItem.Passives[j].OnHitByProjectile(npc, ref damage, ref crit, player, legItem);
                        }
                    }
                    if (legItem.Active != null)
                    {
                        if (legItem.Active.currentlyActive)
                            legItem.Active.OnHitByProjectile(npc, ref damage, ref crit, player, legItem);
                    }
                }
            }
        }

        static public void RunEnabled_OnKilledNPC(Player player, NPC npc, ref int damage, ref bool crit)
        {
            for (int i = 3; i < 9; i++)
            {
                LeagueItem legItem = player.armor[i].modItem as LeagueItem;

                if (legItem != null)
                {
                    if (legItem.Passives != null)
                    {
                        for (int j = 0; j < legItem.Passives.Length; j++)
                        {
                            if (legItem.Passives[j].currentlyActive)
                                legItem.Passives[j].OnKilledNPC(npc, damage, crit, player, legItem);
                        }
                    }
                    //if (legItem.Active != null)
                    //{
                    //    legItem.Active.OnKilledNPC();
                    //}
                }
            }
        }

        /// <summary>
        /// Returns false if you live, true if you die, and null if vanilla will decide
        /// </summary>
        /// <param name="player"></param>
        /// <param name="damage"></param>
        /// <param name="hitDirection"></param>
        /// <param name="pvp"></param>
        /// <param name="playSound"></param>
        /// <param name="genGore"></param>
        /// <param name="damageSource"></param>
        /// <returns></returns>
        static public bool? RunEnabled_PreKill(Player player, double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            int doesKill = -1;

            for (int i = 3; i < 9; i++)
            {
                LeagueItem legItem = player.armor[i].modItem as LeagueItem;

                if (legItem != null)
                {
                    if (legItem.Passives != null)
                    {
                        for (int j = 0; j < legItem.Passives.Length; j++)
                        {
                            if (legItem.Passives[j].currentlyActive)
                                doesKill = legItem.Passives[j].PreKill(damage, hitDirection, pvp, ref playSound, ref genGore, ref damageSource, player, legItem);
                        }
                    }
                    //if (legItem.Active != null)
                    //{
                    //    legItem.Active.PreKill(damage, hitDirection, pvp, ref playSound, ref genGore, ref damageSource, player, legItem);
                    //}
                }
            }
            
            if (doesKill != -1)
            {
                return doesKill == 0 ? false : true;
            }
            return null;
        }
    }
}
