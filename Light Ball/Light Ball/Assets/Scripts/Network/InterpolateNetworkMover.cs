using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InterpolateNetworkMover : Photon.MonoBehaviour {
    
    public double interpolationBackTime = 0.1;


    internal struct State
    {
        internal double timestamp;
        internal Vector3 pos;
        internal Quaternion rot;
    }

    State[] m_BufferedState = new State[20];
    int m_TimestampCount;

    private PhotonView pv;

    void Start()
    {
        pv = GetComponent<PhotonView>();

        if (pv.isMine)
        {
            int _teamBlueCount = 0;
            int _teamRedCount = 0;

            foreach (var teamName in PunTeams.PlayersPerTeam.Keys)
            {
                List<PhotonPlayer> teamPlayers = PunTeams.PlayersPerTeam[teamName];
                //Debug.Log("TEAM: " + teamName + "   Players: " + teamPlayers.Count);

                if (teamName == PunTeams.Team.red)
                    _teamRedCount = teamPlayers.Count;
                else if(teamName == PunTeams.Team.blue)
                    _teamBlueCount = teamPlayers.Count;

            }

            PhotonNetwork.player.SetTeam(PickTeam(_teamRedCount, _teamBlueCount));

            GetComponent<RigidbodyFirstPersonController>().enabled = true;
            GetComponent<Player>().enabled = true;
            GetComponent<PlayerShoot>().enabled = true;
            GetComponent<Rigidbody>().useGravity = true;
            GetComponent<Player>().GunAnimation.GetComponent<Animation>().enabled = true;

            foreach (Transform child in transform)
            {
                if (child.transform.name == "FirstPersonCharacter")
                {
                    child.GetComponent<Camera>().enabled = true;
                    child.GetComponent<AudioListener>().enabled = true;
                    child.GetComponent<HeadBob>().enabled = true;

                    foreach (Camera cam in child.GetComponentsInChildren<Camera>())
                    {
                        cam.enabled = true;
                        cam.cullingMask |= (1 << 8);
                    }
                }
            }
        }
    }

    private PunTeams.Team PickTeam(int teamRedNum, int teamBlueNum)
    {
        if (teamBlueNum > teamRedNum)
        {
            var playerScript = GetComponent<Player>();
            playerScript.team = PunTeams.Team.red;
            return PunTeams.Team.red;
        }
        else
        {
            var playerScript = GetComponent<Player>();
            playerScript.team = PunTeams.Team.blue;
            return PunTeams.Team.blue;
        }
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            Vector3 pos = transform.localPosition;
            Quaternion rot = transform.localRotation;
            stream.Serialize(ref pos);
            stream.Serialize(ref rot);
        }
        else
        {
            Vector3 pos = transform.position;
            Quaternion rot = Quaternion.identity;
            stream.Serialize(ref pos);
            stream.Serialize(ref rot);

            // Shift buffer contents, oldest data erased, 18 becomes 19, ... , 0 becomes 1
            for (int i = m_BufferedState.Length - 1; i >= 1; i--)
            {
                m_BufferedState[i] = m_BufferedState[i - 1];
            }

            // Save currect received state as 0 in the buffer, safe to overwrite after shifting
            State state;
            state.timestamp = info.timestamp;
            state.pos = pos;
            state.rot = rot;
            m_BufferedState[0] = state;

            // Increment state count but never exceed buffer size
            m_TimestampCount = Mathf.Min(m_TimestampCount + 1, m_BufferedState.Length);

            // Check integrity, lowest numbered state in the buffer is newest and so on
            for (int i = 0; i < m_TimestampCount - 1; i++)
            {
                if (m_BufferedState[i].timestamp < m_BufferedState[i + 1].timestamp)
                    Debug.Log("State inconsistent");
            }

            //Debug.Log("stamp: " + info.timestamp + "my time: " + PhotonNetwork.time + "delta: " + (PhotonNetwork.time - info.timestamp));
        }
    }

    // This only runs where the component is enabled, which is only on remote peers (server/clients)
    void Update()
    {

        if (!pv.isMine)
        {
            double currentTime = PhotonNetwork.time;
            double interpolationTime = currentTime - interpolationBackTime;
            // We have a window of interpolationBackTime where we basically play
            // By having interpolationBackTime the average ping, you will usually use interpolation.

            // Use interpolation
            if (m_BufferedState[0].timestamp > interpolationTime)
            {
                for (int i = 0; i < m_TimestampCount; i++)
                {
                    // Find the state which matches the interpolation time (time+0.1) or use last state
                    if (m_BufferedState[i].timestamp <= interpolationTime || i == m_TimestampCount - 1)
                    {
                        // The state one slot newer (<100ms) than the best playback state
                        State rhs = m_BufferedState[Mathf.Max(i - 1, 0)];

                        // The best playback state (closest to 100 ms old (default time))
                        State lhs = m_BufferedState[i];

                        double length = rhs.timestamp - lhs.timestamp;
                        float t = 0.0F;

                        if (length > 0.0001)
                            t = (float)((interpolationTime - lhs.timestamp) / length);

                        transform.localPosition = Vector3.Lerp(lhs.pos, rhs.pos, t);
                        transform.localRotation = Quaternion.Slerp(lhs.rot, rhs.rot, t);
                        return;
                    }
                }
            }
            else
            {
                State latest = m_BufferedState[0];
                transform.localPosition = latest.pos;
                transform.localRotation = latest.rot;
            }
        }
    }

}
