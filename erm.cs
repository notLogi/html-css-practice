using System.Collections; // Imported 2 namespaces: system.collections(equivalent to using iostream in C++, standard functions needed)
using UnityEngine; //unity engine(functionalities for the unity engine)


public class DeskWorker : Employee { //public class DeskWorker that inherits from a base class Employee
    private bool isSenior;   //a boolean used to determine if the employee is a senior

    [SerializeField] private Sprite[] seniorFaceSprites = new Sprite[6];
    [SerializeField] private Sprite[] seniorBodySprites = new Sprite[7];
	//making an array for face/body sprites

    public int revenue = 3000; //initialize 3 int variables for revenue
    public int bigRevenue = 5000; //big = 5000
    public int massiveRevenue = 10000; //massikve = 10000
    //I think AudioClip is a function that plays the audio?(not sure)
    [SerializeField] private AudioClip makeRevenueAudio; //plays the audioclip "makeRevenueAudio"
    [SerializeField] private AudioClip makeBigRevenueAudio;//plays the audioclip "makeBigRevenueAudio"
    [SerializeField] private AudioClip makeMassiveRevenueAudio;//plays the audioclip "makeMassiveRevenueAudio"
    [SerializeField] private GameObject promotionAnimationPrefab;//prefab for the promotion animation?
    [SerializeField] private AudioClip promotionAudio;//plays the audioclip “promotionAudio”
    protected new void Start() {//
        base.Start(); //The “base” employee calls the start method
        StartCoroutine(StartTenure()); //starts the coroutine by using the return value of StartTenure()
    }
//Protected - methods that can be accessed by classes inherited by the superclass
    private IEnumerator StartTenure() { //StartTenure method, not sure 
        if (company.flagsManager.GetLockSeniorWorkerFlag()) {//if true for getlockseniorworkerflag, then 
            yield break;//break out of the loop
        }

        var random1 = rng.Next(1, 6); //like auto in C++, uses rng to grab numbers from 1-6          	
        var random2 = rng.Next(1, 6); // same thing here
        var aggregate = random1 + random2; //adds up random1 and random2
        var tenure = aggregate * 60; //multiply result by 60
        if (tenure > 600) { //if tenure is more than 600 seconds
            yield break; //stop the loop
        }

        yield return new WaitForSeconds(tenure);//wait for how many seconds tenure is equal to.

        if (promotionAudio != null) { //if there is a promotionAudio
            audioSource.PlayOneShot(promotionAudio);//calls the method PlayOneShot from the instance audioSource
        }
        Instantiate(promotionAnimationPrefab, this.transform.position,
            Quaternion.identity);//calls the method instantiate with 3 parameters


        yield return new WaitForSeconds(0.3f);//wait for 0.3 seconds before returning?

        isSenior = true; //sets the isSenior boolean to true.

        if (!currentlyWorking) {//if it is not working, set body sprites to 1
            SetBodySprites(1);
        }   
        SetFaceSprites(happiness); //calls the method to set face sprites to happiness

        company.SetAchievement("SENIOR_WORKER"); //calls in the method in the instance company to set the achievement "SENIOR_WORKER"
    }

    protected override IEnumerator DoWork() {
        StopIdleTimer();//to stop the timer
        var startTime = Time.time; //stores the current time into startTime

        timer.gameObject.SetActive(true); //activate the timer 

        while (isSenior 
            ? Time.time < startTime + (2 * workDuration)
            : Time.time < startTime + workDuration) {//if it's a senior, it will check if the current time is less than the start time plus 2 times the duration
                                                     // if false, checks if the current time is less than the start time + the duration
            var percentageDone = (Time.time - startTime) / workDuration; //percentage is determined by the current time - start time divided by duration
            var timerAngle = percentageDone * 360; //store the calculation of the timer based on the percentage
            timer.SetHandDegrees(timerAngle); //sets the timer angle
            yield return new WaitForSeconds(0.01f); //wait for 0.01 seconds before looping again
        }

        timer.gameObject.SetActive(false);//deactvate the timer

        FinishWork(); //calls in the finishwork method
    }

