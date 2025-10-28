using UnityEngine;
using UnityEngine.SceneManagement;

public class MissionComplete : MonoBehaviour
{
    [Header("UI Reference")]
    public GameObject missionCompleteUI; // assign your Mission Complete panel

    [Header("Player Reference")]
    public GameObject player; // assign your Player object here

    private bool missionEnded = false;

    private void OnTriggerEnter(Collider other)
    {
        if (missionEnded) return;

        if (other.CompareTag("Player"))
        {
            missionEnded = true;
            player = other.gameObject;
            ShowMissionComplete();
        }
    }

    void ShowMissionComplete()
    {
        // ?? Disable player scripts
        MonoBehaviour[] playerScripts = player.GetComponentsInChildren<MonoBehaviour>();
        foreach (MonoBehaviour script in playerScripts)
        {
            if (script != this)
                script.enabled = false;
        }

        // ?? Stop all audio sources
        AudioSource[] allAudioSources = Object.FindObjectsByType<AudioSource>(FindObjectsSortMode.None);
        foreach (AudioSource audio in allAudioSources)
        {
            audio.Stop();
        }

        // ?? Unlock cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // ? Pause game
        Time.timeScale = 0f;

        // ?? Show UI
        if (missionCompleteUI != null)
            missionCompleteUI.SetActive(true);
    }

    // ?? Called by "Home" button
    public void OnHomeButton()
    {
        Time.timeScale = 1f; // unfreeze
        SceneManager.LoadScene("MainMenu"); // change to your actual main menu scene name
    }
}
