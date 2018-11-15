using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Controllers;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Models
{
	public class Robot : Movable
	{
		private int hazRun = 0;
		private int hazRunTheSecond = 0;
		private int _counter; //counter that starts when <thHere == true>, sets <rReady> to true
		private int ogCounter;
		public bool thunderhawkHere = false; //is there a thunderhawk docked
		public bool robotPath = false;
		public bool robotReady = false; //is the robot ready to receive a shelf
		public bool robotLoaded = false; //is the robot carrying a shelf
		public bool robotDropped = false; //has the robot dropped the shelf
		public bool robotPlaced = false;
		public bool robotDone = true; //has the robot returned to its place
		public bool robotReset = false;

		public Robot(string rName, double targetX, double targetY, double targetZ, double x, double y, double z, double rotationX, double rotationY, double rotationZ, int counter) : base("robot", x, y, z, rotationX, rotationY, rotationZ)
		{
			this._tX = targetX;
			this._tY = targetY;
			this._tZ = targetZ;

			this.ogCounter = counter;
			this._counter = counter;
		}

		public async void GetPath(string target, List<string> path, List<string> iList, List<double> xList, List<double> zList)
		{
			this._target = target;

			for (int i = 0; i < path.Count(); i++)
			{
				string next = path[i];
				int nodeindex = iList.IndexOf(next);
				double tx = xList[nodeindex];
				double tz = zList[nodeindex];
				this.MoveTarget(tx, 0.301, tz);
				await Task.Delay(4000);
				hazRun++;
			}

			robotPlaced = true;

			if (hazRun == path.Count() && hazRunTheSecond == 0)
			{
				robotDropped = true;
				hazRun = 0;
				hazRunTheSecond++;
			}

			if (hazRun == path.Count() && hazRunTheSecond == 1)
			{
				this._rY = 0;
				this.robotReset = true;
				this.needsUpdate = true;
			}
		}

		public override bool Update(int tick)
		{
			if (this.x >= this._tX - 0.1 && this.x <= this._tX + 0.1)
			{
				if (this.z >= this._tZ - 0.1 && this.z <= this._tZ + 0.1)
				{

				}

				else
				{
					if (this.z < this._tZ)
					{
						this.Move(this.x, this.y, this.z + 0.2);
						this._rY = 0;
					}

					else if (this.z > this._tZ)
					{
						this.Move(this.x, this.y, this.z - 0.2);
						this._rY = -Math.PI;
					}
				}
			}

			else
			{
				if (this.x < this._tX)
				{
					this.Move(this.x + 0.2, this.y, this.z);
					this._rY = Math.PI / 2;
				}

				else if (this.x > this._tX)
				{
					this.Move(this.x - 0.2, this.y, this.z);
					this._rY = -Math.PI / 2;
				}
			}

			if (thunderhawkHere && _counter > 0)
			{
				_counter--;
				Console.WriteLine(_counter);
			}

			if (_counter <= 0 && !robotLoaded)
			{
				robotReady = true;
			}

			return base.Update(tick);
		}

		public void RESET()
		{
			hazRun = 0;
			hazRunTheSecond = 0;
			_counter = ogCounter;
			thunderhawkHere = false;
			robotPath = false;
			robotReady = false;
			robotLoaded = false;
			robotDropped = false;
			robotPlaced = false;
			robotDone = true;
			robotReset = false;

		}
	}
}
