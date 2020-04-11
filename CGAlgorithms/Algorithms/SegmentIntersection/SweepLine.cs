using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CGUtilities;
using CGUtilities.DataStructures;
namespace CGAlgorithms.Algorithms.SegmentIntersection
{
    public class Event
    {
        public Point point;
        public string type;
        public int LineIndex;
        public int OtherIndex;
        public Line segment1;
        public Line segment2;

        public Event(Point p, string t, int i, int o, Line s1, Line s2)
        {
            point = p;
            type = t;
            LineIndex = i;
            OtherIndex = o;
            segment1 = s1;
            segment2 = s2;
        }

    }

    class SweepLine : Algorithm
    {

        public OrderedSet<Event> E;
        public OrderedSet<Event> L;
        public OrderedSet<Point> output;

        public Point CheckIntersection(Line line1, Line line2)
        {
            Point s1 = line1.Start;
            Point s2 = line2.Start;
            Point e1 = line1.End;
            Point e2 = line2.End;

            double a1 = e1.Y - s1.Y;
            double b1 = s1.X - e1.X;
            double c1 = a1 * s1.X + b1 * s1.Y;

            double a2 = e2.Y - s2.Y;
            double b2 = s2.X - e2.X;
            double c2 = a2 * s2.X + b2 * s2.Y;

            double delta = a1 * b2 - a2 * b1;
            //If lines are parallel, the result will be null
            if (delta == 0)
                return null;

            Point point = new Point((b2 * c1 - b1 * c2) / delta, (a1 * c2 - a2 * c1) / delta);
            //check if they are on both segments
            bool l1 = HelperMethods.PointOnSegment(point, s1, e1);
            bool l2 = HelperMethods.PointOnSegment(point, s2, e2);

            if (l1 && l2)
                return point;
            else
                return null;



        }

        public override void Run(List<CGUtilities.Point> points, List<CGUtilities.Line> lines, List<CGUtilities.Polygon> polygons, ref List<CGUtilities.Point> outPoints, ref List<CGUtilities.Line> outLines, ref List<CGUtilities.Polygon> outPolygons)
        {


            output = new OrderedSet<Point>();
            E = new OrderedSet<Event>(new Comparison<Event>(CompareEventsX));
            L = new OrderedSet<Event>(new Comparison<Event>(CompareEventsY));
            

            List<CGUtilities.Line> lines2 = new List<Line>();
            List<CGUtilities.Line> lines3 = new List<Line>();
            List<CGUtilities.Line> lines4 = new List<Line>();

            for (int i=0; i<lines.Count; i++)
            {
                lines4.Add(lines[i].Clone() as Line);
                
            }

            lines2 = lines4;
            lines3 = lines4;

            IntializeEvents(lines3);
            while (E.Count != 0)
            {
                Event CurrentPoint = new Event(E[0].point, E[0].type, E[0].LineIndex, E[0].OtherIndex, E[0].segment1, E[0].segment2);
                E.Remove(E[0]);
                HandleEvent(CurrentPoint, lines3, lines2);

            }


            //add the intersection points to output
            for (int i = 0; i < output.Count; i++)
            {
                outPoints.Add(new Point(output[i].X, output[i].Y));
            }

            //outPoints.Add(new Point(0, 0));
            /*output.Clear();
            E.Clear();
            L.Clear();
            lines.Clear();
            for (int i = 0; i < lines4.Count; i++)
            {
                lines.Add(lines4[i].Clone() as Line);
            }*/

        }

