using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Platformer
{
    public class PlayerController : MonoBehaviour
    {
        public float movingSpeed;
        public float jumpForce;
        private float moveInput;

        private bool facingRight = false;
        [HideInInspector]
        public bool deathState = false;

        private bool isGrounded;
        public Transform groundCheck;

        private Rigidbody2D rigidbody;
        private Animator animator;
        private GameManager gameManager;

        [Header("Audio")]
        public AudioClip jumpSound;
        public AudioClip coinSound;
        private AudioSource audioSource;

        void Start()
        {
            rigidbody = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            audioSource = GetComponent<AudioSource>();
        }

        private void FixedUpdate()
        {
            CheckGround();
        }

        void Update()
        {
            if (Input.GetButton("Horizontal"))
            {
                moveInput = Input.GetAxis("Horizontal");
                transform.Translate(Vector3.right * moveInput * movingSpeed * Time.deltaTime);
                animator.SetInteger("playerState", 1); // Run animation
            }
            else
            {
                if (isGrounded)
                    animator.SetInteger("playerState", 0); // Idle animation
            }

            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                rigidbody.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);

                // Tocar som de pulo
                if (jumpSound != null)
                {
                    audioSource.PlayOneShot(jumpSound);
                }
            }

            if (!isGrounded)
                animator.SetInteger("playerState", 2); // Jump animation 

            if (!facingRight && moveInput > 0)
            {
                Flip();
            }
            else if (facingRight && moveInput < 0)
            {
                Flip();
            }
        }

        private void Flip()
        {
            facingRight = !facingRight;
            Vector3 Scaler = transform.localScale;
            Scaler.x *= -1;
            transform.localScale = Scaler;
        }

        private void CheckGround()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.transform.position, 0.2f);
            isGrounded = colliders.Length > 1;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.tag == "Enemy")
            {
                deathState = true;
            }
            else
            {
                deathState = false;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            UnityEngine.Debug.Log("PlayerController detectou: " + other.name + " com tag: " + other.tag);

            if (other.gameObject.tag == "Coin")
            {
                gameManager.coinsCounter += 1;

                // Tocar som da moeda
                if (coinSound != null)
                {
                    audioSource.PlayOneShot(coinSound);
                }

                Destroy(other.gameObject);
            }

            if (other.gameObject.tag == "Princess")
            {
                UnityEngine.Debug.Log("PlayerController detectou Princess! Carregando youwin...");
                SceneManager.LoadScene("youwin");
            }
        }
    }
}
