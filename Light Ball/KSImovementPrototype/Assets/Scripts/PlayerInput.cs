using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour 
{
    public Texture2D padBackground;
    public Texture2D padController;
    private bool isMovingFinger = false;
    private Vector2 padControllerPosition = Vector2.zero;
    private Vector2 padBackgroundPosition = Vector2.zero;
    private float joystickSize = 100.0f;

    public const float padRadius = 5.0f;

    public Vector2 movement = Vector2.zero;


    public void Update()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.touches[0];
            // get the finger position and transform
            // the coordinate system to top-to-bottom:
            Vector2 touchPosition = new Vector2(
                touch.position.x,
                Screen.height - touch.position.y
            );

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    this.isMovingFinger = true;
                    this.padControllerPosition = touchPosition;
                    this.padBackgroundPosition = touchPosition;
                    break;

                case TouchPhase.Moved:
                    this.padControllerPosition = touchPosition;

                    // get the distance between the two pads
                    float padsDistance = Vector2.Distance(
                        this.padBackgroundPosition,
                        this.padControllerPosition
                    );

                    if (padsDistance > PlayerInput.padRadius)
                    {
                        // get the direction of the movement (B-A)
                        Vector2 padDirection = this.padControllerPosition;
                        padDirection -= this.padBackgroundPosition;

                        // get the normalized relation between the max
                        // threshold and the current distance between the two pads:
                        float t = PlayerInput.padRadius / padsDistance;

                        // calculate the pad background position by linearly
                        // interpolating the two pads positions with the
                        // normalized relation from above:
                        this.padBackgroundPosition = Vector2.Lerp(
                            this.padControllerPosition,
                            this.padBackgroundPosition,
                            t
                        );

                        // get the direction of the pad:
                        Vector2 direction = this.padControllerPosition;
                        direction -= this.padBackgroundPosition;

                        // normalize it and store the value in the movement
                        // variable:
                        this.movement = direction.normalized;
                    }
                    break;

                case TouchPhase.Ended:
                    this.isMovingFinger = false;
                    break;
            }
        }
    }

    public void OnGUI()
    {
        if (this.isMovingFinger)
        {
            Rect backgroundRect = new Rect(
                this.padBackgroundPosition.x - (this.joystickSize / 2.0f),
                this.padBackgroundPosition.y - (this.joystickSize / 2.0f),
                this.joystickSize,
                this.joystickSize
            );

            Rect controllerRect = new Rect(
                this.padControllerPosition.x - (this.joystickSize / 2.0f),
                this.padControllerPosition.y - (this.joystickSize / 2.0f),
                this.joystickSize,
                this.joystickSize
            );

            GUI.DrawTexture(backgroundRect, this.padBackground);
            GUI.DrawTexture(controllerRect, this.padController);

            GUI.Box(new Rect((Screen.width / 2) - 100, (Screen.height / 2) - 100, 100, 100), "" + movement);
        }
    }
}
