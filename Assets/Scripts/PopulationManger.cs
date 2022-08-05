using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class PopulationManger : MonoBehaviour
{
    bool respawningPopulation = false;
    public GameObject personPrefab;
    public int populationSize = 10;
    List<GameObject> populationList = new List<GameObject>();
    static float elapsed = 0;
    static float trialTime = 10;
    int generation = 1;
    public static PopulationManger instance;
    public GameObject mainCanvas;
    public GameObject TimerGO;
    TextMeshProUGUI TimerText;
    List<GameObject> collisionObjectList = new List<GameObject>();
    private void Awake()
    {
        TimerText = TimerGO.GetComponent<TextMeshProUGUI>();
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }
    }
    private void Start()
    {
        SpawnNewPopulation(populationSize,generation);
    }

 
    public void SpawnNewPopulation(int populationSize,int generation)
    {
        
        //spawn population
        for (int i = 0; i < populationSize; i++)
        {
            Vector2 pos = Camera.main.ViewportToWorldPoint(new Vector2(Random.value, Random.value));
            GameObject go = Instantiate(personPrefab, pos, Quaternion.identity, mainCanvas.transform); //spawn person
            go.GetComponent<DNA>().ChooseRandomColor();
            go.GetComponent<DNA>().ChooseRandomScale();
            populationList.Add(go); //add person on population list
  
        }
      
    }

    public void SpawnEvolvedPopulation()
    {
        if (populationList.Count > 0)
        {
            List<GameObject> newPopulation = new List<GameObject>();
            //delete unfit persons
            //the last on the list are the one that died later.
            List<GameObject> fitPopulationList = populationList.OrderBy(o => o.GetComponent<DNA>().deathTime).ToList();

            //clear population list
            populationList.Clear();

            //cuts the list in half and uses the bottom part --> the fittest are mixed together 
            for (int i = ((int)(fitPopulationList.Count / 2) - 1); i < fitPopulationList.Count - 1; i++)
            { 
                populationList.Add(MixPopulation(fitPopulationList[i], fitPopulationList[i + 1]));
                populationList.Add(MixPopulation(fitPopulationList[i + 1], fitPopulationList[i]));
            }

            for (int i = 0; i < fitPopulationList.Count; i++)
            {
                Destroy(fitPopulationList[i]);
            }
            generation++;
            ChatManager.instance.AddChatEvent(("GENERATION: " + generation), Color.black);
            
        }
        //StartCoroutine(DestroyCollisionListCorotuine());
    }
    public GameObject MixPopulation(GameObject parent1, GameObject parent2)
    {
        Vector2 pos = Camera.main.ViewportToWorldPoint(new Vector2(Random.value, Random.value));
        GameObject child = Instantiate(personPrefab, pos, Quaternion.identity, mainCanvas.transform); //spawn person
        DNA dnaParent1 = parent1.GetComponent<DNA>();
        DNA dnaParent2 = parent2.GetComponent<DNA>();

        //chance of mutation 10% -> the child can inherit from their parents or do a color mutation
        if (Random.Range(1, 11) > 1)
        {

            //it swaps the dna randomly
            //get color
            //if the random number is less or equal than 5 take dna the "r" value of parent 1 else take dna "r" value of parent 2
            child.GetComponent<DNA>().r = Random.Range(1, 11) <= 5 ? dnaParent1.r : dnaParent2.r;
            //if the random number is less or equal than 5 take dna the "g" value of parent 1 else take dna "g" value of parent 2
            child.GetComponent<DNA>().g = Random.Range(1, 11) <= 5 ? dnaParent1.g : dnaParent2.g;
            //if the random number is less or equal than 5 take dna the "b" value of parent 1 else take dna "b" value of parent 2
            child.GetComponent<DNA>().b = Random.Range(1, 11) <= 5 ? dnaParent1.b : dnaParent2.b;

            //get size
            //if the random number is less or equal than 5 get scale of parent 1 else get scale of parent 2
            child.GetComponent<DNA>().scale = Random.Range(1, 11) <= 5 ? dnaParent1.scale : dnaParent2.scale;
            

            //set the colours
            child.GetComponent<DNA>().SetColor();
            //set the scale
            child.GetComponent<DNA>().SetScale();
        }
        else
        {
            //color mutation
            child.GetComponent<DNA>().ChooseRandomColor();

            //scale mutation
            child.GetComponent<DNA>().ChooseRandomScale();

            ChatManager.instance.AddChatEvent(("MUTATION OCCURRED"), Color.cyan);
        }

        return child;
    }

   
    private void Update()
    {
        elapsed += Time.deltaTime;
        if (elapsed > trialTime)
        {
            elapsed = 0;
            SpawnEvolvedPopulation();
        }
        else
        {
            TimerText.text = elapsed.ToString("0.00");
        }
    }

}
