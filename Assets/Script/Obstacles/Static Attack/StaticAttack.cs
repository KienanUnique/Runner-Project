using System.Collections;
using Script;
using UnityEngine;

public class StaticAttack : MonoBehaviour
{
    private PlayerUtilities _player;
    private BoxCollider2D _playerBoxCollider2D;
    private Animator _animator;
    private BoxCollider2D _attackZone;

    private readonly int _hashAttack = Animator.StringToHash("Attack");
    
    void Start()
    {
        _player = GameObject.Find(GameConst.PlayerGameObjName).GetComponent<PlayerUtilities>();
        _playerBoxCollider2D = GameObject.Find(GameConst.PlayerGameObjName).GetComponent<BoxCollider2D>();
        _animator = GetComponent<Animator>();
        _attackZone = transform.GetChild(1).gameObject.GetComponent<BoxCollider2D>();
    }
    
    public void OnPlayerEnteredTrigger()
    {
        _animator.SetTrigger(_hashAttack);
    }

    public void AttackAnimation()
    {
        if(_attackZone.IsTouching(_playerBoxCollider2D))
        {
            _player.Kill();
        }
    }

}
