namespace vwatch.Models
{
    public class DataGridModel
    {
        public string Filename { get; set; }
        public string Debugger { get; set; }
        public string AttachAtProcessStartWhen { get; set; }
        public string AtProcessStart { get; set; }
        public string Conditions { get; set; }

        public DataGridModel()
        {
        }
    }
}
