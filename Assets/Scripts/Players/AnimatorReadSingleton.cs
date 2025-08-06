using UnityEngine;

namespace TwoOnPlane.Players
{
    public class AnimatorReadSingleton : MonoBehaviour
    {
        public static AnimatorReadSingleton s_Instance { get; private set; }

        [SerializeField] private GameObject firstPlayerPrefab;
        [SerializeField] private GameObject secondPlayerPrefab;

        public Animator FirstAnimator { get; private set; }
        public Animator SecondAnimator { get; private set; }

        void Awake()
        {
            s_Instance = this;
            FirstAnimator = firstPlayerPrefab.GetComponentInChildren<Animator>();
            SecondAnimator = secondPlayerPrefab.GetComponentInChildren<Animator>();
        }
    }
}