using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public Text countText;
    public Text winText;
    public Text livesText;
    public Text timeText;
    public Button restart;
    public float jumpForce;

    private Rigidbody rb;
    private int count;
    private int lives;
    private int total_yellow;
    private float jump;
    private float previousTime;

    private void Awake()
    {
        count = 0;
        total_yellow = 0;
        lives = 3;
        jump = 0.0f;
        winText.text = "";
    }
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        SetCountText();
        previousTime = -Time.time;
        restart.gameObject.SetActive(false);

    }
    private void Update()
    {
        if (Input.GetKeyDown("space") && GetComponent<Transform>().position.y < 3)
        {
            jump = jumpForce;
        }
        else
        {
            jump = 0.0f;
        }
        if (lives > 0 && winText.text == "")
        {
            timeText.text = (Time.time + previousTime).ToString("F2");
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 movement;
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        movement = new Vector3(moveHorizontal, jump, moveVertical);
        if (lives > 0 && winText.text == "") {
            rb.AddForce(movement * speed);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pick Up"))
        {
            other.gameObject.SetActive(false);
            count++;
            total_yellow++;
            SetCountText();
        }
        if (other.gameObject.CompareTag("Bad Item"))
        {
            other.gameObject.SetActive(false);
            if (count > 0)
            {
                count--;
            }
            StartCoroutine(FadeText(.5f, winText, "-1"));
            lives--;
            SetCountText();
        }
        if (other.gameObject.CompareTag("Death"))
        {
            lives = 0;
            SetCountText();
        }
        if (other.gameObject.CompareTag("Health"))
        {
            other.gameObject.SetActive(false);
            lives++;
            StartCoroutine(FadeText(1.5f, winText, "+1"));
            SetCountText();
        }
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
        livesText.text = "Lives: " + lives.ToString();
        if (total_yellow >= 16)
        {
            winText.text = "You Win!";
            restart.gameObject.SetActive(true);

        }
        else if(lives == 0)
        {
            winText.text = "You Lose!";
            restart.gameObject.SetActive(true);
        }
    }
    IEnumerator FadeText(float t, Text i, string s)
    {
        i.text = s;
        i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
        while (i.color.a > 0.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
            yield return null;
        }
        i.text = "";
        i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
    }
    public void Restart()
    {
        SceneManager.LoadScene(0);
        previousTime = Time.time;
    }
}
