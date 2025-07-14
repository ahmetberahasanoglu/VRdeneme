using UnityEngine;

public class Glass : MonoBehaviour
{
 
    public Transform targetPosition;
    public float moveSpeed = 3f;
    private bool isMoving = false;
 
    private void Update()
    {
      

        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition.position, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPosition.position) < 0.01f)
            {
                isMoving = false;
                OnGlassesReachedTarget();
            }
        }
    }

    public void StartMoving()
    {
        if (MachineManager.Instance.currentMachineIndex == 1)
        {
            isMoving = true;
        }

    }

    private void OnGlassesReachedTarget()
    {
        HUDController.instance.ShowGlassPanel();
        
    }



}
