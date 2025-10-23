using UnityEngine;

public class BoxingAnimationTester : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Punch");
        }else if (Input.GetMouseButtonDown(1))
        {
            animator.SetTrigger("Defense");
        }
        else if (Input.GetMouseButtonDown(2))
        {
            animator.SetTrigger("Damaged");
        }
    }
}