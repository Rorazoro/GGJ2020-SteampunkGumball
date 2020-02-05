using UnityEngine;
using System.Collections.Generic;

public class HexGrid<HEX_TYPE>
{
	public int width;
	public int height;
	public float worldSize;
	HEX_TYPE[] blocks;
	bool flatTop;

	public static Vector2Int InvalidPos = new Vector2Int(-1, -1);

	public void init(int width, int height, float worldSize, bool flatTop)
	{
		//Store data
		this.width = width;
		this.height = height;
		this.worldSize = worldSize;
		this.flatTop = flatTop;

		//Alloc blocks
		int size = width * this.height;
		blocks = new HEX_TYPE[size];
		for(int i=0; i<size; i++)
			blocks[i] = default(HEX_TYPE);
	}
	public HEX_TYPE find(Vector2Int origin)
	{
		return get(origin);
	}
	public HEX_TYPE get(Vector2Int origin)
	{
		//Check range
		if(origin.x < 0 || origin.y < 0 || origin.x >= width || origin.y >= height)
			return default(HEX_TYPE);

		//Return block
		return blocks[(origin.y * width) + (origin.x)];
	}
	public void set(Vector2Int origin, HEX_TYPE block)
	{
		//Check range
		if(origin.x < 0 || origin.y < 0 || origin.x >= width || origin.y >= height)
			return;

		//Set
		blocks[(origin.y * width) + (origin.x)] = block;
	}
	public bool validate(Vector2Int origin)
	{
		return !(origin.x < 0 || origin.y < 0 || origin.x >= width || origin.y >= height);
	}

	public Vector2Int getNeighborOrigin(Vector2Int origin, HexMath.Side side)
	{
		if(flatTop)
		{
			switch(side)
			{
				case HexMath.Side.NORTH:		return origin + new Vector2Int(0, 1);
				case HexMath.Side.NORTH_EAST:	return origin + new Vector2Int(1, (origin.x % 2) == 0 ? 1 : 0);
				//case HexMath.Side.EAST:			return InvalidPos;
				case HexMath.Side.SOUTH_EAST:	return origin + new Vector2Int(1, (origin.x % 2) == 0 ? 0 : -1);
				case HexMath.Side.SOUTH:		return origin + new Vector2Int(0, -1);
				case HexMath.Side.SOUTH_WEST:	return origin + new Vector2Int(-1, (origin.x % 2) == 0 ? 0 : -1);
				//case HexMath.Side.WEST:			return InvalidPos;
				case HexMath.Side.NORTH_WEST:	return origin + new Vector2Int(-1, (origin.x % 2) == 0 ? 1 : 0);
			}
		}
		else
		{
			switch(side)
			{
				//case HexMath.Side.NORTH: return InvalidPos;
				case HexMath.Side.NORTH_EAST: return origin + new Vector2Int((origin.y % 2) == 0 ? 1 : 0, 1);
				case HexMath.Side.EAST: return origin + new Vector2Int(1, 0);
				case HexMath.Side.SOUTH_EAST: return origin + new Vector2Int((origin.y % 2) == 0 ? 1 : 0, -1);
				//case HexMath.Side.SOUTH: return InvalidPos;
				case HexMath.Side.SOUTH_WEST: return origin + new Vector2Int((origin.y % 2) == 0 ? 0 : -1 , -1);
				case HexMath.Side.WEST: return origin + new Vector2Int(-1, 0);
				case HexMath.Side.NORTH_WEST: return origin + new Vector2Int((origin.y % 2) == 0 ? 0 : -1, 1);
			}
		}
		return InvalidPos;
	}
	public HEX_TYPE findNeighbor(Vector2Int origin, HexMath.Side side)
	{
		return find(getNeighborOrigin(origin, side));
	}
	public List<HEX_TYPE> findNeighbors(Vector2Int origin)
	{
		List<HEX_TYPE> neighbors = new List<HEX_TYPE>();
		void findSide(HexMath.Side side)
		{
			var neighbor = findNeighbor(origin, side);
			if(neighbor != null)
				neighbors.Add(neighbor);

		}
		findSide(HexMath.Side.NORTH);
		findSide(HexMath.Side.NORTH_EAST);
		findSide(HexMath.Side.EAST);
		findSide(HexMath.Side.SOUTH_EAST);
		findSide(HexMath.Side.SOUTH);
		findSide(HexMath.Side.SOUTH_WEST);
		findSide(HexMath.Side.WEST);
		findSide(HexMath.Side.NORTH_WEST);
		return neighbors;
	}
	public int findNeighborCount(Vector2Int origin)
	{
		int count = 0;
		void check(HexMath.Side side)
		{
			var neighbor = findNeighbor(origin, side);
			if(neighbor != null)
				count += 1;
		}
		check(HexMath.Side.NORTH);
		check(HexMath.Side.NORTH_EAST);
		check(HexMath.Side.EAST);
		check(HexMath.Side.SOUTH_EAST);
		check(HexMath.Side.SOUTH);
		check(HexMath.Side.SOUTH_WEST);
		check(HexMath.Side.WEST);
		check(HexMath.Side.NORTH_WEST);

		return count;
	}


	public Vector2 getWorldRect()
	{
		return convertGridToWorld(new Vector2(width-1, height-1));
	}

	public Vector2 convertGridToWorld(Vector2 origin)
	{
		return HexMath.Offset.ConvertGridToWorld(origin, worldSize, flatTop);
	}
	public Vector2 convertWorldToGrid(Vector2 origin)
	{
		return HexMath.Offset.ConvertWorldToGrid(origin, worldSize, flatTop);
	}
}
