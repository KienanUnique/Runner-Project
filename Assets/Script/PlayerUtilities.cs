using UnityEngine;

namespace Script
{
    public class PlayerUtilities : MonoBehaviour
    {
        public bool isAlive = true;
        public Grid mainGrid;

        private float _moveToX;
        private Animator _animator;

        private readonly int _hashIsMoving = Animator.StringToHash("IsMoving");
        private readonly int _hashDir = Animator.StringToHash("Direction");

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _animator.SetInteger(_hashDir, 1);
            _animator.SetBool(_hashIsMoving, true);
        }

        void Update()
        {
            if (isAlive)
            {
                transform.position = new Vector3(Mathf.Lerp(transform.position.x, _moveToX, Time.deltaTime * 8),
                    transform.position.y + GameConst.s_Speed * Time.deltaTime, 0);
            }
        }

        public void Move(int direction)
        {
            int moveToCellX = mainGrid.WorldToCell(transform.position).x;
            if (direction == GameConst.s_LeftDir && moveToCellX - 1 >= GameConst.s_LeftBorder)
            {
                moveToCellX -= 1;
            }
            else if (direction == GameConst.s_RightDir && moveToCellX + 1 <= GameConst.s_RightBorder)
            {
                moveToCellX += 1;
            }

            _moveToX = mainGrid.GetCellCenterWorld(new Vector3Int(moveToCellX, 0, 0)).x;
        }

        public void Respawn()
        {
            _animator.SetInteger(_hashDir, 1);
            _animator.SetBool(_hashIsMoving, true);
            transform.position = mainGrid.GetCellCenterWorld((Vector3Int) GameConst.PlayerStartPos);
            _moveToX = transform.position.x;
            isAlive = true;
        }

        public void Kill()
        {
            isAlive = false;
            _animator.SetInteger(_hashDir, 0);
            _animator.SetBool(_hashIsMoving, false);
        }
    }
}
