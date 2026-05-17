using UnityEngine;

public class EndingManager : MonoBehaviour
{
    [Header("연결할 오브젝트")]
    public GameObject dialogPanel; 

    private bool isEnded = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isEnded || !collision.CompareTag("Player")) return;

        isEnded = true;

        YoungPrince_2DPlayerController player = collision.GetComponent<YoungPrince_2DPlayerController>();
        if (player != null)
        {
            player.TriggerEnding();
        }

        if (dialogPanel != null)
        {
            dialogPanel.SetActive(true);
        }
    }
}