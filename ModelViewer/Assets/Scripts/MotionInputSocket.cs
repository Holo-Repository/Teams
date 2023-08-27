using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using NativeWebSocket;

public class MotionInputSocket : MonoBehaviour
{
  [SerializeField] GameObject target;

  public GameObject Target { get => target; set => target = value; }

  WebSocket websocket;

  // Start is called before the first frame update
  async void Start()
  {
    // The web socker server will need to be changed base on the IP address of the computer running the server
    // This is a hard coded IP address for the computer running the server during development   
    websocket = new WebSocket("ws://127.0.0.1:33705");

    websocket.OnOpen += () =>
    {
      Debug.Log("Connection open!");
    };

    websocket.OnError += (e) =>
    {
      Debug.Log("Error! " + e);
    };

    websocket.OnClose += (e) =>
    {
      Debug.Log("Connection closed!");
    };

    websocket.OnMessage += (bytes) =>
    {
      target.GetComponent<ModelRotationController>().SetRotationMI(System.Text.Encoding.UTF8.GetString(bytes));
    };

    // waiting for messages
    await websocket.Connect();
  }

  void Update()
  {
    #if !UNITY_WEBGL || UNITY_EDITOR
      websocket.DispatchMessageQueue();
    #endif
  }

  private async void OnApplicationQuit()
  {
    await websocket.Close();
  }

}