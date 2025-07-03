using UnityEngine;
using System.Collections;

public class UnitProducer : MonoBehaviour
{
    public GameObject unitPrefab;
    public Transform spawnPoint;

    public int muscatCost = 50;
    public float productionTime = 5f;

    public void StartProduction()
    {
        Debug.Log("1. ���Y�{�^�����N���b�N����܂����I");

        if (ResourceManager.Instance.SpendResources(muscatCost, 0))
        {
            Debug.Log("2. ���\�[�X�̏���ɐ����B���Y���J�n���܂��B");
            StartCoroutine(ProduceUnit());
        }
        else
        {
            Debug.LogError("���\�[�X������܂���I ���݂̃��\�[�X: " + ResourceManager.Instance.currentMuscat);
        }
    }

    private IEnumerator ProduceUnit()
    {
        Debug.Log("3. ���j�b�g�𐶎Y���ł�...");
        yield return new WaitForSeconds(productionTime);

        if (unitPrefab == null || spawnPoint == null)
        {
            Debug.LogError("Unit Prefab�܂���Spawn Point���ݒ肳��Ă��܂���I");
            yield break;
        }

        Instantiate(unitPrefab, spawnPoint.position, Quaternion.identity);
        Debug.Log("4. ���Y�����I");
    }
}