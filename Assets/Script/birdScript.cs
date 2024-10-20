using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.EventSystems; // Add this for UI interaction

public class birdScript : MonoBehaviour
{
    public float jumpForce = 5f; // Adjustable jump force
    public float fallMultiplier = 2.5f; // Fall speed multiplier
    public float rotateSpeed = 2f; // Speed of rotation adjustment
    Rigidbody rb; // Reference to the Rigidbody component
    public float moveSpeed = 5f;
    public bool dead = false;
    public TMP_Text scoreText, highScoreText;
    public int score = 0;
    public GameObject gameOver, pauseMenu, fpsCam, Cam, startMenu, pauseButton, beak, switchCamButton;
    int highScore;
    bool paused = false;
    static bool gameStarted;
    AudioSource audio;
    public AudioClip crash;
    public Animator anim1, anim2;
    static bool fps;
    SphereCollider sphereCollider;

    // Start is called before the first frame update
    void Start()
    {

        Application.targetFrameRate = 60;
        rb = GetComponent<Rigidbody>();
        gameOver.SetActive(false);
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        if (!gameStarted)
        {
            startMenu.SetActive(true);
            Time.timeScale = 0;
            pauseButton.SetActive(false);
        }
        audio = gameObject.GetComponent<AudioSource>();
        sphereCollider = GetComponent<SphereCollider>();
        if (fps)
        {
            FpsCam();
        }
        else
        {
            TpsCam();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!dead) // Only allow movement when the bird is alive
        {
            if (gameStarted)
            {
                // Detect mouse click for jumping (ignore clicks on UI elements)
                if (Input.GetMouseButtonDown(0) && !IsPointerOverUI())
                {
                    Jump();
                }

                // Detect touch input for jumping (Android support) (ignore touches on UI elements)
                if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && !IsPointerOverUI())
                {
                    Jump();
                }
            }

            // Apply extra gravity when falling to mimic quick drop like in Flappy Bird
            if (rb.velocity.y < 0)
            {
                rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
                if (!fps)
                {
                    // Rotate the bird downwards while falling
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(30f, transform.eulerAngles.y, transform.eulerAngles.z), rotateSpeed * Time.deltaTime);
                }
                else
                {
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(30f, transform.eulerAngles.y, transform.eulerAngles.z), rotateSpeed/2 * Time.deltaTime);
                }
            }
            else if (rb.velocity.y > 0) // When jumping
            {
                if (!fps)
                {
                    // Rotate the bird upwards while jumping
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(-30f, transform.eulerAngles.y, transform.eulerAngles.z), rotateSpeed * Time.deltaTime);
                }
                else
                {
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(-30f, transform.eulerAngles.y, transform.eulerAngles.z), rotateSpeed/2 * Time.deltaTime);
                }
            }
            
        }
        else
        {
            // When dead, remove freeze rotation constraints on the Rigidbody
            rb.constraints = RigidbodyConstraints.None; // Disable all rotation constraints
            gameOver.SetActive(true);
            if (score > highScore)
            {
                highScore = score;
                PlayerPrefs.SetInt("HighScore", highScore);
            }
            highScoreText.text = "High Score: " + highScore.ToString();
            // Detect mouse click for jumping (ignore clicks on UI elements)
            if (Input.GetMouseButtonDown(0))
            {
                Retry();
            }

            // Detect touch input for jumping (Android support) (ignore touches on UI elements)
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && !IsPointerOverUI())
            {
                Retry();
            }
            switchCamButton.SetActive(false);  
        }

        // Pause functionality
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }

        // Toggle between first-person and third-person views
        if (Input.GetKeyDown("v"))
        {
            SwitchCam();
        }
    }

    void Jump()
    {
        // Apply a consistent upward force to the bird each time
        rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
        anim1.SetTrigger("fly");
        anim2.SetTrigger("fly"); ;
        audio.Play();
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag != "limiter")
        {
            dead = true;
            audio.PlayOneShot(crash);
        }
    }

    public void ScoreUp()
    {
        score++;
        scoreText.text = score.ToString();
    }

    public void Retry()
    {
        SceneManager.LoadScene("Game");
    }

    public void StartGame()
    {
        Time.timeScale = 1;
        startMenu.SetActive(false);
        gameStarted = true;
        pauseButton.SetActive(true);
    }

    public void PauseGame()
    {
        if (!dead)
        {
            if (!paused)
            {
                Time.timeScale = 0;
                paused = true;
                pauseMenu.SetActive(true);
            }
            else
            {
                Time.timeScale = 1;
                paused = false;
                pauseMenu.SetActive(false);
            }
        }
    }

    // Check if the pointer is over a UI element
    bool IsPointerOverUI()
    {
        // For mouse input
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return true;
        }

        // For touch input
        if (Input.touchCount > 0 && EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
        {
            return true;
        }

        return false;
    }

    void FpsCam()
    {
        fps = true;
        fpsCam.SetActive(true);
        Cam.SetActive(false);
        beak.SetActive(false);
        Quaternion currentRotation = transform.rotation;
        Quaternion newRotation = Quaternion.Euler(0, currentRotation.eulerAngles.y, currentRotation.eulerAngles.z);
        transform.rotation = newRotation;
        RenderSettings.fog = true;
        sphereCollider.radius = 0.7f;
    }

    void TpsCam()
    {
        fps = false;
        fpsCam.SetActive(false);
        Cam.SetActive(true);
        beak.SetActive(true);
        RenderSettings.fog = false;
        sphereCollider.radius = 1f;
    }

    public void SwitchCam()
    {
        if (!dead)
        {
            if (fps)
            {
                TpsCam();
            }
            else
            {
                FpsCam();
            }
        }
    }
}
