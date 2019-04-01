﻿using System;
using System.Diagnostics;
using EntryEngine.Serialize;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EntryEngine.Xna
{
	using GameTime = Microsoft.Xna.Framework.GameTime;

    public class XnaGate : Microsoft.Xna.Framework.Game
    {
        public static XnaGate Gate
        {
            get;
            private set;
        }

		struct FPSTest
		{
			public int InvokeCount;
			public TimeSpan InvokeTime;
		}

        // 帧效率测试
		FPSTest updateTime;
		FPSTest drawTime;
		TimeSpan realTime;

        GraphicsDeviceManager graphics;
        GameTime gameTime;
        public Color BGColor = Color.TransparentBlack;
        public event Func<Entry> OnCreateEntry;
        public event Action<Entry> OnInitialize;
        public event Action<Entry> OnInitialized;

        public GraphicsDeviceManager GraphicsManager
        {
            get { return graphics; }
        }
        public GameTime GameTime
        {
            get { return gameTime; }
        }
        public Entry Entry
        {
            get;
            private set;
        }

        public XnaGate()
        {
            graphics = new GraphicsDeviceManager(this);
            base.Content.RootDirectory = "Content";
            Gate = this;
            //this.IsFixedTimeStep = false;
        }

        protected sealed override void Initialize()
        {
            base.Initialize();
        }
        protected sealed override void LoadContent()
        {
            if (OnCreateEntry != null)
                Entry = OnCreateEntry();
            else
                Entry = new EntryXna();
            if (OnInitialize != null)
                OnInitialize(Entry);
            Entry.Initialize();
            if (OnInitialized != null)
                OnInitialized(Entry);
        }
        protected override void UnloadContent()
        {
            //Entry.Dispose();
        }
        protected override void Update(GameTime gameTime)
        {
            this.gameTime = gameTime;

			Stopwatch clock = new Stopwatch();
			clock.Start();
			Entry.Update();
			clock.Stop();
			updateTime.InvokeTime = updateTime.InvokeTime.Add(clock.Elapsed);
			updateTime.InvokeCount++;

            base.Update(gameTime);

            realTime += gameTime.ElapsedRealTime;
            if (realTime.TotalSeconds >= 1)
            {
                Window.Title = string.Format("Update: {0} / {1} = {2} Render: {3} / {4} = {5}",
                    updateTime.InvokeTime.TotalMilliseconds.ToString("0.00"), updateTime.InvokeCount, (updateTime.InvokeTime.TotalMilliseconds / updateTime.InvokeCount).ToString("0.00"),
                    drawTime.InvokeTime.TotalMilliseconds.ToString("0.00"), drawTime.InvokeCount, (drawTime.InvokeTime.TotalMilliseconds / drawTime.InvokeCount).ToString("0.00"));
                realTime -= TimeSpan.FromSeconds(1);
                updateTime = new FPSTest();
                drawTime = new FPSTest();
            }
        }
        CAMERA camera = new CAMERA() { Position = new VECTOR3(5, 5, 20) };
        protected override void Draw(GameTime gameTime)
        {
            Stopwatch clock = new Stopwatch();
            clock.Start();
            GraphicsDevice.Clear(BGColor);

			Entry.Draw();

			clock.Stop();
			drawTime.InvokeTime = drawTime.InvokeTime.Add(clock.Elapsed);
			drawTime.InvokeCount++;

            base.Draw(gameTime);
        }
    }
}
