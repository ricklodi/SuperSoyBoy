using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    public GameObject playerDeathPrefab;
    public AudioClip deathClip;
    public Sprite hitSprite;
    private SpriteRenderer spriteRenderer;
    void Awake()
    {
        //caches a reference to the SpriteRenderer when the script starts.
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void OnCollisionEnter2D(Collision2D coll)
    {
        // check to ensure the colliding GameObject has the "Player" tag.
        if (coll.transform.tag == "Player")
        {
            // determine if an AudioClip has been assigned to the script and that a valid
            //Audio Source component exists. If so, play the deathClip audio effect
            var audioSource = GetComponent<AudioSource>();
            if (audioSource != null && deathClip != null)
            {
                audioSource.PlayOneShot(deathClip);
            }
            // Instantiate the playerDeathPrefab at the collision point and swap the sprite of the saw blade with the hitSprite version.
            Instantiate(playerDeathPrefab, coll.contacts[0].point,
            Quaternion.identity);
            spriteRenderer.sprite = hitSprite;
            // Lastly, destroy the colliding object (the player).
            Destroy(coll.gameObject);
            GameManager.instance.RestartLevel(1.25f);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
