using UnityEngine;

public class Lens : MonoBehaviour
{
    public Transform targetPosition; 
    public float moveSpeed = 3f;
    public bool isMoving = false;
  

  
    private void Update()
    {
       
       
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition.position, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPosition.position) < 0.01f)
            {
                isMoving = false;
                OnLensReachedTarget();
              //  fokometre.Instance.SetupPrescription();
            }
        }
    }

    public void StartMoving()
    {
        if (MachineManager.Instance.currentMachineIndex==0) {
            isMoving = true;
        }
       
    }

    private void OnLensReachedTarget()
    {
        Debug.Log("Hedefe ulastý");
        if (this.gameObject.name == "lens")
        {
            HUDController.instance.ShowfokoPanel();
        }
        else if (this.gameObject.name == "lensforcihaz3")
        {
            HUDController.instance.ShowCihaz3Panel();
        }
      
      
        /* if (this.gameObject.name == "lens")
        {
            HUDController.instance.ShowfokoPanel();
        }else if(this.gameObject.name == "glasses")
        {
            HUDController.instance.ShowGlassPanel();
        }*/
    }

}
