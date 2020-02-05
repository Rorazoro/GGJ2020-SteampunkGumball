using System.Collections.Generic;
using System.Collections;

public static class AStar
{
	public abstract class NodeBase
	{
		//Abstract Methods
		public abstract IEnumerable<NodeBase> getNodes();

		//Methods
		public void resetNavigation()
		{
			minDistance = double.MaxValue;
		}

		//Meta-data
		public double travelDistance = 1;
		public double minDistance = 0;
	}
	public abstract class Node: NodeBase
	{
		List<NodeBase> nodes = new List<NodeBase>();
		object userObj = null;

		public override IEnumerable<NodeBase> getNodes()
		{
			return nodes;
		}
	}

	public class NodeGrid
	{
		public class Node: NodeBase
		{
			public NodeGrid grid;
			public UnityEngine.Vector2Int pos;
			public object userObj = null;

			public override IEnumerable<NodeBase> getNodes()
			{
				return NodeIter();
			}
			public IEnumerable<Node> NodeIter()
			{
				yield return grid.find(pos + new UnityEngine.Vector2Int(1, 0));
				yield return grid.find(pos + new UnityEngine.Vector2Int(-1, 0));
				yield return grid.find(pos + new UnityEngine.Vector2Int(0, 1));
				yield return grid.find(pos + new UnityEngine.Vector2Int(0, -1));
			}
		}

		int width;
		int height;
		public Node[] data;
		public void init(int width, int height)
		{
			this.width = width;
			this.height = height;

			data = new Node[width * height];
			for(int x=0; x<width; x++)
			{
				for(int y = 0; y < height; y++)
				{
					set(new UnityEngine.Vector2Int(x, y), new Node());
				}
			}
		}
		public void set(UnityEngine.Vector2Int pos, Node node)
		{
			node.grid = this;
			node.pos = pos;
			data[pos.x + pos.y * width] = node;
		}
		public Node get(UnityEngine.Vector2Int pos)
		{
			return data[pos.x + pos.y * width];
		}
		public Node find(UnityEngine.Vector2Int pos)
		{
			if(pos.x < 0 || pos.x >= width || pos.y < 0 || pos.y >= height)
				return null;
			return data[pos.x + pos.y * width];
		}

		public void setObj(UnityEngine.Vector2Int pos, object userObj)
		{
			get(pos).userObj = userObj;
		}
		public object getObj(UnityEngine.Vector2Int pos)
		{
			return get(pos).userObj;
		}
	}

	//Resets the node's metadata
	public static void ResetNodes(IEnumerable<NodeBase> nodes)
	{
		foreach(var node in nodes)
			node.resetNavigation();
	}

	//Make sure all nodes are reset before calling any distance calculation method
	public static double CalcDistanceBetweenToNodes(NodeBase nodeStart, NodeBase nodeEnd, ValidateConnection validate = null)
	{
		List<NodeBase> queue = new List<NodeBase>();
		nodeStart.minDistance = 0;
		queue.Add(nodeStart);
		int iterCount = 0;
		while(queue.Count > 0)
		{
			iterCount += 1;

			//Get node
			var node = queue[0];
			if(node == nodeEnd)
				return node.minDistance;

			//Check children
			var children = node.getNodes();
			foreach(var child in children)
			{
				//Validate
				if(validate != null && !validate(node, child))
					continue;

				//Check if we should branch down that path
				if(node.minDistance + child.travelDistance < child.minDistance)
				{
					//Add to queue
					child.minDistance = node.minDistance + child.travelDistance;
					queue.Add(child);
				}
			}

			//Remove this node
			queue.RemoveAt(0);
		}

		//Return can not find
		return -1;
	}
	public static void CalcAllDistancesFromNode(NodeBase node)
	{
		CalcDistanceBetweenToNodes(node, null);
	}

	public delegate bool EndCallback(NodeBase node);
	public delegate bool ValidateConnection(NodeBase from, NodeBase to);

	public static double CalcMinDistanceToAny(NodeBase nodeStart, EndCallback callback)
	{
		List<NodeBase> queue = new List<NodeBase>();
		double minDistance = double.MaxValue;
		nodeStart.minDistance = 0;
		queue.Add(nodeStart);
		int iterCount = 0;
		while(queue.Count > 0)
		{
			iterCount += 1;

			//Get node
			var node = queue[0];
			if(callback(node))
			{
				//Check min distance
				if(node.minDistance < minDistance)
					minDistance = node.minDistance;

				//Don't proccess this node
				queue.RemoveAt(0);
				continue;
			}

			//Check children
			var children = node.getNodes();
			foreach(var child in children)
			{
				//Check if we should branch down that path
				if(node.minDistance + child.travelDistance < child.minDistance)
				{
					//Add to queue
					child.minDistance = node.minDistance + child.travelDistance;
					queue.Add(child);
				}
			}

			//Remove this node
			queue.RemoveAt(0);
		}

		//Return min distance
		return minDistance;
	}

	
	public static List<NodeBase> CalcPathBetweenNodes(NodeBase nodeStart, NodeBase nodeEnd, ValidateConnection validate=null)
	{
		//Calc distance map
		CalcDistanceBetweenToNodes(nodeStart, nodeEnd, validate);

		//Reverse walk the path
		var path = new List<NodeBase>();
		path.Add(nodeEnd);
		var nodeIter = nodeEnd;
		while(true)
		{
			//Check if complete
			if(nodeIter == nodeStart)
				break;

			//Move down path
			NodeBase minChild = null;
			foreach(var child in nodeIter.getNodes())
			{
				//Validate
				if(validate != null && !validate(child, nodeIter))
					continue;
				if(child.minDistance >= double.MaxValue)
					continue;

				//Check for min
				if(minChild == null || child.minDistance < minChild.minDistance)
					minChild = child;
			}

			//Error
			if(minChild == null)
				return null;

			//Continue
			nodeIter = minChild;
			path.Add(nodeIter);
		}

		//Reverse to correct order and return
		path.Reverse();
		return path;
	}
}


