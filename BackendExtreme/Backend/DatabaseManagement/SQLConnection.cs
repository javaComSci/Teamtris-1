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
    private static string connString = "Server=198.199.64.158;Database=Scores;Uid=dbuser;Port=3306;Pwd=mypassword;";

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
    public static long AddTeamScore(ScoresInfo scoresInfo) {
        // create connection
        MySqlConnection conn = new MySqlConnection(connString);

        // id of the team that has just inserted
        long imageId = -1;

        try
        {
            // open connection
            conn.Open();

            // create new command
            MySqlCommand command = conn.CreateCommand();

            // text for command with parameterization
            command.CommandText = "INSERT IGNORE INTO Scores(TeamName, Player1, Player2, Player3, Player4, TeamScore, TimePlayed) VALUES(@teamName, @player1, @player2, @player3, @player4, @teamScore, @timePlayed)";
            
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

            // get the id of the last inserted score for the team that has just placed in the scores
            imageId = command.LastInsertedId;

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }

        // close the connection
        conn.Close();

        // return the image id that has just been inserted
        return imageId;
    }

    /**
     * #function SQLConnection::UpdateTeamScore |
     * @author JavaComSci |
     * @desc Connects to the database in order to update a team information about scores |
     * @param String teamName: Name of team |
     * @param List<String> playerNames: Players of team |
     * @param int score: Score of team |
     * @param int timeInSeconds: Time played of team |
     */
    public static long UpdateTeamScore(ScoresInfo scoresInfo, long imageId) {
        // create connection
        MySqlConnection conn = new MySqlConnection(connString);

        try
        {
            // open connection
            conn.Open();

            // create new command
            MySqlCommand command = conn.CreateCommand();

            // text for command with parameterization
            command.CommandText = "UPDATE Scores SET TeamName = @teamName, Player1 = @player1, Player2 = @player2, Player3 = @player3, Player4 = @player4, TeamScore = @teamScore, TimePlayed = @timePlayed WHERE Id = @id";
            
            // add all the params
            command.Parameters.AddWithValue("@teamName", scoresInfo.teamName);
            command.Parameters.AddWithValue("@player1", scoresInfo.playerNames[0]);
            command.Parameters.AddWithValue("@player2", scoresInfo.playerNames[1] ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@player3", scoresInfo.playerNames[2] ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@player4", scoresInfo.playerNames[3] ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@teamScore", scoresInfo.teamScore);
            command.Parameters.AddWithValue("@timePlayed", scoresInfo.timePlayed);
            command.Parameters.AddWithValue("@id", imageId);

            // execute the query
            command.ExecuteNonQuery();

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }

        // close the connection
        conn.Close();

        // return the image id that has just been updated
        return imageId;
    }


     /**
     * #function SQLConnection::GetTopTeamsAndCurrentTeam |
     * @author JavaComSci |
     * @desc Connects to the database in order to obtain team results |
     * @param long teamId: Id of team |
     */
    public static Tuple<List<ScoresInfo>, ScoresInfo> GetTopTeamsAndCurrentTeam(long id) {
        // create connection
        MySqlConnection conn = new MySqlConnection(connString);

        // scoresInfo list for the top teams
        List<ScoresInfo> topTeams = new List<ScoresInfo>();

        // information for current team
        ScoresInfo currentTeam = null;

        try
        {
            // open connection
            conn.Open();

            // create new command
            MySqlCommand command = conn.CreateCommand();

            // text for command with top teams
            // command.CommandText = "SELECT * FROM Scores ORDER BY TeamScore DESC LIMIT 10";
            command.CommandText = "SELECT * FROM (SELECT *, rank() OVER (ORDER BY TeamScore DESC) as Ranking FROM Scores) AS ScoresRanked WHERE Ranking < 11 OR Id = @teamId";
            command.Parameters.AddWithValue("@teamId", id);
            // create a reader to read the high scores
            MySqlDataReader reader1 = command.ExecuteReader();

            // team counter
            int teamCount = 0;

            if (reader1.HasRows == true) {   
                while(reader1.Read()) {
                    
                    teamCount += 1;

                    // put the information read into the score info object
                    int teamId = Convert.ToInt32(reader1[0]);
                    String teamName = Convert.ToString(reader1[1]);
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
                    int ranking = Convert.ToInt32(reader1[8]);

                    // add the top team to the list of top teams
                    ScoresInfo team = new ScoresInfo(teamName, playerNames, teamScore, timePlayed);
                    team.rank = ranking;

                    // current team looking at
                    if(teamId == id) {
                        // current team
                        currentTeam = team;
                    }

                    // add to the top teams
                    if(teamCount < 11) {
                        topTeams.Add(team);
                    }
                }
            }            
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }

        // close the connection
        conn.Close();

        // return the tuple
        return Tuple.Create(topTeams, currentTeam);
    }



     /**
     * #function SQLConnection::GetTopTeamsAndCurrentTeam |
     * @author JavaComSci |
     * @desc Connects to the database in order to obtain team results |
     */
    public static List<ScoresInfo> GetTopTeams() {
        // create connection
        MySqlConnection conn = new MySqlConnection(connString);

        // scoresInfo list for the top teams
        List<ScoresInfo> topTeams = new List<ScoresInfo>();

        try
        {
            // open connection
            conn.Open();

            // create new command
            MySqlCommand command = conn.CreateCommand();

            // text for command with top teams
            command.CommandText = "SELECT * FROM Scores ORDER BY TeamScore DESC LIMIT 10";
            // create a reader to read the high scores
            MySqlDataReader reader1 = command.ExecuteReader();

            if (reader1.HasRows == true) {   
                while(reader1.Read()) {

                    // put the information read into the score info object
                    int teamId = Convert.ToInt32(reader1[0]);
                    String teamName = Convert.ToString(reader1[1]);
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

                    // add the top team to the list of top teams
                    ScoresInfo team = new ScoresInfo(teamName, playerNames, teamScore, timePlayed);

                    topTeams.Add(team);
                }
            }            
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }

        // close the connection
        conn.Close();

        // return the topteams
        return topTeams;
    }


}