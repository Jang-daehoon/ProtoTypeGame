using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropPlatformSet : MonoBehaviour
{
    public Transform[] DropPlatformPos;  // �÷����� ��ȯ�� ��ġ��
    public GameObject DropPlatformObj;   // ��ȯ�� �÷��� ������Ʈ ������

    // Update is called once per frame
    void Update()
    {
        foreach (Transform t in DropPlatformPos)
        {
            // �� ��ġ�� �÷����� ������ ��ȯ
            if (t.childCount == 0)
            {
                StartCoroutine(SpawnPlatformWithDelay(t));
            }
        }
    }

    // �� �÷����� ��ġ�� 3�� �Ŀ� �÷����� �ٽ� ��ȯ�ϴ� �ڷ�ƾ
    private IEnumerator SpawnPlatformWithDelay(Transform spawnPosition)
    {
        // �÷��� ��ȯ�� ���� ��� �ð� (3��)
        yield return new WaitForSeconds(3f);

        // �ش� ��ġ�� �÷����� ���� ���� ��쿡�� ��ȯ
        if (spawnPosition.childCount == 0)
        {
            GameObject newPlatform = Instantiate(DropPlatformObj, spawnPosition.position, Quaternion.identity);

            // ��ȯ�� �÷����� �ش� ��ġ�� �ڽ����� �����Ͽ� ����
            newPlatform.transform.SetParent(spawnPosition);
        }
    }
}
