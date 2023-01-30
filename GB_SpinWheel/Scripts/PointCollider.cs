using UnityEngine;
namespace GameBench
{
    public class PointCollider : MonoBehaviour
    {
        public SpriteRenderer pointSprite;
        void Start()
        {
            if (pointSprite == null)
                pointSprite = GetComponent<SpriteRenderer>();
        }

        void OnCollisionEnter2D(Collision2D coll)
        {
            SpinWheel.Instance.HitStart(pointSprite);
        }
        void OnCollisionExit2D(Collision2D collisionInfo)
        {
            SpinWheel.Instance.HitEnd(pointSprite);
        }
    }
}