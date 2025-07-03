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
	
	// UnitMovement.cs �� Update���\�b�h��u��������

private void Update()
{
    if (Input.GetMouseButtonDown(1))
    {
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            // �K�v�ȃR���|�[�l���g���擾
            AttackController attackController = GetComponent<AttackController>();
            Worker worker = GetComponent<Worker>();

            //�y�D��x1�z�G���j�b�g���N���b�N�����ꍇ (�U������)
            if (attackController != null && hit.collider.CompareTag("Enemy"))
            {
                Debug.Log("�U�����߂�F���I�^�[�Q�b�g�� " + hit.collider.name);
                attackController.targetToAttack = hit.transform;
            }
            //�y�D��x2�z�������N���b�N�����ꍇ (�����̏W����)
            else if (worker != null && hit.collider.GetComponent<ResourceSource>() != null)
            {
                Debug.Log("�����̏W���߂�F���I");
                ResourceSource resource = hit.collider.GetComponent<ResourceSource>();
                worker.StartGathering(resource);
            }
            //�y�D��x3�z�n�ʂ��N���b�N�����ꍇ (�ړ�����)
            else if (((1 << hit.collider.gameObject.layer) & ground) != 0)
            {
                Debug.Log("�ړ����߂�F���I");
                // �ړ�����Ƃ��́A�U����̏W�̃^�[�Q�b�g����������
                if (attackController != null) attackController.targetToAttack = null;
                // if (worker != null) worker.StopGathering(); // �K�v�Ȃ�̏W�𒆒f���鏈��

                agent.SetDestination(hit.point);
            }
        }
    }

    // �A�j���[�V�����̍X�V
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