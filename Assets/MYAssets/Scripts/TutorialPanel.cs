using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialPanel : MonoBehaviour
{
    [SerializeField] private GameObject tutTxtForLvl1;
    [SerializeField] private GameObject tutTxtForLvl2;
   



    public void ShowTutForLvl1()
    {
        tutTxtForLvl2.SetActive(false);
        tutTxtForLvl1.SetActive(true);
    }
    public void ShowTutForLvl2()
    {
        tutTxtForLvl1.SetActive(false);
        tutTxtForLvl2.SetActive(true);
    }
}
