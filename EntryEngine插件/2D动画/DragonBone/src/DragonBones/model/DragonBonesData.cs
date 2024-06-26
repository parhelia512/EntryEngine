﻿using System;
using System.Collections.Generic;
using System.Text;

 namespace EntryEngine.DragonBone.DBCore
{
    /// <summary>
    /// - The DragonBones data.
    /// A DragonBones data contains multiple armature data.
    /// </summary>
    /// <see cref="DragonBones.ArmatureData"/>
    /// <version>DragonBones 3.0</version>
    /// <language>en_US</language>

    /// <summary>
    /// - 龙骨数据。
    /// 一个龙骨数据包含多个骨架数据。
    /// </summary>
    /// <see cref="DragonBones.ArmatureData"/>
    /// <version>DragonBones 3.0</version>
    /// <language>zh_CN</language>
    public class DragonBonesData : BaseObject
    {
        /// <private/>
        public bool autoSearch;
        /// <summary>
        /// - The animation frame rate.
        /// </summary>
        /// <version>DragonBones 3.0</version>
        /// <language>en_US</language>

        /// <summary>
        /// - 动画帧频。
        /// </summary>
        /// <version>DragonBones 3.0</version>
        /// <language>zh_CN</language>
        public uint frameRate;
        /// <summary>
        /// - The data version.
        /// </summary>
        /// <version>DragonBones 3.0</version>
        /// <language>en_US</language>

        /// <summary>
        /// - 数据版本。
        /// </summary>
        /// <version>DragonBones 3.0</version>
        /// <language>zh_CN</language>
        public string version;
        /// <summary>
        /// - The DragonBones data name.
        /// The name is consistent with the DragonBones project name.
        /// </summary>
        /// <version>DragonBones 3.0</version>
        /// <language>en_US</language>

        /// <summary>
        /// - 龙骨数据名称。
        /// 该名称与龙骨项目名保持一致。
        /// </summary>
        /// <version>DragonBones 3.0</version>
        /// <language>zh_CN</language>
        public string name;
        /// <private/>
        public ArmatureData stage;
        /// <internal/>
        /// <private/>
        public readonly List<uint> frameIndices = new List<uint>();
        /// <internal/>
        /// <private/>
        public readonly List<float> cachedFrames = new List<float>();
        /// <summary>
        /// - All armature data names.
        /// </summary>
        /// <version>DragonBones 3.0</version>
        /// <language>en_US</language>

        /// <summary>
        /// - 所有的骨架数据名称。
        /// </summary>
        /// <version>DragonBones 3.0</version>
        /// <language>zh_CN</language>
        public readonly List<string> armatureNames = new List<string>();
        /// <private/>
        public readonly Dictionary<string, ArmatureData> armatures = new Dictionary<string, ArmatureData>();
        /// <internal/>
        /// <private/>
        internal byte[] binary;
        /// <internal/>
        /// <private/>
        internal short[] intArray;
        /// <internal/>
        /// <private/>
        internal float[] floatArray;
        /// <internal/>
        /// <private/>
        internal short[] frameIntArray;
        /// <internal/>
        /// <private/>
        internal float[] frameFloatArray;
        /// <internal/>
        /// <private/>
        internal short[] frameArray;
        /// <internal/>
        /// <private/>
        internal ushort[] timelineArray;
        /// <private/>
        internal UserData userData = null; // Initial value.

        /// <inheritDoc/>
        protected override void _OnClear()
        {
            foreach (var k in this.armatures.Keys)
            {
                this.armatures[k].ReturnToPool();
            }

            if (this.userData != null)
            {
                this.userData.ReturnToPool();
            }

            this.autoSearch = false;
            this.frameRate = 0;
            this.version = "";
            this.name = "";
            this.stage = null;
            this.frameIndices.Clear();
            this.cachedFrames.Clear();
            this.armatureNames.Clear();
            this.armatures.Clear();
            this.binary = null;
            this.intArray = null; //
            this.floatArray = null; //
            this.frameIntArray = null; //
            this.frameFloatArray = null; //
            this.frameArray = null; //
            this.timelineArray = null; //
            this.userData = null;
        }

        /// <internal/>
        /// <private/>
        public void AddArmature(ArmatureData value)
        {
            ArmatureData data;
            if (this.armatures.TryGetValue(value.name, out data))
            {
                data.ReturnToPool();
            }

            value.parent = this;
            this.armatures[value.name] = value;
            this.armatureNames.Add(value.name);
        }

        /// <summary>
        /// - Get a specific armature data.
        /// </summary>
        /// <param name="armatureName">- The armature data name.</param>
        /// <version>DragonBones 3.0</version>
        /// <language>en_US</language>

        /// <summary>
        /// - 获取特定的骨架数据。
        /// </summary>
        /// <param name="armatureName">- 骨架数据名称。</param>
        /// <version>DragonBones 3.0</version>
        /// <language>zh_CN</language>
        public ArmatureData GetArmature(string armatureName)
        {
            if (this.armatures.Count == 1)
                return this.armatures[this.armatureNames[0]];
            return this.armatures.ContainsKey(armatureName) ? this.armatures[armatureName] : null;
        }
    }
}
