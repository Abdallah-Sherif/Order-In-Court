using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChangeText : MonoBehaviour
{
    private TextMeshPro textMeshProObject;
    private HashSet<KeyCode> pressedKeys = new HashSet<KeyCode>();
    [SerializeField] GameObject pc;
    [SerializeField] Light spotLight;
    [SerializeField] Light bigLight;
    [SerializeField] GameObject Enemy;
    private bool canAttack = false, canDash = false, canJump = false, doneInputs = false, special1 = false, special2 = false;

    void Start()
    {
        textMeshProObject = GetComponent<TextMeshPro>();
    }

    void Update()
    {
        if (!doneInputs)
        {
            CheckWASD();
            CheckMouse();
            CheckShift();
            CheckSpace();
        }
        else
        {
            if (special1)
            {
                checkSpecialAttack1();
                if (special2)
                {
                    checkSpecialAttack2();
                    if (pc.GetComponent<Health>().health == 0)
                    {
                        ModifyText("I guess you are ready now");
                    }
                }
            }
        }
    }

    private void CheckKeyPress(KeyCode key)
    {
        if (Input.GetKeyDown(key) && !pressedKeys.Contains(key))
        {
            pressedKeys.Add(key);
        }
    }

    private void CheckWASD()
    {
        CheckKeyPress(KeyCode.W);
        CheckKeyPress(KeyCode.A);
        CheckKeyPress(KeyCode.S);
        CheckKeyPress(KeyCode.D);

        if (pressedKeys.Count == 4)
        {
            ModifyText("Good job! Now try using left and right click on your mouse.");
            canAttack = true;
        }
    }

    private void CheckMouse()
    {
        if (canAttack)
        {
            CheckKeyPress(KeyCode.Mouse0);
            CheckKeyPress(KeyCode.Mouse1);
            if (pressedKeys.Count == 6)
            {
                ModifyText("Great job! Now try using the Shift key to dash.");
                canDash = true;
            }
        }
    }

    private void CheckShift()
    {
        if (canDash)
        {
            CheckKeyPress(KeyCode.LeftShift);
            if (pressedKeys.Count == 7)
            {
                ModifyText("Nice! Now try using the space bar to jump.");
                canJump = true;
            }
        }
    }

    private void CheckSpace()
    {
        if (canJump)
        {
            CheckKeyPress(KeyCode.Space);
            if (pressedKeys.Count == 8)
            {
                ModifyText("Unlock the power of your special attack1! Press 'Q' and left-click to send your foes soaring into the skies. Let the chaos begin!");
                doneInputs = true;
                special1 = true;
                Enemy.SetActive(true);
            }
        }
    }

    private void checkSpecialAttack1()
    {
        if (Input.GetKey(KeyCode.Mouse0) && Input.GetKey(KeyCode.Q))
        {
            ModifyText("For the special attack 2, press 'Q' and right-click simultaneously to unleash a powerful strike with increased attack speed!");
            special2 = true;
        }
        /*else if(Enemy.GetComponent<Health>().health == 0)
        {
            ModifyText("Judiciously navigating the level, whether with the mighty special attack or a subtle approach, your honor shines. Onward, honorable judge!");
            StartCoroutine(Delay());
            ModifyText("For the special attack 2, press 'Q' and right-click simultaneously to unleash a powerful strike with increased attack speed!");
            special2 = true;
        }*/
    }

    private void checkSpecialAttack2()
    {
        if (Input.GetKey(KeyCode.Q) && Input.GetMouseButton(1))
        {
            StartCoroutine(special2Delay());
        }
    }
    IEnumerator special2Delay()
    {
        ModifyText("Explore the map – hunt down unique objectives like the golden PC and unleash destruction upon it!");
        if(Enemy != null) Enemy.SetActive(false);
        yield return new WaitForSeconds(3);
        pc.SetActive(true);
        bigLight.enabled = false;
        spotLight.enabled = true;
        yield return new WaitForSeconds(2);

    }
    IEnumerator Delay()
    {
        yield return new WaitForSeconds(3);
    }

    private void ModifyText(string newText)
    {
        textMeshProObject.text = newText;
    }
}
