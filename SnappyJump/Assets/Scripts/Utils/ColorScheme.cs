using UnityEngine;

[CreateAssetMenu(fileName = "ColorScheme", menuName = "ColorScheme/default", order = 1)]
public class ColorScheme : ScriptableObject
{
    public Color _background;
    public Color _primary;
    public Color _secondary;
    public Color _warning;
    public Color _danger;
    public Color _accent;
    public Color _accept;
    public Color _text;
    public Color _textAccent;
    public Color _buttonPrimary;
    public Color _buttonSecondary;
    public Color _buttonText;
    public Color _buttonAccent;
    public Color _buttonTextAccent;
}
