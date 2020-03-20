using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class GrahamScan : Algorithm
    {
        public List<Point> sortPoints(List<double> arr, List<Point> points)
        {
            for (int i = 1; i < arr.Count; ++i)
            {
                double key = arr[i];
                Point p1 = points[i];
                int j = i - 1;
                while (j >= 0 && arr[j] > key)
                {
                    arr[j + 1] = arr[j];
                    points[j + 1] = points[j];
                    j = j - 1;
                }
                arr[j + 1] = key;
                points[j + 1] = p1;
            }
            return points;
        }
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            if (points.Count == 1)
                outPoints.Add(points[0]);
            else if (points.Count == 2 && points[0] != points[1])
            {
                outPoints.Add(points[0]);
                outPoints.Add(points[1]);
            }
            else
            {
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
                List<double> angList = new List<double>();
                Point m = new Point(minX, minY);
                Point ex = new Point(minX - 20, minY);
                Point start = m;
                Point ab = new Point(m.X - ex.X, m.Y - ex.Y);

                for (int i = 0; i < points.Count; i++)
                {
                    Point ac = new Point(points[i].X - m.X, points[i].Y - m.Y);

                    double cross = HelperMethods.CrossProduct(ab, ac);
                    double dot = (ab.X * ac.X) + (ab.Y * ac.Y);

                    double angle = Math.Atan2(cross, dot);
                    if (angle < 0)
                        angle = angle + (2 * Math.PI);
                    angList.Add(angle);
                }
                List<Point> points2 = sortPoints(angList, points);

                Stack<Point> stack = new Stack<Point>();
                stack.Push(points2[0]);
                stack.Push(points2[1]);
                stack.Push(points2[2]);
                for (int i = 3; i < points2.Count; i++)
                {
                    Point pTop = stack.Peek();
                    stack.Pop();
                    Point pRes = stack.Peek();
                    stack.Push(pTop);
                    while (CGUtilities.HelperMethods.CheckTurn(new Line(pRes, pTop), points2[i]) != CGUtilities.Enums.TurnType.Left)
                    {
                        stack.Pop();
                        pTop = stack.Peek();
                        stack.Pop();
                        pRes = stack.Peek();
                        stack.Push(pTop);
                    }
                    stack.Push(points2[i]);
                }

                for (int i = 0; i < stack.Count; i++)
                {
                    outPoints.Add(stack.Pop());
                    i--;
                }
            }
        }

        public override string ToString()
        {
            return "Convex Hull - Graham Scan";
        }
    }
}
