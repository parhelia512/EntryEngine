using System.Collections.Generic;

namespace EntryEngine.DragonBone.DBCore
{
    /// <internal/>
    /// <private/>
    public class VerticesData
    {
        public bool isShared;
        public bool inheritDeform;
        public int offset;
        public DragonBonesData data;
        public WeightData weight; // Initial value.

        public void Clear()
        {
            if (!this.isShared && this.weight != null)
            {
                this.weight.ReturnToPool();
            }

            this.isShared = false;
            this.inheritDeform = false;
            this.offset = 0;
            this.data = null;
            this.weight = null;
        }

        public void ShareFrom(VerticesData value)
        {
            this.isShared = true;
            this.offset = value.offset;
            this.weight = value.weight;
        }
    }
    /// <internal/>
    /// <private/>
    public abstract class DisplayData : BaseObject
    {
        public DisplayType type;
        public string name;
        public string path;
        public SkinData parent;
        public readonly Transform transform = new Transform();

        protected override void _OnClear()
        {
            this.name = "";
            this.path = "";
            this.transform.Identity();
            this.parent = null; //
        }
    }

    /// <internal/>
    /// <private/>
    public class ImageDisplayData : DisplayData
    {
        public readonly Point pivot = new Point();
        public TextureData texture = null;

        protected override void _OnClear()
        {
            base._OnClear();

            this.type = DisplayType.Image;
            this.pivot.Clear();
            this.texture = null;
        }
    }

    /// <internal/>
    /// <private/>
    public class ArmatureDisplayData : DisplayData
    {
        public bool inheritAnimation;
        public readonly List<ActionData> actions = new List<ActionData>();
        public ArmatureData armature = null;

        protected override void _OnClear()
        {
            base._OnClear();

            foreach (var action in this.actions)
            {
                action.ReturnToPool();
            }

            this.type = DisplayType.Armature;
            this.inheritAnimation = false;
            this.actions.Clear();
            this.armature = null;
        }

        /// <private/>
        internal void AddAction(ActionData value)
        {
            this.actions.Add(value);
        }
    }

    /// <internal/>
    /// <private/>
    public class MeshDisplayData : DisplayData
    {
        public readonly VerticesData vertices = new VerticesData();
        public TextureData texture;

        protected override void _OnClear()
        {
            base._OnClear();

            this.type = DisplayType.Mesh;
            this.vertices.Clear();
            this.texture = null;
        }
    }

    /// <internal/>
    /// <private/>
    public class BoundingBoxDisplayData : DisplayData
    {
        public BoundingBoxData boundingBox = null; // Initial value.

        protected override void _OnClear()
        {
            base._OnClear();

            if (this.boundingBox != null)
            {
                this.boundingBox.ReturnToPool();
            }

            this.type = DisplayType.BoundingBox;
            this.boundingBox = null;
        }
    }

    /// <internal/>
    /// <private/>
    public class PathDisplayData : DisplayData
    {
        public bool closed;
        public bool constantSpeed;
        public readonly VerticesData vertices = new VerticesData();
        public readonly List<float> curveLengths = new List<float>();

        protected override void _OnClear()
        {
            base._OnClear();

            this.type = DisplayType.Path;
            this.closed = false;
            this.constantSpeed = false;
            this.vertices.Clear();
            this.curveLengths.Clear();
        }
    }

    /// <internal/>
    /// <private/>
    public class WeightData : BaseObject
    {
        public int count;
        public int offset; // IntArray.
        public readonly List<BoneData> bones = new List<BoneData>();

        protected override void _OnClear()
        {
            this.count = 0;
            this.offset = 0;
            this.bones.Clear();
        }

        internal void AddBone(BoneData value)
        {
            this.bones.Add(value);
        }
    }
}
