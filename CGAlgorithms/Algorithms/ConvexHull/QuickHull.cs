using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class QuickHull : Algorithm
    {
        public List<Point> CH = new List<Point>();


        public double getH(Line line, Point point)
        {
            double x = (line.Start.X - line.End.X) * (line.Start.X - line.End.X);
            double y = (line.Start.Y - line.End.Y) * (line.Start.Y - line.End.Y);

            double Base = Math.Sqrt(x + y);

            Point a = new Point(point.X - line.Start.X, point.Y - line.Start.Y);
            Point b = new Point(point.X - line.End.X, point.Y - line.End.Y);

            double cross = HelperMethods.CrossProduct(a, b);

            double h = 4 * cross / Base;
            return h;

        }


        public void Quickhull(List<Point> points, Line line)
        {
            //for each point get h
            //then get max h
            //cross(a,b) = 1/4 b*h
            //get h
            if (points.Count <= 0)
            {
                return;
            }

            double maxh = 0;
            Point maxP = new Point(0, 0);
            for (int i = 0; i < points.Count; i++)
            {
                double h = getH(line, points[i]);
                if (Math.Abs(h) > maxh)
                {
                    maxh = h;
                    maxP = points[i];
                }
            }
            //add the point to convex hull
            CH.Add(maxP);

            List<Point> leftPoints = new List<Point>();
            List<Point> rightPoints = new List<Point>();


            Line leftLine = new Line(line.Start, maxP);
            Line rightLine = new Line(maxP, line.End);

            for (int i = 0; i < points.Count; i++)
            {
                Enums.TurnType turnType = HelperMethods.CheckTurn(leftLine, points[i]);
                Enums.PointInPolygon pointInPolygon = HelperMethods.PointInTriangle(points[i], maxP, line.Start, line.End);

                if (turnType == Enums.TurnType.Left) leftPoints.Add(points[i]);
                else if (pointInPolygon == Enums.PointInPolygon.Outside) rightPoints.Add(points[i]);

                // turnType = HelperMethods.CheckTurn(rightLine, points[i]);
                // if (turnType == Enums.TurnType.Right) rightPoints.Add(points[i]);


            }


            Quickhull(leftPoints, leftLine);
            Quickhull(rightPoints, rightLine);


        }

        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {


            if (points.Count <= 3)
            {
                outPoints = points;
                return;
            }

            //get extreames(minX, maxX)
            Point minXP = new Point(0, 0), maxXP = new Point(0, 0);
            double minx = 100000000;
            double maxx = -100000000;
            for (int i = 0; i < points.Count; i++)
            {
                if (points[i].X < minx)
                {
                    minx = points[i].X;
                    minXP = points[i];
                }
                if (points[i].X > maxx)
                {
                    maxx = points[i].X;
                    maxXP = points[i];
                }
            }
            //add two points to quickhull
            CH.Add(minXP);
            CH.Add(maxXP);

            //make a line btw them
            Line lineL = new Line(minXP, maxXP);
            Line lineR = new Line(maxXP, minXP);

            //split data
            List<Point> left = new List<Point>();
            List<Point> right = new List<Point>();

            for (int i = 0; i < points.Count; i++)
            {
                Enums.TurnType turnType = HelperMethods.CheckTurn(lineL, points[i]);
                if (turnType == Enums.TurnType.Left) left.Add(points[i]);
                else if (turnType == Enums.TurnType.Right) right.Add(points[i]);

            }


            Quickhull(left, lineL);
            Quickhull(right, lineR);

            outPoints = CH;


        }

        public override string ToString()
        {
            return "Convex Hull - Quick Hull";
        }
    }
}
