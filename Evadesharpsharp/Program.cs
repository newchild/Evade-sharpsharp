using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LeagueSharp;
using SharpDX;
using System.Drawing;
using LeagueSharp.Common;
using System.Threading.Tasks;

namespace Evadesharpsharp
{
	class Program
	{
		private static Menu _Menu;
		private static Obj_AI_Hero Player;
		static void Main(string[] args)
		{
			CustomEvents.Game.OnGameLoad+=Game_OnGameLoad;
		}

		private static void Game_OnGameLoad(EventArgs args)
		{
			_Menu = new Menu("Evade##","evade",true);
			var commonMenu = new Menu("General","evade.general");
			commonMenu.AddItem(new MenuItem("evade.general.printinfo","Print Info in chat").SetValue(true));
			_Menu.AddSubMenu(commonMenu);
			_Menu.AddToMainMenu();
			Game.PrintChat("Evade## loaded");
			Game.OnGameUpdate += Game_OnGameUpdate;
			Obj_AI_Base.OnProcessSpellCast += Obj_AI_Base_OnProcessSpellCast;
		}

		static void Obj_AI_Base_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
		{
			if(!sender.IsMinion)
				Game.Say("/all Spell fired at x: " + args.Start.X + " y: " + args.Start.Y + " z: " + args.Start.Z + " visible? " + sender.IsVisible + " Spellname: " + args.SData.Name);
		}

		

		static void Game_OnGameUpdate(EventArgs args)
		{
			
		}
	}
}
