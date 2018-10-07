using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class SearchEngine
    {
        private Node start;
        private Node end;

        public List<Node> Nodes { get; set; }

        private List<Node> shortestPath;
        private List<Connection> connections;
        private List<Path> paths;
        private List<Node> visitedNodes;
        private Path currentPath;

        //Constructor
        public SearchEngine(List<Node> nodes)
        {
            Nodes = nodes;
        }

        //Returns a list of nodes that represent the shortest path to move from A -> B along the nodes in the list
        public List<Node> FindShortestPath(Node start, Node end)
        {
            //Changing private values
            this.start = start;
            this.end = end;

            //Initiating main variables
            visitedNodes = new List<Node>();

            //Find start
            Node node = start;
            visitedNodes.Add(node);

            //Find Connections
            connections = FindConnections(node);

            //Makes the paths
            paths = MakePaths(connections);

            //Find path lengths
            List<double> pathLengths = FindLengths(paths);

            //Set previous node
            Node previousNode = start;

            //While the given end node has not been visited, continue
            while (!visitedNodes.Contains(end))
            {
                bool searchForNode = true;
                //While search for node is true, keep looping to find a new node
                while (searchForNode)
                {
                    //Find shortest path length
                    double shortesPathLength = pathLengths.Min();
                    //Find the current path using the shorstest path
                    currentPath = paths.Find(i => i.Length == shortesPathLength);
                    //Set current path length to the length of the current path
                    double currentPathLength = currentPath.Length;

                    //Find a temporay node then check if it hasn't been visited and make it the next node
                    Node tempNode = currentPath.EndNode;
                    //If the node we found has NOT been visited it we set the node to the temp node and remove the path length
                    if (!visitedNodes.Contains(tempNode))
                    {
                        node = tempNode;
                        pathLengths.Remove(currentPathLength);
                        paths.Remove(currentPath);
                        //Search for node is set to false to break the while loop
                        searchForNode = false;
                    }
                    //If the node we found has been visited we remove the path length and find a new node
                    else
                    {
                        pathLengths.Remove(shortesPathLength);
                        paths.Remove(currentPath);
                    }
                }

                //When we have found a node to work with we find all the connections on this node
                connections = FindConnections(node);
                //Remove any connections that lead to a node that has already been visited
                connections = RemoveDeadEndConnections(connections);

                //Make a new list of paths using the exisiting path and extending it
                List<Path> extendedPaths = ExtendPaths(connections, currentPath);
                //Merge the old and extended list
                paths = MergePathLists(paths, extendedPaths);

                //Find the lengths of all the paths
                pathLengths = FindLengths(paths);

                //Add the node we looked through to visited nodes
                visitedNodes.Add(node);
            }

            //Create a list of nodes out of the current path and set it to shortest path
            shortestPath = MakeShortestPath(currentPath);

            //!-- UNCOMMENT SELECTION UNDERNEATH TO PRINT THE SHORSTEST PATH TO THE CONSOLE --!
            //foreach (Node shortestPathNode in shortestPath)
            //{
            //    Console.Write(shortestPathNode.Name, ", ");
            //}
            //Console.WriteLine();

            //Return shortest path
            return shortestPath;
        }

        //Makes the shorstest path which is a list of nodes using a given path
        private List<Node> MakeShortestPath(Path path)
        {
            //Initiating node list
            List<Node> nodes = new List<Node>();

            //Add first node
            nodes.Add(path.StartNode);

            //Add all the nodes in between
            foreach (Node node in path.MiddleNodes)
            {
                nodes.Add(node);
            }

            //Add last node
            nodes.Add(path.EndNode);

            //Delete all duplicate nodes and return the list
            nodes = nodes.Distinct().ToList();
            return nodes;
        }

        //Find all the connections on a node (this method is technically redundant but I made it anyway)
        private List<Connection> FindConnections(Node node)
        {
            //Initiating list
            List<Connection> connections = new List<Connection>();

            //Add every connection in the node connection list to the list of connections
            foreach (Connection connection in node.Connections)
            {
                connections.Add(connection);
            }

            //Return found connections
            return connections;
        }

        //Remove all the connections in a list of connections that lead to a node we have already visited
        private List<Connection> RemoveDeadEndConnections(List<Connection> connections)
        {
            //Initiating list
            List<Connection> editedConnections = new List<Connection>();

            //For each connection connection we check if neither of the nodes in the connection have been visited
            foreach (Connection connection in connections)
            {
                //If both the source and connected node are NOT in the visited nodes list the connection is added to the list
                if (!visitedNodes.Contains(connection.SourceNode) && !visitedNodes.Contains(connection.ConnectedNode))
                {
                    editedConnections.Add(connection);
                }
            }

            //Return all connections that dont have any nodes that have been visited
            return editedConnections;
        }

        //Make the first set of paths using the given connections
        private List<Path> MakePaths(List<Connection> connections)
        {
            //Initiating list
            List<Path> paths = new List<Path>();

            //Foreach connection we make a new path using the info in the connection and start, then we add it to the list
            foreach (Connection connection in connections)
            {
                Path path = new Path(start, SourceOrConnectedNode(connection, start));
                paths.Add(path);
            }

            //Return the list of paths
            return paths;
        }

        //Extend all the paths using
        private List<Path> ExtendPaths(List<Connection> connections, Path path)
        {
            //Initiating list
            List<Path> paths = new List<Path>();

            //For each connection in connections we create a copy of the path and tell it to extend using the given connection
            foreach (Connection connection in connections)
            {
                //Making a copy
                Path extendedPath = new Path(start, path.EndNode);
                extendedPath.MiddleNodes = path.MiddleNodes;

                //If the connection source node is the same as the end of the path then the other node on the connection is the extension
                if (connection.SourceNode == path.EndNode)
                {
                    extendedPath.ExtendPath(connection.ConnectedNode);
                    paths.Add(extendedPath);
                }
                //If the connection connected node is the same as the end of the path then the other node on the connection is the extension 
                else if (connection.ConnectedNode == path.EndNode)
                {
                    extendedPath.ExtendPath(connection.SourceNode);
                    paths.Add(extendedPath);
                }
            }

            //Return list of paths that have now been extended
            return paths;
        }

        //Merge 2 lists of paths, couldn't find a built in method that just merged the 2 lists without affecting the data or being weird so i made my own
        private List<Path> MergePathLists(List<Path> list1, List<Path> list2)
        {
            //Initiating list
            List<Path> paths = new List<Path>();

            //Add each path in the first list to the new list
            foreach (Path path in list1)
            {
                paths.Add(path);
            }

            //Add each path in the second list to the new list
            foreach (Path path in list2)
            {
                paths.Add(path);
            }

            //Remove any duplicates
            paths = paths.Distinct().ToList();
            //Return the merged list
            return paths;
        }

        //Find the lengths of a list of paths
        private List<double> FindLengths(List<Path> paths)
        {
            //Initiating list
            List<double> lengths = new List<double>();

            //For each path in paths add the length of the path to the list
            foreach (Path path in paths)
            {
                lengths.Add(path.Length);
            }

            //Returns the list of path lengths
            return lengths;
        }

        //Similar to merge path lists but instead of a list of paths we use a list of doubles
        private List<double> MergePathLengths(List<double> list1, List<double> list2)
        {
            //Initiating list
            List<double> lengths = new List<double>();

            //Add each value in the first list to the new list
            foreach (double length in list1)
            {
                lengths.Add(length);
            }

            //Add each value in the second list to the new list
            foreach (double length in list2)
            {
                lengths.Add(length);
            }

            //Return the list of lengths
            return lengths;
        }

        //Returns the node that is NOT the same node in the connection as the given one
        private Node SourceOrConnectedNode(Connection connection, Node node)
        {
            //If the source node is NOT the same as the given node return source node
            if (!(connection.SourceNode == node))
            {
                return connection.SourceNode;
            }
            //If the connected node is NOT the same as the given node return connected node
            else if (!(connection.ConnectedNode == node))
            {
                return connection.ConnectedNode;
            }

            //Return null because its possible both if statements cannot be fulfilled and then the method doesnt return anything, so just in case we return null here
            return null;
        }
    }
}