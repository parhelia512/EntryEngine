﻿#if CLIENT

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EntryEngine
{
	public static partial class __GRAPHICS
	{
		/// <summary>
		/// 视口适配：计算画布要在屏幕内显示时的缩放和平移量
		/// </summary>
		/// <param name="graphics">画布尺寸</param>
		/// <param name="screen">屏幕尺寸</param>
		/// <param name="scale">缩放值</param>
		/// <param name="offset">平移值</param>
		public static void ViewAdapt(VECTOR2 graphics, VECTOR2 screen, out float scale, out VECTOR2 offset)
		{
			float scaleG = graphics.Y / graphics.X;
			float scaleS = screen.Y / screen.X;

			offset = new VECTOR2();
			if (scaleG > scaleS)	// vertical
			{
				scale = screen.Y / graphics.Y;
				offset.X = (screen.X - ((graphics.X / graphics.Y) / (screen.X / screen.Y)) * screen.X) / 2f;
			}
			else
			{
				scale = screen.X / graphics.X;
				offset.Y = (screen.Y - scaleG / scaleS * screen.Y) / 2f;
			}
		}
		/// <summary>
		/// 景深公式
		/// (远景-屏幕)/(焦距-屏幕)*偏移值
		/// 宽高要分别计算
		/// </summary>
		/// <param name="faraway">远景尺寸</param>
		/// <param name="near">焦距尺寸</param>
		/// <param name="offset">焦距偏移值</param>
		/// <param name="screen">屏幕尺寸</param>
		public static VECTOR2 ViewDepth(VECTOR2 faraway, VECTOR2 near, VECTOR2 offset, VECTOR2 screen)
		{
			VECTOR2 output = new VECTOR2();
			if (near.X == screen.X)
				output.X = offset.X;
			else
				output.X = (faraway.X - screen.X) / (near.X - screen.X) * offset.X;
			if (near.Y == screen.Y)
				output.Y = offset.Y;
			else
				output.Y = (faraway.Y - screen.Y) / (near.Y - screen.Y) * offset.Y;
			return output;
		}
		/// <summary>
		/// (faraway - screen) / (near - screen) * offset
		/// </summary>
		/// <returns>(faraway - screen) / (near - screen) * offset</returns>
		public static float ViewDepth(float faraway, float near, float offset, float screen)
		{
			return (faraway - screen) / (near - screen) * offset;
		}
		public static VECTOR3 MoveCamera(this MATRIX camera, VECTOR2 move)
		{
			VECTOR3 directionY = VECTOR3.Normalize(camera.Up);
			if (directionY.Y != 0)
			{
				move.Y /= directionY.Y;
				directionY.Y = 0;
			}
			VECTOR3 directionX = VECTOR3.Normalize(camera.Right);
            return VECTOR3.Add(VECTOR3.Multiply(directionY, move.Y), VECTOR3.Multiply(directionX, -move.X));
		}
        public static MATRIX2x3 DrawMatrix(RECT rect, RECT source, float rotation, VECTOR2 origin, EFlip flip)
        {
            MATRIX2x3 result;
            DrawMatrix(ref rect, ref source, rotation, ref origin, flip, out result);
            return result;
        }
        /// <summary>
        /// if need begin with the result matrix, you should set the rect's location to 0,0
        /// </summary>
        public static void DrawMatrix(ref RECT rect, ref RECT source, float rotation, ref VECTOR2 origin, EFlip flip, out MATRIX2x3 result)
        {
            MATRIX2x3 mirror = MATRIX2x3.Identity;
            if ((flip & EFlip.FlipHorizontally) != EFlip.None)
            {
                mirror.M11 = -1;
                rotation = -rotation;
            }
            if ((flip & EFlip.FlipVertically) != EFlip.None)
            {
                mirror.M22 = -1;
                rotation = -rotation;
            }
            MATRIX2x3 rotate = MATRIX2x3.CreateRotation(rotation, origin.X / source.Width * rect.Width, origin.Y / source.Height * rect.Height);
            MATRIX2x3 translation = MATRIX2x3.CreateTranslation(rect.X, rect.Y);

            MATRIX2x3.Multiply(ref rotate, ref mirror, out result);
            MATRIX2x3.Multiply(ref result, ref translation, out result);
        }
        /// <summary>
        /// if need begin with the result matrix, you should set the rect's location to 0,0
        /// </summary>
        public static void DrawMatrixWithoutMirror(ref RECT rect, ref RECT source, float rotation, ref VECTOR2 origin, out MATRIX2x3 result)
        {
            MATRIX2x3 rotate = MATRIX2x3.CreateRotation(rotation, origin.X / source.Width * rect.Width, origin.Y / source.Height * rect.Height);
            MATRIX2x3 translation = MATRIX2x3.CreateTranslation(rect.X, rect.Y);
            MATRIX2x3.Multiply(ref rotate, ref translation, out result);
        }
        /// <summary>通过像素差计算锚点</summary>
        public static float CalcOrigin(float position, float size, float originPosition)
        {
            return (originPosition - position) / size;
        }
	}
}

#endif