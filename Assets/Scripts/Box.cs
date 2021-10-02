using UnityEngine;

public class Box : MonoBehaviour {
    // Configuration
    public BoxColor Color;
    public Transform Target;
    [Range(0.01f, 0.3f)]
    public float AnimationSpeed;
    bool ySnapped;
    bool snapped;

    void Update() {
        if (snapped) return;
        
        transform.position = Vector3.Lerp(transform.position, Target.position, AnimationSpeed);

        // Extra weight to keep up with moving scale
        transform.position = Vector3.MoveTowards(transform.position, Target.position, Time.deltaTime * 3);
        
        //if (yDist < .01f) {
        //    ySnapped = true;
        //}
        //if (ySnapped) {
        //    transform.position = new Vector3(transform.position.x, Target.position.y, transform.position.z);
        //}
        
        if ((transform.position - Target.position).magnitude < .01f) {
            snapped = true;
            transform.parent = Target;
            transform.localPosition = Vector3.zero;
        }
    }

    public void SetTarget(Transform target) {
        ySnapped = false;
        snapped = false;
        transform.parent = null;
        Target = target;
    }
}