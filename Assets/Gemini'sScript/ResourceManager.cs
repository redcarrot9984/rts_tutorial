using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }

    public int currentMuscat { get; private set; }
    public int currentGas { get; private set; }
    // TODO: UI Text�Ɍ��݂̃��\�[�X�ʂ�\�����鏈����ǉ�����

    private void Awake()
    {
        // �V���O���g���̐ݒ�
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    // ���\�[�X��ǉ�����֐�
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
        Debug.Log(type + ": " + amount + " ���l���I ���݂̃~�l����: " + currentMuscat);
    }

    // ���\�[�X�������֐�
    public bool SpendResources(int muscatCost, int gasCost)
    {
        if (currentMuscat >= muscatCost && currentGas >= gasCost)
        {
            currentMuscat -= muscatCost;
            currentGas -= gasCost;
            return true; // �w������
        }
        else
        {
            Debug.Log("���\�[�X������܂���I");
            return false; // �w�����s
        }
    }
}