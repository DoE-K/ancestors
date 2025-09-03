using System.Collections.Generic;

[System.Serializable]
public class WorldItemData
{
    public string prefabName;
    public float posX, posY, posZ;
    public float rotX, rotY, rotZ;
}

[System.Serializable]
public class PlayerData
{
    public float hunger;
    public float thirst;
    public string rightHandItemSave;
    public string leftHandItemSave;
    public float[] position; // x,y,z

    public List<WorldItemData> worldItems = new List<WorldItemData>();
}
