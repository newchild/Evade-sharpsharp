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
using SpellDetector.Skillshots;

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
			SkillshotDetector.OnSkillshot+=SkillshotDetector_OnSkillshot;
		}

		private static void SkillshotDetector_OnSkillshot(Skillshot spell)
		{
			Game.PrintChat("Spell recoginized");
		}



	}
}
