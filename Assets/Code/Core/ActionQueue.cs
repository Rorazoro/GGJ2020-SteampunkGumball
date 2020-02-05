using UnityEngine;
using System.Collections.Generic;

namespace Game
{
	public class ActionQueue
	{
		Action activeAction = null;
		List<Action> queue = new List<Action>();
		List<Action> nonBlocking = new List<Action>();

		List<Action.Priority> priorityStack = new List<Action.Priority>();
		public void pushPriority(Action.Priority priority)
		{
			priorityStack.Add(priority);
		}
		public void pushPriority()
		{
			pushPriority(getDefaultPriority() + 1);
		}
		public void popPriority()
		{
			if(priorityStack.Count > 0)
				priorityStack.RemoveAt(priorityStack.Count - 1);
		}
		public Action.Priority getDefaultPriority()
		{
			if(priorityStack.Count > 0)
				return priorityStack[priorityStack.Count - 1];
			else
				return Action.Priority.Default;
		}

		public Action push(Action action)
		{
			action.parentQueue = this;
			action.isComplete = false;

			//Inherit Priority
			if(action.priority == Action.Priority.Default)
				action.priority = getDefaultPriority();

			//Search for the correct place to add the action
			bool hasBeenAdded = false;
			for(int i = 0; i < queue.Count; i++)
			{
				var nextAction = queue[i];
				if(nextAction.priority < action.priority)
				{
					hasBeenAdded = true;
					queue.Insert(i, action);
					break;
				}
			}

			//Push to the back
			if(!hasBeenAdded)
				queue.Add(action);

			//Return
			return action;
		}
		public void pushNext(Action action)
		{
			//action.game = game;
			action.parentQueue = this;
			queue.Insert(0, action);
		}
		public void pushNext(IEnumerable<Action> actions)
		{
			foreach(var action in actions)
			{
				//action.game = game;
				action.parentQueue = this;
			}
			queue.InsertRange(0, actions);
		}
		public bool update(float timeElapsed)
		{
			//Choose next action
			if(activeAction == null)
			{
				if(queue.Count > 0)
				{
					//Pop the next action
					var action = queue[0];
					queue.RemoveAt(0);

					//Move to correct bucket
					if(action.isBlocking)
					{
						activeAction = action;
						pushPriority(activeAction.priority);
					}
					else
						nonBlocking.Add(action);

					//Start
					action.SetIsStarted(true);
				}
			}

			//Process active action
			if(activeAction != null)
			{
				activeAction.Update(Time.deltaTime);
				if(activeAction.isComplete)
				{
					//Clear
					activeAction.SetIsStarted(false);
					activeAction = null;
					popPriority();
				}
			}

			//Process non-blocking
			if(nonBlocking.Count > 0)
			{
				for(int i = 0; i < nonBlocking.Count; i++)
				{
					var action = nonBlocking[i];
					action.Update(Time.deltaTime);
					if(action.isComplete)
					{
						action.SetIsStarted(false);
						action.parentQueue = null;
						nonBlocking.RemoveAt(i);
						i--;
					}
				}
			}

			//Return
			return (queue.Count > 0 || nonBlocking.Count > 0);
		}
		public bool hasActions()
		{
			return (queue.Count > 0 || nonBlocking.Count > 0 || activeAction != null);
		}
		public void Clear()
		{
			queue.Clear();
		}
	}



	/*public class ActionQueue
	{
		public GameInst game;
		Action activeAction = null;
		List<Action> queue = new List<Action>();
		List<Action> nonBlocking = new List<Action>();

		public void push(Action action)
		{
			action.game = game;

			//Search for the correct place to add the action
			bool hasBeenAdded = false;
			for(int i = 0; i < queue.Count; i++)
			{
				var nextAction = queue[i];
				if(nextAction.priority < action.priority)
				{
					hasBeenAdded = true;
					queue.Insert(i, action);
					break;
				}
			}

			//Push to the back
			if(!hasBeenAdded)
				queue.Add(action);
		}
		public void pushNext(Action action)
		{
			action.game = game;
			if(queue.Count == 0)
				queue.Add(action);
			else
				queue.Insert(1, action);
		}
		public void pushNext(IEnumerable<Action> actions)
		{
			foreach(var action in actions)
			{
				action.game = game;
			}
			if(queue.Count == 0)
				queue.AddRange(actions);
			else
				queue.InsertRange(1, actions);
		}
		public bool update(float timeElapsed)
		{
			//Choose next action
			if(activeAction == null)
			{
				if(queue.Count > 0)
				{
					//Pop the next action
					var action = queue[0];
					queue.RemoveAt(0);
					action.setIsStarted(true);

					//Move to correct bucket
					if(action.isBlocking)
						activeAction = action;
					else
						nonBlocking.Add(action);
				}
			}

			//Process active action
			if(activeAction != null)
			{
				activeAction.update(Time.deltaTime);
				if(activeAction.isComplete)
				{
					//Clear
					activeAction.setIsStarted(false);
					activeAction = null;
				}
			}

			//Process actions
			/*if(queue.Count > 0)
			{
				var action = queue[0]; //First in first out
				action.setIsStarted(true);
				if(action.isBlocking)
				{
					//Process
					action.update(Time.deltaTime);
					if(action.isComplete)
					{
						//Remove action
						action.setIsStarted(false);
						queue.Remove(action);
					}
				}
				else
				{
					//Move to non-blocking queue
					queue.Remove(action);
					nonBlocking.Add(action);
				}
			}*/

	//Process non-blocking
	/*if(nonBlocking.Count > 0)
	{
		for(int i=0; i<nonBlocking.Count; i++)
		{
			var action = nonBlocking[i];
			action.update(Time.deltaTime);
			if(action.isComplete)
			{
				action.setIsStarted(false);
				nonBlocking.RemoveAt(i);
				i--;
			}
		}
	}

	//Return
	return (queue.Count > 0 || nonBlocking.Count > 0);
}
public bool hasActions()
{
	return (queue.Count > 0);
}
public void clear()
{
	queue.Clear();
}
}*/
}

