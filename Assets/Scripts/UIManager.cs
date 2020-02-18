using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    static public UIManager instance;

    public GameObject buttonPrefab;
    public Transform contentScrollView;

    void Awake()
    {
        instance = this;
    }

    public void ClearButtonList()
    {
        foreach (Transform child in contentScrollView)
            Destroy(child);
    }

    public void InitAnimationPanel(GameObject instantiedCharacter, Character character)
    {
        ClearButtonList();

        foreach (CharacterInfo info in character.characterInfos)
        {
            GameObject btn = Instantiate(buttonPrefab, contentScrollView);

            if (info.image != null)
            {
                btn.GetComponent<Image>().sprite = info.image;
                btn.transform.GetChild(0).gameObject.SetActive(false);
            }
            else
            {
                btn.GetComponentInChildren<Text>().text = info.name;
                btn.transform.GetChild(0).gameObject.SetActive(true);
            }

            btn.GetComponent<Button>().onClick.AddListener(() => instantiedCharacter.GetComponent<Animator>().Play(info.name));
        }
    }
}
