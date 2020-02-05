using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Game.Data
{
	public class IffReader
	{
		//Stack
		Stream stream;
		BinaryReader reader;
		List<Chunk> chunkStack = new List<Chunk>();
		Chunk currentChunk;

		public IffReader()
		{
			stream = null;
		}
		public IffReader(Stream stream)
		{
			this.stream = stream;
			this.reader = new BinaryReader(stream);
		}
		~IffReader() { }

		public enum SeekPos
		{
			BEGIN,
			CURRENT,
			END,
		};

		//Chunks
		public class Chunk
		{
			//Actual Data
			public UInt32 tag = 0;
			public IffWriter.ChunkType type = IffWriter.ChunkType.DEFAULT;
			public UInt32 size = 0;

			//Metadata
			public long beginPos = 0;
			public long dataPos = 0;
			public long endPos = 0;
		};

		//Open
		public void open(Stream stream)
		{
			this.stream = stream;
			this.reader = new BinaryReader(stream);
		}
		public Stream getStream()
		{
			return stream;
		}
		public BinaryReader getReader()
		{
			return reader;
		}

		//Standard Chunks
		public bool findChunk(IffTag tag, SeekPos seekPos = SeekPos.BEGIN)
		{
			if(chunkStack.Count != 0)
			{
				//Move stream to beginning of chunk
				if(seekPos == SeekPos.BEGIN)
					stream.Seek(currentChunk.dataPos, SeekOrigin.Begin);

				//Look for the chunk
				while(true)
				{
					//Read in chunk header
					Chunk chunk = new Chunk();
					if(!readChunk(ref chunk))
						return false;

					//Check if tag
					if(chunk.tag == tag.value)
					{
						//Push chunk
						chunkStack.Add(chunk);
						currentChunk = chunk;

						//Return
						return true;
					}
					else
					{
						//Otherwise, skip this tag
						stream.Seek(chunk.size, SeekOrigin.Current);
					}
				}
			}
			else
			{
				//Read in chunk header
				Chunk chunk = new Chunk();
				if(!readChunk(ref chunk))
					return false;

				//Check if tag
				if(chunk.tag == tag.value)
				{
					//Push chunk
					chunkStack.Add(chunk);
					currentChunk = chunk;

					//Return
					return true;
				}
				else
					return false;
			}
		}

		public void endChunk()
		{
			//Pop chunk off stack
			int stackSize = chunkStack.Count;
			if(stackSize == 0)
			{
				Debug.Assert(false);
				return;
			}

			//Seek end of chunk
			stream.Seek(currentChunk.endPos, SeekOrigin.Begin);

			//Remove chunk
			chunkStack.RemoveAt(stackSize - 1);
			if(chunkStack.Count != 0)
				currentChunk = chunkStack[stackSize - 2];
			else
				currentChunk = new Chunk();
		}
		public void seekChunk(Int32 offset)
		{
			//Offset from beginning
			offset += (Int32)currentChunk.dataPos;
			stream.Seek(offset, SeekOrigin.Begin);
		}
		public void seekChunk(Int32 offset, SeekPos seekPos)
		{
			offset += (Int32)currentChunk.dataPos;
			SeekOrigin way;
			switch(seekPos)
			{
				default:
				case SeekPos.BEGIN: way = SeekOrigin.Begin; break;
				case SeekPos.CURRENT: way = SeekOrigin.Current; break;
				case SeekPos.END: way = SeekOrigin.End; break;
			}
			stream.Seek(offset, way);
		}

		public Chunk getCurrentChunk()
		{
			return currentChunk;
		}
		public bool readChunk(ref Chunk chunk)
		{
			//Find current position
			chunk.beginPos = stream.Position;
			if(chunkStack.Count != 0 && chunk.beginPos >= currentChunk.endPos)
				return false;

			//Tag
			chunk.tag = IffTag.TagByteSwap(reader.ReadUInt32());

			//Type
			chunk.type = (IffWriter.ChunkType)reader.ReadByte();

			//Size
			switch(chunk.type)
			{
				case IffWriter.ChunkType.DEFAULT:
				case IffWriter.ChunkType.BINARY:
				case IffWriter.ChunkType.UTF8:
				chunk.size = reader.ReadUInt32();
				break;
				case IffWriter.ChunkType.UINT8:
				case IffWriter.ChunkType.INT8:
				chunk.size = 1;
				break;
				case IffWriter.ChunkType.UINT16:
				case IffWriter.ChunkType.INT16:
				chunk.size = 2;
				break;
				case IffWriter.ChunkType.UINT32:
				case IffWriter.ChunkType.INT32:
				case IffWriter.ChunkType.FLOAT32:
				chunk.size = 4;
				break;
				case IffWriter.ChunkType.UINT64:
				case IffWriter.ChunkType.INT64:
				case IffWriter.ChunkType.FLOAT64:
				chunk.size = 8;
				break;
				default:
				Debug.Assert(false); //Data malformed
				return false;
			}

			//Metadata
			chunk.dataPos = stream.Position;
			chunk.endPos = chunk.dataPos + chunk.size;

			//Return
			return true;
		}

		//Value
		public bool findBool(IffTag tag, ref bool result, SeekPos seekPos = SeekPos.BEGIN)
		{
			//Begin chunk
			if(!findChunk(tag, seekPos))
				return false;

			//Parse
			result = BitConverter.ToBoolean(parseNumeric(), 0);

			//Return
			endChunk();
			return true;
		}
		public bool findUInt8(IffTag tag, ref byte result, SeekPos seekPos = SeekPos.BEGIN)
		{
			//Begin chunk
			if(!findChunk(tag, seekPos))
				return false;

			//Parse
			result = parseNumeric()[0];

			//Return
			endChunk();
			return true;
		}
		public bool findUInt16(IffTag tag, ref UInt16 result, SeekPos seekPos = SeekPos.BEGIN)
		{
			//Begin chunk
			if(!findChunk(tag, seekPos))
				return false;

			//Parse
			result = BitConverter.ToUInt16(parseNumeric(), 0);

			//Return
			endChunk();
			return true;
		}
		public bool findUInt32(IffTag tag, ref UInt32 result, SeekPos seekPos = SeekPos.BEGIN)
		{
			//Begin chunk
			if(!findChunk(tag, seekPos))
				return false;

			//Parse
			result = BitConverter.ToUInt32(parseNumeric(), 0);

			//Return
			endChunk();
			return true;
		}
		public bool findUInt64(IffTag tag, ref UInt64 result, SeekPos seekPos = SeekPos.BEGIN)
		{
			//Begin chunk
			if(!findChunk(tag, seekPos))
				return false;

			//Parse
			result = BitConverter.ToUInt64(parseNumeric(), 0);

			//Return
			endChunk();
			return true;
		}
		public bool findInt8(IffTag tag, ref byte result, SeekPos seekPos = SeekPos.BEGIN)
		{
			//Begin chunk
			if(!findChunk(tag, seekPos))
				return false;

			//Parse
			result = parseNumeric()[0];

			//Return
			endChunk();
			return true;
		}
		public bool findInt16(IffTag tag, ref Int16 result, SeekPos seekPos = SeekPos.BEGIN)
		{
			//Begin chunk
			if(!findChunk(tag, seekPos))
				return false;

			//Parse
			result = BitConverter.ToInt16(parseNumeric(), 0);

			//Return
			endChunk();
			return true;
		}
		public bool findInt32(IffTag tag, ref Int32 result, SeekPos seekPos = SeekPos.BEGIN)
		{
			//Begin chunk
			if(!findChunk(tag, seekPos))
				return false;

			//Parse
			result = BitConverter.ToInt32(parseNumeric(), 0);

			//Return
			endChunk();
			return true;
		}
		public bool findInt64(IffTag tag, ref Int64 result, SeekPos seekPos = SeekPos.BEGIN)
		{
			//Begin chunk
			if(!findChunk(tag, seekPos))
				return false;

			//Parse
			result = BitConverter.ToInt64(parseNumeric(), 0);

			//Return
			endChunk();
			return true;
		}
		public bool findFloat32(IffTag tag, ref Single result, SeekPos seekPos = SeekPos.BEGIN)
		{
			//Begin chunk
			if(!findChunk(tag, seekPos))
				return false;

			//Parse
			result = BitConverter.ToSingle(parseNumeric(), 0);

			//Return
			endChunk();
			return true;
		}
		public bool findFloat64(IffTag tag, ref Double result, SeekPos seekPos = SeekPos.BEGIN)
		{
			//Begin chunk
			if(!findChunk(tag, seekPos))
				return false;

			//Parse
			result = BitConverter.ToDouble(parseNumeric(), 0);

			//Return
			endChunk();
			return true;
		}

		//Read
		public bool readBool(IffTag tag, bool defaultValue, SeekPos seekPos = SeekPos.BEGIN)
		{
			bool value = false;
			if(findBool(tag, ref value, seekPos))
				return value;
			else
				return defaultValue;
		}
		public byte readUInt8(IffTag tag, byte defaultValue, SeekPos seekPos = SeekPos.BEGIN)
		{
			byte value = 0;
			if(findUInt8(tag, ref value, seekPos))
				return value;
			else
				return defaultValue;
		}
		public UInt16 readUInt16(IffTag tag, UInt16 defaultValue, SeekPos seekPos = SeekPos.BEGIN)
		{
			UInt16 value = 0;
			if(findUInt16(tag, ref value, seekPos))
				return value;
			else
				return defaultValue;
		}
		public UInt32 readUInt32(IffTag tag, UInt32 defaultValue, SeekPos seekPos = SeekPos.BEGIN)
		{
			UInt32 value = 0;
			if(findUInt32(tag, ref value, seekPos))
				return value;
			else
				return defaultValue;
		}
		public UInt64 readUInt64(IffTag tag, UInt64 defaultValue, SeekPos seekPos = SeekPos.BEGIN)
		{
			UInt64 value = 0;
			if(findUInt64(tag, ref value, seekPos))
				return value;
			else
				return defaultValue;
		}
		public byte readInt8(IffTag tag, byte defaultValue, SeekPos seekPos = SeekPos.BEGIN)
		{
			byte value = 0;
			if(findInt8(tag, ref value, seekPos))
				return value;
			else
				return defaultValue;
		}
		public Int16 readInt16(IffTag tag, Int16 defaultValue, SeekPos seekPos = SeekPos.BEGIN)
		{
			Int16 value = 0;
			if(findInt16(tag, ref value, seekPos))
				return value;
			else
				return defaultValue;
		}
		public Int32 readInt32(IffTag tag, Int32 defaultValue, SeekPos seekPos = SeekPos.BEGIN)
		{
			Int32 value = 0;
			if(findInt32(tag, ref value, seekPos))
				return value;
			else
				return defaultValue;
		}
		public Int64 readInt64(IffTag tag, Int64 defaultValue, SeekPos seekPos = SeekPos.BEGIN)
		{
			Int64 value = 0;
			if(findInt64(tag, ref value, seekPos))
				return value;
			else
				return defaultValue;
		}
		public Single readFloat32(IffTag tag, Single defaultValue, SeekPos seekPos = SeekPos.BEGIN)
		{
			Single value = 0;
			if(findFloat32(tag, ref value, seekPos))
				return value;
			else
				return defaultValue;
		}
		public Double readFloat64(IffTag tag, Double defaultValue, SeekPos seekPos = SeekPos.BEGIN)
		{
			Double value = 0;
			if(findFloat64(tag, ref value, seekPos))
				return value;
			else
				return defaultValue;
		}

		//String
		public bool findString(IffTag tag, ref string result, SeekPos seekPos = SeekPos.BEGIN)
		{
			//Begin chunk
			if(!findChunk(tag, seekPos))
				return false;

			//Read
			switch(currentChunk.type)
			{
				case IffWriter.ChunkType.UTF8:
				{
					//Read data
					byte[] bytes = reader.ReadBytes((int)currentChunk.size);
					result = System.Text.Encoding.UTF8.GetString(bytes);
					break;
				}
				default:
				{
					Debug.Assert(false);
					endChunk();
					return false;
				}
			}

			//End
			endChunk();

			//Return
			return true;
		}
		public string readString(IffTag tag, string defaultValue, SeekPos seekPos = SeekPos.BEGIN)
		{
			string value = "";
			if(findString(tag, ref value, seekPos))
				return value;
			else
				return defaultValue;
		}

		//Parse
		byte[] parseNumeric()
		{
			//Read data

			switch(currentChunk.type)
			{
				default:
				case IffWriter.ChunkType.DEFAULT:
				case IffWriter.ChunkType.BINARY:
				case IffWriter.ChunkType.UTF8:
				{
					Debug.Assert(false);
					endChunk();
					return null;
				}
				case IffWriter.ChunkType.UINT8:
				{
					byte[] result = new byte[sizeof(byte)];
					stream.Read(result, 0, sizeof(byte));
					return result;
				}
				case IffWriter.ChunkType.UINT16:
				{
					byte[] result = new byte[sizeof(UInt16)];
					stream.Read(result, 0, sizeof(UInt16));
					return result;
				}
				case IffWriter.ChunkType.UINT32:
				{
					byte[] result = new byte[sizeof(UInt32)];
					stream.Read(result, 0, sizeof(UInt32));
					return result;
				}
				case IffWriter.ChunkType.UINT64:
				{
					byte[] result = new byte[sizeof(UInt64)];
					stream.Read(result, 0, sizeof(UInt64));
					return result;
				}
				case IffWriter.ChunkType.INT8:
				{
					byte[] result = new byte[sizeof(byte)];
					stream.Read(result, 0, sizeof(byte));
					return result;
				}
				case IffWriter.ChunkType.INT16:
				{
					byte[] result = new byte[sizeof(Int16)];
					stream.Read(result, 0, sizeof(Int16));
					return result;
				}
				case IffWriter.ChunkType.INT32:
				{
					byte[] result = new byte[sizeof(Int32)];
					stream.Read(result, 0, sizeof(Int32));
					return result;
				}
				case IffWriter.ChunkType.INT64:
				{
					byte[] result = new byte[sizeof(Int64)];
					stream.Read(result, 0, sizeof(Int64));
					return result;
				}
				case IffWriter.ChunkType.FLOAT32:
				{
					byte[] result = new byte[sizeof(float)];
					stream.Read(result, 0, sizeof(float));
					return result;
				}
				case IffWriter.ChunkType.FLOAT64:
				{
					byte[] result = new byte[sizeof(Int64)];
					stream.Read(result, 0, sizeof(Int64));
					return result;
				}
			}
		}

		//Struct
		bool findStruct<TYPE>(IffTag tag, ref TYPE data, SeekPos seekPos = SeekPos.BEGIN) where TYPE : struct
		{
			//Begin chunk
			if(!findChunk(tag, seekPos))
				return false;

			//Read
			switch(currentChunk.type)
			{
				case IffWriter.ChunkType.BINARY:
				{
					//Check size
					int structSize = Marshal.SizeOf(data);
					if(currentChunk.size != structSize)
					{
						Debug.Assert(false);
						return false;
					}

					//Read data
					byte[] bytes = reader.ReadBytes(structSize);
					GCHandle handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
					data = (TYPE)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(TYPE));
					handle.Free();

					//End
					break;
				}
				default:
				{
					Debug.Assert(false);
					endChunk();
					return false;
				}
			}

			//End
			endChunk();

			//Return
			return true;
		}
		TYPE readStruct<TYPE>(IffTag tag, TYPE defaultValue, SeekPos seekPos = SeekPos.BEGIN) where TYPE : struct
		{
			TYPE result = defaultValue;
			if(findStruct<TYPE>(tag, ref result, seekPos))
				return result;
			else
				return defaultValue;
		}
		public byte[] readBytes(IffTag tag, byte[] defaultValue, SeekPos seekPos = SeekPos.BEGIN)
		{
			if(findChunk(tag, seekPos))
			{
				if(currentChunk.type != IffWriter.ChunkType.BINARY)
				{
					endChunk();
					return defaultValue;
				}
				else
				{
					var data = reader.ReadBytes((int)currentChunk.size);
					endChunk();
					return data;
				}
			}

			//Return default	
			return defaultValue;
		}

	};
}