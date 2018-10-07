using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class Connection
    {
        public double Length { get; }
        public Node SourceNode { get; }
        public Node ConnectedNode { get; }

        //Constructor
        public Connection(Node source, Node connected)
        {
            SourceNode = source;
            ConnectedNode = connected;

            //Length = √(Δx²+Δy²)
            Length = Math.Sqrt(Math.Pow(source.X - connected.X, 2) + Math.Pow(source.Y - connected.Y, 2));
            Length = Math.Round(Length, 2);

            //Add connection to both nodes
            SourceNode.AddConnection(this);
            ConnectedNode.AddConnection(this);


        }
    }
}
