using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class NetworkManagerUI : MonoBehaviour {
    [SerializeField] private Button serverBtn;
    [SerializeField] private Button HostBtn;
    [SerializeField] private Button ClientBtn;
    [SerializeField] private Button StopBtn;

    private void Awake() {
        serverBtn.onClick.AddListener(() => {
            NetworkManager.Singleton.StartServer();
        });
        HostBtn.onClick.AddListener(() => {
            NetworkManager.Singleton.StartHost();
        });
        ClientBtn.onClick.AddListener(() => {
            NetworkManager.Singleton.StartClient();
        });
        StopBtn.onClick.AddListener(() => {
            NetworkManager.Singleton.Shutdown();
        });
    }
}
