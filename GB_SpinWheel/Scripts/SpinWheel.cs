using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using GlobalsNS;
using AudioManagerNS;

namespace GameBench
{
    public class SpinWheel : MonoBehaviour
    {
        public Dropdown dropDown;
        public SpriteRenderer circleBorderSpRend, arrowBgSpRend;
        public AudioClip pointHitSound;
        public Transform chunksParent;
        public Sprite[] pointSp, chunkSp;
        public Sprite circleBorderSp, arrowBgSp;
        public AnimationCurve animationCurve;
        private bool spinning;
        private float anglePerItem;
        private int randomTime;
        int _selectReward;
        AudioSource[] audSource;
        ChunkSlice[] chunkSlices;
        public TextMeshProUGUI currentCoinsText;
        public TextMeshProUGUI currentDiamondsText;
        private AudioManager audioManager;

        public int SelectedReward
        {
            get
            {
                return _selectReward;
            }
            set
            {
                _selectReward = Mathf.Clamp(value, 0, SpinWheelSetup.Instance.rewarItem.Length);
            }
        }

        void Start()
        {
            updateTreasure();
            int index = (int)SpinWheelSetup.Instance.theme;
            chunkSp[0] = UIManager.Instance.chunkSp[index];
            pointSp[0] = UIManager.Instance.dotSp[index];
            circleBorderSp = UIManager.Instance.circleBorderSp[index];
            arrowBgSp = UIManager.Instance.arrowSp[index];
            SetupWheel();
            audioManager = FindObjectOfType<AudioManager>();

            //dropDown.onValueChanged.AddListener(OnThemeChanged);
        }

