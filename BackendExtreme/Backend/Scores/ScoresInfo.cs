using System;
using System.Collections.Generic;
using System.Linq;

/**
 * #class ScoresInfo |
 * @author JavaComSci | 
 * @language csharp | 
 * @desc TODO |
 */

 public class ScoresInfo {
     
    public String teamName { get; set; }

    public List<String> players { get; set; }

    public int teamScore { get; set; }

    public int timePlayed { get; set; }

    public ScoresInfo(String teamName, List<String> players, int teamScore, int timePlayed)
    {
        
    }
 }

