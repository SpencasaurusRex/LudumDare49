using UnityEngine;

public class Scale : MonoBehaviour {
    
    // Configuration
    public Transform Bar;
    public Plate LeftPlate;
    public Plate RightPlate;
    public float TiltScale;

    // Runtime
    public float tilt;
    // TODO polish on the tilt animation

    void Start() {
        tilt = 0;
    }

    void Update() {
        float delta = RightPlate.NumberOfBoxes - LeftPlate.NumberOfBoxes;
        float targetTilt = delta * TiltScale;

        tilt = Mathf.Lerp(tilt, targetTilt, Time.deltaTime * 4);

        UpdatePositions();
    }

    void UpdatePositions() {
        // Update bar
        Bar.rotation = Quaternion.Euler(0, 0, -tilt);

        // Update plates
        var barLeft = Bar.transform.position - Bar.transform.right * Bar.transform.localScale.x * 3.3f;
        LeftPlate.transform.position = barLeft;
        
        var barRight = Bar.transform.position + Bar.transform.right * Bar.transform.localScale.x * 3.3f;
        RightPlate.transform.position = barRight;
    }

    public void Clear() {
        LeftPlate.Clear();
        RightPlate.Clear();
    }
}
