using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float mainThrust = 1000;
    [SerializeField] float rotationThrust = 100;
    [SerializeField] AudioClip thrustSFX;
    [SerializeField] ParticleSystem rightboostVFX;
    [SerializeField] ParticleSystem leftboostVFX;
    [SerializeField] ParticleSystem mainboostVFX;


    Rigidbody rb;
    AudioSource audioSource;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessThrust();
        ProcessRotation();
    }

    void ProcessThrust()
    {
        if(Input.GetKey(KeyCode.Space))
        {
            StartThrusting();
        }
        else
        {
            StopThrusting();
        }
    }

    void StartThrusting()
    {
        rb.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
        if (!mainboostVFX.isPlaying)
        {
            mainboostVFX.Play();
        }
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(thrustSFX);
        }
    }
    void StopThrusting()
    {
        audioSource.Stop();
        mainboostVFX.Stop();
    }

    void ProcessRotation()
    {
        if(Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            StartRotateLeft();
        }
        else
        {
            StopRotateLeft();
        }
        if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
        {
            StartRotateRight();
        }
        else
        {
            StopRotateRight();
        }
    }

    void StartRotateLeft()
        {
            ApplyRotation(rotationThrust);
            if (!rightboostVFX.isPlaying)
            {
                rightboostVFX.Play();
            }
        }
    void StopRotateLeft()
    {
        rightboostVFX.Stop();
    }

    void StartRotateRight()
    {
        ApplyRotation(-rotationThrust);
        if (!leftboostVFX.isPlaying)
        {
            leftboostVFX.Play();
        }
    }
    void StopRotateRight()
    {
        leftboostVFX.Stop();
    }

    void ApplyRotation(float rotationThisFrame)
    {
        rb.freezeRotation = true; // freezing rotation so we can maually rotate
        transform.Rotate(Vector3.forward * Time.deltaTime * rotationThisFrame);
        rb.freezeRotation = false; // unfreezing rotation so the physics system can take over
    }
}
