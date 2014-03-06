﻿using UnityEngine;
using System.Collections;

public class Cannon : MonoBehaviour
{
	void Start ()
	{
		updateHitPointBar ();
	}
	
	void Update ()
	{
		updateHitPoint ();
	}

	#region fire
	public Transform gunpoint;
	public float flyTime = 1f;
	public HitSpot[] hitSpots;
	public AudioSource fireSound;
	public Ship ship;

	public void Fire ()
	{
		Bomb bomb = ship.PopBomb ();
		iTween.Stop (bomb.gameObject);
		bomb.transform.position = gunpoint.position;

		GameObject target = hitSpots [Random.Range (0, hitSpots.Length)].gameObject;

		iTween.MoveTo (bomb.gameObject, iTween.Hash ("position", target.transform, "time", flyTime));
		iTween.ScaleTo (bomb.gameObject, new Vector3 (0.5f, 0.5f, 0.5f), flyTime);

		fireSound.Play ();
		playBacklashAnimationOnShip ();

		consumeHitPoint (bomb.mass);
	}

	private void playBacklashAnimationOnShip ()
	{
		iTween.PunchPosition (ship.gameObject, new Vector3 (0f, -1f, 0f), 0.1f);
	}

	#endregion

	#region hit point
	public UIProgressBar hitPointBar;
	private float maxHitPoint = 100.0f;
	private float curHitPoint = 100.0f;
	private float recoverySpeed = 3.0f;

	public void Upgrade (int level)
	{
		float max;
		float recovery;

		switch (level) {
		case 2:
			max = 200.0f;
			recovery = 2.0f;
			break;

		case 1:
		default:
			max = 100.0f;
			recovery = 1.0f;
			break;
		}

		setAttributes (max, recovery);
	}

	private void setAttributes (float maxHitPoint, float recoverySpeed)
	{
		this.maxHitPoint = maxHitPoint;
		this.curHitPoint = recoverySpeed;
		this.recoverySpeed = recoverySpeed;
		updateHitPointBar ();
	}

	private void consumeHitPoint (float amount)
	{
		this.curHitPoint = Mathf.Max (this.curHitPoint - amount, 0f);
		updateHitPointBar ();
	}

	private void updateHitPoint ()
	{
		if (curHitPoint < maxHitPoint) {
			curHitPoint = Mathf.Min (curHitPoint + recoverySpeed * Time.deltaTime, maxHitPoint);
			updateHitPointBar ();
		}

	}

	private void updateHitPointBar ()
	{
		hitPointBar.value = this.curHitPoint / this.maxHitPoint;
	}

	#endregion
}