        void SetWheelTheme(int index)
        {
            chunkSp[0] = UIManager.Instance.chunkSp[index];
            pointSp[0] = UIManager.Instance.dotSp[index];
            circleBorderSp = UIManager.Instance.circleBorderSp[index];
            arrowBgSp = UIManager.Instance.arrowSp[index];

            //SetupWheel();

            circleBorderSpRend.sprite = circleBorderSp;
            arrowBgSpRend.sprite = arrowBgSp;

        }
        void SetupWheel()
        {
            spinning = false;
            anglePerItem = 360 / SpinWheelSetup.Instance.rewarItem.Length;
            chunkSlices = chunksParent.GetComponentsInChildren<ChunkSlice>();
            for (int i = 0; i < 8; i++)
            {
                chunkSlices[i].transform.localEulerAngles = new Vector3(0, 0, (i * -anglePerItem));
            }
            circleBorderSpRend.sprite = circleBorderSp;
            arrowBgSpRend.sprite = arrowBgSp;
            audSource = new AudioSource[5];
            for (int i = 0; i < 5; i++)
            {
                AudioSource source = gameObject.AddComponent<AudioSource>();
                source.playOnAwake = false;
                source.loop = false;
                audSource[i] = source;
            }
            WheelAnimation(true);
        }
        public void WheelAnimation(bool playAnim)
        {
            foreach (var item in chunkSlices)
            {
                item.AnimatePoints(playAnim);
                item.spRend.sprite = chunkSp[0];
            }
            if (playAnim)
            {
                StartCoroutine(PlayLoopRoutine(chunkSp[0], chunkSp[1]));
            }
            else
            {
                StopAllCoroutines();
            }
        }
        int count = 0;
        IEnumerator PlayChunkPatternRoutine(int index)
        {
            if (index > 7)
            {
                count = 0;
                StartCoroutine(PlayLoopRoutine(chunkSp[0], chunkSp[1]));
            }
            else
            {
                chunkSlices[index].spRend.sprite = chunkSp[1];
                yield return new WaitForSeconds(0.1f);
                chunkSlices[index].spRend.sprite = chunkSp[0];
                yield return new WaitForSeconds(0.0f);
                StartCoroutine(PlayChunkPatternRoutine(index + 1));
            }
        }
        IEnumerator PlayLoopRoutine(Sprite sp1, Sprite sp2)
        {
            yield return new WaitForSeconds(0.2f);
            count++;
            for (int i = 0; i < chunkSlices.Length; i++)
            {
                chunkSlices[i].spRend.sprite = (i % 2 == 0) ? sp1 : sp2;
            }
            if (count < Random.Range(10, 25))
            {
                StartCoroutine(PlayLoopRoutine(sp2, sp1));
            }
            else
            {
                StartCoroutine(PlayChunkPatternRoutine(0));
            }
        }
        public void PlayHitClip()
        {
            for (int i = 0; i < audSource.Length; i++)
            {
                if (!audSource[i].isPlaying)
                {
                    audSource[i].clip = pointHitSound;
                    audSource[i].Play();
                    break;
                }
            }
        }
        private static SpinWheel _instance;
        public static SpinWheel Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = GameObject.FindObjectOfType<SpinWheel>();
                }
                return _instance;
            }
        }
        public void StartSpin()
        {
            if (!spinning)
            {
                randomTime = Random.Range(1, 3);
                float maxAngle = (SpinWheelSetup.Instance.speed * 360) * randomTime + (Random.Range(0, 8) * anglePerItem);
                WheelAnimation(false);
                StartCoroutine(SpinTheWheel(2.8f * randomTime, maxAngle));
            }
        }
        IEnumerator SpinTheWheel(float time, float maxAngle)
        {
            spinning = true;
            float timer = 0.0f;
            float startAngle = transform.eulerAngles.z;
            maxAngle = maxAngle - startAngle;
            while (timer < time)
            {
                //to calculate rotation
                float angle = maxAngle * animationCurve.Evaluate(timer / time);
                transform.eulerAngles = new Vector3(0.0f, 0.0f, angle + startAngle);
                timer += Time.deltaTime;
                yield return 0;
            }
            transform.eulerAngles = new Vector3(0.0f, 0.0f, maxAngle + startAngle);
            WheelAnimation(true);
            RewardPlayer();
            spinning = false;
        }
        void RewardPlayer()
        {
            //print("You have won " + SelectedReward);
            UIManager.Instance.SpinCompleted();
            int quantity = SpinWheelSetup.Instance.rewarItem[SelectedReward].rewardQuantity;
            if (quantity < 0) quantity = 0;
            if (quantity > 100) quantity = 100;


            if (SpinWheelSetup.Instance.rewarItem[SelectedReward].rewardType == RewardType.Coin)
            {                
                Globals.addCoins(quantity);
                currentCoinsText.text = Globals.coins.ToString();                
            } else if (SpinWheelSetup.Instance.rewarItem[SelectedReward].rewardType == RewardType.Diamond)
            {
                Globals.addDiamonds(quantity);
            }

            updateTreasure();
            audioManager.Play("elementAppear");
        }

        private void updateTreasure()
        {
            updateGlobalCoinsText();
            updateGlobalDiamondText();
        }

        private void updateGlobalCoinsText()
        {
            currentCoinsText = GameObject.Find("shopCurrentCoinsText").GetComponent<TextMeshProUGUI>();
            currentCoinsText.text = Globals.coins.ToString();
        }

        private void updateGlobalDiamondText()
        {
            currentDiamondsText = GameObject.Find("shopCurrentDiamondsText").GetComponent<TextMeshProUGUI>();
            currentDiamondsText.text = Globals.diamonds.ToString();
        }

        internal void HitStart(SpriteRenderer sp)
        {
            PlayHitClip();
            sp.sprite = pointSp[1];
        }
        internal void HitEnd(SpriteRenderer sp)
        {
            sp.sprite = pointSp[0];
        }
        public void OnThemeChanged(int themeID)
        {
            SetWheelTheme(themeID);
        }
    }

    [System.Serializable]

    public class RewardItem
    {
        public Sprite rewardSprite;
        public int rewardQuantity;
        public RewardType rewardType;
    }
    public enum RewardType
    {
        Coin,
        Diamond,
        Time,
        Magnet,
        Shield
    }
}