using UnityEngine;

public class PetController : MonoBehaviour
{
    [SerializeField] private float PetScreenProportionY = 0.4f;
    private bool _isSpawned;
    void Start()
    {
        transform.gameObject.SetActive(false);
    }

    void Update()
    {
        if(_isSpawned){
            UpdatePetPosition();
        }
    }

    public void Spawn(){
        _isSpawned = true;
        UpdatePetPosition();
        transform.gameObject.SetActive(true);
    }

    public void Despawn(){
        _isSpawned = false;
        transform.gameObject.SetActive(false);
    }

    public Vector2 GetScreenPetPosition(){
        return new Vector2(Screen.width / 2, Screen.height  * PetScreenProportionY);
    }

    private void UpdatePetPosition(){
        Vector2 petPosition = Camera.main.ScreenToWorldPoint(GetScreenPetPosition());
        transform.position = petPosition;
    }

    // TODO: public void AttackToDirection(Vector2 attackDirection)

}
