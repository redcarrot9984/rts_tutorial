using UnityEngine;
using UnityEngine.AI; // NavMeshAgentを使うために必要

[RequireComponent(typeof(NavMeshAgent))] // このスクリプトにはNavMeshAgentが必須
public class Worker : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;

    // 労働者の状態を定義する列挙型
    private enum WorkerState
    {
        Idle,               // 待機
        MovingToResource,   // 資源へ移動中
        Gathering,          // 収集中
        MovingToDepot,      // 拠点へ移動中
    }
    private WorkerState currentState;

    private ResourceSource currentResourceNode; // 対象の資源
    public Transform depot; // 資源を届ける拠点

    public int maxCarriedResources = 10; // 一度に運べる資源の最大量
    private int carriedResources = 0;   // 現在運んでいる資源の量
    public float gatherRate = 2f; // 2秒かけて1回収集する

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        // ゲーム開始時は一番近い拠点を自動で設定すると便利
        // depot = FindObjectOfType<UnitProducer>().transform; 
        currentState = WorkerState.Idle;
    }

    void Update()
    {
        // 状態に応じてUpdate内の処理を切り替える
        switch (currentState)
        {
            case WorkerState.Idle:
                // TODO: 待機中の振る舞い（例：周りの資源を自動で探しに行く）
                break;

            case WorkerState.MovingToResource:
                // 資源に十分に近づいたら収集状態に移行
                if (Vector3.Distance(transform.position, currentResourceNode.transform.position) < 2f)
                {
                    ChangeState(WorkerState.Gathering);
                }
                break;

            case WorkerState.Gathering:
                // 収集は時間のかかる処理なので何もしない（コルーチンに任せる）
                break;

            case WorkerState.MovingToDepot:
                // 拠点に十分に近づいたら資源を預けて、再び資源へ向かう
                if (Vector3.Distance(transform.position, depot.position) < 3f)
                {
                    // 1. 資源をマネージャーに渡す
                    ResourceManager.Instance.AddResources(currentResourceNode.resourceType, carriedResources);
                    carriedResources = 0; // 手持ちをリセット

                    // 2. 再び資源へ向かう
                    ChangeState(WorkerState.MovingToResource);
                }
                break;
        }

        // アニメーションの更新
        animator.SetBool("isMoving", agent.velocity.magnitude > 0.1f);
    }

    // 外部から収集命令を出すための関数
    public void StartGathering(ResourceSource resourceNode)
    {
        currentResourceNode = resourceNode;
        ChangeState(WorkerState.MovingToResource);
    }
    
    // 状態を変更する関数
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
                StartCoroutine(GatherCoroutine()); // 収集コルーチンを開始
                break;
            case WorkerState.MovingToDepot:
                agent.isStopped = false;
                agent.SetDestination(depot.position);
                break;
        }
    }

    // 資源収集の処理（時間のかかる処理はコルーチンで）
    private System.Collections.IEnumerator GatherCoroutine()
    {
        yield return new WaitForSeconds(gatherRate); // gatherRate秒待つ

        if (currentResourceNode != null)
        {
            // 資源ノードから資源をもらう
            int gathered = currentResourceNode.Gather(1); // とりあえず1ずつ収集
            if (gathered > 0)
            {
                carriedResources += gathered;
            }

            // 満タンになったか、資源が枯渇したら拠点へ
            if (carriedResources >= maxCarriedResources || currentResourceNode == null)
            {
                ChangeState(WorkerState.MovingToDepot);
            }
            else // まだ運べるなら、もう一度収集
            {
                StartCoroutine(GatherCoroutine());
            }
        }
        else // 収集中に資源が枯渇した場合
        {
            ChangeState(WorkerState.Idle);
        }
    }
}