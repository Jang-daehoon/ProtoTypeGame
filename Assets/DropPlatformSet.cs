using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropPlatformSet : MonoBehaviour
{
    public Transform[] DropPlatformPos;  // 플랫폼이 소환될 위치들
    public GameObject DropPlatformObj;   // 소환할 플랫폼 오브젝트 프리팹

    // Update is called once per frame
    void Update()
    {
        foreach (Transform t in DropPlatformPos)
        {
            // 각 위치에 플랫폼이 없으면 소환
            if (t.childCount == 0)
            {
                StartCoroutine(SpawnPlatformWithDelay(t));
            }
        }
    }

    // 각 플랫폼의 위치에 3초 후에 플랫폼을 다시 소환하는 코루틴
    private IEnumerator SpawnPlatformWithDelay(Transform spawnPosition)
    {
        // 플랫폼 소환을 위한 대기 시간 (3초)
        yield return new WaitForSeconds(3f);

        // 해당 위치에 플랫폼이 아직 없는 경우에만 소환
        if (spawnPosition.childCount == 0)
        {
            GameObject newPlatform = Instantiate(DropPlatformObj, spawnPosition.position, Quaternion.identity);

            // 소환된 플랫폼을 해당 위치의 자식으로 설정하여 관리
            newPlatform.transform.SetParent(spawnPosition);
        }
    }
}
