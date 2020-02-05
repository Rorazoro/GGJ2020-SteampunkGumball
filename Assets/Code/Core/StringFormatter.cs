using UnityEngine;
using UnityEngine;
using System.Collections;
using System;
using Game;

public abstract class StringFormatter
{
	public string FormatText(string text, int start = 0)
	{
		if(String.IsNullOrEmpty(text))
			return "";

		var escapes = new char[] { '{', '}' };
		while (true)
		{
			//Find the next braket
			int bracket = text.IndexOfAny(escapes, start);
			if (bracket == -1)
				break;
			if(text[bracket] == '{')
			{
				//Recursive
				text = FormatText(text, bracket + 1);
			}
			else
			{
				//Closing
				var command = text.Substring(start, (bracket - start));
				var args = command.Split(',');
				var result = FormatCommand(args);

				//Replace in text
				text = text.Substring(0, start-1 ) + result + text.Substring(bracket + 1);
				break;
			}
		}

		//Return
		return text;
	}

	public abstract string FormatCommand(string[] args);
}

public class ObjStringFormatter: StringFormatter
{
	string errorText = "(PARSE ERROR)";
	public System.Object obj;

	public override string FormatCommand(string[] args)
	{
		//Check command
		if(args[0] == "plural")
		{
			//Verify
			if(args.Length != 4)
				return errorText;

			//Find variable
			var value = GetValue(args[1]);
			if (value != null)
			{
				int num = System.Convert.ToInt32(value);
				return (num == 1) ? args[2] : args[3];
			}
		}
		else if(args[0] == "if")
		{
			if(!(args.Length == 4))
				return errorText;

			bool order = System.Convert.ToBoolean(GetValue(args[1]));
			if(order)
			{
				return System.Convert.ToString(GetValue(args[2]));
			}
			else
			{
				return System.Convert.ToString(GetValue(args[3]));
			}
		}
		else if(args[0] == "order")
		{
			if(!(args.Length == 4))
				return errorText;

			bool order = System.Convert.ToBoolean(GetValue(args[1]));
			if(order)
			{
				return System.Convert.ToString(GetValue(args[2])) + System.Convert.ToString(GetValue(args[3]));
			}
			else
			{
				return System.Convert.ToString(GetValue(args[3])) + System.Convert.ToString(GetValue(args[2]));
			}
		}
		else
		{
			//Find variable
			var value = GetValue(args[0]);
			if (value != null)
				return value.ToString();
		}
		return errorText;
	}
	object GetValue(string name)
	{
		if (String.IsNullOrEmpty(name)) //Empty
			return null;
		else if(Char.IsNumber(name[0])) //Numeric
		{
			return name;
		}
		else
		{
			//Attempt to get field
			var field = obj.GetType().GetField(name, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
			if (field != null)
				return field.GetValue(obj);

			//Attempt to get property
			var property = obj.GetType().GetProperty(name, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
			if(property != null)
				return property.GetValue(obj);

			//Fail
			return null;
		}
	}
}
