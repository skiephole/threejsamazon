using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Models;
using Views;

namespace Controllers
{
	struct ObservingClient
	{
		public View currentView;
		public IDisposable unsubscribe;
	}

	public class SimulationController
	{
		private World world;
		private List<ObservingClient> views = new List<ObservingClient>();
		private bool running = false;
		private int tickTime = 50;
		private int ReadySetGo = 0;
		private int cycles = 0;

		public SimulationController(World world)
		{
			this.world = world;
		}

		public void AddView(View view)
		{
			ObservingClient oc = new ObservingClient();

			oc.unsubscribe = this.world.Subscribe(view);
			oc.currentView = view;

			views.Add(oc);
		}

		public void RemoveView(View view)
		{
			for (int i = 0; i < views.Count; i++)
			{
				ObservingClient currentOC = views[i];

				if (currentOC.currentView == view)
				{
					views.Remove(currentOC);
					currentOC.unsubscribe.Dispose();
				}
			}
		}

		public void Simulate()
		{
			running = true;

			while (running)
			{
				ThunderHawk thunderhawk = (ThunderHawk)world.worldObjects[4];
				Robot robot1 = (Robot)world.worldObjects[0];
				Robot robot2 = (Robot)world.worldObjects[1];
				Robot robot3 = (Robot)world.worldObjects[2];
				Robot robot4 = (Robot)world.worldObjects[3];
				Shelf shelf1 = (Shelf)world.worldObjects[28 + (4 * cycles)];
				Shelf shelf2 = (Shelf)world.worldObjects[29 + (4 * cycles)];
				Shelf shelf3 = (Shelf)world.worldObjects[30 + (4 * cycles)];
				Shelf shelf4 = (Shelf)world.worldObjects[31 + (4 * cycles)];

				if (thunderhawk.thunderhawkHere == true)
				{
					robot1.thunderhawkHere = true;
					robot2.thunderhawkHere = true;
					robot3.thunderhawkHere = true;
					robot4.thunderhawkHere = true;
				}

				if (robot1.robotReady == true)
				{
					world.AddShelfToRobot(robot1, shelf1);
					shelf1.needsUpdate = true;
					robot1.robotLoaded = true;
					ReadySetGo++;
				}

				if (robot2.robotReady == true)
				{
					world.AddShelfToRobot(robot2, shelf2);
					shelf2.needsUpdate = true;
					robot2.robotLoaded = true;
					ReadySetGo++;
				}

				if (robot3.robotReady == true)
				{

					world.AddShelfToRobot(robot3, shelf3);
					shelf3.needsUpdate = true;
					robot3.robotLoaded = true;
					ReadySetGo++;
				}

				if (robot4.robotReady == true)
				{
					world.AddShelfToRobot(robot4, shelf4);
					shelf4.needsUpdate = true;
					robot4.robotLoaded = true;
					ReadySetGo++;
				}

				if (robot1.robotDropped == true)
				{
					world.RobotGoesBack(robot1, robot1._target, "A");
				}

				if (robot2.robotDropped == true)
				{
					world.RobotGoesBack(robot2, robot2._target, "null1");
				}

				if (robot3.robotDropped == true)
				{
					world.RobotGoesBack(robot3, robot3._target, "null2");
				}

				if (robot4.robotDropped == true)
				{
					world.RobotGoesBack(robot4, robot4._target, "null3");
				}

				if (ReadySetGo == 4)
				{
					thunderhawk.thunderhawkEmpty = true;
					robot1.robotDone = false;
					robot2.robotDone = false;
					robot3.robotDone = false;
					robot4.robotDone = false;
					ReadySetGo++;
					world.THHere();
					Console.WriteLine("gay,haha");
				}

				if ((robot1.robotReady == true || robot1.robotDone == false) && robot1.robotPlaced == false)
				{
					shelf1._x = robot1._x;
					shelf1._z = robot1._z;
					shelf1.needsUpdate = true;
				}

				if ((robot2.robotReady == true || robot2.robotDone == false) && robot2.robotPlaced == false)
				{
					shelf2._x = robot2._x;
					shelf2._z = robot2._z;
					shelf2.needsUpdate = true;
				}

				if ((robot3.robotReady == true || robot3.robotDone == false) && robot3.robotPlaced == false)
				{
					shelf3._x = robot3._x;
					shelf3._z = robot3._z;
					shelf3.needsUpdate = true;
				}

				if ((robot4.robotReady == true || robot4.robotDone == false) && robot4.robotPlaced == false)
				{
					shelf4._x = robot4._x;
					shelf4._z = robot4._z;
					shelf4.needsUpdate = true;
				}

				if (robot1.robotReset == true && robot2.robotReset == true && robot3.robotReset == true && robot4.robotReset == true)
				{
					ReadySetGo = 0;
					cycles++;
					robot1.RESET();
					robot2.RESET();
					robot3.RESET();
					robot4.RESET();
					thunderhawk.RESET();
					// world.RESET();
				}

				world.Update(tickTime);
				Thread.Sleep(tickTime);
			}
		}

		public void EndSimulation()
		{
			running = false;
		}
	}
}
