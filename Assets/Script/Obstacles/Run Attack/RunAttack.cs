using UnityEngine;
using Pathfinding;
using Script;

public class RunAttack : MonoBehaviour
{
    public Transform target;
    
    public float nextWaypointDistance = 1f;

    private Path _path;
    private int _currentWaypoint;
    private bool _reachedEndOfPath;

    private Seeker _seeker;
    private GameObject _enemyGfx;

    private Animator _animator;
    void Start()
    {
        _seeker = GetComponent<Seeker>();
        _enemyGfx = transform.GetChild(0).gameObject;
        _animator = _enemyGfx.GetComponent<Animator>();
        InvokeRepeating(nameof(UpdatePath), 0f, .5f);
        _animator.Play("Run");
    }

    void UpdatePath()
    {
        if(_seeker.IsDone())
            _seeker.StartPath(transform.position, target.position, OnPathComplete);
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            _path = p;
            _currentWaypoint = 0;
        }
    }
    void FixedUpdate()
    {
        if (_path == null)
            return;

        if (_currentWaypoint >= _path.vectorPath.Count)
        {
            _reachedEndOfPath = true;
            return;
        }
        else
        {
            _reachedEndOfPath = false;
        }
        
        Vector2 directon = ((Vector2)_path.vectorPath[_currentWaypoint] - (Vector2)transform.position).normalized;
        
        transform.position += (Vector3)directon * GameConst.s_Speed * Time.deltaTime; 
        
        float distance = Vector2.Distance(transform.position, _path.vectorPath[_currentWaypoint]);
        
        if (distance < nextWaypointDistance)
        {
            _currentWaypoint++;
        }
        
        if (directon.x >= 0)
        {
            _enemyGfx.transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
        }
        else
        {
            _enemyGfx.transform.localScale = new Vector3(-2.5f, 2.5f, 2.5f);
        }
    }
}
