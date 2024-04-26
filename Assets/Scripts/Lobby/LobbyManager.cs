using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using IngameDebugConsole;



public class LobbyManager : MonoBehaviour {

    public event EventHandler<LobbyEventArgs> OnJoinedLobby;
    public event EventHandler<LobbyEventArgs> OnJoinedLobbyUpdate;
    public event EventHandler<LobbyEventArgs> OnKickedFromLobby;
    public event EventHandler OnLeftLobby;
    public event EventHandler<LobbyEventArgs> OnLobbyGameModeChanged;

    public class LobbyEventArgs : EventArgs {
        public Lobby lobby;
    }

    public event EventHandler<OnLobbyListChangedEventArgs> OnLobbyListChanged;

    public class OnLobbyListChangedEventArgs : EventArgs {
        public List<Lobby> lobbyList;
    }

    public const string KEY_PLAYER_NAME = "PlayerName";
    public const string KEY_PLAYER_CHARACTER = "PlayerCharacter";
    public const string KEY_GAME_MODE = "GameMode";




    public static LobbyManager Instance { get; private set; }

    private Lobby joinedLobby;
    private string _playerName = "Player";
    private PlayerCharacter _playerCharacter;

    private void Awake() {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        DebugLogConsole.AddCommand("player.authenticate", "Authenticate player", Authenticate);
        DebugLogConsole.AddCommand<string, int, bool>("lobby.create", "Create a lobby", CreateLobby, "name", "players", "private");
        DebugLogConsole.AddCommand("lobby.player_list", "List players in lobby", () => {
            if (joinedLobby != null) {
                foreach (Player player in joinedLobby.Players) {
                    Debug.Log(player.Data[KEY_PLAYER_NAME].Value);
                }
            }
        });
        //DebugLogConsole.AddCommand("lobby.list", "List lobbies", ListLobbies);
        //DebugLogConsole.AddCommand("lobby.join_first", "Join first lobby", JoinLobbyFirst);
        //DebugLogConsole.AddCommand<string>("lobby.join_code", "Join lobby by code", JoinLobbyByCode);
    }

    private void Start() {
        Authenticate();
        OnJoinedLobby += (sender, e) => {
            StartCoroutine("LobbyHeartbeatCoroutine");
            StartCoroutine("SingleLobbyRefreshCoroutine");
        };
        OnLeftLobby += (sender, e) => {
            StopCoroutine("LobbyHeartbeatCoroutine");
            StopCoroutine("SingleLobbyRefreshCoroutine");
        };
        OnKickedFromLobby += (sender, e) => {
            StopCoroutine("LobbyHeartbeatCoroutine");
            StopCoroutine("SingleLobbyRefreshCoroutine");
        };
    }

    public async void Authenticate() {

        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () => {
            Debug.Log("Signed in! " + AuthenticationService.Instance.PlayerId);
        };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }


    public async void CreateLobby(string lobbyName, int maxPlayers, bool isPrivate) {
        Player player = GetPlayer();

        CreateLobbyOptions options = new CreateLobbyOptions {
            Player = player,
            IsPrivate = isPrivate,
        };

        Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, options);

        joinedLobby = lobby;

        OnJoinedLobby?.Invoke(this, new LobbyEventArgs { lobby = lobby });

