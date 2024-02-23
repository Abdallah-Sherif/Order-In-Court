using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.UIElements;

public class Objective : MonoBehaviour
{
    [SerializeField] int numberOfObjectivesToComplete;
    private int objectivesCompleted = 0;
    [SerializeField] UnityEvent onObjectiveComplete;
    [SerializeField] bool findEnemies;
    [SerializeField] PlayableDirector pd;
    [SerializeField] Transform p_model;
    [SerializeField] TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        if(findEnemies) 
        {
            FindNumberOfEnemies();
        }
    }

    bool isdone = false;
    // Update is called once per frame
    void Update()
    {
        if (pd.duration == pd.time && !isdone)
        {
            p_model.GetComponent<Camera>().enabled = true;

            isdone = true;
        }
        text.text = "Destroy the companies computer" +
            " to lower their stock price! \n" +
            " Computers crashed: " + objectivesCompleted + "/" + numberOfObjectivesToComplete;
    }
    private void FindNumberOfEnemies()
    {
        numberOfObjectivesToComplete = GameObject.FindGameObjectsWithTag("Enemie").Length;
    }
    public void CompleteAnObjective()
    {
        objectivesCompleted++;
        if(objectivesCompleted >=numberOfObjectivesToComplete) 
        {
            onObjectiveComplete?.Invoke();
            complete();
        }
    }
    public void complete()
    {
        p_model.GetComponent<Camera>().enabled = false;

        Debug.Log("Complete");
    }
}
