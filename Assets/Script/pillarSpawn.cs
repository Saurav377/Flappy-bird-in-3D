using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pillarSpawn : MonoBehaviour
{
    public GameObject[] pillar;
    int pillarno;
    birdScript bird;

    // Start is called before the first frame update
    void Start()
    {
        bird = GameObject.FindWithTag("Bird").GetComponent<birdScript>();
        StartCoroutine(Spawn()); // Start the coroutine to spawn pillars
    }

    // Update is called once per frame
    void Update()
    {
        // You no longer need to stop the coroutine here, so this can be empty
    }

    IEnumerator Spawn()
    {
        while (true)
        {
            if (!bird.dead) // Only spawn if the bird is alive
            {
                pillarno = Random.Range(0, 3);

                // Instantiate the selected pillar at the spawner's position
                Instantiate(pillar[pillarno],
                            new Vector3(pillar[pillarno].transform.position.x,
                                        pillar[pillarno].transform.position.y,
                                        transform.position.z),
                            transform.rotation);

                // Wait for 1.5 seconds before spawning the next pillar
                yield return new WaitForSeconds(1.5f);
            }
            else
            {
                // If the bird is dead, break the loop and stop spawning
                yield break;
            }
        }
    }
}
