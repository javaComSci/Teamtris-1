using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data;
using MySql.Data.MySqlClient;

/**
 * #class SQLConnection |
 * @author JavaComSci |
 * @desc Connection class in order to connect, insert, and update from the database. |
 * @language csharp |
 */
public class SQLConnection
{
    private static string connString = "Server=198.199.64.158;Database=Scores;Uid=root;Port=3306;Pwd=mypassword;";

    public SQLConnection() {
        Console.WriteLine("DB connection setup");    
    }

    /**
     * #function SQLConnection::AddTeamScore |
     * @author JavaComSci |
     * @desc Connects to the database in order to add the team information about scores |
     * @param String teamName: Name of team |
     * @param List<String> playerNames: Players of team |
     * @param int score: Score of team |
     * @param int timeInSeconds: Time played of team |
     */
    public static void AddTeamScore(ScoresInfo scoresInfo) {
        MySqlConnection conn = new MySqlConnection(connString);

        try
        {
            // open connection
            conn.Open();

            // create new command
            MySqlCommand command = conn.CreateCommand();

            // text for command with parameterization
            command.CommandText = "INSERT INTO Scores(TeamName, Player1, Player2, Player3, Player4, TeamScore, TimePlayed) VALUES(@teamName, @player1, @player2, @player3, @player4, @teamScore, @timePlayed)";
            
            // add all the params
            command.Parameters.AddWithValue("@teamName", scoresInfo.teamName);
            command.Parameters.AddWithValue("@player1", scoresInfo.playerNames[0]);
            command.Parameters.AddWithValue("@player2", scoresInfo.playerNames[1] ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@player3", scoresInfo.playerNames[2] ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@player4", scoresInfo.playerNames[3] ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@teamScore", scoresInfo.teamScore);
            command.Parameters.AddWithValue("@timePlayed", scoresInfo.timePlayed);

            // execute the query
            command.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }

        // close the connection
        conn.Close();
    }


     /**
     * #function SQLConnection::GetTopTeamsAndCurrentTeam |
     * @author JavaComSci |
     * @desc Connects to the database in order to obtain team results |
     * @param String teamName: Name of team |
     */
    public static void GetTopTeamsAndCurrentTeam(String teamName) {
        MySqlConnection conn = new MySqlConnection(connString);

        // ScoresInfo list for the top teams
        List<ScoresInfo> topTeams = new List<ScoresInfo>();

        try
        {
            // open connection
            conn.Open();

            // create new command
            MySqlCommand commandTopTeams = conn.CreateCommand();

            // text for command with parameterization
            commandTopTeams.CommandText = "SELECT * FROM Scores ORDER BY TeamScore ASC LIMIT 10";

            // create a reader to read the high scores
            MySqlDataReader reader1 = commandTopTeams.ExecuteReader();

            while (reader1.HasRows == true)
            {   
                // put the information read into the score info object
                String tname = Convert.ToString(reader1[1]);

                List<String> playerNames = new List<String>();
                if(reader1[2] != DBNull.Value) {
                    playerNames.Add(Convert.ToString(reader1[2]));
                }
                if(reader1[3] != DBNull.Value) {
                    playerNames.Add(Convert.ToString(reader1[3]));
                }
                if(reader1[4] != DBNull.Value) {
                    playerNames.Add(Convert.ToString(reader1[4]));
                }
                if(reader1[5] != DBNull.Value) {
                    playerNames.Add(Convert.ToString(reader1[5]));
                }

                int teamScore = Convert.ToInt32(reader1[6]);
                int timePlayed = Convert.ToInt32(reader1[7]);
                ScoresInfo topTeam = new ScoresInfo(tname, playerNames, teamScore, timePlayed);
            }
            
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }

        // close the connection
        conn.Close();
    }
}