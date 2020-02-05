using UnityEngine;
using System.Collections;

public static class HexMath
{
	//Conversion
	//public static float HEX_WIDTH_CONVERT_FACTOR = 0.75f;
	//public static float HEX_HEIGHT_CONVERT_FACTOR = 0.866025404f;

	public static float HEX_WIDTH_CONVERT_FACTOR = 0.75f;
	public static float HEX_HEIGHT_CONVERT_FACTOR = 1.0f;

	public enum Side
	{
		NORTH,
		NORTH_EAST,
		EAST,
		SOUTH_EAST,
		SOUTH,
		SOUTH_WEST,
		WEST,
		NORTH_WEST
	}

	public static Vector2 ConvertPointToPointyTopAxialHex(float x, float y, float size)
	{
		float q = (x * (Mathf.Sqrt(3.0f) / 3.0f) - (y / 3.0f)) / size;
		float r = (y * (2.0f / 3.0f) / size);
		return new Vector2(q, r);
	}
	public static Vector2 ConvertPointToFlatTopAxialHex(float x, float y, float size)
	{
		float q = (x * (2.0f / 3.0f) / size);
		float r = (y * (Mathf.Sqrt(3.0f) / 3.0f) - (x / 3.0f)) / size;
		return new Vector2(q, r);
	}
	public static Vector3 ConvertFlatTopAxialPointToCube(Vector2 axial)
	{
		float x = axial.x; //Q
		float z = axial.y - (axial.x + (Mathf.Abs((int)axial.x) % 2)) / 2.0f;
		float y = -x - z;
		return new Vector3(x, y, z);
	}
	public static Vector3 ConvertPointyTopAxialPointToCube(Vector2 axial)
	{
		float x = axial.x - (axial.x + (Mathf.Abs((int)axial.x) % 2)) / 2.0f;
		float z = axial.y;
		float y = -x - z;
		return new Vector3(x, y, z);
	}
	public static Vector3 RoundHexCube(Vector3 h)
	{
		float rx = Mathf.Round(h.x);
		float ry = Mathf.Round(h.y);
		float rz = Mathf.Round(h.z);

		float x_diff = Mathf.Abs(rx - h.x);
		float y_diff = Mathf.Abs(ry - h.y);
		float z_diff = Mathf.Abs(rz - h.z);

		if((x_diff > y_diff) && (x_diff > z_diff))
			rx = -ry - rz;
		else if(y_diff > z_diff)
			ry = -rx - rz;
		else
			rz = -rx - ry;

		return new Vector3(rx, ry, rz);
	}
	public static Vector2 ConvertCubeToGridEvenPointyTop(Vector3 h)
	{
		int col = (int)h.x + ((int)h.z + (Mathf.Abs((int)h.z) % 2)) / 2;
		int row = (int)h.z;
		return new Vector2(col, row);
	}
	public static Vector2 ConvertCubeToGridEvenFlatTop(Vector3 h)
	{
		int col = (int)h.x;
		int row = (int)h.z + ((int)h.z + (Mathf.Abs((int)h.z) % 2)) / 2;
		return new Vector2(col, row);
	}

