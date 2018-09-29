﻿using System;
using System.Collections.Generic;
using System.Linq;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;
using SFarmer = StardewValley.Farmer;

namespace QiExchanger
{
   public class ModEntry : Mod
    {
        private ModConfig Config;
        private int ExchangeRate;
        public override void Entry(IModHelper helper)
        {
            this.Config = helper.ReadConfig<ModConfig>();

            //Set Up Events
            ControlEvents.KeyPressed += this.Control_Events_Key_Pressed;
        }
        public void Control_Events_Key_Pressed(object sender, EventArgsKeyPressed e)
        {
            if (!Context.IsWorldReady)
                return;
            if (e.KeyPressed.ToString() == this.Config.ActivationKey)
                doMenu();

            this.ExchangeRate = this.Config.ExchangeRate;
        }
        //Void to Show Menu with the options
        private void doMenu()
        {
            var i18n = this.Helper.Translation;
            List<Response> options = new List<Response>();
            bool hasQiCoins = true;
            SFarmer Player = Game1.player;
            if (Player.clubCoins == 0)
                hasQiCoins = false;
            if (Player.clubCoins >= 100 && hasQiCoins)
                options.Add(new Response("100", i18n.Get("option.one")));
            if(Player.clubCoins >= 1000 && hasQiCoins)
                options.Add(new Response("1000", i18n.Get("option.two")));
            if (Player.clubCoins >= 10000 && hasQiCoins)
                options.Add(new Response("10000", i18n.Get("option.three")));
            if (Player.clubCoins >= 100000 && hasQiCoins)
                options.Add(new Response("100000", i18n.Get("option.four")));
            if (Player.clubCoins >= 1000000 && hasQiCoins)
                options.Add(new Response("1000000", i18n.Get("option.five")));
            if (hasQiCoins)
            {
                options.Add(new Response("exit", "None Today"));
                Game1.currentLocation.createQuestionDialogue(i18n.Get("main.text", new { player_name = Player.Name, qi_amount = Player.clubCoins, exchange_rate = this.ExchangeRate }), options.ToArray(), new GameLocation.afterQuestionBehavior(this.answer), (NPC)null);
            }
            else
            {
                // Game1.addHUDMessage (new HUDMessage(i18n.Get("no.qi.coins")));   
                Game1.drawObjectDialogue(i18n.Get("no.qi.coins"));
            }
        }
        //void to do the Answers
        private void answer(SFarmer who, string answer)
        {
            string ans = answer.Split(' ')[0];
            if(ans != "exit")
            {
                doExchange(Convert.ToInt32(ans));
            }
        }
        //Void that exchanges Qi Coins for Gold.
        public void doExchange(int inValue)
        {
            var i18n = Helper.Translation;
            if (inValue > 0)
            {
                if(Game1.player.clubCoins >= inValue)
                {
                    int outter = this.ExchangeRate > 0 ? (inValue * this.ExchangeRate) : 0;
                    Game1.player.clubCoins -= inValue;
                    Game1.player.Money += Math.Max(0, inValue * this.ExchangeRate);
                    Game1.drawObjectDialogue(i18n.Get("do.exchange", new { qi_coins = inValue, g_coins = outter}));
                }
                else
                {                    
                    Game1.drawObjectDialogue(i18n.Get("not.enough.qicoins"));
                }
                
                
            }
            else
            {
                this.Monitor.Log("Negative value was passed through.");
            }
        }
    }
}
