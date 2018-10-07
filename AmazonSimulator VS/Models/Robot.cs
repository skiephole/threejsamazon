using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class Robot : threeDObjects
    {
        int count = 0;
        bool heenweg = true;
        double xWaarde1, zWaarde1, xWaarde2, zWaarde2;
        static double xStellage, zStellage;
        List<Node> heenList = new List<Node>();
        List<Node> terugList = new List<Node>();
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
        public void RouteHeenweg(List<Node> graafPad)
        {
            heenList = graafPad;
            //Krijg de eerste Node uit de lijst
            Node firstNode = heenList.First();
            //krijgt de x en y waarde uit de eerste node
            xWaarde1 = firstNode.X;
            zWaarde1 = firstNode.Y;
        }
        public void RouteTerugweg(List<Node> graafPad)
        {
            terugList = graafPad;
            //Krijg de eerste Node uit de lijst
            Node firstNode = terugList.First();
            //krijgt de x en y waarde uit de eerste node
            xWaarde2 = firstNode.X;
            zWaarde2 = firstNode.Y;
        }


        public override bool Update(int tick)
        {
           
                if (heenweg == true)
                {
                    if (zWaarde1 != Math.Round(z, 2))
                    {
                        if (zWaarde1 > Math.Round(z, 2))
                        {
                            this.Move(this.x, this.y, this.z + 0.1);

                        }
                        else if (zWaarde1 < Math.Round(z, 2))
                        {
                            this.Move(this.x, this.y, this.z - 0.1);
                        }
                    }
                    else if (xWaarde1 != Math.Round(x, 2))
                    {

                        if (xWaarde1 > Math.Round(x, 2))
                        {
                            this.Move(this.x + 0.1, this.y, this.z);


                        }
                        else if (xWaarde1 < Math.Round(x, 2))
                        {
                            this.Move(this.x - 0.1, this.y, this.z);

                        }
                    }
                    else if (xWaarde1 == Math.Round(x, 2) && zWaarde1 == Math.Round(z, 2))
                    {

                    count++;
                        heenList.RemoveAt(0);
                        RouteHeenweg(heenList);
                    if (count == 4)
                    {
                        heenweg = false;
                    }
                        
                }
                
            }
                else if (heenweg == false)
                {
                count = 0;
                
                    if (zWaarde2 != Math.Round(z, 2))
                    {
                        if (zWaarde2 > Math.Round(z, 2))
                        {
                            this.Move(this.x, this.y, this.z + 0.1);

                        }
                        else if (zWaarde2 < Math.Round(z, 2))
                        {
                            this.Move(this.x, this.y, this.z - 0.1);
                        }
                    }
                    else if (xWaarde2 != Math.Round(x, 2))
                    {

                        if (xWaarde2 > Math.Round(x, 2))
                        {
                            this.Move(this.x + 0.1, this.y, this.z);


                        }
                        else if (xWaarde2 < Math.Round(x, 2))
                        {
                            this.Move(this.x - 0.1, this.y, this.z);

                        }
                    }
                    else if (xWaarde2 == Math.Round(x, 2) && zWaarde2 == Math.Round(z, 2))
                    {
                        
                        terugList.RemoveAt(0);
                        RouteTerugweg(terugList);
                    }
                }


            return base.Update(tick);
        }

    }
}
