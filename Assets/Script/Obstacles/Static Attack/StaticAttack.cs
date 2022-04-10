using System.Collections;
using Script;
using Script.Player;
using UnityEngine;

public class StaticAttack : MonoBehaviour
{
    private PlayerCharacter _playerCharacter;
    private BoxCollider2D _playerBoxCollider2D;
    private Animator _animator;
    private BoxCollider2D _attackZone;

    private readonly int _hashAttack = Animator.StringToHash("Attack");

    private void Start()
    {
        _playerCharacter = GameObject.FindGameObjectWithTag(GameConst.PlayerTag).GetComponent<PlayerCharacter>();
        _playerBoxCollider2D = GameObject.FindGameObjectWithTag(GameConst.PlayerTag).GetComponent<BoxCollider2D>();
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
            _playerCharacter.Kill();
        }
    }

}
