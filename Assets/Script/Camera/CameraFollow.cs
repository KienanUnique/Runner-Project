using UnityEngine;

namespace Script.Camera
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private bool centerBetween;
        [SerializeField] private int defaultLeftCellX;
        [SerializeField] private int defaultRightCellX;
        [SerializeField] private int defaultCenterCellX;
        
        [SerializeField] private float lerpTime = 0.1f;
     
        private Transform _target;
        private float _defaultCameraMiddleX = 0.5f;
        private float _currentCameraMiddleX;
        private Vector3 _offset;
        private Vector3 _targetPos;

        private LevelUtilities _levelUtilities;
        private Grid _mainGrid;
        private Vector3 _velocity = Vector3.zero;

        private void Start()
        {
            _target = GameObject.Find(GameConst.PlayerGameObjName).transform;
            _levelUtilities = GameObject.Find(GameConst.PlayerGameObjName).GetComponent<LevelUtilities>();
            _mainGrid = _levelUtilities.GetMainGrid();
            
            _offset = transform.position - _target.position;
            
            if (centerBetween)
            {
                _defaultCameraMiddleX = SetCameraXBetweenCellsCenters(defaultLeftCellX, defaultRightCellX);
            }
            else
            {
                _defaultCameraMiddleX = SetCameraXOnCellCenter(defaultCenterCellX);
            }
        }

        private void Update()
        {
            if (_target == null) return;

            _targetPos = new Vector3(_currentCameraMiddleX, _target.position.y + _offset.y, _offset.z);
            transform.position = Vector3.SmoothDamp(transform.position, _targetPos, ref _velocity, lerpTime);
        }

        public void SetCameraX(float centerX)
        {
            _currentCameraMiddleX = centerX;
        }
        
        public float SetCameraXBetweenCellsCenters(int leftCellX, int rightCellX)
        {
            _currentCameraMiddleX = (_mainGrid.GetCellCenterWorld(new Vector3Int(leftCellX, 0, 0)).x +
                                     _mainGrid.GetCellCenterWorld(new Vector3Int(rightCellX, 0, 0)).x) / 2;
            return _currentCameraMiddleX;
        }
        
        public float SetCameraXOnCellCenter(int centerCellX)
        {
            _currentCameraMiddleX = _mainGrid.GetCellCenterWorld(new Vector3Int(centerCellX, 0, 0)).x;
            return _currentCameraMiddleX;
        }

        public void ReturnCenter()
        {
            _currentCameraMiddleX = _defaultCameraMiddleX;
        }
    }
}
