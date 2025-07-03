using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UnitMovement : MonoBehaviour
{
	Camera cam;
	UnityEngine.AI.NavMeshAgent agent;
	public LayerMask ground;

    public bool isCommandedToMove;
    
    Animator animator;

	private void Start()
	{
		cam = Camera.main;
		agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
		animator = GetComponent<Animator>();
	}
	
	// UnitMovement.cs の Updateメソッドを置き換える

private void Update()
{
    if (Input.GetMouseButtonDown(1))
    {
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            // 必要なコンポーネントを取得
            AttackController attackController = GetComponent<AttackController>();
            Worker worker = GetComponent<Worker>();

            //【優先度1】敵ユニットをクリックした場合 (攻撃命令)
            if (attackController != null && hit.collider.CompareTag("Enemy"))
            {
                Debug.Log("攻撃命令を認識！ターゲットは " + hit.collider.name);
                attackController.targetToAttack = hit.transform;
            }
            //【優先度2】資源をクリックした場合 (資源採集命令)
            else if (worker != null && hit.collider.GetComponent<ResourceSource>() != null)
            {
                Debug.Log("資源採集命令を認識！");
                ResourceSource resource = hit.collider.GetComponent<ResourceSource>();
                worker.StartGathering(resource);
            }
            //【優先度3】地面をクリックした場合 (移動命令)
            else if (((1 << hit.collider.gameObject.layer) & ground) != 0)
            {
                Debug.Log("移動命令を認識！");
                // 移動するときは、攻撃や採集のターゲットを解除する
                if (attackController != null) attackController.targetToAttack = null;
                // if (worker != null) worker.StopGathering(); // 必要なら採集を中断する処理

                agent.SetDestination(hit.point);
            }
        }
    }

    // アニメーションの更新
    if (agent.remainingDistance > agent.stoppingDistance)
    {
        animator.SetBool("isMoving", true);
    }
    else
    {
        animator.SetBool("isMoving", false);
    }
}
}