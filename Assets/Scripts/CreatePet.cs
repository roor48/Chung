using UnityEngine;

public class CreatePet : MonoBehaviour
{
    public GameObject[] pets;
    public int petIndex;
    public void MakePet()
    {
        if (petIndex >= pets.Length)
        {
            GameManager.Instance.Score += 500;
            return;
        }
        
        pets[petIndex++].SetActive(true);
        PlayerStats.Instance.petCnt = petIndex;
    }
}
