using System.Collections;
using UnityEngine;

public class isitici : MonoBehaviour
{
    Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void onInteraction()
    {
        
        if (MachineManager.Instance.currentMachineIndex == 4)//hardcodelad�k buray� belki d�zeltirm
        {
            animator.SetTrigger("interacted");
            HUDController.instance.onIsiticiInteracted();
            StartCoroutine(Isitici());
        }
       
        
    }
   private IEnumerator Isitici()
    {
        yield return new WaitForSeconds(1);
        MachineManager.Instance.NextMachine();
    }
}
