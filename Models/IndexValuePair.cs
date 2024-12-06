namespace QuizzingApp341.Models;

public class IndexValuePair(int index, string value) {
    public int Index { get { return index; } }
    public string Value { get { return value; } }
    public bool IsSelected { get; set; }
}
