using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

public class mineScript : MonoBehaviour
{
    [SerializeField] private BoxCollider mineTrigger;
    [SerializeField] private GameObject explosionPrefab; // Prefab version
    [SerializeField] private AudioClip boomSound;
    [SerializeField] private AudioSource audioSource;
       
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI deathMessage;
    [SerializeField] private TextMeshProUGUI countdownText;

    private GameObject mineDetector;
    private TextMeshProUGUI statusText;




    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var health = other.GetComponent<PlayerHealth>();
            var controller = other.GetComponent<FirstPersonPlayer>();
            var cameraLook = other.GetComponentInChildren<FirstPersonPlayer>();

            statusText = GameObject.Find("statusText").GetComponent<TextMeshProUGUI>();
            statusText.gameObject.SetActive(false);

            mineDetector = GameObject.FindWithTag("MineDetector");
            if (mineDetector != null)
            {
                mineDetector.SetActive(false);
            }

            if (health)
            {
                mineTrigger.enabled = false;

                // locks screen and disable controls - not sure it works, haven't tested without explosion.
                if (controller) controller.enabled = false;
                if (cameraLook) cameraLook.enabled = false;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                StartCoroutine(HandleMineTrigger(health));
            }
        }
    }

    private IEnumerator HandleMineTrigger(PlayerHealth playerHealth)
    {
        // spawns explosion at mine’s position
        if (explosionPrefab)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }

        // plays explosion sfx
        if (boomSound && audioSource)
        {
            audioSource.PlayOneShot(boomSound);
        }

        // Show UI
        if (deathMessage) deathMessage.gameObject.SetActive(true);

        if (countdownText)
        {
            countdownText.gameObject.SetActive(true);
            for (int i = 4; i > 0; i--)
            {
                countdownText.text = $"Restarting in {i}";
                yield return new WaitForSeconds(1f);
            }
            countdownText.gameObject.SetActive(false);
        }

        yield return new WaitForSeconds(2f); // total 5 seconds delay

        /* playerHealth.DecreaseHealth(playerHealth.maxHealth);*/
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);


        yield return new WaitForSeconds(1f);
        if (deathMessage) deathMessage.gameObject.SetActive(false);
    }
}
