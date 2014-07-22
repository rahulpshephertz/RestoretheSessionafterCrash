using UnityEngine;
using System.Collections;
using com.shephertz.app42.gaming.multiplayer.client;
using com.shephertz.app42.gaming.multiplayer.client.listener;
using com.shephertz.app42.gaming.multiplayer.client.events;
using System.Text;
using com.shephertz.app42.gaming.multiplayer.client.command;
using System;
using System.Threading;
using System.Collections.Generic;

public class ClientSample : MonoBehaviour
{

		public static string roomId = null;
		private static string localUser = "rahul";
		private static string apiKey = "<your api key>";
		private static string secretKey = "<your secret key>";
		ConnectionListener connListener;
		RoomListener roomListener;
		NotifyList notify;
		ZoneListen zoneListen;
		string debug = "";
		public void Start ()
		{
				initAppwarp ();
				connListener = new ConnectionListener ();
				roomListener = new RoomListener ();
				notify = new NotifyList ();
				zoneListen = new ZoneListen ();
				WarpClient.GetInstance ().AddConnectionRequestListener (connListener);
				WarpClient.GetInstance ().AddRoomRequestListener (roomListener);
				WarpClient.GetInstance ().AddNotificationListener (notify);
				WarpClient.GetInstance ().AddZoneRequestListener (zoneListen);
		}
	
		private void initAppwarp ()
		{
		     WarpClient.initialize (apiKey, secretKey);
		     WarpClient.setRecoveryAllowance (20);
		}
	
		public void Update ()
		{
				WarpClient.GetInstance ().Update ();
		}

		void OnGUI ()
		{
				GUI.contentColor = Color.white;
				GUI.Label (new Rect (10, 10, 300, 100), getDebug()+ " " +connListener.getDebug () + " " + zoneListen.getDebug () + " " + roomListener.getDebug () + " " + notify.getDebug ());

				if (GUI.Button (new Rect (10, 130, 150, 60), "Connect")) {		
						WarpClient.GetInstance ().Connect (localUser);
				}

				if (GUI.Button (new Rect (10, 200, 150, 60), "Create Room")) {		
						Dictionary<string, object> properties = new Dictionary<string, object> ();
						properties.Add ("user", "xyz");
						properties.Add ("lang", "eng");
						WarpClient.GetInstance ().CreateRoom ("UnityRoom", "UnityRoom", 1, properties);
				}

				if (GUI.Button (new Rect (10, 270, 150, 60), "Recover Connection")) {		
						if (PlayerPrefs.HasKey ("SessionID")) {
								int sessionID = PlayerPrefs.GetInt ("SessionID");
				WarpClient.GetInstance ().RecoverConnectionWithSessioId (sessionID, localUser);
			}else {
				Log ("SessionId is not available");
			}
				}

				if (GUI.Button (new Rect (10, 340, 150, 60), "Disconnect")) {
						WarpClient.GetInstance ().Disconnect ();
				}
				if (GUI.Button (new Rect (10, 410, 150, 60), "Raise Exception")) {
							throw new Exception();
					}
		}

		void OnApplicationQuit ()
		{
				if (PlayerPrefs.HasKey ("SessionID")) {
						PlayerPrefs.DeleteKey ("SessionID");
						PlayerPrefs.Save ();
				}
		}
	private void Log (string msg)
	{
		debug = msg + "  " + debug;
	}
	
	public string getDebug ()
	{
		return debug;
	}

}

public class ConnectionListener : ConnectionRequestListener
{
		string debug = "";
		private void Log (string msg)
		{
				debug = msg + "  " + debug;
		}

		public string getDebug ()
		{
				return debug;
		}

		public void onConnectDone (ConnectEvent e)
		{
				switch (e.getResult ()) {
				case WarpResponseResultCode.AUTH_ERROR:
						if (e.getReasonCode () == WarpReasonCode.WAITING_FOR_PAUSED_USER) {
								Log ("Auth Error:Server is waiting from previous session");
						} else {
								Log ("Auth Error:SessionId Expired");
						}
						break;
				case WarpResponseResultCode.SUCCESS:
						PlayerPrefs.SetInt ("SessionID", WarpClient.GetInstance ().GetSessionId ());
						PlayerPrefs.Save ();
						Log ("Connect Done ");
						break;
				case WarpResponseResultCode.CONNECTION_ERROR_RECOVERABLE:
						Log ("Connect Error Recoverable ");
			//RecoverConnection();
						break;
				case WarpResponseResultCode.SUCCESS_RECOVERED:
						Log ("Connection Success Recovered");
			//ConnectionRecovered();
						break;
				default:
						Log ("Connect Failed " + e.getResult ());
						break;
				}
		}

		public void onLog (string logs)
		{
				Log (logs);
		}

		public void onDisconnectDone (ConnectEvent e)
		{
				if (e.getResult () == 0) {
						Log ("Disconnect Done ");
				} else {
						Log ("Disconnect Failed " + e.getResult ());
				}
		}
	
		public void onInitUDPDone (byte bytes)
		{
		}
}

public class RoomListener: RoomRequestListener
{

		string debug = "";

		private void Log (string msg)
		{
				debug = msg + "  " + debug;
		}

