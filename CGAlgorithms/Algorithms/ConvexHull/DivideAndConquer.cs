using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class DivideAndConquer : Algorithm
    {
        List<Line> lines;
        List<Polygon> polygons;
        List<Point> outPoints;
        List<Line> outLines;
        List<Polygon> outPolygons;

        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {

            // Sort X then Y
            this.lines = lines;
            this.polygons = polygons;
            this.outPolygons = outPolygons;
            this.outLines = outLines;
            this.outPoints = outPoints;

            points.OrderBy(p => p.X).ThenBy(p => p.Y);
            outPoints = DC(points);
            //outPoints.Clear();
            /* foreach(Point p in outPnts)
             {
                 outPoints.Add(p);
             }*/
            var x = 0;
         
        }

        public List<Point> Merge(List<Point> left, List<Point> right)
        {
            //make the one
    
            Point p = left[0];
            Point q = right[0];

            int pInd = 0, pCnt = 0;
            int qInd = 0, qCnt = 0;
            foreach(Point pnt in left)
            {
                if (pnt.X == p.X && pnt.Y > p.Y)
                { p = pnt; pInd = pCnt; }
                if (pnt.X > p.X)
                { p = pnt; pInd = pCnt; }
                pCnt++;
            }

            foreach (Point pnt in right)
            {
                if (pnt.X == p.X && pnt.Y < p.Y) { q = pnt; qInd = qCnt; }
                if (pnt.X < p.X) { q = pnt; qInd = qCnt; }
                qCnt++;
            }

            int ULP = pInd; int URP = qInd; //Indices of max Left and min Right
            int NextLP = (ULP + 1) % left.Count;
            int PrevRP = (URP - 1) ;
            if (PrevRP < 0) PrevRP = right.Count + PrevRP;

            int ULPBefore = ULP;
            int URPBefore = URP;
            do
            {
                ULPBefore = ULP;
                URPBefore = URP;
                while (HelperMethods.CheckTurn(new Line(right[URP], left[ULP]), left[NextLP]) == Enums.TurnType.Right)
                {
                    ULP = NextLP;
                    NextLP = (ULP + 1) % left.Count;
                }
                while (HelperMethods.CheckTurn(new Line(left[ULP], right[URP]), right[PrevRP]) == Enums.TurnType.Left)
                {
                    URP = PrevRP;
                    PrevRP = (URP - 1);
                    if (PrevRP < 0) PrevRP = right.Count + PrevRP;
                }
            }
            while (ULPBefore != ULP || URPBefore != URP);


            int DLP = pInd; int DRP = qInd; //Indices of max Left and min Right
            int PrevLP = DLP - 1;
            if (PrevLP < 0) PrevLP = left.Count + PrevLP;
            int NextRP = (DRP + 1) % right.Count;

            int DRPBefore = DRP;
            int DLPBefore = DLP;

            do
            {
                DRPBefore = DRP;
                DLPBefore = DLP;
                while (HelperMethods.CheckTurn(new Line(right[DRP], left[DLP]), left[PrevLP]) == Enums.TurnType.Left)
                {
                    DLP = PrevLP;
                    PrevLP = DLP - 1;
                    if (PrevLP < 0) PrevLP = left.Count + PrevLP;
                }
                while (HelperMethods.CheckTurn(new Line(left[DLP], right[DRP]), right[NextRP]) == Enums.TurnType.Right)
                {
                    DRP = NextRP;
                    NextRP = (DRP + 1) % right.Count ;
                   
                }
            }
            while (DRPBefore != DRP || DLPBefore != DLP);

            //Interest 
            List<Point> Merged = new List<Point>();
            if (DLP <= ULP)
            {
                for (int i = DLP; i <= ULP; i++)  Merged.Add(left[i]); 
            }
            else if (DLP >= ULP)
            {
                for (int i = DLP; i <= left.Count+ULP; i++) Merged.Add(left[i%left.Count]);
            }
            if (URP <= DRP)
            {
                for (int i = URP; i <= DRP; i++) Merged.Add(right[i]);
            }
            else if (URP >= DRP)
            {
                for (int i = URP; i <= right.Count+DRP; i++) Merged.Add(right[i%right.Count]);
            }
            //Interest
            return Merged;
        }

        public List<Point> DC(List<Point> points)

        {
            if (points.Count < 6)
            {
                GrahamScan grahamScan = new GrahamScan();
                //List<Point> p = new List<Point>();
                List<Point> outPnts = new List<Point>();
                grahamScan.Run(points, lines, polygons, ref outPnts, ref outLines, ref outPolygons);
                //outPoints.Reverse();

                //Interest 
                /*List<Point> outPnts = new List<Point>();
                foreach(Point o in outPoints)
                {
                    foreach (Point p in points)
                    {
                        if (o == p && o != null && p != null)
                       outPnts.Add(p);
                        
                    }
                }
                outPoints.Clear();*/
                return outPnts;
                //Interest 
            }
            else
            {
                int middle = (points.Count/ 2)-1;

                List<Point> leftHalf = new List<Point>();
                List<Point> rightHalf = new List<Point>();

                for (int i=0;i<=middle;i++)
                {
                    leftHalf.Add(points[i]);
                }
                for(int i = middle+1; i<points.Count; i++)
                {
                    rightHalf.Add(points[i]);
                }
                List<Point>left =  DC(leftHalf);
                List<Point> right = DC(rightHalf);

                return Merge(left, right);// merge;
            }
        }

        public override string ToString()
        {
            return "Convex Hull - Divide & Conquer";
        }

    }
}
