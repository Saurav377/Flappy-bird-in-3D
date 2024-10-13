using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pillarScript : MonoBehaviour
{
    public float moveSpeed = 10f;
    birdScript bird;
    AudioSource audio;

    // Start is called before the first frame update
    void Start()
    {
        bird = GameObject.FindWithTag("Bird").GetComponent<birdScript>();
        audio = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!bird.dead)
        {
            transform.Translate(0, 0, -moveSpeed * Time.deltaTime);
        }
        if(transform.position.z < -10f)
        {
            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bird")
        {
            bird.ScoreUp();
            audio.Play();
        }
    }
}
