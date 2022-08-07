using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class BrainSenses : MonoBehaviour
{
    int dnaLength = 2;
    
    public DNASenses dna;
    public GameObject eyes;
    bool seeGround = true;
    private bool alive = true;
    public float timeAlive;
    public float timeWalking;
    private ThirdPersonCharacter character;
    private Vector3 moveSettings;

    public GameObject ethanPrefab;
    GameObject ethan;

    private void OnDestroy()
    {
        Destroy(ethan);
    }

    //make character die
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "dead")
        {
            alive = false;
            gameObject.SetActive(false);
            OnDestroy();
        }
    }

    public void InitializeDNA()
    {
        //0 forward
        //1 right
        //2 left

        dna = new DNASenses(dnaLength, 3);
        character = GetComponent<ThirdPersonCharacter>();
        timeAlive = 0;
        alive = true;
        ethan = Instantiate(ethanPrefab, this.transform.position, this.transform.rotation);
        ethan.GetComponent<UnityStandardAssets.Characters.ThirdPerson.AICharacterControl>().target = this.transform;
    }

    //fixed update is called in sync with physics
    private void FixedUpdate()
    {
        if(!alive) { return; }
        //draw a red line from the eye where the bot is looking
        Debug.DrawRay(eyes.transform.position, eyes.transform.forward * 10, Color.red, 10);
        seeGround = false;


        //if the raycast hits the platform it means that the bot can proceed forward 
        RaycastHit raycastHit;
        if (Physics.Raycast(eyes.transform.position, eyes.transform.forward * 10,out raycastHit))
        {
            if (raycastHit.collider.gameObject.tag == "platform")
            {
                seeGround = true;
            }
        }
        timeAlive = PopulationManagerSenses.elapsed;

        float verticalMove = 0;
        float horizontalMove = 0;

        
        //if the bot can see the ground take the gene 1
        if (seeGround)
        {
            int dnaGene = dna.GetGene(0); //caching first gene value
            if (dnaGene == 0) { verticalMove = 1; timeWalking += Time.deltaTime; } //if the gene is 0 move forward 
            else if (dnaGene == 1) { horizontalMove = 90; } //if the gene is 1 turn right
            else if (dnaGene == 2) { horizontalMove = -90; } //if the gene is 2 turn left
        }
        else //if the bot can't see the ground take the gene 2
        {
            int dnaGene = dna.GetGene(1); //caching second gene value
            if (dnaGene == 0) { verticalMove = 1; timeWalking += Time.deltaTime; } //if the gene is 0 move forward 
            else if (dnaGene == 1) { horizontalMove = 90; } //if the gene is 1 turn right
            else if (dnaGene == 2) { horizontalMove = -90; } //if the gene is 2 turn left
        }

        //apply motion to the character
        this.transform.Translate(0, 0, verticalMove * 0.2f);
        this.transform.Rotate(0, horizontalMove, 0);
    }
}
