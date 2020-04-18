using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CGUtilities;
using CGUtilities.DataStructures;



namespace CGAlgorithms.Algorithms.PolygonTriangulation
{
    public enum vertexType {
        Start,
        Split,
        End,
        Merge,
        Regular 
    }
    public class Event {

        public Point vertix;
        public vertexType type; //fnc to find it
        public int edgeIndex;

        Event() { }
        Event(Point v, vertexType t, int i)
        {
            vertix = v;
            type = t;
            edgeIndex = i;
        }

    };

   

    class MonotonePartitioning : Algorithm
    {
        Dictionary<int, Point> helper = new Dictionary<int, Point>();  // edgeIndex , vertix
        double SweepLinePositionY; //keeps track of Y position of sweep line

        Polygon P;
        OrderedSet<Event> Q;
        OrderedSet<Event> T;

        public int PriorityQueueComp(Event ev1, Event ev2) // return -1 if p1 is bigger, 1 if p2 is bigger, 0 if equal
        {
            if (ev1.vertix.Y > ev2.vertix.Y) return -1;
            else if (ev1.vertix.Y == ev2.vertix.Y)
            {
                if (ev1.vertix.X < ev2.vertix.X) return -1;
                else if (ev1.vertix.X == ev2.vertix.X) return 0;
                else return 1;
            }
            else return 1;
        }

        Point intersection(Event ev1)
        {
            Point P1 = P.lines[ev1.edgeIndex].Start;
            Point P2 = P.lines[ev1.edgeIndex].End;
            if (P1.X > P2.X) { P2 = P1; P1 = P.lines[ev1.edgeIndex].End; }

            double Slope = (P2.Y - P1.Y) / (P2.X - P1.X);

            double C = P1.Y - (Slope * P1.X);

            return new Point((SweepLinePositionY - C) / Slope,SweepLinePositionY);

        }

        public int T_mode(Event ev1, Event ev2)
        {
            //intersection of both events with sweep line
            Point P1 = intersection(ev1); 
            Point P2 = intersection(ev2);

            if (P1.X < P1.X) return -1;
            else if (P1.X == P2.X) return 0;
            else return 1;
        }



        public override void Run(List<CGUtilities.Point> points, List<CGUtilities.Line> lines, List<CGUtilities.Polygon> polygons, ref List<CGUtilities.Point> outPoints, ref List<CGUtilities.Line> outLines, ref List<CGUtilities.Polygon> outPolygons)
        {
            P = polygons[0];
            Q = new OrderedSet<Event>(new Comparison<Event>(PriorityQueueComp));
            T = new OrderedSet<Event>(new Comparison<Event>(T_mode));

        }

        public override string ToString()
        {
            return "Monotone Partitioning";
        }
    }
}
