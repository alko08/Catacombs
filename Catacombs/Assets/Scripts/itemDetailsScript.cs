using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class itemDetailsScript : MonoBehaviour
{
    public inventoryScript inventory;
    public Button button;

    // Components of Details Screen.
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemBio;
    public GameObject itemImage;
    public bool bookOpenTrigger;
    
    // Start is called before the first frame update
    void Start()
    {
        inventory = GameObject.Find("EventSystem").GetComponent<inventoryScript>();
        button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(TaskOnClick);

        itemName = GameObject.Find("details_name").GetComponent<TextMeshProUGUI>();
        itemBio = GameObject.Find("details_bio").GetComponent<TextMeshProUGUI>();
        itemImage = GameObject.Find("details_image");

        bookOpenTrigger = false;
    }

    // Function to change the details screen.
    void TaskOnClick()
    {
        Debug.Log("Button Pressed");
        
        itemName.text = this.name;
        itemImage.GetComponent<RawImage>().texture 
            = gameObject.GetComponent<RawImage>().texture;

        itemBio.text = findItemBio(this.name);

        // Update bookOpenTrigger.
        if ( (this.name == "Green Book") && (bookOpenTrigger == false) ) {
            activateBookTrigger();
        }
    }

    // Function that returns an item bio depending on the name of the item. As
    // more items get added, this function will get longer and messier.
    string findItemBio(string itemName)
    {
        string bio;

        if (itemName.Contains("testBook")) {
            bio = "Nothing to see here. Just a test.";
        } 
        
        else if (itemName.Contains("A Battery")) {
            bio = "Charges the flashlight an extra 50%.";
        }

        else if (itemName.Contains("A Music-Maker")) {
            bio = "Produces loud music and distracts anything nearby.";
        }

        else if (itemName.Contains("A Ring of Keys")) {
            bio = "A ring of keys. Unlocks locked doors.";
        } 
        
        else if (itemName.Contains("Welcome")) {
            bio = "I see you got past the monster. Congratulations. I'm sorry " +
                  "to say that this is the furthest you will get. Turn around " +
                  "now or face the consequences.";
        } 
        
        else if (itemName.Contains("A Hint")) {
            bio = "If journal is what you seek, search the opposite corner " +
                  "of the library.";
        } 

        else if (itemName.Contains("Journal")) {
            bio = "We had no choice but to wipe your memory. You simply " +
                  "learned too much. We suggest that you leave before you " +
                  "uncover our secrets again.";
        }

        else if (itemName.Contains("Rats")) {
            bio = "There are rats in the walls. There are rats in the walls. " +
                  "There are rats in the walls. There are rats in the walls. ";
        }

        else if (itemName.Contains("A Poem")) {
            bio = "In the dark, I seem alone.\n" + "But there's " +
                  "a monster, my light has shown.";
        }

        else if (itemName.Contains("The Wretch")) {
            bio = "There is a monster that lurks between the shelves. It " +
                  "is called the wretch. It hungers for human flesh, and " +
                  "fears light";
        }

        else if (itemName.Contains("Notes 1")) {
            bio = "I believe that there's something sinister happening " +
                  "under Tisch. Students have gone missing, and monsters have " +
                  "begun lurking the halls.";
        }

        else if (itemName.Contains("Notes 2")) {
            bio = "I have come down here to investigate those disappearances, " +
                  "and get to the bottom of what's behind the monsters. Wish " +
                  "me luck.";
        }

        else if (itemName.Contains("The Meldon Archives: Vol 1")) {
            bio = "\n \n \n \n Shark Meldon Returns";
        }

        else if (itemName.Contains("Turn Back")) {
            bio = "In case our warning was insufficient, know that we've " +
                  "placed a guardian on this level. Hopefully it will keep " +
                  "you at bay.";
        }

        else if (itemName.Contains("Midas Project 1")) {
            bio = "April 12, 2018. After using over $2 million of student " +
                  "tuition money, we've finally managed to distill pure " +
                  "Monaconium. The future of the project looks bright.";
        }

        else if (itemName.Contains("Midas Project 2")) {
            bio = "However, several students have begun wondering what their " +
                  "tuition is being used for here. Some have gotten dangerously" +
                  "close to finding the truth...";
        }

        else if (itemName.Contains("2nd Hint")) {
            bio = "Around a corner and a turn.\n" + 
                  "More secrets you will learn.";
        }

        else if (itemName.Contains("A Warning")) {
            bio = "I thought we asked you to leave. Very well. If you intend " +
                  "on continuing your pointless search for answers, know that " +
                  "there are more monsters ahead.";
        }

        else if (itemName.Contains("It Listens")) {
            bio = "You are safe outside the glass. Watch the monster's movements. " +
                  "Predict its movements. Hide under tables. Get the keys. Progress.";
        }

        else if (itemName.Contains("Tread Softly")) {
            bio = "Sprinting draws the monster's attention. Do so only when it " +
                  "isn't nearby, or when absolutely necessary.";
        }

        else if (itemName.Contains("Stay Hidden")) {
            bio = "Hide under desks to avoid its sight and turn off the light!";
        }

        else if (itemName.Contains("The Final Hours")) {
            bio = "\"Joey, a student at Tuft University, is in his last round of" +
            " finals when he realizes a killer is after him.\"" + 
            " THE FINAL HOURS, coming to a tufts bookstore near you";
        }

        else if (itemName.Contains("The Final Hours 2")) {
            bio =  "Will Joey be able to finish finals, or will something find" +
            " him first? Spoilers: it's both";
        }

        else if (itemName.Contains("Pebble Ghosts")) {
            bio = "Pebbles (or stones) that have had a constitiuant removed by" +
            " chemical weathering so that the pebble has lower density";
        }

        else if (itemName.Contains("Blythe The Bug")) {
            bio = "While searching through the library, I found this bug and get this," + 
            " HE CAN TALK. I don't know how or why, but at least he is friendly." +
            " He told me his name is Blythe...";
        }

        else if (itemName.Contains("Pots & Sculptures")) {
            bio = "A historical analysis of the progression of scultpure carvings" +
            " and pots iconography from the Neolithic Greek Period to the Imperium" +
            " Romanum Justinian Dynasty...";
        }

        else if (itemName.Contains("Milod Quotes 1")) {
            bio = "- \"You don't need to understand my code. C++ library go brrrr\"\n" +
            "- \"Self plagiarism is BEAUTIFUL\"\n" + 
            "- \"You played with sticks. Now here's a machete. Go have fun\"";
        }

        else if (itemName.Contains("Pots & Sculptures 2")) {
            bio = "These times show the evolution of the greek" +
            " and roman culture over time with their shifts in ideals over elite" +
            " iconography, the proper spending of money, and what makes a pot popular.";
        }

        else if (itemName.Contains("It Is Hunting You")) {
            bio = "Its always listening. Always patrolling. The only place safe" +
            " from its search is to leave this library. Its protecting something," +
            " but what?";
        }

        else if (itemName.Contains("Technology & Magic")) {
            bio = "\"Any sufficiently advanced technology is indistinguishable from magic\" - Arthur C. Clarke";
        }

        else if (itemName.Contains("Loud Sounds")) {
            bio = "The monster hates loud sounds so I created a device to distract it." +
            " Throw it far away either when in danger or when trying to sneak past it.";
        }

        else if (itemName.Contains("The Maze")) {
            bio = "To go deeper you are going to have to find the key at the center of the maze." +
            " Good luck, you are going to need it.\nThe monster is growing restless...";
        }

        else if (itemName.Contains("IT FOUND ME")) {
            bio = "The Wretch, it found me. I... I got away but not unharmed." +
            " I noticed its attracted to sound like the bug told me. I should have" +
            " listened to him more and kept slient...";
        }

        else if (itemName.Contains("HELP ME")) {
            bio = "THE NOISE.... IT WON'T GO AWAY... PLEASE... PUT A STOP TO THIS...";
        }

        else if (itemName.Contains("Dogs and Cats")) {
            bio = "Who doesn't want a pet? This book will" +
            " tell you how to acquire and raise the two most common pets there are," +
            " dogs and cats! First acquire a dog or cat...";
        }

        else if (itemName.Contains("Among Us Car")) {
            bio = "All hail Among Us Car.\nFound beneath the stairs.\n???";
        }

        else if (itemName.Contains("Computer Science")) {
            bio = "Ever wanted to spend hours stairing at a computer screen?" +
            " Well then a job in the massive field of computer science is the" +
            " right job for you! ";
        }

        else if (itemName.Contains("Lab Assitants")) {
            bio = "Come work on the Midas project with top Tuft's proffesors!" +
            "\nRequirements:" +
            "\n - Has nobody who will look for them if they go missing." +
            "\n - Doesn't care about silly things such as safety...";
        }

        else if (itemName.Contains("Chronicles of Big Sash 1")) {
            bio = "Whoops I Committed Vehicular Man Slaughter:\n" +
            "As Big Sash put pulled out of the Taco Bell parking lot at 2am," +
            " he thought everything would be fine, until he heard a thud and scream...";
        }

        else if (itemName.Contains("Chronicles of Big Sash 2")) {
            bio = "Big Sash and the Taco Bell Incident:\n" +
            "Big Sash thought the police had forgotten about his 2am shenanigans," +
            " until officer dikenbal asked him where he was that night...";
        }

        else if (itemName.Contains("Chronicles of Big Sash 3")) {
            bio = "Big Sash on Death Row:\n" +
            "Big Sash thought his alibi of playing league at 2am was flawless," +
            " until he saw the horrified faces of the judge and jury...";
        }

        else if (itemName.Contains("Chronicles of Big Sash 4")) {
            bio = "The Second Coming of Big Sash:\n" +
            "The prison guards may have thought killing Big Sash would be easy," +
            " boy were they wrong. Next time they should have destroyed his bed first...";
        }

        else if (itemName.Contains("Chronicles of Big Sash 5")) {
            bio = "Big Sash vs the Irs:\n" +
            "The good thing about being dead is you don't need to pay your taxes," +
            " or so big sash thought... until the tax monster showed up outside his house...";
        }

        else if (itemName.Contains("Who Wrote These Books?")) {
            bio = "Guest Writers:\n" +
            "- \"The Final Hours\" by Amanda\n" +
            "- \"Pebble Ghosts\" & \"Pots & Sculptures\" by Anna\n" +
            "- \"Chronicles of Big Sash\" by Sasha";
        }

        else if (itemName.Contains("The Catacombs")) {
            bio = "This is it, the Catacombs. Every secret of the Midas Project" +
            " is located here.\n Be careful...";
        }

        
        
        else {
            bio = "ITEM UNKNOWN";
        }

        return bio;
    }

    // Function that lets mission_01 know when to print the 2nd dialogue text.
    void activateBookTrigger()
    {
        Debug.Log("GreenBook Open");
        // GameObject.Find("missionManager").GetComponent<mission_01>().bookTextTrigger2 = true;
        bookOpenTrigger = true;
    }
}
