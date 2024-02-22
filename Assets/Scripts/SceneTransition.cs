using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] string scene;
    [SerializeField] Color color;
    [SerializeField] float speed;
    // Start is called before the first frame update
    public void SwitchScene()
    {
        Initiate.Fade(scene,color,speed);
    }
}
