using UnityEngine;
using TMPro;

public class MineDestructor : MonoBehaviour
{
    [Range(0, 100)] public float destroyChance = 50f;
    public TextMeshProUGUI statusText;
    public TextMeshPro detectorStatusText;

    private Collider currentMine;
    private bool isMinigameActive = false;

    private MinigameManager mmManager;

    [SerializeField] GameObject detectorModel;
    [SerializeField] Animator detectorAnim;
    [SerializeField] ParticleSystem smoke;

    private void Start()
    {
        mmManager = FindFirstObjectByType<MinigameManager>();
        if (mmManager == null)
            Debug.LogError("MineDestructor: No MinigameManager found in scene!");
        detectorAnim = detectorModel.GetComponent<Animator>();
        
    }

    private void Update()
    {
        if (isMinigameActive) return;

        if (currentMine != null && Input.GetKeyDown(KeyCode.E))
        {
            float roll = Random.Range(0f, 100f);
            if (roll <= destroyChance)
            {
                UpdateStatus("Mine destroyed successfully!");
                Destroy(currentMine.gameObject);
                currentMine = null;
                //anim and smoke
                detectorAnim.Play("detector_disarm");
                smoke.Play();
            }
            else
            {
                UpdateStatus("Malfunction detected! Must be disarmed...");
                if (mmManager != null)
                {
                    isMinigameActive = true;
                    var mineRef = currentMine;
                    currentMine = null;

                    mmManager.LaunchRandomMinigame(
                        () => OnMinigameSuccess(mineRef),
                        () => OnMinigameFailure(mineRef),
                        15f
                    );
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Mine"))
        {
            currentMine = other;
            UpdateStatus("Mine in range. Press [E] to attempt destruction.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other == currentMine)
        {
            UpdateStatus("Left mine range.");
            currentMine = null;
        }
    }

    private void OnMinigameSuccess(Collider mine)
    {
        UpdateStatus("Mine defused!");
        Destroy(mine.gameObject);
        isMinigameActive = false;
    }

    private void OnMinigameFailure(Collider mine)
    {
        UpdateStatus("Mine exploded while disarming!");
        Destroy(mine.gameObject);
        isMinigameActive = false;
    }

    private void UpdateStatus(string msg)
    {
        if (statusText != null) statusText.text = msg;
        if (detectorStatusText != null) detectorStatusText.text = msg;
    }
}
