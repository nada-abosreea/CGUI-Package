using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CGUtilities;
namespace CGAlgorithms.Algorithms.ConvexHull
{
    class polygonIntersectPolygon:Algorithm
    {
        
        public bool LinesIntersect(Line l1, Line l2)
        {

            bool x = false, y = false;
            Enums.TurnType e1 = HelperMethods.CheckTurn(l1, l2.Start);
            Enums.TurnType e2 = HelperMethods.CheckTurn(l1, l2.End);
            if ((e1 == Enums.TurnType.Left && e2 == Enums.TurnType.Left) || (e1 == Enums.TurnType.Right && e2 == Enums.TurnType.Right))
            {
                x = true;
            }

            e1 = HelperMethods.CheckTurn(l2, l1.Start);
            e2 = HelperMethods.CheckTurn(l2, l1.End);
            if ((e1 == Enums.TurnType.Left && e2 == Enums.TurnType.Left) || (e1 == Enums.TurnType.Right && e2 == Enums.TurnType.Right))
            {
                y = true;
            }

            if (x || y)
            {
                return false;
            }
            else
            {
                return true;
            }




        }
        //if they intersect one of them will turn red
        //else nothing happens
        public override void Run(
            List<Point> points, List<Line> lines, List<Polygon> polygons,
            ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons
            )
        {

            for (int i = 0; i < polygons[0].lines.Count; i++)
            {
                for (int j = 0; j < polygons[1].lines.Count; j++)
                {
                    if (LinesIntersect(polygons[1].lines[j], polygons[0].lines[i]))
                    {
                        outLines.Add(polygons[0].lines[0]);
                        outLines.Add(polygons[0].lines[1]);
                        outLines.Add(polygons[0].lines[2]);
                        outPolygons.Add(polygons[1]);//doesnt draw polygons
                        


                    }
                }


            }



        }

        /// <summary>
        /// Retuens the name of the algorithm (used in UI).
        /// </summary>
        /// <returns>The algorithm name.</returns>
        public override string ToString()
        {
            return "polygon Intersect Polygon";
        }



    }
}