        public void HandleEvent(Event CurrentPoint, List<Line> lines, List<Line> lines2)
        {

            if (CurrentPoint.type == "s")
            {
                Event CurrentPointL = new Event(null, null, CurrentPoint.LineIndex, 0, lines[CurrentPoint.LineIndex], null);

                L.Add(CurrentPointL);
                Event tmp1 = L[0];

                KeyValuePair<Event, Event> UpAndDown = L.DirectUpperAndLower(CurrentPointL);

                Point point;
                //upper with point
                if (UpAndDown.Key != null)
                {
                    point = CheckIntersection(lines2[UpAndDown.Key.LineIndex], lines2[CurrentPoint.LineIndex]);
                    if (point != null)
                    {
                        output.Add(point);
                        E.Add(new Event(point, "i", CurrentPoint.LineIndex, UpAndDown.Key.LineIndex, lines[CurrentPoint.LineIndex], lines[UpAndDown.Key.LineIndex]));

                    }
                }
                //lower with point

                if (UpAndDown.Value != null)
                {
                    point = CheckIntersection(lines2[UpAndDown.Value.LineIndex], lines2[CurrentPoint.LineIndex]);
                    if (point != null)
                    {
                        output.Add(point);
                        E.Add(new Event(point, "i", CurrentPoint.LineIndex, UpAndDown.Value.LineIndex, lines[CurrentPoint.LineIndex], lines[UpAndDown.Value.LineIndex]));

                    }
                }

            }
            else if (CurrentPoint.type == "e")
            {
                Event CurrentPointL = new Event(null, null, CurrentPoint.LineIndex, 0, lines[CurrentPoint.LineIndex], null);

                KeyValuePair<Event, Event> UpAndDown = L.DirectUpperAndLower(CurrentPointL);

                if (UpAndDown.Key != null && UpAndDown.Value != null)
                {
                    Point point = CheckIntersection(lines2[UpAndDown.Key.LineIndex], lines2[UpAndDown.Value.LineIndex]);
                    if (point != null)
                    {
                        output.Add(point);
                        E.Add(new Event(point, "i", UpAndDown.Value.LineIndex, UpAndDown.Key.LineIndex, lines[UpAndDown.Value.LineIndex], lines[UpAndDown.Key.LineIndex]));

                    }
                }
                L.Remove(CurrentPointL);


            }
            else
            {
                Line s1 = lines[CurrentPoint.LineIndex]; 
                Line s2 = lines[CurrentPoint.OtherIndex]; 

                int indexS1 = L.IndexOf(new Event(null, null, CurrentPoint.LineIndex, 0, lines[CurrentPoint.LineIndex], null));
                int indexS2 = L.IndexOf(new Event(null, null, CurrentPoint.OtherIndex, 0, lines[CurrentPoint.OtherIndex], null));

                if (indexS1 > indexS2)
                {
                    Line tmp = s2;
                    s2 = s1;
                    s1 = tmp;

                    int tmpi = CurrentPoint.LineIndex;
                    CurrentPoint.LineIndex = CurrentPoint.OtherIndex;
                    CurrentPoint.OtherIndex = tmpi;
                }

                KeyValuePair<Event, Event> UpAndDownS1 = L.DirectUpperAndLower(new Event(null, null, CurrentPoint.LineIndex, 0, s1, null));
                KeyValuePair<Event, Event> UpAndDownS2 = L.DirectUpperAndLower(new Event(null, null, CurrentPoint.OtherIndex, 0, s2, null));
                //s1 with upper of s2
                if (UpAndDownS2.Key != null)
                {
                    Point point1 = CheckIntersection(lines2[CurrentPoint.LineIndex], lines2[UpAndDownS2.Key.LineIndex]);
                    if (point1 != null)
                    {
                        output.Add(point1);
                        E.Add(new Event(point1, "i", CurrentPoint.LineIndex, UpAndDownS2.Key.LineIndex, s1, lines[UpAndDownS2.Key.LineIndex]));

                    }
                }

                //s2 with lower s1
                if (UpAndDownS1.Value != null)
                {
                    Point point2 = CheckIntersection(lines2[CurrentPoint.OtherIndex], lines2[UpAndDownS1.Value.LineIndex]);
                    if (point2 != null)
                    {
                        output.Add(point2);
                        E.Add(new Event(point2, "i", CurrentPoint.OtherIndex, UpAndDownS1.Value.LineIndex, s2, lines[UpAndDownS1.Value.LineIndex]));


                    }
                }
                //swap
                L.Remove(new Event(null, null, CurrentPoint.LineIndex, 0, s1, null));
                L.Remove(new Event(null, null, CurrentPoint.OtherIndex, 0, s2, null));
        
                L.Add(new Event(null, null, CurrentPoint.LineIndex, 0, new Line(CurrentPoint.point, s1.End), null));
                L.Add(new Event(null, null, CurrentPoint.OtherIndex, 0, new Line(CurrentPoint.point, s2.End), null));
                lines[CurrentPoint.LineIndex].Start = CurrentPoint.point;
                lines[CurrentPoint.OtherIndex].Start = CurrentPoint.point;

            }

        }
        public void IntializeEvents(List<Line> lines)
        {
            for (int i = 0; i < lines.Count; i++)
            {
                Point start, end;
                if (lines[i].Start.X == lines[i].End.X)
                {
                    if (lines[i].Start.Y < lines[i].End.Y)
                    {
                        start = lines[i].Start;
                        end = lines[i].End;
                    }
                    else
                    {
                        start = lines[i].End;
                        end = lines[i].Start;
                        lines[i].Start = start;
                        lines[i].End = end;


                    }
                }
                else
                {
                    if (lines[i].Start.X < lines[i].End.X)
                    {
                        start = lines[i].Start;
                        end = lines[i].End;
                    }
                    else
                    {
                        start = lines[i].End;
                        end = lines[i].Start;
                        lines[i].Start = start;
                        lines[i].End = end;

                    }
                }

                E.Add(new Event(start, "s", i, -1, lines[i], null));
                E.Add(new Event(end, "e", i, -1, lines[i], null));

            }
        }
        public static int CompareEventsY(Event p1, Event p2)
        {
            if (p1.segment1.Start.Y < p2.segment1.Start.Y)
                return -1;
            else if (p1.segment1.Start.Y == p2.segment1.Start.Y)
            {
                if (p1.segment1.Start.X < p2.segment1.Start.X) return -1; //lower x in case of tie <   
                else if (p1.segment1.Start.X > p2.segment1.Start.X) return 1;
                else
                {
                    //check on end point
                    if (p1.segment1.End.Y < p2.segment1.End.Y)
                        return -1;
                    else if (p1.segment1.End.Y == p2.segment1.End.Y)
                    {
                        if (p1.segment1.End.X < p2.segment1.End.X) return -1; //lower x in case of tie <   
                        else if (p1.segment1.End.X > p2.segment1.End.X) return 1;
                        else return 0;

                    }
                    else
                        return 1;
                }
            }
            else
                return 1;
        }
        public static int CompareEventsX(Event p1, Event p2)
        {
            if (p1.point.X < p2.point.X)
                return -1;
            else if (p1.point.X == p2.point.X)
            {
                if (p1.point.Y < p2.point.Y) return -1;
                else if (p1.point.Y > p2.point.Y) return 1;
                else return 0;
            }
            else
                return 1;
        }
        public override string ToString()
        {
            return "Sweep Line";
        }
    }
}