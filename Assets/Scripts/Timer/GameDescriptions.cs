using UnityEngine;

[CreateAssetMenu(fileName = "GameDescription", menuName = "Game/Game Descriptions")]
public class GameDescriptions : ScriptableObject
{
    [System.Serializable]
    public struct GameDescription
    {
        public string sceneName;
        [TextArea(3, 5)]
        public string descriptionText;
    }

    public GameDescription[] gameDescriptions;
}