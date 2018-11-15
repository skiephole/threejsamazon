using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Controllers;

namespace Models
{
	public class Node : Model
	{
		string _i;
		public Node(string i, double x, double y, double z, double rotationX, double rotationY, double rotationZ) : base("node", x, y, z, rotationX, rotationY, rotationZ)
		{
			this._y = 0.001;
			this._rX = Math.PI / 2.0;
			this._i = i;
		}
	}
}
