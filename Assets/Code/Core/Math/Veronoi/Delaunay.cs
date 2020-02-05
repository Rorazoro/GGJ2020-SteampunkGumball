using System;
using System.Collections.Generic;
using System.Linq;

namespace DelaunayVoronoi
{
    public class Delaunay
    {
		public static IEnumerable<Point> GeneratePoints(int amount, double minX, double minY, double maxX, double maxY)
		{
			var points = new List<Point>(amount);
			var random = new Random();
			for(int i=0; i<amount; i++)
			{
				double x = minX + random.NextDouble() * (maxX - minX);
				double y = minY + random.NextDouble() * (maxY - minY);
				points.Add(new Point(x, y));
			}
			return points;
		}
		public static IEnumerable<Triangle> GenerateTriangles(IEnumerable<Point> points) //BowyerWatson
		{
			//Find min/max area
			double minX = double.MaxValue;
			double minY = double.MaxValue;
			double maxX = double.MinValue;
			double maxY = double.MinValue;
			foreach(var point in points)
			{
				if(point.X < minX) minX = point.X;
				if(point.Y < minY) minY = point.Y;
				if(point.X > maxX) maxX = point.X;
				if(point.Y > maxY) maxY = point.Y;
			}
			minX -= 1.0;
			minY -= 1.0;
			maxX += 1.0;
			maxY += 1.0;

			//Create border
			var point0 = new Point(minX, minY);
			var point1 = new Point(minX, maxY);
			var point2 = new Point(maxX, maxY);
			var point3 = new Point(maxX, minY);
			//var borderPoints = new List<Point>() { point0, point1, point2, point3 };
			var tri1 = new Triangle(point0, point1, point2);
			var tri2 = new Triangle(point0, point2, point3);
			var borderTris = new List<Triangle>() { tri1, tri2 };

			//Perform BowyerWatson
			var triangulation = new HashSet<Triangle>(borderTris);
            foreach (var point in points)
            {
                var badTriangles = FindBadTriangles(point, triangulation);
                var polygon = FindHoleBoundaries(badTriangles);

                foreach (var triangle in badTriangles)
                {
                    foreach (var vertex in triangle.Vertices)
                    {
                        vertex.AdjacentTriangles.Remove(triangle);
                    }
                }
                triangulation.RemoveWhere(o => badTriangles.Contains(o));

                foreach (var edge in polygon)
                {
                    var triangle = new Triangle(point, edge.Point1, edge.Point2);
                    triangulation.Add(triangle);
                }
            }

            //triangulation.RemoveWhere(o => o.Vertices.Any(v => supraTriangle.Vertices.Contains(v)));
            return triangulation;
        }

		public static IEnumerable<Edge> GenerateDelaunayEdges(IEnumerable<Triangle> triangles)
		{
			var edges = new HashSet<Edge>();
			foreach(var triA in triangles)
			{
				foreach(var triB in triA.TrianglesWithSharedEdge)
				{
					//Find matching points
					List<Point> points = new List<Point>();
					foreach(var point in triA.Vertices)
					{
						if(triB.Vertices.Contains(point))
							points.Add(point);
					}
					if(points.Count != 2)
						continue;

					//Make edge
					edges.Add(new Edge(points[0], points[1]));
				}
			}
			return edges;
		}

		private static IEnumerable<Edge> FindHoleBoundaries(ISet<Triangle> badTriangles)
        {
            var edges = new List<Edge>();
            foreach (var triangle in badTriangles)
            {
                edges.Add(new Edge(triangle.Vertices[0], triangle.Vertices[1]));
                edges.Add(new Edge(triangle.Vertices[1], triangle.Vertices[2]));
                edges.Add(new Edge(triangle.Vertices[2], triangle.Vertices[0]));
            }
            var grouped = edges.GroupBy(o => o);
            var boundaryEdges = edges.GroupBy(o => o).Where(o => o.Count() == 1).Select(o => o.First());
            return boundaryEdges.ToList();
        }

        /*private Triangle GenerateSupraTriangle()
        {
            //   1  -> maxX
            //  / \
            // 2---3
            // |
            // v maxY
            var margin = 500;
            var point1 = new Point(0.5 * MaxX, -2 * MaxX - margin);
            var point2 = new Point(-2 * MaxY - margin, 2 * MaxY + margin);
            var point3 = new Point(2 * MaxX + MaxY + margin, 2 * MaxY + margin);
            return new Triangle(point1, point2, point3);
        }*/

        private static ISet<Triangle> FindBadTriangles(Point point, HashSet<Triangle> triangles)
        {
            var badTriangles = triangles.Where(o => o.IsPointInsideCircumcircle(point));
            return new HashSet<Triangle>(badTriangles);
        }
    }
}