using UnityEngine;

public class mineScript : MonoBehaviour
{
    [SerializeField]
    private BoxCollider mineTrigger;
    [SerializeField]
    private ParticleSystem explosion;
    [SerializeField]
    private AudioClip boomSound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            var health = other.gameObject.GetComponent<PlayerHealth>();
            if(health)
            {
                health.DecreaseHealth(health.maxHealth);
            }
        }
    }
}
