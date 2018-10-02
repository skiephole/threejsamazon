using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class VanCar : threeDObjects
    {
        public VanCar(double x, double y, double z, double rotationX, double rotationY, double rotationZ)
        {
            this.type = "vancar";
            this.guid = Guid.NewGuid();

            this._x = x;
            this._y = y;
            this._z = z;

            this._rX = rotationX;
            this._rY = rotationY;
            this._rZ = rotationZ;
        }
    }
}
