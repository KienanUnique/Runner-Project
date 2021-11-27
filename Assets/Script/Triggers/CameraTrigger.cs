using UnityEngine;

namespace Script.Triggers
{
    public class CameraTrigger : MonoBehaviour
    {
        private CameraFollow _cameraCf;
        private Grid _mainGrid;
        private float _finalCenterX;
        private LevelUtilities _levelUtilities;
        
        [SerializeField] private bool centerBetween;
        [SerializeField] private int leftCellX;
        [SerializeField] private int rightCellX;
        [SerializeField] private int centerCellX;


        void Start()
        {
            _levelUtilities = GameObject.Find(GameConst.PlayerGameObjName).GetComponent<LevelUtilities>();
            if (Camera.main is { }) _cameraCf = Camera.main.gameObject.GetComponent<CameraFollow>();
            _mainGrid = _levelUtilities.GetLevelGrid();
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("Player")) return;
            
            if (centerBetween)
            {
                _cameraCf.SetCameraXBetweenCellsCenters(leftCellX, rightCellX);
            }
            else
            {
                _cameraCf.SetCameraXOnCellCenter(centerCellX);
            }
        }
    }
}
