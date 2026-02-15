using System.Collections.Generic;
using UnityEngine;

namespace Configuration
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "TowerOfBlocks/GameConfig")]
    public sealed class GameConfig : ScriptableObject
    {
        [Header("Blocks")]
        [SerializeField] private Sprite[] _blockSprites = new Sprite[20];

        [Header("Tower Bounds")]
        [SerializeField] private TowerBoundsSettings _towerBounds = new TowerBoundsSettings
        {
            HorizontalMarginPercent = 0.2f,
            BottomMarginPercent = 0f,
            TopMarginPercent = 0f,
            BlockHeight = 135f
        };

        [Header("Animations")]
        [SerializeField] private AnimationSettings _animations = new AnimationSettings
        {
            PlaceBounce = new PlaceBounceSettings { BounceUpDuration = 0.15f, BounceDownDuration = 0.15f, BounceOffsetPixels = 20f },
            HoleFall = new HoleFallSettings { ArcHeight = 120f, ArcDuration = 0.4f, FadeDuration = 0.2f },
            CollapseDown = new CollapseDownSettings { Duration = 0.2f },
            MissDisappear = new MissDisappearSettings { FadeDuration = 0.2f, HideDelay = 0.2f },
            DragGhost = new DragGhostSettings { Alpha = 0.6f },
            Timing = new AnimationTimingSettings { RemoveAfterAnimationSeconds = 0.6f }
        };

        [Header("Save")]
        [SerializeField] private string _saveKey = "TowerState";

        public int BlocksCount => _blockSprites.Length;
        public IReadOnlyList<Sprite> BlockSprites => _blockSprites;
        public Sprite GetSprite(int index) => index >= 0 && index < _blockSprites.Length ? _blockSprites[index] : null;

        public TowerBoundsSettings TowerBounds => _towerBounds;
        public AnimationSettings Animations => _animations;
        public string SaveKey => _saveKey;
        public float DragGhostAlpha => _animations.DragGhost.Alpha;
    }
}
