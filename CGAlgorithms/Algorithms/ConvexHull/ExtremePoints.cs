using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CGUtilities;
namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class ExtremePoints : Algorithm
    {
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {



            List<Point> points2 = new List<Point>();
            
            foreach(var item in points)
            {
                if (!points2.Contains(item))
                    points2.Add(item);
            }
            points = points2;


            if (points.Count == 1 || points.Count==2)
            {
                outPoints = points;
                return;
            }



            for (int i =0; i<points.Count; i++)
            {

                bool found = false;
                for (int j=0; j<points.Count; j++)
                {
                    for (int k = 0; k < points.Count; k++)
                    {
                        for (int z = 0; z < points.Count; z++)
                        {

                            if (points[i] != points[j] && points[i]!=points[k] && points[i] != points[z])
                            {
                                Enums.PointInPolygon e = HelperMethods.PointInTriangle(points[i], points[j], points[k], points[z]);
                                if (e == Enums.PointInPolygon.Inside || e == Enums.PointInPolygon.OnEdge)
                                {
                                    points.Remove(points[i]);
                                    i--;
                                    found = true;
                                    break;

                                }

                            }
                            if (found)
                                break;

                        }
                        if (found)
                            break;

                    }
                    if (found)
                        break;

                }


            }

            outPoints = points;


        }

        public override string ToString()
        {
            return "Convex Hull - Extreme Points";
        }
    }
}
