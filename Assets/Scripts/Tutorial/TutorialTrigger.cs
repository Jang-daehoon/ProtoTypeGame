﻿
using UnityEngine;

public class TutorialTrigger : TutorialBase
{
	[SerializeField] 
	private CharacterMovement	player;
	[SerializeField]
	private	Transform			triggerObject;
	
	public	bool isTrigger { set; get; } = false;

	public override void Enter()
	{
		// Trigger 오브젝트 활성화
		triggerObject.gameObject.SetActive(true);
	}

	public override void Execute(TutorialController controller)
	{

		/// 거리 기준
		/*
		if ( (triggerObject.position - playerController.transform.position).sqrMagnitude < 0.1f )
		{
			controller.SetNextTutorial();
		}
		*/

		/// 충돌 기준
		// TutorialTrigger 오브젝트의 위치를 플레이어와 동일하게 설정 (Trigger 오브젝트와 충돌할 수 있도록)
		transform.position = player.transform.position;

		if ( isTrigger == true )
		{
			controller.SetNextTutorial();
		}
	}

	public override void Exit()
	{
		// Trigger 오브젝트 비활성화
		triggerObject.gameObject.SetActive(false);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if ( collision.transform.Equals(triggerObject) )
		{
			isTrigger = true;

			collision.gameObject.SetActive(false);
		}
	}
}

