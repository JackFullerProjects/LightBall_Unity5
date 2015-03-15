using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(PhotonView))]
public class ChatBox : Photon.MonoBehaviour
{

    public Rect GuiRect = new Rect(0, 0, 323, 170);
    public bool IsVisible;
    public bool AlignBottom;
    public List<string> messages = new List<string>();
    private string inputLine = "";
    private Vector2 scrollPos = Vector2.zero;
    public static readonly string ChatRPC = "Chat";


    //scaling
    private float native_width = 1920;
    private float native_height  = 1080;


    public void Start()
    {
        if (this.AlignBottom)
        {
            this.GuiRect.y = Screen.height - this.GuiRect.height;
        }
    }
    public void OnGUI()
    {
        float width = Screen.width / native_width;
        float height  = Screen.height / native_height;
        GUI.matrix = Matrix4x4.TRS(new Vector3(0, 0, 0), Quaternion.identity, new Vector3(width, height, 1)); 
        if (!this.IsVisible || PhotonNetwork.connectionStateDetailed != PeerState.Joined)
        {
            return;
        }
        if (Event.current.type == EventType.KeyDown && (Event.current.keyCode == KeyCode.KeypadEnter || Event.current.keyCode == KeyCode.Return))
        {
            if (!string.IsNullOrEmpty(this.inputLine))
            {
                this.photonView.RPC("Chat", PhotonTargets.All, this.inputLine);
                this.inputLine = "";
                GUI.FocusControl("");
                return;
            }
            else
            {
                GUI.FocusControl("ChatInput");
            }
        }
        GUI.SetNextControlName("");
        GUILayout.BeginArea(this.GuiRect);
        scrollPos = GUILayout.BeginScrollView(scrollPos);
        GUILayout.FlexibleSpace();
       
        for (int i = messages.Count - 1; i >= 0; i--)
        {
            GUILayout.Label(messages[i]);
        }

        GUILayout.EndScrollView();
        GUILayout.BeginHorizontal();
        GUI.SetNextControlName("ChatInput");
        inputLine = GUILayout.TextField(inputLine);
        GUILayout.EndHorizontal();
        GUILayout.EndArea();
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
    }


    [RPC]
    public void Chat(string newLine, PhotonMessageInfo mi)
    {
        string senderName = "anonymous";
        if (mi != null && mi.sender != null)
        {
            if (!string.IsNullOrEmpty(mi.sender.name))
            {
                senderName = mi.sender.name;
            }
            else
            {
                senderName = "Player " + mi.sender.ID;
            }
        }
        this.messages.Add(senderName + ": " + newLine);
    }
    public void AddLine(string newLine)
    {
        this.messages.Add(newLine);
    }
}


