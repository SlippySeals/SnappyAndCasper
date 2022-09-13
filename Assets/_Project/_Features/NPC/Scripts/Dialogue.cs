using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{
public InputRouter ip;

public TextMeshProUGUI textComponent;
public List<string> lines;
public float textSpeed;

    public TextManager tm;

private int index;

private bool buttonUp = true;

    // Start is called before the first frame update
    public void Start()
    {
        tm = FindObjectOfType<TextManager>();
        ip = FindObjectOfType<InputRouter>();
        textComponent.text = string.Empty;
    }

    // Update is called once per frame
    public void Update()
    {
        if (ip.doAttack)
        {
            tm.EndText();
        }

        if(ip.doInteract && buttonUp)
        {
            buttonUp = false;
            if(textComponent.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }

        if (buttonUp == false)
        {
            if (ip.doInteract == false)
            {
                buttonUp = true;
            }
        }
    }
    public void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            yield return new WaitForSeconds(textSpeed);
            textComponent.text += c;
        }
    }

    private void NextLine()
    {
        if (index < lines.Count - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            tm.EndText();
        }
    }

    public void ResetText()
    {
        Debug.Log("Text Reset");
        StopAllCoroutines();
        textComponent.text = string.Empty;
    }

    public void CloseText()
    {
        FindObjectOfType<PlayerMovement>().canMove = true;
        gameObject.SetActive(false);
    }
}
