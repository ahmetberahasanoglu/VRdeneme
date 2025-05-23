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
        if (MachineManager.Instance.currentMachineIndex == 0 && this.gameObject.name == "lens")
        {
            isMoving = true;
        }
        else if (this.gameObject.name == "lensforcihaz3"&& MachineManager.Instance.currentMachineIndex == 2)
        {
            isMoving = true;
        }
        else if (this.gameObject.name == "lensforcihaz4" && MachineManager.Instance.currentMachineIndex == 3)
        {
            isMoving = true;
        }
       

    }

    private void OnLensReachedTarget()
    {
        Debug.Log("Hedefe ulast�");
        if (this.gameObject.name == "lens")
        {
            HUDController.instance.ShowfokoPanel();
        }
        else if (this.gameObject.name == "lensforcihaz3")
        {
            HUDController.instance.ShowCihaz3Panel();
        }
        else if (this.gameObject.name == "lensforcihaz4")
        {
            HUDController.instance.ShowCihaz4Panel();
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
