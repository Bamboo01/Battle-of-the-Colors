using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;
using UnityEngine.UI;

// CSZZ - 褰╄壊鎴樹簤
namespace CSZZGame.Networking
{
    public class NetworkRoomPlayerScript : NetworkRoomPlayer
    {
        [SyncVar(hook = nameof(TeamStateChanged))]
        public bool team;

        [SyncVar(hook = nameof(IDChanged))]
        public int idTag = 1;

        public GameObject UIObject;
        public GameObject hostIcon;
        public GameObject removeIcon;
        public GameObject readyIcon;
        public TextMeshProUGUI playerTextLabel;

        public Transform team1Container;
        public Transform team2Container;

        #region Commands

        [Command]
        public void CmdChangeTeamState(bool teamState)
        {
            team = teamState;
        }

        #endregion

        #region SyncVar Hooks

        /// <param name="newTeamState">New Ready State</param>
        public virtual void TeamStateChanged(bool oldTeamState, bool newTeamState)
        {
            RPCSwapTeam(newTeamState);
        }
        /// <param name="newID">New Ready State</param>
        public virtual void IDChanged(int oldID, int newID)
        {
            playerTextLabel.text = "Player " + idTag;
        }
        //[ClientRpc]
        public void RPCSwapTeam(bool newTeamState)
        {
            UIObject.transform.SetParent(newTeamState ? team2Container : team1Container);
        }


        public override void ReadyStateChanged(bool oldReadyState, bool newReadyState)
        {
            base.ReadyStateChanged(oldReadyState, newReadyState);
            readyIcon.SetActive(newReadyState);

        }
        #endregion

        public override void OnStartLocalPlayer()
        {
            base.OnStartLocalPlayer();
            NetworkRoomManagerScript room = NetworkManager.singleton as NetworkRoomManagerScript;
            if (room)
            {
                GameController.localRoomPlayer = this;
                room.ReadyButton.onClick.AddListener(() => ReadyButtonClick());
                room.SwitchTeamButton.onClick.AddListener(() => TeamButtonClick());
            }
        }

        public override void OnClientEnterRoom()
        {
            base.OnClientEnterRoom();
            NetworkRoomManagerScript room = NetworkManager.singleton as NetworkRoomManagerScript;
            if (room)
            {
                team1Container = room.team1Container;
                team2Container = room.team2Container;
                RPCSwapTeam(team);

                hostIcon.SetActive(index == 0);
                if (((isServer && index > 0) || isServerOnly))
                {
                    removeIcon.GetComponent<Button>().onClick.AddListener(() => GetComponent<NetworkIdentity>().connectionToClient.Disconnect());
                    removeIcon.SetActive(true);
                }
                else
                {
                    removeIcon.SetActive(false);
                }
            }
        }

        public override void OnDisable()
        {
            base.OnDisable();
            Destroy(UIObject);
        }

        //public override void OnGUI()
        //{
        //    if (!showRoomGUI)
        //        return;

        //    NetworkRoomManager room = NetworkManager.singleton as NetworkRoomManager;
        //    if (room)
        //    {
        //        if (!room.showRoomGUI)
        //            return;

        //        if (!NetworkManager.IsSceneActive(room.RoomScene))
        //            return;

        //        DrawPlayerReadyState();
        //        DrawPlayerReadyButton();
        //    }
        //}

        public void ReadyButtonClick()
        {
            CmdChangeReadyState(!readyToBegin);
        }
        public void TeamButtonClick()
        {
            CmdChangeTeamState(!team);
            // lazy
        }

        //void DrawPlayerReadyState()
        //{
        //    GUILayout.BeginArea(new Rect(20f + (index * 100), 200f, 90f, 130f));

        //    GUILayout.Label($"Player [{index + 1}]");

        //    if (readyToBegin)
        //        GUILayout.Label("Ready");
        //    else
        //        GUILayout.Label("Not Ready");

        //    if (team)
        //        GUILayout.Label("Team 2");
        //    else
        //        GUILayout.Label("Team 1");


        //    if (((isServer && index > 0) || isServerOnly) && GUILayout.Button("REMOVE"))
        //    {
        //        // This button only shows on the Host for all players other than the Host
        //        // Host and Players can't remove themselves (stop the client instead)
        //        // Host can kick a Player this way.
        //        GetComponent<NetworkIdentity>().connectionToClient.Disconnect();
        //    }

        //    GUILayout.EndArea();
        //}

        //void DrawPlayerReadyButton()
        //{
        //    if (NetworkClient.active && isLocalPlayer)
        //    {
        //        GUILayout.BeginArea(new Rect(20f, 300f, 100f, 20f));

        //        if (readyToBegin)
        //        {
        //            if (GUILayout.Button("Cancel"))
        //                CmdChangeReadyState(false);
        //        }
        //        else
        //        {
        //            if (GUILayout.Button("Ready"))
        //                CmdChangeReadyState(true);
        //        }
        //        GUILayout.EndArea();
        //        GUILayout.BeginArea(new Rect(20f, 275f, 100f, 20f));
        //        if (team)
        //        {
        //            if (GUILayout.Button("Team 2"))
        //                CmdChangeTeamState(false);
        //        }
        //        else
        //        {
        //            if (GUILayout.Button("Team 1"))
        //                CmdChangeTeamState(true);
        //        }

        //        GUILayout.EndArea();
        //    }
        //}
    }
}

