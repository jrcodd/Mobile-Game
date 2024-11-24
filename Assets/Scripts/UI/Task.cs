using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class task_Script : MonoBehaviour
{
    [Header("Task Reward")]
    [SerializeField] private int xpReward = 100; // xp reward for completing the task

    [Header("UI")]
    [SerializeField] private Sprite originalImage; // original image is the image that is displayed when the task is not completed
    [SerializeField] private Sprite completedImage; // completed image is the image that is displayed when the task is completed
    [SerializeField] private Slider competionSlider; // slider that shows the progress of the task
    [SerializeField] private TextMeshProUGUI coinsText; // text that shows the amount of coins the player has

    private int currentProgress; // current progress of the task
    private PlayerCharacter player; // player object

    // Start is called before the first frame update
    void OnEnable()
    {
        //TODO: get current progress from database value that corresponds with player id
        player = PlayerCharacter.Singleton;
        //for now set the progress to 0%
        currentProgress = 0;

        //100% complete
        competionSlider.maxValue = 100;

        // set the slider value to the current progress
        competionSlider.value = currentProgress;

        //set the task to the original image so it can be completed
        gameObject.GetComponent<Image>().sprite = originalImage;
    }

    // update the progress of the task on click before we check with the apple watch and other stuff
    public void Click()
    {
        //click increases progress by 10%
        currentProgress += 10;
        //update the slider value
        competionSlider.value = currentProgress;

        //give rewards when task is completed and set the image to completedImage
        if (currentProgress == competionSlider.maxValue)
        {
            gameObject.GetComponent<Image>().sprite = completedImage;
            player.AddXp(xpReward);
            player.AddCoins(10);
            coinsText.text = PlayerCharacter.Singleton.GetCoins().ToString();
        }
    }
}
