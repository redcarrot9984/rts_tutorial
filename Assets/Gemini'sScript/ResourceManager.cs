using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }

    public int currentMuscat { get; private set; }
    public int currentGas { get; private set; }
    // TODO: UI Textに現在のリソース量を表示する処理を追加する

    private void Awake()
    {
        // シングルトンの設定
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    // リソースを追加する関数
    public void AddResources(ResourceSource.ResourceType type, int amount)
    {
        if (type == ResourceSource.ResourceType.Muscat)
        {
            currentMuscat += amount;
        }
        else if (type == ResourceSource.ResourceType.Gas)
        {
            currentGas += amount;
        }
        Debug.Log(type + ": " + amount + " を獲得！ 現在のミネラル: " + currentMuscat);
    }

    // リソースを消費する関数
    public bool SpendResources(int muscatCost, int gasCost)
    {
        if (currentMuscat >= muscatCost && currentGas >= gasCost)
        {
            currentMuscat -= muscatCost;
            currentGas -= gasCost;
            return true; // 購入成功
        }
        else
        {
            Debug.Log("リソースが足りません！");
            return false; // 購入失敗
        }
    }
}