using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class Path
    {
        public double Length { get; set; }
        public Node StartNode { get; }
        public List<Node> MiddleNodes { get; set; }
        public Node EndNode { get; set; }

        //Constructor
        public Path(Node start, Node end)
        {
            StartNode = start;
            MiddleNodes = new List<Node>();
            EndNode = end;

            Length = CalculateLength(start, end);
        }

        //Calculates the length between two nodes
        private double CalculateLength(Node node1, Node node2)
        {
            //Length = √(Δx²+Δy²)
            Length = Math.Sqrt(Math.Pow(node1.X - node2.X, 2) + Math.Pow(node1.Y - node2.Y, 2));
            //Length is rounded to 2 decimals and returned
            Length = Math.Round(Length, 2);
            return Length;
        }

        //Extends the path with a given node
        public Path ExtendPath(Node extension)
        {
            //Find new length for this path
            Length = ExtendLength(extension);
            //Add the end node to middle nodes
            MiddleNodes.Add(EndNode);
            //Removes duplicates in the list
            MiddleNodes = MiddleNodes.Distinct().ToList();
            //Set the end node to the extension
            EndNode = extension;

            //Returns this path
            return this;
        }

        //Extend the length of the path with a new length using a node
        private double ExtendLength(Node extension)
        {
            //Find the length between the end node and the extension
            double extendedLength = CalculateLength(EndNode, extension);
            //Length is rounded to 2 decimals and returned
            Length = Math.Round(Length + extendedLength, 2);
            return Length;
        }
    }
}