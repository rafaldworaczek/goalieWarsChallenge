using UnityEngine;
using UnityEngine.UI;
namespace GameBench
{
    public class UIManager : MonoBehaviour
    {
        public SpinTurnTimer spinTimer;
        TurnType spinTurnType = TurnType.PaidOnly;
        public Text coinsText, nextFreeSpinText, paidTurnCostText, animTextCost;
        public const string COINS_COUNT = "COINS_COUNT";
        public GameObject _animCost;
        private void Awake()
        {
            Coins = PlayerPrefs.GetInt(COINS_COUNT, 3000);
            chunkSp = Resources.LoadAll<Sprite>("Skins/Chunk");
            dotSp = Resources.LoadAll<Sprite>("Skins/Dot");
            circleBorderSp = Resources.LoadAll<Sprite>("Skins/Border");
            arrowSp = Resources.LoadAll<Sprite>("Skins/BgArrow");
            SetTurnType();
            switch (spinTurnType)
            {
                case TurnType.PaidOnly:
                    spinBtnPaid.gameObject.SetActive(true);
                    //paidTurnCostText.text = (-SpinWheelSetup.Instance.spinTurnCost).ToString();
                    //spinTimer.gameObject.SetActive(false);
                    //spinBtnFree.gameObject.SetActive(false);
                    break;
                case TurnType.FreeOnly:
                    spinBtnFree.gameObject.SetActive(true);
                    //spinTimer.gameObject.SetActive(false);
                    //spinBtnPaid.gameObject.SetActive(false);
                    break;
                case TurnType.Both:
                    spinBtnPaid.gameObject.SetActive(true);
                    //spinTimer.gameObject.SetActive(true);
                    //paidTurnCostText.text = (-SpinWheelSetup.Instance.spinTurnCost).ToString();
                    //spinBtnFree.gameObject.SetActive(false);
                    break;
            }

            //spinBtnFree.onClick.AddListener(OnClickFreeSpin);
            //spinBtnPaid.onClick.AddListener(OnClickPaidSpin);
        }
        [HideInInspector]
        public Sprite[] chunkSp, dotSp, circleBorderSp, arrowSp;
        int _coins;

        public Button spinBtnFree, spinBtnPaid;
        public int Coins
        {
            get { return _coins; }
            set
            {
                _coins = value;
                //coinsText.text = _coins.ToString();
            }
        }
        void SetTurnType()
        {
            if (!SpinWheelSetup.Instance.freeTurn && !SpinWheelSetup.Instance.paidTurn
                || (SpinWheelSetup.Instance.freeTurn && !SpinWheelSetup.Instance.paidTurn))
            {
                spinTurnType = TurnType.FreeOnly;
            }
            else if (SpinWheelSetup.Instance.freeTurn && SpinWheelSetup.Instance.paidTurn)
            {
                spinTurnType = TurnType.Both;
            }
            else if (SpinWheelSetup.Instance.paidTurn && !SpinWheelSetup.Instance.freeTurn)
            {
                spinTurnType = TurnType.PaidOnly;
            }
        }
        public void ActivateFreeSpin()
        {
            spinBtnFree.interactable = true;
            spinBtnFree.gameObject.SetActive(true);
            spinBtnPaid.gameObject.SetActive(false);
        }
        public void OnClickFreeSpin()
        {
            spinBtnFree.interactable = false;
            SpinWheel.Instance.StartSpin();
        }
        public void OnClickPaidSpin()
        {
            //if (Coins >= SpinWheelSetup.Instance.spinTurnCost)
            //{
            //    ManipulateCoins(-SpinWheelSetup.Instance.spinTurnCost);
                spinBtnPaid.interactable = false;
                SpinWheel.Instance.StartSpin();
            //}
        }
        bool IsPaidTurnPossible()
        {
            return Coins >= SpinWheelSetup.Instance.spinTurnCost;
        }
        public void SpinCompleted()
        {
            if (spinTurnType == TurnType.FreeOnly)
            {
                spinBtnFree.interactable = true;
            }
            else if (spinTurnType == TurnType.Both)
            {
                spinBtnPaid.gameObject.SetActive(true);
                spinBtnPaid.interactable = IsPaidTurnPossible();
                spinTimer.gameObject.SetActive(true);
            }
            else if (spinTurnType == TurnType.PaidOnly)
            {
                spinBtnPaid.interactable = IsPaidTurnPossible();
            }
        }
        public void ManipulateCoins(int val)
        {
            animTextCost.text = string.Format("{0} {1}", val > 0 ? "+" : "", val);
            _animCost.gameObject.SetActive(true);
            Coins += val;
        }

        private void OnApplicationPause(bool pause)
        {
            if (pause)
            {
                PlayerPrefs.SetInt(COINS_COUNT, Coins);
                PlayerPrefs.Save();
            }
        }
        private static UIManager _instance;
        public static UIManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = GameObject.FindObjectOfType<UIManager>();
                }

                return _instance;
            }
        }
    }
}
public enum TurnType
{
    PaidOnly = 0,
    FreeOnly,
    Both
}