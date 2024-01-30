using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Markup;

namespace StswNotes;
public class MainContext : StswObservableObject
{
    /// <summary>
    /// Example command
    /// </summary>
    public StswCommand RefreshCommand { get; set; }
    public StswCommand AddNewCommand { get; set; }
    public StswCommand RemoveCommand { get; set; }
    public StswCommand SaveCommand { get; set; }

    public MainContext()
    {
        RefreshCommand = new(Refresh);
        AddNewCommand = new(AddNew);
        RemoveCommand = new(Remove);
        SaveCommand = new(Save);

        Refresh();
    }

    #region Commands & methods
    /// Refresh
    private void Refresh()
    {
        NoteModels = SQL.GetNotes().ToObservableCollection();
        NoteView = CollectionViewSource.GetDefaultView(NoteModels);
        NoteView.Filter += CollectionFilter;
    }

    /// AddNew
    private void AddNew()
    {
        NoteModels.Add(new());
        SelectedNoteModel = NoteModels.LastOrDefault();
    }

    /// Remove
    private async void Remove()
    {
        if (SelectedNoteModel != null)
        {
            if (SelectedNoteModel.ID == 0)
                NoteModels.Remove(SelectedNoteModel);
            else if (await StswMessageDialog.Show("Czy na pewno usunąć zaznaczoną notatkę?", "Ostrzeżenie", StswDialogButtons.YesNo, StswDialogImage.Warning) == true)
            {
                SQL.DeleteNote(SelectedNoteModel.ID);
                NoteModels.Remove(SelectedNoteModel);
            }
        }
    }

    /// Save
    private void Save()
    {
        if (SelectedNoteModel != null)
        {
            SelectedNoteModel.Document = XamlWriter.Save(Document);
            SelectedNoteModel.ID = SQL.SaveNote(SelectedNoteModel);
            StswMessageDialog.Show($"Zapisano notatkę o nazwie \"{SelectedNoteModel.Name}\".", "Informacja", StswDialogButtons.OK, StswDialogImage.Information);
        }
    }

    /// CollectionFilter
    private bool CollectionFilter(object o)
    {
        if (o is NoteModel model)
            return model.Name?.ToLower()?.Contains(SearchText.ToLower()) == true;
        
        return true;
    }
    #endregion

    #region Properties
    /// Document
    public FlowDocument? Document
    {
        get => _document;
        set => SetProperty(ref _document, value);
    }
    private FlowDocument? _document = new(new Paragraph(new Run(string.Empty)));

    /// NoteModels
    public ObservableCollection<NoteModel> NoteModels
    {
        get => _noteModels;
        set => SetProperty(ref _noteModels, value);
    }
    private ObservableCollection<NoteModel> _noteModels = new();

    /// NoteView
    public ICollectionView? NoteView
    {
        get => _noteView;
        set => SetProperty(ref _noteView, value);
    }
    private ICollectionView? _noteView;

    /// SearchText
    public string SearchText
    {
        get => _searchText;
        set
        {
            SetProperty(ref _searchText, value);
            NoteView?.Refresh();
        }
    }
    private string _searchText = string.Empty;

    /// SelectedNoteModel
    public NoteModel? SelectedNoteModel
    {
        get => _selectedNoteModel;
        set
        {
            if (_selectedNoteModel != null)
                _selectedNoteModel.Document = XamlWriter.Save(Document);

            SetProperty(ref _selectedNoteModel, value);

            if (value != null && value.ID > 0 && value.Document == null)
                value.Document = SQL.GetNoteDocument(value.ID);
            Document = value?.Document == null ? new(new Paragraph(new Run(string.Empty))) : XamlReader.Parse(value.Document) as FlowDocument;
        }
    }
    private NoteModel? _selectedNoteModel;
    #endregion
}
