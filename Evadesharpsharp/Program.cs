using System;
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
		private static bool isEvading = false;
		static Spell q = new Spell(SpellSlot.Q, 600);
		static System.Timers.Timer aTimer = new System.Timers.Timer();
		static void Main(string[] args)
		{
			CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
		}

		private static void Game_OnGameLoad(EventArgs args)
		{
			Player = ObjectManager.Player;
			_Menu = new Menu("Evade##", "evade", true);
			var commonMenu = new Menu("General", "evade.general");
			commonMenu.AddItem(new MenuItem("evade.general.printinfo", "Print Info in chat").SetValue(true));
			_Menu.AddSubMenu(commonMenu);
			_Menu.AddToMainMenu();
			Game.PrintChat("Evade## loaded");
			Obj_AI_Hero.OnProcessSpellCast+=Obj_AI_Hero_OnProcessSpellCast;
			foreach (var spellData in ObjectManager.Player.Spellbook.Spells)
			{
				//Access the data you need:
				Game.PrintChat(spellData.Name + " " + spellData.SData.SpellCastTime);
			} 
		}

		private static void Obj_AI_Hero_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
		{
			if (!sender.IsMinion)
			{
				if (args.Target.IsMe && q.IsReady() && !args.SData.IsAutoAttack())
				{
					Game.PrintChat("Dodging...1");
					
					Game.PrintChat("" + Game.Time.ToString()  + " " + (args.TimeSpellEnd).ToString());
					aTimer.Elapsed+=new ElapsedEventHandler(OnTimedEvent);
					aTimer.Interval=(args.TimeSpellEnd*-1)+0.4;
					aTimer.Enabled=true;

					
				}
			}
		}

		private static void OnTimedEvent(object sender, ElapsedEventArgs e)
		{
			Game.PrintChat("Dodging...2");
			foreach (var minion in MinionManager.GetMinions(q.Range))
			{
				if (minion.IsValidTarget(q.Range))
				{
					q.Cast(minion);
				}
			}
			aTimer.Enabled = false;
		}



	}
}
