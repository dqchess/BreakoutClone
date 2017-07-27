﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowPowerUp : TimedPowerUp 
{
	public float speed;
	private Ball ball;
	private float ballDefaultSpeed;

	new void Start()
	{
		PowerUpHolder.Instance.Remove<FastPowerUp>();
		
		base.Start();
	}

	protected override void PowerUpStart()
	{
		ball = StageManager.Instance.ball.GetComponent<Ball>();
        ballDefaultSpeed = ball.speed;
		ball.SetSpeed(speed);
	}

	protected override void PowerUpEnd()
	{
		ball.SetSpeed(ballDefaultSpeed);
	}
}
