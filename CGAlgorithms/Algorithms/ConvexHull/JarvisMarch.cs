
using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class JarvisMarch : Algorithm
    {
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {


            List<Point> points2 = new List<Point>();

            foreach (var item in points)
            {
                if (!points2.Contains(item))
                    points2.Add(item);
            }
            points = points2;


            Point minP;
            double minY = 100000000, minX = 0;
            for (int i = 0; i < points.Count; i++)
            {
                if (points[i].Y < minY)
                {
                    minY = points[i].Y;
                    minX = points[i].X;
                    minP = points[i];
                }
            }

            Point m = new Point(minX, minY);
            Point ex = new Point(minX - 20, minY);
            Point start = m;



            outPoints.Add(m);
            while (true)
            {

                double largest_angle = 0;
                Point next = m;
                double dist = 0;
                double largest_dist = 0;
                for (int i = 0; i < points.Count; i++)
                {
                    Point ab = new Point(m.X - ex.X, m.Y - ex.Y);
                    Point ac = new Point(points[i].X - m.X, points[i].Y - m.Y);

                    double cross = HelperMethods.CrossProduct(ab, ac);
                    double dot = (ab.X * ac.X) + (ab.Y * ac.Y);

                    double angle = Math.Atan2(cross, dot);
                    if (angle < 0)
                        angle = angle + (2*Math.PI);

                    dist = Math.Sqrt((m.X - points[i].X) + (m.Y - points[i].Y));
                    if (angle > largest_angle)
                    {
                        
                        largest_angle = angle;
                        largest_dist = dist;
                        next = points[i];
                    }
                    else if (angle == largest_angle && dist > largest_dist)
                    {
                        
                        largest_dist = dist;
                        next = points[i];

                    }


                }

                outLines.Add(new Line(next, m));
                if (start.X == next.X && start.Y == next.Y)
                {
                    break;
                }

                outPoints.Add(next);
                
                ex = m;
                m = next;



            }




        }

        public override string ToString()
        {
            return "Convex Hull - Jarvis March";
        }
    }
}
