using UnityEngine;

public class npcAnimSelect : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] bool crouch, injured, lookAround;
    [SerializeField] private Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (crouch == true) anim.Play("crouch");
        if (injured == true) anim.Play("fallen_idle");
        if (lookAround == true) anim.Play("look_around");
    }
}
