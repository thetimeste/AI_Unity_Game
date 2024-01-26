using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class Brain : MonoBehaviour
{
    public int dnaLength = 1;
    public float timeAlive;
    public Vector3 deathPosition;
    public DNAWalker dna;

    private ThirdPersonCharacter character;
    private Vector3 moveSettings;

    private bool alive = true;

    
    //make character die
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "dead")
        {
            deathPosition = gameObject.transform.position;
            alive = false;
            gameObject.SetActive(false);
        }
    }

    public void InitializeDNA()
    {
        //0 forward
        //1 backward
        //2 right
        //3 left
        //4 jump
        //5 crouch
        dna = new DNAWalker(dnaLength, 6);
        character = GetComponent<ThirdPersonCharacter>();
        timeAlive = 0;
        alive = true;
    }

    //fixed update is called in sync with physics
    private void FixedUpdate()
    {
         
        float verticalMove = 0;
        float horizontalMove = 0;
        bool crouch = false;
        bool jump = false;

        int dnaGene = dna.GetGene(0); //caching the dna value
        if (dnaGene == 0) { verticalMove = 1; } //if the gene is 0 move forward 
        if (dnaGene == 1) { verticalMove = -1; } //if the gene is 1 move backwards
        if (dnaGene == 2) { horizontalMove = 1; } //if the gene is 2 move right
        if (dnaGene == 3) { horizontalMove = -1; } //if the gene is 3 move left
        if (dnaGene == 4) { jump = true; } //if the gene is 4 jump
        if (dnaGene == 5) { crouch=true; } //if the gene is 5 crouch

        //apply motion to the character
        moveSettings = verticalMove * Vector3.forward + horizontalMove * Vector3.right;
        character.Move(moveSettings , crouch, jump);
        jump = false;
        if (alive)
        {
            deathPosition = gameObject.transform.position;
            timeAlive += Time.deltaTime;
        }
    }
}
