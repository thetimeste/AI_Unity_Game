using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class PopulationManagerSenses : MonoBehaviour
{

    public GameObject botPrefab;
    int populationSize = 10;
    List<GameObject> populationList = new List<GameObject>();
    public static float elapsed = 0;
    static float trialTime = 10;
    int generation = 1;
    public static PopulationManagerSenses instance;
    public GameObject mainCanvas;
    public GameObject TimerGO;
    TextMeshProUGUI TimerText;
    Vector3 spawnPosition;

    private void Awake()
    {
        spawnPosition = this.transform.position;
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
        SpawnNewPopulation(populationSize, generation);
    }


    public void SpawnNewPopulation(int populationSize, int generation)
    {

        //spawn population
        for (int i = 0; i < populationSize; i++)
        {
            Vector3 pos = new Vector3(
                this.transform.position.x + Random.Range(-5f, 5f),
                this.transform.position.y,
                this.transform.position.z + Random.Range(-5f, 5f));
            GameObject go = Instantiate(botPrefab, pos, this.transform.rotation); //spawn person
            go.GetComponent<BrainSenses>().InitializeDNA();

            populationList.Add(go); //add person on population list

        }
        ChatManager.instance.AddChatEvent(("GENERATION: " + generation), Color.black);

    }

    public void SpawnEvolvedPopulation()
    {
        if (populationList.Count > 0)
        {
            List<GameObject> newPopulation = new List<GameObject>();
            //delete unfit persons
            //the last on the list are the one that died later.
            List<GameObject> fitPopulationList = populationList.OrderBy(o => o.GetComponent<BrainSenses>().timeWalking)
                                                               .ThenBy(o => o.GetComponent<BrainSenses>().timeAlive)
                                                               .ToList();

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

    }
    public GameObject MixPopulation(GameObject parent1, GameObject parent2)
    {
        Vector3 pos = new Vector3(
                this.transform.position.x + Random.Range(-5f, 5f),
                this.transform.position.y,
                this.transform.position.z + Random.Range(-5f, 5f));

        GameObject child = Instantiate(botPrefab, pos, Quaternion.identity); //spawn person
        BrainSenses BrainSenses = child.GetComponent<BrainSenses>();

        //chance of mutation 1% -> the child can inherit from their parents or do a mutation
        if (Random.Range(1, 101) != 1)
        {
            //it swaps the dna randomly
            BrainSenses.InitializeDNA();
            BrainSenses.dna.MixDna(parent1.GetComponent<BrainSenses>().dna, parent2.GetComponent<BrainSenses>().dna);
        }
        else
        {
            BrainSenses.InitializeDNA();
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
