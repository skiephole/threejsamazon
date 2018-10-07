using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class Robot : threeDObjects
    {
        double xWaarde, zWaarde;
        List<Node> nodeList = new List<Node>();
        public Robot(double x, double y, double z, double rotationX, double rotationY, double rotationZ)
        {
            this.type = "robot";
            this.guid = Guid.NewGuid();

            this._x = x;
            this._y = y;
            this._z = z;

            this._rX = rotationX;
            this._rY = rotationY;
            this._rZ = rotationZ;


        }
        public void GiveDestination(List<Node> graafPad)
        {
            nodeList = graafPad;
            //Krijg de eerste Node uit de lijst
            Node firstNode = nodeList.First();
            //krijgt de x en y waarde uit de eerste node
            xWaarde = firstNode.X;
            zWaarde = firstNode.Y;
            
        }
        public override bool Update(int tick)
        {
            
            if (xWaarde != this.x)
            {
                if (xWaarde > this.x)
                {
                    this.Move(this.x + 0.1, this.y, this.z);

                }
                if (xWaarde < this.x)
                {
                    this.Move(this.x - 0.1, this.y, this.z);
                }
            }

            else if (zWaarde != this.z)
            {
                if (zWaarde > this.z)
                {
                    this.Move(this.x, this.y, this.z + 0.1);

                }
                if (xWaarde < this.z)
                {
                    this.Move(this.x, this.y, this.z - 0.1);
                }
            }
            else
            {
                nodeList.RemoveAt(0);
            }
            return base.Update(tick);
        }

    }
}
