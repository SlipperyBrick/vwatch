using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using vwatch.Models;

namespace vwatch.ViewModels
{
    public class DataGridViewModel : ObservableObject
    {
        public RelayCommand AddItemCommand { get; }
        public RelayCommand RemoveItemCommand { get; }
        public RelayCommand ClearSelectionCommand { get; }

        private ObservableCollection<DataGridModel> _items = new ObservableCollection<DataGridModel>();
        public ObservableCollection<DataGridModel> Items
        {
            get => _items;
            set => SetProperty(ref _items, value);
        }

        private DataGridModel _selectedItem;
        public DataGridModel SelectedItem
        {
            get => _selectedItem;
            set => SetProperty(ref _selectedItem, value);
        }

        private bool _isCollectionEmpty;
        public bool IsCollectionEmpty
        {
            get => _isCollectionEmpty;
            private set => SetProperty(ref _isCollectionEmpty, value);
        }

        public List<string> DebuggerOptions { get; } = new List<string> { "Managed", "Native" };
        public List<string> AttachProcessOptions { get; } = new List<string> { "Always", "I'm debugging its immediate parent", "I'm debugging any of its parents", "I'm not already debugging its exe" };
        public List<string> ProcessStartOptions { get; } = new List<string> { "Continue", "Break" };
        public List<string> ConditionsOptions { get; } = new List<string> { "Add", "None" };

        public DataGridViewModel()
        {
            Items.CollectionChanged += (s, e) =>
            {
                UpdateIsEmpty();
                RemoveItemCommand.NotifyCanExecuteChanged();
            };

            AddItemCommand = new RelayCommand(AddNewItem);
            RemoveItemCommand = new RelayCommand(RemoveSelectedItem, CanRemoveSelectedItem);
            ClearSelectionCommand = new RelayCommand(ClearSelection);

            UpdateIsEmpty();
        }

        private void AddNewItem()
        {
            var newItem = new DataGridModel()
            {
                Filename = "myexe.exe",
                Debugger = "Native",
                AttachAtProcessStartWhen = "I'm not already debugging its exe",
                AtProcessStart = "Continue",
                Conditions = "None"
            };

            Items.Add(newItem);
        }

        private void RemoveSelectedItem()
        {
            if (SelectedItem != null)
            {
                Items.Remove(SelectedItem);
                SelectedItem = null;
            }
            else if (Items.Any())
            {
                Items.RemoveAt(Items.Count - 1);
            }

            RemoveItemCommand.NotifyCanExecuteChanged();
        }

        private bool CanRemoveSelectedItem()
        {
            return Items.Any();
        }

        public void ClearSelection()
        {
            SelectedItem = null;
        }

        private void UpdateIsEmpty()
        {
            IsCollectionEmpty = Items.Count == 0;
        }

        public void UpdateItems(ObservableCollection<DataGridModel> items)
        {
            Items.Clear();

            foreach (var item in items)
            {
                Items.Add(item);
            }

            UpdateIsEmpty();
        }
    }
}
