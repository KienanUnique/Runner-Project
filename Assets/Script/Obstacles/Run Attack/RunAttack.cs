using UnityEngine;
using Pathfinding;
using Script;

public class RunAttack : MonoBehaviour
{
    public Transform target;
    
    public float nextWaypointDistance = 1f;

    private Path path;
    private int currentWaypoint = 0;
    private bool reachedEndOfPath = false;

    private Seeker seeker;
    //private Rigidbody2D rb;
    private GameObject enemyGFX;

    private Animator _animator;
    void Start()
    {
        seeker = GetComponent<Seeker>();
        //rb = GetComponent<Rigidbody2D>();
        enemyGFX = transform.GetChild(0).gameObject;
        _animator = enemyGFX.GetComponent<Animator>();
        InvokeRepeating("UpdatePath", 0f, .5f);
        _animator.Play("Run");
    }

    void UpdatePath()
    {
        if(seeker.IsDone())
            seeker.StartPath(transform.position, target.position, OnPathComplete);
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }
    void FixedUpdate()
    {
        if (path == null)
            return;

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }
        
        Vector2 directon = ((Vector2)path.vectorPath[currentWaypoint] - (Vector2)transform.position).normalized;
        
        transform.position += (Vector3)directon * GameConst.s_Speed * Time.deltaTime; 
        
        float distance = Vector2.Distance(transform.position, path.vectorPath[currentWaypoint]);
        
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
        
        if (directon.x >= 0)
        {
            enemyGFX.transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
        }
        else
        {
            enemyGFX.transform.localScale = new Vector3(-2.5f, 2.5f, 2.5f);
        }
    }
}
