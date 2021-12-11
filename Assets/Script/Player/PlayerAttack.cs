using Script.Player;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float slowdownFactor = 0.3f;
    
    private bool _isAiming;

    private PlayerEffects _playerEffects;
    void Start()
    {
        _playerEffects = GetComponent<PlayerEffects>();
        _isAiming = false;
    }

    public void EnterAimingMode()
    {
        _isAiming = true;
        _playerEffects.SetBlackout(true);
        Time.timeScale = slowdownFactor;
        Time.fixedDeltaTime = Time.timeScale * .02f;
    }
    
    public void ExitAimingMode()
    {
        _playerEffects.SetBlackout(false);
        Time.timeScale = 1f;
        _isAiming = false;
    }

    public bool IsInAimingMode()
    {
        return _isAiming;
    }
}
