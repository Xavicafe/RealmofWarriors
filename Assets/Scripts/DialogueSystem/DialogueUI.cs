using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private GameObject dialogBox;
    [SerializeField] private TMP_Text textLabel;
    //[SerializeField] private DialogueObjetct testDialogue;

    public bool IsOpen { get; set; }

    private ResponseHandler responseHandler;
    private TypeWriter typewritterEffect;
    private void Start()
    {
        typewritterEffect= GetComponent<TypeWriter>();
        responseHandler= GetComponent<ResponseHandler>();

        CloseDialogBox();
        //ShowDialogue(testDialogue);
    }

    public void ShowDialogue(DialogueObjetct dialogueObject)
    {
        IsOpen= true;
        dialogBox.SetActive(true);
        StartCoroutine(StepThroughDialogue(dialogueObject));
    }

    private IEnumerator StepThroughDialogue(DialogueObjetct dialogueObject) 
    {      
        for (int i = 0; i < dialogueObject.Dialogue.Length; i++) 
        { 
            string dialogue = dialogueObject.Dialogue[i];
           
            yield return RunTypingEffect(dialogue);

            textLabel.text = dialogue;

            if ((i == dialogueObject.Dialogue.Length - 1)&& (dialogueObject.HasResponses))
                break;

            yield return null;
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        }

        if (dialogueObject.HasResponses) 
        {
            responseHandler.ShowResponses(dialogueObject.Responses);
        }
        else 
        {
            CloseDialogBox();
        }
    }

    private void CloseDialogBox() 
    {
        IsOpen = false;
        dialogBox.SetActive(false);
        textLabel.text = string.Empty;
    }

    private IEnumerator RunTypingEffect(string dialogue) 
    {
        typewritterEffect.Run(dialogue, textLabel);

        while (typewritterEffect.IsRunning) 
        {
            yield return null;
            if (Input.GetKeyDown(KeyCode.Space)) 
            {
                typewritterEffect.Stop();
            }
        }
    }

}
