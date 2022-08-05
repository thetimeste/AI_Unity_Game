using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChatManager : MonoBehaviour
{
    public static ChatManager instance;
    public GameObject chatContainer;
    public GameObject chatPrefab;
    private void Awake()
    {

        // if the singleton hasn't been initialized yet
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;//Avoid doing anything else
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);

    }

    public void AddChatEvent(string chatString, Color color)
    {
        GameObject go = Instantiate(chatPrefab,chatContainer.transform);
        chatPrefab.GetComponent<TextMeshProUGUI>().text = chatString;
        chatPrefab.GetComponent<TextMeshProUGUI>().color = color;
    }
}
