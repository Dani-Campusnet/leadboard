using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class DataManager : MonoBehaviour
{

    string urlGetInfo = "https://strackdev.com/runner/getleadboard.php";
    public UserInfo[] users;
    public Transform parentUsers;
    public GameObject userPrefab;
    
    public void GetInfoLeadboard()
    {
        StartCoroutine(cGetData());
    }
    public void PrintLeadboard()
    {
        for (int i = 0; i < users.Length; i++)
        {
            GameObject temp= Instantiate(userPrefab, parentUsers);
            temp.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text=users[i].username;
            temp.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = users[i].score.ToString();
        }
    }
    IEnumerator cGetData()
    {
        UnityWebRequest request = UnityWebRequest.Get(urlGetInfo);
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(request.error);
        }
        else
        {
            Debug.Log(request.downloadHandler.text);
            string jsonData = request.downloadHandler.text;
            RootUsers info = JsonUtility.FromJson<RootUsers>("{\"users\":" + jsonData + "}");
            users = info.users;
            PrintLeadboard();
        }
    }
}


[System.Serializable]
public class UserInfo
{
    public int id;
    public string username;
    public int score;
}
[System.Serializable]
public class RootUsers
{
    public UserInfo[] users;
}
