using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(PhotonView))]
public class ChatBox : Photon.MonoBehaviour
{

    public Rect GuiRect = new Rect(0, 0, 323, 170);
    public bool IsVisible = true;
    public bool AlignBottom = true;
    public List<string> messages = new List<string>();
    private string inputLine = "";
    private Vector2 scrollPos = Vector2.zero;
    public static readonly string ChatRPC = "Chat";
    public void Start()
    {
        if (this.AlignBottom)
        {
            this.GuiRect.y = Screen.height - this.GuiRect.height;
        }
    }
    public void OnGUI()
    {
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
                return; // printing the now modified list would result in an error. to avoid this, we just skip this single frame
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


