using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextManager : MonoBehaviour
{
    [SerializeField] GameObject textBox;
    [SerializeField] Dialogue dialogueControl;
    [SerializeField] PlayerManager pm;

    [SerializeField] private bool hideTextOnStart = true;

    [SerializeField] private List<string> nextTextList;

    // Start is called before the first frame update
    void Start()
    {
        if (textBox == null)
        {
            textBox = GameObject.FindGameObjectWithTag("Dialogue");
        }
        
        dialogueControl = FindObjectOfType<Dialogue>();
        pm = FindObjectOfType<PlayerManager>();

        if (dialogueControl == null)
        {
            return;
        }

        if (hideTextOnStart)
        {
            EndText();
        }
        else
        {
            StartText();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartText()
    {
        pm.canMove = false;
        pm.canSwitch = false;
        textBox.SetActive(true);
        dialogueControl.StartDialogue();
    }

    public void EndText()
    {
        pm.canMove = true;
        pm.InteractCooldownActivePlayer(); // ADAM -- This prevented the dialog zone form closing properly
        Invoke(nameof(UnblockInput), 0.1f);
        dialogueControl.ResetText();
        textBox.SetActive(false);        
    }

    public void StartNewText(List<string> newText)
    {
        pm.canMove = false;
        pm.canSwitch = false;
        textBox.SetActive(true);
        dialogueControl.lines = newText;
        dialogueControl.StartDialogue();
    }

    public void UnblockInput()
    {
        pm.canSwitch = true;
    }
}
