using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Game.Data
{
	public struct IffTag
	{
		public static IffTag Bytes(string value)
		{
			Debug.Assert(value.Length == 4);
			return new IffTag(
				(((UInt32)value[0]) << 24) | (((UInt32)value[1]) << 16) | (((UInt32)value[2]) << 8) | (((UInt32)value[3]) << 0)
			);
		}
		public static implicit operator IffTag(string text)
		{
			return new IffTag(text);
		}
		public IffTag(UInt32 tag = 0) { value = tag; }
		public IffTag(string value)
		{
			this.value = GenerateTagHash(value);
		}

		static UInt32 GenerateTagHash(string tag)
		{
			UInt32 hash = 0;

			int size = tag.Length;
			for(int i = 0; i < size; i++)
				hash = 37 * hash + tag[i];

			return hash; // or, h % ARRAY_SIZE;
		}
		public static UInt32 TagByteSwap(UInt32 WORD)
		{
			return (((WORD >> 24) & 0x000000FF) | ((WORD >> 8) & 0x0000FF00) | ((WORD << 8) & 0x00FF0000) | ((WORD << 24) & 0xFF000000));
		}

		public UInt32 value;
	}

	public class IffWriter
	{
		Stream stream;
		BinaryWriter writer;
		List<Chunk> chunkStack = new List<Chunk>();

		class Chunk
		{
			//Data
			public UInt32 tag;
			public ChunkType type;

			//Meta Data
			public long posBegin;
			public long posData;
			public long posEnd;
		};

		public enum ChunkType : byte
		{
			DEFAULT,
			BINARY,
			UINT8,
			UINT16,
			UINT32,
			UINT64,
			INT8,
			INT16,
			INT32,
			INT64,
			FLOAT32,
			FLOAT64,
			UTF8,
		};

		public IffWriter(Stream stream)
		{
			this.stream = stream;
			this.writer = new BinaryWriter(stream);
		}
		~IffWriter()
		{
		}

		public Stream getStream()
		{
			return stream;
		}
		public BinaryWriter getWriter()
		{
			return writer;
		}
		public void beginChunk(IffTag tag, ChunkType type = IffWriter.ChunkType.DEFAULT)
		{
			//Check if this makes sense
			if(chunkStack.Count != 0 && chunkStack[chunkStack.Count - 1].type != ChunkType.DEFAULT)
			{
				Debug.Assert(false);
				return;
			}

			//Push chunk onto stack
			Chunk chunk = new Chunk();
			chunk.tag = tag.value;
			chunk.type = type;
			chunk.posBegin = stream.Position;
			chunk.posData = chunk.posBegin + 9;
			chunkStack.Add(chunk);

			//Write
			stream.Seek(9, SeekOrigin.Current);
		}
		public void endChunk()
		{
			//Pop chunk off stack
			int stackSize = chunkStack.Count;
			Debug.Assert(stackSize != 0);
			Chunk chunk = chunkStack[stackSize - 1];
			chunkStack.RemoveAt(stackSize - 1);

			long prevPos = stream.Position;

			//Move to chunk start
			long delta = prevPos - chunk.posBegin;
			stream.Seek(-delta, SeekOrigin.Current);

			//Write tag and size
			writer.Write(IffTag.TagByteSwap(chunk.tag));
			writer.Write((byte)chunk.type);
			writer.Write((UInt32)(prevPos - chunk.posData));

			//Move back to position
			stream.Seek(prevPos, SeekOrigin.Begin);
		}

		public void writeBool(IffTag tag, bool value)
		{
			writer.Write(IffTag.TagByteSwap(tag.value));
			writer.Write((byte)ChunkType.UINT8);
			writer.Write(Convert.ToByte(value));
		}
		public void writeUInt8(IffTag tag, byte value)
		{
			writer.Write(IffTag.TagByteSwap(tag.value));
			writer.Write((byte)ChunkType.UINT8);
			writer.Write(value);
		}
		public void writeUInt16(IffTag tag, UInt16 value)
		{
			writer.Write(IffTag.TagByteSwap(tag.value));
			writer.Write((byte)ChunkType.UINT16);
			writer.Write(value);
		}
		public void writeUInt32(IffTag tag, UInt32 value)
		{
			writer.Write(IffTag.TagByteSwap(tag.value));
			writer.Write((byte)ChunkType.UINT32);
			writer.Write(value);
		}
		public void writeUInt64(IffTag tag, UInt64 value)
		{
			writer.Write(IffTag.TagByteSwap(tag.value));
			writer.Write((byte)ChunkType.UINT64);
			writer.Write(value);
		}
		public void writeInt8(IffTag tag, byte value)
		{
			writer.Write(IffTag.TagByteSwap(tag.value));
			writer.Write((byte)ChunkType.INT8);
			writer.Write(value);
		}
		public void writeInt16(IffTag tag, Int16 value)
		{
			writer.Write(IffTag.TagByteSwap(tag.value));
			writer.Write((byte)ChunkType.INT16);
			writer.Write(value);
		}
		public void writeInt32(IffTag tag, Int32 value)
		{
			writer.Write(IffTag.TagByteSwap(tag.value));
			writer.Write((byte)ChunkType.INT32);
			writer.Write(value);
		}
		public void writeInt64(IffTag tag, Int64 value)
		{
			writer.Write(IffTag.TagByteSwap(tag.value));
			writer.Write((byte)ChunkType.INT64);
			writer.Write(value);
		}
		public void writeFloat32(IffTag tag, Single value)
		{
			writer.Write(IffTag.TagByteSwap(tag.value));
			writer.Write((byte)ChunkType.FLOAT32);
			writer.Write(value);
		}
		public void writeFloat64(IffTag tag, Double value)
		{
			writer.Write(IffTag.TagByteSwap(tag.value));
			writer.Write((byte)ChunkType.FLOAT64);
			writer.Write(value);
		}
		public void writeString(IffTag tag, string value)
		{
			if(!String.IsNullOrEmpty(value))
			{
				UInt32 length = (UInt32)value.Length;
				writer.Write(IffTag.TagByteSwap(tag.value));
				writer.Write((byte)ChunkType.UTF8);
				writer.Write(length);
				writer.Write(value.ToCharArray());
			}
			else
			{
				writer.Write(IffTag.TagByteSwap(tag.value));
				writer.Write((byte)ChunkType.UTF8);
				writer.Write((UInt32)0);
			}
		}
		public void writeBytes(IffTag tag, byte[] data, UInt32 length)
		{
			writer.Write(IffTag.TagByteSwap(tag.value));
			writer.Write((byte)ChunkType.BINARY);
			writer.Write(length);
			stream.Write(data, 0, (int)length);
		}
	}
}
