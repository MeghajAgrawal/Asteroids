using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid_Behavior : MonoBehaviour
{
    public float maxSpeed;
    public float maxSpin;
    public Rigidbody2D rigidbody2D;
    public int asteroidSize;
    public GameObject MediumA;
    public GameObject SmallA;
    public GameObject explosion;

    public int asteroidsPoints;
    public GameObject player;

    public LevelManager levelManager;
    // Start is called before the first frame update
    void Start()
    {
        Vector2 speed = new Vector2(Random.Range(-maxSpeed,maxSpeed),Random.Range(-maxSpeed,maxSpeed));
        float spin = Random.Range(-maxSpin,maxSpin);

        rigidbody2D.AddForce(speed);
        rigidbody2D.AddTorque(spin);

        player = GameObject.FindWithTag("Player");
        levelManager = GameObject.FindObjectOfType<LevelManager>();

    }

    // Update is called once per frame
    void Update()
    {
        var position = Camera.main.WorldToScreenPoint(transform.position);
        if(position.y > Screen.height){
            position.y =  0;
        }
        if(position.y < 0){
            position.y =  Screen.height;
        }
        if(position.x > Screen.width){
            position.x =  0;
        }
        if(position.x < 0){
            position.x =  Screen.width;
        }
        var newposition = Camera.main.ScreenToWorldPoint(position);
        transform.position = newposition;
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Bullet")){
            Destroy(other.gameObject);
            if(asteroidSize == 3){
                levelManager.UpdateAsteroidCount(1);
                Instantiate(MediumA,transform.position,transform.rotation);
                Instantiate(MediumA,transform.position,transform.rotation);
            }
            else if(asteroidSize == 2){
                levelManager.UpdateAsteroidCount(1);
                Instantiate(SmallA,transform.position,transform.rotation);
                Instantiate(SmallA,transform.position,transform.rotation);
            }
            else if(asteroidSize == 1){
                levelManager.UpdateAsteroidCount(-1);
            }
            player.SendMessage("scorePoints",asteroidsPoints);
            GameObject exp = Instantiate(explosion,transform.position,transform.rotation);
            Destroy(gameObject);
            Destroy(exp,1.0f);
        }
        

    }
}
