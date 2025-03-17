using TMPro;
using UnityEngine;

public class UI_Tooltips_Stat : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI description;

    public void EnableTooltips(string _description)
    {
        description.text = _description;

        CorrectPosition();
        gameObject.SetActive(true);
    }

    public void DisableTooltips()
    {
        gameObject.SetActive(false);
    }

    private void CorrectPosition()
    {
        Vector2 mousePos = Input.mousePosition;
        Vector2 offset = new(150, 150);
        if (mousePos.x > Screen.width / 2) offset.x *= -1;
        if (mousePos.y > Screen.height / 2) offset.y *= -1;
        transform.position = mousePos + offset;
    }
}
