using DamageSystem;
using UnityEngine;
using VRBeats.ScriptableEvents;

namespace VRBeats
{
    public class VR_BeatCube : MonoBehaviour
    {
        [SerializeField] private float minCutSpeed = 0.1f;
        [SerializeField] private OnSliceAction sliceAction = null;
        [SerializeField] private GameEvent onCorrectSlice = null;
        [SerializeField] private GameEvent onIncorrectSlice = null;
        [SerializeField] private GameEvent onPlayerMiss = null;
        [SerializeField] private IntGameEvent onPunchArea = null;


        private MaterialBindings materialBindings = null;
        private ColorSide thisColorSide = ColorSide.Right;
        private Transform player = null;
        private VR_BeatCubeSpawneable thisSpawneable = null;
        private Collider perfectCollider;
        private bool canBeKilled = true;
        private bool spawnComplete = false;
        private bool destroyed = false;

        public float MinCutSpeed
        {
            get { return minCutSpeed; }
        }

        public Direction HitDirection
        {
            get { return thisSpawneable.HitDirection; }
        }

        public ColorSide ThisColorSide
        {
            get { return thisColorSide; }
        }

        private void Awake()
        {
            player = VR_BeatManager.instance.Player.transform;
        }

        public void Start()
        {
            thisSpawneable = GetComponent<VR_BeatCubeSpawneable>();
            thisSpawneable.onSpawnComplete += delegate { spawnComplete = true; };

            materialBindings = GetComponent<MaterialBindings>();

            thisColorSide = thisSpawneable.ColorSide;
            Color color = VR_BeatManager.instance.GetColorFromColorSide(thisColorSide);
            materialBindings.SetEmmisiveColor(color);
        }

        private void OnDestroy()
        {
            destroyed = true;
        }

        public void OnCut(DamageInfo info)
        {
            canBeKilled = false;

            //notify to whoever is listening that the player did a correct/incorrect slice
            if (IsCutIntentValid(info as BeatDamageInfo))
            {
                onCorrectSlice.Invoke();
            }
            else
            {
                onIncorrectSlice.Invoke();
            }

            
            if (transform.position.z > player.position.z + 0.75f|| transform.position.z < player.position.z + 0.25f)
            {
                onPunchArea?.Invoke(1);
            }
            else if (transform.position.z > player.position.z && transform.position.z <= player.position.z + 0.75f && transform.position.z >= player.position.z + 0.25f)
            {
                onPunchArea?.Invoke(2);
            }
        }

        private bool IsCutIntentValid(BeatDamageInfo info)
        {
            if (info == null) return false;

            if (info.velocity < minCutSpeed) return false;

            //no matter the hit direction as soon as we have the right velocity for a cube that has a dot
            if (HitDirection == Direction.Center)
                return true;

            float cutAngle = Vector2.Angle(transform.up, info.hitDir);
            return info.colorSide == ThisColorSide && cutAngle < 80.0f;
        }

        private void Update()
        {
            if (spawnComplete)
                transform.position += transform.forward * -5 * Time.deltaTime;
                // transform.position += transform.forward * thisSpawneable.Speed * Time.deltaTime;

            if (ShouldKillCube())
            {
                onPunchArea?.Invoke(0);
                Kill();
            }
        }

        private bool ShouldKillCube()
        {
            return canBeKilled && transform.position.z < player.position.z - 2.0f;
        }

        public void Kill()
        {
            onPlayerMiss.Invoke();
            canBeKilled = false;
            transform.ScaleTween(Vector3.zero, 2.0f).SetEase(Ease.EaseOutExpo).SetOnComplete(delegate
            {
                if (!destroyed)
                    Destroy(gameObject);
            });
        }
    }
}