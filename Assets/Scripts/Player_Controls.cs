using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player_Controls : MonoBehaviour
{
    public Rigidbody2D rigidbody2D;
    public float forward_thrust;
    public float rotation_speed;
    private float forwardInput;
    private float rotationInput;
    public float bulletSpeed;
    public GameObject bullet;

    public float spaceJump;
    public float tripleBullet;

    public float shield;

    // User Interface
    private int score;
    private float health;
    private int lives;
    public Healthbar health_bar;
    public Healthbar boost_bar;
    private float boost;
    public Text textScore;
    public Text textLife;
    public Text textTbullet;
    public Text textSpacejump;
    public Text textShield;
    public GameObject gopanel;
    public GameObject pausepanel;

    //Respawn Variables
    SpriteRenderer spriteRenderer;
    Collider2D collider2D;
    public Color cColor;
    public Color nColor;

    bool active;
    
    public GameObject explosion;
    public AudioSource audio;
    

    // Start is called before the first frame update
    void Start()
    {
        active = true;
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider2D = GetComponent<Collider2D>();
        score = 0;
        textScore.text = "Score : "+score; 
        health = 10f;
        lives = 3;
        boost = 10f;
        spaceJump = 0f;
        shield = 0f;
        
        tripleBullet = 0f;
        boost_bar.setMaxHealth(boost);
        textLife.text = "Lives : "+lives; 
        health_bar.setMaxHealth(health);
    }

    // Update is called once per frame
    void Update()
    {
        forwardInput = Input.GetAxis("Vertical");
        rotationInput = Input.GetAxis("Horizontal");
        if(spaceJump>0){
            spaceJump -= Time.deltaTime;
            textSpacejump.text = "SpaceJump : "+spaceJump;
            }
        if(tripleBullet>0){
            tripleBullet -= Time.deltaTime;
            textTbullet.text = "Triple Bullet : " + tripleBullet;
            }
        if(spaceJump <= 0){
            textSpacejump.text = "Space Jump : Ready";
        }
        if(tripleBullet <= 0){
            textTbullet.text = "Triple Bullet : Ready";
        }
        if(shield >0){
            shield -= Time.deltaTime;
            textShield.text = "Shield : "+shield;
            }
        if(shield <= 0){
            textShield.text = "Shield : Ready";
        }
        if(active){
        if(Input.GetButtonDown("Fire1")){
            GameObject bulletCreated = Instantiate(bullet,transform.position,transform.rotation);
            bulletCreated.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.up * bulletSpeed);
            Destroy(bulletCreated, 3.0f);
        }
        if(Input.GetButtonDown("SpaceJump")){
            if(spaceJump<=0){
            active = false;
            spriteRenderer.enabled = false;
            collider2D.enabled = false;
            Invoke("spaceJumping",1f);
            }
        }
        if(Input.GetButtonDown("TBullet")){
            if (tripleBullet <= 0){
            var rotation1 = transform.rotation * Quaternion.Euler(0, 30.0f, 0);
            var rotation2 = transform.rotation * Quaternion.Euler(0, -30.0f, 0);
            GameObject bulletCreated1 = Instantiate(bullet,transform.position,transform.rotation);
            GameObject bulletCreated2 = Instantiate(bullet,transform.position,rotation1);
            GameObject bulletCreated3 = Instantiate(bullet,transform.position,rotation2);
            bulletCreated1.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.up* bulletSpeed);
            bulletCreated2.GetComponent<Rigidbody2D>().AddRelativeForce((Quaternion.AngleAxis(30,transform.right) * transform.forward) * bulletSpeed);
            bulletCreated3.GetComponent<Rigidbody2D>().AddRelativeForce((Quaternion.AngleAxis(-30,transform.right) * transform.forward) * bulletSpeed);
            Destroy(bulletCreated1, 3.0f);
            Destroy(bulletCreated2, 3.0f);
            Destroy(bulletCreated3, 3.0f);
            tripleBullet = 5;
            textTbullet.text = "Triple Bullet : " + tripleBullet;
        }
        }
        if(Input.GetButtonDown("Shield")){
            if(shield<=0){
                spriteRenderer.color = cColor;
                collider2D.enabled = false;
                shield = 15;
                textShield.text = "Shield : "+shield; 
                Invoke("collision",2f);
            }
        }

        if( boost > 0 && Input.GetButton("Fire2")){
            
            boost -= Time.deltaTime;
            boost_bar.SetHealth(boost);
            forward_thrust = 8;
        }
        else{
            
            forward_thrust = 4;
            if(boost<10){
            boost +=Time.deltaTime;
            boost_bar.SetHealth(boost);
        }
        }
        if(Input.GetButton("Jump")){
            rigidbody2D.drag +=Time.deltaTime;
        }
        else{
            rigidbody2D.drag = 0.25f;
        }
        }
        
        if(Input.GetButtonDown("Cancel")){
            Time.timeScale = 0;
            pausepanel.SetActive(true);
        }

        transform.Rotate(Vector3.forward * rotationInput * Time.deltaTime * -rotation_speed);
        
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
    void FixedUpdate(){
        rigidbody2D.AddRelativeForce(Vector2.up * forwardInput * forward_thrust);  
    }
    void Respawm(){
        active = true;
        rigidbody2D.velocity = Vector2.zero;
        transform.position = Vector2.zero;
        
        spriteRenderer.enabled = true;
        spriteRenderer.color = cColor;
        
        health = 10f;
        health_bar.SetHealth(health);
        boost = 10f;
        boost_bar.setMaxHealth(boost);
        
        Invoke("collision",3f);
    }
    void collision(){
        spriteRenderer.color = nColor;
        collider2D.enabled = true;
    }
    void OnCollisionEnter2D(Collision2D collision){
        
        if(health > 0){
            audio.Play();
            health -= collision.relativeVelocity.magnitude; 
            health_bar.SetHealth(health);
        }
        if(health <= 0){
            lives -= 1;
            GameObject newexplode = Instantiate(explosion,transform.position,transform.rotation);
            Destroy(newexplode,2.0f);
            textLife.text = "Lives : "+lives; 

            spriteRenderer.enabled = false;
            collider2D.enabled = false;
            
            if(lives <=0){
                GameOver();
            }
            else{
                active = false;
                Invoke("Respawm",3f);
            }
            
        }
    }
    void OnTriggerEnter2D(Collider2D other){
        
    }
    void scorePoints(int points){
        score += points;
        textScore.text = "Score : "+score;
    }
    void GameOver(){
        active = false;
        gopanel.SetActive(!active);

    }

    public void restart(){
        SceneManager.LoadScene("SampleScene");
    }
    public void menu(){
        SceneManager.LoadScene("MenuScene");
    }

    void spaceJumping(){
        
        var tempPosition = Camera.main.WorldToScreenPoint(transform.position);
        tempPosition.x = Random.Range(25f,Screen.width-25f);
        tempPosition.y = Random.Range(25f,Screen.height-25f);
        var jumpPosition = Camera.main.ScreenToWorldPoint(tempPosition);
        transform.position = jumpPosition;
        
        active = true;
        spriteRenderer.enabled = true;
        collider2D.enabled = true;
        spaceJump = 5f;
        textSpacejump.text = "Space Jump : "+spaceJump;
    }
    public void Resume(){
        pausepanel.SetActive(false);
        Time.timeScale = 1;
    }
}
