using System.Data;
using System.Data.SqlClient;

namespace StswNotes;
internal static class SQL
{
    /// <summary>
    /// Example connection string
    /// </summary>
    private static string connString = new StswDatabaseModel()
    {
#if RELEASE
        Server = "",
#else
        Server = "",
#endif
        Database = "",
        Login = "",
        Password = ""
    }.GetConnString();

    /// GetNotes
    internal static IEnumerable<NoteModel> GetNotes()
    {
        using (var sqlConn = new SqlConnection(connString))
        {
            sqlConn.Open();

            var query = $@"
                select top 1000
                    ID [{nameof(NoteModel.ID)}],
                    Name [{nameof(NoteModel.Name)}],
                    Author [{nameof(NoteModel.Author)}],
                    null [{nameof(NoteModel.Document)}],
                    CreateDT [{nameof(NoteModel.CreateDT)}]
                from dbo.Notes with(nolock)";
            using (var sqlDA = new SqlDataAdapter(query, sqlConn))
            {
                var dt = new DataTable();
                sqlDA.Fill(dt);
                return dt.MapTo<NoteModel>();
            }
        }
    }

    /// GetNoteDocument
    internal static string? GetNoteDocument(int noteID)
    {
        using (var sqlConn = new SqlConnection(connString))
        {
            sqlConn.Open();

            var query = $@"
                select Document
                from dbo.Notes with(nolock)
                where ID=@ID";
            using (var sqlCmd = new SqlCommand(query, sqlConn))
            {
                sqlCmd.Parameters.AddWithValue("@ID", noteID);
                using (var sqlDR = sqlCmd.ExecuteReader())
                {
                    if (sqlDR.Read())
                        return Convert.ToString(sqlDR[0]);
                    else
                        return null;
                }
            }
        }
    }

    /// SaveNote
    internal static int SaveNote(NoteModel model)
    {
        using (var sqlConn = new SqlConnection(connString))
        {
            sqlConn.Open();

            SqlCommand sqlCmd;
            if (model.ID > 0)
            {
                var query = $@"
                    update dbo.Notes
                    set Name=@Name, Author=@Author, Document=iif(@Document is null, Document, @Document), CreateDT=@CreateDT
                    where ID=@ID;
                    select @ID";
                sqlCmd = new SqlCommand(query, sqlConn);
                sqlCmd.Parameters.AddWithValue("@ID", model.ID);
            }
            else
            {
                var query = $@"
                    insert into dbo.Notes (Name, Author, Document, CreateDT)
                    values (@Name, @Author, @Document, @CreateDT);
                    select scope_identity()";
                sqlCmd = new SqlCommand(query, sqlConn);
            }
            sqlCmd.Parameters.AddWithValue("@Name", model.Name);
            sqlCmd.Parameters.AddWithValue("@Author", model.Author);
            sqlCmd.Parameters.AddWithValue("@Document", model.Document);
            sqlCmd.Parameters.AddWithValue("@CreateDT", model.CreateDT);
            return Convert.ToInt32(sqlCmd.ExecuteScalar());
        }
    }

    /// DeleteNote
    internal static void DeleteNote(int noteID)
    {
        using (var sqlConn = new SqlConnection(connString))
        {
            sqlConn.Open();

            var query = $@"delete from dbo.Notes where ID=@ID";
            using (var sqlCmd = new SqlCommand(query, sqlConn))
            {
                sqlCmd.Parameters.AddWithValue("ID", noteID);
                sqlCmd.ExecuteNonQuery();
            }
        }
    }
}
