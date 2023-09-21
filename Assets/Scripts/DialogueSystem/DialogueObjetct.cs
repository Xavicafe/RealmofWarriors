using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/DialgoqueObject")]
public class DialogueObjetct : ScriptableObject
{
    [SerializeField][TextArea] private string[] dialgoue;
    [SerializeField] private Response[] responses;
    public string[] Dialogue => dialgoue;

    public bool HasResponses => Responses != null && Responses.Length > 0;
    public Response[] Responses => responses;
}
