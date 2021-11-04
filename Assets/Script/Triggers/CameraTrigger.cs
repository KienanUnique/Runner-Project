using UnityEngine;

namespace Script.Triggers
{
    public class CameraTrigger : MonoBehaviour
    {
        private CameraFollow _cameraCf;
        private Grid _mainGrid;
        private float _finalCenterX;
        
        [SerializeField] private bool centerBetween;
        [SerializeField] private int leftCellX;
        [SerializeField] private int rightCellX;
        [SerializeField] private int centerCellX;


        void Start()
        {
            if (Camera.main is { }) _cameraCf = Camera.main.gameObject.GetComponent<CameraFollow>();
            _mainGrid = GameObject.Find(GameConst.PlayerGameObjName).GetComponent<PlayerUtilities>().mainGrid;

            if (centerBetween)
            {
                _finalCenterX = (_mainGrid.GetCellCenterWorld(new Vector3Int(leftCellX, 0, 0)).x +
                                 _mainGrid.GetCellCenterWorld(new Vector3Int(rightCellX, 0, 0)).x) / 2;
            }
            else
            {
                _finalCenterX = _mainGrid.GetCellCenterWorld(new Vector3Int(centerCellX, 0, 0)).x;
            }
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            if(other.gameObject.CompareTag("Player"))
                _cameraCf.SetCenter(_finalCenterX);
        }
    }
}
