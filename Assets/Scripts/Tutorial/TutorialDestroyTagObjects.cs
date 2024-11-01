﻿using UnityEngine;

public class TutorialDestroyTagObjects : TutorialBase
{
	[SerializeField]
	private	CharacterMovement	playerMove;
	[SerializeField]
	private	GameObject[]		objectList;
	[SerializeField]
	private	string				tagName;

	public override void Enter()
	{
		// 플레이어의 이동, 공격이 가능하도록 설정


		// 파괴해야할 오브젝트들을 활성화
		for ( int i = 0; i < objectList.Length; ++ i )
		{
			objectList[i].SetActive(true);
		}
	}

	public override void Execute(TutorialController controller)
	{
		GameObject[] objects = GameObject.FindGameObjectsWithTag(tagName);

		if ( objects.Length == 0 )
		{
			controller.SetNextTutorial();
		}
	}

	public override void Exit()
	{
		//플레이어 이동, 공격 불가능하도록 설정
	}
}

