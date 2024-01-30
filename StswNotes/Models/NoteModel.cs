namespace StswNotes;
public class NoteModel : StswObservableObject
{
    /// ID
    public int ID
    {
        get => _id;
        set => SetProperty(ref _id, value);
    }
    private int _id;

    /// Name
    public string? Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }
    private string? _name = string.Empty;

    /// Author
    public string? Author
    {
        get => _author;
        set => SetProperty(ref _author, value);
    }
    private string? _author = Environment.UserName;

    /// Document
    public string? Document
    {
        get => _document;
        set => SetProperty(ref _document, value);
    }
    private string? _document;

    /// CreateDT
    public DateTime CreateDT
    {
        get => _createDT;
        set => SetProperty(ref _createDT, value);
    }
    private DateTime _createDT = DateTime.Now;
}
