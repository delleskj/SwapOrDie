using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Seeker))]
[RequireComponent(typeof(CharacterController))]
public class EnemyMove : MonoBehaviour {

    [SerializeField]
    private float m_MoveSpeed;

    private Seeker m_PathSeeker;
    private CharacterController m_CharController;
    private Transform m_PlayerPos;

    private int m_CurrentWayPoint = 0;
    private Pathfinding.Path m_Path = null;

    [SerializeField]
    private float m_NextWayPointDist = 0.5f;

    private EnemyCombat m_Comb;

	// Use this for initialization
	void Start () {
        m_PathSeeker = GetComponent<Seeker>();
        m_CharController = GetComponent<CharacterController>();
        m_Comb = GetComponent<EnemyCombat>();

        GameObject player = GameObject.Find("Player");
        if (player != null)
        {
            m_PlayerPos = player.transform;
        }
        else { Debug.LogError("did not find Player"); }

        FindNewPath();
    }


    void FindNewPath()
    {
        m_PathSeeker.StartPath(transform.position, m_PlayerPos.position, OnPathFound);
    }
	

    void OnPathFound(Pathfinding.Path newPath)
    {
        if (!newPath.error)
        {
            //Debug.Log("found new path");
            m_Path = newPath;
            m_CurrentWayPoint = 0;
        }
    }

	// Update is called once per frame
	void Update () {
        if (m_Path == null) return;

        float distToPlayer = Vector3.Distance(transform.position, m_PlayerPos.position);

        if (m_CurrentWayPoint >= m_Path.vectorPath.Count)
        {
            // requeue if not in attack range
            if (m_Comb != null)
            {

                if (distToPlayer > m_Comb.GetAttackRange())
                {
                    FindNewPath();
                }
            }
            return;
        }

        //update path every 30 frames
        if(Time.frameCount % 30 == 0)
        {
            FindNewPath();
        }
       

        Vector3 moveDir = (m_Path.vectorPath[m_CurrentWayPoint] - transform.position).normalized;
        moveDir *= m_MoveSpeed;

        m_CharController.SimpleMove(moveDir);

        if(Vector3.Distance(transform.position, m_Path.vectorPath[m_CurrentWayPoint]) <= m_NextWayPointDist)
        {
            m_CurrentWayPoint++;
        }

        //also check for range
        if (m_Comb != null)
        {
           
            if (distToPlayer < m_Comb.GetAttackRange())
            {
                //destination reached
                m_CurrentWayPoint = m_Path.vectorPath.Count;
            }
        }
    }
}
