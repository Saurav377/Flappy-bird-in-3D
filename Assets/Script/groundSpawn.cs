using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class groundSpawn : MonoBehaviour
{
    public Transform spawnPoint;
    public GameObject ground;
    public float moveSpeed = 10f;
    birdScript bird;

    // Start is called before the first frame update
    void Start()
    {
        bird = GameObject.FindWithTag("Bird").GetComponent<birdScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!bird.dead)
        {
            transform.Translate(0, 0, -moveSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bird")
        {
            Instantiate(ground, spawnPoint.position, spawnPoint.rotation);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Bird")
        {
            Destroy(this.gameObject);
        }
    }
}
