using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] float checkpointtimeextension = 5f;

    GameManager gamemanager;

    const string playerString = "Player";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gamemanager = FindFirstObjectByType<GameManager>();

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gamemanager.IncreaseTime(checkpointtimeextension);
            
        }
    }

   
}
