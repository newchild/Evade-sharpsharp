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

namespace Evadesharpsharp
{
	class Program
	{
		private static Menu _Menu;
		private static Obj_AI_Hero Player;
		private static bool isEvading = false;
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
				if (args.Target.IsMe)
				{
					Game.PrintChat("Dodging...1");
					Spell q = new Spell(SpellSlot.Q,600);
					Game.PrintChat("" + Game.Time.ToString()  + " " + (args.TimeSpellEnd).ToString());
					while(!(0 >= (args.TimeSpellEnd*-1)+0.5)){
						
					}
					Game.PrintChat("Dodging...");
					foreach (var minion in MinionManager.GetMinions(q.Range))
					{
						if (minion.IsValidTarget(q.Range))
						{
							q.Cast(minion);
						}
					}
				}
			}
		}



	}
}
