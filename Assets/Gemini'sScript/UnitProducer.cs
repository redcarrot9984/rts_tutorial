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
        Debug.Log("1. 生産ボタンがクリックされました！");

        if (ResourceManager.Instance.SpendResources(muscatCost, 0))
        {
            Debug.Log("2. リソースの消費に成功。生産を開始します。");
            StartCoroutine(ProduceUnit());
        }
        else
        {
            Debug.LogError("リソースが足りません！ 現在のリソース: " + ResourceManager.Instance.currentMuscat);
        }
    }

    private IEnumerator ProduceUnit()
    {
        Debug.Log("3. ユニットを生産中です...");
        yield return new WaitForSeconds(productionTime);

        if (unitPrefab == null || spawnPoint == null)
        {
            Debug.LogError("Unit PrefabまたはSpawn Pointが設定されていません！");
            yield break;
        }

        Instantiate(unitPrefab, spawnPoint.position, Quaternion.identity);
        Debug.Log("4. 生産完了！");
    }
}