using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject productionPanel; // インスペクターで生産UIパネルを割り当てる

    void Update()
    {
        // 選択されているオブジェクトが1つだけかチェック
        if (UnitSelectionManager.Instance.unitsSelected.Count == 1)
        {
            GameObject selectedObject = UnitSelectionManager.Instance.unitsSelected[0];

            // そのオブジェクトがUnitProducerを持っているかチェック
            if (selectedObject.GetComponent<UnitProducer>() != null)
            {
                // 持っていたら生産パネルを表示
                productionPanel.SetActive(true);
            }
            else
            {
                // 持っていなければ非表示
                productionPanel.SetActive(false);
            }
        }
        else
        {
            // 複数選択または何も選択されていない場合は非表示
            productionPanel.SetActive(false);
        }
    }
}