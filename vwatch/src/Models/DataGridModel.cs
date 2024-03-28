using System.ComponentModel;

namespace vwatch.Models
{
    public class DataGridModel : INotifyPropertyChanged
    {
        private string filename;
        public string Filename
        {
            get => filename;
            set
            {
                if (filename != value)
                {
                    filename = value;
                    OnPropertyChanged(nameof(Filename));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string Debugger { get; set; }
        public string AttachAtProcessStartWhen { get; set; }
        public string AtProcessStart { get; set; }
        public string Conditions { get; set; }

        public DataGridModel()
        {
        }
    }
}