        Debug.Log("Created Lobby " + lobby.Name);
    }

    public async void RefreshLobbyList() {
        try {
            QueryLobbiesOptions options = new QueryLobbiesOptions();
            options.Count = 25;

            // Filter for open lobbies only
            options.Filters = new List<QueryFilter> {
                new QueryFilter(
                    field: QueryFilter.FieldOptions.AvailableSlots,
                    op: QueryFilter.OpOptions.GT,
                    value: "0")
            };

            // Order by newest lobbies first
            options.Order = new List<QueryOrder> {
                new QueryOrder(
                    asc: false,
                    field: QueryOrder.FieldOptions.Created)
            };

            QueryResponse lobbyListQueryResponse = await Lobbies.Instance.QueryLobbiesAsync();
            Debug.Log("Results size: " + lobbyListQueryResponse.Results.Count);


            OnLobbyListChanged?.Invoke(this, new OnLobbyListChangedEventArgs { lobbyList = lobbyListQueryResponse.Results });
        }
        catch (LobbyServiceException e) {
            Debug.Log(e);
        }
    }

    IEnumerator LobbyHeartbeatCoroutine() {
        if (IsLobbyHost()) {
            var delay = new WaitForSecondsRealtime(15f);
            while (joinedLobby != null) {
                var task = Task.Run(async () => LobbyService.Instance.SendHeartbeatPingAsync(joinedLobby.Id));
                yield return new WaitUntil(() => task.IsCompleted);
                Debug.Log("Heartbeat sent");
                yield return delay;
            }
        }
    }

    IEnumerator SingleLobbyRefreshCoroutine() {
        var delay = new WaitForSecondsRealtime(1.1f);
        while (joinedLobby != null) {
            var task = Task.Run(async () => await LobbyService.Instance.GetLobbyAsync(joinedLobby.Id));
            yield return new WaitUntil(() => task.IsCompleted);

            joinedLobby = task.Result;
            OnJoinedLobbyUpdate?.Invoke(this, new LobbyEventArgs { lobby = joinedLobby });

            if (!IsPlayerInLobby()) {
                Debug.Log("Kicked from lobby");
                OnKickedFromLobby?.Invoke(this, new LobbyEventArgs { lobby = joinedLobby });
                joinedLobby = null;
            }
            yield return delay;

        }
    }


    public async void UpdatePlayerName(string playerName) {
        this._playerName = playerName;

        if (joinedLobby != null) {
            try {
                UpdatePlayerOptions options = new UpdatePlayerOptions();

                options.Data = new Dictionary<string, PlayerDataObject>() {
                    {
                        KEY_PLAYER_NAME, new PlayerDataObject(
                            visibility: PlayerDataObject.VisibilityOptions.Public,
                            value: playerName)
                    }
                };

                string playerId = AuthenticationService.Instance.PlayerId;

                Lobby lobby = await LobbyService.Instance.UpdatePlayerAsync(joinedLobby.Id, playerId, options);
                joinedLobby = lobby;

                OnJoinedLobbyUpdate?.Invoke(this, new LobbyEventArgs { lobby = joinedLobby });
            }
            catch (LobbyServiceException e) {
                Debug.Log(e);
            }
        }
    }

    public async void UpdatePlayerCharacter(PlayerCharacter playerCharacter) {
        if (joinedLobby != null) {
            try {
                UpdatePlayerOptions options = new UpdatePlayerOptions();

                options.Data = new Dictionary<string, PlayerDataObject>() {
                    {
                        KEY_PLAYER_CHARACTER, new PlayerDataObject(
                            visibility: PlayerDataObject.VisibilityOptions.Public,
                            value: playerCharacter.ToString())
                    }
                };

                string playerId = AuthenticationService.Instance.PlayerId;

                Lobby lobby = await LobbyService.Instance.UpdatePlayerAsync(joinedLobby.Id, playerId, options);
                joinedLobby = lobby;

                OnJoinedLobbyUpdate?.Invoke(this, new LobbyEventArgs { lobby = joinedLobby });
            }
            catch (LobbyServiceException e) {
                Debug.Log(e);
            }
        }
    }

    // Lobby Actions
    public async void JoinLobbyByCode(string lobbyCode) {
        Player player = GetPlayer();

        Lobby lobby = await LobbyService.Instance.JoinLobbyByCodeAsync(lobbyCode, new JoinLobbyByCodeOptions {
            Player = player
        });

        joinedLobby = lobby;

        OnJoinedLobby?.Invoke(this, new LobbyEventArgs { lobby = lobby });
    }

    public async void JoinLobby(Lobby lobby) {
        Player player = GetPlayer();

        joinedLobby = await LobbyService.Instance.JoinLobbyByIdAsync(lobby.Id, new JoinLobbyByIdOptions {
            Player = player
        });

        OnJoinedLobby?.Invoke(this, new LobbyEventArgs { lobby = lobby });
    }

    public async void QuickJoinLobby() {
        try {
            QuickJoinLobbyOptions options = new QuickJoinLobbyOptions();

            Lobby lobby = await LobbyService.Instance.QuickJoinLobbyAsync(options);
            joinedLobby = lobby;

            OnJoinedLobby?.Invoke(this, new LobbyEventArgs { lobby = lobby });
        }
        catch (LobbyServiceException e) {
            Debug.Log(e);
        }
    }

    public async void LeaveLobby() {
        if (joinedLobby != null) {
            try {
                await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, AuthenticationService.Instance.PlayerId);

                joinedLobby = null;

                OnLeftLobby?.Invoke(this, EventArgs.Empty);
            }
            catch (LobbyServiceException e) {
                Debug.Log(e);
            }
        }
    }

    public async void KickPlayer(string playerId) {
        if (IsLobbyHost()) {
            try {
                await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, playerId);
            }
            catch (LobbyServiceException e) {
                Debug.Log(e);
            }
        }
    }

    // Getters
    public Lobby GetJoinedLobby() {
        return joinedLobby;
    }

    public bool IsLobbyHost() {
        return joinedLobby != null && joinedLobby.HostId == AuthenticationService.Instance.PlayerId;
    }

    private bool IsPlayerInLobby() {
        if (joinedLobby != null && joinedLobby.Players != null) {
            foreach (Player player in joinedLobby.Players) {
                if (player.Id == AuthenticationService.Instance.PlayerId) {
                    return true;
                }
            }
        }
        return false;
    }

    private Player GetPlayer() {
        return new Player(AuthenticationService.Instance.PlayerId, null, new Dictionary<string, PlayerDataObject> {
        { KEY_PLAYER_NAME, new PlayerDataObject(PlayerDataObject.VisibilityOptions.Public, _playerName) },
        { KEY_PLAYER_CHARACTER, new PlayerDataObject(PlayerDataObject.VisibilityOptions.Public, PlayerCharacter.A.ToString()) }
    });
    }
}
