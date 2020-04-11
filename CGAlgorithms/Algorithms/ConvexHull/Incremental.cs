using CGUtilities;
using CGUtilities.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class Incremental : Algorithm
    {
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
 
            OrderedDictionary<double, Point> point = new OrderedDictionary<double, Point>();
            OrderedSet<double> angles = new OrderedSet<double>();
            if (points.Count == 1)
            {
                outPoints.Add(points[0]);
            }
            else if (points.Count == 2 && points[0] != points[1])
            {
                outPoints.Add(points[0]);
                outPoints.Add(points[1]);
            }
            else
            {
                double X = 0;
                double Y = 0;
                for (int i = 0; i < 3; i++)
                {
                    X += points[i].X;
                    Y += points[i].Y;
                }
                Point medPoint = new Point(X / 3, Y / 3);
                Point medPoint2 = new Point((X / 3) + 100, Y / 3);
                
                Point ab = new Point(medPoint.X - medPoint2.X, medPoint.Y - medPoint2.Y);
              
                for (int i = 0; i < points.Count; i++)
                {
                    Point ac = new Point(points[i].X - medPoint.X, points[i].Y - medPoint.Y);
                    double cross = HelperMethods.CrossProduct(ab, ac);
                    double dot = (ab.X * ac.X) + (ab.Y * ac.Y);

                    double angle = Math.Atan2(cross, dot);
                    if (angle < 0)
                        angle = angle + (2 * Math.PI);
                    angles.Add(angle);
                    point[angle] = points[i];
                }
                for (int i = 3; i < point.Count; i++)
                {
                    double next = 0;
                    double prev = 0;
                        if (angles[i] == angles.Max())
                        {
                            next = angles[0];
                            prev = angles.Max();
                        }
                        else if (angles[i] == angles.Min())
                        {
                            prev = angles[0];
                            next = angles.Max();
                        }
                        else
                        {
                            KeyValuePair<double, double> x = angles.DirectUpperAndLower(angles[i]);
                            next = x.Key;
                            if (next == 0.0)
                            {
                                next = angles[0];
                            }
                            prev = x.Value;
                            if (prev == 0.0)
                            {
                                prev = angles.Max();
                            }
                        }
                    
                    if (HelperMethods.CheckTurn(new Line(point[prev], point[next]), point.ElementAt(i).Value) == Enums.TurnType.Right || HelperMethods.CheckTurn(new Line(point[prev], point[next]), point.ElementAt(i).Value) == Enums.TurnType.Colinear)
                        {
                            int count = 0;
                            double newprev = angles.DirectUpperAndLower(prev).Value;
                            if (newprev == 0.0)
                            {
                                newprev = angles.Max();
                            }
                       
                            while (HelperMethods.CheckTurn(new Line(point[angles[i]], point[prev]), point[newprev]) == Enums.TurnType.Left)
                            {
                                double oldprev = prev;
                                prev = newprev;
                                newprev = angles.DirectUpperAndLower(prev).Value;
                                if (newprev == 0.0)
                                {
                                    newprev = angles.Max();
                                }
                                point.Remove(oldprev);
                                angles.Remove(oldprev);
                                i--;
                            }
                            double newnext = angles.DirectUpperAndLower(next).Key;
                            if (newnext == 0.0)
                            {
                                newnext = angles[0];
                            }

                            while (HelperMethods.CheckTurn(new Line(point[angles[i]], point[next]), point[newnext]) == Enums.TurnType.Right)
                            {
                                double oldnext = next;
                                next = newnext;
                                newnext = angles.DirectUpperAndLower(next).Key;
                                if (newnext == 0.0)
                                {
                                    newnext = angles[0];
                                }
                                point.Remove(oldnext);
                                angles.Remove(oldnext);
                            }
                        }
                        else
                        {
                            point.Remove(angles[i]);
                            angles.Remove(angles[i]);
                            i--;
                        }
                }
            }
            for (int i = 0; i < point.Count; i++)
            {
                outPoints.Add(point.ElementAt(i).Value);
            }
        }

        public override string ToString()
        {
            return "Convex Hull - Incremental";
        }
    }
}
