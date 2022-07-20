using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    // PARAMETERS - for tuning, typically set in the editor
    // CACHE - e.g. references for readability or speed
    // STATE - private instance (member) variables

    [SerializeField] float levelLoadDelay = 2f;
    [SerializeField] AudioClip crashSFX;
    [SerializeField] AudioClip finishSFX;
    [SerializeField] ParticleSystem crashVFX;
    [SerializeField] ParticleSystem finishVFX;

    AudioSource audioSource;

    bool isTransitioning = false;
    // bool isCollisionDisabled = false; //GOD MODE

    void Start() 
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update() 
    {
        //RespondToDebugKeys(); //GOD MODE
    }
    /* GOD MODE
    void RespondToDebugKeys()
    {
        if(Input.GetKey(KeyCode.L))
        {
            GoToNextLevel();
        }
        else if(Input.GetKey(KeyCode.C))
        {
            isCollisionDisabled = !isCollisionDisabled; //toggle collision
            Debug.Log("cheat Active");
        }
    }
    */

    void OnCollisionEnter(Collision other) 
    {
        // if(isTransitioning || !isCollisionDisabled) //GOD MODE
        if(isTransitioning)
        {
            return;
        }

        switch(other.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("Touch Friendly");
                break;
            case "Fuel":
                Debug.Log("You pick the fuel");
                break;
            case "Finish":
                FinishSequence();
                break;
            default:
                StartCrashSequence();
                break;
        }
    }

    void StartCrashSequence()
    {
        isTransitioning = true;
        audioSource.Stop();
        audioSource.PlayOneShot(crashSFX);
        crashVFX.Play();
        GetComponent<Movement>().enabled = false;
        Invoke("ReloadLevel", levelLoadDelay);
    }

    void FinishSequence()
    {
        isTransitioning = true;
        audioSource.Stop();
        audioSource.PlayOneShot(finishSFX);
        finishVFX.Play();
        GetComponent<Movement>().enabled = false;
        Invoke("GoToNextLevel", levelLoadDelay);
    }

    void GoToNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if(nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
    }

    void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }


}
