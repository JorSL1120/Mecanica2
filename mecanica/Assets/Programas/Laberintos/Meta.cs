using UnityEngine;

public class Meta : MonoBehaviour
{
    public GameObject WinPlayer1, WinPlayer2;

    void Start()
    {
        WinPlayer1 = GameObject.Find("WinPlayer1");
        WinPlayer2 = GameObject.Find("WinPlayer2");

        WinPlayer1.SetActive(false);
        WinPlayer2.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player1"))
        {
            WinPlayer1.SetActive(true);
            Time.timeScale = 0;
        }
        else if (other.CompareTag("Player2"))
        {
            WinPlayer2.SetActive(true);
            Time.timeScale = 0;
        }
    }
}
