using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

// CSZZ - 褰╄壊鎴樹簤
namespace CSZZGame.Networking
{
    [AddComponentMenu("")]
    public class NetworkRoomPlayerScript : NetworkRoomPlayer
    {
        [SyncVar(hook = nameof(TeamStateChanged))]
        public bool team;

        #region Commands

        [Command]
        public void CmdChangeTeamState(bool teamState)
        {
            team = teamState;
            //characterData.swapTeam(teamState);
        }

        #endregion

        #region SyncVar Hooks

        /// <param name="newTeamState">New Ready State</param>
        public virtual void TeamStateChanged(bool oldTeamState, bool newTeamState) { }

        #endregion


        public override void OnGUI()
        {
            if (!showRoomGUI)
                return;

            NetworkRoomManager room = NetworkManager.singleton as NetworkRoomManager;
            if (room)
            {
                if (!room.showRoomGUI)
                    return;

                if (!NetworkManager.IsSceneActive(room.RoomScene))
                    return;

                DrawPlayerReadyState();
                DrawPlayerReadyButton();
            }
        }

        void DrawPlayerReadyState()
        {
            GUILayout.BeginArea(new Rect(20f + (index * 100), 200f, 90f, 130f));

            GUILayout.Label($"Player [{index + 1}]");

            if (readyToBegin)
                GUILayout.Label("Ready");
            else
                GUILayout.Label("Not Ready");

            if (team)
                GUILayout.Label("Team 2");
            else
                GUILayout.Label("Team 1");


            if (((isServer && index > 0) || isServerOnly) && GUILayout.Button("REMOVE"))
            {
                // This button only shows on the Host for all players other than the Host
                // Host and Players can't remove themselves (stop the client instead)
                // Host can kick a Player this way.
                GetComponent<NetworkIdentity>().connectionToClient.Disconnect();
            }

            GUILayout.EndArea();
        }

        void DrawPlayerReadyButton()
        {
            if (NetworkClient.active && isLocalPlayer)
            {
                GUILayout.BeginArea(new Rect(20f, 300f, 100f, 20f));

                if (readyToBegin)
                {
                    if (GUILayout.Button("Cancel"))
                        CmdChangeReadyState(false);
                }
                else
                {
                    if (GUILayout.Button("Ready"))
                        CmdChangeReadyState(true);
                }
                GUILayout.EndArea();
                GUILayout.BeginArea(new Rect(20f, 275f, 100f, 20f));
                if (team)
                {
                    if (GUILayout.Button("Team 2"))
                        CmdChangeTeamState(false);
                }
                else
                {
                    if (GUILayout.Button("Team 1"))
                        CmdChangeTeamState(true);
                }

                GUILayout.EndArea();
            }
        }
    }
}

