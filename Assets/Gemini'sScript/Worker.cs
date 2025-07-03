using UnityEngine;
using UnityEngine.AI; // NavMeshAgent���g�����߂ɕK�v

[RequireComponent(typeof(NavMeshAgent))] // ���̃X�N���v�g�ɂ�NavMeshAgent���K�{
public class Worker : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;

    // �J���҂̏�Ԃ��`����񋓌^
    private enum WorkerState
    {
        Idle,               // �ҋ@
        MovingToResource,   // �����ֈړ���
        Gathering,          // ���W��
        MovingToDepot,      // ���_�ֈړ���
    }
    private WorkerState currentState;

    private ResourceSource currentResourceNode; // �Ώۂ̎���
    public Transform depot; // ������͂��鋒�_

    public int maxCarriedResources = 10; // ��x�ɉ^�ׂ鎑���̍ő��
    private int carriedResources = 0;   // ���݉^��ł��鎑���̗�
    public float gatherRate = 2f; // 2�b������1����W����

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        // �Q�[���J�n���͈�ԋ߂����_�������Őݒ肷��ƕ֗�
        // depot = FindObjectOfType<UnitProducer>().transform; 
        currentState = WorkerState.Idle;
    }

    void Update()
    {
        // ��Ԃɉ�����Update���̏�����؂�ւ���
        switch (currentState)
        {
            case WorkerState.Idle:
                // TODO: �ҋ@���̐U�镑���i��F����̎����������ŒT���ɍs���j
                break;

            case WorkerState.MovingToResource:
                // �����ɏ\���ɋ߂Â�������W��ԂɈڍs
                if (Vector3.Distance(transform.position, currentResourceNode.transform.position) < 2f)
                {
                    ChangeState(WorkerState.Gathering);
                }
                break;

            case WorkerState.Gathering:
                // ���W�͎��Ԃ̂����鏈���Ȃ̂ŉ������Ȃ��i�R���[�`���ɔC����j
                break;

            case WorkerState.MovingToDepot:
                // ���_�ɏ\���ɋ߂Â����玑����a���āA�Ăю����֌�����
                if (Vector3.Distance(transform.position, depot.position) < 3f)
                {
                    // 1. �������}�l�[�W���[�ɓn��
                    ResourceManager.Instance.AddResources(currentResourceNode.resourceType, carriedResources);
                    carriedResources = 0; // �莝�������Z�b�g

                    // 2. �Ăю����֌�����
                    ChangeState(WorkerState.MovingToResource);
                }
                break;
        }

        // �A�j���[�V�����̍X�V
        animator.SetBool("isMoving", agent.velocity.magnitude > 0.1f);
    }

    // �O��������W���߂��o�����߂̊֐�
    public void StartGathering(ResourceSource resourceNode)
    {
        currentResourceNode = resourceNode;
        ChangeState(WorkerState.MovingToResource);
    }
    
    // ��Ԃ�ύX����֐�
    private void ChangeState(WorkerState newState)
    {
        currentState = newState;

        switch (currentState)
        {
            case WorkerState.Idle:
                agent.isStopped = true;
                break;
            case WorkerState.MovingToResource:
                agent.isStopped = false;
                agent.SetDestination(currentResourceNode.transform.position);
                break;
            case WorkerState.Gathering:
                agent.isStopped = true;
                StartCoroutine(GatherCoroutine()); // ���W�R���[�`�����J�n
                break;
            case WorkerState.MovingToDepot:
                agent.isStopped = false;
                agent.SetDestination(depot.position);
                break;
        }
    }

    // �������W�̏����i���Ԃ̂����鏈���̓R���[�`���Łj
    private System.Collections.IEnumerator GatherCoroutine()
    {
        yield return new WaitForSeconds(gatherRate); // gatherRate�b�҂�

        if (currentResourceNode != null)
        {
            // �����m�[�h���玑�������炤
            int gathered = currentResourceNode.Gather(1); // �Ƃ肠����1�����W
            if (gathered > 0)
            {
                carriedResources += gathered;
            }

            // ���^���ɂȂ������A�������͊������狒�_��
            if (carriedResources >= maxCarriedResources || currentResourceNode == null)
            {
                ChangeState(WorkerState.MovingToDepot);
            }
            else // �܂��^�ׂ�Ȃ�A������x���W
            {
                StartCoroutine(GatherCoroutine());
            }
        }
        else // ���W���Ɏ������͊������ꍇ
        {
            ChangeState(WorkerState.Idle);
        }
    }
}