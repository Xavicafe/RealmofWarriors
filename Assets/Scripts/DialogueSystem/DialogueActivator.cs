using UnityEngine;

public class DialogueActivator : MonoBehaviour, IInteractable
{
    [SerializeField] private DialogueObjetct dialogueObjetct;

   
    public void Interact(CharacterControl game)
    {
        game.DialogueUI.ShowDialogue(dialogueObjetct);
    }

    
}
