using UnityEngine;
using System.Collections;

public struct Callback<TYPE>
{
	public Callback(CallbackWithArg callback)
	{
		withArg = callback;
		noArg = null;
	}
	public Callback(CallbackNoArg callback)
	{
		withArg = null;
		noArg = callback;
	}

	public static implicit operator Callback<TYPE>(CallbackWithArg callback)
	{
		return new Callback<TYPE>(callback);
	}
	public static implicit operator Callback<TYPE>(CallbackNoArg callback)
	{
		return new Callback<TYPE>(callback);
	}

	public delegate void CallbackWithArg(object userObj);
	public delegate void CallbackNoArg();

	public void invoke(TYPE arg)
	{
		if(withArg != null)
			withArg(arg);
		else if(noArg != null)
			noArg();
	}

	CallbackWithArg withArg;
	CallbackNoArg noArg;
}