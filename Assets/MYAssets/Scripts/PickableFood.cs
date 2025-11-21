using UnityEngine;

public class PickableFood : MonoBehaviour
{
    public int value = 1;

    // Bounce settings
    public float bounceHeight = 0.2f;
    public float bounceSpeed = 2f;
    public ParticleSystem pickedUpPs;
    public AudioClip pickUpSfx;
    public AudioSource audioSource;



    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float offset = Mathf.Sin(Time.time * bounceSpeed) * bounceHeight;
        transform.position = startPos + Vector3.up * offset;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.CollectFood(value);

            GetComponent<Collider>().enabled = false;   
            GetComponent<MeshRenderer>().enabled = false;

            pickedUpPs.Play();
            audioSource.PlayOneShot(pickUpSfx);
            Destroy(gameObject, .5f);
        }
    }
}
