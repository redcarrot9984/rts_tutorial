using UnityEngine;

public class ResourceSource : MonoBehaviour
{
    public enum ResourceType { Muscat, Gas } // �����̎�ނ��`
    public ResourceType resourceType;
    public int amount = 1500; // ���̎����I�u�W�F�N�g�Ɋ܂܂���

    // ���������W����֐�
    public int Gather(int gatherAmount)
    {
        int gathered = Mathf.Min(gatherAmount, amount);
        amount -= gathered;

        if (amount <= 0)
        {
            // �������͊�������I�u�W�F�N�g������
            Destroy(gameObject);
        }
        return gathered;
    }
}