﻿#if CLIENT

using System.Text;

namespace EntryEngine.UI
{
    // 若不实现ITypist，则Readonly将使用Label.Readonly的显示实现
    /// <summary>文本框</summary>
	public class TextBox : Label, ITypist
	{
		private char password;
        private UIText defaultText = new UIText();

        /// <summary>只读，true时不能修改</summary>
        public bool Readonly
        {
            get;
            set;
        }
        /// <summary>多行，true时可以多行编辑</summary>
        public bool Multiple
        {
            get;
            set;
        }
        bool ITypist.IsMask
        {
            get { return IsPasswordMode; }
        }
        /// <summary>是否为密码模式</summary>
		public bool IsPasswordMode
		{
			get { return password != default(char); }
			set
			{
				if (value)
					Password = '*';
				else
					Password = default(char);
			}
		}
        /// <summary>密码模式现实的代替字符</summary>
		public char Password
		{
			get { return password; }
			set
			{
				if (password != value)
				{
					password = value;
					ResetDisplay();
				}
			}
		}
        /// <summary>当文本框文字内容为空时显示的默认提示文字</summary>
        public UIText DefaultText
        {
            get { return defaultText; }
            set
            {
                if (value != null)
                    defaultText = value;
            }
        }
        //public override bool CanFocused
        //{
        //    get { return !Readonly; }
        //}
        public override EUIType UIType
        {
            get { return EUIType.TextBox; }
        }

		public TextBox()
		{
            CanSelect = true;
            //Click += ClickFocus;
            Clicked += TextBox_Clicked;
            DefaultText.FontColor = COLOR.LightGray;
		}

        void TextBox_Clicked(UIElement sender, Entry e)
        {
            //Handled = true;
            Handle();
        }
		public override void ResetDisplay()
		{
			StringBuilder builder = new StringBuilder(Text);

			if (IsPasswordMode)
			{
				for (int i = 0; i < builder.Length; i++)
				{
					builder[i] = password;
				}
			}

            InternalResetDisplay(builder.ToString());
		}
        protected override void BaseDrawFont(GRAPHICS spriteBatch, Entry e)
        {
            base.BaseDrawFont(spriteBatch, e);
            if (!Focused && string.IsNullOrEmpty(Text) && !string.IsNullOrEmpty(DefaultText.Text))
            {
                DefaultText.Draw(spriteBatch, ViewClip);
            }
        }
	}
    /// <summary>数字框，仅允许输入数字</summary>
    public class NumberBox : TextBox
    {
        private double minValue = double.MinValue;
        private double maxValue = double.MaxValue;
        private double value;
        /// <summary>y+1px时对应的Value变化</summary>
        public double DragStep = 1;
        public event DUpdate<NumberBox> ValueChanged;

        public double MinValue
		{
			get { return minValue; }
			set
			{
				minValue = value;

				if (minValue > maxValue)
					maxValue = minValue;

				if (minValue > Value)
					Value = minValue;
			}
		}
        public double MaxValue
		{
			get { return maxValue; }
			set
			{
				maxValue = value;

				if (maxValue < minValue)
					minValue = maxValue;

				if (maxValue < Value)
					Value = maxValue;
			}
		}
        public double Value
		{
			get { return value; }
			set
			{
				value = value > maxValue ? maxValue : value;
				value = value < minValue ? minValue : value;
				if (this.value != value)
				{
					this.value = value;
                    if (ValueChanged != null)
                        ValueChanged(this, Entry.Instance);
                    //Text = value.ToString();
				}
                string result = value.ToString();
                if (Text != result)
                    Text = result;
			}
		}

		public NumberBox()
		{
            this.Drag += DragModifyValue;
            this.TextChanged += OnTextChanged;
		}

        protected void OnTextChanged(Label sender)
        {
            double value;
            if (double.TryParse(sender.Text, out value))
                Value = value;
        }
		protected void DragModifyValue(UIElement sender, Entry e)
		{
            VECTOR2 moved = e.INPUT.Pointer.DeltaPosition;
			if (moved.Y != 0 && !moved.IsNaN())
			{
                SetFocus(false);
				Value += -moved.Y * DragStep;
			}
		}
        public override bool Filter(ref char c)
        {
            if (c == '.')
                return Text.IndexOf('.') != -1;
            else if (c == '-')
                if (Focused && InputDevice.Typist == this)
                    return InputDevice.Index != 0 && !Text.StartsWith(InputDevice.SelectedText);
                else
                    return Text.Length > 0;
            else if (c >= '0' && c <= '9')
                return false;
            else
                return true;
        }
    }
}

#endif