using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EventSender<EVENT>
{
	public delegate void Callback_WithArg(EVENT eventData);
	public delegate void Callback_NoArg();

	public struct Callback
	{
		public Callback(Callback_WithArg withArg)
		{
			this.withArg = withArg;
			this.noArg = null;
		}
		public Callback(Callback_NoArg noArg)
		{
			this.withArg = null;
			this.noArg = noArg;
		}
		public void Invoke(EVENT eventData)
		{
			if(withArg != null)
				withArg(eventData);
			if(noArg != null)
				noArg();
		}
		

		public Callback_WithArg withArg;
		public Callback_NoArg noArg;
	}

	List<Callback> list = new List<Callback>();

	public static EventSender<EVENT> operator+ (EventSender<EVENT> eventSender, Callback_WithArg func)
	{
		eventSender.list.Add(new Callback(func));
		return eventSender;
	}
	public static EventSender<EVENT> operator +(EventSender<EVENT> eventSender, Callback_NoArg func)
	{
		eventSender.list.Add(new Callback(func));
		return eventSender;
	}
	public static EventSender<EVENT> operator- (EventSender<EVENT> eventSender, Callback_WithArg func)
	{
		for(int i=0; i<eventSender.list.Count; i++)
		{
			var callback = eventSender.list[i];
			if(callback.withArg == func)
			{
				eventSender.list.RemoveAt(i);
				i--;
			}
		}

		return eventSender;
	}
	public static EventSender<EVENT> operator -(EventSender<EVENT> eventSender, Callback_NoArg func)
	{
		for(int i = 0; i < eventSender.list.Count; i++)
		{
			var callback = eventSender.list[i];
			if(callback.noArg == func)
			{
				eventSender.list.RemoveAt(i);
				i--;
			}
		}

		return eventSender;
	}
	public void Invoke(EVENT eventData)
	{
		if (list.Count > 0)
		{
			var cache = list.ToArray();
			foreach (var func in cache)
			{
				func.Invoke(eventData);
			}
		}
	}
}
