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
			CustomEvents.Game.OnGameLoad+=Game_OnGameLoad;
		}

		private static void Game_OnGameLoad(EventArgs args)
		{
			Player = ObjectManager.Player;
			_Menu = new Menu("Evade##","evade",true);
			var commonMenu = new Menu("General","evade.general");
			commonMenu.AddItem(new MenuItem("evade.general.printinfo","Print Info in chat").SetValue(true));
			_Menu.AddSubMenu(commonMenu);
			_Menu.AddToMainMenu();
			Game.PrintChat("Evade## loaded");
			Game.OnGameUpdate += Game_OnGameUpdate;
			Obj_AI_Base.OnProcessSpellCast += Obj_AI_Base_OnProcessSpellCast;
			Obj_AI_Hero.OnIssueOrder += Obj_AI_Hero_OnIssueOrder;
		}

		

		static void Obj_AI_Base_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
		{
			SpellDetector.Skillshots.SkillshotData data = new SpellDetector.Skillshots.SkillshotData();
			SpellDetector.Skillshots.Skillshot help = new SpellDetector.Skillshots.Skillshot(SpellDetector.Skillshots.DetectionType.ProcessSpell, data, (int)args.TimeCast, args.Start.To2D(), args.End.To2D(), sender);
			if (help.IsDanger(Player.Position.To2D()))
			{
				moveToBestLocation(sender,args,help);
			};
		}

		
		

		static void Game_OnGameUpdate(EventArgs args)
		{
			
		}
		static void Obj_AI_Hero_OnIssueOrder(Obj_AI_Base sender, GameObjectIssueOrderEventArgs args)
		{
			if (isEvading)
			{
				args.Process = false;
			}
		}
		static void moveToBestLocation(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args, SpellDetector.Skillshots.Skillshot skill)
		{
			Vector2 bufferVector = sender.Position.To2D();
			for (int i = -100; i <= 100; i++)
			{
				for (int j = -100; j <= 100; j++)
				{
					bufferVector.X = sender.Position.To2D().X + i;
					bufferVector.Y = sender.Position.To2D().Y + j;
					if (!skill.IsDanger(bufferVector) && bufferVector.IsValid() && !bufferVector.IsWall())
					{
						Player.IssueOrder(GameObjectOrder.MoveTo, bufferVector.To3D());
						isEvading = true;
						while(Player.IsMoving){

						}
						isEvading = false;
						goto End;
					}

				}
			}
			Game.PrintChat("Couldn't find evade spot");
			return;
		End:
			Game.PrintChat("Successfully evaded!");
		}
	}
}
