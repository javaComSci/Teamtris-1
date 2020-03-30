using System;
using System.Collections.Generic;
using System.Linq;

/**
 * #class ScoresInfo |
 * @author JavaComSci | 
 * @language csharp | 
 * @desc Contains the infomation to create a scores object |
 */

 public class ScoresInfo {
     
    public String teamName { get; set; }

    public List<String> playerNames { get; set; }

    public int teamScore { get; set; }

    public int timePlayed { get; set; }

    public int rank {get; set;}

    public ScoresInfo(String teamName, List<String> playerNames, int teamScore, int timePlayed)
    {
        this.teamName = teamName;
        this.playerNames = playerNames;
        this.teamScore = teamScore;
        this.timePlayed = timePlayed;
        this.rank = -1;
    }
 }

