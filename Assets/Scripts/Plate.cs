using UnityEngine;
using System.Linq;

public class Plate : MonoBehaviour {
    // Configuration
    public Transform[] Positions;

    // Runtime
    Box[] boxes;
    public int NumberOfBoxes => boxes?.Count(x => x != null) ?? 0;

    void Awake() {
        boxes = new Box[Positions.Length];
    }

    public void Clear() {
        for (int i = 0; i < boxes.Length; i++) {
            var b = boxes[i];
            Destroy(b.gameObject);
            boxes[i] = null;
        }
    }

    public bool AddBox(Box box) {
        for (int i = 0; i < boxes.Length; i++) {
            if (boxes[i] == null) {
                //box.transform.parent = Positions[i];
                //box.transform.localPosition = Vector3.zero;
                box.SetTarget(Positions[i]);
                boxes[i] = box;
                return true;
            }
        }
        return false;
    }

    public Box RemoveBox(BoxColor color, out bool has) {
        for (int i = boxes.Length - 1; i >= 0; i--) {
            var b = boxes[i];
            if (b != null && b.Color == color) {
                boxes[i] = null;
                has = true;
                return b;
            }
        }
        has = false;
        return null;
    }
}
