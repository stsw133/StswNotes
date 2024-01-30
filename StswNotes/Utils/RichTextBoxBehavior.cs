using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace StswNotes;
public static class RichTextBoxBehavior
{
    public static readonly DependencyProperty DocumentProperty =
        DependencyProperty.RegisterAttached("Document", typeof(FlowDocument), typeof(RichTextBoxBehavior),
            new PropertyMetadata(OnDocumentChanged));

    public static FlowDocument GetDocument(DependencyObject obj)
    {
        return (FlowDocument)obj.GetValue(DocumentProperty);
    }

    public static void SetDocument(DependencyObject obj, FlowDocument value)
    {
        obj.SetValue(DocumentProperty, value);
    }

    private static void OnDocumentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is RichTextBox richTextBox)
        {
            richTextBox.Document = e.NewValue as FlowDocument;
        }
    }
}
