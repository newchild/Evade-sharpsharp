﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LeagueSharp;
using SharpDX;
using SpellDetector;
using System.Drawing;
using LeagueSharp.Common;
using System.Threading.Tasks;
using System.Timers;

namespace Evadesharpsharp
{
	class Program
	{
		private static Menu _Menu;
		private static Obj_AI_Hero Player;
		static GameObjectProcessSpellCastEventArgs argument;
		private static bool isEvading = false;
		static Spell q = new Spell(SpellSlot.Q, 620);
		static System.Timers.Timer aTimer = new System.Timers.Timer();
		static void Main(string[] args)
		{
			CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
		}

		private static void Game_OnGameLoad(EventArgs args)
		{
			Player = ObjectManager.Player;
			if (Player.BaseSkinName != "MasterYi")
				return;
			_Menu = new Menu("Evade##", "evade", true);
			var commonMenu = new Menu("General", "evade.general");
			
			commonMenu.AddItem(new MenuItem("evade.general.printinfo", "Print Info in chat").SetValue(true));
			_Menu.AddSubMenu(commonMenu);
			Menu ts = _Menu.AddSubMenu(new Menu("Target Selector", "Target Selector")); ;
			TargetSelector.AddToMenu(ts);
			_Menu.AddToMainMenu();
			
			
			Game.PrintChat("Evade## loaded");
			Obj_AI_Hero.OnProcessSpellCast+=Obj_AI_Hero_OnProcessSpellCast;
			
		}

		private static void Obj_AI_Hero_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
		{
			
			if (!sender.IsMinion)
			{
				if (args.Target.IsMe && q.IsReady() && !args.SData.IsAutoAttack() && sender.IsEnemy	)
				{
					if (_Menu.Item("evade.general.printinfo").GetValue<bool>())
					{
						Game.PrintChat("Dodging :^)");
					}
					
					Game.PrintChat("" + Game.Time.ToString()  + " " + (args.TimeSpellEnd).ToString());
					aTimer.Elapsed+=new ElapsedEventHandler(OnTimedEvent);
					aTimer.Interval=(args.TimeSpellEnd*-1000)+0.3*1000;
					aTimer.Enabled=true;
					argument = args;

					
				}
				if (args.SData.Name == "KarthusFallenOne")
				{
					if (_Menu.Item("evade.general.printinfo").GetValue<bool>())
					{
						Game.PrintChat("Dodging :^)");
					}
					Game.PrintChat((args.TimeSpellEnd*-1).ToString());
					aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
					aTimer.Interval = (args.TimeCast * -1 * 1000) + 0.3 * 1000 + 2400;
					aTimer.Enabled = true;
					argument = args;
				}
			}
		}

		private static void OnTimedEvent(object sender, ElapsedEventArgs e)
		{
			var x = TargetSelector.GetTarget(q.Range,TargetSelector.DamageType.True);
			if(x.IsValidTarget()){
				q.Cast(x);
				aTimer.Enabled = false;
				return;
				
			}
			foreach (var minion in MinionManager.GetMinions(q.Range))
			{
			
				
					if (minion.IsValidTarget(q.Range))
					{
						q.Cast(minion);
						aTimer.Enabled = false;
						return;
					}
			}
				
				
			
			aTimer.Enabled = false;
		}



	}
}