	public static class Offset
	{
		public static Vector2 ConvertGridToWorld(Vector2 origin, float width, bool flatTop)
		{
			//Flip
			if(!flatTop)
			{
				origin = new Vector2(origin.y, origin.x);
			}

			//Algorithm
			if((int)origin.x % 2 == 1)
				origin.y -= 0.5f;
			var result = new Vector2(origin.x * HEX_WIDTH_CONVERT_FACTOR * width, origin.y * HEX_HEIGHT_CONVERT_FACTOR * width);

			//Flip
			if(!flatTop)
			{
				result = new Vector2(result.y, result.x);
			}

			//Return
			return result;
		}
		public static Vector2 ConvertWorldToGrid(Vector2 origin, float width, bool flatTop)
		{
			//Flip
			if(!flatTop)
			{
				origin = new Vector2(origin.y, origin.x);
			}

			float HexXSpacing = width * HEX_WIDTH_CONVERT_FACTOR;
			float HexYSpacing = width * HEX_HEIGHT_CONVERT_FACTOR;

			// NOTE:  HexCoord(0,0)'s x() and y() just define the origin
			//        for the coordinate system; replace with your own
			//        constants.  (HexCoord(0,0) is the origin in the hex
			//        coordinate system, but it may be offset in the x/y
			//        system; that's why I subtract.)
			float x = 1.0f * (origin.x) / HexXSpacing;
			float y = 1.0f * (origin.y) / HexYSpacing;
			float z = -0.5f * x - y;
			y = -0.5f * x + y;
			int ix = (int)Mathf.Floor(x + 0.5f);
			int iy = (int)Mathf.Floor(y + 0.5f);
			int iz = (int)Mathf.Floor(z + 0.5f);
			int s = ix + iy + iz;
			if(s != 0)
			{
				double abs_dx = Mathf.Abs(ix - x);
				double abs_dy = Mathf.Abs(iy - y);
				double abs_dz = Mathf.Abs(iz - z);
				if(abs_dx >= abs_dy && abs_dx >= abs_dz)
					ix -= s;
				else if(abs_dy >= abs_dx && abs_dy >= abs_dz)
					iy -= s;
				else
					iz -= s;
			}

			Vector2 result = new Vector2(ix, (iy -iz + (1 - ix % 2)) / 2);
			/*
			The above produces a proper Hex to World shift. EXCEPT the y's are not shifted to the system we are using.
			It produces
				 (1,0) 
			(0,0) 

			Rather than
				 (1,1)
			(0,0)

			Thus, if the x value is odd, we need to add one to our hex

			*/
			if((int)result.x % 2 == 1)
				result.y += 1.0f;

			//Flip
			if(!flatTop)
			{
				result = new Vector2(result.y, result.x);
			}

			//Return
			return result;
		}
	}

	public static class Axial
	{
		public static Vector2 ConvertGridToWorld(Vector2 origin, float width, Orientation orientation)
		{
			Vector2 size = new Vector2(width, width);
			double x = (orientation.f0 * origin.x + orientation.f1 * origin.y) * size.x;
			double y = (orientation.f2 * origin.x + orientation.f3 * origin.y) * size.y;
			return new Vector2((float)x, (float)y);
		}
		public static Vector2 ConvertWorldToGrid(Vector2 origin, float width, Orientation orientation)
		{
			Vector2 size = new Vector2(width, width);
			Vector2 pt = new Vector2((origin.x) / size.x, (origin.y) / size.y);
			double q = orientation.b0 * pt.x + orientation.b1 * pt.y;
			double r = orientation.b2 * pt.x + orientation.b3 * pt.y;
			Vector3 result = RoundHexCube(new Vector3((float)q, (float)r, (float)(-q - r)));
			return result;
		}
	}

	public struct Orientation
	{
		public double f0;
		public double f1;
		public double f2;
		public double f3;
		public double b0;
		public double b1;
		public double b2;
		public double b3;
		public double start_angle;
		public Orientation(double f0, double f1, double f2, double f3, double b0, double b1, double b2, double b3, double start_angle)
		{
			this.f0 = f0;
			this.f1 = f1;
			this.f2 = f2;
			this.f3 = f3;
			this.b0 = b0;
			this.b1 = b1;
			this.b2 = b2;
			this.b3 = b3;
			this.start_angle = start_angle;
		}
	};
	public static Orientation layout_pointy = new Orientation(System.Math.Sqrt(3.0), System.Math.Sqrt(3.0) / 2.0, 0.0, 3.0 / 2.0, System.Math.Sqrt(3.0) / 3.0, -1.0 / 3.0, 0.0, 2.0 / 3.0, 0.5);
	public static Orientation layout_flat = new Orientation(3.0 / 2.0, 0.0, System.Math.Sqrt(3.0) / 2.0, System.Math.Sqrt(3.0), 2.0 / 3.0, 0.0, -1.0 / 3.0, System.Math.Sqrt(3.0) / 3.0, 0.0);
}