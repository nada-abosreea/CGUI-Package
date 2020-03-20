using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CGUtilities;
namespace CGAlgorithms.Algorithms
{
    class orien:Algorithm
    {
        public override void Run(
            List<Point> points, List<Line> lines, List<Polygon> polygons,
            ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygon)
        {

            Enums.TurnType e  =  HelperMethods.CheckTurn(lines[0], points[0]);
            if (e == Enums.TurnType.Left)
            {
                outPoints.Add(points[0]);
                
            }
            else
            {
                outLines.Add(lines[0]);

            }





        }

        /// <summary>
        /// Retuens the name of the algorithm (used in UI).
        /// </summary>
        /// <returns>The algorithm name.</returns>
        public override string ToString()
        {
            return "orientation test";

        }


    }
}
