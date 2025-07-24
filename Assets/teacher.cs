using UnityEngine;

public class teacher : MonoBehaviour
{
    private GameObject player;
    public void TurnTowardsTo()
    {
        Vector3 direction = player.transform.position - transform.position;
        direction.y = 0; 
        transform.rotation = Quaternion.LookRotation(direction);
    }
    private void Start()
    {
        player = GameObject.FindWithTag("Player");
    }
    private void Update()
    {
        TurnTowardsTo();
    }
}
