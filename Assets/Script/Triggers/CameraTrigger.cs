using Script.Camera;
using UnityEngine;

namespace Script.Triggers
{
    public class CameraTrigger : MonoBehaviour
    {
        private CameraFollow _cameraCf;

        [SerializeField] private bool centerBetween;
        [SerializeField] private int leftCellX;
        [SerializeField] private int rightCellX;
        [SerializeField] private int centerCellX;


        private void Start()
        {
            if (UnityEngine.Camera.main is { }) _cameraCf = UnityEngine.Camera.main.gameObject.GetComponent<CameraFollow>();
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
