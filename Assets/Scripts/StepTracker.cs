    using BeliefEngine.HealthKit;
    using System;
    using UnityEngine;
    using Riptide;

[RequireComponent(typeof(HealthStore))]
[RequireComponent(typeof(HealthKitDataTypes))]

///<summary>
/// This script is for getting the steps of the user out of the apple health api
/// </summary>
///<author>Jackson Codd</author>
///<version>1.0 Build 2024.11.29</version>
public class StepTracker : MonoBehaviour
{
    ///<summary>
    /// The total steps of the user
    ///</summary>
    public int totalSteps = 0;

    ///<summary>
    /// The health store that will be used to get the steps
    ///</summary>
    HealthStore healthStore;

    ///<summary>
    /// The data types that will be used to get the steps
    ///</summary>
    HealthKitDataTypes dataTypes;

    ///<summary>
    /// The steps of the user
    ///</summary>
    public int Steps { get; private set; }

    ///<summary>
    /// The singleton instance of the StepTracker
    ///</summary>
    private static StepTracker _singleton;
    public static StepTracker Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
            {
                _singleton = value;
            }
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(StepTracker)} instance already exists, destroying duplicate!");
                Destroy(value);
            }

        }
    }

    ///<summary>
    /// Instantiate the health kit variables.
    ///</summary>
    private void OnEnable()
    {
        this.healthStore = GetComponent<HealthStore>();
        this.dataTypes = GetComponent<HealthKitDataTypes>();
        this.healthStore.Authorize(this.dataTypes);
    }

    ///<summary>
    /// Set the total steps of the user
    ///</summary>
    ///<param name="_steps">The total steps of the user</param>
    public void SetTotalSteps(int _steps)
    {
        totalSteps = _steps;
    }

    ///<summary> 
    /// Get the steps of the user and add them to the total steps of the user
    ///</summary>
    public void GetSteps()
    {
        print("Getting Steps from the last month...");
        this.healthStore = GetComponent<HealthStore>();
        this.dataTypes = GetComponent<HealthKitDataTypes>();
        this.healthStore.Authorize(this.dataTypes);
        DateTimeOffset now = DateTimeOffset.UtcNow;
        DateTimeOffset firstDayOfMonth = new DateTime(now.Year, now.Month, 1);
        this.healthStore.ReadSteps(firstDayOfMonth, now, delegate (double steps, Error error) {
            print($"Steps: {steps}");
            Steps = (int)steps - totalSteps;
        });
    }
   
    ///<summary>
    /// instantiate the singleton instance of the StepTracker
    ///</summary>
    private void Awake()
    {
        Singleton = this;

    }
}


