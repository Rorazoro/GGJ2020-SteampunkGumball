using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationXYOfset : MonoBehaviour
{
	float animationOffset = 0.5f;

    void Start()
    {
		var animator = GetComponent<Animator>();
		int pos = Mathf.FloorToInt(transform.position.y);
		//if (transform.position.y < 0)
		//	pos += 1;
		int offset = System.Math.Abs(pos % 2);
		if (offset == 0)
			animator.Play("Animation", 0, animationOffset);
		else
			animator.Play("Animation", 0, 0);
	}
}
