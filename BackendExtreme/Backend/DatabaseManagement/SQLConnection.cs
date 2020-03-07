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
    public static void AddTeamScore(String teamName, List<String> playerNames, int score, int timeInSeconds) {
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
            command.Parameters.AddWithValue("@teamName", teamName);
            command.Parameters.AddWithValue("@player1", playerNames[0]);
            command.Parameters.AddWithValue("@player2", playerNames[1] ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@player3", playerNames[2] ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@player4", playerNames[3] ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@teamScore", score);
            command.Parameters.AddWithValue("@timePlayed", timeInSeconds);

            // execute the query
            command.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }

        // close the connection
        conn.Close();
        Console.WriteLine("Done.");
    }


     /**
     * #function SQLConnection::GetTopTeamsAndCurrentTeam |
     * @author JavaComSci |
     * @desc Connects to the database in order to obtain team results |
     * @param String teamName: Name of team |
     */
    public static void GetTopTeamsAndCurrentTeam(String teamName) {
        MySqlConnection conn = new MySqlConnection(connString);

        try
        {
            // open connection
            conn.Open();

            // create new command
            MySqlCommand commandTopTeams = conn.CreateCommand();

            // text for command with parameterization
            commandTopTeams.CommandText = "SELECT * FROM Scores ORDER BY TeamScore ASC LIMIT 10";

            MySqlDataReader reader1 = commandTopTeams.ExecuteReader();
            if (reader1.HasRows == true)
            {   
                
            }
            
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }

        // close the connection
        conn.Close();
        Console.WriteLine("Done.");
    }
}