using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class Node
    {
        private double _x;
        private double _y;

        public string Name { get; }
        public Guid Guid { get; set; }
        public double X { get { return _x; } }
        public double Y { get { return _y; } }
        public List<Connection> Connections { get; }

        //Constructor
        public Node(string name, double x, double y)
        {
            Guid = Guid.NewGuid();
            Name = name;

            _x = x;
            _y = y;

            Connections = new List<Connection>();
        }

        //Add a connection to the node
        public void AddConnection(Connection connection)
        {
            //If the possible connection is empty because it could not find a connection with the given node it is added
            if (!Connections.Contains(connection))
            {
                Connections.Add(connection);
            }
            //If the possible connection is made and therefore exists in the list it will send an error to the console and do nothing
            else if (Connections.Contains(connection))
            {
                Console.WriteLine("Error! Connection already exists on this node");
            }
        }

        //Remove a connection from a node (not sure if I even need this, but in case I do I made it already)
        public void RemoveConnection(Connection connection)
        {
            //If the possible connection is made and therefore exists in the list it will be removed
            if (Connections.Contains(connection))
            {
                Connections.Remove(connection);
            }
            //If the possible connection is empty because it could not find a connection with the given node it cannot be removed as it does not exist
            else if (!Connections.Contains(connection))
            {
                Console.WriteLine("Error! Connection does not exist on this node");
            }
        }
    }
}