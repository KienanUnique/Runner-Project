using UnityEngine;

namespace Script
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private bool centerBetween;
        [SerializeField] private int leftCellX;
        [SerializeField] private int rightCellX;
        [SerializeField] private int centerCellX;
        
        [SerializeField] private float lerpSpeed = 4f;
     
        private Transform _target;
        private float _defaultCameraMiddleX = 0.5f;
        private float _currentCameraMiddleX;
        private Vector3 _offset;
        private Vector3 _targetPos;

        private LevelUtilities _levelUtilities;

        private void Start()
        {
            _target = GameObject.Find(GameConst.PlayerGameObjName).transform;
            _levelUtilities = GameObject.Find(GameConst.PlayerGameObjName).GetComponent<LevelUtilities>(); 
            Grid mainGrid = _levelUtilities.GetLevelGrid();
            
            if (centerBetween)
            {
                _defaultCameraMiddleX = (mainGrid.GetCellCenterWorld(new Vector3Int(leftCellX, 0, 0)).x +
                                 mainGrid.GetCellCenterWorld(new Vector3Int(rightCellX, 0, 0)).x) / 2;
            }
            else
            {
                _defaultCameraMiddleX = mainGrid.GetCellCenterWorld(new Vector3Int(centerCellX, 0, 0)).x;
            }
            _currentCameraMiddleX = _defaultCameraMiddleX;
            _offset = transform.position - _target.position;
        }

        private void Update()
        {
            if (_target == null) return;

            _targetPos = new Vector3(_currentCameraMiddleX, _target.position.y + _offset.y, _offset.z);

            transform.position = Vector3.Lerp(transform.position, _targetPos, lerpSpeed * Time.deltaTime);
        }

        public void SetCenter(float centerX)
        {
            _currentCameraMiddleX = centerX;
        }

        public void ReturnCenter()
        {
            _currentCameraMiddleX = _defaultCameraMiddleX;
        }
    }
}
