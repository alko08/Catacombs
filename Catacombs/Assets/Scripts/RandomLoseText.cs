using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RandomLoseText : MonoBehaviour
{
    private float rand;
    private TextMeshProUGUI loseText;
    // Start is called before the first frame update
    void Start()
    {
        loseText = GetComponent<TextMeshProUGUI>();
        rand = Random.Range(0f, 5.0f);
        if (rand < 1f) {
            loseText.SetText("The monster can detect light." +
            " Turn off the flashlight when hiding.");
        } else if (rand < 2f) {
            loseText.SetText("The monster can hear running when it is nearby." +
            " Try to be quieter.");
        } else if (rand < 3f) {
            loseText.SetText("After a chase, the monster will pause before" + 
            " it starts patroling again.");
        } else if (rand < 4f) {
            loseText.SetText("The monster is slow." + 
            " Try to out run it or dodge it when it gets close.");
        } else {
            loseText.SetText("Next time\n" + 
            "avoid the monster.");
        }
    }
}
