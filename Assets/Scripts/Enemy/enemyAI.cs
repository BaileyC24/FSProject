using UnityEngine;
using System.Collections;

using UnityEngine.AI;

public class enemyAI : MonoBehaviour, IDamage
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Renderer model;
    [SerializeField] Transform shootPos;
    [SerializeField] Transform headPos;

    [SerializeField] int HP;
    [SerializeField] int faceTargetSpeed; //how fast we want him to turn, higher = faster
    [SerializeField] int FOV;
    [SerializeField] int roamDist; // Jan 21
    [SerializeField] int roamPauseTime; //

    [SerializeField] GameObject bullet;
    [SerializeField] float shootRate;

    [SerializeField] GameObject dropItem; //LEcture 5 example

    Color colorOrig;

    float shootTimer;
    float roamTimer;
    float angleToPlayer; //to compare to FOV
    float stoppingDistOrig;

    Vector3 playerDir;
    Vector3 startingPos;

    bool playerInTrigger; //is the player close enough to shoot?

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        colorOrig = model.material.color; //when we hit it, flash red, then back to original
                                          // gameManager.instance.updateGameGoal(1);   _now handled by spawner as of lecture 6
        startingPos = transform.position; //jan 21 lecture 5
        stoppingDistOrig = agent.stoppingDistance; //lecture 5
    }

    // Update is called once per frame
    void Update()
    {
        shootTimer += Time.deltaTime; //constantly going up, like with Player

        if (agent.remainingDistance < 0.01f) //added jan 21 lecture 5
            roamTimer += Time.deltaTime;

        // if(playerInTrigger && canSeePlayer()) _from previous lecture
        if (playerInTrigger && canSeePlayer())
        { //if the player is in the trigger and enemy can see them...
          //leaving this intentionally blank per instructor

            checkRoam();

            //we moved this down
            //playerDir = (gameManager.instance.player.transform.position - transform.position);

            //agent.SetDestination(gameManager.instance.player.transform.position);

            //if (agent.remainingDistance <= agent.stoppingDistance) //If I'm cloase enough to the player, manually rotate my body so I'm facing them
            //{
            //    faceTarget();
            //}

            //if (shootTimer >= shootRate) //it will never be = because floats have so many digits
            //{
            //    shoot();
            //}
        }
        else if (!playerInTrigger) //using Trigger instead of Range, because that's what we did previously in this term
        {
            checkRoam();
        }

    }

    void checkRoam() //lecture 5
    {
        if (agent.remainingDistance < 0.01f && roamTimer >= roamPauseTime)
        {
            roam();
        }
    }

    void roam() //lecture 5
    {
        roamTimer = 0;
        agent.stoppingDistance = 0;

        Vector3 ranPos = Random.insideUnitSphere * roamDist;
        ranPos += startingPos;

        NavMeshHit hit;
        NavMesh.SamplePosition(ranPos, out hit, roamDist, 1);
        agent.SetDestination(hit.position);
    }

    bool canSeePlayer() //most complicated part of class
    {
        playerDir = (gameManager.instance.player.transform.position - headPos.position);
        angleToPlayer = Vector3.Angle(playerDir, transform.forward);
        Debug.DrawRay(headPos.position, playerDir); //so we can see it

        RaycastHit hit;
        if (Physics.Raycast(headPos.position, playerDir, out hit))
        {

            if (angleToPlayer <= FOV && hit.collider.CompareTag("Player"))
            {
                agent.SetDestination(gameManager.instance.player.transform.position);

                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    faceTarget();
                }

                if (shootTimer >= shootRate)
                {
                    shoot();
                }

                agent.stoppingDistance = stoppingDistOrig; //jan 21 lecture 5
                return true;
            }
        }
        agent.stoppingDistance = 0; //lecture 5
        return false;

    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) //we don't want, say, the bullet to trigger it
            playerInTrigger = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInTrigger = false;
        agent.stoppingDistance = 0; //lecture 5
    }


    void faceTarget() //turn yourself to face things you care about (e.g., turnTowardPlayer)
    {                                       //formerly playerDir in ()  _replacing  transform.position.y with 0f to avoid twitching
        Quaternion rot = Quaternion.LookRotation(new Vector3(playerDir.x, 0f, playerDir.z)); //ignore player's Y position _could also set it to 0_ to help with the turning of char _avoids the twitching //we're having him look at a direction, which is a position minus a position
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * faceTargetSpeed); // (where we're at, where we're going, how quickly)

    }

    void shoot()
    {
        shootTimer = 0;

        Instantiate(bullet, shootPos.position, transform.rotation);
    }

    public void takeDamage(int amount)
    {
        HP -= amount; //the amount of damage that we sent in

        if (HP <= 0)
        {
          //  gameManager.instance.updateGameGoal(-1);  _commenting out until I can communicate with team about goal tracking system
            if (dropItem != null)
                Instantiate(dropItem, transform.position, transform.rotation);
            Destroy(gameObject); //this is like hiting Delete key on game object, it deletes it from scene
        }
        else //if he has some health
        {
            StartCoroutine(flashRed()); //this is how we can call flashRed(), we can't just call it itself
        }

    }

    IEnumerator flashRed() //a timer
    {
        model.material.color = Color.red; //turn red,
        yield return new WaitForSeconds(0.1f); // wait this amount of time and then...
        model.material.color = colorOrig; //return to original color. (This flash will be in every game)
    }
}
