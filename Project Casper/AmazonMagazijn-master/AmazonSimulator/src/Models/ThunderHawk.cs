using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Models
{
	public class ThunderHawk : Model
	{
		private int _counter = 100;
		public bool thunderhawkHere = false;
		public bool thunderhawkEmpty = false;

		public ThunderHawk(double x, double y, double z, double rotationX, double rotationY, double rotationZ) : base("thunderhawk", x, y, z, rotationX, rotationY, rotationZ)
		{

		}

		public override bool Update(int tick)
		{
			if (this._x == -20)
			{
				thunderhawkHere = true;
			}
			else
			{
				thunderhawkHere = false;
			}

			if (thunderhawkHere && !thunderhawkEmpty)
			{

			}

			else if (!thunderhawkHere && !thunderhawkEmpty)
			{
				this.Move(this.x + 1, this.y, this.z);
			}

			else if (thunderhawkEmpty == true)
			{
				if (_counter <= 0)
				{
					this.Move(this.x + 2, this.y, this.z);
					thunderhawkHere = false;
				}

				_counter--;
			}

			else
			{
			}

			return base.Update(tick);

		}

		public void RESET()
		{
			thunderhawkHere = false;
			thunderhawkEmpty = false;
			this._x = -150;
		}
	}
}
