using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    private Transform cam;
    private float length;
    private float startPosX;
    private float startPosY;
    [SerializeField] private float parallaxEffectX;
    [SerializeField] private float parallaxEffectY;

    private void Start()
    {
        cam = GameObject.Find("Main Camera").transform;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
        startPosX = GetComponent<Transform>().position.x;
        startPosY = GetComponent<Transform>().position.y;
    }

    private void Update()
    {
        // Parallax Effect
        float distanceX = cam.position.x * parallaxEffectX;
        float distanceY = cam.position.y * parallaxEffectY;
        transform.position = new Vector3(startPosX + distanceX, startPosY + distanceY);

        // Endless Background
        float movement = cam.position.x * (1 - parallaxEffectX);
        // Go to the right edge
        if (movement > startPosX + length)
        {
            startPosX += length;
        }
        // Go to the left edge
        else if (movement < startPosX - length)
        {
            startPosX -= length;
        }
    }
}
