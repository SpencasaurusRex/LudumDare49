using System.Collections;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour {
    // Configuration
    public Plate leftPlate;
    public Plate rightPlate;
    public Box[] BoxPrefabs;
    public float StartingTime = 60;
    public TMP_Text TimeLeftLabel;

    // Runtime
    bool timerRunning;
    bool gameOver;
    float timeRemaining;

    void Start() {
        timeRemaining = StartingTime;
        SetupRound();
    }

    void Update() {
        if (gameOver) {
            // TODO: Restart button
            return;
        }
        if (leftPlate.NumberOfBoxes == rightPlate.NumberOfBoxes) {
            StartCoroutine(FinishRound());
        }
        if (timerRunning) {
            timeRemaining -= Time.deltaTime;
            if (timeRemaining <= 0) {
                StartCoroutine(GameOver());
            }
            else {
                Controls();
            }
        }
        TimeLeftLabel.text = $"Time Left: {Mathf.Ceil(timeRemaining)}";
    }

    IEnumerator GameOver() {
        gameOver = true;
        timerRunning = false;
        // TODO
        yield return new WaitForSeconds(0);
    }

    IEnumerator FinishRound() {
        timerRunning = false;
        // TODO: Write "Stabilized" on screen
        yield return new WaitForSeconds(3.0f);
        SetupRound();
    }

    void SetupRound() {
        timerRunning = true;

        var boxCounts = GetStartingCounts();
        // print($"Starting counts: Left: {c[0]}r {c[1]}g {c[2]}b   Right: {c[3]}r {c[4]}g {c[5]}b");
        for (int i = 0; i < 3; i++) {
            for (int c = 0; c < boxCounts[i]; c++) {
                var b = Instantiate(BoxPrefabs[i]);
                leftPlate.AddBox(b);
            }
        }
        for (int i = 3; i < 6; i++) {
            for (int c = 0; c < boxCounts[i]; c++) {
                var b = Instantiate(BoxPrefabs[i - 3]);
                rightPlate.AddBox(b);
            }
        }
    }

    const int MaxCount = 6;
    int[] GetStartingCounts() {
        int leftCount = 0;
        int rightCount = 0;
        int[] counts = new int[6];

        while (true) {
            leftCount = 0;
            rightCount = 0;
            counts = new int[6];

            for (int i = 0; i < 3; i++) {
                var c = Random.Range(0, MaxCount + 1);
                var maxLeft = MaxCount - leftCount;
                if (c > maxLeft) c = maxLeft;
                if (c < 0) c = 0;
                counts[i] = c;
                leftCount += c;
                //print($"{i} {c} {leftCount}");
            }
            for (int i = 3; i < 6; i++) {
                var c = Random.Range(0, MaxCount + 1);
                c = Mathf.Clamp(c, 0, MaxCount - rightCount);
                counts[i] = c;
                rightCount += c;
            }
            if ((leftCount + rightCount) % 2 == 1) {
                if (leftCount < MaxCount) {
                    for (int i = 0; i < 3; i++) {
                        counts[i]++;
                    }
                }
                else {
                    for (int i = 3; i < 6; i++) {
                        counts[i]++;
                    }
                }
            }

            //print($"Checking {counts[0]} {counts[1]} {counts[2]} {counts[3]} {counts[4]} {counts[5]}");

            if (Solvable(counts)) {
                return counts;
            }
        }
    }

    const int MinSteps = 1;
    const int MaxSteps = 4;
    bool Solvable(int[] counts) {
        //var buffer = new StringBuilder();
        var s = StepSolve(0, counts[0], counts[1], counts[2], counts[3], counts[4], counts[5]/*, buffer*/);
        //if (s >= MinSteps && s <= MaxSteps) {
        //    File.WriteAllText(@"C:\users\miste\Desktop\debug.txt", buffer.ToString());
        //}
        return s >= MinSteps && s <= MaxSteps;
    }

    int StepSolve(int step, int lr, int lg, int lb, int rr, int rg, int rb/*, StringBuilder sb*/) {
        void print(string s) {
            // sb.AppendLine(s);
        }

        if (lr + lg + lb == rr + rg + rb) {
            // Solved
            return step;
        }
        if (step >= MaxSteps) {
            return -1;
        }
        int minStepCount = 999;
        for (int i = 0; i < 6; i++) {
            int rightCount = rr + rg + rb;
            int leftCount = lr + lg + lb;
            if (i == 0) {
                // Move red left
                int amount = Mathf.Min(MaxCount - leftCount, rr);
                //lr += amount;
                //rr -= amount;
                print($"{new string('\t', step)} red left: {lr + amount} {lg} {lb} {rr - amount} {rg} {rb}");
                int s = StepSolve(step + 1, lr + amount, lg, lb, rr - amount, rg, rb/*, sb*/);
                if (s != -1 && s < minStepCount) {
                    minStepCount = s;
                }
            }
            else if (i == 1) {
                // Move green left
                int amount = Mathf.Min(MaxCount - leftCount, rg);
                //lg += amount;
                //rg -= amount;
                print($"{new string('\t', step)} green left: {lr} {lg + amount} {lb} {rr} {rg - amount} {rb}");
                int s = StepSolve(step + 1, lr, lg + amount, lb, rr, rg - amount, rb/*, sb*/);
                if (s != -1 && s < minStepCount) {
                    minStepCount = s;
                }
            }
            else if (i == 2) {
                // Move blue left
                int amount = Mathf.Min(MaxCount - leftCount, rb);
                //lb += amount;
                //rb -= amount;
                print($"{new string('\t', step)} blue left: {lr} {lg} {lb + amount} {rr} {rg} {rb - amount}");
                int s = StepSolve(step + 1, lr, lg, lb + amount, rr, rg, rb - amount/*, sb*/);
                if (s != -1 && s < minStepCount) {
                    minStepCount = s;
                }
            }
            else if (i == 3) {
                // Move red right
                int amount = Mathf.Min(MaxCount - rightCount, lr);
                //rr += amount;
                //lr -= amount;
                print($"{new string('\t', step)} red right: {lr - amount} {lg} {lb} {rr + amount} {rg} {rb}");
                int s = StepSolve(step + 1, lr - amount, lg, lb, rr + amount, rg, rb/*, sb*/);
                if (s != -1 && s < minStepCount) {
                    minStepCount = s;
                }
            }
            else if (i == 4) {
                // Move green right
                int amount = Mathf.Min(MaxCount - rightCount, lg);
                //rg += amount;
                //lg -= amount;
                print($"{new string('\t', step)} green right: {lr} {lg - amount} {lb} {rr} {rg + amount} {rb}");
                int s = StepSolve(step + 1, lr, lg - amount, lb, rr, rg + amount, rb/*, sb*/);
                if (s != -1 && s < minStepCount) {
                    minStepCount = s;
                }
            }
            else if (i == 5) {
                // Move blue right
                int amount = Mathf.Min(MaxCount - rightCount, lb);
                //rb += amount;
                //lb -= amount;
                print($"{new string('\t', step)} blue right: {lr} {lg} {lb - amount} {rr} {rg} {rb + amount}");
                int s = StepSolve(step + 1, lr, lg, lb - amount, rr, rg, rb + amount/*, sb*/);
                if (s != -1 && s < minStepCount) {
                    minStepCount = s;
                }
            }
        }
        if (minStepCount > 0 && minStepCount != 999) {
            // Solvable!
            return minStepCount;
        }
        return -1;
    }

    void Controls() {
        if (Input.GetKeyDown(KeyCode.Q)) {
            Move(BoxColor.Red, rightPlate, leftPlate);
        }
        if (Input.GetKeyDown(KeyCode.E)) {
            Move(BoxColor.Red, leftPlate, rightPlate);
        }
        if (Input.GetKeyDown(KeyCode.A)) {
            Move(BoxColor.Green, rightPlate, leftPlate);
        }
        if (Input.GetKeyDown(KeyCode.D)) {
            Move(BoxColor.Green, leftPlate, rightPlate);
        }
        if (Input.GetKeyDown(KeyCode.Z)) {
            Move(BoxColor.Blue, rightPlate, leftPlate);
        }
        if (Input.GetKeyDown(KeyCode.C)) {
            Move(BoxColor.Blue, leftPlate, rightPlate);
        }
    }

    void Move(BoxColor color, Plate from, Plate to) {
        int remaining = MaxCount - to.NumberOfBoxes;
        if (remaining == 0) return;

        print("Moving " + remaining + " boxes from " + from.name + " to " + to.name);

        for (int i = 0; i < MaxCount; i++) {
            var b = from.RemoveBox(color, out var has);
            if (has) {
                to.AddBox(b);
                remaining--;
                if (remaining <= 0) {
                    break;
                }
            }
            else break;
        }
    }

    void DebugControls() {
        if (Input.GetKeyDown(KeyCode.A)) {
            var box = Instantiate(BoxPrefabs[0]);
            if (!leftPlate.AddBox(box)) {
                Destroy(box.gameObject);
            }
        }
        if (Input.GetKeyDown(KeyCode.D)) {
            var box = Instantiate(BoxPrefabs[1]);
            if (!rightPlate.AddBox(box)) {
                Destroy(box.gameObject);
            }
        }
    }
}