		public string getDebug ()
		{
				return debug;
		}

		public void onSubscribeRoomDone (RoomEvent eventObj)
		{
				//throw new System.NotImplementedException ();
		}

		public void onUnSubscribeRoomDone (RoomEvent eventObj)
		{
				//throw new System.NotImplementedException ();
		}

		public void onLeaveRoomDone (RoomEvent eventObj)
		{
				//throw new System.NotImplementedException ();
		}

		public void onGetLiveRoomInfoDone (LiveRoomInfoEvent eventObj)
		{
				//throw new System.NotImplementedException ();
		}

		public void onSetCustomRoomDataDone (LiveRoomInfoEvent eventObj)
		{
				//throw new System.NotImplementedException ();
		}

		public void onUpdatePropertyDone (LiveRoomInfoEvent liveRoomInfoEvent)
		{
				Log ("Result :" + liveRoomInfoEvent.getResult () + "Count :" + liveRoomInfoEvent.getProperties ().Count);
		}

		public void onLockPropertiesDone (byte result)
		{
				//throw new System.NotImplementedException ();
		}

		public void onUnlockPropertiesDone (byte result)
		{
				//throw new System.NotImplementedException ();
		}
		/// Invoked when the response for joinRoom request is received.
		/// <param name="eventObj"></param>
		public void onJoinRoomDone (RoomEvent eventObj)
		{
				Log ("on JoinRoomDone " + eventObj.getResult ());
		}
		/// other methods
}

public class NotifyList:NotifyListener
{
	#region NotifyListener implementation
		string debug = "";

		private void Log (string msg)
		{
				debug = msg + "  " + debug;
		}

		public string getDebug ()
		{
				return debug;
		}

		public void onRoomCreated (RoomData eventObj)
		{
				//throw new System.NotImplementedException ();
		}

		public void onRoomDestroyed (RoomData eventObj)
		{
				//throw new System.NotImplementedException ();
		}

		public void onUserLeftRoom (RoomData eventObj, string username)
		{
				//throw new System.NotImplementedException ();
		}

		public void onUserJoinedRoom (RoomData eventObj, string username)
		{
				//throw new System.NotImplementedException ();
		}

		public void onUserLeftLobby (LobbyData eventObj, string username)
		{
				//throw new System.NotImplementedException ();
		}

		public void onUserJoinedLobby (LobbyData eventObj, string username)
		{
				//throw new System.NotImplementedException ();
		}

		public void onChatReceived (ChatEvent eventObj)
		{
				//throw new System.NotImplementedException ();
		}

		public void onUpdatePeersReceived (UpdateEvent eventObj)
		{
				//throw new System.NotImplementedException ();
		}

		public void onUserChangeRoomProperty (RoomData roomData, string sender, System.Collections.Generic.Dictionary<string, object> properties, System.Collections.Generic.Dictionary<string, string> lockedPropertiesTable)
		{
				//throw new System.NotImplementedException ();
		}

		public void onPrivateChatReceived (string sender, string message)
		{
				//hrow new System.NotImplementedException ();
		}

		public void onMoveCompleted (MoveEvent moveEvent)
		{
				//throw new System.NotImplementedException ();
		}

		public void onUserPaused (string locid, bool isLobby, string username)
		{
				//throw new System.NotImplementedException ();
		}

		public void onUserResumed (string locid, bool isLobby, string username)
		{
				//throw new System.NotImplementedException ();
		}

		public void onGameStarted (string sender, string roomId, string nextTurn)
		{
				//throw new System.NotImplementedException ();
		}

		public void onGameStopped (string sender, string roomId)
		{
				//throw new System.NotImplementedException ();
		}

		public void onPrivateUpdateReceived (string sender, byte[] update, bool fromUdp)
		{
				Log (sender + " " + Encoding.UTF8.GetString (update, 0, update.Length) + " " + fromUdp);
		}
	#endregion
}

public class ZoneListen:ZoneRequestListener
{
		string debug = "";

		private void Log (string msg)
		{
				debug = msg + "  " + debug;
		}

		public string getDebug ()
		{
				return debug;
		}

		public void onDeleteRoomDone (RoomEvent eventObj)
		{
				throw new System.NotImplementedException ();
		}

		public void onGetAllRoomsDone (AllRoomsEvent eventObj)
		{
				throw new System.NotImplementedException ();
		}

		public void onCreateRoomDone (RoomEvent eventObj)
		{
				ClientSample.roomId = eventObj.getData ().getId ();
				WarpClient.GetInstance ().JoinRoom (ClientSample.roomId);
		}

		public void onGetOnlineUsersDone (AllUsersEvent eventObj)
		{
				throw new System.NotImplementedException ();
		}

		public void onGetLiveUserInfoDone (LiveUserInfoEvent eventObj)
		{
				throw new System.NotImplementedException ();
		}

		public void onSetCustomUserDataDone (LiveUserInfoEvent eventObj)
		{
				throw new System.NotImplementedException ();
		}

		public void onGetMatchedRoomsDone (MatchedRoomsEvent matchedRoomsEvent)
		{
				throw new System.NotImplementedException ();
		}
}
