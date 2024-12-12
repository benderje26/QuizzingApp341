using System.ComponentModel;

namespace QuizzingApp341.Models;

public class IndexValuePair(int index, string value) : INotifyPropertyChanged {
    public int Index { get; set; } = index;
    public string Value { get; set; } = value;
    public bool IsSelected {
        get => isSelected;
        set {
            isSelected = value;
            OnPropertyChanged(nameof(IsSelected));
        }
    }
    private bool isSelected;

    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged(string propertyName) =>
       PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

}
