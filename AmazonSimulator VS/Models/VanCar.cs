using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class VanCar : threeDObjects
    {
        double beginZ = 0;
        double beginX = 0;
        double eindX = 28;
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
             public override bool Update(int tick)
        {
            if (this.x < eindX)
            {
                this.Move(this.x + 0.1, this.y, this.z);
            }
            if (this.x > eindX)
            {
                this.Move(beginX, this.y, this.z);
            }
            return base.Update(tick);
        }
    }
}
