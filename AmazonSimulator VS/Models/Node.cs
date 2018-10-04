using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class Node<T>
    {
        public double x { get; }

        public double y { get; }

        public string name { get; }

        public List<Node<T>> connections { get; }

        private T innerObject;
        public Node(string name, double x, double y)
        {
            this.name = name;
            this.x = x;
            this.y = y;
            this.connections = new List<Node<T>>();

        }
        public T GetObject()
        {
            return innerObject;
        }
        public void GiveObject(T o)
        {
            this.innerObject = o;
        }
    }
}
