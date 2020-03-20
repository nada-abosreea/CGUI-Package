using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CGUtilities;

namespace CGAlgorithms.Algorithms
{
    class segmentIntersectSegment : Algorithm
    {
        public override void Run(
            List<Point> points, List<Line> lines, List<Polygon> polygons,
            ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygon)
        {

            bool x = false, y = false;
            Enums.TurnType e1 = HelperMethods.CheckTurn(lines[0], lines[1].Start);
            Enums.TurnType e2 = HelperMethods.CheckTurn(lines[0], lines[1].End);
            if ((e1 == Enums.TurnType.Left && e2 == Enums.TurnType.Left) || (e1 == Enums.TurnType.Right && e2 == Enums.TurnType.Right))
            {
                x = true;
            }

            e1 = HelperMethods.CheckTurn(lines[1], lines[0].Start);
            e2 = HelperMethods.CheckTurn(lines[1], lines[0].End);
            if ((e1 == Enums.TurnType.Left && e2 == Enums.TurnType.Left) || (e1 == Enums.TurnType.Right && e2 == Enums.TurnType.Right))
            {
                y = true;
            }

            if (!x && !y)
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
            return "segment intersect segment";

        }


    }
}
