using UnityEngine;

namespace Script.Obstacles.Static_Attack
{
    public class TriggerStaticAttack : MonoBehaviour
    {
        private StaticAttack _enemyController;
        private void Start()
        {
            _enemyController = GetComponentInParent<StaticAttack>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if(other.gameObject.CompareTag("Player"))
                _enemyController.OnPlayerEnteredTrigger();
        }
    }
}
