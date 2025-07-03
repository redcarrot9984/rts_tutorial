using UnityEngine;

public class ResourceSource : MonoBehaviour
{
    public enum ResourceType { Muscat, Gas } // 資源の種類を定義
    public ResourceType resourceType;
    public int amount = 1500; // この資源オブジェクトに含まれる量

    // 資源を収集する関数
    public int Gather(int gatherAmount)
    {
        int gathered = Mathf.Min(gatherAmount, amount);
        amount -= gathered;

        if (amount <= 0)
        {
            // 資源が枯渇したらオブジェクトを消す
            Destroy(gameObject);
        }
        return gathered;
    }
}