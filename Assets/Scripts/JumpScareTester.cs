using UnityEngine;

public class JumpScareTester : MonoBehaviour
{
    private JumpScareController jumpScareController;

    void Start()
    {
        jumpScareController = GetComponent<JumpScareController>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J)) // Press 'J' to trigger the jumpscare
        {
            jumpScareController.TriggerJumpScare();
        }
    }
}
