using System;
using WebSocketSharp;
using WebSocketSharp.Server;
using System.Threading;
using System.Collections.Generic;
using Newtonsoft.Json;

public class LobbyManager : WebSocketBehavior
{
    private Thread thread;
    private int count;
    private Dictionary<string, Lobby> lobbies;
    private string bean;


    public LobbyManager(Dictionary<string, Lobby> lobbies)
    {
        this.lobbies = lobbies;
        // thread = new Thread(SendState);
        // thread.Start();
    }

    protected override void OnOpen()
    {
        Console.WriteLine("user joined");
    }

    protected override void OnMessage(MessageEventArgs e)
    {
        Console.WriteLine(e.Data);
        Packet packet = JsonConvert.DeserializeObject<Packet>(e.Data);
        string socketID = ID;
        // join packet
        if (packet.type == Packets.JOIN)
        {
            JoinPacket jPacket = JsonConvert.DeserializeObject<JoinPacket>(packet.data);
            joinLobby(jPacket.lobbyID, jPacket.playerID, jPacket.name, socketID);
        }
        // create packet
        else if (packet.type == Packets.CREATE)
        {
            try
            {
                Console.WriteLine("making new create packet");
                // Create a lobby with given parameters
                CreatePacket createPacket = JsonConvert.DeserializeObject<CreatePacket>(packet.data);
                createLobby(createPacket.maxPlayers, createPacket.name, createPacket.playerID, socketID);
            }
            catch (NullReferenceException nre)
            {
                Console.WriteLine("hi");
                Console.WriteLine(nre);
            }
        }
        else if (packet.type == 2)
        {
            PlayPacket playPacket = JsonConvert.DeserializeObject<PlayPacket>(packet.data);
            // 5 times number of players plus 5
            Lobby gameLobby = lobbies[playPacket.lobbyID];
            GameState game = new GameState(5 * gameLobby.numPlayers + 5, 20);
            Dictionary<int, Player> players = new Dictionary<int, Player>();
            for (int i = 0; i < gameLobby.players.Count; i++)
            {
                players[gameLobby.players[i].id] = gameLobby.players[i];
            }
            game.players = players;
            gameLobby.game = game;
            // update all players that game will start
            alertLobby(0, playPacket.lobbyID, Packets.START);
        }
        else
        {
            Console.WriteLine("bad packet");
            Send("bad packet!!! :(");
        }
    }

    public void createLobby(int maxPlayers, string name, int id, string socketID)
    {
        int playerID = RandomNumber(1001, 9998);
        // initialize a new lobby, player, and list of players
        Lobby newLobby = new Lobby(getToken(), maxPlayers);
        Player newPlayer = new Player(playerID, name, socketID);
        newLobby.players = new List<Player>();

        newLobby.players.Add(newPlayer);
        // lobbies.Add(newLobby);
        lobbies[newLobby.id] = newLobby;

        // Create a packet to confirm creation of new lobby
        ConfirmationPacket confirmationPacket = new ConfirmationPacket();
        confirmationPacket.lobbyID = newLobby.id;
        confirmationPacket.playerID = playerID;
        Send(JsonConvert.SerializeObject(confirmationPacket));

        Console.WriteLine(maxPlayers + name + id);
        Console.WriteLine("sending new lobby");
    }

    public void joinLobby(string lobbyID, int playerID, string name, string socketID)
    {
        lobbyID = lobbyID.ToLower();
        Lobby lobby;
        if (lobbies == null)
        {
            Console.WriteLine("lobbies is null");
        }
        if (lobbies.ContainsKey(lobbyID))
        {
            lobby = lobbies[lobbyID];
            if (lobby.numPlayers < lobby.maxPlayers)
            {
                int newPlayerID = RandomNumber(1001, 9998);
                lobby.numPlayers += 1;
                Player newPlayer = new Player(newPlayerID, name, socketID);
                lobby.players.Add(newPlayer);
                // send message to user of lobby id
                ConfirmationPacket confirmationPacket = new ConfirmationPacket();
                confirmationPacket.lobbyID = lobbyID;
                confirmationPacket.playerID = newPlayerID;
                alertLobby(playerID, lobbyID, Packets.UPDATE);
                Send(JsonConvert.SerializeObject(confirmationPacket));
            }
            else
            {
                // maxplayers reached :(
            }
        }
        else
        {
            //send message invalid ID
            Send("bad");
        }
    }

    private string getToken()
    {
        var chars = "abcdefghijklmnopqrstuvwxyz";
        var stringChars = new char[4];
        var random = new Random();

        for (int i = 0; i < stringChars.Length; i++)
        {
            stringChars[i] = chars[random.Next(chars.Length)];
        }

        var finalString = new String(stringChars);
        return finalString;
    }

    static int RandomNumber(int min, int max)
    {
        Random random = new Random(); return random.Next(min, max);

    }

    private void alertLobby(int playerID, string lobbyID, int type)
    {
        Console.WriteLine("checking for lobbyid " + lobbyID);
        LobbyInfoPacket lobbyInfoPacket = new LobbyInfoPacket();
        Lobby lobby;
        if (lobbies.ContainsKey(lobbyID))
        {
            lobby = lobbies[lobbyID];
            for (int j = 0; j < lobby.players.Count; j++)
            {
                lobbyInfoPacket.players = lobby.players;
                lobbyInfoPacket.lobbyID = lobbyID;
                lobbyInfoPacket.maxPlayers = lobby.maxPlayers;
                lobbyInfoPacket.dataType = type;
                Sessions.SendTo(JsonConvert.SerializeObject(lobbyInfoPacket), lobby.players[j].socketID);
            }
        }
    }

    protected override void OnClose(CloseEventArgs e)
    {
        // thread.Abort(); // terminate thread on socket close

        // On socket close remove player from lobby
        foreach (var lobby in lobbies.Values)
        {
            for (int j = 0; j < lobby.players.Count; j++)
            {
                if (lobby.players[j].socketID == ID)
                {
                    Console.WriteLine("deleting player " + lobby.players[j].name);
                    lobby.players.Remove(lobby.players[j]);
                    // if lobby is void of players, remove the lobby
                    break;
                }
            }
            if (lobby.players.Count == 0)
            {
                Console.WriteLine("deleting lobby " + lobby.id);
                lobbies.Remove(lobby.id);
                break;
            }
        }
    }
}