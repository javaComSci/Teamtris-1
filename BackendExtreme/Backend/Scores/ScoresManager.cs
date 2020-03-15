using System;
using WebSocketSharp;
using WebSocketSharp.Server;
using System.Threading;
using System.Collections.Generic;
using Newtonsoft.Json;

public class ScoresManager : WebSocketBehavior
{
    private Thread thread;
    private int count;
    private string bean;

    public ScoresManager(){}

    protected override void OnOpen()
    {
        Console.WriteLine("score recieved");
    }

    protected override void OnMessage(MessageEventArgs e)
    {
        Console.WriteLine(e.Data);
        Packet packet = JsonConvert.DeserializeObject<Packet>(e.Data);
        string socketID = ID;

        // score packet -- 10
        if (packet.type == Packets.SCORES) {
            // format the packet to be a scors packet with all the scoring information
            ScoresPacket sPacket = JsonConvert.DeserializeObject<ScoresPacket>(packet.data);
            
            // make the information for the scores object
            List<String> players = sPacket.playerNames;
            while(players.Count < 4) {
                players.Add(null);
            }
            ScoresInfo scores = new ScoresInfo(sPacket.teamName, players, sPacket.teamScore, sPacket.timePlayed);

            // add the scores to the database and retrieve the team information including the current team
            long id = SQLConnection.AddTeamScore(scores);   
            Tuple<List<ScoresInfo>, ScoresInfo> retrievedInfo = SQLConnection.GetTopTeamsAndCurrentTeam(id);
            
        }
    }
}

    
