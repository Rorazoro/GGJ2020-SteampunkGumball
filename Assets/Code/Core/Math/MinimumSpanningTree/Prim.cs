using System.Collections.Generic;

namespace PrimAlgorithm
{
	class Prim<T>
	{
		static int kInfinite = int.MaxValue - 1;

		public class Edge
		{
			public Edge(T objA, T objB)
			{
				this.objA = objA;
				this.objB = objB;
			}

			public T objA;
			public T objB;
		}

		public static List<Edge> GenerateEdges(Graph<T> g, Graph<T>.Node r)
		{
			var tree = new Dictionary<Graph<T>.Node, Graph<T>.Node>();
			Graph<T> Q = new Graph<T>(g);
			var d = new Dictionary<Graph<T>.Node, int>();
			foreach(var u in Q.nodes)
			{
				d[u] = kInfinite;
			}
			d[r] = 0;

			int temp = 0;
			while(Q.n > 0)
			{
				var u = DeleteMin(Q, d);
				foreach(var v in u.GetNeighbors())
				{
					if(Q.Contains(v) && (u.GetWeight(v) < d[v]))
					{
						d[v] = u.GetWeight(v);
						tree[v] = u;
						temp++;
					}
				}
			}

			//Generate edges
			var edges = new List<Edge>(tree.Count);
			foreach(var treeEdge in tree)
				edges.Add(new Edge(treeEdge.Key.context, treeEdge.Value.context));
			return edges;
		}

		private static Graph<T>.Node DeleteMin(Graph<T> Q, Dictionary<Graph<T>.Node, int> d)
		{
			Graph<T>.Node min_node = null;
			int min_weight = kInfinite;
			foreach(var n in Q.nodes)
			{
				if(d[n] <= min_weight)
				{
					min_weight = d[n];
					min_node = n;
				}
			}
			Q.Remove(min_node);
			return min_node;
		}

	}
}