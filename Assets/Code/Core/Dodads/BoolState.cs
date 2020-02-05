public struct BoolState
{
	int count;
	bool defaultState;
	public BoolState(bool defaultState)
	{
		this.count = 0;
		this.defaultState = defaultState;
	}
	public void push()
	{
		count += 1;
	}
	public void pop()
	{
		count -= 1;
		if(count < 0)
			count = 0;
	}
	public bool getValue()
	{
		if(defaultState)
			return (count == 0);
		else
			return (count > 0);
	}
}