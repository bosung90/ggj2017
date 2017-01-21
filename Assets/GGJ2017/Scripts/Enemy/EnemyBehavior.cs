using UnityEngine;

public class EnemyBehavior : MonoBehaviour {

    public Transform Player;
    private Vector3 playerFeet;
    private Animator anim;
    // Enemy Info
    public float MoveSpeed = 0.6f;
    public int AttackRange = 6;
    public int MinRange = 4;
    public int AttackDamage = 1;
    //private int Health = 2;

    void Awake() {
        anim = GetComponent<Animator>();
    }

	// Use this for initialization
	void Start () {
        anim.SetBool("Moving", true);
        anim.SetBool("Running", true);
    }
	
	// Update is called once per frame
	void Update () {
		
        // Make sure that player exists
        if(Player != null)
        {
            playerFeet = new Vector3(Player.position.x, 0, Player.position.z);

            // Always look at Player
            transform.LookAt(playerFeet);

            /****
            ** If enemy is close by, then attack
            */
            if (Vector3.Distance(transform.position, playerFeet) <= AttackRange)
            {
                if (anim.GetBool("Moving") && anim.GetBool("Running")) {
                    anim.SetBool("Moving", false);
                    anim.SetBool("Running", false);
                }

                // So it doesn't go inside the player
                if (Vector3.Distance(transform.position, playerFeet) <= MinRange)
                {
                    transform.position = transform.position = (transform.position - playerFeet).normalized * MinRange + playerFeet;
                }

                
                anim.SetTrigger("Attack1Trigger");
            } else
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

    //// Function for taking damage
    //void TakeDamage(int damage)
    //{
    //    Debug.Log("Ahhh it hurts took " + damage);
    //    this.Health -= damage;
    //}

    //// Function for dying
    //void Death()
    //{

    //}

    //// Function for exploding
    //void Explode()
    //{

    //}
}
