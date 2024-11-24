    using BeliefEngine.HealthKit;
    using System;
    using UnityEngine;
    using Riptide;


public class StepTracker : MonoBehaviour
{
    public int totalSteps = 0;
    HealthStore healthStore;
    HealthKitDataTypes dataTypes;
    public int Steps { get; private set; }
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
    private void OnEnable()
    {
        this.healthStore = GetComponent<HealthStore>();
        this.dataTypes = GetComponent<HealthKitDataTypes>();
        this.healthStore.Authorize(this.dataTypes);
    }
    public void SetTotalSteps(int _steps)
    {
        totalSteps = _steps;
    }
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
   

    private void Awake()
    {
        Singleton = this;

    }


}


