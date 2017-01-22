using UnityEngine;

public class EnemyBehavior : MonoBehaviour {

    private Transform Player;
    private Vector3 playerFeet;
    private Animator anim;
    // Enemy Info
    public float MoveSpeed = 10;
    public int AttackRange = 4;
    public int MinRange = 3;
    public int AttackDamage = 1;
    //private int Health = 2;

    void Awake() {
        anim = GetComponent<Animator>();
        Player = GameObject.Find("Camera (eye)").transform;
    }

	// Use this for initialization
	void Start () {
        anim.SetBool("Moving", true);
        anim.SetBool("Running", true);
    }
	
	// Update is called once per frame
	void Update () {

        // Make sure that player exists
        if (Player != null)
        {
            playerFeet = new Vector3(Player.position.x, 0, Player.position.z);
            
            // Always look at Player
            transform.LookAt(playerFeet);

            /****
            ** If enemy is close by, then attack
            */
            if (Vector3.Distance(transform.position, playerFeet) <= AttackRange)
            {
                if (anim.GetBool("Moving") && anim.GetBool("Running"))
                {
                    anim.SetBool("Moving", false);
                    anim.SetBool("Running", false);
                }

                // So it doesn't go inside the player
                if (Vector3.Distance(transform.position, playerFeet) <= MinRange)
                {
                   transform.position = (transform.position - playerFeet).normalized * MinRange + playerFeet;
                }

                anim.SetTrigger("Attack1Trigger");

            }
            else
            // If enemy is not close, move towards player
            {
                // Assumes always looking at player and moves towards them
                //Vector3 forward = transform.forward;
                //forward.y = 0;
                transform.position += transform.forward * MoveSpeed * Time.deltaTime;

                //Vector3 pos = transform.position;
                //pos.y = 0;
                //transform.position = pos;

                if (!(anim.GetBool("Moving") && anim.GetBool("Running")))
                {
                    anim.SetBool("Moving", true);
                    anim.SetBool("Running", true);
                }

            }
            /*
            ** 
            ****/

        }
    }

    void AttackPlayer()
    {
        Debug.Log("Attack player!");
        GameObject.Find("Player").GetComponent<PlayerBehavior>().TakeDamage(AttackDamage);
    }

    //// Function for taking damage
    //void TakeDamage(int damage)
    //{
    //    Debug.Log("Ahhh it hurts took " + damage);
    //    this.Health -= damage;
    //}

    //// Function for Deleting Enemy
    public void DestroyEnemy()
    {
		GameManager.Instance.setScore (1.0f);
        Destroy(transform.gameObject);
    }

    //// Function for exploding
    //void Explode()
    //{

    //}
}
