using UnityEngine;
using UnityEngine.UI;

public class P1ScoreScript : MonoBehaviour
{

    public static int p1ScoreValue = 0;

    Text score;

    // Start is called before the first frame update
    void Start()
    {
        score = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        score.text = "Score: " + p1ScoreValue;
    }
}
