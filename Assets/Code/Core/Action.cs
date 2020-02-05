using UnityEngine;
using System.Collections;

namespace Game
{
	public abstract class Action
	{
		public ActionQueue parentQueue;
		public bool isComplete = false;
		public bool isStarted = false;
		public bool isBlocking = true;	//Does it prevent progression of the queue?

		public delegate void Event();
		public Event eventOnStart;
		public Event eventOnStop;
		

		public enum Priority
		{
			Default = 0,
			
			Game = 100,
			World = 200,
			Combat = 300,
			CombatPhase = 400,
			Card = 500,
			CardAction = 600,

			Highest = 9999,
		}
		public Priority priority = Priority.Default;

		public void SetIsStarted(bool state)
		{
			if(isStarted == state)
				return;
			isStarted = state;

			if(isStarted)
				OnStart();
			else
				OnStop();
		}

		public virtual void OnStart()
		{
			if(eventOnStart != null)
				eventOnStart();
		}
		public virtual void OnStop()
		{
			if(eventOnStop != null)
				eventOnStop();
		}
		public abstract void Update(float timeElapsed);
	}
}

