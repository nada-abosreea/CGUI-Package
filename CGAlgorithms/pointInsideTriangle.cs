using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CGUtilities;


namespace CGAlgorithms.Algorithms
{
    class pointInsideTriangle : Algorithm
    {
        public override void Run(
            List<Point> points, List<Line> lines, List<Polygon> polygons,
            ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygon)
        {

            int left = 0, right = 0;
            for(int i=0;  i<polygons[0].lines.Count; i++)
            {
                Enums.TurnType e = HelperMethods.CheckTurn(polygons[0].lines[i], points[0]);
                if (e == Enums.TurnType.Left)
                    left++;
                else if (e == Enums.TurnType.Right)
                    right++;



            }
            if (left==3 || right==3)
            {
                outLines.Add(polygons[0].lines[0]);
                outLines.Add(polygons[0].lines[1]);
                outLines.Add(polygons[0].lines[2]);
            }
            else
            {
                outPoints.Add(points[0]);
            }


           




        }

        /// <summary>
        /// Retuens the name of the algorithm (used in UI).
        /// </summary>
        /// <returns>The algorithm name.</returns>
        public override string ToString()
        {
            return "point inside triangle";

        }


    }
}