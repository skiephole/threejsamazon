using System;
using System.Collections.Generic;
using System.Linq;
using Controllers;

namespace Models {
    public class World : IObservable<Command>, IUpdatable
    {
        
        private List<threeDObjects> worldObjects = new List<threeDObjects>();
        private List<IObserver<Command>> observers = new List<IObserver<Command>>();
        private List<Node> nodes = new List<Node>();
        public World() {
            Robot r = CreateRobot(0,0,0);
            r.Move(28, 0.1, 15);
            VanCar v = CreateVanCar(0, 0, 0);
            v.Move(14, 0, -3);
            Stellage s1 = CreateStellage(0, 0, 0);
            s1.Move(15, 1.5, 11);
            Stellage s2 = CreateStellage(0, 0, 0);
            s2.Move(15, 1.5, 19);
            Stellage s3 = CreateStellage(0, 0, 0);
            s3.Move(15, 1.5, 27);

            Node A = CreateNode("A", 2, 4);
            Node B = CreateNode("B", 14, 4);
            Node C = CreateNode("C", 28, 4);
            Node D = CreateNode("D", 2, 12);
            Node E = CreateNode("E", 14, 12);
            Node F = CreateNode("F", 28, 12);
            Node G = CreateNode("G", 2, 20);
            Node H = CreateNode("H", 14, 20);
            Node I = CreateNode("I", 28, 20);
            Node J = CreateNode("J", 2, 28);
            Node K = CreateNode("K", 14, 28);
            Node L = CreateNode("L", 28, 28);

            CreateConnection(A, B);
            CreateConnection(A, D);

            CreateConnection(B, A);
            CreateConnection(B, C);

            CreateConnection(C, B);
            CreateConnection(C, F);

            CreateConnection(D, A);
            CreateConnection(D, E);
            CreateConnection(D, G);

            CreateConnection(E, D);
            CreateConnection(E, F);

            CreateConnection(F, C);
            CreateConnection(F, E);
            CreateConnection(F, I);

            CreateConnection(G, D);
            CreateConnection(G, H);
            CreateConnection(G, J);

            CreateConnection(H, G);
            CreateConnection(H, I);

            CreateConnection(I, F);
            CreateConnection(I, H);
            CreateConnection(I, L);

            CreateConnection(J, K);
            CreateConnection(J, G);

            CreateConnection(K, J);
            CreateConnection(K, L);

            CreateConnection(L, K);
            CreateConnection(L, I);



            SearchEngine searchEngine = new SearchEngine(nodes);

            searchEngine.FindShortestPath(A, C);
            searchEngine.FindShortestPath(A, D);

            searchEngine.FindShortestPath(B, E);

            List<Node> NodeList = new List<Node>();
            NodeList = searchEngine.FindShortestPath(B, E); 
            r.GiveDestination(NodeList);
        }


        private Node CreateNode(string name, double x, double y)
        {
            Node n = new Node(name, x, y);
            nodes.Add(n);
            return n;
        }

        private Connection CreateConnection(Node start, Node end)
        {
            Connection c = new Connection(start, end);
            return c;
        }
        private Robot CreateRobot(double x, double y, double z)
        {
            Robot r = new Robot(x, y, z, 0, 0, 0);
            worldObjects.Add(r);
            return r;

        }
        private VanCar CreateVanCar(double x, double y, double z)
        {
            VanCar v = new VanCar(x, y, z, 0, 0, 0);
            worldObjects.Add(v);
            return v;

        }
        private Stellage CreateStellage(double x, double y, double z)
        {
            Stellage s = new Stellage(x, y, z, 0, 0, 0);
            worldObjects.Add(s);
            return s;

        }

        public IDisposable Subscribe(IObserver<Command> observer)
        {
            if (!observers.Contains(observer)) {
                observers.Add(observer);

                SendCreationCommandsToObserver(observer);
            }
            return new Unsubscriber<Command>(observers, observer);
        }

        private void SendCommandToObservers(Command c) {
            for(int i = 0; i < this.observers.Count; i++) {
                this.observers[i].OnNext(c);
            }
        }

        private void SendCreationCommandsToObserver(IObserver<Command> obs) {
            foreach(threeDObjects m3d in worldObjects) {
                obs.OnNext(new UpdateModel3DCommand(m3d));
            }
        }

        public bool Update(int tick)
        {
            for(int i = 0; i < worldObjects.Count; i++) {
                threeDObjects u = worldObjects[i];

                if(u is IUpdatable) {
                    bool needsCommand = ((IUpdatable)u).Update(tick);

                    if(needsCommand) {
                        SendCommandToObservers(new UpdateModel3DCommand(u));
                    }
                }
            }

            return true;
        }
    }

    internal class Unsubscriber<Command> : IDisposable
    {
        private List<IObserver<Command>> _observers;
        private IObserver<Command> _observer;

        internal Unsubscriber(List<IObserver<Command>> observers, IObserver<Command> observer)
        {
            this._observers = observers;
            this._observer = observer;
        }

        public void Dispose() 
        {
            if (_observers.Contains(_observer))
                _observers.Remove(_observer);
        }
    }
}