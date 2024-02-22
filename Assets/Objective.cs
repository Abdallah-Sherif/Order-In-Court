using System.Collections;
using System.Collections.Generic;
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
