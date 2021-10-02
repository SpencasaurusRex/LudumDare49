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

    public bool AddBox(Box box) {
        for (int i = 0; i < boxes.Length; i++) {
            if (boxes[i] == null) {
                box.transform.parent = Positions[i];
                box.transform.localPosition = Vector3.zero;
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
                has = true;
                return b;
            }
        }
        has = false;
        return null;
    }
}
