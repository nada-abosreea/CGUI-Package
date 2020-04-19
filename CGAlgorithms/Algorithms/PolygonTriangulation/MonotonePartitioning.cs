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

        public Event() { }
        public Event(Point v, vertexType t, int i)
        {
            vertix = v;
            type = t;
            edgeIndex = i;
        }

    };

   

    class MonotonePartitioning : Algorithm
    {
        Dictionary<int, Event> helper = new Dictionary<int, Event>();  // edgeIndex , vertix
        double SweepLinePositionY; //keeps track of Y position of sweep line

        Polygon P;
        OrderedSet<Event> Q;
        OrderedSet<Event> T;

        List<Line> output = new List<Line>();

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

        public vertexType getVertexType(int index) //index of vertex
        {
            Enums.TurnType turn = new Enums.TurnType();
            //double angle = 0;

            Point Prev, Vertex, Next;
            Vertex = P.lines[index].Start;
            Next = P.lines[index % P.lines.Count].End;
            if (index == 0) Prev = P.lines[P.lines.Count - 1].Start;    
            else Prev = P.lines[index - 1].Start;     
            
            turn = HelperMethods.CheckTurn(new Line(Prev, Vertex), Next);

            /*
            Point CB = new Point(Vertex.X - Prev.X, Vertex.Y - Prev.Y);
            Point BA = new Point(Next.X - Vertex.X , Next.Y - Vertex.Y);
            double dot = (CB.X * BA.Y) - (CB.Y * BA.X);            double CBnorm = Math.Sqrt((CB.X * CB.X) + (CB.Y * CB.Y));
            double BAnorm = Math.Sqrt((BA.X * BA.X) + (BA.Y * BA.Y));

            angle = Math.Abs(Math.Asin(dot / (CBnorm * BAnorm)));*/

            //Left <180, Right >180
            if (turn == Enums.TurnType.Left && Prev.Y < Vertex.Y && Next.Y < Vertex.Y)
                return vertexType.Start;
            else if (turn == Enums.TurnType.Right && Prev.Y < Vertex.Y && Next.Y < Vertex.Y)
                return vertexType.Split;
            else if (turn == Enums.TurnType.Left && Prev.Y > Vertex.Y && Next.Y > Vertex.Y)
                return vertexType.End;
            else if (turn == Enums.TurnType.Right && Prev.Y > Vertex.Y && Next.Y > Vertex.Y)
                return vertexType.Merge;
            else 
                return vertexType.Regular;

        }

        public override void Run(List<CGUtilities.Point> points, List<CGUtilities.Line> lines, List<CGUtilities.Polygon> polygons, ref List<CGUtilities.Point> outPoints, ref List<CGUtilities.Line> outLines, ref List<CGUtilities.Polygon> outPolygons)
        {
            P = polygons[0];
            Q = new OrderedSet<Event>(new Comparison<Event>(PriorityQueueComp));
            T = new OrderedSet<Event>(new Comparison<Event>(T_mode));


            for(int i=0;i<P.lines.Count;i++)
            {
                Event ev = new Event(P.lines[i].Start, getVertexType(i),i);
                Q.Add(ev);
            }

            SweepLinePositionY = Q.GetFirst().vertix.Y;

            while (Q.Count > 0)
            {
                Event ev = Q.GetFirst();
                SweepLinePositionY = Q.GetFirst().vertix.Y;
                Q.RemoveFirst(); //pop

                if (ev.type == vertexType.Start) HandleStart(ev);
                else if (ev.type == vertexType.End) HandleEnd(ev);
                else if (ev.type == vertexType.Split) HandleSplit(ev);
                else if (ev.type == vertexType.Merge) HandleMerge(ev);
                else if (ev.type == vertexType.Regular) HandleRegular(ev);

            }

            foreach(Line l in output)
            {
                outLines.Add(l);
            }

        }

        private void HandleStart(Event ev)
        {
            T.Add(ev);
            helper[ev.edgeIndex] = ev;
            //throw new NotImplementedException();
        }

        private void HandleEnd(Event ev)
        {
            if(helper[ev.edgeIndex-1].type == vertexType.Merge && ev.edgeIndex!=0)
            {
                output.Add(new Line(ev.vertix, helper[ev.edgeIndex - 1].vertix));
            }
            if (helper[P.lines.Count - 1].type == vertexType.Merge && ev.edgeIndex == 0)
            {
                output.Add(new Line(ev.vertix, helper[ev.edgeIndex - 1].vertix));
            }
            T.Remove(ev);
            //throw new NotImplementedException();
        }

        private void HandleSplit(Event ev)
        {
            Event Left = T.DirectUpperAndLower(ev).Value;
            output.Add(new Line(ev.vertix,helper[Left.edgeIndex].vertix)); //here
            helper[Left.edgeIndex] = ev;
            T.Add(ev);
            helper[ev.edgeIndex] = ev;
            //throw new NotImplementedException();
        }

        private void HandleMerge(Event ev)
        {
            if(helper[ev.edgeIndex-1].type == vertexType.Merge)
            {
                output.Add(new Line(ev.vertix, helper[ev.edgeIndex - 1].vertix));
            }

            if (ev.edgeIndex != 0)
            {
                Event prev = new Event(P.lines[ev.edgeIndex - 1].Start, getVertexType(ev.edgeIndex - 1), ev.edgeIndex - 1);
                T.Remove(prev);
            }
            else
            {
                Event prev = new Event(P.lines[P.lines.Count- 1].Start, getVertexType(P.lines.Count - 1), P.lines.Count - 1);
                T.Remove(prev);
            }

            Event Left = T.DirectUpperAndLower(ev).Value;
            
            if(helper[Left.edgeIndex].type== vertexType.Merge)
            {
                output.Add(new Line(ev.vertix,helper[Left.edgeIndex].vertix));
            }

            helper[Left.edgeIndex] = ev;
            //throw new NotImplementedException();
        }

        private void HandleRegular(Event ev)
        {
            Point Prev, Next;
            Next = P.lines[ev.edgeIndex % P.lines.Count].End;
            if (ev.edgeIndex == 0) Prev = P.lines[P.lines.Count - 1].Start;
            else Prev = P.lines[ev.edgeIndex - 1].Start;

            if(Prev.Y > Next.Y) //P interior is at the right 
            {
                if(helper[ev.edgeIndex-1].type == vertexType.Merge)
                {
                    output.Add(new Line(ev.vertix, helper[ev.edgeIndex - 1].vertix));
                }

                Event prv = new Event(P.lines[ev.edgeIndex - 1].Start, getVertexType(ev.edgeIndex - 1), ev.edgeIndex - 1);
                T.Remove(prv);

                T.Add(ev);
                helper[ev.edgeIndex] = ev;

            }
            else
            {
                //Left is null
                Event Left = T.DirectUpperAndLower(ev).Value;
                
                if(helper[Left.edgeIndex].type == vertexType.Merge)
                {
                    output.Add(new Line(ev.vertix,helper[Left.edgeIndex].vertix));
                }
                helper[Left.edgeIndex] = ev;
            }
            //throw new NotImplementedException();
        }


        public override string ToString()
        {
            return "Monotone Partitioning";
        }
    }
}
