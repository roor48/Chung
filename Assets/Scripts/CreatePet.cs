using UnityEngine;

public class CreatePet : MonoBehaviour
{
    public GameObject[] pets;
    public int petIndex;
    public void MakePet()
    {
        if (petIndex >= pets.Length)
            return;
        
        pets[petIndex++].SetActive(true);
    }
}
