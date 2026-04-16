using UnityEngine;
using UnityEngine.UI;

public class CameraBobbing : MonoBehaviour
{
    public Slider bobbingSlider;

    private float timer = 0f;
    private Vector3 originalPosition;

    void Start()
    {
  
        originalPosition = transform.localPosition;

  
        if (bobbingSlider != null)
        {
            bobbingSlider.minValue = 0f;
            bobbingSlider.maxValue = 10f;
            bobbingSlider.value = 0f; 
        }
    }

    void Update()
    {
    
        float speed = 0f;
        if (bobbingSlider != null)
        {
            speed = bobbingSlider.value;
        }

 
        bool isMoving = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) ||
                        Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) ||
                        Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) ||
                        Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow);

   
        bool isLeftMouseDown = Input.GetMouseButton(0);


        if (isMoving && speed > 0f && !isLeftMouseDown)
        {
            timer += Time.deltaTime * speed;

            float yBob = Mathf.Sin(timer) * 0.05f;
            float xBob = Mathf.Cos(timer * 0.5f) * 0.025f;

            transform.localPosition = originalPosition + new Vector3(xBob, yBob, 0f);
        }
        else
        {
       
            timer = 0f;
            transform.localPosition = Vector3.Lerp(transform.localPosition, originalPosition, Time.deltaTime * 10f);
        }
    }
}