    protected override void FinishWork() {
        base.FinishWork(); //calls the base finishwork function

        var random = rng.Next(0, 100); //store a random number from 1-100

        Color green = Color.green; //green color
        if (isSenior) {//if employee is a senior
            if (random == 0) {//if random = 0
          totalMoneyEarned += 2 * massiveRevenue;//multiply massiveRevenue by 2
                company.MakeMoney(2 * massiveRevenue);//call company’s makemoney and adds the revenue to the company
                StartCoroutine(statusIcons.AnimateMakeHugeMoneySprites()); //calls in the startCoroutine method     
                ShowFloatingText("+$" + $"{(2 * massiveRevenue / 1000):n0}" + "K", green); //shows the amount of money there is in green text
                if (makeMassiveRevenueAudio != null) { //if there is an audio, play it.
                    audioSource.PlayOneShot(makeMassiveRevenueAudio);
                }
            } else if (random < 6) { //else if random is less than 6
                totalMoneyEarned += 2 * bigRevenue;//total money earned will be incremented by 2 times the bigRevenue
                company.MakeMoney(2 * bigRevenue);//call in the MakeMoney method in the instance company, adding 2 times the bigRevenue
                StartCoroutine(statusIcons.AnimateMakeMoreMoneySprites()); //starts the coroutine to animate sprites
                ShowFloatingText("+$" + $"{(2 * bigRevenue / 1000):n0}" + "K", green); //outputs the amount of money there is in green text
                if (makeBigRevenueAudio != null) {//if there is an audio, play it.
                    audioSource.PlayOneShot(makeBigRevenueAudio);
                }
            } else {
                totalMoneyEarned += 2 * revenue;
                company.MakeMoney(2 * revenue);
                StartCoroutine(statusIcons.AnimateMakeMoneySprites());//starts the coroutine to animate sprites
                ShowFloatingText("+$" + $"{(2 * revenue / 1000):n0}" + "K", green);//outputs the amount of money there is in green text
                if (makeRevenueAudio != null) {//if there is an audio, play it.
                    audioSource.PlayOneShot(makeRevenueAudio);
                }
            }
        } else {
            if (random == 0) {//if the random number is 0
                totalMoneyEarned += massiveRevenue; //add the total amount of money with the massive revenue
                company.MakeMoney(massiveRevenue); //add the total money to the company's money
                StartCoroutine(statusIcons.AnimateMakeHugeMoneySprites()); //starts the coroutine to animate sprites
                ShowFloatingText("+$" + $"{(massiveRevenue / 1000):n0}" + "K", green);//starts the coroutine to animate sprites
                if (makeMassiveRevenueAudio != null) {//if there is an audio, play it.
                    audioSource.PlayOneShot(makeMassiveRevenueAudio);
                }
            } else if (random < 6) {//if the random number is less than 6
                totalMoneyEarned += bigRevenue;// add the total amount of money earned by the number that corresponds with big revenue
                company.MakeMoney(bigRevenue); //adds it to the company's money
                StartCoroutine(statusIcons.AnimateMakeMoreMoneySprites());//starts the coroutine to animate sprites
                ShowFloatingText("+$" + $"{(bigRevenue / 1000):n0}" + "K", green);//outputs the amount of money there is in green text
                if (makeBigRevenueAudio != null) {//if there is an audio, play it.
                    audioSource.PlayOneShot(makeBigRevenueAudio);
                }
            } else {
                totalMoneyEarned += revenue; //adds the total amount of money earned by the revenue
                company.MakeMoney(revenue); //add the money into the company's money
                StartCoroutine(statusIcons.AnimateMakeMoneySprites());//starts the coroutine to animate sprites
                ShowFloatingText("+$" + $"{(revenue / 1000):n0}" + "K", green);//outputs the amount of money there is in green text
                if (makeRevenueAudio != null) {//if there is an audio, play it.
                    audioSource.PlayOneShot(makeRevenueAudio);
                }
            }
        }

    }

    protected override bool IsEmployeeTheRightSeniority(Employee employee) {  //override the method if the employee is a right seniority
        return false;
    }

    protected override void ApplyHappinessPenalties() { //override the method to apply penalties
        workDuration += 2; //increment the workDuration by 2
        base.ApplyHappinessPenalties();//calls in the base method to apply happiness penalties
    }

    protected override void ApplyHappinessPerks() { //override the method to apply happiness perks
        workDuration -= 2; //decrement the workDuration by 2
        base.ApplyHappinessPerks(); //calls in the base method to apply happiness perks
    }

    protected override void SetBodySprites(int spriteIndex) {//override the method to set body sprites
        if (isSenior) {//if the instance is a senior
            body.sprite = seniorBodySprites[spriteIndex];// set body sprite from the senior body sprites
        } else {//else, call the base method and set body sprites to the spriteIndex
            base.SetBodySprites(spriteIndex);
        }
    }

    protected override void SetFaceSprites(int happinessIndex) {//override the method to set face sprites
        if (isSenior) {//if it is a senior
            face.sprite = seniorFaceSprites[happinessIndex]; //set sprite to seniorFaceSprites array 
        } else {//else, call the base method to set face sprites to the happinessIndex
            base.SetFaceSprites(happinessIndex);
        }
    }

    public override EmployeeType Type() { //overrides the Type method and returns the desk worker from the employeetype instance.
        return EmployeeType.DeskWorker;
    }
}

