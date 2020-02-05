using UnityEngine;
using System.Collections;

public class FadeOut : Game.Action
{
	float timer = 0;
	float duration = 0.5f;
	Color color = Color.black;
	public FadeOut(float duration = 0.5f)
	{
		this.duration = duration;
	}
	public override void OnStart()
	{
		var fade = AppMain.inst.screenFade;
		fade.gameObject.SetActive(true);

		var color = this.color;
		color.a = 0.0f;
		fade.color = color;
	}
	public override void OnStop()
	{
		//Nothing
	}
	public override void Update(float timeElapsed)
	{
		timer += timeElapsed;
		float ratio = Mathf.Min(timer / duration, 1.0f);

		//Fade
		var fade = AppMain.inst.screenFade;
		var color = this.color;
		color.a = Tween.SineEaseOut(ratio);
		fade.color = color;

		//Check if complete
		if (timer >= duration)
			isComplete = true;
	}
}
public class FadeIn : Game.Action
{
	float timer = 0;
	float duration = 0.5f;
	Color color = Color.black;
	public FadeIn(float duration = 0.5f)
	{
		this.duration = duration;
	}
	public override void OnStart()
	{
		var fade = AppMain.inst.screenFade;
		fade.gameObject.SetActive(true);

		var color = this.color;
		color.a = 1.0f;
		fade.color = color;
	}
	public override void OnStop()
	{
		//Disable the fade
		var fade = AppMain.inst.screenFade;
		fade.gameObject.SetActive(false);
	}
	public override void Update(float timeElapsed)
	{
		timer += timeElapsed;
		float ratio = Mathf.Min(timer / duration, 1.0f);

		//Fade
		var fade = AppMain.inst.screenFade;
		var color = this.color;
		color.a = Tween.SineEaseOut(1.0f - ratio);
		fade.color = color;

		//Check if complete
		if (timer >= duration)
			isComplete = true;
	}
}
