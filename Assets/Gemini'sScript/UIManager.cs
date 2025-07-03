using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject productionPanel; // �C���X�y�N�^�[�Ő��YUI�p�l�������蓖�Ă�

    void Update()
    {
        // �I������Ă���I�u�W�F�N�g��1�������`�F�b�N
        if (UnitSelectionManager.Instance.unitsSelected.Count == 1)
        {
            GameObject selectedObject = UnitSelectionManager.Instance.unitsSelected[0];

            // ���̃I�u�W�F�N�g��UnitProducer�������Ă��邩�`�F�b�N
            if (selectedObject.GetComponent<UnitProducer>() != null)
            {
                // �����Ă����琶�Y�p�l����\��
                productionPanel.SetActive(true);
            }
            else
            {
                // �����Ă��Ȃ���Δ�\��
                productionPanel.SetActive(false);
            }
        }
        else
        {
            // �����I���܂��͉����I������Ă��Ȃ��ꍇ�͔�\��
            productionPanel.SetActive(false);
        }
    }
}