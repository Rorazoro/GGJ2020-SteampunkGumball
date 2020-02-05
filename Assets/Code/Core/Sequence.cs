using UnityEngine;
using System.Collections.Generic;

namespace Game
{
	namespace Sequence
	{
		public interface ISequenceProvider<TYPE>
		{
			TYPE nextInSequence();
		}

		public class SequencedNumerical<TYPE> : ISequenceProvider<TYPE>
		{
			class Event
			{
				public TYPE value;
				public int amount;
			}
			List<Event> events = new List<Event>();
			int index = 0;

			public void addEvent(TYPE value, int amount)
			{
				Event ourEvent = new Event();
				ourEvent.value = value;
				ourEvent.amount = amount;
				events.Add(ourEvent);
			}
			public TYPE nextInSequence()
			{
				//Get event
				Event currentEvent = null;
				if(events.Count  > 0)
					currentEvent = events[0];

				//Increment
				if(currentEvent != null)
				{
					index += 1;
					if(index > currentEvent.amount)
					{
						//Cleanup
						index = 0;
						events.RemoveAt(0);

						//Get next event
						if(events.Count > 0)
							currentEvent = events[0];
					}
				}

				//Return
				if(currentEvent != null)
					return currentEvent.value;
				else
					return default(TYPE);
			}
		}
		public class AtRandom<TYPE> : ISequenceProvider<TYPE>
		{
			List<TYPE> reserved = new List<TYPE>();
			List<TYPE> available = new List<TYPE>();
			public int minRepeat = 0;
			int totalCount = 0;

			public AtRandom()
			{
			}
			public void addItem(TYPE obj)
			{
				available.Add(obj);
				totalCount += 1;
			}
			public int getItemCount()
			{
				return available.Count;
			}
			public TYPE nextInSequence()
			{
				//Aquire random item
				int index = UnityEngine.Random.Range(0, available.Count);
				var obj = available[index];
				available.RemoveAt(index);

				//Add to reserve
				reserved.Add(obj);
				while(reserved.Count > System.Math.Min(minRepeat, totalCount - 1))
				{
					//Remove
					var oldObj = reserved[0];
					reserved.RemoveAt(0);

					//Add
					available.Add(oldObj);
				}

				//Return
				return obj;
			}
		}


	}
}