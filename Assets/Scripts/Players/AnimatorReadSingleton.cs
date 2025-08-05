using UnityEngine;

namespace TwoOnPlane.Players
{
    public class AnimatorReadSingleton : MonoBehaviour
    {
        public static AnimatorReadSingleton s_Instance { get; private set; }

        [SerializeField] private GameObject firstPlayerPrefab;
        [SerializeField] private GameObject secondPlayerPrefab;

        private Animator firstAnimator;
        private Animator secondAnimator;

        public Animator FirstAnimator => firstAnimator;
        public Animator SecondAnimator => secondAnimator;

        void Awake()
        {
            s_Instance = this;
            firstAnimator = firstPlayerPrefab.GetComponentInChildren<Animator>();
            secondAnimator = secondPlayerPrefab.GetComponentInChildren<Animator>();
        }
    }
}