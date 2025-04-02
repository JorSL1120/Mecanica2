using UnityEngine;

public class V2_Meta : MonoBehaviour
{
    public GameObject WinPlayer;

    void Start()
    {
        WinPlayer = GameObject.Find("WinPlayer1");

        WinPlayer.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player1"))
        {
            WinPlayer.SetActive(true);
            Time.timeScale = 0;
        }
    }
}
