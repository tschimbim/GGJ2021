using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class BotScript : MonoBehaviour
{
    #region Unity References
    [SerializeField] private float myDesiredWaypointDist = 0.0f;
    #endregion

    #region Variables - Private
    private NavMeshAgent myAgent = null;
    #endregion

    #region Unity Callbacks
    private void Awake()
    {
        myAgent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        FindNewTarget();
    }

    private void Update()
    {
        if (!myAgent.hasPath && !myAgent.pathPending && myAgent.pathStatus == NavMeshPathStatus.PathComplete)
            FindNewTarget();
    }
    #endregion

    #region Methods - Public
    public void FindNewTarget()
    {
        float angle = Random.value * 360.0f;
        NavMesh.SamplePosition(transform.position + Quaternion.AngleAxis(angle, Vector3.up) * Vector3.forward * myDesiredWaypointDist, out NavMeshHit hit, float.MaxValue, NavMesh.AllAreas);

        myAgent.destination = hit.position;
    }
    #endregion
}
