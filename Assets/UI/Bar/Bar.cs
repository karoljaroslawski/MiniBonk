using UnityEngine;

public class Bar : MonoBehaviour
{
    public RectTransform barActual;

    public void SetValue(float value)
    {
        barActual.localScale = new Vector3(value, 1f, 1f);
    }
}
