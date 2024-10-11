namespace Curiologix
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine.UI;
    using UnityEngine;
    using System;

    public class TimerForSpin : MonoBehaviour
    {
        public static TimerForSpin _instance;

        public FortuneWheel spin;
        string TimerForWhat = "Spin";
        [HideInInspector]
        public bool canClaimReward;
        public float claimCooldown = 24f;
        public float claimDeadLine = 200f;
        [SerializeField]
        private Button ClaimBtn;
        [SerializeField]
        private Text[] Status;
        bool isClaimActive = false;
        bool isGiveReward = false;
        private DateTime? lastClaimTime
        {
            get
            {
                string data = PlayerPrefs.GetString(TimerForWhat + "lastClaimTime", null);

                if (!string.IsNullOrEmpty(data))
                {
                    return DateTime.Parse(data);
                }
                return null;
            }
            set
            {
                if (value != null)
                {
                    PlayerPrefs.SetString(TimerForWhat + "lastClaimTime", value.ToString());
                }
                else
                {
                    PlayerPrefs.DeleteKey(TimerForWhat + "lastClaimTime");
                }
            }
        }
        private void Awake()
        {
            _instance = this;
        }
        private void Start()
        {

            ClaimBtn.onClick.AddListener(() => ClaimReward());
            StartCoroutine(RewardStateUpdater());
        }
        IEnumerator RewardStateUpdater()
        {
            while (true)
            {
                UpdateRewardState();
                yield return new WaitForSeconds(1);
            }
        }
        void UpdateRewardState()
        {
            canClaimReward = true;
            if (lastClaimTime.HasValue)
            {
                var timespan = DateTime.UtcNow - lastClaimTime.Value;
                if (timespan.TotalHours > claimDeadLine)
                {
                    lastClaimTime = null;
                }
                else if (timespan.TotalHours < claimCooldown)
                {
                    canClaimReward = false;
                }
            }
            if (isGiveReward)
            {
                canClaimReward = true;
            }
            UpdateRewardUI();
        }
        void UpdateRewardUI()
        {
            /*ClaimBtn.interactable = canClaimReward;*/
            if (canClaimReward)
            {
                textUpdate(TimerForWhat);
            }
            else
            {
                var nextClaimTime = lastClaimTime.Value.AddHours(claimCooldown);
                var currentClaimCoolDown = nextClaimTime - DateTime.UtcNow;
                string cd = $"{currentClaimCoolDown.Hours:D2}:{currentClaimCoolDown.Minutes:D2}:{currentClaimCoolDown.Seconds:D2}";
                textUpdate(cd);
            }
        }
        void textUpdate(string status)
        {
            for (int i = 0; i < Status.Length; i++)
            {
                Status[i].text = status;
            }
        }
        public GameObject watchVideoPanal;
        public void SpinTemp()
        {
            PlayerPrefs.SetInt("Hidebtn", 0);
        }
        public void ClaimReward()
        {
            if (!canClaimReward)
            {
                Debug.Log("Reward Spin");
                /*rewardCount video chla kar ya wala function call karna ha
                    StartSpin();*/
                if (PlayerPrefs.GetInt("Hidebtn") == 0)
                    watchVideoPanal.SetActive(true);
            }
            else
            {
                ClaimBtn.interactable = false;
                Debug.Log("Reward Spin Ab");
                spin.StartSpin();
                isGiveReward = false;
                lastClaimTime = DateTime.UtcNow;
                UpdateRewardState();
            }
        }
        public void watch_video()
        {
            if (FindObjectOfType<Handler>())
                FindObjectOfType<Handler>().ShowRewardedAdsBoth(spin.StartSpin);
        }
    }
}